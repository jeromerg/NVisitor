using System;
using System.Collections.Generic;
using NVisitor.Api.Common;

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

        private readonly IVisitEngine<TFamily, LazyDirector<TFamily, TDirector>, IEnumerable<Pause>> mVisitEngine;

        /// <summary>
        /// Initializes a new instance of director with a set of visitors. The Visit(...) method dispatches
        /// to one of the provided visitor instance depending on the node type.
        /// </summary>
        /// <param name="visitors">list of visitor instances: the director dispatches each Visit(...) call to one of these instances</param>
        protected LazyDirector(IEnumerable<ILazyVisitorClass<TFamily, TDirector>> visitors)
        {
            mVisitEngine = new VisitEngine<TFamily, LazyDirector<TFamily, TDirector>, 
                                                   ILazyVisitorClass<TFamily, TDirector>, 
                                                   IEnumerable<Pause>>
                                                   (visitors, typeof(ILazyVisitor<,,>), 2, "Visit");
        }

        /// <summary>
        /// Initializes a new instance of director with a shared cache. The Visit(...) method dispatches
        /// to one of the provided visitor instance depending on the node type.
        /// </summary>
        /// <param name="visitEngine">The shared director cache</param>
        protected LazyDirector(IVisitEngine<TFamily, LazyDirector<TFamily, TDirector>, IEnumerable<Pause>> visitEngine)
        {
            mVisitEngine = visitEngine;
        }

        /// <summary>
        /// The Visit method, that dispatches the call to the best visitor depending on the node type.
        /// This Visit method is the entry-point for a visit. It is also called by the visitors themselves, in order to
        /// continue the visit to the node children / parent / neighbors
        /// </summary>
        /// <param name="node">The node to visit</param>
        public IEnumerable<Pause> Visit(TFamily node)
        {
            if (ReferenceEquals(node, null))
                throw new ArgumentNullException("node");

            return mVisitEngine.Visit(this, node);
        }
    }
}