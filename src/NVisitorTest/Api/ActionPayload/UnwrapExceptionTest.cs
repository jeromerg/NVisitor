using System;
using NUnit.Framework;
using NVisitor.Api.Action;
using NVisitor.Api.ActionPayload;

namespace NVisitorTest.Api.ActionPayload
{
    [TestFixture]
    public class UnwrapExceptionTest
    {
        public class Node
        {
        }

        public class MyVisitor
            : IActionPayloadVisitor<Node, MyDir, Node, string>
        {
            public void Visit(MyDir director, Node node, string payl)
            {
                throw new NotImplementedException();
            }
        }


        public class MyDir : ActionPayloadDirector<Node, MyDir, string>
        {
            public MyDir(params IActionPayloadVisitorClass<Node, MyDir, string>[] visitorArray)
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
            dir.Visit(node, "coucou");
        }
   }
}