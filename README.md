NVisitor
========

Lightweight Framework to develop modular extendable visitors

*NVisitor* enables to define visitors for a class family, let's say a family of *nodes*, without renouncing to extend the class family itself.

To have an overview of the initial problem solved by *NVisitor*, please look at the blog entry [nvisitor-released](https://jeromerg.github.io/blog/2015/01/06/nvisitor-released/).

UPDATE
*NVisitor* has been extended to support lazy visitors: Lazy visitors can make a break in the middle of the visit, a tee-break or coffee-break for example. During this pause, the calling procedure can process the intermediate state and wake up the visitors after. This optimization is important if you have limited time or memory resources and you can process the intermediate result. The implementation is based on the C# enumerator pattern and `yield` keyword.

Installation
------------

### Via nuGet

Install the following *NuGet* package : [https://www.nuget.org/packages/NVisitor](https://www.nuget.org/packages/NVisitor).
Remark: the NuGet package only contains the `NVisitor.dll` assembly.

### Via GitHub
- Clone locally this github repository
- Add a reference to the `NVisitor` library in your custom project
    - Compile the solution `NVisitor.sln` and then reference the assembly `NVisitor.dll` in your custom .Net project
    - Or reference the project `NVisitor` itself in your custom .Net project (fix the build by removing the link to `GlobalAssemblyInfo.cs`. The assembly `NVisitor.dll` will not contain the version and other metadata provided by the `GlobalAssemblyInfo.cs`)

Usage
-----

You have a perfect starting-point at [/src/NVisitorTest/Api/Demo](https://github.com/jeromerg/NVisitor/tree/master/src/NVisitorTest/Api/Demo). The folder contains demo-tests illustrating the most relevant features introduced by *NVisitor*.

But to have a quick overview, let's say, you have a class family of nodes. Each class in the family implements the `INode` interface. 
Now, you need to implement several algorithms that apply to this class family.

### Declare a director

For each algorithm that you need, you declare a director. A director is a class that: 

- Identifies the algorithm
- Configures the set of visitors, that visit the nodes
- Stores the visit-state while visiting the nodes

Example:
```C# 
class DumpDirector : Director<INode, DumpDirector> {
    
    public DumpDirector(IVisitorClass<INode, DumpDirector>[] visitors) 
        : base(visitors) { }
    
    // visit state 
    public StringBuilder StringBuilder = new StringBuilder();
}
```

### Implement visitor methods

Implement the visitor methods for all types or base types that you want to visit. 

You can implement them in a single class:

```C# 
class VisitorCollection
    : IVisitor<INode, DumpDirector, NodeA>   // visitor for nodes of type NodeA or subclass
    , IVisitor<INode, DumpDirector, NodeB>   // visitor for nodes of type NodeB or subclass
{        
    void Visit(DumpDirector director, NodeA n) {
        director.StringBuilder.AppendLine("NodeA: " + n); // visit logic for NodeA
    }

    void Visit(DumpDirector director, NodeB n) {
        director.StringBuilder.AppendLine("NodeB: " + n); // visit logic for NodeB
    }
}
```

Or you can implement them in multiple classes:
```C# 
class Visitor1
    : IVisitor<INode, DumpDirector, NodeA>
{        
    void Visit(DumpDirector director, NodeA n) {
        director.StringBuilder.AppendLine("NodeA: " + n); // visit logic for NodeA
    }
}

class Visitor2
    : IVisitor<INode, DumpDirector, NodeB> 
{        
    void Visit(DumpDirector director, NodeB n) {
        director.StringBuilder.AppendLine("NodeB: " + n); // visit logic for NodeB
    }
}
```

That's the point why *NVisitor* is so powerful! You can place the visitors into multiple classes at multiple locations, allowing you to extend the class family (`INode` family) and update the algorithms to support the new node types.

Remark: the classes implementing the visitor-methods should remain stateless. During the visit, you must store the visit-state in the director. 

### Configure and visit!

That's it. Now, you can configure and use the director to visit your nodes:

```C# 
void Main() {

    // Create a new director instance
    var director = new DumpDirector(new [] {
        new Visitor1(),  // list here all visitor collections
        new Visitor2()
    });    

    INode myNode = GetNodeFromSomewhere();
    director.Visit(myNode);        
    Console.WriteLine(director.StringBuilder.ToString());    
}
```

NVisitor integrates well with IoC-Container
------------------------------

If you use an IoC-Container, you can let the container find and inject automatically the visitors into the `DumpTxtDirector`'s constructor. 

For example with the IoC-Container *Autofac*, you add the directors and visitors into the container as following:

```C# 
var builder = new ContainerBuilder();
Assembly executingAssembly = Assembly.GetExecutingAssembly();

// register visitor's directors. 
builder.RegisterAssemblyTypes(executingAssembly)
	.Where(t => typeof (IDirectorMarker).IsAssignableFrom(t))
	.AsImplementedInterfaces()
	.InstancePerDependency(); // Directors are stateful

// register visitors. 
builder.RegisterAssemblyTypes(executingAssembly)
	.Where(t => typeof (IVisitorMarker).IsAssignableFrom(t))
	.AsImplementedInterfaces()
	.SingleInstance(); // Visitors are stateless

IContainer container = builder.Build();
```

And now, you can get an new director from the container and visit the nodes:

```C# 
var dumpTxtDirector = container.Resolve<DumpDirector>();
    dumpTxtDirector.Visit(myNode);
Console.WriteLine(director.StringBuilder.ToString());    
```



