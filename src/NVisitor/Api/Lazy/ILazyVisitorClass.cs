using NVisitor.Api.Marker;

namespace NVisitor.Api.Lazy
{
    /// <summary>Identify the visitor class related to a director</summary>
    // ReSharper disable once UnusedTypeParameter
    public interface ILazyVisitorClass<in TFamily, in TDirector> : IVisitorMarker
        where TDirector : ILazyDirector<TFamily>
    {
    }
}