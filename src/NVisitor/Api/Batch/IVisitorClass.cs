using NVisitor.Api.Marker;

namespace NVisitor.Api.Batch
{
    /// <summary>Identify the class of visitors related to a director</summary>
    // ReSharper disable once UnusedTypeParameter
    public interface IVisitorClass<in TFamily, in TDirector> : IVisitorMarker
        where TDirector : IDirector<TFamily>
    {
    }
}