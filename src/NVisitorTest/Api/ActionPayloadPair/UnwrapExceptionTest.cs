using System;
using NUnit.Framework;
using NVisitor.Api.ActionPair;
using NVisitor.Api.ActionPayloadPair;

namespace NVisitorTest.Api.ActionPayloadPair
{
    [TestFixture]
    public class UnwrapExceptionTest
    {
        public class Node
        {
        }

        public class MyVisitor
            : IActionPayloadPairVisitor<Node, Node, MyDir, Node, Node, string>
        {
            public void Visit(MyDir director, Node node1, Node node2, string pay)
            {
                throw new NotImplementedException();
            }
        }


        public class MyDir : ActionPayloadPairDirector<Node, Node, MyDir, string>
        {
            public MyDir(params IActionPayloadPairVisitorClass<Node, Node, MyDir, string>[] visitorArray)
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
            dir.Visit(node, node, "coucou");
        }
   }
}