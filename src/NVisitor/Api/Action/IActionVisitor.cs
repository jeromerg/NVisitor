using System.Diagnostics.CodeAnalysis;

namespace NVisitor.Api.Action
{
    /// <summary>Visitor's contract</summary>
    /// <typeparam name="TFamily">The node family</typeparam>
    /// <typeparam name="TDir">The director type</typeparam>
    /// <typeparam name="TNode">The concrete node type supported by the visitor</typeparam>
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY CARACTERIZE THE VISITOR
    public interface IActionVisitor<TFamily, TDir, TNode> : IActionVisitorClass<TFamily, TDir> 
        where TNode : TFamily
        where TDir : IActionDirector<TFamily, TDir>
    {
        /// <summary>The visit method to implement</summary>
        /// <param name="director">director to continue the visit or read/write into the director's state </param>
        /// <param name="node">The node to visit</param>
        void Visit(TDir director, TNode node);
    }
}