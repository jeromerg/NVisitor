using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using NVisitor.Api;
using NVisitor.Api.Lazy;

namespace NVisitorTest.Api.Lazy
{
    [TestFixture]
    public class LazyDirectorWithConflictingVisitor
    {
        public interface IMyFamily {}
        public interface IMyFamily1 : IMyFamily { }
        public interface IMyFamily2 : IMyFamily { }
        public class MyNode : IMyFamily1, IMyFamily2 { }

        public interface IConflictingVisitor
            : ILazyVisitor<IMyFamily, MyDir, IMyFamily>
            , ILazyVisitor<IMyFamily, MyDir, IMyFamily1>
            , ILazyVisitor<IMyFamily, MyDir, IMyFamily2>
        {
        }

        public interface ISolvingVisitor
            : ILazyVisitor<IMyFamily, MyDir, MyNode>
        {
        }

        public class MyDir : LazyDirector<IMyFamily, MyDir>
        {
            public MyDir(IEnumerable<ILazyVisitorClass<IMyFamily, MyDir>> visitors) 
                : base(visitors) { }
        }

        [Test]
        [ExpectedException(typeof(VisitorNotFoundException))]
        public void TestWithConflictingVisitor()
        {
            var mockConflictingVisitor = new Mock<IConflictingVisitor>();
            var dir = new MyDir(new [] { mockConflictingVisitor.Object});

            IMyFamily node = new MyNode();
            dir.Visit(node);
        }

        [Test]
        public void TestWithAdditionalSolvingVisitor()
        {
            var mockConflictingVisitor = new Mock<IConflictingVisitor>();
            var mockSolvingVisitor = new Mock<ISolvingVisitor>();
            var dir = new MyDir(new ILazyVisitorClass<IMyFamily, MyDir>[] { mockConflictingVisitor.Object, mockSolvingVisitor.Object });

            IMyFamily node = new MyNode();
            dir.Visit(node);

            mockConflictingVisitor.Verify(v => v.Visit(dir, It.IsAny<IMyFamily>()), Times.Never);
            mockConflictingVisitor.Verify(v => v.Visit(dir, It.IsAny<IMyFamily1>()), Times.Never);
            mockConflictingVisitor.Verify(v => v.Visit(dir, It.IsAny<IMyFamily2>()), Times.Never);
            mockSolvingVisitor.Verify(v => v.Visit(dir, It.Is<MyNode>(n => n == node)), Times.Once);
        }

    }
}
