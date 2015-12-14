using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using NUnit.Framework;
using RentStuff.Property.Application.HouseServices.Commands;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Ports.Adapter.Rest.Resources;
using Spring.Context.Support;

namespace RentStuff.Property.Ports.Tests
{
    [TestFixture]
    public class HouseControllerTests
    {
        // Flag which allows tests to run or not, as these tests call Google APIs which has a threshold limie. So we want to be sure
        // that we are not exhausting resources by unknowingly running such tests
        private bool _runTests = true;

        [Test]
        public void SaveAndGetHouseInstanceByEmailTest_TestsThatHouseIsSavedAndRetreivedAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                HouseController houseController = (HouseController) ContextRegistry.GetContext()["HouseController"];
                Assert.NotNull(houseController);

                string ownerEmail = "thorin@oakenshield123.com";
                string ownerPhoneNumber = "+923211234567";
                string houseNo = "CT-141/A";
                string streetNo = "14";

                CreateHouseCommand house = new CreateHouseCommand(105000, 2, 2, 2, false, true, false, true, true,
                    true, true, true, "Apartment", ownerEmail, ownerPhoneNumber, houseNo, streetNo,
                    "1600 Amphitheatre Parkway, Mountain View, CA");
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
                Assert.AreEqual(house.MonthlyRent, houseResponse.MonthlyRent);
                Assert.AreEqual(Math.Round(37.4220459, 1), Math.Round(houseResponse.Location.Latitude, 1));
                Assert.AreEqual(Math.Round(-122.0841477, 1), Math.Round(houseResponse.Location.Longitude,1));

                // Remove the house instance
                DeleteHouse(houseResponse.Id);
            }
        }

        [Test]
        public void SaveAndGetAllHousesTest_TestsThatHousesAreSavedAndRetreivedAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                HouseController houseController = (HouseController) ContextRegistry.GetContext()["HouseController"];
                Assert.NotNull(houseController);

                string ownerEmail1 = "thorin@oakenshield123.com";
                string ownerPhoneNumber1 = "+923211234567";
                string houseNo1 = "CT-141/A";
                string streetNo1 = "14";

                CreateHouseCommand house1 = new CreateHouseCommand(105000, 2, 2, 2, false, true, false, true, true,
                    true, true, true, "Apartment", ownerEmail1, ownerPhoneNumber1, houseNo1, streetNo1,
                    "1600+Amphitheatre+Parkway,+Mountain+View,+CA");
                houseController.Post(house1);

                string ownerEmail2 = "thorin@oakenshield1234.com";
                string ownerPhoneNumber2 = "+923211234568";
                string houseNo2 = "CT-141/B";
                string streetNo2 = "13";

                // Hopuse # 2
                CreateHouseCommand house2 = new CreateHouseCommand(150000, 2, 2, 2, true, false, false, true, true,
                    true, true, true, "House", ownerEmail2, ownerPhoneNumber2, houseNo2, streetNo2,
                    "1600+Amphitheatre+Parkway,+Mountain+View,+CA");
                houseController.Post(house2);

                IHttpActionResult httpActionResult = houseController.Get();
                IList<House> houseList = ((OkNegotiatedContentResult<IList<House>>) httpActionResult).Content;
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
                Assert.AreEqual(house1.PropertyType, houseResponse1.PropertyType.ToString());
                Assert.AreEqual(house1.MonthlyRent, houseResponse1.MonthlyRent);
                Assert.AreEqual(Math.Round(37.4220459, 1), Math.Round(houseResponse1.Location.Latitude, 1));
                Assert.AreEqual(Math.Round(-122.0841477, 1), Math.Round(houseResponse1.Location.Longitude, 1));

                // Checking assertions on House # 1
                House houseResponse2 = houseList[1];
                Assert.NotNull(houseResponse2);
                Assert.AreEqual(house2.OwnerEmail, houseResponse2.OwnerEmail);
                Assert.AreEqual(house2.NumberOfBathrooms, houseResponse2.NumberOfBathrooms);
                Assert.AreEqual(house2.NumberOfBedrooms, houseResponse2.NumberOfBedrooms);
                Assert.AreEqual(house2.NumberOfKitchens, houseResponse2.NumberOfKitchens);
                Assert.AreEqual(house2.InternetAvailable, houseResponse2.InternetAvailable);
                Assert.AreEqual(house2.CableTvAvailable, houseResponse2.CableTvAvailable);
                Assert.AreEqual(house2.PropertyType, houseResponse2.PropertyType.ToString());
                Assert.AreEqual(house2.MonthlyRent, houseResponse2.MonthlyRent);
                Assert.AreEqual(Math.Round(37.4220459, 1), Math.Round(houseResponse2.Location.Latitude, 1));
                Assert.AreEqual(Math.Round(-122.0841477, 1), Math.Round(houseResponse2.Location.Longitude, 1));

                // Remove the house instance
                DeleteHouse(houseResponse1.Id);
                DeleteHouse(houseResponse2.Id);
            }
        }

        private void DeleteHouse(string id)
        {
            HouseController houseController = (HouseController)ContextRegistry.GetContext()["HouseController"];
            houseController.Delete(id);
        }
    }
}
