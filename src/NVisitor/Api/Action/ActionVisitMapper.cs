using System;
using System.Collections.Generic;
using System.Reflection;
using NVisitor.Api.Common;

namespace NVisitor.Api.Action
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily">Node's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    public class ActionVisitMapper<TFamily, TDir>
        : VisitMapperBase<TFamily, IActionVisitorClass<TFamily, TDir>, Action<IActionDirector<TFamily, TDir>, TFamily>>
        , IActionVisitMapper<TFamily, TDir> 
        where TDir : IActionDirector<TFamily, TDir>
    {
        public ActionVisitMapper(IEnumerable<IActionVisitorClass<TFamily, TDir>> visitors)
            : base(visitors, typeof (IActionVisitor<,,>), 2)
        {
        }

        protected override Action<IActionDirector<TFamily, TDir>, TFamily> 
            CreateVisitDelegate(TFamily node, 
                                Type visitorNodeType, 
                                IActionVisitorClass<TFamily, TDir> visitorInstance, 
                                Action<IActionDirector<TFamily, TDir>, TFamily> directorDelegate)
        {
            // create the closed generic type of the visitor
            Type visitorClosedType = typeof (IActionVisitor<,,>).MakeGenericType(typeof(TFamily), typeof(TDir), visitorNodeType);

            // find the visit method in the closed generic type of the visitor
            MethodInfo visitMethod = visitorClosedType.GetMethod("Visit");

            // prepare the visit action and dispatcher it
            return (someDirector, someNode) => visitMethod.Invoke(visitorInstance, new object[] { someDirector, someNode });

        }
    }
}