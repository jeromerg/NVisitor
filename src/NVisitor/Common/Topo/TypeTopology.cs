using System;
using System.Collections.Generic;
using System.Linq;
using NVisitor.Common.Quality;

namespace NVisitor.Common.Topo
{
    public class TypeTopology
    {
        private readonly Type mType;
        private readonly Dictionary<Type, HashSet<Type>> mAllTypesWithChildren = new Dictionary<Type, HashSet<Type>>();

        public TypeTopology(Type type)
        {
            mType = type;
            mAllTypesWithChildren.Add(type, new HashSet<Type>());
            CollectAllParentTypes(type);
        }

        /// <exception cref="TargetTypeNotResolvedException"></exception>
        [NotNull]
        public Type ResolveBestUnambiguousTargetType(HashSet<Type> targetCandidates)
        {
            var excludedTypes = new List<TargetTypeInfo>();

            var concernedCandidates = new HashSet<Type>();
            foreach (var candidate in targetCandidates)
            {
                if (!mAllTypesWithChildren.ContainsKey(candidate))
                    excludedTypes.Add(new TargetTypeInfo(candidate, TargetTypeStatus.OutsideTypeTopology));
                else
                    concernedCandidates.Add(candidate);
            }

            foreach (var candidate in concernedCandidates)
            {
                bool contains = IsAnyTopologicalChildACandidate(candidate, concernedCandidates);
                if (contains)
                {
                    excludedTypes.Add(new TargetTypeInfo(candidate, TargetTypeStatus.ChildClassTakesPrecedence));
                    continue;
                }

                var parents = new HashSet<Type>(GetAllParents(candidate));

                Type candidateInClosure = candidate;
                if (concernedCandidates.Any(otherCandidate => otherCandidate != candidateInClosure && !parents.Contains(otherCandidate)))
                {
                    excludedTypes.Add(new TargetTypeInfo(candidate, TargetTypeStatus.AmbiguousMatch));
                    continue;
                }

                return candidate;
            }

            throw new TargetTypeNotResolvedException(mType, excludedTypes);
        }

        private void CollectAllParentTypes(Type type)
        {
            if (type.BaseType != null)
            {
                AddParentAndLink(type, type.BaseType);
            }

            foreach (var implementedInterface in type.GetInterfaces())
            {
                AddParentAndLink(type, implementedInterface);
            }
        }

        private void AddParentAndLink(Type type, Type parent)
        {
            HashSet<Type> parentsChildren;
            if (!mAllTypesWithChildren.TryGetValue(parent, out parentsChildren))
            {
                parentsChildren = new HashSet<Type>();
                mAllTypesWithChildren.Add(parent, parentsChildren);
            }

            parentsChildren.Add(type);

            CollectAllParentTypes(parent);
        }

        private IEnumerable<Type> GetAllParents(Type type)
        {
            if (type.BaseType != null)
            {
                yield return type.BaseType;
                foreach (var parentOfParent in GetAllParents(type.BaseType))
                    yield return parentOfParent;
            }

            foreach (var implementedInterface in type.GetInterfaces())
            {
                yield return implementedInterface;
                foreach (var parentOfParent in GetAllParents(implementedInterface))
                    yield return parentOfParent;
            }

        }

        private bool IsAnyTopologicalChildACandidate(Type type, HashSet<Type> candidates)
        {
            foreach (var child in mAllTypesWithChildren[type])
            {
                if (candidates.Contains(child))
                    return true;

                if (IsAnyTopologicalChildACandidate(child, candidates))
                    return true;
            }
            return false;
        }
    }
    

}
