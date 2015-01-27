using System.Diagnostics.CodeAnalysis;
using NVisitor.Api.Marker;

namespace NVisitor.Api.Common
{
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY CARACTERIZE THE DIRECTOR
    public interface IDispatcherBase<TFamily, TGenericDirector, TResult> : IDispatcherMarker
    {
        TResult Visit(TGenericDirector director, TFamily node);
    }
}