using System;
using System.Collections.Generic;
using NVisitor.Common.Topo;

namespace NVisitor.Api.FuncPayloadPair
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
    public abstract class FuncPayloadPairDirector<TFamily1, TFamily2, TDir, TPayload, TResult> 
        : IFuncPayloadPairDirector<TFamily1, TFamily2, TDir, TPayload, TResult>
        where TDir : IFuncPayloadPairDirector<TFamily1, TFamily2, TDir, TPayload, TResult>
    {
        private readonly IFuncPayloadPairVisitMapper<TFamily1, TFamily2, TDir, TPayload, TResult> mVisitMapper;

        /// <summary>Initializes a new director for a set of visitors</summary>
        /// <param name="visitorEnumerable">list of visitors belonging to the same visitor class</param>
        protected FuncPayloadPairDirector(IEnumerable<IFuncPayloadPairVisitorClass<TFamily1, TFamily2, TDir, TPayload, TResult>> visitorEnumerable)
        {
            mVisitMapper = new FuncPayloadPairVisitMapper<TFamily1, TFamily2, TDir, TPayload, TResult>(visitorEnumerable);
        }

        /// <summary>Initializes a new director with a shared visitMapper</summary>
        /// <param name="visitMapper">shared visitMapper for all directors of this type</param>
        protected FuncPayloadPairDirector(IFuncPayloadPairVisitMapper<TFamily1, TFamily2, TDir, TPayload, TResult> visitMapper)
        {
            mVisitMapper = visitMapper;
        }

        /// <summary>Dispatches the call to the best visitor depending on the node's type</summary>
        /// <param name="node1">The node1 to visit</param>
        /// <param name="node2">The node2 to visit</param>
        public TResult Visit(TFamily1 node1, TFamily2 node2, TPayload payload)
        {
            if (ReferenceEquals(node1, null))
                throw new ArgumentNullException("node1");

            if (ReferenceEquals(node2, null))
                throw new ArgumentNullException("node2");

            Func<IFuncPayloadPairDirector<TFamily1, TFamily2, TDir, TPayload, TResult>, TFamily1, TFamily2, TPayload, TResult> visitAction;
            try
            {
                visitAction = mVisitMapper.GetVisitDelegate(node1, node2);
            }
            catch (TargetTypeNotResolvedException e)
            {
                throw new VisitorNotFoundException(GetType(), e);
            }
            return visitAction(this, node1, node2, payload);
        }
    }
}