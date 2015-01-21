using System.Collections.Generic;
using NVisitor.Api.Marker;
using NVisitor.Util.Quality;

namespace NVisitor.Api.Lazy
{
    /// <summary> 
    /// Don't use this interface directly. Use the LazyDirector implementation instead
    /// It is internally used to publish the dispatching LazyDirector.Visit(TFamily node) to the visitors 
    /// </summary>
    /// <typeparam name="TFamily">The node family type</typeparam>
    public interface ILazyDirector<in TFamily> : IDirectorMarker
    {
        IEnumerable<Pause> Visit([NotNull] TFamily node);
    }
}