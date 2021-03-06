﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NVisitor.Api.Lazy;

// ReSharper disable once CheckNamespace

namespace NVisitorTest.Api.Demo.LazyVisitor
{
    //
    // THIS FILE PRESENTS THE LAZY VISIT FEATURE INTRODUCED BY NVisitor
    // 

    // ---------------------------------------------------------------------------------------------
    // 1. Say, you have a node family following the composite pattern and two node types NodeA and NodeB:
    // ---------------------------------------------------------------------------------------------

    public class NodeFamily : List<NodeFamily>
    {
        public NodeFamily(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }

    public class NodeA : NodeFamily
    {
        public NodeA(string name)
            : base(name)
        {
        }
    }

    public class NodeB : NodeFamily
    {
        public NodeB(string name)
            : base(name)
        {
        }
    }

    // ---------------------------------------------------------------------------------------------
    // 2. You want to iterate along all leaves of a NodeFamily tree, but you are lazy and you don't
    //    want to first collect all the leaves and then process them. No, you want to process each 
    //    leaf as soon as the the visitor found it. So you create for that purpose a LazyDirector
    // ---------------------------------------------------------------------------------------------

    public class FindLeavesDir : LazyDirector<NodeFamily, FindLeavesDir>
    {
        private readonly StringBuilder mLog = new StringBuilder();

        public FindLeavesDir(params ILazyVisitorClass<NodeFamily, FindLeavesDir>[] visitors)
            : base(visitors)
        {
        }

        public NodeFamily CurrentLeaf { get; set; }

        public void Log(string format, params object[] args)
        {
            string line = string.Format(format, args);
            Console.WriteLine(line);
            mLog.AppendLine(line);
        }

        public string GetDumpResult()
        {
            return mLog.ToString();
        }
    }

    public class FindLeavesVisitors
        : ILazyVisitor<NodeFamily, FindLeavesDir, NodeA>,
          ILazyVisitor<NodeFamily, FindLeavesDir, NodeB>
    {
        public IEnumerable<Pause> Visit(FindLeavesDir director, NodeA node)
        {
            director.Log("... visiting node {0} (NodeA's visitor is speaking)", node.Name);

            // if leaf, then pause the visit
            if (node.Count == 0)
            {
                director.CurrentLeaf = node;
                yield return Pause.Now;
            }

            // continue visit: visit children
            foreach (NodeFamily child in node)
                foreach (Pause pause in director.Visit(child))
                    yield return pause;
        }

        public IEnumerable<Pause> Visit(FindLeavesDir director, NodeB node)
        {
            director.Log("... visiting node {0} (NodeB's visitor is speaking)", node.Name);

            if (node.Count == 0)
            {
                director.CurrentLeaf = node;
                yield return Pause.Now;
            }

            foreach (NodeFamily child in node)
                foreach (Pause pause in director.Visit(child))
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
                               new NodeB("4") // leaf
                           },
                           new NodeA("5") // leaf
                       };


            var director = new FindLeavesDir(new FindLeavesVisitors());

            IEnumerable<Pause> visitPauses = director.Visit(root);

            director.Log("Starting processing leaves:");
            // ReSharper disable once UnusedVariable
            foreach (Pause pause in visitPauses)
            {
                director.Log("PROCESSING node " + director.CurrentLeaf.Name);
            }

            // Result in console:
            Assert.AreEqual("Starting processing leaves:\r\n" +
                            "... visiting node 1 (NodeA's visitor is speaking)\r\n" +
                            "... visiting node 2 (NodeB's visitor is speaking)\r\n" +
                            "... visiting node 3 (NodeA's visitor is speaking)\r\n" +
                            "PROCESSING node 3\r\n" +
                            "... visiting node 4 (NodeB's visitor is speaking)\r\n" +
                            "PROCESSING node 4\r\n" +
                            "... visiting node 5 (NodeA's visitor is speaking)\r\n" +
                            "PROCESSING node 5\r\n",
                            director.GetDumpResult());
        }
    }

    // ---------------------------------------------
    // RESULT: As you see, the nodes are processed as soon as their 
    // are encountered during the visit.
    // 
    // With the LazyDirector implementation you have the same benefits 
    // as the default Director implementation (Batch implementation)
    // You can additionally make the processing of the visit-result while the visit occurs.
    // It may be necessary in two cases:
    // - If the visit of the full tree is too expensive in time and you need to start
    //   its processing as soon as possible
    // - If the memory footprint of the collected values during the visit is too large, 
    //   so that you can only process a few collected values at once. It can be combined
    //   with a NodeFamily.Children IEnumerable collection, to load nodes in memory one after the other.
}