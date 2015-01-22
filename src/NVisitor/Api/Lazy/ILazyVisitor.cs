using System.Collections;
using System.Collections.Generic;
using NVisitor.Api.Batch;

namespace NVisitor.Api.Lazy
{
    /// <summary> Contract for each single visitor implementation</summary>
    /// <typeparam name="TFamily">The node family that the director applies to</typeparam>
    /// <typeparam name="TDirector">The director related to the visitor</typeparam>
    /// <typeparam name="TNode">The node type that the visitor visit</typeparam>
    public interface ILazyVisitor<in TFamily, in TDirector, in TNode> : ILazyVisitorClass<TFamily, TDirector> 
        where TDirector : ILazyDirector<TFamily>
        where TNode : TFamily
    {
        /// <summary>The visit method to implement</summary>
        /// <param name="director">You can read and update state of the director during the visit</param>
        /// <param name="node">The node to visit</param>
        IEnumerable<Pause> Visit(TDirector director, TNode node);
    }
}