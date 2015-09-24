using System;
using System.Collections.Generic;
using System.Reflection;
using NVisitor.Api.Common;

namespace NVisitor.Api.ActionPayload
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily">Node's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    /// <typeparam name="TPayload">Payload passed to visitor</typeparam>
    public class ActionPayloadVisitMapper<TFamily, TDir, TPayload>
        : VisitMapperBase
              <TFamily, IActionPayloadVisitorClass<TFamily, TDir, TPayload>,
              Action<IActionPayloadDirector<TFamily, TDir, TPayload>, TFamily, TPayload>>,
          IActionPayloadVisitMapper<TFamily, TDir, TPayload>
        where TDir : IActionPayloadDirector<TFamily, TDir, TPayload>
    {
        public ActionPayloadVisitMapper(IEnumerable<IActionPayloadVisitorClass<TFamily, TDir, TPayload>> visitors)
            : base(visitors, typeof (IActionPayloadVisitor<,,,>), 2)
        {
        }

        protected override Action<IActionPayloadDirector<TFamily, TDir, TPayload>, TFamily, TPayload>
            CreateVisitDelegate(TFamily node,
                                Type visitorNodeType,
                                IActionPayloadVisitorClass<TFamily, TDir, TPayload> visitorInstance,
                                Action<IActionPayloadDirector<TFamily, TDir, TPayload>, TFamily, TPayload> directorDelegate)
        {
            // create the closed generic type of the visitor
            Type visitorClosedType = typeof (IActionPayloadVisitor<,,,>).MakeGenericType(typeof (TFamily),
                                                                                         typeof (TDir),
                                                                                         visitorNodeType,
                                                                                         typeof (TPayload));

            // find the visit method in the closed generic type of the visitor
            MethodInfo visitMethod = visitorClosedType.GetMethod("Visit");

            // prepare the visit action and dispatcher it
            return
                (someDirector, someNode, somePayload) =>
                visitMethod.Invoke(visitorInstance, new object[] {someDirector, someNode, somePayload});
        }
    }
}