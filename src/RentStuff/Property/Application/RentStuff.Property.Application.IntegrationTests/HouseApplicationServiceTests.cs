using System;
using System.Collections.Generic;
using System.Configuration;
using NUnit.Framework;
using RentStuff.Common;
using RentStuff.Property.Application.HouseServices;
using RentStuff.Property.Application.HouseServices.Commands;
using RentStuff.Property.Application.HouseServices.Representation;
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
            string title = "Bellagio";
            string area = "1600+Amphitheatre+Parkway,+Mountain+View,+CA";
            string dimensionType = "Kanal";
            string dimensionString = "50";
            string ownerName = "Owner Name 1";
            var createNewHouseCommand = new CreateHouseCommand(title, monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
                false, false, true, true, true, true, true, true, "Apartment", email, phoneNumber, houseNo, streetNo, area,
                dimensionType, dimensionString, 0, ownerName);
            string houseCreated = houseApplicationService.SaveNewHouseOffer(createNewHouseCommand);
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseCreated));
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
            string title = "Bellagio";
            string dimensionType = "Kanal";
            string dimensionString = "50";
            string ownerName = "Owner Name 1";

            IGeocodingService geocodingService = (IGeocodingService)ContextRegistry.GetContext()["GeocodingService"];
            Tuple<decimal, decimal> coordinates = geocodingService.GetCoordinatesFromAddress(area);
            Assert.IsNotNull(coordinates);

            decimal latitude = coordinates.Item1;
            decimal longitude = coordinates.Item2;
            var house = new CreateHouseCommand(title, monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
                false, false, true, true, true, true, true, true, "Apartment", email, phoneNumber, houseNo, streetNo, area,
                dimensionType, dimensionString, 0, ownerName);
            string houseCreated = houseApplicationService.SaveNewHouseOffer(house);
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseCreated));

            IList<HouseRepresentation> houses = houseApplicationService.GetHouseByEmail(email);
            HouseRepresentation retreivedHouse = houses[0];

            Assert.NotNull(retreivedHouse);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouse.NumberOfBedrooms);
            Assert.AreEqual(house.PropertyType, retreivedHouse.PropertyType);
            Assert.AreEqual(house.Area, retreivedHouse.Area);
            Assert.AreEqual(house.MonthlyRent, retreivedHouse.Rent);
            Assert.AreEqual(house.Title, retreivedHouse.Title);
            Assert.AreEqual(house.DimensionStringValue + " " + house.DimensionType, retreivedHouse.Dimension);
            Assert.AreEqual(house.OwnerName, retreivedHouse.OwnerName);
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
            string title = "Bellagio";
            string dimensionType = "Kanal";
            string dimensionString = "50";
            string ownerName = "Owner Name 1";

            var house = new CreateHouseCommand(title, monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
            false, false, true, true, true, true, true, true, "Apartment", email, phoneNumber, houseNo, streetNo, area, 
            dimensionType, dimensionString, 0, ownerName);
            houseApplicationService.SaveNewHouseOffer(house);

            IGeocodingService geocodingService = (IGeocodingService)ContextRegistry.GetContext()["GeocodingService"];
            Tuple<decimal, decimal> coordinates = geocodingService.GetCoordinatesFromAddress(area);
            Assert.IsNotNull(coordinates);

            IList<HouseRepresentation> retreivedHouses = houseApplicationService.SearchHousesByAddress(area);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(1, retreivedHouses.Count);

            HouseRepresentation retreivedHouse = retreivedHouses[0];
            Assert.NotNull(retreivedHouse);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouse.NumberOfBedrooms);
            Assert.AreEqual(house.PropertyType, retreivedHouse.PropertyType);
            Assert.AreEqual(house.Area, retreivedHouse.Area);
            Assert.AreEqual(house.MonthlyRent, retreivedHouse.Rent);
            Assert.AreEqual(house.Title, retreivedHouse.Title);
            Assert.AreEqual(house.OwnerName, retreivedHouse.OwnerName);
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
            string title = "Bellagio";
            string dimensionType = "Kanal";
            string dimensionString = "50";
            string ownerName = "Owner Name 1";

            var house = new CreateHouseCommand(title, monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
            false, false, true, true, true, true, true, true, "Apartment", email, phoneNumber, houseNo, streetNo, area, 
            dimensionType, dimensionString, 0, ownerName);
            houseApplicationService.SaveNewHouseOffer(house);

            IGeocodingService geocodingService = (IGeocodingService)ContextRegistry.GetContext()["GeocodingService"];
            Tuple<decimal, decimal> coordinates = geocodingService.GetCoordinatesFromAddress(area);
            Assert.IsNotNull(coordinates);

            area = "Kremlin,+Moscow,+Russia";
            IList<HouseRepresentation> retreivedHouses = houseApplicationService.SearchHousesByAddress(area);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(0, retreivedHouses.Count);
        }
    }
}
