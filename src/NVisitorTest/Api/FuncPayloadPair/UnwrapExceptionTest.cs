using System;
using NUnit.Framework;
using NVisitor.Api.ActionPayloadPair;
using NVisitor.Api.FuncPayloadPair;

namespace NVisitorTest.Api.FuncPayloadPair
{
    [TestFixture]
    public class UnwrapExceptionTest
    {
        public class Node
        {
        }

        public class MyVisitor
            : IFuncPayloadPairVisitor<Node, Node, MyDir, Node, Node, string, string>
        {
            public string Visit(MyDir director, Node node1, Node node2, string pay)
            {
                throw new NotImplementedException();
            }
        }


        public class MyDir : FuncPayloadPairDirector<Node, Node, MyDir, string, string>
        {
            public MyDir(params IFuncPayloadPairVisitorClass<Node, Node, MyDir, string, string>[] visitorArray)
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