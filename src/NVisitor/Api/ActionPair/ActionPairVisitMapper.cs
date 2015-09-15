using System;
using System.Collections.Generic;
using System.Reflection;
using NVisitor.Api.Common;

namespace NVisitor.Api.ActionPair
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily1">Node1's family</typeparam>
    /// <typeparam name="TFamily2">Node2's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    public class ActionPairVisitMapper<TFamily1, TFamily2, TDir>
        : PairVisitMapperBase<TFamily1, TFamily2, IActionPairVisitorClass<TFamily1, TFamily2, TDir>, Action<IActionPairDirector<TFamily1, TFamily2, TDir>, TFamily1, TFamily2>>
        , IActionPairVisitMapper<TFamily1, TFamily2, TDir>
        where TDir : IActionPairDirector<TFamily1, TFamily2, TDir>
    {
        public ActionPairVisitMapper(IEnumerable<IActionPairVisitorClass<TFamily1, TFamily2, TDir>> visitors)
            : base(visitors, typeof (IActionPairVisitor<,,,,>), 3, 4)
        {
        }

        protected override Action<IActionPairDirector<TFamily1, TFamily2, TDir>, TFamily1, TFamily2> CreateVisitDelegate(TFamily1 node1, TFamily2 node2, Type visitorNode1Type, Type visitorNode2Type, IActionPairVisitorClass<TFamily1, TFamily2, TDir> visitorInstance, Action<IActionPairDirector<TFamily1, TFamily2, TDir>, TFamily1, TFamily2> directorDelegate)
        {
            // create the closed generic type of the visitor
            Type visitorClosedType = typeof (IActionPairVisitor<,,,,>).MakeGenericType(typeof(TFamily1), typeof(TFamily2), typeof(TDir), visitorNode1Type, visitorNode2Type);

            // find the visit method in the closed generic type of the visitor
            MethodInfo visitMethod = visitorClosedType.GetMethod("Visit");

            // prepare the visit action and dispatcher it
            return (someDirector, someNode1, someNode2) => visitMethod.Invoke(visitorInstance, new object[] { someDirector, someNode1, someNode2 });

        }
    }
}