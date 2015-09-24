using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NVisitor.Api.Marker;

namespace NVisitor.Api.Func
{
    /// <summary>Director's contract published to the visitors</summary>
    /// <typeparam name="TFamily">The node family</typeparam>
    /// <typeparam name="TDir">The director type</typeparam>
    /// <typeparam name="TResult">Result of the Visit call</typeparam>
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY IDENTIFY THE DIRECTOR
    public interface IFuncDirector<TFamily, TDir, TResult> : IDirectorMarker
        where TDir : IFuncDirector<TFamily, TDir, TResult>
    {
        TResult Visit([NotNull] TFamily node);
    }
}