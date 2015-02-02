﻿using Moq;
using NUnit.Framework;
using NVisitor.Api.Batch;

namespace NVisitorTest.Api.Batch
{
    
    /// <summary>
    /// This class test the scenario, where the dispatcher is represented by an interface `IMyDir`. It is typically the case
    /// if you use dependency injection (for example, if want to mock it while you test the visitors one by one)
    /// </summary>
    [TestFixture]
    public class DirectorWithDirectorContractTest
    {
        
        public interface INode {}
        public class MyNodeO : INode {}

        /// <summary> Interface IDENTYING the dispatcher uniquely </summary>
        public interface IMyDir : IDirector<INode, IMyDir> { }

        public class MyDir1 : Director<INode, IMyDir>, IMyDir
        {
            public MyDir1(params IVisitorClass<INode, IMyDir>[] visitorArray)
                : base(visitorArray)
            {
            }
        }

        public interface IMyVisitor
            : IVisitor<INode, IMyDir, INode>
            , IVisitor<INode, IMyDir, MyNodeO>
        {}

        [Test]
        public void AllRelatedToIMyDir_Test()
        {
            var mock = new Mock<IMyVisitor>();
            var dir = new MyDir1(mock.Object);

            MyNodeO node = new MyNodeO();
            
            mock.Setup(v => v.Visit(dir, node));

            dir.Visit(node);

            mock.Verify(v => v.Visit(dir, node), Times.Once);
        }

    }
}
