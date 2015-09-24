using System.Diagnostics.CodeAnalysis;
using NVisitor.Api.Marker;

namespace NVisitor.Api.Lazy
{
    /// <summary>Identify the class of visitors related to a director</summary>
    // GENERIC PARAMETERS STRICTLY CARACTERIZE THE DIRECTOR (used by reflection)
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")]
    // GENERIC PARAMETERS STRICTLY CARACTERIZE THE DIRECTOR (used by reflection)
    [SuppressMessage("ReSharper", "UnusedTypeParameter")]
    public interface ILazyVisitorClass<TFamily, TDir> : IVisitorMarker
        where TDir : ILazyDirector<TFamily, TDir>
    {
    }
}