using System;
using System.Collections.Generic;
using System.Reflection;
using NVisitor.Api.Common;

namespace NVisitor.Api.FuncBatch
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily">Node's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    public class FuncVisitMapper<TFamily, TDir, TPayload, TResult>
        : VisitMapperBase<TFamily, IFuncVisitorClass<TFamily, TDir, TPayload, TResult>, Func<IFuncDirector<TFamily, TDir, TPayload, TResult>, TFamily, TPayload, TResult>>
        , IFuncVisitMapper<TFamily, TDir, TPayload, TResult>
        where TDir : IFuncDirector<TFamily, TDir, TPayload, TResult>
    {
        public FuncVisitMapper(IEnumerable<IFuncVisitorClass<TFamily, TDir, TPayload, TResult>> visitors)
            : base(visitors, typeof (IFuncVisitor<,,,,>), 2)
        {
        }

        protected override Func<IFuncDirector<TFamily, TDir, TPayload, TResult>, TFamily, TPayload, TResult> 
            CreateVisitDelegate(TFamily node, 
                                Type visitorNodeType,
                                IFuncVisitorClass<TFamily, TDir, TPayload, TResult> visitorInstance,
                                Func<IFuncDirector<TFamily, TDir, TPayload, TResult>, TFamily, TPayload, TResult> directorDelegate)
        {
            // create the closed generic type of the visitor
            Type visitorClosedType = typeof(IFuncVisitor<,,,,>).MakeGenericType(typeof(TFamily), typeof(TDir), visitorNodeType, typeof(TPayload), typeof(TResult));

            // find the visit method in the closed generic type of the visitor
            MethodInfo visitMethod = visitorClosedType.GetMethod("Visit");

            // prepare the visit action and dispatcher it
            return (someDirector, someNode, somePayload) => (TResult) visitMethod.Invoke(visitorInstance, new object[] { someDirector, someNode, somePayload });

        }
    }
}