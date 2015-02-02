using Moq;
using NUnit.Framework;
using NVisitor.Api.Lazy;

namespace NVisitorTest.Api.Lazy
{
    [TestFixture]
    public class LazyDirectorWithSingleCompleteVisitorTest
    {
        public interface INode {}
        public class MyNodeO : INode { }
        public class MyNodeA : INode { }
        public class MyNodeB : MyNodeA {}

        public interface IMyVisitor
            : ILazyVisitor<INode, MyDir, INode>
            , ILazyVisitor<INode, MyDir, MyNodeO>
            , ILazyVisitor<INode, MyDir, MyNodeA>
            , ILazyVisitor<INode, MyDir, MyNodeB>
        {
        }

        public class MyDir : LazyDirector<INode, MyDir>
        {
            public MyDir(params ILazyVisitorClass<INode, MyDir>[] visitors) : base(visitors)
            {
            }
        }

        [Test]
        public void TestNodeO()
        {
            var mock = new Mock<IMyVisitor>();
            var dir = new MyDir(mock.Object);

            INode node = new MyNodeO();
            dir.Visit(node);

            mock.Verify(v => v.Visit(dir, It.Is<MyNodeO>(n => n == node)), Times.Once);
            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeA>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeB>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.IsAny<INode>()), Times.Never);
        }

        [Test]
        public void TestNodeA()
        {
            var mock = new Mock<IMyVisitor>();
            var dir = new MyDir(mock.Object);

            INode node = new MyNodeA();
            dir.Visit(node);

            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeO>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.Is<MyNodeA>(n => n == node)), Times.Once);
            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeB>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.IsAny<INode>()), Times.Never);
        }

        [Test]
        public void TestNodeB()
        {
            var mock = new Mock<IMyVisitor>();
            var dir = new MyDir(mock.Object);

            INode node = new MyNodeB();
            dir.Visit(node);

            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeO>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeA>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.Is<MyNodeB>(n => n == node)), Times.Once);
            mock.Verify(v => v.Visit(dir, It.IsAny<INode>()), Times.Never);
        }

        [Test]
        public void TestNodeForeignNode()
        {
            var mock = new Mock<IMyVisitor>();
            var dir = new MyDir(mock.Object);

            INode node = new Mock<INode>().Object;
            dir.Visit(node);

            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeO>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeA>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeB>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.Is<INode>(n => n == node)), Times.Once);
        }
    }
}
