using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using NVisitor.Api.Batch;

namespace NVisitorTest.Api.Batch
{
    [TestFixture]
    public class DirectorWithTwoCompleteVisitorsTest
    {
        public interface IMyFamily { }
        public class MyNodeO : IMyFamily { }
        public class MyNodeA : IMyFamily { }
        public class MyNodeB : MyNodeA { }

        public interface IMyVisitor1
            : IVisitor<IMyFamily, MyDir, IMyFamily>
            , IVisitor<IMyFamily, MyDir, MyNodeO>
        {
        }

        public interface IMyVisitor2
            : IVisitor<IMyFamily, MyDir, MyNodeA>
            , IVisitor<IMyFamily, MyDir, MyNodeB>
        {
        }

        public class MyDir : Director<IMyFamily, MyDir>
        {
            public MyDir(IEnumerable<IVisitorClass<IMyFamily, MyDir>> visitors)
                : base(visitors) { }
        }

        [Test]
        public void TestNodeO()
        {
            var mock1 = new Mock<IMyVisitor1>();
            var mock2 = new Mock<IMyVisitor2>();
            var dir = new MyDir(new IVisitorClass<IMyFamily, MyDir>[] { mock1.Object, mock2.Object });

            IMyFamily node = new MyNodeO();
            dir.Visit(node);

            mock1.Verify(v => v.Visit(dir, It.Is<MyNodeO>(n => n == node)), Times.Once);
            mock1.Verify(v => v.Visit(dir, It.IsAny<IMyFamily>()), Times.Never);
            
            mock2.Verify(v => v.Visit(dir, It.IsAny<MyNodeA>()), Times.Never);
            mock2.Verify(v => v.Visit(dir, It.IsAny<MyNodeB>()), Times.Never);
        }

        [Test]
        public void TestNodeForeignNode()
        {
            var mock1 = new Mock<IMyVisitor1>();
            var mock2 = new Mock<IMyVisitor2>();
            var dir = new MyDir(new IVisitorClass<IMyFamily, MyDir>[] { mock1.Object, mock2.Object });

            IMyFamily node = new Mock<IMyFamily>().Object;
            dir.Visit(node);

            mock1.Verify(v => v.Visit(dir, It.IsAny<MyNodeO>()), Times.Never);
            mock1.Verify(v => v.Visit(dir, It.Is<IMyFamily>(n => n == node)), Times.Once);
            
            mock2.Verify(v => v.Visit(dir, It.IsAny<MyNodeA>()), Times.Never);
            mock2.Verify(v => v.Visit(dir, It.IsAny<MyNodeB>()), Times.Never);
        }

        [Test]
        public void TestNodeA()
        {
            var mock1 = new Mock<IMyVisitor1>();
            var mock2 = new Mock<IMyVisitor2>();
            var dir = new MyDir(new IVisitorClass<IMyFamily, MyDir>[] { mock1.Object, mock2.Object });

            IMyFamily node = new MyNodeA();
            dir.Visit(node);

            mock1.Verify(v => v.Visit(dir, It.IsAny<MyNodeO>()), Times.Never);
            mock1.Verify(v => v.Visit(dir, It.IsAny<IMyFamily>()), Times.Never);
            
            mock2.Verify(v => v.Visit(dir, It.Is<MyNodeA>(n => n == node)), Times.Once);
            mock2.Verify(v => v.Visit(dir, It.IsAny<MyNodeB>()), Times.Never);
        }

        [Test]
        public void TestNodeB()
        {
            var mock1 = new Mock<IMyVisitor1>();
            var mock2 = new Mock<IMyVisitor2>();
            var dir = new MyDir(new IVisitorClass<IMyFamily, MyDir>[] { mock1.Object, mock2.Object });

            IMyFamily node = new MyNodeB();
            dir.Visit(node);

            mock1.Verify(v => v.Visit(dir, It.IsAny<MyNodeO>()), Times.Never);
            mock1.Verify(v => v.Visit(dir, It.IsAny<IMyFamily>()), Times.Never);
            
            mock2.Verify(v => v.Visit(dir, It.IsAny<MyNodeA>()), Times.Never);
            mock2.Verify(v => v.Visit(dir, It.Is<MyNodeB>(n => n == node)), Times.Once);
        }
    }
}
