Car Operation Example
=====================

Show how the framework can be used to implement more specific kind of visitors.
It derivates from the FuncPair visitor, but contains specific properties:

- The `Visit(...)` method has been renamed into `Perform(...)`
- The output and the first input argument are correlated types: Input type is `ICarOperation<TResult>` enforcing 
  the output type to be `TResult`

