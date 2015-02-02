using System.Diagnostics.CodeAnalysis;
using NVisitor.Api.Marker;

namespace NVisitor.Api.Common
{
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY CARACTERIZE THE DIRECTOR
    public interface IDispatcherBase<TFamily, TDir, TResult> : IDispatcherMarker
    {
        TResult Visit(TDir director, TFamily node);
    }
}