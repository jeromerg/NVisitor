using System.Diagnostics.CodeAnalysis;
using NVisitor.Api.Marker;

namespace NVisitor.Api.FuncPayload
{
    /// <summary>Identify the class of visitors related to a FuncDirector</summary>
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY CARACTERIZE THE DIRECTOR (used by reflection)
    [SuppressMessage("ReSharper", "UnusedTypeParameter")]       // GENERIC PARAMETERS STRICTLY CARACTERIZE THE DIRECTOR (used by reflection)
    public interface IFuncPayloadVisitorClass<TFamily, TDir, TPayload, TResult> : IVisitorMarker
        where TDir : IFuncPayloadDirector<TFamily, TDir, TPayload, TResult>
    {
    }
}