using System;
using System.Collections.Generic;

namespace NVisitor.Api.Batch
{
    /// <summary>
    /// A Director 
    /// 1) Is the entry-point for a visit
    /// 2) Dispatches visit to the best visitor
    /// 3) Holds the state of the visit via its property State
    /// </summary>
    /// <typeparam name="TFamily">The node family type</typeparam>
    /// <typeparam name="TDir"> Identifies the visitor's class and can contain the state of the visit</typeparam>
    public sealed class Director<TFamily, TDir> : IDirector<TFamily, TDir>
    {
        private readonly IDirectorCache<TFamily,TDir> mCache;

        /// <summary>Initializes a new cache for a set of visitors</summary>
        /// <param name="visitorEnumerable">list of visitors belonging to the same visitor class</param>
        public Director(IEnumerable<IVisitorClass<TDir>> visitorEnumerable)
        {
            mCache = new DirectorCache<TFamily, TDir>(visitorEnumerable);
        }

        /// <summary>Initializes a new cache for a set of visitors</summary>
        /// <param name="visitorArray">list of visitors belonging to the same visitor class</param>
        public Director(params IVisitorClass<TDir>[] visitorArray)
        {
            mCache = new DirectorCache<TFamily, TDir>(visitorArray);
        }

        /// <summary>Initializes a new cache with a shared cache</summary>
        /// <param name="cache">shared cache for all directors of this type</param>
        public Director(IDirectorCache<TFamily, TDir> cache)
        {
            mCache = cache;
        }

        /// <summary>Get/Set a director's state</summary>
        public TDir State { get; set; }

        /// <summary>Dispatches the call to the best visitor depending on the node's type</summary>
        /// <param name="node">The node to visit</param>
        public void Visit(TFamily node)
        {
            if (ReferenceEquals(node, null))
                throw new ArgumentNullException("node");

            Action<IDirector<TFamily, TDir>, TFamily> visitAction = mCache.GetOrCreate(node);

            visitAction(this, node);
        }
    }
}