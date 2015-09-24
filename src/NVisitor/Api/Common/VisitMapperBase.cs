using System;
using System.Collections.Generic;
using NVisitor.Common;

namespace NVisitor.Api.Common
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily">Node's family</typeparam>
    /// <typeparam name="TVisitDelegate">the visit delegate</typeparam>
    /// <typeparam name="TVisitorClass">The visitor's class characterizing all visitors of a director</typeparam>
    public abstract class VisitMapperBase<TFamily, TVisitorClass, TVisitDelegate>
        : IVisitMapperBase<TFamily, TVisitDelegate>
    {
        private readonly VisitorCollection<TVisitorClass> mVisitorCollection;
        private readonly Dictionary<Type, TVisitDelegate> mVisitCacheByNodeType;

        protected VisitMapperBase(IEnumerable<TVisitorClass> visitors, Type visitorGenericOpenType, int nodeTypeIndex)
        {
            mVisitorCollection = new VisitorCollection<TVisitorClass>(visitors, visitorGenericOpenType, nodeTypeIndex);
            mVisitCacheByNodeType = new Dictionary<Type, TVisitDelegate>();
        }

        public TVisitDelegate GetVisitDelegate(TFamily node)
        {
            if (ReferenceEquals(node, null))
                throw new ArgumentNullException("node");

            TVisitDelegate directorDelegate;
            if (!mVisitCacheByNodeType.TryGetValue(node.GetType(), out directorDelegate))
            {
                // get the visitor instance and the node-type of the visitor
                Type visitorNodeType;
                TVisitorClass visitor = mVisitorCollection.FindNearestVisitor(node.GetType(), out visitorNodeType);

                directorDelegate = CreateVisitDelegate(node, visitorNodeType, visitor, directorDelegate);
                mVisitCacheByNodeType.Add(node.GetType(), directorDelegate);
            }

            return directorDelegate;
        }

        /// <summary>Factory to implement in concrete implementations</summary>
        protected abstract TVisitDelegate CreateVisitDelegate(TFamily node,
                                                              Type visitorNodeType,
                                                              TVisitorClass visitorInstance,
                                                              TVisitDelegate directorDelegate);
    }
}