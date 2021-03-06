﻿using Moq;
using NUnit.Framework;
using NVisitor.Api;
using NVisitor.Api.Lazy;

namespace NVisitorTest.Api.Lazy
{
    [TestFixture]
    public class LazyDirectorWithSingleIncompleteVisitorTest
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

        public interface IMyVisitor
            : ILazyVisitor<INode, MyDir, MyNodeO>
        {
        }

        public class MyDir : LazyDirector<INode, MyDir>
        {
            public MyDir(params ILazyVisitorClass<INode, MyDir>[] visitors)
                : base(visitors)
            {
            }
        }

        [Test]
        [ExpectedException(typeof (VisitorNotFoundException))]
        public void TestNodeA()
        {
            var mock = new Mock<IMyVisitor>();
            var dir = new MyDir(mock.Object);

            INode node = new MyNodeA();
            dir.Visit(node);
        }

        [Test]
        [ExpectedException(typeof (VisitorNotFoundException))]
        public void TestNodeB()
        {
            var mock = new Mock<IMyVisitor>();
            var dir = new MyDir(mock.Object);

            INode node = new MyNodeB();
            dir.Visit(node);
        }

        [Test]
        [ExpectedException(typeof (VisitorNotFoundException))]
        public void TestNodeForeignNode()
        {
            var mock = new Mock<IMyVisitor>();
            var dir = new MyDir(mock.Object);

            INode node = new Mock<INode>().Object;
            dir.Visit(node);
        }

        [Test]
        public void TestNodeO()
        {
            var mock = new Mock<IMyVisitor>();
            var dir = new MyDir(mock.Object);

            INode node = new MyNodeO();
            dir.Visit(node);

            mock.Verify(v => v.Visit(dir, It.Is<MyNodeO>(n => n == node)), Times.Once);
        }
    }
}