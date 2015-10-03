using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using NVisitor.Common.Topo;

namespace NVisitorTest.Util.Topo
{
    [TestFixture]
    [SuppressMessage("ReSharper", "UnusedTypeParameter")]
    public class TypeTopologyCoAndContravarianceTests
    {
        public class A
        {
        }

        public class B : A
        {
        }

        public interface ICovariant<out T>
        {
        }

        public interface IContravariant<in T>
        {
        }

        public interface ICoAndContravariantMix<in T1, out T2>
        {
        }

        public class Covariant<T> : ICovariant<T>
        {
        }

        public class Contravariant<T> : IContravariant<T>
        {
        }

        public class CoAndContravariant<T> : ICovariant<T>, IContravariant<T>
        {
        }

        public class CoAndContravariantMix<T1, T2> : ICoAndContravariantMix<T1, T2>
        {
        }

        #region Implementation implementing multiple co- and contravariant contracts

        [Test]
        public void TestClassImplementionCoAndContravariantInterfaces_Covariance()
        {
            Type result = TypeHelper.FindBestCandidateToAssignFrom(typeof (CoAndContravariant<B>),
                                                                   new[] {typeof (ICovariant<A>)});
            Assert.AreEqual(typeof (ICovariant<A>), result);
        }

        [Test]
        public void TestClassImplementionCoAndContravariantInterfaces_Contravariance()
        {
            Type result = TypeHelper.FindBestCandidateToAssignFrom(typeof (CoAndContravariant<A>),
                                                                   new[] {typeof (IContravariant<B>)});
            Assert.AreEqual(typeof(IContravariant<B>), result);
        }

        #endregion

        #region multiple co- and contravariant generics parameters

        [Test]
        public void TestCoAndContravariantMix_Covariance()
        {
            Type result = TypeHelper.FindBestCandidateToAssignFrom(typeof (CoAndContravariantMix<A, B /*!*/>),
                                                                   new[] {typeof (ICoAndContravariantMix<A, A /*!*/>)});
            Assert.AreEqual(typeof (ICoAndContravariantMix<A, A /*!*/>), result);
        }

        [Test]
        [ExpectedException(typeof (TargetTypeNotResolvedException))]
        public void TestCoAndContravariantMix_Covariance_Opposite()
        {
            TypeHelper.FindBestCandidateToAssignFrom(typeof (CoAndContravariantMix<A, A /*!*/>),
                                                     new[] {typeof (ICoAndContravariantMix<A, B /*!*/>)});
        }

        [Test]
        public void TestCoAndContravariantMix_Contravariance()
        {
            Type result = TypeHelper.FindBestCandidateToAssignFrom(typeof (CoAndContravariantMix<A /*!*/, B>),
                                                                   new[] {typeof (ICoAndContravariantMix<B /*!*/, B>)});
            Assert.AreEqual(typeof (ICoAndContravariantMix<B /*!*/, B>), result);
        }

        [Test]
        [ExpectedException(typeof (TargetTypeNotResolvedException))]
        public void TestCoAndContravariantMix_Contravariance_Opposite()
        {
            TypeHelper.FindBestCandidateToAssignFrom(typeof (CoAndContravariantMix<B /*!*/, B>),
                                                     new[] {typeof (ICoAndContravariantMix<A /*!*/, B>)});
        }

        #endregion

        #region Contravariance

        [Test]
        public void TestContravariant()
        {
            Type result = TypeHelper.FindBestCandidateToAssignFrom(typeof (Contravariant<A>), new[] {typeof (IContravariant<B>)});

            Assert.AreEqual(typeof (IContravariant<B>), result);
        }

        [Test]
        [ExpectedException(typeof (TargetTypeNotResolvedException))]
        public void TestContravariant_Opposite()
        {
            TypeHelper.FindBestCandidateToAssignFrom(typeof (Contravariant<B>), new[] {typeof (IContravariant<A>)});
        }

        #endregion

        #region Covariance

        [Test]
        public void TestCovariance()
        {
            Type result = TypeHelper.FindBestCandidateToAssignFrom(typeof (Covariant<B>), new[] {typeof (ICovariant<A>)});

            Assert.AreEqual(typeof (ICovariant<A>), result);
        }

        [Test]
        [ExpectedException(typeof (TargetTypeNotResolvedException))]
        public void TestCovariance_Opposite()
        {
            TypeHelper.FindBestCandidateToAssignFrom(typeof (Covariant<A>), new[] {typeof (ICovariant<B>)});
        }

        #endregion
    }
}