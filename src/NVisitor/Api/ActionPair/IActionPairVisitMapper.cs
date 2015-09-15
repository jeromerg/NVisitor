using System;
using NVisitor.Api.Marker;

namespace NVisitor.Api.ActionPair
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily1">Node1's family</typeparam>
    /// <typeparam name="TFamily2">Node2's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    public interface IActionPairVisitMapper<TFamily1, TFamily2, TDir> : IVisitMapperMarker
        where TDir : IActionPairDirector<TFamily1, TFamily2, TDir>
    {
        Action<IActionPairDirector<TFamily1, TFamily2, TDir>, TFamily1, TFamily2> GetVisitDelegate(TFamily1 node1, TFamily2 node2);
    }
}