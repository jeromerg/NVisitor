using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace NVisitor.Common.Topo
{
    public class TypeHelper
    {
        /// <exception cref="TargetTypeNotResolvedException"></exception>
        [NotNull]
        public static Type FindBestCandidateToAssignFrom([NotNull] Type typeToAssignTo,
                                                         [NotNull] IEnumerable<Type> targetCandidates)
        {
            if (typeToAssignTo == null) throw new ArgumentNullException("typeToAssignTo");
            if (targetCandidates == null) throw new ArgumentNullException("targetCandidates");

            var debugInfo = new List<TargetTypeInfo>();
            var validCandidates = new List<Type>();

            // exclude candidate that are not assignable from the type to pass
            foreach (Type targetCandidate in targetCandidates)
            {
                if (!targetCandidate.IsAssignableFrom(typeToAssignTo))
                    debugInfo.Add(new TargetTypeInfo(targetCandidate, TargetTypeStatus.OutsideTypeTopology));
                else
                    validCandidates.Add(targetCandidate);
            }

            // exclude candidate that are less specific than another one
            for (int i = validCandidates.Count - 1; i >= 0; i--)
            {
                for (int j = validCandidates.Count - 1; j >= 0; j--)
                {
                    if (i == j)
                        continue;

                    Type typeI = validCandidates[i];
                    Type typeJ = validCandidates[j];
                    bool isJmoreSpecificThanI = typeI.IsAssignableFrom(typeJ);
                    if (isJmoreSpecificThanI)
                    {
                        debugInfo.Add(new TargetTypeInfo(typeI, TargetTypeStatus.DoesntHavePrecedence));
                        validCandidates.RemoveAt(i);
                        if (j > i)
                            j--; // fix index
                    }
                }
            }

            // validate that only one candidate remains
            if (validCandidates.Count != 1)
            {
                foreach (Type remainingCandidate in validCandidates)
                    debugInfo.Add(new TargetTypeInfo(remainingCandidate, TargetTypeStatus.AmbiguousMatch));

                throw new TargetTypeNotResolvedException(typeToAssignTo, debugInfo);
            }

            return validCandidates[0];
        }
    }
}