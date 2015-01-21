using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using NVisitor.Api.Lazy;

namespace NVisitorTest.Api.Lazy
{
    [TestFixture]
    public class LazyDirectorWithSingleCompleteVisitorTest
    {
        public interface IMyFamily {}
        public class MyNodeO : IMyFamily { }
        public class MyNodeA : IMyFamily { }
        public class MyNodeB : MyNodeA {}

        public interface IMyVisitor
            : ILazyVisitor<IMyFamily, MyDir, IMyFamily>
            , ILazyVisitor<IMyFamily, MyDir, MyNodeO>
            , ILazyVisitor<IMyFamily, MyDir, MyNodeA>
            , ILazyVisitor<IMyFamily, MyDir, MyNodeB>
        {
        }

        public class MyDir : LazyDirector<IMyFamily, MyDir>
        {
            public MyDir(IEnumerable<ILazyVisitorClass<IMyFamily, MyDir>> visitors) 
                : base(visitors) { }
        }

        [Test]
        public void TestNodeO()
        {
            var mock = new Mock<IMyVisitor>();
            var dir = new MyDir(new[] { mock.Object });

            IMyFamily node = new MyNodeO();
            dir.Visit(node);

            mock.Verify(v => v.Visit(dir, It.Is<MyNodeO>(n => n == node)), Times.Once);
            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeA>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeB>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.IsAny<IMyFamily>()), Times.Never);
        }

        [Test]
        public void TestNodeA()
        {
            var mock = new Mock<IMyVisitor>();
            var dir = new MyDir(new[] { mock.Object });

            IMyFamily node = new MyNodeA();
            dir.Visit(node);

            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeO>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.Is<MyNodeA>(n => n == node)), Times.Once);
            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeB>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.IsAny<IMyFamily>()), Times.Never);
        }

        [Test]
        public void TestNodeB()
        {
            var mock = new Mock<IMyVisitor>();
            var dir = new MyDir(new[] { mock.Object });

            IMyFamily node = new MyNodeB();
            dir.Visit(node);

            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeO>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeA>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.Is<MyNodeB>(n => n == node)), Times.Once);
            mock.Verify(v => v.Visit(dir, It.IsAny<IMyFamily>()), Times.Never);
        }

        [Test]
        public void TestNodeForeignNode()
        {
            var mock = new Mock<IMyVisitor>();
            var dir = new MyDir(new[] { mock.Object });

            IMyFamily node = new Mock<IMyFamily>().Object;
            dir.Visit(node);

            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeO>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeA>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.IsAny<MyNodeB>()), Times.Never);
            mock.Verify(v => v.Visit(dir, It.Is<IMyFamily>(n => n == node)), Times.Once);
        }
    }
}
