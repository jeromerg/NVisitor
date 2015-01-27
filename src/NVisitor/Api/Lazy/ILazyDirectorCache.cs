using System;
using System.Collections.Generic;
using NVisitor.Api.Marker;

namespace NVisitor.Api.Lazy
{
    public interface ILazyDirectorCache<TFamily, TDir> : IDirectorCacheMarker
    {        
        Func<ILazyDirector<TFamily, TDir>, TFamily, IEnumerable<Pause>> GetOrCreate(TFamily node);
    }
}