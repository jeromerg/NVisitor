using System;
using NVisitor.Api.Marker;

namespace NVisitor.Api.FuncPayloadPair
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily1">Node1's family</typeparam>
    /// <typeparam name="TFamily2">Node2's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    public interface IFuncPayloadPairVisitMapper<TFamily1, TFamily2, TDir, TPayload, TResult> : IVisitMapperMarker
        where TDir : IFuncPayloadPairDirector<TFamily1, TFamily2, TDir, TPayload, TResult>
    {
        Func<IFuncPayloadPairDirector<TFamily1, TFamily2, TDir, TPayload, TResult>, TFamily1, TFamily2, TPayload, TResult>
            GetVisitDelegate(TFamily1 node1, TFamily2 node2);
    }
}