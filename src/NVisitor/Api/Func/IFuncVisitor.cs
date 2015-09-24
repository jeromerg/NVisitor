using System.Diagnostics.CodeAnalysis;

namespace NVisitor.Api.Func
{
    /// <summary>Visitor's contract</summary>
    /// <typeparam name="TFamily">The node family</typeparam>
    /// <typeparam name="TDir">The director type</typeparam>
    /// <typeparam name="TNode">The concrete node type supported by the visitor</typeparam>
    /// <typeparam name="TResult">Result of the Visit call</typeparam>
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY CARACTERIZE THE VISITOR
    public interface IFuncVisitor<TFamily, TDir, TNode, TResult> : IFuncVisitorClass<TFamily, TDir, TResult>
        where TNode : TFamily
        where TDir : IFuncDirector<TFamily, TDir, TResult>
    {
        /// <summary>The visit method to implement</summary>
        /// <param name="director">director to continue the visit or read/write into the director's state </param>
        /// <param name="node">The node to visit</param>
        /// <returns>The result of the visit</returns>
        TResult Visit(TDir director, TNode node);
    }
}