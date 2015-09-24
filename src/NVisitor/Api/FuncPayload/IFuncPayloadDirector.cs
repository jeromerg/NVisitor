using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NVisitor.Api.Marker;

namespace NVisitor.Api.FuncPayload
{
    /// <summary>Director's contract published to the visitors</summary>
    /// <typeparam name="TFamily">The node family</typeparam>
    /// <typeparam name="TDir">The director type</typeparam>
    /// <typeparam name="TResult">Result of the Visit call</typeparam>
    /// <typeparam name="TPayload">Payload for the visit call</typeparam>
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY IDENTIFY THE DIRECTOR
    public interface IFuncPayloadDirector<TFamily, TDir, TPayload, TResult> : IDirectorMarker
        where TDir : IFuncPayloadDirector<TFamily, TDir, TPayload, TResult>
    {
        TResult Visit([NotNull] TFamily node, TPayload payload);
    }
}