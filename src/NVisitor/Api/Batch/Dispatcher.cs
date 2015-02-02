using System.Collections.Generic;
using NVisitor.Api.Common;

namespace NVisitor.Api.Batch
{
    /// <summary>Dispatches the call to the best visit method, caches the resulting delegates for subsequent calls</summary>
    /// <typeparam name="TFamily">The node family</typeparam>
    /// <typeparam name="TDir">The concrete director type (and state)</typeparam>
    public class Dispatcher<TFamily, TDir>
        : DispatcherBase<TFamily, TDir, IDirector<TFamily, TDir>, IVisitorClass<TFamily, TDir>, object>
        , IDispatcher<TFamily, TDir>
        where TDir : IDirector<TFamily, TDir>
    {
        public Dispatcher(IEnumerable<IVisitorClass<TFamily, TDir>> visitors)
            : base(visitors, typeof(IVisitor<,,>), 2, "Visit")
        {
        }
    }
}