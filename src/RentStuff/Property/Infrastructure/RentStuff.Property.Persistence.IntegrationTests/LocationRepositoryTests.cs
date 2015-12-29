using System.Collections;
using NUnit.Framework;
using RentStuff.Property.Domain.Model.HouseAggregate;
using Spring.Context.Support;

namespace RentStuff.Property.Persistence.IntegrationTests
{
    [TestFixture]
    class LocationRepositoryTests
    {
        [Test]
        public void GetNearestCoordinatesTest_TestsThatTheNearstCoordinatesAreReturnedAsExpected_VerifiesThroughTheReturnValue()
        {
            ILocationRepository locationRepository = (ILocationRepository)ContextRegistry.GetContext()["LocationRepository"];
            IList coordinatesList = locationRepository.GetLocationByCoordinates(22, 100);
            Assert.NotNull(coordinatesList);
            Assert.AreEqual(20, coordinatesList.Count);
        }
    }
}
