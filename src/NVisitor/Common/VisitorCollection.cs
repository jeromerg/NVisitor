using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NVisitor.Api;
using NVisitor.Common.Topo;

namespace NVisitor.Common
{
    public class VisitorCollection<TVisitorClass>
    {
        private readonly Type mVisitorGenericOpenType;

        private readonly Dictionary<Type, TVisitorClass> mNodeTypeToVisitorMapping = new Dictionary<Type, TVisitorClass>();

        public VisitorCollection(IEnumerable<TVisitorClass> visitorInstances, Type visitorGenericOpenType, int nodeTypeIndex)
        {
            mVisitorGenericOpenType = visitorGenericOpenType;

            foreach (TVisitorClass visitorInstance in visitorInstances)
            {
                IEnumerable<Type> claimedVisitorTypes = visitorInstance
                    .GetType()
                    .GetInterfaces()
                    .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == mVisitorGenericOpenType);

                foreach (Type claimedVisitorType in claimedVisitorTypes)
                {
                    Type claimedNodeType = claimedVisitorType.GetGenericArguments()[nodeTypeIndex];

                    // check whether a previously added visitors claim exactly the same type
                    TVisitorClass conflictingVisitor;
                    if (mNodeTypeToVisitorMapping.TryGetValue(claimedNodeType, out conflictingVisitor))
                    {
                        throw new VisitorConflictException("Visitor instances {0} and {1} apply for the same Node type {2}",
                                                           conflictingVisitor.GetType().Name,
                                                           visitorInstance.GetType().Name,
                                                           claimedNodeType.Name);
                    }

                    mNodeTypeToVisitorMapping.Add(claimedNodeType, visitorInstance);
                }
            }
        }

        [NotNull]
        public TVisitorClass FindNearestVisitor([NotNull] Type nodeType, out Type visitorNodeType)
        {
            if (ReferenceEquals(nodeType, null))
                throw new ArgumentNullException("nodeType");

            var typeTopology = new TypeTopology(nodeType);
            visitorNodeType = typeTopology.ResolveBestUnambiguousTargetType(new HashSet<Type>(mNodeTypeToVisitorMapping.Keys));

            return mNodeTypeToVisitorMapping[visitorNodeType];
        }
    }
}