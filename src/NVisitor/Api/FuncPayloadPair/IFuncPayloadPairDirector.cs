using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NVisitor.Api.Marker;

namespace NVisitor.Api.FuncPayloadPair
{
    /// <summary>Director's contract published to the visitors</summary>
    /// <typeparam name="TFamily1">The node1 family</typeparam>
    /// <typeparam name="TFamily2">The node2 family</typeparam>
    /// <typeparam name="TDir">The director type</typeparam>
    /// <typeparam name="TPayload">Payload passed to visitor during the visit</typeparam>
    /// <typeparam name="TResult">Result of the visit</typeparam>
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY IDENTIFY THE DIRECTOR
    public interface IFuncPayloadPairDirector<TFamily1, TFamily2, TDir, TPayload, TResult> : IDirectorMarker
        where TDir : IFuncPayloadPairDirector<TFamily1, TFamily2, TDir, TPayload, TResult>
    {
        TResult Visit([NotNull] TFamily1 node1, [NotNull] TFamily2 node2, TPayload payload);
    }
}