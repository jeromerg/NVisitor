using System.Collections.Generic;
using NUnit.Framework;
using NVisitor.Api;
using NVisitor.Api.Lazy;

namespace NVisitorTest.Api.Lazy
{
    [TestFixture]
    public class LazyDirectorWithoutVisitorTest
    {
        public interface INode {}
        public class NodeO : INode { }
        public class NodeA : INode { }
        public class NodeB : NodeA {}
        
        public class Dir { }

        public IEnumerable<INode> TestCaseSource()
        {
            yield return new NodeO();
            yield return new NodeA();
            yield return new NodeB();
        }

        [Test]
        [TestCaseSource("TestCaseSource")]
        [ExpectedException(typeof(VisitorNotFoundException))]
        public void Test(INode node)
        {
            var dir = new LazyDirector<INode, Dir>();
            dir.Visit(node);
        }
    }
}
