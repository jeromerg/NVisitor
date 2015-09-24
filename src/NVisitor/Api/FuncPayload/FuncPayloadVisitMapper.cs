using System;
using System.Collections.Generic;
using System.Reflection;
using NVisitor.Api.Common;

namespace NVisitor.Api.FuncPayload
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily">Node's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    public class FuncPayloadVisitMapper<TFamily, TDir, TPayload, TResult>
        : VisitMapperBase
              <TFamily, IFuncPayloadVisitorClass<TFamily, TDir, TPayload, TResult>,
              Func<IFuncPayloadDirector<TFamily, TDir, TPayload, TResult>, TFamily, TPayload, TResult>>,
          IFuncPayloadVisitMapper<TFamily, TDir, TPayload, TResult>
        where TDir : IFuncPayloadDirector<TFamily, TDir, TPayload, TResult>
    {
        public FuncPayloadVisitMapper(IEnumerable<IFuncPayloadVisitorClass<TFamily, TDir, TPayload, TResult>> visitors)
            : base(visitors, typeof (IFuncPayloadVisitor<,,,,>), 2)
        {
        }

        protected override Func<IFuncPayloadDirector<TFamily, TDir, TPayload, TResult>, TFamily, TPayload, TResult>
            CreateVisitDelegate(TFamily node,
                                Type visitorNodeType,
                                IFuncPayloadVisitorClass<TFamily, TDir, TPayload, TResult> visitorInstance,
                                Func<IFuncPayloadDirector<TFamily, TDir, TPayload, TResult>, TFamily, TPayload, TResult>
                                    directorDelegate)
        {
            // create the closed generic type of the visitor
            Type visitorClosedType = typeof (IFuncPayloadVisitor<,,,,>).MakeGenericType(typeof (TFamily),
                                                                                        typeof (TDir),
                                                                                        visitorNodeType,
                                                                                        typeof (TPayload),
                                                                                        typeof (TResult));

            // find the visit method in the closed generic type of the visitor
            MethodInfo visitMethod = visitorClosedType.GetMethod("Visit");

            // prepare the visit action and dispatcher it
            return
                (someDirector, someNode, somePayload) =>
                (TResult) visitMethod.Invoke(visitorInstance, new object[] {someDirector, someNode, somePayload});
        }
    }
}