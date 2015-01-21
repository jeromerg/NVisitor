using NVisitor.Api.Marker;
using NVisitor.Util.Quality;

namespace NVisitor.Api.Batch
{
    /// <summary> 
    /// Don't use this interface directly. Use the Director implementation instead
    /// It is internally used to publish the dispatching Director.Visit(TFamily node) to the visitors 
    /// </summary>
    /// <typeparam name="TFamily">The node family type</typeparam>
    public interface IDirector<in TFamily> : IDirectorMarker
    {
        void Visit([NotNull] TFamily node);
    }
}