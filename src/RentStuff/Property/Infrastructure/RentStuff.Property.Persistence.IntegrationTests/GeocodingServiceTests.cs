using System;
using System.Threading;
using NUnit.Framework;
using RentStuff.Property.Domain.Model.Services;
using Spring.Context.Support;

namespace RentStuff.Property.Persistence.IntegrationTests
{
    [TestFixture]
    class GeocodingServiceTests
    {
        [Test]
        public void GetGeocodingCoordinatesFromAddressTest_TestsIfTheCoordinatesAreReturnedGivenTheAddressAsExpected_VerifiesThroughTheReturnVariables()
        {
            IGeocodingService geocodingService = (IGeocodingService)ContextRegistry.GetContext()["GeocodingService"];
            Tuple<decimal,decimal> coordinates = geocodingService.GetCoordinatesFromAddress("Pindora, Rawalpindi, Pakistan");
            Assert.IsNotNull(coordinates);
            Assert.AreEqual(33.6497937, coordinates.Item1);
            Assert.AreEqual(73.0685665, coordinates.Item2);
        }
    }
}
