using System;
using System.Collections.Generic;
using NVisitor.Api;
using NVisitor.Common.Topo;

namespace NVisitorTest.ExtensionExample.CarOperation
{
    public class CarOperationDirector : ICarOperationDirector
    {
        private readonly ICarOperationVisitMapper mVisitMapper;

        public CarOperationDirector(IEnumerable<ICarOperationVisitorClass> visitorEnumerable)
        {
            mVisitMapper = new CarOperationVisitMapper(visitorEnumerable);
        }

        public CarOperationDirector(ICarOperationVisitMapper visitMapper)
        {
            mVisitMapper = visitMapper;
        }

        public TResult Perform<TResult>(IOperation<TResult> operation, ICar car)
        {
            if (ReferenceEquals(operation, null))
                throw new ArgumentNullException("operation", "The operation passed to the director's Visit method cannot be null");

            if (ReferenceEquals(car, null))
                throw new ArgumentNullException("car");

            Func<ICarOperationDirector, IOperation, ICar, object> visitAction;
            try
            {
                visitAction = mVisitMapper.GetVisitDelegate(operation, car);
            }
            catch (TargetTypeNotResolvedException e)
            {
                throw new VisitorNotFoundException(GetType(), e);
            }
            return (TResult) visitAction(this, operation, car);
        }
    }
}