using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using NUnit.Framework;
using NVisitor.Api.Batch;
using NVisitor.Api.Marker;
using NVisitorTest.Api.Demo.Batch.Extension;

// ReSharper disable once CheckNamespace
namespace NVisitorTest.Api.Demo.Batch
{
    //
    // THIS FILE PRESENTS THE CORE FEATURE INTRODUCED BY NVisitor
    // 

    // ----------------------------------------------------------------------
    // 1. In some Core Assembly, you have a family of two nodes as following:
    // ----------------------------------------------------------------------

    public class NodeFamily : List<NodeFamily> {}
    public class NodeA : NodeFamily { }
    public class NodeB : NodeFamily { }

    // ---------------------------------------------------------------------------------------------
    // 2. In the same Core Assembly, you declare a Dump director and the visitors for the two nodes:
    // ---------------------------------------------------------------------------------------------

    public class DumpDir 
    {
        public StringBuilder StringBuilder = new StringBuilder();
    }

    public class DumpVisitors
        : IVisitor<NodeFamily, DumpDir, NodeA>
        , IVisitor<NodeFamily, DumpDir, NodeB>
    {
        public void Visit(IDirector<NodeFamily, DumpDir> director, NodeA node)
        {
            director.State.StringBuilder.AppendLine("NodeA visited");
            node.ForEach(n => director.Visit(n)); // visit children
        }

        public void Visit(IDirector<NodeFamily, DumpDir> director, NodeB node)
        {
            director.State.StringBuilder.AppendLine("NodeB visited");
            node.ForEach(n => director.Visit(n)); // visit children
        }
    }

    // ---------------------------------------------
    // 3. Now you can use the director as following:
    // ---------------------------------------------

    [TestFixture]
    public class A
    {
        [Test]
        public void InitialNodesAndVisitors()
        {
            var root = new NodeA
            {
                new NodeB { new NodeA(), new NodeB()},
                new NodeA()
            };

            var dumpDirector = new Director<NodeFamily, DumpDir>(new DumpVisitors());
            dumpDirector.State = new DumpDir();

            dumpDirector.Visit(root);

            string dump = dumpDirector.State.StringBuilder.ToString();
            Console.WriteLine(dump);

            // Result in console:
            Assert.AreEqual("NodeA visited\r\n" +
                            "NodeB visited\r\n" +
                            "NodeA visited\r\n" +
                            "NodeB visited\r\n" +
                            "NodeA visited\r\n", dump);

        }
    }

    // ----------------------------------------------------------------
    // 4. Now imagine, you implement an extension with a new node type:
    // ----------------------------------------------------------------

    namespace Extension
    {
        public class NodeC : NodeFamily { }
    }

    // ------------------------------------------------------------------------------
    // 5. With NVisitor, you can upgrade the DumpDir with an additional visitor:
    // ------------------------------------------------------------------------------

    namespace Extension
    {
        public class VisitorForNodeC
            : IVisitor<NodeFamily, DumpDir, NodeC>
        {
            public void Visit(IDirector<NodeFamily, DumpDir> director, NodeC node)
            {
                director.State.StringBuilder.AppendLine("NodeC visited !!!!!!!!");
            }
        }
    }

    // ---------------------------------------------------------------------
    // 6. You can then use it to visit a tree of node containing some NodeC:
    // ---------------------------------------------------------------------

    [TestFixture]
    public class B
    {
        [Test]
        public void VisitWithAdditionalNodeAndVisitorTest()
        {
            var root = new NodeA
            {
                new NodeC(), // NEW!!!
                new NodeB { new NodeA(), new NodeB()},
                new NodeA()
            };

            var dumpDirector = new Director<NodeFamily, DumpDir>(new DumpVisitors(), new VisitorForNodeC());
            dumpDirector.State = new DumpDir(); // NEW!!! Updated injected visitors

            dumpDirector.Visit(root);

            string dump = dumpDirector.State.StringBuilder.ToString();
            Console.WriteLine(dump);

            // Result in console:
            Assert.AreEqual("NodeA visited\r\n" +
                            "NodeC visited !!!!!!!!\r\n" +
                            "NodeB visited\r\n" +
                            "NodeA visited\r\n" +
                            "NodeB visited\r\n" +
                            "NodeA visited\r\n", dump);
        }
    }

    // -------------------------------------------------------------------------------------
    // 7. With an IoC container you don't even have to care about the injection of visitors:
    // -------------------------------------------------------------------------------------

    [TestFixture]
    public class C
    {
        [Test]
        public void VisitWithAdditionalNodeAndVisitorTest()
        {
            var container = CreateContainer();

            // nodes to visit
            var root = new NodeA
            {
                new NodeC(), // NEW!!!
                new NodeB { new NodeA(), new NodeB()},
                new NodeA()
            };

            // !!!!!! Let the container create a new instance of DumpDir with all available related directors:
            var dumpDirector = container.Resolve<IDirector<NodeFamily, DumpDir>>();
            dumpDirector.State = new DumpDir();

            dumpDirector.Visit(root);

            string dump = dumpDirector.State.StringBuilder.ToString();
            Console.WriteLine(dump);

            // Result in console is the same:
            Assert.AreEqual("NodeA visited\r\n" +
                            "NodeC visited !!!!!!!!\r\n" +
                            "NodeB visited\r\n" +
                            "NodeA visited\r\n" +
                            "NodeB visited\r\n" +
                            "NodeA visited\r\n", dump);
        }

        // The IoC container scan the assemblies and injects automatically all found visitors into the Director
        private static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            Assembly assembly = Assembly.GetExecutingAssembly();

            // register visitor's directors. 
            builder.RegisterGeneric(typeof(Director<,>))
                .FindConstructorsWith(t => t.GetConstructors()
                                            .Where(c => c.GetParameters()[0].Name == "visitorArray").ToArray())
                .AsSelf()
                .As(typeof(IDirector<,>))
                .InstancePerDependency(); // Directors are stateful

            // register visitors. 
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => typeof (IVisitorMarker).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .SingleInstance(); // Visitors are stateless

            IContainer container = builder.Build();
            return container;
        }
    }

}
