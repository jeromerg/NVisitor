using System;
using System.Collections.Generic;
using System.Reflection;
using NVisitor.Api.Common;

namespace NVisitor.Api.PairBatch
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily1">Node1's family</typeparam>
    /// <typeparam name="TFamily2">Node2's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    public class PairVisitMapper<TFamily1, TFamily2, TDir>
        : PairVisitMapperBase<TFamily1, TFamily2, IPairVisitorClass<TFamily1, TFamily2, TDir>, Action<IPairDirector<TFamily1, TFamily2, TDir>, TFamily1, TFamily2>>
        , IPairVisitMapper<TFamily1, TFamily2, TDir>
        where TDir : IPairDirector<TFamily1, TFamily2, TDir>
    {
        public PairVisitMapper(IEnumerable<IPairVisitorClass<TFamily1, TFamily2, TDir>> visitors)
            : base(visitors, typeof (IPairVisitor<,,,,>), 3, 4)
        {
        }

        protected override Action<IPairDirector<TFamily1, TFamily2, TDir>, TFamily1, TFamily2> CreateVisitDelegate(TFamily1 node1, TFamily2 node2, Type visitorNode1Type, Type visitorNode2Type, IPairVisitorClass<TFamily1, TFamily2, TDir> visitorInstance, Action<IPairDirector<TFamily1, TFamily2, TDir>, TFamily1, TFamily2> directorDelegate)
        {
            // create the closed generic type of the visitor
            Type visitorClosedType = typeof (IPairVisitor<,,,,>).MakeGenericType(typeof(TFamily1), typeof(TFamily2), typeof(TDir), visitorNode1Type, visitorNode2Type);

            // find the visit method in the closed generic type of the visitor
            MethodInfo visitMethod = visitorClosedType.GetMethod("Visit");

            // prepare the visit action and dispatcher it
            return (someDirector, someNode1, someNode2) => visitMethod.Invoke(visitorInstance, new object[] { someDirector, someNode1, someNode2 });

        }
    }
}