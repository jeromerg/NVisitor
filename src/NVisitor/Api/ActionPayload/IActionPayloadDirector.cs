using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NVisitor.Api.Marker;

namespace NVisitor.Api.ActionPayload
{
    /// <summary>Director's contract published to the visitors</summary>
    /// <typeparam name="TFamily">The node family</typeparam>
    /// <typeparam name="TDir">The director type</typeparam>
    /// <typeparam name="TPayload">Payload for the visit call</typeparam>
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY IDENTIFY THE DIRECTOR
    public interface IActionPayloadDirector<TFamily, TDir, TPayload> : IDirectorMarker
        where TDir : IActionPayloadDirector<TFamily, TDir, TPayload>
    {
        void Visit([NotNull] TFamily node, TPayload payload);
    }
}