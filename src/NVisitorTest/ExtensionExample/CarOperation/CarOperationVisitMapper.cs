using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NVisitor.Api.Common;

namespace NVisitorTest.ExtensionExample.CarOperation
{
    public class CarOperationVisitMapper
        : PairVisitMapperBase<
              IOperation,
              ICar,
              ICarOperationVisitorClass,
              Func<ICarOperationDirector, IOperation, ICar, object>>,
          ICarOperationVisitMapper
    {
        public CarOperationVisitMapper(IEnumerable<ICarOperationVisitorClass> visitors)
            : base(visitors, typeof (ICarOperationImp<,,>), 0, 1)
        {
        }

        protected override Func<ICarOperationDirector, IOperation, ICar, object>
            CreateVisitDelegate(
            IOperation operation,
            ICar car,
            Type operationType,
            Type carType,
            ICarOperationVisitorClass visitorInstance,
            Func<ICarOperationDirector, IOperation, ICar, object> directorDelegate)
        {
            List<Type> operationOpenTypes = operation
                .GetType()
                .GetInterfaces()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof (IOperation<>))
                .ToList();

            if (operationOpenTypes.Count != 1)
                throw new ArgumentException(string.Format("Type {0} must implement IOperation once and only once",
                                                          operation.GetType().FullName));

            Type resultType = operationOpenTypes[0].GetGenericArguments()[0];

            Type visitorClosedType = typeof (ICarOperationImp<,,>)
                .MakeGenericType(operationType, carType, resultType);

            // find the visit method in the closed generic type of the visitor
            MethodInfo visitMethod = visitorClosedType.GetMethod("Perform");

            // prepare the visit action and dispatcher it
            return (someDirector, operation1, car1) =>
                   visitMethod.Invoke(visitorInstance, new object[] {someDirector, operation1, car1});
        }
    }
}