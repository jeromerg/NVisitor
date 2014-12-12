using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using NVisitor.Api;
using NVisitor.Api.Marker;

namespace NVisitorTest.Api
{
    [TestFixture]
    public class DirectorWithConflictingVisitor
    {
        public interface IMyNode {}
        public interface IMyNode1 : IMyNode { }
        public interface IMyNode2 : IMyNode { }
        public class MyNode : IMyNode1, IMyNode2 { }

        public interface IConflictingVisitor 
            : IVisitor<MyDir, IMyNode, IMyNode>
            , IVisitor<MyDir, IMyNode, IMyNode1>
            , IVisitor<MyDir, IMyNode, IMyNode2>
        {
        }

        public interface ISolvingVisitor 
            : IVisitor<MyDir, IMyNode, MyNode>
        {
        }

        public class MyDir : Director<MyDir, IMyNode>
        {
            public MyDir(IEnumerable<IVisitor<MyDir, IMyNode>> visitors) 
                : base(visitors) { }
        }

        [Test]
        [ExpectedException(typeof(VisitorNotFoundException))]
        public void TestWithConflictingVisitor()
        {
            var mockConflictingVisitor = new Mock<IConflictingVisitor>();
            var dir = new MyDir(new [] { mockConflictingVisitor.Object});

            IMyNode node = new MyNode();
            dir.Visit(node);
        }

        [Test]
        public void TestWithAdditionalSolvingVisitor()
        {
            var mockConflictingVisitor = new Mock<IConflictingVisitor>();
            var mockSolvingVisitor = new Mock<ISolvingVisitor>();
            var dir = new MyDir(new IVisitor<MyDir, IMyNode>[] { mockConflictingVisitor.Object, mockSolvingVisitor.Object });

            IMyNode node = new MyNode();
            dir.Visit(node);

            mockConflictingVisitor.Verify(v => v.Visit(dir, It.IsAny<IMyNode>()), Times.Never);
            mockConflictingVisitor.Verify(v => v.Visit(dir, It.IsAny<IMyNode1>()), Times.Never);
            mockConflictingVisitor.Verify(v => v.Visit(dir, It.IsAny<IMyNode2>()), Times.Never);
            mockSolvingVisitor.Verify(v => v.Visit(dir, It.Is<MyNode>(n => n == node)), Times.Once);
        }

    }
}
