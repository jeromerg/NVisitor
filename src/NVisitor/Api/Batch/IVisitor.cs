using System.Diagnostics.CodeAnalysis;

namespace NVisitor.Api.Batch
{
    /// <summary>Visitor's contract</summary>
    /// <typeparam name="TFamily">The node family</typeparam>
    /// <typeparam name="TDir">The director type</typeparam>
    /// <typeparam name="TNode">The concrete node type supported by the visitor</typeparam>
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY CARACTERIZE THE VISITOR
    public interface IVisitor<TFamily, TDir, TNode> : IVisitorClass<TDir> 
        where TNode : TFamily
    {
        /// <summary>The visit method to implement</summary>
        /// <param name="director">director to continue the visit or read/write into the director's state </param>
        /// <param name="node">The node to visit</param>
        void Visit(IDirector<TFamily, TDir> director, TNode node);
    }
}