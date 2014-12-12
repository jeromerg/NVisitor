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
        public interface IMyNode {}
        public class MyNodeO : IMyNode { }
        public class MyNodeA : IMyNode { }
        public class MyNodeB : MyNodeA {}

        public interface IMyVisitor 
            : IVisitor<MyDir, IMyNode, MyNodeO>
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
        }

        [Test]
        [ExpectedException(typeof(VisitorNotFoundException))]
        public void TestNodeA()
        {
            var mock = new Mock<IMyVisitor>();
            var dir = new MyDir(new[] { mock.Object });

            IMyNode node = new MyNodeA();
            dir.Visit(node);
        }

        [Test]
        [ExpectedException(typeof(VisitorNotFoundException))]
        public void TestNodeB()
        {
            var mock = new Mock<IMyVisitor>();
            var dir = new MyDir(new[] { mock.Object });

            IMyNode node = new MyNodeB();
            dir.Visit(node);
        }

        [Test]
        [ExpectedException(typeof(VisitorNotFoundException))]
        public void TestNodeForeignNode()
        {
            var mock = new Mock<IMyVisitor>();
            var dir = new MyDir(new[] { mock.Object });

            IMyNode node = new Mock<IMyNode>().Object;
            dir.Visit(node);

        }
    }
}
