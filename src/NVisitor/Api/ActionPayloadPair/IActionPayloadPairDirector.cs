using System.Diagnostics.CodeAnalysis;
using NVisitor.Api.Marker;
using NVisitor.Common.Quality;

namespace NVisitor.Api.ActionPayloadPair
{
    /// <summary>Director's contract published to the visitors</summary>
    /// <typeparam name="TFamily1">The node1 family</typeparam>
    /// <typeparam name="TFamily2">The node2 family</typeparam>
    /// <typeparam name="TDir">The director type</typeparam>
    /// <typeparam name="TPayload">Payload passed to visitor</typeparam>
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY IDENTIFY THE DIRECTOR
    public interface IActionPayloadPairDirector<TFamily1, TFamily2, TDir, TPayload> : IDirectorMarker
        where TDir : IActionPayloadPairDirector<TFamily1, TFamily2, TDir, TPayload>
    {
        void Visit([NotNull] TFamily1 node1, [NotNull] TFamily2 node2, TPayload payload);
    }
}