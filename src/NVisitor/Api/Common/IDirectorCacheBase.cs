using System.Diagnostics.CodeAnalysis;
using NVisitor.Api.Marker;

namespace NVisitor.Api.Common
{
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY CARACTERIZE THE DIRECTOR
    public interface IDirectorCacheBase<TFamily, TDelegate> : IDirectorCacheMarker
    {
        TDelegate GetOrCreate(TFamily node);
    }
}