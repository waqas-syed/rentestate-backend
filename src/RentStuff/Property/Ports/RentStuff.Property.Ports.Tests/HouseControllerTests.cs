using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Results;
using NUnit.Framework;
using RentStuff.Common;
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
        private DatabaseUtility _databaseUtility;

        [SetUp]
        public void Setup()
        {
            var connection = ConfigurationManager.ConnectionStrings["MySql"].ToString();
            _databaseUtility = new DatabaseUtility(connection);
            _databaseUtility.Create();
            //_databaseUtility.Populate();
        }

        [TearDown]
        public void Teardown()
        {
            _databaseUtility.Create();
        }

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
                House retreivedHouse = houseList[0];
                Assert.NotNull(retreivedHouse);
                Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
                Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
                Assert.AreEqual(house.NumberOfBedrooms, retreivedHouse.NumberOfBedrooms);
                Assert.AreEqual(house.NumberOfKitchens, retreivedHouse.NumberOfKitchens);
                Assert.AreEqual(house.FamiliesOnly, retreivedHouse.FamiliesOnly);
                Assert.AreEqual(house.GarageAvailable, retreivedHouse.LandlinePhoneAvailable);
                Assert.AreEqual(house.SmokingAllowed, retreivedHouse.SmokingAllowed);
                Assert.AreEqual(house.InternetAvailable, retreivedHouse.InternetAvailable);
                Assert.AreEqual(Enum.Parse(typeof(PropertyType), house.PropertyType), retreivedHouse.PropertyType);
                Assert.AreEqual(decimal.Round(37.4224504M, 5), decimal.Round(retreivedHouse.Latitude, 5));
                Assert.AreEqual(decimal.Round(-122.0840859M, 5), decimal.Round(retreivedHouse.Longitude, 5));
                Assert.AreEqual(house.HouseNo, retreivedHouse.HouseNo);
                Assert.AreEqual(house.Area, retreivedHouse.Area);
                Assert.AreEqual(house.StreetNo, retreivedHouse.StreetNo);
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
                House retreivedHouse1 = houseList[0];
                Assert.NotNull(retreivedHouse1);
                Assert.AreEqual(house1.NumberOfBathrooms, retreivedHouse1.NumberOfBathrooms);
                Assert.AreEqual(house1.NumberOfBathrooms, retreivedHouse1.NumberOfBathrooms);
                Assert.AreEqual(house1.NumberOfBedrooms, retreivedHouse1.NumberOfBedrooms);
                Assert.AreEqual(house1.NumberOfKitchens, retreivedHouse1.NumberOfKitchens);
                Assert.AreEqual(house1.FamiliesOnly, retreivedHouse1.FamiliesOnly);
                Assert.AreEqual(house1.GarageAvailable, retreivedHouse1.LandlinePhoneAvailable);
                Assert.AreEqual(house1.SmokingAllowed, retreivedHouse1.SmokingAllowed);
                Assert.AreEqual(house1.InternetAvailable, retreivedHouse1.InternetAvailable);
                Assert.AreEqual(Enum.Parse(typeof(PropertyType), house1.PropertyType), retreivedHouse1.PropertyType);
                Assert.AreEqual(decimal.Round(37.4224504M, 5), decimal.Round(retreivedHouse1.Latitude, 5));
                Assert.AreEqual(decimal.Round(-122.0840859M, 5), decimal.Round(retreivedHouse1.Longitude, 5));
                Assert.AreEqual(house1.HouseNo, retreivedHouse1.HouseNo);
                Assert.AreEqual(house1.Area, retreivedHouse1.Area);
                Assert.AreEqual(house1.StreetNo, retreivedHouse1.StreetNo);

                // Checking assertions on House # 2
                House retreivedHouse2 = houseList[1];
                Assert.NotNull(retreivedHouse2);
                Assert.AreEqual(house2.NumberOfBathrooms, retreivedHouse2.NumberOfBathrooms);
                Assert.AreEqual(house2.NumberOfBathrooms, retreivedHouse2.NumberOfBathrooms);
                Assert.AreEqual(house2.NumberOfBedrooms, retreivedHouse2.NumberOfBedrooms);
                Assert.AreEqual(house2.NumberOfKitchens, retreivedHouse2.NumberOfKitchens);
                Assert.AreEqual(house2.FamiliesOnly, retreivedHouse2.FamiliesOnly);
                Assert.AreEqual(house2.GarageAvailable, retreivedHouse2.LandlinePhoneAvailable);
                Assert.AreEqual(house2.SmokingAllowed, retreivedHouse2.SmokingAllowed);
                Assert.AreEqual(house2.InternetAvailable, retreivedHouse2.InternetAvailable);
                Assert.AreEqual(Enum.Parse(typeof(PropertyType), house2.PropertyType), retreivedHouse2.PropertyType);
                Assert.AreEqual(decimal.Round(37.4224504M, 5), decimal.Round(retreivedHouse2.Latitude, 5));
                Assert.AreEqual(decimal.Round(-122.0840859M, 5), decimal.Round(retreivedHouse2.Longitude, 5));
                Assert.AreEqual(house2.HouseNo, retreivedHouse2.HouseNo);
                Assert.AreEqual(house2.Area, retreivedHouse2.Area);
                Assert.AreEqual(house2.StreetNo, retreivedHouse2.StreetNo);
            }
        }

        [Test]
        public void SearchHousesByAddressSuccessTest_TestsIfTheHousesInTheSorroundingsAreReturnedGivenOneAddress_VeririfesThroughTheReturnValue()
        {
            if (_runTests)
            {
                HouseController houseController = (HouseController)ContextRegistry.GetContext()["HouseController"];
                Assert.NotNull(houseController);

                string ownerEmail = "thorin@oakenshield123.com";
                string ownerPhoneNumber = "+923211234567";
                string houseNo = "CT-141/A";
                string streetNo = "14";
                string area = "1600 Amphitheatre Parkway, Mountain View, CA";

                CreateHouseCommand house = new CreateHouseCommand(105000, 2, 2, 2, false, true, false, true, true,
                    true, true, true, "Apartment", ownerEmail, ownerPhoneNumber, houseNo, streetNo, area);
                houseController.Post(house);
                IHttpActionResult httpActionResult = houseController.Get(null, area);
                IList<House> houseList = ((OkNegotiatedContentResult<IList<House>>)httpActionResult).Content;
                Assert.NotNull(houseList);
                Assert.AreEqual(1, houseList.Count);
                House retreivedHouse = houseList[0];
                Assert.NotNull(retreivedHouse);
                Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
                Assert.AreEqual(house.NumberOfBedrooms, retreivedHouse.NumberOfBedrooms);
                Assert.AreEqual(house.NumberOfKitchens, retreivedHouse.NumberOfKitchens);
                Assert.AreEqual(house.FamiliesOnly, retreivedHouse.FamiliesOnly);
                Assert.AreEqual(house.GarageAvailable, retreivedHouse.LandlinePhoneAvailable);
                Assert.AreEqual(house.SmokingAllowed, retreivedHouse.SmokingAllowed);
                Assert.AreEqual(house.InternetAvailable, retreivedHouse.InternetAvailable);
                Assert.AreEqual(Enum.Parse(typeof(PropertyType), house.PropertyType), retreivedHouse.PropertyType);
                Assert.AreEqual(decimal.Round(37.4224504M, 5), decimal.Round(retreivedHouse.Latitude, 5));
                Assert.AreEqual(decimal.Round(-122.0840859M, 5), decimal.Round(retreivedHouse.Longitude, 5));
                Assert.AreEqual(house.HouseNo, retreivedHouse.HouseNo);
                Assert.AreEqual(house.Area, retreivedHouse.Area);
                Assert.AreEqual(house.StreetNo, retreivedHouse.StreetNo);
            }
        }
    }
}
