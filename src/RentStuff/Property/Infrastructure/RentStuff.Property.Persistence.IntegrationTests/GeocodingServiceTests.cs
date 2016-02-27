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
            Tuple<decimal,decimal> coordinates = geocodingService.GetCoordinatesFromAddress("1600+Amphitheatre+Parkway,+Mountain+View,+CA");
            Assert.IsNotNull(coordinates);
            Assert.AreEqual(37.4224504, coordinates.Item1);
            Assert.AreEqual(-122.0840859, coordinates.Item2);
        }
    }
}
