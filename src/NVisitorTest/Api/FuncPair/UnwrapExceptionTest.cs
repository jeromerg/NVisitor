using System;
using NUnit.Framework;
using NVisitor.Api.ActionPair;
using NVisitor.Api.FuncPair;

namespace NVisitorTest.Api.FuncPair
{
    [TestFixture]
    public class UnwrapExceptionTest
    {
        public class Node
        {
        }

        public class MyVisitor
            : IFuncPairVisitor<Node, Node, MyDir, Node, Node, string>
        {
            public string Visit(MyDir director, Node node1, Node node2)
            {
                throw new NotImplementedException();
            }
        }


        public class MyDir : FuncPairDirector<Node, Node, MyDir, string>
        {
            public MyDir(params IFuncPairVisitorClass<Node, Node, MyDir, string>[] visitorArray)
                : base(visitorArray)
            {
            }
        }

        [Test]
        
        
        [ExpectedException(typeof(NotImplementedException))]
        public void TestUnwrapException()
        {
            var dir = new MyDir(new MyVisitor());
            var node = new Node();
            dir.Visit(node, node);
        }
   }
}