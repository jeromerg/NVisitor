using System;
using NVisitor.Api.Marker;

namespace NVisitor.Api.Action
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily">Node's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    public interface IActionVisitMapper<TFamily, TDir> : IVisitMapperMarker
        where TDir : IActionDirector<TFamily, TDir>
    {
        Action<IActionDirector<TFamily, TDir>, TFamily> GetVisitDelegate(TFamily node);
    }
}