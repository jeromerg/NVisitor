using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using NVisitor.Api;
using NVisitor.Api.Marker;

namespace NVisitorTest.Api
{
    [TestFixture]
    public class DirectorWithSingleIncompleteVisitorTest
    {
        public interface IMyFamily {}
        public class MyNodeO : IMyFamily { }
        public class MyNodeA : IMyFamily { }
        public class MyNodeB : MyNodeA {}

        public interface IMyVisitor
            : IVisitor<IMyFamily, MyDir, MyNodeO>
        {
        }

        public class MyDir : Director<IMyFamily, MyDir>
        {
            public MyDir(IEnumerable<IVisitor<IMyFamily, MyDir>> visitors) 
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
        }

        [Test]
        [ExpectedException(typeof(VisitorNotFoundException))]
        public void TestNodeA()
        {
            var mock = new Mock<IMyVisitor>();
            var dir = new MyDir(new[] { mock.Object });

            IMyFamily node = new MyNodeA();
            dir.Visit(node);
        }

        [Test]
        [ExpectedException(typeof(VisitorNotFoundException))]
        public void TestNodeB()
        {
            var mock = new Mock<IMyVisitor>();
            var dir = new MyDir(new[] { mock.Object });

            IMyFamily node = new MyNodeB();
            dir.Visit(node);
        }

        [Test]
        [ExpectedException(typeof(VisitorNotFoundException))]
        public void TestNodeForeignNode()
        {
            var mock = new Mock<IMyVisitor>();
            var dir = new MyDir(new[] { mock.Object });

            IMyFamily node = new Mock<IMyFamily>().Object;
            dir.Visit(node);

        }
    }
}
