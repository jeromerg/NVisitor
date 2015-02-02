using System.Diagnostics.CodeAnalysis;
using NVisitor.Api.Marker;
using NVisitor.Common.Quality;

namespace NVisitor.Api.Batch
{
    /// <summary>Director's contract published to the visitors</summary>
    /// <typeparam name="TFamily">The node family</typeparam>
    /// <typeparam name="TDir">The director type</typeparam>
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY IDENTIFY THE DIRECTOR
    public interface IDirector<TFamily, TDir> : IDirectorMarker
        where TDir : IDirector<TFamily, TDir>
    {
        void Visit([NotNull] TFamily node);
    }
}