using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NVisitor.Api.Lazy
{
    /// <summary>Visitor's contract</summary>
    /// <typeparam name="TFamily">The node family</typeparam>
    /// <typeparam name="TDir">The director type</typeparam>
    /// <typeparam name="TNode">The concrete node type supported by the visitor</typeparam>
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY CARACTERIZE THE VISITOR
    public interface ILazyVisitor<TFamily, TDir, TNode> : ILazyVisitorClass<TFamily, TDir>
        where TNode : TFamily
        where TDir : ILazyDirector<TFamily, TDir>
    {
        /// <summary>The visit method to implement</summary>
        /// <param name="director">director to continue the visit or read/write into the director's state </param>
        /// <param name="node">The node to visit</param>
        IEnumerable<Pause> Visit(TDir director, TNode node);
    }
}