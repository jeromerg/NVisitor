using System.Diagnostics.CodeAnalysis;

namespace NVisitor.Api.FuncPayloadPair
{
    /// <summary>Visitor's contract</summary>
    /// <typeparam name="TFamily1">The node1 family</typeparam>
    /// <typeparam name="TFamily2">The node2 family</typeparam>
    /// <typeparam name="TDir">The director type</typeparam>
    /// <typeparam name="TNode1">The concrete node1 type supported by the visitor</typeparam>
    /// <typeparam name="TNode2">The concrete node2 type supported by the visitor</typeparam>
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY CARACTERIZE THE VISITOR
    public interface IFuncPayloadPairVisitor<TFamily1, TFamily2, TDir, TNode1, TNode2, TPayload, TResult> 
        : IFuncPayloadPairVisitorClass<TFamily1, TFamily2, TDir, TPayload, TResult> 
        where TNode1 : TFamily1
        where TNode2 : TFamily2
        where TDir : IFuncPayloadPairDirector<TFamily1, TFamily2, TDir, TPayload, TResult>
    {
        /// <summary>The visit method to implement</summary>
        /// <param name="director">director to continue the visit or read/write into the director's state </param>
        /// <param name="node1">The node1 to visit</param>
        /// <param name="node2">The node2 to visit</param>
        TResult Visit(TDir director, TNode1 node1, TNode2 node2, TPayload payload);
    }
}