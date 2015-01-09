NVisitor
========

Lightweight Framework to develop modular extendable visitors

*NVisitor* enables to define visitors for a class family, let's say a family of *nodes*, without renouncing to extend the class family itself.

To have an overview of the problem solved by *NVisitor*, please look at the blog entry [nvisitor-released](https://jeromerg.github.io/blog/2015/01/06/nvisitor-released/).

Installation
------------

- Clone locally this github repository
- Add a reference to the `NVisitor` library in your custom project
    - Either reference the project `NVisitor` itself in your custom .Net project
    - Or compile the solution `NVisitor.sln` and reference the assembly `NVisitor.dll` in your custom .Net project

Usage
-----

You can have a look at the unit test located in the visual studio solution, in order to get an insight on how to use the library.

Let's say, you have a class family of nodes. Each class in the family implements the `INode` interface. 
Now, you need to implement several algorithms that apply to this class family.

### Declare a director

For each algorithm that you need, you declare a director. A director is a class that: 

- Identifies the algorithm
- Configures the set of visitors, that visit the nodes
- Stores the visit-state while visiting the nodes

Example:
```C# 
class DumpDirector : Director<INode, DumpDirector> {
    
    public DumpDirector(IVisitor<INode, DumpDirector>[] visitors) 
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
    : IVisitor<INode, DumpTxtDirector, NodeA>   // visitor for nodes of type NodeA or subclass
    , IVisitor<INode, DumpTxtDirector, NodeB>   // visitor for nodes of type NodeB or subclass
{        
    void Visit(DumpTxtDirector director, NodeA n) {
        director.StringBuilder.AppendLine("NodeA: " + n); // visit logic for NodeA
    }

    void Visit(DumpTxtDirector director, NodeB n) {
        director.StringBuilder.AppendLine("NodeB: " + n); // visit logic for NodeB
    }
}
```

Or you can implement them in multiple classes:
```C# 
class Visitor1
    : IVisitor<INode, DumpTxtDirector, NodeA>
{        
    void Visit(DumpTxtDirector director, NodeA n) {
        director.StringBuilder.AppendLine("NodeA: " + n); // visit logic for NodeA
    }
}

class Visitor2
    : IVisitor<INode, DumpTxtDirector, NodeB> 
{        
    void Visit(DumpTxtDirector director, NodeB n) {
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
    var director = new DumpTxtDirector(new [] {
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

// add directors to container
builder.RegisterAssemblyTypes(executingAssembly)
    .Where(t => typeof (IDirector).IsAssignableFrom(t))
    .AsClosedTypesOf(typeof (IDirector<>))
    .InstancePerDependency();

// add visitors to container
builder.RegisterAssemblyTypes(executingAssembly)
    .Where(t => typeof (IVisitor).IsAssignableFrom(t))
    .AsClosedTypesOf(typeof (IVisitor<,>));

IContainer container = builder.Build();
```

And now, you can get an new director from the container and visit the nodes:

```C# 
var dumpTxtDirector = container.Resolve<DumpDirector>();
    dumpTxtDirector.Visit(myNode);
Console.WriteLine(director.StringBuilder.ToString());    
```



