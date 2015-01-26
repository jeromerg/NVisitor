using System;
using System.Collections.Generic;
using NVisitor.Api.Common;
using NVisitor.Common.Quality;

namespace NVisitor.Api.Batch
{
    /// <summary>
    /// Inherit from this Director class to create a new algorithm that applies to the TFamily. Then implement multiple
    /// visitors that implement the IVisitor interface
    /// </summary>
    /// <typeparam name="TFamily">The node family type</typeparam>
    /// <typeparam name="TDirector">The concrete Director contract/implementation, which is provided to the visitor during the visit</typeparam>
    public abstract class Director<TFamily, TDirector> : IDirector<TFamily>
        where TDirector : IDirector<TFamily>
    {
        [UsedImplicitly]
        private class Void { }

        private readonly IVisitEngine<TFamily, TDirector, IEnumerable<Void>> mVisitEngine;

        /// <summary>
        /// Initializes a new instance of director with a set of visitors. The Visit(...) method dispatches
        /// to one of the provided visitor instance depending on the node type.
        /// </summary>
        /// <param name="visitors">list of visitor instances: the director dispatches each Visit(...) call to one of these instances</param>
        protected Director(IEnumerable<IVisitorClass<TFamily, TDirector>> visitors)
        {
            mVisitEngine = new VisitEngine<TFamily, TDirector,
                                                   IVisitorClass<TFamily, TDirector>,
                                                   IEnumerable<Void>>
                                                   (visitors, typeof(IVisitor<,,>), 2, "Visit");
        }

        /// <summary>
        /// The Visit method, that dispatches the call to the best visitor depending on the node type.
        /// This Visit method is the entry-point for a visit. It is also called by the visitors themselves, in order to
        /// continue the visit to the node children / parent / neighbors
        /// </summary>
        /// <param name="node">The node to visit</param>
        public void Visit(TFamily node)
        {
            if (ReferenceEquals(node, null))
                throw new ArgumentNullException("node");

            mVisitEngine.Visit(this, node);
        }
    }
}