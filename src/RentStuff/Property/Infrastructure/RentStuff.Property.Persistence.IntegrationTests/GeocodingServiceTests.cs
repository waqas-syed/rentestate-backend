using System;
using System.Threading;
using Ninject;
using NUnit.Framework;
using RentStuff.Common.NinjectModules;
using RentStuff.Common.Services.LocationServices;
using RentStuff.Property.Infrastructure.Persistence.Ninject.Modules;
using IGeocodingService = RentStuff.Property.Domain.Model.Services.IGeocodingService;

//using Spring.Context.Support;

namespace RentStuff.Property.Persistence.IntegrationTests
{
    [TestFixture]
    class GeocodingServiceTests
    {
        private IKernel _kernel;

        [SetUp]
        public void Setup()
        {
            _kernel = new StandardKernel();
            _kernel.Load<PropertyPersistenceNinjectModule>();
            _kernel.Load<CommonNinjectModule>();
        }

        [Test]
        public void GetGeocodingCoordinatesFromAddressTest_TestsIfTheCoordinatesAreReturnedGivenTheAddressAsExpected_VerifiesThroughTheReturnVariables()
        {
            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService = _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();
            Tuple<decimal,decimal> coordinates = geocodingService.GetCoordinatesFromAddress("Pindora, Rawalpindi, Pakistan");
            Assert.IsNotNull(coordinates);
            Assert.AreEqual(33.6497937, coordinates.Item1);
            Assert.AreEqual(73.0685665, coordinates.Item2);
        }
    }
}
