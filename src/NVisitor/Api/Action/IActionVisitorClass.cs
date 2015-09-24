using System.Diagnostics.CodeAnalysis;
using NVisitor.Api.Marker;

namespace NVisitor.Api.Action
{
    /// <summary>Identify the class of visitors related to a director</summary>
    // GENERIC PARAMETERS STRICTLY CARACTERIZE THE DIRECTOR (used by reflection)
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")]
    // GENERIC PARAMETERS STRICTLY CARACTERIZE THE DIRECTOR (used by reflection)
    [SuppressMessage("ReSharper", "UnusedTypeParameter")]
    public interface IActionVisitorClass<TFamily, TDir> : IVisitorMarker
        where TDir : IActionDirector<TFamily, TDir>
    {
    }
}