using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            Assert.AreEqual(37.4220459, coordinates.Item1);
            Assert.AreEqual(-122.0841477, coordinates.Item2);
            Thread.Sleep(2000);
        }
    }
}
