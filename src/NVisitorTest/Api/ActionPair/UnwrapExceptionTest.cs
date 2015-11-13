using System;
using NUnit.Framework;
using NVisitor.Api.Action;
using NVisitor.Api.ActionPair;

namespace NVisitorTest.Api.ActionPair
{
    [TestFixture]
    public class UnwrapExceptionTest
    {
        public class Node
        {
        }

        public class MyVisitor
            : IActionPairVisitor<Node, Node, MyDir, Node, Node>
        {
            public void Visit(MyDir director, Node node1, Node node2)
            {
                throw new NotImplementedException();
            }
        }


        public class MyDir : ActionPairDirector<Node, Node, MyDir>
        {
            public MyDir(params IActionPairVisitorClass<Node, Node, MyDir>[] visitorArray)
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