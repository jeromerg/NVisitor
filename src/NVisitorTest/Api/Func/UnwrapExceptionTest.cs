using System;
using NUnit.Framework;
using NVisitor.Api.ActionPayloadPair;
using NVisitor.Api.Func;

namespace NVisitorTest.Api.Func
{
    [TestFixture]
    public class UnwrapExceptionTest
    {
        public class Node
        {
        }

        public class MyVisitor
            : IFuncVisitor<Node, MyDir, Node, string>
        {
            public string Visit(MyDir director, Node node1)
            {
                throw new NotImplementedException();
            }
        }


        public class MyDir : FuncDirector<Node, MyDir, string>
        {
            public MyDir(params IFuncVisitorClass<Node, MyDir, string>[] visitorArray)
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