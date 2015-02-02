using System.Collections.Generic;
using NVisitor.Api.Common;

namespace NVisitor.Api.Lazy
{
    public interface ILazyDispatcher<TFamily, TDir> : IDispatcherBase<TFamily, ILazyDirector<TFamily, TDir>, IEnumerable<Pause>>
    {        
    }

    /// <summary>Dispatches the call to the best visit method, caches the resulting delegates for subsequent calls</summary>
    /// <typeparam name="TFamily">The node family</typeparam>
    /// <typeparam name="TDir">The concrete director type (and state)</typeparam>
    public class LazyDispatcher<TFamily, TDir>
        : DispatcherBase<TFamily, TDir, ILazyDirector<TFamily, TDir>, ILazyVisitorClass<TFamily, TDir>, IEnumerable<Pause>>
        , ILazyDispatcher<TFamily, TDir> 
        where TDir : ILazyDirector<TFamily, TDir>
    {
        public LazyDispatcher(IEnumerable<ILazyVisitorClass<TFamily, TDir>> visitors)
            : base(visitors, typeof(ILazyVisitor<,,>), 2, "Visit")
        {
        }
    }
}