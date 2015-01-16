Congratulation, you have succesfully installed *NVisitor*
=========================================================

Lightweight Framework to develop modular extendable visitors

*NVisitor* enables to define visitors for a class family, let's say a family of *nodes*, without renouncing to extend the class family itself.

Next Steps
----------

### Declare a director

Say, you have a class family of nodes and you want to implement algorithms that apply to the family.
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

Usage with an IoC-Container
---------------------------

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

Links
-----

- Blog entry: [http://jeromerg.github.io/blog/2015/01/06/nvisitor-released/](http://jeromerg.github.io/blog/2015/01/06/nvisitor-released/)

- GitHub repository: [http://github.com/jeromerg/NVisitor](http://github.com/jeromerg/NVisitor)

- NuGet home page: [https://www.nuget.org/packages/NVisitor](https://www.nuget.org/packages/NVisitor)

