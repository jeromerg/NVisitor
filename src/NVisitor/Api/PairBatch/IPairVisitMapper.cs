using System;
using NVisitor.Api.Marker;

namespace NVisitor.Api.PairBatch
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily1">Node1's family</typeparam>
    /// <typeparam name="TFamily2">Node2's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    public interface IPairVisitMapper<TFamily1, TFamily2, TDir> : IVisitMapperMarker
        where TDir : IPairDirector<TFamily1, TFamily2, TDir>
    {
        Action<IPairDirector<TFamily1, TFamily2, TDir>, TFamily1, TFamily2> GetVisitDelegate(TFamily1 node1, TFamily2 node2);
    }
}