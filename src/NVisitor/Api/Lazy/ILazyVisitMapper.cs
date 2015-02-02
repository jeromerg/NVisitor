using System;
using System.Collections.Generic;
using NVisitor.Api.Marker;

namespace NVisitor.Api.Lazy
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily">Node's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    public interface ILazyVisitMapper<TFamily, TDir> : IVisitMapperMarker
    {        
        Func<ILazyDirector<TFamily, TDir>, TFamily, IEnumerable<Pause>> GetVisitDelegate(TFamily node);
    }
}