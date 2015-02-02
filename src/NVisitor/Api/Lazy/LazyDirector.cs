using System;
using System.Collections.Generic;

namespace NVisitor.Api.Lazy
{
    /// <summary>
    /// A Director 
    /// 1) Is the entry-point for a visit
    /// 2) Dispatches visit to the best visitor
    /// 3) Holds the state of the visit via its property State
    /// </summary>
    /// <typeparam name="TFamily">The node family type</typeparam>
    /// <typeparam name="TDir"> Identifies the visitor's class and can contain the state of the visit</typeparam>
    public abstract class LazyDirector<TFamily, TDir> : ILazyDirector<TFamily, TDir> 
        where TDir : ILazyDirector<TFamily, TDir>
    {
        private readonly ILazyDispatcher<TFamily, TDir> mDispatcher;

        /// <summary>Initializes a new dispatcher for a set of visitors</summary>
        /// <param name="visitors">list of visitors belonging to the same visitor class</param>
        protected LazyDirector(IEnumerable<ILazyVisitorClass<TFamily, TDir>> visitors)
        {
            mDispatcher = new LazyDispatcher<TFamily, TDir>(visitors);
        }

        /// <summary>Initializes a new dispatcher for a set of visitors</summary>
        /// <param name="visitors">list of visitors belonging to the same visitor class</param>
        protected LazyDirector(params ILazyVisitorClass<TFamily, TDir>[] visitors)
        {
            mDispatcher = new LazyDispatcher<TFamily, TDir>(visitors);
        }

        /// <summary>Initializes a new dispatcher with a shared Engine</summary>
        /// <param name="dispatcher">shared dispatcher for all directors of this type</param>
        protected LazyDirector(ILazyDispatcher<TFamily, TDir> dispatcher)
        {
            mDispatcher = dispatcher;
        }

        /// <summary>Dispatches the call to the best visitor depending on the node's type</summary>
        /// <param name="node">The node to visit</param>
        public IEnumerable<Pause> Visit(TFamily node)
        {
            if (ReferenceEquals(node, null))
                throw new ArgumentNullException("node");

            return mDispatcher.Visit(this, node);
        }

    }
}