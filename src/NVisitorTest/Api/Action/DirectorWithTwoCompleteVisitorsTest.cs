using Moq;
using NUnit.Framework;
using NVisitor.Api.Action;

namespace NVisitorTest.Api.Action
{
    [TestFixture]
    public class DirectorWithTwoCompleteVisitorsTest
    {
        public interface INode
        {
        }

        public class MyNodeO : INode
        {
        }

        public class MyNodeA : INode
        {
        }

        public class MyNodeB : MyNodeA
        {
        }

        public interface IMyVisitor1
            : IActionVisitor<INode, MyDir, INode>
              ,
              IActionVisitor<INode, MyDir, MyNodeO>
        {
        }

        public interface IMyVisitor2
            : IActionVisitor<INode, MyDir, MyNodeA>
              ,
              IActionVisitor<INode, MyDir, MyNodeB>
        {
        }

        public class MyDir : ActionDirector<INode, MyDir>
        {
            public MyDir(params IActionVisitorClass<INode, MyDir>[] visitorArray)
                : base(visitorArray)
            {
            }
        }

        [Test]
        public void TestNodeA()
        {
            var mock1 = new Mock<IMyVisitor1>();
            var mock2 = new Mock<IMyVisitor2>();
            var dir = new MyDir(mock1.Object, mock2.Object);

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
            var dir = new MyDir(mock1.Object, mock2.Object);

            INode node = new MyNodeB();
            dir.Visit(node);

            mock1.Verify(v => v.Visit(dir, It.IsAny<MyNodeO>()), Times.Never);
            mock1.Verify(v => v.Visit(dir, It.IsAny<INode>()), Times.Never);

            mock2.Verify(v => v.Visit(dir, It.IsAny<MyNodeA>()), Times.Never);
            mock2.Verify(v => v.Visit(dir, It.Is<MyNodeB>(n => n == node)), Times.Once);
        }

        [Test]
        public void TestNodeForeignNode()
        {
            var mock1 = new Mock<IMyVisitor1>();
            var mock2 = new Mock<IMyVisitor2>();
            var dir = new MyDir(mock1.Object, mock2.Object);

            INode node = new Mock<INode>().Object;
            dir.Visit(node);

            mock1.Verify(v => v.Visit(dir, It.IsAny<MyNodeO>()), Times.Never);
            mock1.Verify(v => v.Visit(dir, It.Is<INode>(n => n == node)), Times.Once);

            mock2.Verify(v => v.Visit(dir, It.IsAny<MyNodeA>()), Times.Never);
            mock2.Verify(v => v.Visit(dir, It.IsAny<MyNodeB>()), Times.Never);
        }

        [Test]
        public void TestNodeO()
        {
            var mock1 = new Mock<IMyVisitor1>();
            var mock2 = new Mock<IMyVisitor2>();
            var dir = new MyDir(mock1.Object, mock2.Object);

            INode node = new MyNodeO();
            dir.Visit(node);

            mock1.Verify(v => v.Visit(dir, It.Is<MyNodeO>(n => n == node)), Times.Once);
            mock1.Verify(v => v.Visit(dir, It.IsAny<INode>()), Times.Never);

            mock2.Verify(v => v.Visit(dir, It.IsAny<MyNodeA>()), Times.Never);
            mock2.Verify(v => v.Visit(dir, It.IsAny<MyNodeB>()), Times.Never);
        }
    }
}