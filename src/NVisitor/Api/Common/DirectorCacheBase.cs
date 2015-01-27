using System;
using System.Collections.Generic;
using NVisitor.Common;

namespace NVisitor.Api.Common
{
    public abstract class DirectorCacheBase<TFamily, TVisitorClass, TDelegate>
        : IDirectorCacheBase<TFamily, TDelegate>
    {
        private readonly VisitorCollection<TVisitorClass> mVisitorCollection;
        private readonly Dictionary<Type, TDelegate> mVisitCacheByNodeType;

        protected DirectorCacheBase(IEnumerable<TVisitorClass> visitors, Type visitorGenericOpenType, int nodeTypeIndex)
        {
            mVisitorCollection = new VisitorCollection<TVisitorClass>(visitors, visitorGenericOpenType, nodeTypeIndex);
            mVisitCacheByNodeType = new Dictionary<Type, TDelegate>();
        }

        public TDelegate GetOrCreate(TFamily node)
        {
            if (ReferenceEquals(node, null))
                throw new ArgumentNullException("node");

            TDelegate directorDelegate;
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

        protected abstract TDelegate CreateVisitDelegate(TFamily node, Type visitorNodeType, TVisitorClass visitorInstance, TDelegate directorDelegate);
    }
}