using System;
using NVisitor.Api.Marker;

namespace NVisitor.Api.ActionPayloadPair
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily1">Node1's family</typeparam>
    /// <typeparam name="TFamily2">Node2's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    /// <typeparam name="TPayload">Payload passed to visitor</typeparam>
    public interface IActionPayloadPairVisitMapper<TFamily1, TFamily2, TDir, TPayload> : IVisitMapperMarker
        where TDir : IActionPayloadPairDirector<TFamily1, TFamily2, TDir, TPayload>
    {
        Action<IActionPayloadPairDirector<TFamily1, TFamily2, TDir, TPayload>, TFamily1, TFamily2, TPayload>
            GetVisitDelegate(TFamily1 node1, TFamily2 node2);
    }
}