using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NVisitor.Api.Marker;
using NVisitor.Util.Quality;
using NVisitor.Util.Topo;

namespace NVisitor.Api
{
    public abstract class Director<TFamily, TDirector> : IDirector<TFamily>
        where TDirector : IDirector<TFamily>
    {

        private readonly Dictionary<Type, IVisitor<TFamily, TDirector>> mVisitorsByNodeType
            = new Dictionary<Type, IVisitor<TFamily, TDirector>>();

        private readonly Dictionary<Type, Action<TFamily>> mVisitCacheByNodeType = new Dictionary<Type, Action<TFamily>>();

        protected Director(IEnumerable<IVisitor<TFamily, TDirector>> visitors)
        {
            foreach (var visitor in visitors)
            {
                IEnumerable<Type> implementedVisitorTypes 
                    = visitor.GetType().GetInterfaces()
                                       .Where(interfaceType => interfaceType.IsGenericType
                                                  && interfaceType.GetGenericTypeDefinition() == typeof(IVisitor<,,>));

                foreach (var implementedVisitorType in implementedVisitorTypes)
                {
                    // remark: IVisitor<in TDir, in TNod, in TConcreteNode>

                    Type concreteNodeType = implementedVisitorType.GetGenericArguments()[2];

                    IVisitor<TFamily, TDirector> conflictingVisitor;
                    if (mVisitorsByNodeType.TryGetValue(concreteNodeType, out conflictingVisitor))
                    {
                        string msg = string.Format("Two visitors apply for the same Node type: {0} and {1}",
                            conflictingVisitor.GetType().Name, visitor.GetType().Name);
                        throw new ArgumentException(msg);
                    }

                    mVisitorsByNodeType.Add(concreteNodeType, visitor);
                }
            }
        }

        public virtual void Visit([NotNull] TFamily node)
        {
            if (ReferenceEquals(node, null))
                throw new ArgumentNullException("node");

            Action<TFamily> visitAction;
            if (!mVisitCacheByNodeType.TryGetValue(node.GetType(), out visitAction))
            {
                Type nearestVisitorNodeType = FindNearestVisitorNodeType(node.GetType());

                Type bestVisitorType = typeof(IVisitor<,,>)
                    .MakeGenericType(typeof(TFamily), typeof(TDirector), nearestVisitorNodeType);

                IVisitor<TFamily, TDirector> bestVisitor = mVisitorsByNodeType[nearestVisitorNodeType];
                MethodInfo visitMethod = bestVisitorType.GetMethod("Visit");
                
                visitAction = someNode => visitMethod.Invoke(bestVisitor, new object[] { this, someNode });
            }

            visitAction(node);
        }

        [NotNull]
        private Type FindNearestVisitorNodeType(Type nodeType)
        {
            try
            {
                var typeTopology = new TypeTopology(nodeType);
                return typeTopology
                    .ResolveBestUnambiguousTargetType(new HashSet<Type>(mVisitorsByNodeType.Keys));
            }
            catch (TargetTypeNotResolvedException e)
            {                    
                throw new VisitorNotFoundException(e);
            }
        }
    }
}