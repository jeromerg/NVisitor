using NVisitor.Api.Marker;
using NVisitor.Util.Quality;

namespace NVisitor.Api
{
    public interface IDirector<in TNodeBase> : IDirector
    {
        void Visit([NotNull] TNodeBase node);
    }
}