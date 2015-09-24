using System;
using System.Collections.Generic;
using System.Reflection;
using NVisitor.Api.Common;

namespace NVisitor.Api.ActionPayloadPair
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily1">Node1's family</typeparam>
    /// <typeparam name="TFamily2">Node2's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    /// <typeparam name="TPayload">Payload passed to visitor</typeparam>
    public class ActionPayloadPairVisitMapper<TFamily1, TFamily2, TDir, TPayload>
        : PairVisitMapperBase<TFamily1,
              TFamily2,
              IActionPayloadPairVisitorClass<TFamily1, TFamily2, TDir, TPayload>,
              Action<IActionPayloadPairDirector<TFamily1, TFamily2, TDir, TPayload>,
              TFamily1, TFamily2, TPayload>>,
          IActionPayloadPairVisitMapper<TFamily1, TFamily2, TDir, TPayload>
        where TDir : IActionPayloadPairDirector<TFamily1, TFamily2, TDir, TPayload>
    {
        public ActionPayloadPairVisitMapper(
            IEnumerable<IActionPayloadPairVisitorClass<TFamily1, TFamily2, TDir, TPayload>> visitors)
            : base(visitors, typeof (IActionPayloadPairVisitor<,,,,,>), 3, 4)
        {
        }

        protected override Action<IActionPayloadPairDirector<TFamily1, TFamily2, TDir, TPayload>, TFamily1, TFamily2, TPayload>
            CreateVisitDelegate(
            TFamily1 node1,
            TFamily2 node2,
            Type visitorNode1Type,
            Type visitorNode2Type,
            IActionPayloadPairVisitorClass<TFamily1, TFamily2, TDir, TPayload> visitorInstance,
            Action<IActionPayloadPairDirector<TFamily1, TFamily2, TDir, TPayload>, TFamily1, TFamily2, TPayload> directorDelegate)
        {
            // create the closed generic type of the visitor
            Type visitorClosedType = typeof (
                IActionPayloadPairVisitor< /*TF1*/, /*TF2*/, /*TDir*/, /*TNod1*/, /*TNod2*/, /*TPay*/>)
                .MakeGenericType(typeof (TFamily1),
                                 typeof (TFamily2),
                                 typeof (TDir),
                                 visitorNode1Type,
                                 visitorNode2Type,
                                 typeof (TPayload));

            // find the visit method in the closed generic type of the visitor
            MethodInfo visitMethod = visitorClosedType.GetMethod("Visit");

            // prepare the visit action and dispatcher it
            return (someDirector, someNode1, someNode2, payload) =>
                   visitMethod.Invoke(visitorInstance, new object[] {someDirector, someNode1, someNode2, payload});
        }
    }
}