using System.Diagnostics.CodeAnalysis;
using NVisitor.Api.Marker;
using JetBrains.Annotations;

namespace NVisitor.Api.Action
{
    /// <summary>Director's contract published to the visitors</summary>
    /// <typeparam name="TFamily">The node family</typeparam>
    /// <typeparam name="TDir">The director type</typeparam>
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY IDENTIFY THE DIRECTOR
    public interface IActionDirector<TFamily, TDir> : IDirectorMarker
        where TDir : IActionDirector<TFamily, TDir>
    {
        void Visit([NotNull] TFamily node);
    }
}