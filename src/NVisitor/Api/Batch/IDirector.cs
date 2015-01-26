using System.Diagnostics.CodeAnalysis;
using NVisitor.Api.Marker;
using NVisitor.Common.Quality;

namespace NVisitor.Api.Batch
{
    /// <summary> 
    /// Don't use this interface directly. Use the Director implementation instead
    /// It is internally used to publish the dispatching Director.Visit(TFamily node) to the visitors 
    /// </summary>
    /// <typeparam name="TFamily">The node family type</typeparam>
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY CARACTERIZE THE DIRECTOR
    public interface IDirector<TFamily> : IDirectorMarker
    {
        void Visit([NotNull] TFamily node);
    }
}