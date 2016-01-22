using System;
using System.Collections.Generic;
using NVisitor.Common.Topo;

namespace NVisitor.Api.Lazy
{
    /// <summary>
    ///     A Director
    ///     1) Is the entry-point for a visit
    ///     2) Dispatches visit to the best visitor
    ///     3) Holds the state of the visit via its property State
    /// </summary>
    /// <typeparam name="TFamily">The node family type</typeparam>
    /// <typeparam name="TDir"> Identifies the visitor's class and can contain the state of the visit</typeparam>
    public abstract class LazyDirector<TFamily, TDir> : ILazyDirector<TFamily, TDir>
        where TDir : ILazyDirector<TFamily, TDir>
    {
        private readonly ILazyVisitMapper<TFamily, TDir> mCache;

        /// <summary>Initializes a new dispatcher for a set of visitors</summary>
        /// <param name="visitors">list of visitors belonging to the same visitor class</param>
        protected LazyDirector(IEnumerable<ILazyVisitorClass<TFamily, TDir>> visitors)
        {
            mCache = new LazyVisitMapper<TFamily, TDir>(visitors);
        }

        /// <summary>Initializes a new dispatcher for a set of visitors</summary>
        /// <param name="visitors">list of visitors belonging to the same visitor class</param>
        protected LazyDirector(params ILazyVisitorClass<TFamily, TDir>[] visitors)
        {
            mCache = new LazyVisitMapper<TFamily, TDir>(visitors);
        }

        /// <summary>Initializes a new dispatcher with a shared dispatcher</summary>
        /// <param name="cache">shared dispatcher for all directors of this type</param>
        protected LazyDirector(ILazyVisitMapper<TFamily, TDir> cache)
        {
            mCache = cache;
        }

        /// <summary>Dispatches the call to the best visitor depending on the node's type</summary>
        /// <param name="node">The node to visit</param>
        public IEnumerable<Pause> Visit(TFamily node)
        {
            if (ReferenceEquals(node, null))
                throw new ArgumentNullException("node", "The node passed to the director's Visit method cannot be null");

            Func<ILazyDirector<TFamily, TDir>, TFamily, IEnumerable<Pause>> visitFunction;
            try
            {
                visitFunction = mCache.GetVisitDelegate(node);
            }
            catch (TargetTypeNotResolvedException e)
            {
                throw new VisitorNotFoundException(GetType(), e);
            }

            return visitFunction(this, node);
        }
    }
}