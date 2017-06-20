using System;
using System.Collections.Generic;
using System.Configuration;
using Ninject;
using NUnit.Framework;
using RentStuff.Common;
using RentStuff.Common.Ninject.Modules;
using RentStuff.Common.Utilities;
using RentStuff.Property.Application.HouseServices;
using RentStuff.Property.Application.HouseServices.Commands;
using RentStuff.Property.Application.HouseServices.Representation;
using RentStuff.Property.Application.Ninject.Modules;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Infrastructure.Persistence.Ninject.Modules;

namespace RentStuff.Property.Application.IntegrationTests
{
    [TestFixture]
    public class HouseApplicationServiceTests
    {
        private DatabaseUtility _databaseUtility;
        private IKernel _kernel;

        [SetUp]
        public void Setup()
        {
            var connection = StringCipher.DecipheredConnectionString;
            _databaseUtility = new DatabaseUtility(connection);
            _databaseUtility.Create();
            //_databaseUtility.Populate();
            //NhConnectionDecipherService.SetupDecipheredConnectionString();
            _kernel = new StandardKernel();
            
            _kernel.Load<PropertyPersistenceNinjectModule>();
            _kernel.Load<CommonNinjectModule>();
            _kernel.Load<PropertyApplicationNinjectModule>();
        }

        [TearDown]
        public void Teardown()
        {
            _databaseUtility.Create();
        }

        [Test]
        public void SaveHouseTest_TestsThatANewHouseIsSavedInTheDatabaseAsExpected_VerifiesByOutput()
        {
            IHouseApplicationService houseApplicationService = _kernel.Get<IHouseApplicationService>();
            
            string email = "w@12344321.com";
            string phoneNumber = "01234567890";
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
            string propertyType = "Apartment";
            string genderRestriction = GenderRestriction.GirlsOnly.ToString();
            bool smokingAllowed = false;
            bool landline = true;
            bool cableTv = false;
            bool internet = true;
            bool garage = true;
            var createNewHouseCommand = new CreateHouseCommand(title, monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
                internet, landline, cableTv, garage, smokingAllowed, propertyType, email, phoneNumber, houseNo, streetNo, area,
                dimensionType, dimensionString, 0, ownerName, description, genderRestriction);
            string houseCreated = houseApplicationService.SaveNewHouseOffer(createNewHouseCommand);
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseCreated));
            Assert.AreEqual(email, createNewHouseCommand.OwnerEmail);
            Assert.AreEqual(phoneNumber, createNewHouseCommand.OwnerPhoneNumber);
            Assert.AreEqual(ownerName, createNewHouseCommand.OwnerName);
            Assert.AreEqual(description, createNewHouseCommand.Description);
            Assert.AreEqual(houseNo, createNewHouseCommand.HouseNo);
            Assert.AreEqual(streetNo, createNewHouseCommand.StreetNo);
            Assert.AreEqual(title, createNewHouseCommand.Title);
            Assert.AreEqual(area, createNewHouseCommand.Area);
            Assert.AreEqual(dimensionType, createNewHouseCommand.DimensionType);
            Assert.AreEqual(dimensionString, createNewHouseCommand.DimensionStringValue);
            Assert.AreEqual(dimensionType, createNewHouseCommand.DimensionType);
            Assert.AreEqual(numberofBathrooms, createNewHouseCommand.NumberOfBathrooms);
            Assert.AreEqual(numberOfBedrooms, createNewHouseCommand.NumberOfBedrooms);
            Assert.AreEqual(numberOfKitchens, createNewHouseCommand.NumberOfKitchens);
            Assert.AreEqual(propertyType, createNewHouseCommand.PropertyType);
            Assert.AreEqual(monthlyRent, createNewHouseCommand.MonthlyRent);
            Assert.AreEqual(numberofBathrooms, createNewHouseCommand.NumberOfBathrooms);
            Assert.AreEqual(genderRestriction, createNewHouseCommand.GenderRestriction);
            Assert.AreEqual(internet, createNewHouseCommand.InternetAvailable);
            Assert.AreEqual(garage, createNewHouseCommand.GarageAvailable);
            Assert.AreEqual(landline, createNewHouseCommand.LandlinePhoneAvailable);
            Assert.AreEqual(cableTv, createNewHouseCommand.CableTvAvailable);
            Assert.AreEqual(smokingAllowed, createNewHouseCommand.SmokingAllowed);
        }

        [Test]
        public void SaveHouseAndRetrieveTest_TestsThatANewHouseIsSavedInTheDatabaseAsExpected_VerifiesByOutput()
        {
            IHouseApplicationService houseApplicationService = _kernel.Get<IHouseApplicationService>();

            string email = "w@12344321.com";
            string phoneNumber = "01234567890";
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
            string propertyType = "Apartment";
            string genderRestriction = GenderRestriction.GirlsOnly.ToString();

            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService = _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();
            Tuple<decimal, decimal> coordinates = geocodingService.GetCoordinatesFromAddress(area);
            Assert.IsNotNull(coordinates);

            decimal latitude = coordinates.Item1;
            decimal longitude = coordinates.Item2;
            var house = new CreateHouseCommand(title, monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
                true, true, true, true, true, "Apartment", email, phoneNumber, houseNo, streetNo, area,
                dimensionType, dimensionString, 0, ownerName, description, genderRestriction);
            string houseCreated = houseApplicationService.SaveNewHouseOffer(house);
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseCreated));

            IList<HousePartialRepresentation> houses = houseApplicationService.GetHouseByEmail(email);
            HousePartialRepresentation retreivedHouse = houses[0];

            Assert.NotNull(retreivedHouse);
            Assert.AreEqual(houseCreated, retreivedHouse.HouseId);
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
        public void SearchHousesByAddressAndPropertyTypeTest_TestsThatANewHouseIsSavedInTheDatabaseAndRetreivedAsExpected_VerifiesByOutput()
        {
            IHouseApplicationService houseApplicationService = _kernel.Get<IHouseApplicationService>();

            // Saving House # 1 : Wont appear in search results as the PropertyType is Apartment; search criteria is House
            string email = "w@12344321.com";
            string phoneNumber = "01234567890";
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
            string genderRestriction = GenderRestriction.BoysOnly.ToString();

            var house = new CreateHouseCommand(title, monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
                true, true, true, true, true, propertyType1, email, phoneNumber, houseNo, streetNo, area,
                dimensionType, dimensionString, 0, ownerName, description, genderRestriction);
            string houseCreated = houseApplicationService.SaveNewHouseOffer(house);
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseCreated));

            // Saving House # 2 : Will appear in search results as the PropertyType is House; search criteria is House
            string email2 = "w@12344321-2.com";
            string phoneNumber2 = "01234567891";
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
            string genderRestriction2 = GenderRestriction.GirlsOnly.ToString();

            var house2 = new CreateHouseCommand(title2, monthlyRent2, numberOfBedrooms2, numberOfKitchens2, numberofBathrooms2,
                true, true, true, true, true, propertyType2, email2, phoneNumber2, houseNo2, streetNo2, area2,
                dimensionType2, dimensionString2, 0, ownerName2, description2, genderRestriction2);
            string houseCreated2 = houseApplicationService.SaveNewHouseOffer(house2);
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseCreated2));

            // Saving House # 3 : Wont appear in search results as the PropertyType is Hostel; search criteria is House
            string email3 = "w@12344321-3.com";
            string phoneNumber3 = "01234567892";
            string description3 = "Erebor - 3. Built deep within the mountain itself the beauty of this fortress was legend.";
            long monthlyRent3 = 1300003;
            int numberOfBedrooms3 = 3;
            int numberofBathrooms3 = 3;
            int numberOfKitchens3 = 3;
            string houseNo3 = "747-3";
            string streetNo3 = "13-3";
            string area3 = "Satellite Town, Rawalpindi, Pakistan";
            string title3 = "Bellagio";
            string dimensionType3 = "Kanal";
            string dimensionString3 = "53";
            string ownerName3 = "Owner Name 3";
            string propertyType3 = "House";
            string genderRestriction3 = GenderRestriction.GirlsOnly.ToString();

            var house3 = new CreateHouseCommand(title3, monthlyRent3, numberOfBedrooms3, numberOfKitchens3, numberofBathrooms3,
                true, true, true, true, true, propertyType3, email3, phoneNumber3, houseNo3, streetNo3, area3,
                dimensionType3, dimensionString3, 0, ownerName3, description3, genderRestriction3);
            string houseCreated3 = houseApplicationService.SaveNewHouseOffer(house3);
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseCreated3));

            IList<HousePartialRepresentation> houses = houseApplicationService.SearchHousesByAreaAndPropertyType(area, propertyType2);
            Assert.AreEqual(2, houses.Count);
            HousePartialRepresentation retreivedHouse = houses[0];

            // Verification of House No. 2
            Assert.NotNull(retreivedHouse);
            Assert.AreEqual(houseCreated2, retreivedHouse.HouseId);
            Assert.AreEqual(house2.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(house2.NumberOfBedrooms, retreivedHouse.NumberOfBedrooms);
            Assert.AreEqual(house2.PropertyType, retreivedHouse.PropertyType);
            Assert.AreEqual(house2.Area, retreivedHouse.Area);
            Assert.AreEqual(house2.MonthlyRent, retreivedHouse.Rent);
            Assert.AreEqual(house2.Title, retreivedHouse.Title);
            Assert.AreEqual(house2.Description, retreivedHouse.Description);
            Assert.AreEqual(house2.DimensionStringValue + " " + house2.DimensionType, retreivedHouse.Dimension);
            Assert.AreEqual(house2.OwnerName, retreivedHouse.OwnerName);

            // Verification of House No. 3
            HousePartialRepresentation retreivedHouse2 = houses[1];
            Assert.NotNull(retreivedHouse2);
            Assert.AreEqual(houseCreated3, retreivedHouse2.HouseId);
            Assert.AreEqual(house3.NumberOfBathrooms, retreivedHouse2.NumberOfBathrooms);
            Assert.AreEqual(house3.NumberOfBedrooms, retreivedHouse2.NumberOfBedrooms);
            Assert.AreEqual(house3.PropertyType, retreivedHouse2.PropertyType);
            Assert.AreEqual(house3.Area, retreivedHouse2.Area);
            Assert.AreEqual(house3.MonthlyRent, retreivedHouse2.Rent);
            Assert.AreEqual(house3.Title, retreivedHouse2.Title);
            Assert.AreEqual(house3.Description, retreivedHouse2.Description);
            Assert.AreEqual(house3.DimensionStringValue + " " + house3.DimensionType, retreivedHouse2.Dimension);
            Assert.AreEqual(house3.OwnerName, retreivedHouse2.OwnerName);
        }

        [Test]
        public void SearchHousesByPropertyTypeOnlyTest_TestsThatANewHouseIsSavedInTheDatabaseAndRetreivedAsExpected_VerifiesByOutput()
        {
            IHouseApplicationService houseApplicationService = _kernel.Get<IHouseApplicationService>();

            // Saving House # 1 : Wont appear in search results as the PropertyType is Apartment; search criteria is House
            string email = "w@12344321.com";
            string phoneNumber = "01234567890";
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
            string genderRestriction1 = GenderRestriction.GirlsOnly.ToString();

            var house = new CreateHouseCommand(title, monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
                true, true, true, true, true, propertyType1, email, phoneNumber, houseNo, streetNo, area,
                dimensionType, dimensionString, 0, ownerName, description, genderRestriction1);
            string houseCreated = houseApplicationService.SaveNewHouseOffer(house);
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseCreated));

            // Saving House # 2 : Will appear in search results as the PropertyType is House; search criteria is House
            string email2 = "w@12344321-2.com";
            string phoneNumber2 = "01234567891";
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
            string genderRestriction2 = GenderRestriction.NoRestriction.ToString();

            var house2 = new CreateHouseCommand(title2, monthlyRent2, numberOfBedrooms2, numberOfKitchens2, numberofBathrooms2,
                true, true, true, true, true, propertyType2, email2, phoneNumber2, houseNo2, streetNo2, area2,
                dimensionType2, dimensionString2, 0, ownerName2, description2, genderRestriction2);
            string houseCreated2 = houseApplicationService.SaveNewHouseOffer(house2);
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseCreated2));

            // Saving House # 3 : Wont appear in search results as the PropertyType is Hostel; search criteria is House
            string email3 = "w@12344321-3.com";
            string phoneNumber3 = "01234567892";
            string description3 = "Erebor - 3. Built deep within the mountain itself the beauty of this fortress was legend.";
            long monthlyRent3 = 1300003;
            int numberOfBedrooms3 = 3;
            int numberofBathrooms3 = 3;
            int numberOfKitchens3 = 3;
            string houseNo3 = "747-3";
            string streetNo3 = "13-3";
            string area3 = "Satellite Town, Rawalpindi, Pakistan";
            string title3 = "Bellagio";
            string dimensionType3 = "Kanal";
            string dimensionString3 = "53";
            string ownerName3 = "Owner Name 3";
            string propertyType3 = "Hostel";
            string genderRestriction3 = GenderRestriction.GirlsOnly.ToString();

            var house3 = new CreateHouseCommand(title3, monthlyRent3, numberOfBedrooms3, numberOfKitchens3, numberofBathrooms3,
                true, true, true, true, true, propertyType3, email3, phoneNumber3, houseNo3, streetNo3, area3,
                dimensionType3, dimensionString3, 0, ownerName3, description3, genderRestriction3);
            string houseCreated3 = houseApplicationService.SaveNewHouseOffer(house3);
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseCreated3));

            IList<HousePartialRepresentation> houses = houseApplicationService.SearchHousesByPropertyType(propertyType2);
            Assert.AreEqual(1, houses.Count);
            HousePartialRepresentation retreivedHouse = houses[0];

            // Verification of House No. 2
            Assert.NotNull(retreivedHouse);
            Assert.AreEqual(houseCreated2, retreivedHouse.HouseId);
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
            IHouseApplicationService houseApplicationService = _kernel.Get<IHouseApplicationService>();

            // Saving House # 1
            string email = "w@12344321.com";
            string phoneNumber = "01234567890";
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
            string genderRestriction1 = GenderRestriction.GirlsOnly.ToString();

            var house = new CreateHouseCommand(title, monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
                true, true, true, true, true, propertyType1, email, phoneNumber, houseNo, streetNo, area,
                dimensionType, dimensionString, 0, ownerName, description, genderRestriction1);
            string houseCreated = houseApplicationService.SaveNewHouseOffer(house);
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseCreated));

            // Saving House # 2
            string email2 = "w@12344321-2.com";
            string phoneNumber2 = "01234567891";
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
            string genderRestriction2 = GenderRestriction.GirlsOnly.ToString();

            var house2 = new CreateHouseCommand(title2, monthlyRent2, numberOfBedrooms2, numberOfKitchens2, numberofBathrooms2,
                true, true, true, true, true, propertyType2, email2, phoneNumber2, houseNo2, streetNo2, area2,
                dimensionType2, dimensionString2, 0, ownerName2, description2, genderRestriction2);
            string houseCreated2 = houseApplicationService.SaveNewHouseOffer(house2);
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseCreated2));

            // Saving House # 3
            string email3 = "w@12344321-3.com";
            string phoneNumber3 = "01234567892";
            string description3 = "Erebor - 3. Built deep within the mountain itself the beauty of this fortress was legend.";
            long monthlyRent3 = 1300003;
            int numberOfBedrooms3 = 3;
            int numberofBathrooms3 = 3;
            int numberOfKitchens3 = 3;
            string houseNo3 = "747-3";
            string streetNo3 = "13-3";
            string area3 = "Satellite Town, Rawalpindi, Pakistan";
            string title3 = "Bellagio";
            string dimensionType3 = "Kanal";
            string dimensionString3 = "53";
            string ownerName3 = "Owner Name 3";
            string propertyType3 = "House";
            string genderRestriction3 = GenderRestriction.NoRestriction.ToString();

            var house3 = new CreateHouseCommand(title3, monthlyRent3, numberOfBedrooms3, numberOfKitchens3, numberofBathrooms3,
                true, true, true, true, true, propertyType3, email3, phoneNumber3, houseNo3, streetNo3, area3,
                dimensionType3, dimensionString3, 0, ownerName3, description3, genderRestriction3);
            string houseCreated3 = houseApplicationService.SaveNewHouseOffer(house3);
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseCreated3));

            IList<HousePartialRepresentation> houses = houseApplicationService.SearchHousesByArea(area);
            Assert.AreEqual(2, houses.Count);

            // Verification of House No. 1
            HousePartialRepresentation retreivedHouse1 = houses[0];

            Assert.NotNull(retreivedHouse1);
            Assert.AreEqual(houseCreated, retreivedHouse1.HouseId);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse1.NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouse1.NumberOfBedrooms);
            Assert.AreEqual(house.NumberOfKitchens, retreivedHouse1.NumberOfKitchens);
            Assert.AreEqual(house.PropertyType, retreivedHouse1.PropertyType);
            Assert.AreEqual(house.Area, retreivedHouse1.Area);
            Assert.AreEqual(house.MonthlyRent, retreivedHouse1.Rent);
            Assert.AreEqual(house.Title, retreivedHouse1.Title);
            Assert.AreEqual(house.Description, retreivedHouse1.Description);
            Assert.AreEqual(house.DimensionStringValue + " " + house2.DimensionType, retreivedHouse1.Dimension);
            Assert.AreEqual(house.OwnerName, retreivedHouse1.OwnerName);
            Assert.AreEqual(house.OwnerEmail, retreivedHouse1.OwnerEmail);
            Assert.AreEqual(house.OwnerPhoneNumber, retreivedHouse1.OwnerPhoneNumber);

            // Verification of House No. 3
            HousePartialRepresentation retreivedHouse2 = houses[1];

            Assert.NotNull(retreivedHouse2);
            Assert.AreEqual(houseCreated3, retreivedHouse2.HouseId);
            Assert.AreEqual(house3.NumberOfBathrooms, retreivedHouse2.NumberOfBathrooms);
            Assert.AreEqual(house3.NumberOfBedrooms, retreivedHouse2.NumberOfBedrooms);
            Assert.AreEqual(house3.NumberOfKitchens, retreivedHouse2.NumberOfKitchens);
            Assert.AreEqual(house3.PropertyType, retreivedHouse1.PropertyType);
            Assert.AreEqual(house3.Area, retreivedHouse2.Area);
            Assert.AreEqual(house3.MonthlyRent, retreivedHouse2.Rent);
            Assert.AreEqual(house3.Title, retreivedHouse2.Title);
            Assert.AreEqual(house3.Description, retreivedHouse2.Description);
            Assert.AreEqual(house3.DimensionStringValue + " " + house3.DimensionType, retreivedHouse2.Dimension);
            Assert.AreEqual(house3.OwnerName, retreivedHouse2.OwnerName);
            Assert.AreEqual(house3.OwnerEmail, retreivedHouse2.OwnerEmail);
            Assert.AreEqual(house3.OwnerPhoneNumber, retreivedHouse2.OwnerPhoneNumber);
        }

        [Test]
        public void SaveHouseAndGetHouseByIdTest_TestsThatANewHouseIsSavedInTheDatabaseAsExpected_VerifiesByOutput()
        {
            IHouseApplicationService houseApplicationService = _kernel.Get<IHouseApplicationService>();

            string email = "w@12344321.com";
            string phoneNumber = "01234567890";
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
            string genderRestriction1 = GenderRestriction.GirlsOnly.ToString();

            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService = _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();
            Tuple<decimal, decimal> coordinates = geocodingService.GetCoordinatesFromAddress(area);
            Assert.IsNotNull(coordinates);

            decimal latitude = coordinates.Item1;
            decimal longitude = coordinates.Item2;
            var house = new CreateHouseCommand(title, monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
                true, true, true, true, true, "Apartment", email, phoneNumber, houseNo, streetNo, area,
                dimensionType, dimensionString, 0, ownerName, description, genderRestriction1);
            string houseId = houseApplicationService.SaveNewHouseOffer(house);
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseId));

            HouseFullRepresentation retreivedHouse = houseApplicationService.GetHouseById(houseId);

            Assert.NotNull(retreivedHouse);
            Assert.AreEqual(houseId, retreivedHouse.Id);
            Assert.AreEqual(house.Title, retreivedHouse.Title);
            Assert.AreEqual(house.Description, retreivedHouse.Description);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouse.NumberOfBedrooms);
            Assert.AreEqual(house.NumberOfKitchens, retreivedHouse.NumberOfKitchens);
            Assert.AreEqual(house.GarageAvailable, retreivedHouse.LandlinePhoneAvailable);
            Assert.AreEqual(house.SmokingAllowed, retreivedHouse.SmokingAllowed);
            Assert.AreEqual(house.InternetAvailable, retreivedHouse.InternetAvailable);
            Assert.AreEqual(house.PropertyType, retreivedHouse.PropertyType);
            Assert.AreEqual(house.HouseNo, retreivedHouse.HouseNo);
            Assert.AreEqual(house.Area, retreivedHouse.Area);
            Assert.AreEqual(house.StreetNo, retreivedHouse.StreetNo);
            Assert.AreEqual(house.DimensionStringValue + " " + house.DimensionType, retreivedHouse.Dimension);
            Assert.AreEqual(house.OwnerName, retreivedHouse.OwnerName);
            Assert.AreEqual(house.GenderRestriction, retreivedHouse.GenderRestriction);
        }

        [Test]
        public void SearchHousesByAddressSuccessTest_GetsHousesGivenTheAddress_VerifiesThroughDatabaseRetreival()
        {
            string area = "1600+Amphitheatre+Parkway,+Mountain+View,+CA";

            IHouseApplicationService houseApplicationService = _kernel.Get<IHouseApplicationService>();

            string email = "w@12344321.com";
            string phoneNumber = "01234567890";
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
            string genderRestriction = GenderRestriction.BoysOnly.ToString();

            var house = new CreateHouseCommand(title, monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
            true, true, true, true, true, "Apartment", email, phoneNumber, houseNo, streetNo, area, 
            dimensionType, dimensionString, 0, ownerName, description, genderRestriction);
            var houseCreated = houseApplicationService.SaveNewHouseOffer(house);

            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService = _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();
            Tuple<decimal, decimal> coordinates = geocodingService.GetCoordinatesFromAddress(area);
            Assert.IsNotNull(coordinates);

            IList<HousePartialRepresentation> retreivedHouses = houseApplicationService.SearchHousesByArea(area);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(1, retreivedHouses.Count);

            HousePartialRepresentation retreivedHouse = retreivedHouses[0];
            Assert.NotNull(retreivedHouse);
            Assert.AreEqual(houseCreated, retreivedHouse.HouseId);
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
            string area = "Pindora, Rawalpindi, Pakistan";

            IHouseApplicationService houseApplicationService = _kernel.Get<IHouseApplicationService>();

            string email = "w@12344321.com";
            string phoneNumber = "01234567890";
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
            string genderRestriction = GenderRestriction.BoysOnly.ToString();

            var house = new CreateHouseCommand(title, monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
            true, true, true, true, true, "Apartment", email, phoneNumber, houseNo, streetNo, area, 
            dimensionType, dimensionString, 0, ownerName, description, genderRestriction);
            houseApplicationService.SaveNewHouseOffer(house);

            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService = _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();
            Tuple<decimal, decimal> coordinates = geocodingService.GetCoordinatesFromAddress(area);
            Assert.IsNotNull(coordinates);

            area = "E-11, Islamabad, Pakistan";
            IList<HousePartialRepresentation> retreivedHouses = houseApplicationService.SearchHousesByArea(area);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(0, retreivedHouses.Count);
        }
    }
}
