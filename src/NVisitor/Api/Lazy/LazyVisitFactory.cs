using System;
using System.Collections.Generic;
using System.Reflection;
using NVisitor.Api.Common;

namespace NVisitor.Api.Lazy
{
    /// <summary>Dispatches the call to the best visit method, caches the resulting delegates for subsequent calls</summary>
    /// <typeparam name="TFamily">The node family</typeparam>
    /// <typeparam name="TDir">The concrete director type (and state)</typeparam>
    public class LazyVisitFactory<TFamily, TDir>
        : VisitFactory<TFamily, ILazyVisitorClass<TDir>, Func<ILazyDirector<TFamily, TDir>, TFamily, IEnumerable<Pause>>>
        , ILazyVisitFactory<TFamily, TDir>
    {
        public LazyVisitFactory(IEnumerable<ILazyVisitorClass<TDir>> visitors)
            : base(visitors, typeof(ILazyVisitor<,,>), 2)
        {
        }

        protected override Func<ILazyDirector<TFamily, TDir>, TFamily, IEnumerable<Pause>> CreateVisitDelegate(TFamily node, Type visitorNodeType, ILazyVisitorClass<TDir> visitorInstance, Func<ILazyDirector<TFamily, TDir>, TFamily, IEnumerable<Pause>> directorDelegate)
        {            
            // create the closed generic type of the visitor
            Type visitorClosedType = typeof(ILazyVisitor<,,>).MakeGenericType(typeof(TFamily), typeof(TDir), visitorNodeType);

            // find the visit method in the closed generic type of the visitor
            MethodInfo visitMethod = visitorClosedType.GetMethod("Visit");

            // prepare the visit action and cache it
            return (someDirector, someNode) => (IEnumerable<Pause>) visitMethod.Invoke(visitorInstance, new object[] { someDirector, someNode });

        }
    }
}