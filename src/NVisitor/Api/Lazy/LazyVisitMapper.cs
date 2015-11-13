using System;
using System.Collections.Generic;
using System.Reflection;
using NVisitor.Api.Common;
using NVisitor.Common;

namespace NVisitor.Api.Lazy
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily">Node's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    public class LazyVisitMapper<TFamily, TDir>
        : VisitMapperBase
              <TFamily, ILazyVisitorClass<TFamily, TDir>, Func<ILazyDirector<TFamily, TDir>, TFamily, IEnumerable<Pause>>>,
          ILazyVisitMapper<TFamily, TDir>
        where TDir : ILazyDirector<TFamily, TDir>
    {
        public LazyVisitMapper(IEnumerable<ILazyVisitorClass<TFamily, TDir>> visitors)
            : base(visitors, typeof (ILazyVisitor<,,>), 2)
        {
        }

        protected override Func<ILazyDirector<TFamily, TDir>, TFamily, IEnumerable<Pause>>
            CreateVisitDelegate(TFamily node,
                                Type visitorNodeType,
                                ILazyVisitorClass<TFamily, TDir> visitorInstance,
                                Func<ILazyDirector<TFamily, TDir>, TFamily, IEnumerable<Pause>>
                                    directorDelegate)
        {
            // create the closed generic type of the visitor
            Type visitorClosedType = typeof (ILazyVisitor<,,>).MakeGenericType(typeof (TFamily), typeof (TDir), visitorNodeType);

            // find the visit method in the closed generic type of the visitor
            MethodInfo visitMethod = visitorClosedType.GetMethod("Visit");

            // prepare the visit action and dispatcher it
            return
                (someDirector, someNode) =>
                (IEnumerable<Pause>)InvokeUtil.InvokeWithUnwrapper(visitMethod, visitorInstance, new object[] { someDirector, someNode });
        }
    }
}