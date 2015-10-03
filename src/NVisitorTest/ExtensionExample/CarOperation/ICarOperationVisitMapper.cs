using System;

namespace NVisitorTest.ExtensionExample.CarOperation
{
    public interface ICarOperationVisitMapper
    {
        Func<ICarOperationDirector, IOperation, ICar, object>
            GetVisitDelegate(IOperation operation, ICar car);
    }
}