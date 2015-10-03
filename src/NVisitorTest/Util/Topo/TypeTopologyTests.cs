using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using NVisitor.Common.Topo;

namespace NVisitorTest.Util.Topo
{
    [TestFixture]
    [SuppressMessage("ReSharper", "UnusedTypeParameter")]
    public class TypeTopologyTests
    {
        public class O
        {
        }

        public class A
        {
        }

        public class B : A
        {
        }

        public class X
        {
        }

        public class Y : X
        {
        }

        public class Z : Y
        {
        }

        [Test, ExpectedException(typeof (TargetTypeNotResolvedException))]
        public void TestA_B()
        {
            Type result = TypeHelper.FindBestCandidateToAssignFrom(typeof (A), new[] {typeof (B)});

            Assert.AreEqual(null, result);
        }

        [Test]
        public void TestB_A()
        {
            Type result = TypeHelper.FindBestCandidateToAssignFrom(typeof (B), new[] {typeof (A)});

            Assert.AreEqual(typeof (A), result);
        }


        [Test, ExpectedException(typeof (TargetTypeNotResolvedException))]
        public void TestC_IC1_IC2()
        {
            Type result = TypeHelper.FindBestCandidateToAssignFrom(typeof (C), new[] {typeof (IC1), typeof (IC2)});

            Assert.AreEqual(null, result);
        }

        [Test]
        public void TestC_IC1_IC2_C()
        {
            Type result = TypeHelper.FindBestCandidateToAssignFrom(typeof (C), new[] {typeof (IC1), typeof (IC2), typeof (C)});

            Assert.AreEqual(typeof (C), result);
        }

        [Test]
        public void TestO()
        {
            Type result = TypeHelper.FindBestCandidateToAssignFrom(typeof (O), new[] {typeof (O)});

            Assert.AreEqual(typeof (O), result);
        }

        [Test, ExpectedException(typeof (TargetTypeNotResolvedException))]
        public void TestO_A()
        {
            Type result = TypeHelper.FindBestCandidateToAssignFrom(typeof (O), new[] {typeof (A)});

            Assert.AreEqual(null, result);
        }

        [Test]
        public void TestZ_X_Y()
        {
            Type result = TypeHelper.FindBestCandidateToAssignFrom(typeof (Z), new[] {typeof (Y), typeof (X)});

            Assert.AreEqual(typeof (Y), result);
        }

        // ReSharper disable InconsistentNaming
        public class IC1
        {
        }

        public interface IC2
        {
        }

        public class C : IC1, IC2
        {
        }

        // ReSharper restore InconsistentNaming
    }
}