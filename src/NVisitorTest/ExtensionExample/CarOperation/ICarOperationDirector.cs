using JetBrains.Annotations;

namespace NVisitorTest.ExtensionExample.CarOperation
{
    public interface ICarOperationDirector
    {
        TResult Perform<TResult>([NotNull] IOperation<TResult> operation, [NotNull] ICar car);
    }
}