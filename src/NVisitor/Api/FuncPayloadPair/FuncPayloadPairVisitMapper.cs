using System;
using System.Collections.Generic;
using System.Reflection;
using NVisitor.Api.Common;

namespace NVisitor.Api.FuncPayloadPair
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily1">Node1's family</typeparam>
    /// <typeparam name="TFamily2">Node2's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    /// <typeparam name="TPayload">Payload passed to visitor during the visit</typeparam>
    /// <typeparam name="TResult">Result of the visit</typeparam>
    public class FuncPayloadPairVisitMapper<TFamily1, TFamily2, TDir, TPayload, TResult>
        : PairVisitMapperBase<TFamily1,
              TFamily2,
              IFuncPayloadPairVisitorClass<TFamily1, TFamily2, TDir, TPayload, TResult>,
              Func<IFuncPayloadPairDirector<TFamily1, TFamily2, TDir, TPayload, TResult>,
              TFamily1,
              TFamily2,
              TPayload,
              TResult>>,
          IFuncPayloadPairVisitMapper<TFamily1, TFamily2, TDir, TPayload, TResult>
        where TDir : IFuncPayloadPairDirector<TFamily1, TFamily2, TDir, TPayload, TResult>
    {
        public FuncPayloadPairVisitMapper(
            IEnumerable<IFuncPayloadPairVisitorClass<TFamily1, TFamily2, TDir, TPayload, TResult>> visitors)
            : base(visitors, typeof (IFuncPayloadPairVisitor<,,,,,,>), 3, 4)
        {
        }

        protected override
            Func<IFuncPayloadPairDirector<TFamily1, TFamily2, TDir, TPayload, TResult>, TFamily1, TFamily2, TPayload, TResult>
            CreateVisitDelegate(
            TFamily1 node1,
            TFamily2 node2,
            Type visitorNode1Type,
            Type visitorNode2Type,
            IFuncPayloadPairVisitorClass<TFamily1, TFamily2, TDir, TPayload, TResult> visitorInstance,
            Func<IFuncPayloadPairDirector<TFamily1, TFamily2, TDir, TPayload, TResult>, TFamily1, TFamily2, TPayload, TResult>
                directorDelegate)
        {
            // create the closed generic type of the visitor
            Type visitorClosedType = typeof (
                IFuncPayloadPairVisitor< /*TF1*/, /*TF2*/, /*TDir*/, /*TNod1*/, /*TNod2*/, /*TPay*/, /*TRes*/>)
                .MakeGenericType(typeof (TFamily1),
                                 typeof (TFamily2),
                                 typeof (TDir),
                                 visitorNode1Type,
                                 visitorNode2Type,
                                 typeof (TPayload),
                                 typeof (TResult));

            // find the visit method in the closed generic type of the visitor
            MethodInfo visitMethod = visitorClosedType.GetMethod("Visit");

            // prepare the visit action and dispatcher it
            return (someDirector, someNode1, someNode2, payload) =>
                   (TResult) visitMethod.Invoke(visitorInstance, new object[] {someDirector, someNode1, someNode2, payload});
        }
    }
}