using System;
using NUnit.Framework;
using NVisitor.Api.FuncPair;
using NVisitor.Api.FuncPayload;

namespace NVisitorTest.Api.FuncPayload
{
    [TestFixture]
    public class UnwrapExceptionTest
    {
        public class Node
        {
        }

        public class MyVisitor
            : IFuncPayloadVisitor<Node, MyDir, Node, string, string>
        {
            public string Visit(MyDir director, Node node1, string pay)
            {
                throw new NotImplementedException();
            }
        }


        public class MyDir : FuncPayloadDirector<Node, MyDir, String, string>
        {
            public MyDir(params IFuncPayloadVisitorClass<Node, MyDir, string, string>[] visitorArray)
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