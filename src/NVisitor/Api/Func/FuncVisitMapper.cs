using System;
using System.Collections.Generic;
using System.Reflection;
using NVisitor.Api.Common;

namespace NVisitor.Api.Func
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily">Node's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    /// <typeparam name="TResult">Result Type of the visit</typeparam>
    public class FuncVisitMapper<TFamily, TDir, TResult>
        : VisitMapperBase
              <TFamily, IFuncVisitorClass<TFamily, TDir, TResult>, Func<IFuncDirector<TFamily, TDir, TResult>, TFamily, TResult>>,
          IFuncVisitMapper<TFamily, TDir, TResult>
        where TDir : IFuncDirector<TFamily, TDir, TResult>
    {
        public FuncVisitMapper(IEnumerable<IFuncVisitorClass<TFamily, TDir, TResult>> visitors)
            : base(visitors, typeof (IFuncVisitor<,,,>), 2)
        {
        }

        protected override Func<IFuncDirector<TFamily, TDir, TResult>, TFamily, TResult>
            CreateVisitDelegate(TFamily node,
                                Type visitorNodeType,
                                IFuncVisitorClass<TFamily, TDir, TResult> visitorInstance,
                                Func<IFuncDirector<TFamily, TDir, TResult>, TFamily, TResult> directorDelegate)
        {
            // create the closed generic type of the visitor
            Type visitorClosedType = typeof (IFuncVisitor<,,,>).MakeGenericType(typeof (TFamily),
                                                                                typeof (TDir),
                                                                                visitorNodeType,
                                                                                typeof (TResult));

            // find the visit method in the closed generic type of the visitor
            MethodInfo visitMethod = visitorClosedType.GetMethod("Visit");

            // prepare the visit action and dispatcher it
            return
                (someDirector, someNode) => (TResult) visitMethod.Invoke(visitorInstance, new object[] {someDirector, someNode});
        }
    }
}