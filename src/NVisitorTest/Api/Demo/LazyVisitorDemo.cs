using System;
using System.Collections.Generic;
using NUnit.Framework;
using NVisitor.Api.Lazy;

// ReSharper disable once CheckNamespace
namespace NVisitorTest.Api.Demo.LazyVisitor
{
    //
    // THIS FILE PRESENTS THE LAZY FEATURE INTRODUCED BY NVisitor
    // 

    // ----------------------------------------------------------------------
    // 1. You have a family of two nodes as following:
    // ----------------------------------------------------------------------

    public class NodeFamily : List<NodeFamily>
    {
        public string Name { get; private set; }

        public NodeFamily(string name)
        {
            Name = name;
        }
    }
    public class NodeA : NodeFamily { public NodeA(string name) : base(name) {}}
    public class NodeB : NodeFamily { public NodeB(string name) : base(name) {}}

    // ---------------------------------------------------------------------------------------------
    // 2. You want to iterate along all leaves of a NodeFamily tree, but you are lazy and you don't
    //    want to first collect all the leaves and then process them. No, you want to process each 
    //    leaf as soon as the the visitor found it. So you create for that purpose a LazyDirector
    // ---------------------------------------------------------------------------------------------

    public class FindLeavesDirector : LazyDirector<NodeFamily, FindLeavesDirector>
    {
        public FindLeavesDirector(params ILazyVisitorClass<NodeFamily, FindLeavesDirector>[] visitors)
            : base(visitors)
        {
        }

        // state
        public NodeFamily CurrentLeaf { get; set; }
    }

    public class FindLeavesVisitors
        : ILazyVisitor<NodeFamily, FindLeavesDirector, NodeA>
        , ILazyVisitor<NodeFamily, FindLeavesDirector, NodeB>
    {
        public IEnumerable<Pause> Visit(FindLeavesDirector director, NodeA node)
        {
            Console.WriteLine("... visiting node {0} (NodeA's visitor is speaking)", node.Name);

            // if leaf, then pause the visit
            if (node.Count == 0)
            {
                director.CurrentLeaf = node;
                yield return Pause.Now;
            }

            // continue visit: visit children
            foreach (var child in node)
                foreach (var pause in director.Visit(child))
                    yield return pause;
        }

        public IEnumerable<Pause> Visit(FindLeavesDirector director, NodeB node)
        {
            Console.WriteLine("... visiting node {0} (NodeB's visitor is speaking)", node.Name);
 
            if (node.Count == 0)
            {
                director.CurrentLeaf = node;
                yield return Pause.Now;
            }

            foreach (var child in node)
                foreach (var pause in director.Visit(child))
                    yield return pause;
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
            var root = new NodeA("1")
            {
                new NodeB("2")
                {
                    new NodeA("3"), // leaf
                    new NodeB("4"), // leaf
                },
                new NodeA("5")      // leaf
            };

            var director = new FindLeavesDirector(new FindLeavesVisitors());

            IEnumerable<Pause> pauses = director.Visit(root);

            Console.WriteLine("Starting processing leaves:");
            foreach (var pause in pauses)
            {
                Console.WriteLine("PROCESSING node " + director.CurrentLeaf.Name);
            }

            // Result in console:
            // > Starting processing leaves:
            // > ... visiting node 1 (NodeA's visitor is speaking)
            // > ... visiting node 2 (NodeB's visitor is speaking)
            // > ... visiting node 3 (NodeA's visitor is speaking)
            // > PROCESSING node 3
            // > ... visiting node 4 (NodeB's visitor is speaking)
            // > PROCESSING node 4
            // > ... visiting node 5 (NodeA's visitor is speaking)
            // > PROCESSING node 5
        }
    }

    // ---------------------------------------------
    // RESULT: as you see, the nodes are processed as soon as their 
    // are encountered during the visit!! 
    // 
    // With the LazyDirector implementation you have the same benefits 
    // as the default Director implementation (Batch implementation)
    // but you also pause the visit of the tree to process the collected values so far. 
    // It may be necessary in two cases:
    // - If the visit of the full tree is too expensive in time and you need to start
    //   its processing as soon as possible
    // - If the memory footprint of the collected values during the visit is too big, 
    //   so that you can only process a few collected values at once. It can be combined
    //   with a NodeFamily.Children IEnumerable collection, which avoid loading all nodes in memory!!!

}
