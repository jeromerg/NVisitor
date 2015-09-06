using Moq;
using NUnit.Framework;
using NVisitor.Api;
using NVisitor.Api.Batch;
using NVisitor.Api.PairBatch;

namespace NVisitorTest.Api.BatchPair
{
    [TestFixture]
    public class BatchPairTest
    {
        public interface ICar {}
        public class Bmw : ICar { }
        public class Audi : ICar { }

        public interface IPerson {}
        public class Driver : IPerson { }
        public class Mechanic : IPerson { }


        public interface IVisitor_Car_Person    : IPairVisitor<ICar, IPerson, MyDir, ICar,  IPerson> { }
        public interface IVisitor_Car_Driver    : IPairVisitor<ICar, IPerson, MyDir, ICar,  Driver> { }
        public interface IVisitor_Car_Mechanic  : IPairVisitor<ICar, IPerson, MyDir, ICar,  Mechanic> { }

        public interface IVisitor_Bmw_Person    : IPairVisitor<ICar, IPerson, MyDir, Bmw,  IPerson> { }
        public interface IVisitor_Bmw_Driver    : IPairVisitor<ICar, IPerson, MyDir, Bmw,  Driver> { }
        public interface IVisitor_Bmw_Mechanic  : IPairVisitor<ICar, IPerson, MyDir, Bmw,  Mechanic> { }

        public interface IVisitor_Audi_Person   : IPairVisitor<ICar, IPerson, MyDir, Audi, IPerson> { }
        public interface IVisitor_Audi_Driver   : IPairVisitor<ICar, IPerson, MyDir, Audi, Driver> { }
        public interface IVisitor_Audi_Mechanic : IPairVisitor<ICar, IPerson, MyDir, Audi, Mechanic> { }

        public class MyDir : PairDirector<ICar, IPerson, MyDir>
        {
            public MyDir(params IPairVisitorClass<ICar, IPerson, MyDir>[] visitorArray)
                : base(visitorArray)
            {
            }
        }

        [Test]
        public void TestSimpleDoubleDispatch()
        {
            var mockVisitor_Bmw_Driver    = new Mock<IVisitor_Bmw_Driver    >();
            var mockVisitor_Bmw_Mechanic  = new Mock<IVisitor_Bmw_Mechanic  >();
            var mockVisitor_Audi_Driver   = new Mock<IVisitor_Audi_Driver   >();
            var mockVisitor_Audi_Mechanic = new Mock<IVisitor_Audi_Mechanic >();

            var dir = new MyDir(
                mockVisitor_Bmw_Driver.Object,
                mockVisitor_Bmw_Mechanic.Object,
                mockVisitor_Audi_Driver.Object,
                mockVisitor_Audi_Mechanic.Object);

            Bmw bmw = new Bmw();
            Audi audi = new Audi();
            Driver driver = new Driver();
            Mechanic mechanic = new Mechanic();

            dir.Visit(bmw, driver);

            mockVisitor_Bmw_Driver    .Verify(v => v.Visit(dir, bmw, driver), Times.Once);
            mockVisitor_Bmw_Mechanic  .Verify(v => v.Visit(dir, It.IsAny<Bmw>(), It.IsAny<Mechanic>()), Times.Never);
            mockVisitor_Audi_Driver   .Verify(v => v.Visit(dir, It.IsAny<Audi>(), It.IsAny<Driver>()), Times.Never);
            mockVisitor_Audi_Mechanic .Verify(v => v.Visit(dir, It.IsAny<Audi>(), It.IsAny<Mechanic>()), Times.Never);

            mockVisitor_Bmw_Driver    .ResetCalls();
            mockVisitor_Bmw_Mechanic  .ResetCalls();
            mockVisitor_Audi_Driver   .ResetCalls();
            mockVisitor_Audi_Mechanic.ResetCalls();

            dir.Visit(bmw, mechanic);

            mockVisitor_Bmw_Driver    .Verify(v => v.Visit(dir, It.IsAny<Bmw>(), It.IsAny<Driver>()), Times.Never);
            mockVisitor_Bmw_Mechanic  .Verify(v => v.Visit(dir, bmw, mechanic), Times.Once);
            mockVisitor_Audi_Driver   .Verify(v => v.Visit(dir, It.IsAny<Audi>(), It.IsAny<Driver>()), Times.Never);
            mockVisitor_Audi_Mechanic .Verify(v => v.Visit(dir, It.IsAny<Audi>(), It.IsAny<Mechanic>()), Times.Never);

            mockVisitor_Bmw_Driver    .ResetCalls();
            mockVisitor_Bmw_Mechanic  .ResetCalls();
            mockVisitor_Audi_Driver   .ResetCalls();
            mockVisitor_Audi_Mechanic.ResetCalls();

            dir.Visit(audi, driver);

            mockVisitor_Bmw_Driver    .Verify(v => v.Visit(dir, It.IsAny<Bmw>(), It.IsAny<Driver>()), Times.Never);
            mockVisitor_Bmw_Mechanic  .Verify(v => v.Visit(dir, It.IsAny<Bmw>(), It.IsAny<Mechanic>()), Times.Never);
            mockVisitor_Audi_Driver   .Verify(v => v.Visit(dir, audi, driver), Times.Once);
            mockVisitor_Audi_Mechanic .Verify(v => v.Visit(dir, It.IsAny<Audi>(), It.IsAny<Mechanic>()), Times.Never);

            mockVisitor_Bmw_Driver    .ResetCalls();
            mockVisitor_Bmw_Mechanic  .ResetCalls();
            mockVisitor_Audi_Driver   .ResetCalls();
            mockVisitor_Audi_Mechanic.ResetCalls();

            dir.Visit(audi, mechanic);

            mockVisitor_Bmw_Driver    .Verify(v => v.Visit(dir, It.IsAny<Bmw>(), It.IsAny<Driver>()), Times.Never);
            mockVisitor_Bmw_Mechanic  .Verify(v => v.Visit(dir, It.IsAny<Bmw>(), It.IsAny<Mechanic>()), Times.Never);
            mockVisitor_Audi_Driver   .Verify(v => v.Visit(dir, It.IsAny<Audi>(), It.IsAny<Driver>()), Times.Never);
            mockVisitor_Audi_Mechanic .Verify(v => v.Visit(dir, audi, mechanic), Times.Once);

            mockVisitor_Bmw_Driver    .ResetCalls();
            mockVisitor_Bmw_Mechanic  .ResetCalls();
            mockVisitor_Audi_Driver   .ResetCalls();
            mockVisitor_Audi_Mechanic.ResetCalls();

        }

        [Test]
        public void TestFirstNodeTypeHasPrecedence()
        {
            var mockVisitor_Car_Driver = new Mock<IVisitor_Car_Driver>();
            var mockVisitor_Bmw_Person = new Mock<IVisitor_Bmw_Person>();

            var dir = new MyDir(
                mockVisitor_Car_Driver.Object,
                mockVisitor_Bmw_Person.Object);

            Bmw bmw = new Bmw();
            Driver driver = new Driver();

            dir.Visit(bmw, driver);

            mockVisitor_Car_Driver.Verify(v => v.Visit(dir, It.IsAny<ICar>(), It.IsAny<Driver>()), Times.Never);
            mockVisitor_Bmw_Person.Verify(v => v.Visit(dir, bmw, driver), Times.Once);

        }

    }       
}
