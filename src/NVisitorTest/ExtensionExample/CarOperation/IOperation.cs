using System.Diagnostics.CodeAnalysis;

namespace NVisitorTest.ExtensionExample.CarOperation
{
    public interface IOperation
    {
    }

    [SuppressMessage("ReSharper", "UnusedTypeParameter")]
    public interface IOperation<TResult> : IOperation
    {
    }
}