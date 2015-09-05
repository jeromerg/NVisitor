using System;
using System.Collections.Generic;
using NVisitor.Common.Topo;

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
    public abstract class Director<TFamily, TDir> : IDirector<TFamily, TDir>
        where TDir : IDirector<TFamily, TDir>
    {
        private readonly IVisitMapper<TFamily,TDir> mVisitMapper;

        /// <summary>Initializes a new director for a set of visitors</summary>
        /// <param name="visitorEnumerable">list of visitors belonging to the same visitor class</param>
        protected Director(IEnumerable<IVisitorClass<TFamily, TDir>> visitorEnumerable)
        {
            mVisitMapper = new VisitMapper<TFamily, TDir>(visitorEnumerable);
        }

        /// <summary>Initializes a new director with a shared visitMapper</summary>
        /// <param name="visitMapper">shared visitMapper for all directors of this type</param>
        protected Director(IVisitMapper<TFamily, TDir> visitMapper)
        {
            mVisitMapper = visitMapper;
        }

        /// <summary>Dispatches the call to the best visitor depending on the node's type</summary>
        /// <param name="node">The node to visit</param>
        public void Visit(TFamily node)
        {
            if (ReferenceEquals(node, null))
                throw new ArgumentNullException("node");

            Action<IDirector<TFamily, TDir>, TFamily> visitAction;
            try
            {
                visitAction = mVisitMapper.GetVisitDelegate(node);
            }
            catch (TargetTypeNotResolvedException e)
            {
                throw new VisitorNotFoundException(GetType(), e);
            }
            visitAction(this, node);
        }
    }
}