using Moq;
using NUnit.Framework;
using NVisitor.Api;
using NVisitor.Api.Batch;

namespace NVisitorTest.Api.Batch
{
    [TestFixture]
    public class DirectorWithConflictingVisitor
    {
        public interface INode {}
        public interface INode1 : INode { }
        public interface INode2 : INode { }
        public class MyNode : INode1, INode2 { }

        public interface IConflictingVisitor
            : IVisitor<INode, MyDir, INode>
            , IVisitor<INode, MyDir, INode1>
            , IVisitor<INode, MyDir, INode2>
        {
        }

        public interface ISolvingVisitor
            : IVisitor<INode, MyDir, MyNode>
        {
        }

        public class MyDir {}

        [Test]
        [ExpectedException(typeof(VisitorNotFoundException))]
        public void TestWithConflictingVisitor()
        {
            var mockConflictingVisitor = new Mock<IConflictingVisitor>();
            var dir = new Director<INode, MyDir>(mockConflictingVisitor.Object);

            INode node = new MyNode();
            dir.Visit(node);
        }

        [Test]
        public void TestWithAdditionalSolvingVisitor()
        {
            var mockConflictingVisitor = new Mock<IConflictingVisitor>();
            var mockSolvingVisitor = new Mock<ISolvingVisitor>();
            var dir = new Director<INode, MyDir>(mockConflictingVisitor.Object, mockSolvingVisitor.Object);

            INode node = new MyNode();
            dir.Visit(node);

            mockConflictingVisitor.Verify(v => v.Visit(dir, It.IsAny<INode>()), Times.Never);
            mockConflictingVisitor.Verify(v => v.Visit(dir, It.IsAny<INode1>()), Times.Never);
            mockConflictingVisitor.Verify(v => v.Visit(dir, It.IsAny<INode2>()), Times.Never);
            mockSolvingVisitor.Verify(v => v.Visit(dir, It.Is<MyNode>(n => n == node)), Times.Once);
        }

    }
}
