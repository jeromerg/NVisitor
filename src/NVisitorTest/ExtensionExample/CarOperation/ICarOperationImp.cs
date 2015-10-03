using System.Diagnostics.CodeAnalysis;

namespace NVisitorTest.ExtensionExample.CarOperation
{
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")]
    public interface ICarOperationImp<TOperation, TCar, TResult>
        : ICarOperationVisitorClass
        where TOperation : IOperation<TResult>
        where TCar : ICar
    {
        TResult Perform(ICarOperationDirector director, TOperation operation, TCar car);
    }
}