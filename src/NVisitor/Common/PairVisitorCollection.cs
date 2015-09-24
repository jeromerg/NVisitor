using System;
using System.Collections.Generic;
using System.Linq;
using NVisitor.Api;
using JetBrains.Annotations;
using NVisitor.Common.Topo;

namespace NVisitor.Common
{
    public class PairVisitorCollection<TVisitorClass>
    {
        private readonly Type mVisitorGenericOpenType;

        private readonly Dictionary<Type, Dictionary<Type, TVisitorClass>> mNode1TypeToVisitorsMapping = new Dictionary<Type, Dictionary<Type, TVisitorClass>>();

        public PairVisitorCollection(IEnumerable<TVisitorClass> visitorInstances, Type visitorGenericOpenType, int node1TypeIndex, int node2TypeIndex)
        {
            mVisitorGenericOpenType = visitorGenericOpenType;

            foreach (var visitorInstance in visitorInstances)
            {
                IEnumerable<Type> claimedVisitorTypes = visitorInstance
                                    .GetType()
                                    .GetInterfaces()
                                    .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == mVisitorGenericOpenType);

                foreach (var claimedVisitorType in claimedVisitorTypes)
                {
                    Type claimedNode1Type = claimedVisitorType.GetGenericArguments()[node1TypeIndex];
                    Type claimedNode2Type = claimedVisitorType.GetGenericArguments()[node2TypeIndex];

                    Dictionary<Type, TVisitorClass> node2TypeToVisitorMapping;
                    
                    // NODE1
                    if (!mNode1TypeToVisitorsMapping.TryGetValue(claimedNode1Type, out node2TypeToVisitorMapping))
                    {
                        node2TypeToVisitorMapping = new Dictionary<Type, TVisitorClass>();
                        mNode1TypeToVisitorsMapping.Add(claimedNode1Type, node2TypeToVisitorMapping);
                    }
                    
                    // NODE2
                    TVisitorClass conflictingVisitor;
                    if (node2TypeToVisitorMapping.TryGetValue(claimedNode2Type, out conflictingVisitor))
                    {
                        throw new VisitorConflictException("Visitor instances {0} and {1} apply for the same Node types node1={2}, node2={3}",
                                                            node2TypeToVisitorMapping.GetType().Name,
                                                            visitorInstance.GetType().Name,
                                                            claimedNode1Type.Name,
                                                            claimedNode2Type.Name);
                    }

                    node2TypeToVisitorMapping.Add(claimedNode2Type, visitorInstance);
                }
            }
        }

        [NotNull]
        public TVisitorClass FindNearestVisitor([NotNull] Type node1Type, [NotNull] Type node2Type, out Type visitorNode1Type, out Type visitorNode2Type)
        {
            if (ReferenceEquals(node1Type, null))
                throw new ArgumentNullException("node1Type");

            if (ReferenceEquals(node2Type, null))
                throw new ArgumentNullException("node2Type");

            var node1TypeTopology = new TypeTopology(node1Type);
            visitorNode1Type = node1TypeTopology.ResolveBestUnambiguousTargetType(new HashSet<Type>(mNode1TypeToVisitorsMapping.Keys));

            Dictionary<Type, TVisitorClass> node2TypeToVisitorsMapping = mNode1TypeToVisitorsMapping[visitorNode1Type];

            var node2TypeTopology = new TypeTopology(node2Type);
            visitorNode2Type = node2TypeTopology.ResolveBestUnambiguousTargetType(new HashSet<Type>(node2TypeToVisitorsMapping.Keys));

            return node2TypeToVisitorsMapping[visitorNode2Type];            
        }
    }
}