using System;
using System.Collections.Generic;
using NVisitor.Common.Topo;

namespace NVisitor.Api.FuncPayload
{
    /// <summary>
    ///     A Director for visitor having a payload argument and a return value (function).
    ///     This type of visitor is especially useful if you want to process concurrently a node family: With the payload and
    ///     the return-value, you
    ///     avoid the problem of shared resources in concurrent environment.
    ///     1) Is the entry-point for a visit
    ///     2) Dispatches visit to the best visitor
    ///     3) Holds the state of the visit via its property State
    /// </summary>
    /// <typeparam name="TFamily">The node family type</typeparam>
    /// <typeparam name="TDir"> Identifies the visitor's class and can contain the state of the visit</typeparam>
    /// <typeparam name="TResult">Result of the Visit call</typeparam>
    /// <typeparam name="TPayload">Payload for the visit call</typeparam>
    public abstract class FuncPayloadDirector<TFamily, TDir, TPayload, TResult>
        : IFuncPayloadDirector<TFamily, TDir, TPayload, TResult>
        where TDir : IFuncPayloadDirector<TFamily, TDir, TPayload, TResult>
    {
        private readonly IFuncPayloadVisitMapper<TFamily, TDir, TPayload, TResult> mVisitMapper;

        /// <summary>Initializes a new director for a set of visitors</summary>
        /// <param name="visitorEnumerable">list of visitors belonging to the same visitor class</param>
        protected FuncPayloadDirector(IEnumerable<IFuncPayloadVisitorClass<TFamily, TDir, TPayload, TResult>> visitorEnumerable)
        {
            mVisitMapper = new FuncPayloadVisitMapper<TFamily, TDir, TPayload, TResult>(visitorEnumerable);
        }

        /// <summary>Initializes a new director with a shared visitMapper</summary>
        /// <param name="visitMapper">shared visitMapper for all directors of this type</param>
        protected FuncPayloadDirector(IFuncPayloadVisitMapper<TFamily, TDir, TPayload, TResult> visitMapper)
        {
            mVisitMapper = visitMapper;
        }

        /// <summary>Dispatches the call to the best visitor depending on the node's type</summary>
        /// <param name="node">The node to visit</param>
        /// <param name="payload">Payload to pass to the visitor</param>
        public TResult Visit(TFamily node, TPayload payload)
        {
            if (ReferenceEquals(node, null))
                throw new ArgumentNullException("node");

            Func<IFuncPayloadDirector<TFamily, TDir, TPayload, TResult>, TFamily, TPayload, TResult> visitAction;
            try
            {
                visitAction = mVisitMapper.GetVisitDelegate(node);
            }
            catch (TargetTypeNotResolvedException e)
            {
                throw new VisitorNotFoundException(GetType(), e);
            }
            return visitAction(this, node, payload);
        }
    }
}