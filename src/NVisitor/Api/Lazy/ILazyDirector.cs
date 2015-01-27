using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NVisitor.Api.Marker;
using NVisitor.Common.Quality;

namespace NVisitor.Api.Lazy
{
    /// <summary>Director's contract published to the visitors</summary>
    /// <typeparam name="TFamily">The node family</typeparam>
    /// <typeparam name="TDir">The director type</typeparam>
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY IDENTIFY THE DIRECTOR
    public interface ILazyDirector<TFamily, TDir> : IDirectorMarker
    {
        TDir State { get; }
        IEnumerable<Pause> Visit([NotNull] TFamily node);
    }
}