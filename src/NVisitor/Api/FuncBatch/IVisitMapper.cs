using System;
using NVisitor.Api.Marker;

namespace NVisitor.Api.FuncBatch
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily">Node's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    /// <typeparam name="TResult">Result of the Visit call</typeparam>
    /// <typeparam name="TPayload">Payload for the visit call</typeparam>
    public interface IFuncVisitMapper<TFamily, TDir, TPayload, TResult> : IVisitMapperMarker
        where TDir : IFuncDirector<TFamily, TDir, TPayload, TResult>
    {
        Func<IFuncDirector<TFamily, TDir, TPayload, TResult>, TFamily, TPayload, TResult> GetVisitDelegate(TFamily node);
    }
}