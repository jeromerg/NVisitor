using System.Collections.Generic;
using NUnit.Framework;
using NVisitor.Api;
using NVisitor.Api.Batch;

namespace NVisitorTest.Api.Batch
{
    [TestFixture]
    public class DirectorWithoutVisitorTest
    {
        public interface INode {}
        public class NodeO : INode { }
        public class NodeA : INode { }
        public class NodeB : NodeA {}

        public class Dir : Director<INode, Dir>
        {
            public Dir() : base(new IVisitorClass<INode, Dir>[] { }) { }
        }


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
            var dir = new Dir();
            dir.Visit(node);
        }
    }
}
