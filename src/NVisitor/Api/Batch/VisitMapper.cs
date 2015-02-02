using System;
using System.Collections.Generic;
using System.Reflection;
using NVisitor.Api.Common;

namespace NVisitor.Api.Batch
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily">Node's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    public class VisitMapper<TFamily, TDir>
        : VisitMapperBase<TFamily, IVisitorClass<TFamily, TDir>, Action<IDirector<TFamily, TDir>, TFamily>>
        , IVisitMapper<TFamily, TDir> 
        where TDir : IDirector<TFamily, TDir>
    {
        public VisitMapper(IEnumerable<IVisitorClass<TFamily, TDir>> visitors)
            : base(visitors, typeof (IVisitor<,,>), 2)
        {
        }

        protected override Action<IDirector<TFamily, TDir>, TFamily> 
            CreateVisitDelegate(TFamily node, 
                                Type visitorNodeType, 
                                IVisitorClass<TFamily, TDir> visitorInstance, 
                                Action<IDirector<TFamily, TDir>, TFamily> directorDelegate)
        {
            // create the closed generic type of the visitor
            Type visitorClosedType = typeof (IVisitor<,,>).MakeGenericType(typeof(TFamily), typeof(TDir), visitorNodeType);

            // find the visit method in the closed generic type of the visitor
            MethodInfo visitMethod = visitorClosedType.GetMethod("Visit");

            // prepare the visit action and dispatcher it
            return (someDirector, someNode) => visitMethod.Invoke(visitorInstance, new object[] { someDirector, someNode });

        }
    }
}