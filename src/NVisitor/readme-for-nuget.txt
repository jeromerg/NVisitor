Congratulation, you have succesfully installed *NVisitor*
=========================================================

Framework to develop modular extendable visitors

With NVisitor, you
- Define visitors for a class family without renouncing to extend the family itself
- Can pause the visit to process the intermediate result
- Implement visitors without instrumentalizing the visited class family

*!! Warning: `IVisitor`, the default batch visitor, has been renamed into `ActionVisitor`.
in order to comply with the other visitors*. The list of available visitors:

Name                            | Visit Signature
--------------------------------|-------------------
ActionVisitor					          | `void Visit(Node node)`
FuncVisitor						          | `TResult Visit(Node node)`
FuncPayloadVisitor				      | `TResult Visit(Node node, TPayload payload)`
ActionPairVisitor				        | `void Visit(Car c, Driver d)`
FuncPairVisitor					        | `TResult Visit(Car c, Driver d)`
FuncPayloadPairVisitor			    | `TResult Visit(Car c, Driver d, TPayload payload)`
LazyVisitor						          | `IEnumerable<Pause> Visit(Node node)`

Usage
-----

Look at demo unit tests:
    http://github.com/jeromerg/NVisitor/blob/master/src/NVisitorTest/Api/Demo/ActionVisitorDemo.cs
    http://github.com/jeromerg/NVisitor/blob/master/src/NVisitorTest/Api/Demo/LazyVisitorDemo.cs


Links
-----

- Blog entry:

    http://jeromerg.github.io/blog/2015/01/06/nvisitor-released/

- GitHub repository:

    http://github.com/jeromerg/NVisitor](http://github.com/jeromerg/NVisitor

- NuGet home page:

    http://www.nuget.org/packages/NVisitor
