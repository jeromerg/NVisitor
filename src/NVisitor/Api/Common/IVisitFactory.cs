using System.Diagnostics.CodeAnalysis;
using NVisitor.Api.Marker;

namespace NVisitor.Api.Common
{
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY CARACTERIZE THE DIRECTOR
    public interface IVisitFactory<TFamily, TDelegate> : IVisitFactoryMarker
    {
        TDelegate GetVisitDelegate(TFamily node);
    }
}