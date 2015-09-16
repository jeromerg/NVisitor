using NUnit.Framework;
using NVisitor.Api.ActionPayload;

namespace NVisitorTest.Api.ActionPayload
{
    [TestFixture]
    public class ActionPayloadTest
    {
        #region Node family
        public interface INode { }
        public class Node1 : INode { }
        public class Node2 : INode { }
        #endregion

        #region Simplifying interfaces and classes for the Print visitors
        public interface IPrintVisClass : IActionPayloadVisitorClass<INode, PrintDir, int> { }

        public class PrintDir : ActionPayloadDirector<INode, PrintDir, int>
        {
            public string LastPayload;
            public PrintDir(params IPrintVisClass[] visitorEnumerable) : base(visitorEnumerable) { }
        }

        public interface IPrintVis<TNod> : IActionPayloadVisitor<INode, PrintDir, TNod, int>, IPrintVisClass
            where TNod : INode
        { }
        #endregion

        #region Concrete visitors

        public class MyConcreteVisitors
            : IPrintVis<Node1>
            , IPrintVis<Node2>
        {
            public void Visit(PrintDir director, Node1 node, int payload)
            {
                director.LastPayload = "Node1, payload=" + payload;
            }

            public void Visit(PrintDir director, Node2 node, int payload)
            {
                director.LastPayload = "Node2, payload=" + payload;
            }
        }
        #endregion

        [Test]
        public void TestSimpleActionVisit()
        {
            var dir = new PrintDir(new MyConcreteVisitors());

            dir.Visit(new Node1(), 5);
            Assert.AreEqual("Node1, payload=5", dir.LastPayload);

            dir.Visit(new Node2(), 3);
            Assert.AreEqual("Node2, payload=3", dir.LastPayload);
        }
    }
}
