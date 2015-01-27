using System;
using System.Diagnostics.CodeAnalysis;
using NVisitor.Api.Marker;

namespace NVisitor.Api.Batch
{
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY CARACTERIZE THE DISPATCHER
    public interface IVisitFactory<TFamily, TDir> : IVisitFactoryMarker
    {
        Action<IDirector<TFamily, TDir>, TFamily> GetVisitDelegate(TFamily node);
    }
}