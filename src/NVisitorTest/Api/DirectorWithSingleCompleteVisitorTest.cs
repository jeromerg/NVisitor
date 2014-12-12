using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using NVisitor.Api;
using NVisitor.Api.Marker;

namespace NVisitorTest.Api
{
    [TestFixture]
    public class DirectorWithSingleCompleteVisitorTest
    {
        public interface IMyNode {}
        public class MyNodeO : IMyNode { }
        public class MyNodeA : IMyNode { }
        public class MyNodeB : MyNodeA {}

        public interface IMyVisitor 
            : IVisitor<MyDir, IMyNode, IMyNode>
            , IVisitor<MyDir, IMyNode, MyNodeO>
            , IVisitor<MyDir, IMyNode, MyNodeA>
            , IVisitor<MyDir, IMyNode, MyNodeB>
        {
        }

        public class MyDir : Director<MyDir, IMyNode>
        {
            public MyDir(IEnumerable<IVisitor<MyDir, IMyNode>> visitors) 
                : base(visitors) { }
        }

        [Test]
        public void TestNodeO()
        {
            var mock = new Mock<IMyVisitor>();
            var dir = new MyDir(new[] { mock.Object });

            IMyNode node = new MyNodeO();
            dir.Visit(node);

            mock.Verify(v => v.Visit(dir, It.Is<MyNodeO>(n => n == node)), Times.Once);
            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeA>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeB>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.IsAny<IMyNode>()), Times.Never);
        }

        [Test]
        public void TestNodeA()
        {
            var mock = new Mock<IMyVisitor>();
            var dir = new MyDir(new[] { mock.Object });

            IMyNode node = new MyNodeA();
            dir.Visit(node);

            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeO>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.Is<MyNodeA>(n => n == node)), Times.Once);
            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeB>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.IsAny<IMyNode>()), Times.Never);
        }

        [Test]
        public void TestNodeB()
        {
            var mock = new Mock<IMyVisitor>();
            var dir = new MyDir(new[] { mock.Object });

            IMyNode node = new MyNodeB();
            dir.Visit(node);

            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeO>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeA>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.Is<MyNodeB>(n => n == node)), Times.Once);
            mock.Verify(v => v.Visit(dir, It.IsAny<IMyNode>()), Times.Never);
        }

        [Test]
        public void TestNodeForeignNode()
        {
            var mock = new Mock<IMyVisitor>();
            var dir = new MyDir(new[] { mock.Object });

            IMyNode node = new Mock<IMyNode>().Object;
            dir.Visit(node);

            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeO>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeA>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeB>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.Is<IMyNode>(n => n == node)), Times.Once);
        }
    }
}
