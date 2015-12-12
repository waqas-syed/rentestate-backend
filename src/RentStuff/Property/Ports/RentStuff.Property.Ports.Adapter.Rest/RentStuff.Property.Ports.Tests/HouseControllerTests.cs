using System.Collections.Generic;
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
        public void SaveAndGetHouseInstanceByEmailTest_TestsThatHouseIsSavedAndRetreivedAsExpected_VerfiesThroughInstanceValue()
        {
            HouseController houseController = (HouseController) ContextRegistry.GetContext()["HouseController"];
            Assert.NotNull(houseController);

            string ownerEmail = "thorin@oakenshield123.com";
            House house = new House.HouseBuilder().OwnerEmail(ownerEmail).NumberOfBathrooms(2).NumberOfBedrooms(3)
                .NumberOfKitchens(1).MonthlyRent(55000).PropertyType(PropertyType.Apartment).CableTvAvailable(true)
                .WithInternetAvailable(true).LandlinePhoneAvailable(true).Build();
            houseController.Post(house);
            IHttpActionResult httpActionResult = houseController.Get(ownerEmail);
            IList<House> houseList = ((OkNegotiatedContentResult<IList<House>>) httpActionResult).Content;
            Assert.NotNull(houseList);
            Assert.AreEqual(1, houseList.Count);
            House houseResponse = houseList[0];
            Assert.NotNull(houseResponse);
            Assert.AreEqual(house.OwnerEmail, houseResponse.OwnerEmail);
            Assert.AreEqual(house.NumberOfBathrooms, houseResponse.NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBedrooms, houseResponse.NumberOfBedrooms);
            Assert.AreEqual(house.NumberOfKitchens, houseResponse.NumberOfKitchens);
            Assert.AreEqual(house.InternetAvailable, houseResponse.InternetAvailable);
            Assert.AreEqual(house.CableTvAvailable, houseResponse.CableTvAvailable);
            Assert.AreEqual(house.PropertyType, houseResponse.PropertyType);
            Assert.AreEqual(house.MonthlyRent, houseResponse.MonthlyRent);

            // Remove the house instance
            DeleteHouse(houseResponse.Id);
        }

        [Test]
        public void SaveAndGetAllHousesTest_TestsThatHousesAreSavedAndRetreivedAsExpected_VerfiesThroughInstanceValue()
        {
            HouseController houseController = (HouseController)ContextRegistry.GetContext()["HouseController"];
            Assert.NotNull(houseController);

            string ownerEmail = "thorin@oakenshield123.com";

            // House # 1
            House house1 = new House.HouseBuilder().OwnerEmail(ownerEmail).NumberOfBathrooms(2).NumberOfBedrooms(3)
                .NumberOfKitchens(1).MonthlyRent(55000).PropertyType(PropertyType.Apartment).CableTvAvailable(true)
                .WithInternetAvailable(true).LandlinePhoneAvailable(true).Build();
            houseController.Post(house1);

            // Hopuse # 2
            House house2 = new House.HouseBuilder().OwnerEmail(ownerEmail).NumberOfBathrooms(2).NumberOfBedrooms(3)
                .NumberOfKitchens(1).MonthlyRent(55000).PropertyType(PropertyType.Apartment).CableTvAvailable(true)
                .WithInternetAvailable(true).LandlinePhoneAvailable(true).Build();
            houseController.Post(house2);

            IHttpActionResult httpActionResult = houseController.Get(ownerEmail);
            IList<House> houseList = ((OkNegotiatedContentResult<IList<House>>)httpActionResult).Content;
            Assert.NotNull(houseList);
            Assert.AreEqual(2, houseList.Count);

            // Checking assertions on House # 1
            House houseResponse1 = houseList[0];
            Assert.NotNull(houseResponse1);
            Assert.AreEqual(house1.OwnerEmail, houseResponse1.OwnerEmail);
            Assert.AreEqual(house1.NumberOfBathrooms, houseResponse1.NumberOfBathrooms);
            Assert.AreEqual(house1.NumberOfBedrooms, houseResponse1.NumberOfBedrooms);
            Assert.AreEqual(house1.NumberOfKitchens, houseResponse1.NumberOfKitchens);
            Assert.AreEqual(house1.InternetAvailable, houseResponse1.InternetAvailable);
            Assert.AreEqual(house1.CableTvAvailable, houseResponse1.CableTvAvailable);
            Assert.AreEqual(house1.PropertyType, houseResponse1.PropertyType);
            Assert.AreEqual(house1.MonthlyRent, houseResponse1.MonthlyRent);

            // Checking assertions on House # 1
            House houseResponse2 = houseList[1];
            Assert.NotNull(houseResponse2);
            Assert.AreEqual(house2.OwnerEmail, houseResponse2.OwnerEmail);
            Assert.AreEqual(house2.NumberOfBathrooms, houseResponse2.NumberOfBathrooms);
            Assert.AreEqual(house2.NumberOfBedrooms, houseResponse2.NumberOfBedrooms);
            Assert.AreEqual(house2.NumberOfKitchens, houseResponse2.NumberOfKitchens);
            Assert.AreEqual(house2.InternetAvailable, houseResponse2.InternetAvailable);
            Assert.AreEqual(house2.CableTvAvailable, houseResponse2.CableTvAvailable);
            Assert.AreEqual(house2.PropertyType, houseResponse2.PropertyType);
            Assert.AreEqual(house2.MonthlyRent, houseResponse2.MonthlyRent);

            // Remove the house instance
            DeleteHouse(houseResponse1.Id);
            DeleteHouse(houseResponse2.Id);
        }

        private void DeleteHouse(long id)
        {
            HouseController houseController = (HouseController)ContextRegistry.GetContext()["HouseController"];
            houseController.Delete(id);
        }
    }
}
