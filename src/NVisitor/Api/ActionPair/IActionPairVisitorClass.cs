using System.Diagnostics.CodeAnalysis;
using NVisitor.Api.Marker;

namespace NVisitor.Api.ActionPair
{
    /// <summary>Identify the class of visitors related to a director</summary>
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY CARACTERIZE THE DIRECTOR (used by reflection)
    [SuppressMessage("ReSharper", "UnusedTypeParameter")]       // GENERIC PARAMETERS STRICTLY CARACTERIZE THE DIRECTOR (used by reflection)
    public interface IActionPairVisitorClass<TFamily1, TFamily2, TDir> : IVisitorMarker
        where TDir : IActionPairDirector<TFamily1, TFamily2, TDir>
    {
    }
}