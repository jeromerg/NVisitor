using System.Diagnostics.CodeAnalysis;
using NVisitor.Api.Marker;
using JetBrains.Annotations;

namespace NVisitor.Api.ActionPair
{
    /// <summary>Director's contract published to the visitors</summary>
    /// <typeparam name="TFamily1">The node1 family</typeparam>
    /// <typeparam name="TFamily2">The node2 family</typeparam>
    /// <typeparam name="TDir">The director type</typeparam>
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY IDENTIFY THE DIRECTOR
    public interface IActionPairDirector<TFamily1, TFamily2, TDir> : IDirectorMarker
        where TDir : IActionPairDirector<TFamily1, TFamily2, TDir>
    {
        void Visit([NotNull] TFamily1 node1, [NotNull] TFamily2 node2);
    }
}