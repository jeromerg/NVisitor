using System;
using NVisitor.Api.Marker;

namespace NVisitor.Api.Batch
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily">Node's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    public interface IVisitMapper<TFamily, TDir> : IVisitMapperMarker 
        where TDir : IDirector<TFamily, TDir>
    {
        Action<IDirector<TFamily, TDir>, TFamily> GetVisitDelegate(TFamily node);
    }
}