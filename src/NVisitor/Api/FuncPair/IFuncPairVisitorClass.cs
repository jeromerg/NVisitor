using System.Diagnostics.CodeAnalysis;
using NVisitor.Api.Marker;

namespace NVisitor.Api.FuncPair
{
    /// <summary>Identify the class of visitors related to a director</summary>
    // GENERIC PARAMETERS STRICTLY CARACTERIZE THE DIRECTOR (used by reflection)
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")]
    // GENERIC PARAMETERS STRICTLY CARACTERIZE THE DIRECTOR (used by reflection)
    [SuppressMessage("ReSharper", "UnusedTypeParameter")]
    public interface IFuncPairVisitorClass<TFamily1, TFamily2, TDir, TResult> : IVisitorMarker
        where TDir : IFuncPairDirector<TFamily1, TFamily2, TDir, TResult>
    {
    }
}