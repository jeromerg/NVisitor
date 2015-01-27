using Moq;
using NUnit.Framework;
using NVisitor.Api.Batch;

namespace NVisitorTest.Api.Batch
{
    [TestFixture]
    public class DirectorWithTwoCompleteVisitorsTest
    {
        public interface INode { }
        public class MyNodeO : INode { }
        public class MyNodeA : INode { }
        public class MyNodeB : MyNodeA { }

        public interface IMyVisitor1
            : IVisitor<INode, MyDir, INode>
            , IVisitor<INode, MyDir, MyNodeO>
        {
        }

        public interface IMyVisitor2
            : IVisitor<INode, MyDir, MyNodeA>
            , IVisitor<INode, MyDir, MyNodeB>
        {
        }

        public class MyDir {}

        [Test]
        public void TestNodeO()
        {
            var mock1 = new Mock<IMyVisitor1>();
            var mock2 = new Mock<IMyVisitor2>();
            var dir = new Director<INode, MyDir>(mock1.Object, mock2.Object);

            INode node = new MyNodeO();
            dir.Visit(node);

            mock1.Verify(v => v.Visit(dir, It.Is<MyNodeO>(n => n == node)), Times.Once);
            mock1.Verify(v => v.Visit(dir, It.IsAny<INode>()), Times.Never);
            
            mock2.Verify(v => v.Visit(dir, It.IsAny<MyNodeA>()), Times.Never);
            mock2.Verify(v => v.Visit(dir, It.IsAny<MyNodeB>()), Times.Never);
        }

        [Test]
        public void TestNodeForeignNode()
        {
            var mock1 = new Mock<IMyVisitor1>();
            var mock2 = new Mock<IMyVisitor2>();
            var dir = new Director<INode, MyDir>(mock1.Object, mock2.Object);

            INode node = new Mock<INode>().Object;
            dir.Visit(node);

            mock1.Verify(v => v.Visit(dir, It.IsAny<MyNodeO>()), Times.Never);
            mock1.Verify(v => v.Visit(dir, It.Is<INode>(n => n == node)), Times.Once);
            
            mock2.Verify(v => v.Visit(dir, It.IsAny<MyNodeA>()), Times.Never);
            mock2.Verify(v => v.Visit(dir, It.IsAny<MyNodeB>()), Times.Never);
        }

        [Test]
        public void TestNodeA()
        {
            var mock1 = new Mock<IMyVisitor1>();
            var mock2 = new Mock<IMyVisitor2>();
            var dir = new Director<INode, MyDir>(mock1.Object, mock2.Object);

            INode node = new MyNodeA();
            dir.Visit(node);

            mock1.Verify(v => v.Visit(dir, It.IsAny<MyNodeO>()), Times.Never);
            mock1.Verify(v => v.Visit(dir, It.IsAny<INode>()), Times.Never);
            
            mock2.Verify(v => v.Visit(dir, It.Is<MyNodeA>(n => n == node)), Times.Once);
            mock2.Verify(v => v.Visit(dir, It.IsAny<MyNodeB>()), Times.Never);
        }

        [Test]
        public void TestNodeB()
        {
            var mock1 = new Mock<IMyVisitor1>();
            var mock2 = new Mock<IMyVisitor2>();
            var dir = new Director<INode, MyDir>(mock1.Object, mock2.Object);

            INode node = new MyNodeB();
            dir.Visit(node);

            mock1.Verify(v => v.Visit(dir, It.IsAny<MyNodeO>()), Times.Never);
            mock1.Verify(v => v.Visit(dir, It.IsAny<INode>()), Times.Never);
            
            mock2.Verify(v => v.Visit(dir, It.IsAny<MyNodeA>()), Times.Never);
            mock2.Verify(v => v.Visit(dir, It.Is<MyNodeB>(n => n == node)), Times.Once);
        }
    }
}
