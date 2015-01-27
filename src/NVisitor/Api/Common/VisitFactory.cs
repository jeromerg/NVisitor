using System;
using System.Collections.Generic;
using NVisitor.Common;

namespace NVisitor.Api.Common
{
    public abstract class VisitFactory<TFamily, TVisitorClass, TVisitDelegate>
        : IVisitFactory<TFamily, TVisitDelegate>
    {
        private readonly VisitorCollection<TVisitorClass> mVisitorCollection;
        private readonly Dictionary<Type, TVisitDelegate> mVisitCacheByNodeType;

        protected VisitFactory(IEnumerable<TVisitorClass> visitors, Type visitorGenericOpenType, int nodeTypeIndex)
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

        protected abstract TVisitDelegate CreateVisitDelegate(TFamily node, Type visitorNodeType, TVisitorClass visitorInstance, TVisitDelegate directorDelegate);
    }
}