using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using NVisitor.Api;
using NVisitor.Api.Marker;

namespace NVisitorTest.Api
{
    [TestFixture]
    public class DirectorWithTwoCompleteVisitorsTest
    {
        public interface IMyNode { }
        public class MyNodeO : IMyNode { }
        public class MyNodeA : IMyNode { }
        public class MyNodeB : MyNodeA { }

        public interface IMyVisitor1
            : IVisitor<MyDir, IMyNode, IMyNode>
            , IVisitor<MyDir, IMyNode, MyNodeO>
        {
        }

        public interface IMyVisitor2
            : IVisitor<MyDir, IMyNode, MyNodeA>
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
            var mock1 = new Mock<IMyVisitor1>();
            var mock2 = new Mock<IMyVisitor2>();
            var dir = new MyDir(new IVisitor<MyDir, IMyNode>[] { mock1.Object, mock2.Object });

            IMyNode node = new MyNodeO();
            dir.Visit(node);

            mock1.Verify(v => v.Visit(dir, It.Is<MyNodeO>(n => n == node)), Times.Once);
            mock1.Verify(v => v.Visit(dir, It.IsAny<IMyNode>()), Times.Never);
            
            mock2.Verify(v => v.Visit(dir, It.IsAny<MyNodeA>()), Times.Never);
            mock2.Verify(v => v.Visit(dir, It.IsAny<MyNodeB>()), Times.Never);
        }

        [Test]
        public void TestNodeForeignNode()
        {
            var mock1 = new Mock<IMyVisitor1>();
            var mock2 = new Mock<IMyVisitor2>();
            var dir = new MyDir(new IVisitor<MyDir, IMyNode>[] { mock1.Object, mock2.Object });

            IMyNode node = new Mock<IMyNode>().Object;
            dir.Visit(node);

            mock1.Verify(v => v.Visit(dir, It.IsAny<MyNodeO>()), Times.Never);
            mock1.Verify(v => v.Visit(dir, It.Is<IMyNode>(n => n == node)), Times.Once);
            
            mock2.Verify(v => v.Visit(dir, It.IsAny<MyNodeA>()), Times.Never);
            mock2.Verify(v => v.Visit(dir, It.IsAny<MyNodeB>()), Times.Never);
        }

        [Test]
        public void TestNodeA()
        {
            var mock1 = new Mock<IMyVisitor1>();
            var mock2 = new Mock<IMyVisitor2>();
            var dir = new MyDir(new IVisitor<MyDir, IMyNode>[] { mock1.Object, mock2.Object });

            IMyNode node = new MyNodeA();
            dir.Visit(node);

            mock1.Verify(v => v.Visit(dir, It.IsAny<MyNodeO>()), Times.Never);
            mock1.Verify(v => v.Visit(dir, It.IsAny<IMyNode>()), Times.Never);
            
            mock2.Verify(v => v.Visit(dir, It.Is<MyNodeA>(n => n == node)), Times.Once);
            mock2.Verify(v => v.Visit(dir, It.IsAny<MyNodeB>()), Times.Never);
        }

        [Test]
        public void TestNodeB()
        {
            var mock1 = new Mock<IMyVisitor1>();
            var mock2 = new Mock<IMyVisitor2>();
            var dir = new MyDir(new IVisitor<MyDir, IMyNode>[] { mock1.Object, mock2.Object });

            IMyNode node = new MyNodeB();
            dir.Visit(node);

            mock1.Verify(v => v.Visit(dir, It.IsAny<MyNodeO>()), Times.Never);
            mock1.Verify(v => v.Visit(dir, It.IsAny<IMyNode>()), Times.Never);
            
            mock2.Verify(v => v.Visit(dir, It.IsAny<MyNodeA>()), Times.Never);
            mock2.Verify(v => v.Visit(dir, It.Is<MyNodeB>(n => n == node)), Times.Once);
        }
    }
}
