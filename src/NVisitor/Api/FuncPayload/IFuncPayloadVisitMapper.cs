using System;
using NVisitor.Api.Marker;

namespace NVisitor.Api.FuncPayload
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily">Node's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    /// <typeparam name="TResult">Result of the Visit call</typeparam>
    /// <typeparam name="TPayload">Payload for the visit call</typeparam>
    public interface IFuncPayloadVisitMapper<TFamily, TDir, TPayload, TResult> : IVisitMapperMarker
        where TDir : IFuncPayloadDirector<TFamily, TDir, TPayload, TResult>
    {
        Func<IFuncPayloadDirector<TFamily, TDir, TPayload, TResult>, TFamily, TPayload, TResult> GetVisitDelegate(TFamily node);
    }
}