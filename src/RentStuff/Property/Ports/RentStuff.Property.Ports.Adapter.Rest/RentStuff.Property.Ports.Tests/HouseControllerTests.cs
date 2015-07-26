using System.Web.Http;
using System.Web.Http.Results;
using NUnit.Framework;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Ports.Adapter.Rest.Resources;
using Spring.Context.Support;

namespace RentStuff.Property.Ports.Tests
{
    [TestFixture]
    public class HouseControllerTests
    {
        [Test]
        public void InitializeHouseControllerTest_VerifiesThatTheControllerIsInitializedAsExpected_VerfiesThroughInstanceValue()
        {
            HouseController houseController = (HouseController) ContextRegistry.GetContext()["HouseController"];
            Assert.NotNull(houseController);

            string ownerEmail = "thorin@oakenshield123.com";
            House house = new House.HouseBuilder().OwnerEmail(ownerEmail).NumberOfBathrooms(2).NumberOfBedrooms(3)
                .NumberOfKitchens(1).Price(55000).PropertyType(PropertyType.Apartment).CableTvAvailable(true)
                .WithInternetAvailable(true).LandlinePhoneAvailable(true).Build();
            houseController.Save(house);
            IHttpActionResult httpActionResult = houseController.Get(ownerEmail);
            House houseResponse = ((OkNegotiatedContentResult<House>) httpActionResult).Content;
            Assert.NotNull(houseResponse);
            Assert.AreEqual(house.OwnerEmail, houseResponse.OwnerEmail);
            Assert.AreEqual(house.NumberOfBathrooms, houseResponse.NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBedrooms, houseResponse.NumberOfBedrooms);
            Assert.AreEqual(house.NumberOfKitchens, houseResponse.NumberOfKitchens);
            Assert.AreEqual(house.InternetAvailable, houseResponse.InternetAvailable);
            Assert.AreEqual(house.CableTvAvailable, houseResponse.CableTvAvailable);
            Assert.IsTrue(house.ForRent);
            Assert.AreEqual(house.ForRent, houseResponse.ForRent);
            Assert.AreEqual(house.PropertyType, houseResponse.PropertyType);
            Assert.AreEqual(house.Price, houseResponse.Price);

            // Remove the house instance
            DeleteHouse(houseResponse.Id);
        }

        private void DeleteHouse(long id)
        {
            HouseController houseController = (HouseController)ContextRegistry.GetContext()["HouseController"];
            houseController.Delete(id);
        }
    }
}
