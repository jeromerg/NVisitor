using System.Collections.Generic;
using NUnit.Framework;
using NVisitor.Api.FuncBatch;

namespace NVisitorTest.Api.FuncBatch
{
    [TestFixture]
    public class FuncBatchTest
    {
        #region Node family
        public interface INode { }
        public class Node1 : INode { }
        public class Node2 : INode { }
        #endregion

        #region Simplifying interfaces and classes for the Print visitors
        public interface IPrintVisClass : IFuncVisitorClass<INode, PrintDir, int, string> { }

        public class PrintDir : FuncDirector<INode, PrintDir, int, string>
        {
            public PrintDir(params IPrintVisClass[] visitorEnumerable) : base(visitorEnumerable) { }
        }

        public interface IPrintVis<TNod> : IFuncVisitor<INode, PrintDir, TNod, int, string>, IPrintVisClass
            where TNod : INode
        { }
        #endregion

        #region Concrete visitors

        public class MyConcreteVisitors
            : IPrintVis<Node1>
            , IPrintVis<Node2>
        {
            public string Visit(PrintDir director, Node1 node, int payload)
            {
                return "Node1, payload=" + payload;
            }

            public string Visit(PrintDir director, Node2 node, int payload)
            {
                return "Node2, payload=" + payload;
            }
        }
        #endregion

        [Test]
        public void TestSimpleFuncVisit()
        {
            var dir = new PrintDir(new MyConcreteVisitors());

            string visit1Result = dir.Visit(new Node1(), 5);
            Assert.AreEqual("Node1, payload=5", visit1Result);

            string visit2Result = dir.Visit(new Node2(), 3);
            Assert.AreEqual("Node2, payload=3", visit2Result);
        }
    }
}
