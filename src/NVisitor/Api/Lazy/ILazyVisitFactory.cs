using System;
using System.Collections.Generic;
using NVisitor.Api.Marker;

namespace NVisitor.Api.Lazy
{
    public interface ILazyVisitFactory<TFamily, TDir> : IVisitFactoryMarker
    {        
        Func<ILazyDirector<TFamily, TDir>, TFamily, IEnumerable<Pause>> GetVisitDelegate(TFamily node);
    }
}