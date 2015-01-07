namespace NVisitor.Api.Marker
{
    /// <summary>Used a marker, to lookup assemblies for Visitors (i.e. by using DI-container) </summary>
    public interface IVisitor
    {
    }

    /// <summary>Used for DI-Container initialization of related Directors</summary>
    // ReSharper disable once UnusedTypeParameter
    public interface IVisitor<in TFamily, in TDirector> : IVisitor
        where TDirector : IDirector<TFamily>
    {
    }
}