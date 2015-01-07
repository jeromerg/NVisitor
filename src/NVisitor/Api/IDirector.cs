using NVisitor.Api.Marker;
using NVisitor.Util.Quality;

namespace NVisitor.Api
{
    public interface IDirector<in TFamily> : IDirector
    {
        void Visit([NotNull] TFamily node);
    }
}