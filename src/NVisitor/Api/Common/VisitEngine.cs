using System;
using System.Collections.Generic;
using System.Reflection;
using NVisitor.Common;

namespace NVisitor.Api.Common
{
    public class VisitEngine<TFamily, TDirector, TVisitorClass, TResult> 
        : IVisitEngine<TFamily, TDirector, TResult>
    {
        private readonly Type mVisitorGenericOpenType;
        private readonly VisitorCollection<TVisitorClass> mVisitorCollection;
        private readonly Dictionary<Type, Func<TDirector, TFamily, TResult>> mVisitCacheByNodeType;
        private readonly string mVisitMethodName;

        public VisitEngine(IEnumerable<TVisitorClass> visitors, 
                                Type visitorGenericOpenType, 
                                int nodeTypeIndex, 
                                string visitMethodName)
        {
            mVisitorGenericOpenType = visitorGenericOpenType;
            mVisitorCollection = new VisitorCollection<TVisitorClass>(visitors, visitorGenericOpenType, nodeTypeIndex);
            mVisitCacheByNodeType = new Dictionary<Type, Func<TDirector, TFamily, TResult>>();
            mVisitMethodName = visitMethodName;
        }

        public TResult Visit(TDirector director, TFamily node)
        {
            if (ReferenceEquals(node, null))
                throw new ArgumentNullException("node");

            Func<TDirector, TFamily, TResult> visitAction;
            if (!mVisitCacheByNodeType.TryGetValue(node.GetType(), out visitAction))
            {
                // get the visitor instance and the node-type of the visitor
                Type visitorNodeType;
                TVisitorClass visitor = mVisitorCollection.FindNearestVisitor(node.GetType(), out visitorNodeType);

                // create the closed generic type of the visitor
                Type visitorClosedType = mVisitorGenericOpenType.MakeGenericType(typeof(TFamily), typeof(TDirector), visitorNodeType);

                // find the visit method in the closed generic type of the visitor
                MethodInfo visitMethod = visitorClosedType.GetMethod(mVisitMethodName);
                
                // prepare the visit action and cache it
                visitAction = (someDirector, someNode) => (TResult)visitMethod.Invoke(visitor, new object[] { someDirector, someNode });
                mVisitCacheByNodeType.Add(node.GetType(), visitAction);
            }

            // call the visit action
            return visitAction(director, node);
        }
    }
}