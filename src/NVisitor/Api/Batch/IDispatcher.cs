using NVisitor.Api.Common;

namespace NVisitor.Api.Batch
{
    public interface IDispatcher<TFamily, TDir> : IDispatcherBase<TFamily, IDirector<TFamily, TDir>, object>
    {
        
    }
}