using System;
using System.Collections.Generic;
using System.Reflection;
using NVisitor.Api.Common;

namespace NVisitor.Api.Batch
{
    /// <summary>Caches the visit actions</summary>
    /// <typeparam name="TFamily">The node family</typeparam>
    /// <typeparam name="TDir">The director's type</typeparam>
    public class VisitFactory<TFamily, TDir>
        : VisitFactory<TFamily, IVisitorClass<TDir>, Action<IDirector<TFamily, TDir>, TFamily>>
        , IVisitFactory<TFamily, TDir>
    {
        public VisitFactory(IEnumerable<IVisitorClass<TDir>> visitors)
            : base(visitors, typeof (IVisitor<,,>), 2)
        {
        }

        protected override Action<IDirector<TFamily, TDir>, TFamily> CreateVisitDelegate(TFamily node, 
                                                                                        Type visitorNodeType, 
                                                                                        IVisitorClass<TDir> visitorInstance,
                                                                                        Action<IDirector<TFamily, TDir>, TFamily> directorDelegate)
        {

            // create the closed generic type of the visitor
            Type visitorClosedType = typeof (IVisitor<,,>).MakeGenericType(typeof(TFamily), typeof(TDir), visitorNodeType);

            // find the visit method in the closed generic type of the visitor
            MethodInfo visitMethod = visitorClosedType.GetMethod("Visit");

            // prepare the visit action and cache it
            return (someDirector, someNode) => visitMethod.Invoke(visitorInstance, new object[] { someDirector, someNode });

        }
    }
}