using System.Diagnostics.CodeAnalysis;
using NVisitor.Api.Marker;

namespace NVisitor.Api.PairBatch
{
    /// <summary>Identify the class of visitors related to a director</summary>
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY CARACTERIZE THE DIRECTOR (used by reflection)
    [SuppressMessage("ReSharper", "UnusedTypeParameter")]       // GENERIC PARAMETERS STRICTLY CARACTERIZE THE DIRECTOR (used by reflection)
    public interface IPairVisitorClass<TFamily1, TFamily2, TDir> : IVisitorMarker
        where TDir : IPairDirector<TFamily1, TFamily2, TDir>
    {
    }
}