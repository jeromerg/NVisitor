using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace NVisitor.Api.ActionPayloadPair
{
    /// <summary>Visitor's contract</summary>
    /// <typeparam name="TFamily1">The node1 family</typeparam>
    /// <typeparam name="TFamily2">The node2 family</typeparam>
    /// <typeparam name="TDir">The director type</typeparam>
    /// <typeparam name="TNode1">The concrete node1 type supported by the visitor</typeparam>
    /// <typeparam name="TNode2">The concrete node2 type supported by the visitor</typeparam>
    /// <typeparam name="TPayload">Payload passed to visitor</typeparam>
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY CARACTERIZE THE VISITOR
    public interface IActionPayloadPairVisitor<TFamily1, TFamily2, TDir, TNode1, TNode2, TPayload>
        : IActionPayloadPairVisitorClass<TFamily1, TFamily2, TDir, TPayload>
        where TNode1 : TFamily1
        where TNode2 : TFamily2
        where TDir : IActionPayloadPairDirector<TFamily1, TFamily2, TDir, TPayload>
    {
        /// <summary>The visit method to implement</summary>
        /// <param name="director">director to continue the visit or read/write into the director's state </param>
        /// <param name="node1">The node1 to visit</param>
        /// <param name="node2">The node2 to visit</param>
        /// <param name="payload">Payload passed to visitor</param>
        void Visit([NotNull] TDir director, [NotNull] TNode1 node1, [NotNull] TNode2 node2, TPayload payload);
    }
}