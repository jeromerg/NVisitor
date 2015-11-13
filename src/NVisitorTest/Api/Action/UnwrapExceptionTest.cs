using System;
using Moq;
using NUnit.Framework;
using NVisitor.Api;
using NVisitor.Api.Action;

namespace NVisitorTest.Api.Action
{
    [TestFixture]
    public class UnwrapExceptionTest
    {
        public class Node
        {
        }

        public class MyVisitor
            : IActionVisitor<Node, MyDir, Node>
        {
            public void Visit(MyDir director, Node node)
            {
                throw new NotImplementedException();
            }
        }


        public class MyDir : ActionDirector<Node, MyDir>
        {
            public MyDir(params IActionVisitorClass<Node, MyDir>[] visitorArray)
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
            dir.Visit(node);
        }
   }
}