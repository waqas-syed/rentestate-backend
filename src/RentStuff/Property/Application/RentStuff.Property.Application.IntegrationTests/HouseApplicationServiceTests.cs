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
            string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
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
                dimensionType, dimensionString, 0, ownerName, description);
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
            string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
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
                dimensionType, dimensionString, 0, ownerName, description);
            string houseCreated = houseApplicationService.SaveNewHouseOffer(house);
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseCreated));

            IList<HousePartialRepresentation> houses = houseApplicationService.GetHouseByEmail(email);
            HousePartialRepresentation retreivedHouse = houses[0];

            Assert.NotNull(retreivedHouse);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouse.NumberOfBedrooms);
            Assert.AreEqual(house.PropertyType, retreivedHouse.PropertyType);
            Assert.AreEqual(house.Area, retreivedHouse.Area);
            Assert.AreEqual(house.MonthlyRent, retreivedHouse.Rent);
            Assert.AreEqual(house.Title, retreivedHouse.Title);
            Assert.AreEqual(house.Description, retreivedHouse.Description);
            Assert.AreEqual(house.DimensionStringValue + " " + house.DimensionType, retreivedHouse.Dimension);
            Assert.AreEqual(house.OwnerName, retreivedHouse.OwnerName);
        }

        [Test]
        public void SearchHousesByPropertyTypeOnlyTest_TestsThatANewHouseIsSavedInTheDatabaseAndRetreivedAsExpected_VerifiesByOutput()
        {
            IHouseApplicationService houseApplicationService =
                (IHouseApplicationService)ContextRegistry.GetContext()["HouseApplicationService"];

            // Saving House # 1
            string email = "w@12344321.com";
            string phoneNumber = "+923331234567";
            string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
            long monthlyRent = 130000;
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            string houseNo = "747";
            string streetNo = "13";
            string area = "Pindora, Rawalpindi, Pakistan";
            string title = "Bellagio";
            string dimensionType = "Kanal";
            string dimensionString = "50";
            string ownerName = "Owner Name 1";
            string propertyType1 = "Apartment";

            var house = new CreateHouseCommand(title, monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
                false, false, true, true, true, true, true, true, propertyType1, email, phoneNumber, houseNo, streetNo, area,
                dimensionType, dimensionString, 0, ownerName, description);
            string houseCreated = houseApplicationService.SaveNewHouseOffer(house);
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseCreated));

            // Saving Hosue # 2
            string email2 = "w@12344321-2.com";
            string phoneNumber2 = "+920030000002";
            string description2 = "Erebor - 2. Built deep within the mountain itself the beauty of this fortress was legend.";
            long monthlyRent2 = 1300002;
            int numberOfBedrooms2 = 2;
            int numberofBathrooms2 = 2;
            int numberOfKitchens2 = 2;
            string houseNo2 = "747-2";
            string streetNo2 = "13-2";
            string area2 = "Pindora, Rawalpindi, Pakistan";
            string title2 = "Bellagio";
            string dimensionType2 = "Kanal";
            string dimensionString2 = "50";
            string ownerName2 = "Owner Name 2";
            string propertyType2 = "House";
            
            var house2 = new CreateHouseCommand(title2, monthlyRent2, numberOfBedrooms2, numberOfKitchens2, numberofBathrooms2,
                false, false, true, true, true, true, true, true, propertyType2, email2, phoneNumber2, houseNo2, streetNo2, area2,
                dimensionType2, dimensionString2, 0, ownerName2, description2);
            string houseCreated2 = houseApplicationService.SaveNewHouseOffer(house2);
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseCreated2));

            IList<HousePartialRepresentation> houses = houseApplicationService.SearchHousesByPropertyType(propertyType2);
            Assert.AreEqual(1, houses.Count);
            HousePartialRepresentation retreivedHouse = houses[0];

            Assert.NotNull(retreivedHouse);
            Assert.AreEqual(house2.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(house2.NumberOfBedrooms, retreivedHouse.NumberOfBedrooms);
            Assert.AreEqual(house2.PropertyType, retreivedHouse.PropertyType);
            Assert.AreEqual(house2.Area, retreivedHouse.Area);
            Assert.AreEqual(house2.MonthlyRent, retreivedHouse.Rent);
            Assert.AreEqual(house2.Title, retreivedHouse.Title);
            Assert.AreEqual(house2.Description, retreivedHouse.Description);
            Assert.AreEqual(house2.DimensionStringValue + " " + house2.DimensionType, retreivedHouse.Dimension);
            Assert.AreEqual(house2.OwnerName, retreivedHouse.OwnerName);
        }

        [Test]
        public void SearchHousesByAreaOnlyTest_TestsThatANewHouseIsSavedInTheDatabaseAndRetreivedAsExpected_VerifiesByOutput()
        {
            IHouseApplicationService houseApplicationService =
                (IHouseApplicationService)ContextRegistry.GetContext()["HouseApplicationService"];

            // Saving House # 1
            string email = "w@12344321.com";
            string phoneNumber = "+923331234567";
            string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
            long monthlyRent = 130000;
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            string houseNo = "747";
            string streetNo = "13";
            string area = "Pindora, Rawalpindi, Pakistan";
            string title = "Bellagio";
            string dimensionType = "Kanal";
            string dimensionString = "50";
            string ownerName = "Owner Name 1";
            string propertyType1 = "House";

            var house = new CreateHouseCommand(title, monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
                false, false, true, true, true, true, true, true, propertyType1, email, phoneNumber, houseNo, streetNo, area,
                dimensionType, dimensionString, 0, ownerName, description);
            string houseCreated = houseApplicationService.SaveNewHouseOffer(house);
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseCreated));

            // Saving Hosue # 2
            string email2 = "w@12344321-2.com";
            string phoneNumber2 = "+920030000002";
            string description2 = "Erebor - 2. Built deep within the mountain itself the beauty of this fortress was legend.";
            long monthlyRent2 = 1300002;
            int numberOfBedrooms2 = 2;
            int numberofBathrooms2 = 2;
            int numberOfKitchens2 = 2;
            string houseNo2 = "747-2";
            string streetNo2 = "13-2";
            string area2 = "Saddar, Rawalpindi, Pakistan";
            string title2 = "Bellagio";
            string dimensionType2 = "Kanal";
            string dimensionString2 = "50";
            string ownerName2 = "Owner Name 2";
            string propertyType2 = "House";

            var house2 = new CreateHouseCommand(title2, monthlyRent2, numberOfBedrooms2, numberOfKitchens2, numberofBathrooms2,
                false, false, true, true, true, true, true, true, propertyType2, email2, phoneNumber2, houseNo2, streetNo2, area2,
                dimensionType2, dimensionString2, 0, ownerName2, description2);
            string houseCreated2 = houseApplicationService.SaveNewHouseOffer(house2);
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseCreated2));

            IList<HousePartialRepresentation> houses = houseApplicationService.SearchHousesByArea(area);
            Assert.AreEqual(1, houses.Count);
            HousePartialRepresentation retreivedHouse = houses[0];

            Assert.NotNull(retreivedHouse);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouse.NumberOfBedrooms);
            Assert.AreEqual(house.PropertyType, retreivedHouse.PropertyType);
            Assert.AreEqual(house.Area, retreivedHouse.Area);
            Assert.AreEqual(house.MonthlyRent, retreivedHouse.Rent);
            Assert.AreEqual(house.Title, retreivedHouse.Title);
            Assert.AreEqual(house.Description, retreivedHouse.Description);
            Assert.AreEqual(house.DimensionStringValue + " " + house2.DimensionType, retreivedHouse.Dimension);
            Assert.AreEqual(house.OwnerName, retreivedHouse.OwnerName);
        }

        [Test]
        public void SaveHouseAndGetHouseByIdTest_TestsThatANewHouseIsSavedInTheDatabaseAsExpected_VerifiesByOutput()
        {
            IHouseApplicationService houseApplicationService =
                (IHouseApplicationService)ContextRegistry.GetContext()["HouseApplicationService"];

            string email = "w@12344321.com";
            string phoneNumber = "+923331234567";
            string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
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
                dimensionType, dimensionString, 0, ownerName, description);
            string houseId = houseApplicationService.SaveNewHouseOffer(house);
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseId));

            HouseFullRepresentation retreivedHouse = houseApplicationService.GetHouseById(houseId);

            Assert.NotNull(retreivedHouse);
            Assert.AreEqual(house.Title, retreivedHouse.Title);
            Assert.AreEqual(house.Description, retreivedHouse.Description);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouse.NumberOfBedrooms);
            Assert.AreEqual(house.NumberOfKitchens, retreivedHouse.NumberOfKitchens);
            Assert.AreEqual(house.FamiliesOnly, retreivedHouse.FamiliesOnly);
            Assert.AreEqual(house.GarageAvailable, retreivedHouse.LandlinePhoneAvailable);
            Assert.AreEqual(house.SmokingAllowed, retreivedHouse.SmokingAllowed);
            Assert.AreEqual(house.InternetAvailable, retreivedHouse.InternetAvailable);
            Assert.AreEqual(house.PropertyType, retreivedHouse.PropertyType);
            Assert.AreEqual(house.HouseNo, retreivedHouse.HouseNo);
            Assert.AreEqual(house.Area, retreivedHouse.Area);
            Assert.AreEqual(house.StreetNo, retreivedHouse.StreetNo);
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
            string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
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
            dimensionType, dimensionString, 0, ownerName, description);
            houseApplicationService.SaveNewHouseOffer(house);

            IGeocodingService geocodingService = (IGeocodingService)ContextRegistry.GetContext()["GeocodingService"];
            Tuple<decimal, decimal> coordinates = geocodingService.GetCoordinatesFromAddress(area);
            Assert.IsNotNull(coordinates);

            IList<HousePartialRepresentation> retreivedHouses = houseApplicationService.SearchHousesByArea(area);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(1, retreivedHouses.Count);

            HousePartialRepresentation retreivedHouse = retreivedHouses[0];
            Assert.NotNull(retreivedHouse);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouse.NumberOfBedrooms);
            Assert.AreEqual(house.PropertyType, retreivedHouse.PropertyType);
            Assert.AreEqual(house.Area, retreivedHouse.Area);
            Assert.AreEqual(house.MonthlyRent, retreivedHouse.Rent);
            Assert.AreEqual(house.Title, retreivedHouse.Title);
            Assert.AreEqual(house.Description, retreivedHouse.Description);
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
            string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
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
            dimensionType, dimensionString, 0, ownerName, description);
            houseApplicationService.SaveNewHouseOffer(house);

            IGeocodingService geocodingService = (IGeocodingService)ContextRegistry.GetContext()["GeocodingService"];
            Tuple<decimal, decimal> coordinates = geocodingService.GetCoordinatesFromAddress(area);
            Assert.IsNotNull(coordinates);

            area = "Kremlin,+Moscow,+Russia";
            IList<HousePartialRepresentation> retreivedHouses = houseApplicationService.SearchHousesByArea(area);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(0, retreivedHouses.Count);
        }
    }
}
