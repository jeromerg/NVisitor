using System;
using NVisitor.Api.Marker;

namespace NVisitor.Api.FuncPair
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily1">Node1's family</typeparam>
    /// <typeparam name="TFamily2">Node2's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    public interface IFuncPairVisitMapper<TFamily1, TFamily2, TDir, TResult> : IVisitMapperMarker
        where TDir : IFuncPairDirector<TFamily1, TFamily2, TDir, TResult>
    {
        Func<IFuncPairDirector<TFamily1, TFamily2, TDir, TResult>, TFamily1, TFamily2, TResult>
            GetVisitDelegate(TFamily1 node1, TFamily2 node2);
    }
}