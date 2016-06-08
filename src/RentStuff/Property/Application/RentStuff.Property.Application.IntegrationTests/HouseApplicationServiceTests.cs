using System;
using System.Collections.Generic;
using System.Configuration;
using NUnit.Framework;
using RentStuff.Common;
using RentStuff.Property.Application.HouseServices;
using RentStuff.Property.Application.HouseServices.Commands;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Domain.Model.Services;
using Spring.Context.Support;

namespace RentStuff.Property.Application.IntegrationTests
{
    [TestFixture]
    public class HouseApplicationServiceTests
    {
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
        public void SaveHouseTest_TestsThatANewHouseIsSavedInTheDatabaseAsExpected_VerifiesByOutput()
        {
            IHouseApplicationService houseApplicationService =
                (IHouseApplicationService) ContextRegistry.GetContext()["HouseApplicationService"];
            
            string email = "w@12344321.com";
            string phoneNumber = "+923331234567";
            long monthlyRent = 130000;
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            string houseNo = "747";
            string streetNo = "13";
            string area = "1600+Amphitheatre+Parkway,+Mountain+View,+CA";
            var createNewHouseCommand = new CreateHouseCommand(monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
                false, false, true, true, true, true, true, true, "Apartment", email, phoneNumber, houseNo, streetNo, area, null, null, 0);
            bool houseCreated = houseApplicationService.SaveNewHouseOffer(createNewHouseCommand);
            Assert.IsTrue(houseCreated);
        }

        [Test]
        public void SaveHouseAndRetrieveTest_TestsThatANewHouseIsSavedInTheDatabaseAsExpected_VerifiesByOutput()
        {
            IHouseApplicationService houseApplicationService =
                (IHouseApplicationService)ContextRegistry.GetContext()["HouseApplicationService"];

            string email = "w@12344321.com";
            string phoneNumber = "+923331234567";
            long monthlyRent = 130000;
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            string houseNo = "747";
            string streetNo = "13";
            string area = "1600+Amphitheatre+Parkway,+Mountain+View,+CA";

            IGeocodingService geocodingService = (IGeocodingService)ContextRegistry.GetContext()["GeocodingService"];
            Tuple<decimal, decimal> coordinates = geocodingService.GetCoordinatesFromAddress(area);
            Assert.IsNotNull(coordinates);

            decimal latitude = coordinates.Item1;
            decimal longitude = coordinates.Item2;
            var house = new CreateHouseCommand(monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
                false, false, true, true, true, true, true, true, "Apartment", email, phoneNumber, houseNo, streetNo, area,
                null, null, 0);
            bool houseCreated = houseApplicationService.SaveNewHouseOffer(house);
            Assert.IsTrue(houseCreated);

            IList<House> houses = houseApplicationService.GetHouseByEmail(email);
            House retreivedHouse = houses[0];

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
            Assert.AreEqual(decimal.Round(latitude, 5), decimal.Round(retreivedHouse.Latitude, 5));
            Assert.AreEqual(decimal.Round(longitude, 5), decimal.Round(retreivedHouse.Longitude, 5));
            Assert.AreEqual(house.HouseNo, retreivedHouse.HouseNo);
            Assert.AreEqual(house.Area, retreivedHouse.Area);
            Assert.AreEqual(house.StreetNo, retreivedHouse.StreetNo);
        }

        [Test]
        public void SearchHousesByAddressSuccessTest_GetsHousesGivenTheAddress_VerifiesThroughDatabaseRetreival()
        {
            string area = "1600+Amphitheatre+Parkway,+Mountain+View,+CA";

            IHouseApplicationService houseApplicationService =
                (IHouseApplicationService)ContextRegistry.GetContext()["HouseApplicationService"];

            string email = "w@12344321.com";
            string phoneNumber = "+923331234567";
            long monthlyRent = 130000;
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            string houseNo = "747";
            string streetNo = "13";
            var house = new CreateHouseCommand(monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
            false, false, true, true, true, true, true, true, "Apartment", email, phoneNumber, houseNo, streetNo, area, null, null,
            0);
            houseApplicationService.SaveNewHouseOffer(house);

            IGeocodingService geocodingService = (IGeocodingService)ContextRegistry.GetContext()["GeocodingService"];
            Tuple<decimal, decimal> coordinates = geocodingService.GetCoordinatesFromAddress(area);
            Assert.IsNotNull(coordinates);

            IList<House> retreivedHouses = houseApplicationService.SearchHousesByAddress(area);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(1, retreivedHouses.Count);

            House retreivedHouse = retreivedHouses[0];
            Assert.NotNull(retreivedHouse);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouse.NumberOfBedrooms);
            Assert.AreEqual(house.NumberOfKitchens, retreivedHouse.NumberOfKitchens);
            Assert.AreEqual(house.FamiliesOnly, retreivedHouse.FamiliesOnly);
            Assert.AreEqual(house.GarageAvailable, retreivedHouse.LandlinePhoneAvailable);
            Assert.AreEqual(house.SmokingAllowed, retreivedHouse.SmokingAllowed);
            Assert.AreEqual(house.InternetAvailable, retreivedHouse.InternetAvailable);
            Assert.AreEqual(Enum.Parse(typeof(PropertyType), house.PropertyType), retreivedHouse.PropertyType);
            Assert.AreEqual(decimal.Round(coordinates.Item1, 5), decimal.Round(retreivedHouse.Latitude, 5));
            Assert.AreEqual(decimal.Round(coordinates.Item2, 5), decimal.Round(retreivedHouse.Longitude, 5));
            Assert.AreEqual(house.HouseNo, retreivedHouse.HouseNo);
            Assert.AreEqual(house.Area, retreivedHouse.Area);
            Assert.AreEqual(house.StreetNo, retreivedHouse.StreetNo);
        }

        [Test]
        public void SearchHousesByAddressFailTest_GetsHousesGivenTheAddress_VerifiesThroughDatabaseRetreival()
        {
            // Test fails because we search for a different address than the one we saved
            string area = "1600+Amphitheatre+Parkway,+Mountain+View,+CA";

            IHouseApplicationService houseApplicationService =
                (IHouseApplicationService)ContextRegistry.GetContext()["HouseApplicationService"];

            string email = "w@12344321.com";
            string phoneNumber = "+923331234567";
            long monthlyRent = 130000;
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            string houseNo = "747";
            string streetNo = "13";
            var house = new CreateHouseCommand(monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
            false, false, true, true, true, true, true, true, "Apartment", email, phoneNumber, houseNo, streetNo, area, null,
            null, 0);
            houseApplicationService.SaveNewHouseOffer(house);

            IGeocodingService geocodingService = (IGeocodingService)ContextRegistry.GetContext()["GeocodingService"];
            Tuple<decimal, decimal> coordinates = geocodingService.GetCoordinatesFromAddress(area);
            Assert.IsNotNull(coordinates);

            area = "Kremlin,+Moscow,+Russia";
            IList<House> retreivedHouses = houseApplicationService.SearchHousesByAddress(area);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(0, retreivedHouses.Count);
        }
    }
}
