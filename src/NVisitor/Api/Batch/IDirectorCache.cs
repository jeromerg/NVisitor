using System;
using System.Diagnostics.CodeAnalysis;
using NVisitor.Api.Marker;

namespace NVisitor.Api.Batch
{
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY CARACTERIZE THE DISPATCHER
    public interface IDirectorCache<TFamily, TDir> : IDirectorCacheMarker
    {
        Action<IDirector<TFamily, TDir>, TFamily> GetOrCreate(TFamily node);
    }
}