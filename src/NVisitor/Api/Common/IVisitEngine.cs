using System.Diagnostics.CodeAnalysis;
using NVisitor.Api.Marker;

namespace NVisitor.Api.Common
{
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY CARACTERIZE THE DIRECTOR
    public interface IVisitEngine<TFamily, TDirector, TResult> : IEngineMarker
    {
        TResult Visit(TDirector director, TFamily node);
    }
}