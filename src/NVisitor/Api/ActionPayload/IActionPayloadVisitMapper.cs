using System;
using NVisitor.Api.Marker;

namespace NVisitor.Api.ActionPayload
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily">Node's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    /// <typeparam name="TPayload">Payload for the visit call</typeparam>
    public interface IActionPayloadVisitMapper<TFamily, TDir, TPayload> : IVisitMapperMarker
        where TDir : IActionPayloadDirector<TFamily, TDir, TPayload>
    {
        Action<IActionPayloadDirector<TFamily, TDir, TPayload>, TFamily, TPayload> GetVisitDelegate(TFamily node);
    }
}