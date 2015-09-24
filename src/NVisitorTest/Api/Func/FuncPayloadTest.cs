using NUnit.Framework;
using NVisitor.Api.Func;

namespace NVisitorTest.Api.Func
{
    [TestFixture]
    public class FuncTest
    {
        public interface INode
        {
        }

        public class Node1 : INode
        {
        }

        public class Node2 : INode
        {
        }

        public interface IPrintVisClass : IFuncVisitorClass<INode, PrintDir, string>
        {
        }

        public class PrintDir : FuncDirector<INode, PrintDir, string>
        {
            public PrintDir(params IPrintVisClass[] visitorEnumerable)
                : base(visitorEnumerable)
            {
            }
        }

        public interface IPrintVis<TNod> : IFuncVisitor<INode, PrintDir, TNod, string>, IPrintVisClass
            where TNod : INode
        {
        }

        public class MyConcreteVisitors
            : IPrintVis<Node1>,
              IPrintVis<Node2>
        {
            public string Visit(PrintDir director, Node1 node)
            {
                return "Node1";
            }

            public string Visit(PrintDir director, Node2 node)
            {
                return "Node2";
            }
        }

        [Test]
        public void TestSimpleFuncVisit()
        {
            var dir = new PrintDir(new MyConcreteVisitors());

            string visit1Result = dir.Visit(new Node1());
            Assert.AreEqual("Node1", visit1Result);

            string visit2Result = dir.Visit(new Node2());
            Assert.AreEqual("Node2", visit2Result);
        }
    }
}