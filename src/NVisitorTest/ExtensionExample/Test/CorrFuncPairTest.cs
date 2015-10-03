using NUnit.Framework;
using NVisitor.Api;
using NVisitorTest.ExtensionExample.CarOperation;

namespace NVisitorTest.ExtensionExample.Test
{
    [TestFixture]
    public class CorrFuncPairTest
    {
        public class OperationA : IOperation<string>
        {
        }

        public class OperationB : IOperation<string>
        {
        }

        public class OperationC : IOperation<int>
        {
        }


        public class Bmw : ICar
        {
            public int Wings { get; set; }
        }

        public class Vw : ICar
        {
            public int Flippers { get; set; }
        }

        public class CarOperationImp
            : ICarOperationImp<OperationA, Bmw, string>,
              ICarOperationImp<OperationA, Vw, string>,
              ICarOperationImp<OperationA, ICar, string>,
              ICarOperationImp<OperationB, Vw, string>,
              ICarOperationImp<OperationC, ICar, int>
        {
            public string Perform(ICarOperationDirector director, OperationA operation, Bmw car)
            {
                return "OperationA on Bmw";
            }

            public string Perform(ICarOperationDirector director, OperationA operation, ICar car)
            {
                return "OperationA on a car";
            }

            public string Perform(ICarOperationDirector director, OperationA operation, Vw car)
            {
                return "OperationA on Vw";
            }

            public string Perform(ICarOperationDirector director, OperationB operation, Vw car)
            {
                return "OperationB on Vw";
            }

            public int Perform(ICarOperationDirector director, OperationC operation, ICar car)
            {
                return 7;
            }
        }

        [Test]
        public void TestSimpleActionVisit()
        {
            var dir = new CarOperationDirector(new ICarOperationVisitorClass[] {new CarOperationImp()});

            string result1 = dir.Perform(new OperationA(), new Bmw());
            Assert.AreEqual("OperationA on Bmw", result1);

            string result2 = dir.Perform(new OperationA(), new Vw());
            Assert.AreEqual("OperationA on Vw", result2);

            try
            {
                dir.Perform(new OperationB(), new Bmw());
                Assert.Fail();
            }
            catch (VisitorNotFoundException)
            {
            }

            int result3 = dir.Perform(new OperationC(), new Vw());
            Assert.AreEqual(7, result3);
        }
    }
}