using System;
using System.Collections.Generic;
using System.Reflection;
using NVisitor.Common;

namespace NVisitor.Api.Lazy
{
    /// <summary>
    /// Inherit from this LazyDirector class to create a new algorithm that applies to the TFamily. Then implement multiple
    /// visitors that implement the ILazyVisitor interface
    /// </summary>
    /// <typeparam name="TFamily">The node family type</typeparam>
    /// <typeparam name="TDirector">The LazyDirector concrete implementation</typeparam>
    public abstract class LazyDirector<TFamily, TDirector> : ILazyDirector<TFamily>
        where TDirector : ILazyDirector<TFamily>
    {

        private readonly VisitorCollection<ILazyVisitorClass<TFamily, TDirector>> mVisitorCollection;
        private readonly Dictionary<Type, Func<TFamily, IEnumerable<Pause>>> mVisitCacheByNodeType;

        /// <summary>
        /// Initializes a new instance of director with a set of visitors. The Visit(...) method dispatches
        /// to one of the provided visitor instance depending on the node type.
        /// </summary>
        /// <param name="visitors">list of visitor instances: the director dispatches each Visit(...) call to one of these instances</param>
        protected LazyDirector(IEnumerable<ILazyVisitorClass<TFamily, TDirector>> visitors)
        {
            mVisitorCollection = new VisitorCollection<ILazyVisitorClass<TFamily, TDirector>>(visitors, typeof(ILazyVisitor<,,>), 2);
            mVisitCacheByNodeType = new Dictionary<Type, Func<TFamily, IEnumerable<Pause>>>();
        }

        /// <summary>
        /// The Visit method, that dispatches the call to the best visitor depending on the node type.
        /// This Visit method is the entry-point for a visit. It is also called by the visitors themselves, in order to
        /// continue the visit to the node children / parent / neighbors
        /// </summary>
        /// <param name="node">The node to visit</param>
        public virtual IEnumerable<Pause> Visit(TFamily node)
        {
            if (ReferenceEquals(node, null))
                throw new ArgumentNullException("node");

            Func<TFamily, IEnumerable<Pause>> visitAction;
            if (!mVisitCacheByNodeType.TryGetValue(node.GetType(), out visitAction))
            {
                // get the visitor instance and the node-type of the visitor
                Type visitorNodeType;
                ILazyVisitorClass<TFamily, TDirector> visitor = mVisitorCollection.FindNearestVisitor(node.GetType(), out visitorNodeType);

                // create the closed generic type of the visitor
                Type visitorClosedType = typeof(ILazyVisitor<,,>).MakeGenericType(typeof(TFamily), typeof(TDirector), visitorNodeType);

                // find the visit method in the closed generic type of the visitor
                MethodInfo visitMethod = visitorClosedType.GetMethod("Visit");
                
                // prepare the visit action and cache it
                visitAction = someNode => (IEnumerable<Pause>)visitMethod.Invoke(visitor, new object[] { this, someNode });
                mVisitCacheByNodeType.Add(node.GetType(), visitAction);
            }

            // call the visit action
            return visitAction(node);
        }
    }
}