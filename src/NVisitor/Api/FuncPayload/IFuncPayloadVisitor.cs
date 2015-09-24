using System.Diagnostics.CodeAnalysis;

namespace NVisitor.Api.FuncPayload
{
    /// <summary>Visitor's contract</summary>
    /// <typeparam name="TFamily">The node family</typeparam>
    /// <typeparam name="TDir">The director type</typeparam>
    /// <typeparam name="TNode">The concrete node type supported by the visitor</typeparam>
    /// <typeparam name="TResult">Result of the Visit call</typeparam>
    /// <typeparam name="TPayload">Payload for the visit call</typeparam>
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY CARACTERIZE THE VISITOR
    public interface IFuncPayloadVisitor<TFamily, TDir, TNode, TPayload, TResult>
        : IFuncPayloadVisitorClass<TFamily, TDir, TPayload, TResult>
        where TNode : TFamily
        where TDir : IFuncPayloadDirector<TFamily, TDir, TPayload, TResult>
    {
        /// <summary>The visit method to implement</summary>
        /// <param name="director">director to continue the visit or read/write into the director's state </param>
        /// <param name="node">The node to visit</param>
        /// <param name="payload">The payload to pass to the visitor during the visit</param>
        /// <returns>The result of the visit</returns>
        TResult Visit(TDir director, TNode node, TPayload payload);
    }
}