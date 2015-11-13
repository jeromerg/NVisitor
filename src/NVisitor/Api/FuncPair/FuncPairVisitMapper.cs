using System;
using System.Collections.Generic;
using System.Reflection;
using NVisitor.Api.Common;
using NVisitor.Common;

namespace NVisitor.Api.FuncPair
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily1">Node1's family</typeparam>
    /// <typeparam name="TFamily2">Node2's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    /// <typeparam name="TResult">Result of visit</typeparam>
    public class FuncPairVisitMapper<TFamily1, TFamily2, TDir, TResult>
        : PairVisitMapperBase<TFamily1,
              TFamily2,
              IFuncPairVisitorClass<TFamily1, TFamily2, TDir, TResult>,
              Func<IFuncPairDirector<TFamily1, TFamily2, TDir, TResult>,
              TFamily1,
              TFamily2,
              TResult>>,
          IFuncPairVisitMapper<TFamily1, TFamily2, TDir, TResult>
        where TDir : IFuncPairDirector<TFamily1, TFamily2, TDir, TResult>
    {
        public FuncPairVisitMapper(IEnumerable<IFuncPairVisitorClass<TFamily1, TFamily2, TDir, TResult>> visitors)
            : base(visitors, typeof (IFuncPairVisitor<,,,,,>), 3, 4)
        {
        }

        protected override Func<IFuncPairDirector<TFamily1, TFamily2, TDir, TResult>, TFamily1, TFamily2, TResult>
            CreateVisitDelegate(
            TFamily1 node1,
            TFamily2 node2,
            Type visitorNode1Type,
            Type visitorNode2Type,
            IFuncPairVisitorClass<TFamily1, TFamily2, TDir, TResult> visitorInstance,
            Func<IFuncPairDirector<TFamily1, TFamily2, TDir, TResult>, TFamily1, TFamily2, TResult> directorDelegate)
        {
            // create the closed generic type of the visitor
            Type visitorClosedType = typeof (IFuncPairVisitor< /*TF1*/, /*TF2*/, /*TDir*/, /*TNod1*/, /*TNod2*/, /*TRes*/>)
                .MakeGenericType(typeof (TFamily1),
                                 typeof (TFamily2),
                                 typeof (TDir),
                                 visitorNode1Type,
                                 visitorNode2Type,
                                 typeof (TResult));

            // find the visit method in the closed generic type of the visitor
            MethodInfo visitMethod = visitorClosedType.GetMethod("Visit");

            // prepare the visit action and dispatcher it
            return (someDirector, someNode1, someNode2) =>
                   (TResult)InvokeUtil.InvokeWithUnwrapper(visitMethod, visitorInstance, new object[] { someDirector, someNode1, someNode2 });
        }
    }
}