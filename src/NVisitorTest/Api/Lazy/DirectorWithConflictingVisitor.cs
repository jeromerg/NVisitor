using Moq;
using NUnit.Framework;
using NVisitor.Api;
using NVisitor.Api.Lazy;

namespace NVisitorTest.Api.Lazy
{
    [TestFixture]
    public class LazyDirectorWithConflictingVisitor
    {
        public interface INode
        {
        }

        public interface INode1 : INode
        {
        }

        public interface INode2 : INode
        {
        }

        public class MyNode : INode1, INode2
        {
        }

        public interface IConflictingVisitor
            : ILazyVisitor<INode, MyDir, INode>,
              ILazyVisitor<INode, MyDir, INode1>,
              ILazyVisitor<INode, MyDir, INode2>
        {
        }

        public interface ISolvingVisitor
            : ILazyVisitor<INode, MyDir, MyNode>
        {
        }

        public class MyDir : LazyDirector<INode, MyDir>
        {
            public MyDir(params ILazyVisitorClass<INode, MyDir>[] visitors)
                : base(visitors)
            {
            }
        }

        [Test]
        public void TestWithAdditionalSolvingVisitor()
        {
            var mockConflictingVisitor = new Mock<IConflictingVisitor>();
            var mockSolvingVisitor = new Mock<ISolvingVisitor>();
            var dir = new MyDir(mockConflictingVisitor.Object, mockSolvingVisitor.Object);

            INode node = new MyNode();
            dir.Visit(node);

            mockConflictingVisitor.Verify(v => v.Visit(dir, It.IsAny<INode>()), Times.Never);
            mockConflictingVisitor.Verify(v => v.Visit(dir, It.IsAny<INode1>()), Times.Never);
            mockConflictingVisitor.Verify(v => v.Visit(dir, It.IsAny<INode2>()), Times.Never);
            mockSolvingVisitor.Verify(v => v.Visit(dir, It.Is<MyNode>(n => n == node)), Times.Once);
        }

        [Test]
        [ExpectedException(typeof (VisitorNotFoundException))]
        public void TestWithConflictingVisitor()
        {
            var mockConflictingVisitor = new Mock<IConflictingVisitor>();
            var dir = new MyDir(mockConflictingVisitor.Object);

            INode node = new MyNode();
            dir.Visit(node);
        }
    }
}