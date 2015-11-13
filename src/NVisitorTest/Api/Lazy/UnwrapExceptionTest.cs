using System;
using System.Collections.Generic;
using NUnit.Framework;
using NVisitor.Api.Func;
using NVisitor.Api.Lazy;

namespace NVisitorTest.Api.Lazy
{
    [TestFixture]
    public class UnwrapExceptionTest
    {
        public class Node
        {
        }

        public class MyVisitor
            : ILazyVisitor<Node, MyDir, Node>
        {
            public IEnumerable<Pause> Visit(MyDir director, Node node)
            {
                throw new NotImplementedException();
            }
        }


        public class MyDir : LazyDirector<Node, MyDir>
        {
            public MyDir(params ILazyVisitorClass<Node, MyDir>[] visitorArray)
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