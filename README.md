Status:

[![Build status](https://ci.appveyor.com/api/projects/status/tar7i4r9wbj1s36d/branch/master?svg=true)](https://ci.appveyor.com/project/jeromerg/nvisitor/branch/master)  [![NuGet](https://img.shields.io/nuget/dt/NVisitor.svg)](https://www.nuget.org/packages/NVisitor/)

NVisitor
========

Framework to develop modular extendable visitors

With NVisitor, you
- Define visitors for a class family without renouncing to extend the family itself
- Can pause the visit to process the intermediate result
- Implement visitors without instrumentalizing the visited class family

NVisitor is entirely type-safe even if it performs lookup by reflection to perform visitor dispatching.

See the initial problem solved by *NVisitor*: [nvisitor-released](https://jeromerg.github.io/blog/2015/01/06/nvisitor-released/).

NVisitor is at the core of the test case generator [NCase](https://github.com/jeromerg/NCase). 

Supported type of visitors
--------------------------

Name                            | Visit Signature
--------------------------------|-------------------
ActionVisitor					| `void Visit(Node node)`
FuncVisitor						| `TResult Visit(Node node)`
FuncPayloadVisitor				| `TResult Visit(Node node, TPayload payload)`
ActionPairVisitor				| `void Visit(Car c, Driver d)`
FuncPairVisitor					| `TResult Visit(Car c, Driver d)`
FuncPayloadPairVisitor			| `TResult Visit(Car c, Driver d, TPayload payload)`
LazyVisitor						| `IEnumerable<Pause> Visit(Node node)`


Installation
------------

### NuGet

Install package : [https://www.nuget.org/packages/NVisitor](https://www.nuget.org/packages/NVisitor).

### GitHub

- Clone locally this github repository
- Build the `NVisitor.sln` solution

Usage
-----

See demo-unit-tests: 

- Action visitor: [http://github.com/jeromerg/NVisitor/blob/master/src/NVisitorTest/Api/Demo/ActionVisitorDemo.cs](http://github.com/jeromerg/NVisitor/blob/master/src/NVisitorTest/Api/Demo/ActionVisitorDemo.cs)

- Lazy visitor: [http://github.com/jeromerg/NVisitor/blob/master/src/NVisitorTest/Api/Demo/LazyVisitorDemo.cs](http://github.com/jeromerg/NVisitor/blob/master/src/NVisitorTest/Api/Demo/LazyVisitorDemo.cs)

Extend it!
----------

The core of *NVisitor* is the `VisitFactory`, which resolves the visitor, creates the visit-delegates and caches them. It has been developed with focus on extendability and re-usability. You can easily re-use it to implement alternative visitors, i.e. passing arguments or returning collections...
