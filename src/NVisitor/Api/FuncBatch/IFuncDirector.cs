using System.Diagnostics.CodeAnalysis;
using NVisitor.Api.Marker;
using NVisitor.Common.Quality;

namespace NVisitor.Api.FuncBatch
{
    /// <summary>Director's contract published to the visitors</summary>
    /// <typeparam name="TFamily">The node family</typeparam>
    /// <typeparam name="TDir">The director type</typeparam>
    /// <typeparam name="TResult">Result of the Visit call</typeparam>
    /// <typeparam name="TPayload">Payload for the visit call</typeparam>
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY IDENTIFY THE DIRECTOR
    public interface IFuncDirector<TFamily, TDir, TPayload, TResult> : IDirectorMarker
        where TDir : IFuncDirector<TFamily, TDir, TPayload, TResult>
    {
        TResult Visit([NotNull] TFamily node, TPayload payload);
    }
}