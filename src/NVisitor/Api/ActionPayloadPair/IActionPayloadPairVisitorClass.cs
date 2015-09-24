using System.Diagnostics.CodeAnalysis;
using NVisitor.Api.Marker;

namespace NVisitor.Api.ActionPayloadPair
{
    /// <summary>Identify the class of visitors related to a director</summary>
    // GENERIC PARAMETERS STRICTLY CARACTERIZE THE DIRECTOR (used by reflection)
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")]
    // GENERIC PARAMETERS STRICTLY CARACTERIZE THE DIRECTOR (used by reflection)
    [SuppressMessage("ReSharper", "UnusedTypeParameter")]
    public interface IActionPayloadPairVisitorClass<TFamily1, TFamily2, TDir, TPayload> : IVisitorMarker
        where TDir : IActionPayloadPairDirector<TFamily1, TFamily2, TDir, TPayload>
    {
    }
}