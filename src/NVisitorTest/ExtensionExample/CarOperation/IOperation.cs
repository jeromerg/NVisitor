namespace NVisitorTest.ExtensionExample.CarOperation
{
    public interface IOperation { }

    public interface IOperation<TResult> : IOperation {}
}