using System;
using System.Collections.Generic;
using NVisitor.Common.Topo;

namespace NVisitor.Api.PairBatch
{
    /// <summary>
    /// A Director 
    /// 1) Is the entry-point for a visit
    /// 2) Dispatches visit to the best visitor
    /// 3) Holds the state of the visit via its property State
    /// </summary>
    /// <typeparam name="TFamily1">The node1 family type</typeparam>
    /// <typeparam name="TFamily2">The node2 family type</typeparam>
    /// <typeparam name="TDir"> Identifies the visitor's class and can contain the state of the visit</typeparam>
    public abstract class PairDirector<TFamily1, TFamily2, TDir> : IPairDirector<TFamily1, TFamily2, TDir>
        where TDir : IPairDirector<TFamily1, TFamily2, TDir>
    {
        private readonly IPairVisitMapper<TFamily1, TFamily2,TDir> mVisitMapper;

        /// <summary>Initializes a new director for a set of visitors</summary>
        /// <param name="visitorEnumerable">list of visitors belonging to the same visitor class</param>
        protected PairDirector(IEnumerable<IPairVisitorClass<TFamily1, TFamily2, TDir>> visitorEnumerable)
        {
            mVisitMapper = new PairVisitMapper<TFamily1, TFamily2, TDir>(visitorEnumerable);
        }

        /// <summary>Initializes a new director with a shared visitMapper</summary>
        /// <param name="visitMapper">shared visitMapper for all directors of this type</param>
        protected PairDirector(IPairVisitMapper<TFamily1, TFamily2, TDir> visitMapper)
        {
            mVisitMapper = visitMapper;
        }

        /// <summary>Dispatches the call to the best visitor depending on the node's type</summary>
        /// <param name="node1">The node1 to visit</param>
        /// <param name="node2">The node2 to visit</param>
        public void Visit(TFamily1 node1, TFamily2 node2)
        {
            if (ReferenceEquals(node1, null))
                throw new ArgumentNullException("node1");

            if (ReferenceEquals(node2, null))
                throw new ArgumentNullException("node2");

            Action<IPairDirector<TFamily1, TFamily2, TDir>, TFamily1, TFamily2> visitAction;
            try
            {
                visitAction = mVisitMapper.GetVisitDelegate(node1, node2);
            }
            catch (TargetTypeNotResolvedException e)
            {
                throw new VisitorNotFoundException(GetType(), e);
            }
            visitAction(this, node1, node2);
        }
    }
}