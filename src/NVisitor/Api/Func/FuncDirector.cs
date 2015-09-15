using System;
using System.Collections.Generic;
using NVisitor.Common.Topo;

namespace NVisitor.Api.Func
{
    /// <summary>Director for function visitor</summary>
    /// <typeparam name="TFamily">The node family type</typeparam>
    /// <typeparam name="TDir"> Identifies the visitor's class and can contain the state of the visit</typeparam>
    /// <typeparam name="TResult">Result of the Visit call</typeparam>
    public abstract class FuncDirector<TFamily, TDir, TResult> : IFuncDirector<TFamily, TDir, TResult>
        where TDir : IFuncDirector<TFamily, TDir, TResult>
    {
        private readonly IFuncVisitMapper<TFamily, TDir, TResult> mVisitMapper;

        /// <summary>Initializes a new director for a set of visitors</summary>
        /// <param name="visitorEnumerable">list of visitors belonging to the same visitor class</param>
        protected FuncDirector(IEnumerable<IFuncVisitorClass<TFamily, TDir, TResult>> visitorEnumerable)
        {
            mVisitMapper = new FuncVisitMapper<TFamily, TDir, TResult>(visitorEnumerable);
        }

        /// <summary>Initializes a new director with a shared visitMapper</summary>
        /// <param name="visitMapper">shared visitMapper for all directors of this type</param>
        protected FuncDirector(IFuncVisitMapper<TFamily, TDir, TResult> visitMapper)
        {
            mVisitMapper = visitMapper;
        }

        /// <summary>Dispatches the call to the best visitor depending on the node's type</summary>
        /// <param name="node">The node to visit</param>
        public TResult Visit(TFamily node)
        {
            if (ReferenceEquals(node, null))
                throw new ArgumentNullException("node");

            Func<IFuncDirector<TFamily, TDir, TResult>, TFamily, TResult> visitAction;
            try
            {
                visitAction = mVisitMapper.GetVisitDelegate(node);
            }
            catch (TargetTypeNotResolvedException e)
            {
                throw new VisitorNotFoundException(GetType(), e);
            }
            return visitAction(this, node);
        }
    }
}