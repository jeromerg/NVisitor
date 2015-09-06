using System;
using System.Collections.Generic;
using NVisitor.Common;

namespace NVisitor.Api.Common
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily1">Node1's family</typeparam>
    /// <typeparam name="TFamily2">Node2's family</typeparam>
    /// <typeparam name="TVisitDelegate">the visit delegate</typeparam>
    /// <typeparam name="TVisitorClass">The visitor's class characterizing all visitors of a director</typeparam>
    public abstract class PairVisitMapperBase<TFamily1, TFamily2, TVisitorClass, TVisitDelegate>
        : IPairVisitMapperBase<TFamily1, TFamily2, TVisitDelegate>
    {
        private readonly PairVisitorCollection<TVisitorClass> mVisitorCollection;
        private readonly Dictionary<Tuple<Type, Type>, TVisitDelegate> mVisitCacheByNodeType;

        protected PairVisitMapperBase(IEnumerable<TVisitorClass> visitors, Type visitorGenericOpenType, int node1TypeIndex, int node2TypeIndex)
        {
            mVisitorCollection = new PairVisitorCollection<TVisitorClass>(visitors, visitorGenericOpenType, node1TypeIndex, node2TypeIndex);
            mVisitCacheByNodeType = new Dictionary<Tuple<Type, Type>, TVisitDelegate>();
        }

        public TVisitDelegate GetVisitDelegate(TFamily1 node1, TFamily2 node2)
        {
            if (ReferenceEquals(node1, null))
                throw new ArgumentNullException("node1");

            if (ReferenceEquals(node2, null))
                throw new ArgumentNullException("node2");

            TVisitDelegate directorDelegate;
            var nodeTypesTuple = new Tuple<Type, Type>(node1.GetType(), node2.GetType());
            if (!mVisitCacheByNodeType.TryGetValue(nodeTypesTuple, out directorDelegate))
            {
                // get the visitor instance and the node-type of the visitor
                Type visitorNode1Type;
                Type visitorNode2Type;
                TVisitorClass visitor = mVisitorCollection.FindNearestVisitor(node1.GetType(), node2.GetType(), out visitorNode1Type, out visitorNode2Type);

                directorDelegate = CreateVisitDelegate(node1, node2, visitorNode1Type, visitorNode2Type, visitor, directorDelegate);
                mVisitCacheByNodeType.Add(nodeTypesTuple, directorDelegate);
            }

            return directorDelegate;
        }

        /// <summary>Factory to implement in concrete implementations</summary>
        protected abstract TVisitDelegate CreateVisitDelegate(
            TFamily1 node1, 
            TFamily2 node2, 
            Type visitorNode1Type, 
            Type visitorNode2Type, 
            TVisitorClass visitorInstance, 
            TVisitDelegate directorDelegate);
    }
}