using Ninject;
using NUnit.Framework;
using RentStuff.Common.Ninject.Modules;
using RentStuff.Common.Utilities;
using RentStuff.Property.Application.HouseServices;
using RentStuff.Property.Application.HouseServices.Commands;
using RentStuff.Property.Application.HouseServices.Representation;
using RentStuff.Property.Application.Ninject.Modules;
using RentStuff.Property.Infrastructure.Persistence.Ninject.Modules;
using System;
using System.Collections.Generic;
using RentStuff.Property.Application.HouseServices.Commands.CreateCommands;
using RentStuff.Property.Application.HouseServices.Representation.FullRepresentations;
using RentStuff.Property.Domain.Model.HostelAggregate;
using RentStuff.Property.Domain.Model.HotelAggregate;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Domain.Model.PropertyAggregate;

namespace RentStuff.Property.Application.IntegrationTests
{
    [TestFixture]
    public class PropertyApplicationServiceTests
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

        // Save and retrieve a full House
        [Test]
        public void SaveHouseTest_TestsThatANewHouseIsSavedInTheDatabaseAsExpected_VerifiesByOutput()
        {
            IPropertyApplicationService houseApplicationService = _kernel.Get<IPropertyApplicationService>();
            
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
            bool isShared = true;
            string rentUnit = "Hour";
            string landlineNumber = "0510000000";
            string fax = "0510000000";
            var createNewHouseCommand = new CreateHouseCommand(title, monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
                internet, landline, cableTv, garage, smokingAllowed, propertyType, email, phoneNumber, houseNo, streetNo, area,
                dimensionType, dimensionString, 0, ownerName, description, genderRestriction, isShared, rentUnit,
                landlineNumber, fax);
            string houseId = houseApplicationService.SaveNewHouseOffer(createNewHouseCommand);

            HouseFullRepresentation retreivedHouse = 
                (HouseFullRepresentation)houseApplicationService.GetPropertyById(houseId, Constants.House);

            Assert.NotNull(retreivedHouse);
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseId));
            Assert.AreEqual(houseId, retreivedHouse.Id);
            Assert.AreEqual(email, retreivedHouse.OwnerEmail);
            Assert.AreEqual(phoneNumber, retreivedHouse.OwnerPhoneNumber);
            Assert.AreEqual(ownerName, retreivedHouse.OwnerName);
            Assert.AreEqual(description, retreivedHouse.Description);
            Assert.AreEqual(houseNo, retreivedHouse.HouseNo);
            Assert.AreEqual(streetNo, retreivedHouse.StreetNo);
            Assert.AreEqual(title, retreivedHouse.Title);
            Assert.AreEqual(area, retreivedHouse.Area);
            Assert.AreEqual(dimensionString + " " + dimensionType, retreivedHouse.Dimension);
            Assert.AreEqual(numberofBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(numberOfBedrooms, retreivedHouse.NumberOfBedrooms);
            Assert.AreEqual(numberOfKitchens, retreivedHouse.NumberOfKitchens);
            Assert.AreEqual(propertyType, retreivedHouse.PropertyType);
            Assert.AreEqual(monthlyRent, retreivedHouse.RentPrice);
            Assert.AreEqual(genderRestriction, retreivedHouse.GenderRestriction);
            Assert.AreEqual(internet, retreivedHouse.InternetAvailable);
            Assert.AreEqual(garage, retreivedHouse.GarageAvailable);
            Assert.AreEqual(landline, retreivedHouse.LandlinePhoneAvailable);
            Assert.AreEqual(cableTv, retreivedHouse.CableTvAvailable);
            Assert.AreEqual(smokingAllowed, retreivedHouse.SmokingAllowed);
            Assert.AreEqual(isShared, retreivedHouse.IsShared);
            Assert.AreEqual(rentUnit, retreivedHouse.RentUnit);
            Assert.AreEqual(landlineNumber, retreivedHouse.LandlineNumber);
            Assert.AreEqual(fax, retreivedHouse.Fax);
        }

        // Save and retrieve a full Hostel
        [Test]
        public void SaveNewHostelTest_ChecksThatANewHostelIsSavedAndRetrievedAsExpected_VerifiesByTheReturnValue()
        {
            IPropertyApplicationService propertyApplicationService = _kernel.Get<IPropertyApplicationService>();

            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            string propertyType = "Hostel";
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
            string area = "Pindora, Rawalpindi, Pakistan";
            long monthlyRent = 90000;
            bool cableTv = false;
            bool internet = true;
            bool parking = true;
            string rentUnit = "Hour";
            bool isShared = true;
            bool laundry = true;
            bool ac = true;
            bool geyser = true;
            bool attachedBathroom = true;
            bool fitnessCentre = false;
            bool balcony = false;
            bool lawn = true;
            bool heating = false;
            bool meals = true;
            bool picknDrop = false;
            bool ironing = false;
            bool cctvCameras = true;
            bool backupElectricity = true;
            int numberOfSeats = 3;
            string landlineNumber = "0510000000";
            string fax = "0510000000";
            bool elevator = true;

            var hostelId = propertyApplicationService.SaveNewHouseOffer(new CreateHostelCommand(title, monthlyRent, internet,
                cableTv, parking, propertyType, email, phoneNumber, area, name, description, genderRestriction.ToString(),
                isShared, rentUnit, laundry, ac, geyser, fitnessCentre, attachedBathroom, ironing, balcony, lawn,
                cctvCameras, backupElectricity, heating, landlineNumber, fax, elevator, meals, picknDrop,
                numberOfSeats));
            
            Assert.IsFalse(string.IsNullOrWhiteSpace(hostelId));
            // Now retrieve the Hostel from the database
            HostelFullRepresentation retrievedHostel =
                (HostelFullRepresentation)propertyApplicationService.GetPropertyById(hostelId, Constants.Hostel);
            Assert.IsNotNull(retrievedHostel);
            Assert.AreEqual(hostelId, retrievedHostel.Id);
            Assert.AreEqual(title, retrievedHostel.Title);
            Assert.AreEqual(description, retrievedHostel.Description);
            Assert.AreEqual(email, retrievedHostel.OwnerEmail);
            Assert.AreEqual(name, retrievedHostel.OwnerName);
            Assert.AreEqual(phoneNumber, retrievedHostel.OwnerPhoneNumber);
            Assert.AreEqual(cableTv, retrievedHostel.CableTvAvailable);
            Assert.AreEqual(internet, retrievedHostel.InternetAvailable);
            Assert.AreEqual(parking, retrievedHostel.ParkingAvailable);
            Assert.AreEqual(propertyType, retrievedHostel.PropertyType);
            Assert.AreEqual(genderRestriction.ToString(), retrievedHostel.GenderRestriction);
            Assert.AreEqual(area, retrievedHostel.Area);
            Assert.AreEqual(monthlyRent, retrievedHostel.RentPrice);
            Assert.AreEqual(isShared, retrievedHostel.IsShared);
            Assert.AreEqual(rentUnit, retrievedHostel.RentUnit);
            Assert.AreEqual(laundry, retrievedHostel.Laundry);
            Assert.AreEqual(ac, retrievedHostel.AC);
            Assert.AreEqual(geyser, retrievedHostel.Geyser);
            Assert.AreEqual(attachedBathroom, retrievedHostel.AttachedBathroom);
            Assert.AreEqual(fitnessCentre, retrievedHostel.FitnessCentre);
            Assert.AreEqual(balcony, retrievedHostel.Balcony);
            Assert.AreEqual(lawn, retrievedHostel.Lawn);
            Assert.AreEqual(ironing, retrievedHostel.Ironing);
            Assert.AreEqual(cctvCameras, retrievedHostel.CctvCameras);
            Assert.AreEqual(backupElectricity, retrievedHostel.BackupElectricity);
            Assert.AreEqual(heating, retrievedHostel.Heating);
            Assert.AreEqual(meals, retrievedHostel.Meals);
            Assert.AreEqual(picknDrop, retrievedHostel.PicknDrop);
            Assert.AreEqual(numberOfSeats, retrievedHostel.NumberOfSeats);
            Assert.AreEqual(landlineNumber, retrievedHostel.LandlineNumber);
            Assert.AreEqual(fax, retrievedHostel.Fax);
            Assert.AreEqual(elevator, retrievedHostel.Elevator);
        }

        // Save and retrieve a full Hotel
        [Test]
        public void SaveNewHotelTest_ChecksThatANewHostelIsSavedAndRetrievedAsExpected_VerifiesByTheReturnValue()
        {
            IPropertyApplicationService propertyApplicationService = _kernel.Get<IPropertyApplicationService>();

            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            decimal latitude = 25.43M;
            decimal longitude = 73.41M;
            string propertyType = Constants.Hotel;
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
            string area = "Pindora, Rawalpindi, Pakistan";
            long monthlyRent = 90000;
            bool cableTv = false;
            bool internet = true;
            bool parking = true;
            string image1 = "Image1.jpg";
            string image2 = "Image2.png";
            string rentUnit = Constants.Daily;
            bool isShared = true;
            bool laundry = true;
            bool ac = true;
            bool geyser = true;
            bool attachedBathroom = true;
            bool fitnessCentre = false;
            bool balcony = false;
            bool lawn = true;
            bool heating = false;
            bool ironing = false;
            bool cctvCameras = true;
            bool backupElectricity = true;

            bool restaurant = true;
            bool airportShuttle = true;
            bool breakfastIncluded = true;
            bool sittingArea = true;
            bool carRental = true;
            bool spa = true;
            bool salon = false;
            bool bathtub = true;
            bool swimmingPool = true;
            bool kitchen = true;
            int numberOfAdults = 2;
            int numberOfChildren = 1;

            string landlineNumber = "0510000000";
            string fax = "0510000000";
            bool elevator = false;
            
            // Save the Hotel
            var hotelId = propertyApplicationService.SaveNewHouseOffer(new CreateHotelCommand(title, monthlyRent, internet,
                cableTv, parking, propertyType, email, phoneNumber, area, name, description, genderRestriction.ToString(),
                isShared, rentUnit, laundry, ac, geyser, fitnessCentre, attachedBathroom, ironing, balcony,
                lawn, cctvCameras, backupElectricity, heating, landlineNumber, fax, elevator, restaurant,
                airportShuttle, breakfastIncluded, sittingArea, carRental, spa, salon, bathtub, swimmingPool,
                kitchen, numberOfAdults, numberOfChildren));

            // Retreive the Hotel
            HotelFullRepresentation retrievedHotel = 
                (HotelFullRepresentation)propertyApplicationService.GetPropertyById(hotelId, Constants.Hotel);

            Assert.IsNotNull(retrievedHotel);
            Assert.AreEqual(title, retrievedHotel.Title);
            Assert.AreEqual(description, retrievedHotel.Description);
            Assert.AreEqual(email, retrievedHotel.OwnerEmail);
            Assert.AreEqual(name, retrievedHotel.OwnerName);
            Assert.AreEqual(phoneNumber, retrievedHotel.OwnerPhoneNumber);
            Assert.AreEqual(cableTv, retrievedHotel.CableTvAvailable);
            Assert.AreEqual(internet, retrievedHotel.InternetAvailable);
            Assert.AreEqual(parking, retrievedHotel.ParkingAvailable);
            Assert.AreEqual(propertyType, retrievedHotel.PropertyType);
            Assert.AreEqual(genderRestriction.ToString(), retrievedHotel.GenderRestriction);
            Assert.AreEqual(area, retrievedHotel.Area);
            Assert.AreEqual(monthlyRent, retrievedHotel.RentPrice);
            Assert.AreEqual(isShared, retrievedHotel.IsShared);
            Assert.AreEqual(rentUnit, retrievedHotel.RentUnit);
            Assert.AreEqual(laundry, retrievedHotel.Laundry);
            Assert.AreEqual(ac, retrievedHotel.AC);
            Assert.AreEqual(geyser, retrievedHotel.Geyser);
            Assert.AreEqual(attachedBathroom, retrievedHotel.AttachedBathroom);
            Assert.AreEqual(fitnessCentre, retrievedHotel.FitnessCentre);
            Assert.AreEqual(balcony, retrievedHotel.Balcony);
            Assert.AreEqual(lawn, retrievedHotel.Lawn);
            Assert.AreEqual(ironing, retrievedHotel.Ironing);
            Assert.AreEqual(cctvCameras, retrievedHotel.CctvCameras);
            Assert.AreEqual(backupElectricity, retrievedHotel.BackupElectricity);
            Assert.AreEqual(heating, retrievedHotel.Heating);

            Assert.AreEqual(restaurant, retrievedHotel.Restaurant);
            Assert.AreEqual(airportShuttle, retrievedHotel.AirportShuttle);
            Assert.AreEqual(breakfastIncluded, retrievedHotel.BreakfastIncluded);
            Assert.AreEqual(sittingArea, retrievedHotel.SittingArea);
            Assert.AreEqual(carRental, retrievedHotel.CarRental);
            Assert.AreEqual(spa, retrievedHotel.Spa);
            Assert.AreEqual(salon, retrievedHotel.Salon);
            Assert.AreEqual(bathtub, retrievedHotel.Bathtub);
            Assert.AreEqual(swimmingPool, retrievedHotel.SwimmingPool);
            Assert.AreEqual(kitchen, retrievedHotel.Kitchen);

            Assert.AreEqual(numberOfAdults, retrievedHotel.NumberOfAdults);
            Assert.AreEqual(numberOfChildren, retrievedHotel.NumberOfChildren);
            Assert.AreEqual(numberOfAdults + numberOfChildren, retrievedHotel.TotalOccupants);
            
            Assert.AreEqual(elevator, retrievedHotel.Elevator);
        }

        // Save and retrieve Partial House
        [Test]
        public void SaveHouseAndRetrieveTest_TestsThatANewHouseIsSavedInTheDatabaseAsExpected_VerifiesByOutput()
        {
            IPropertyApplicationService houseApplicationService = _kernel.Get<IPropertyApplicationService>();

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
            bool isShared = false;
            // Null value should never be assigned, defalt value i going to be assigned instead. So pass NULL deliberately
            string rentUnit = null;

            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService = _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();
            Tuple<decimal, decimal> coordinates = geocodingService.GetCoordinatesFromAddress(area);
            Assert.IsNotNull(coordinates);
            
            var house = new CreateHouseCommand(title, monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
                true, true, true, true, true, "Apartment", email, phoneNumber, houseNo, streetNo, area,
                dimensionType, dimensionString, 0, ownerName, description, genderRestriction,isShared, rentUnit,
                null, null);
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
            Assert.AreEqual(house.RentPrice, retreivedHouse.RentPrice);
            Assert.AreEqual(house.Title, retreivedHouse.Title);
            Assert.AreEqual(house.Description, retreivedHouse.Description);
            Assert.AreEqual(house.DimensionStringValue + " " + house.DimensionType, retreivedHouse.Dimension);
            Assert.AreEqual(house.OwnerName, retreivedHouse.OwnerName);
            Assert.AreEqual(house.IsShared, retreivedHouse.IsShared);
            Assert.AreEqual(house.GenderRestriction.ToString(), retreivedHouse.GenderRestriction);
            // "Month" is the default value and should be assigned automatically as we provided a Null value
            Assert.AreEqual("Month", retreivedHouse.RentUnit);
        }

        [Test]
        public void SearchHousesByAddressAndPropertyTypeTest_TestsThatANewHouseIsSavedInTheDatabaseAndRetreivedAsExpected_VerifiesByOutput()
        {
            IPropertyApplicationService houseApplicationService = _kernel.Get<IPropertyApplicationService>();

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
            bool isShared = true;
            string rentUnit = House.GetAllRentUnits()[0];

            var house = new CreateHouseCommand(title, monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
                true, true, true, true, true, propertyType1, email, phoneNumber, houseNo, streetNo, area,
                dimensionType, dimensionString, 0, ownerName, description, genderRestriction, isShared, rentUnit,
                null, null);
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
            bool isShared2 = false;
            string rentUnit2 = House.GetAllRentUnits()[1];

            var house2 = new CreateHouseCommand(title2, monthlyRent2, numberOfBedrooms2, numberOfKitchens2, numberofBathrooms2,
                true, true, true, true, true, propertyType2, email2, phoneNumber2, houseNo2, streetNo2, area2,
                dimensionType2, dimensionString2, 0, ownerName2, description2, genderRestriction2, isShared2,
                rentUnit2, null, null);
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
            bool isShared3 = false;
            string rentUnit3 = House.GetAllRentUnits()[2];

            var house3 = new CreateHouseCommand(title3, monthlyRent3, numberOfBedrooms3, numberOfKitchens3, numberofBathrooms3,
                true, true, true, true, true, propertyType3, email3, phoneNumber3, houseNo3, streetNo3, area3,
                dimensionType3, dimensionString3, 0, ownerName3, description3, genderRestriction3, isShared3, 
                rentUnit3, null, null);
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
            Assert.AreEqual(house2.RentPrice, retreivedHouse.RentPrice);
            Assert.AreEqual(house2.Title, retreivedHouse.Title);
            Assert.AreEqual(house2.Description, retreivedHouse.Description);
            Assert.AreEqual(house2.DimensionStringValue + " " + house2.DimensionType, retreivedHouse.Dimension);
            Assert.AreEqual(house2.OwnerName, retreivedHouse.OwnerName);
            Assert.AreEqual(house2.IsShared, retreivedHouse.IsShared);
            Assert.AreEqual(house2.GenderRestriction.ToString(), retreivedHouse.GenderRestriction);
            Assert.AreEqual(house2.RentUnit, retreivedHouse.RentUnit);

            // Verification of House No. 3
            HousePartialRepresentation retreivedHouse2 = houses[1];
            Assert.NotNull(retreivedHouse2);
            Assert.AreEqual(houseCreated3, retreivedHouse2.HouseId);
            Assert.AreEqual(house3.NumberOfBathrooms, retreivedHouse2.NumberOfBathrooms);
            Assert.AreEqual(house3.NumberOfBedrooms, retreivedHouse2.NumberOfBedrooms);
            Assert.AreEqual(house3.PropertyType, retreivedHouse2.PropertyType);
            Assert.AreEqual(house3.Area, retreivedHouse2.Area);
            Assert.AreEqual(house3.RentPrice, retreivedHouse2.RentPrice);
            Assert.AreEqual(house3.Title, retreivedHouse2.Title);
            Assert.AreEqual(house3.Description, retreivedHouse2.Description);
            Assert.AreEqual(house3.DimensionStringValue + " " + house3.DimensionType, retreivedHouse2.Dimension);
            Assert.AreEqual(house3.OwnerName, retreivedHouse2.OwnerName);
            Assert.AreEqual(house3.IsShared, retreivedHouse2.IsShared);
            Assert.AreEqual(house3.GenderRestriction.ToString(), retreivedHouse2.GenderRestriction);
            Assert.AreEqual(house3.RentUnit, retreivedHouse2.RentUnit);
        }

        [Test]
        public void SearchHousesByPropertyTypeOnlyTest_TestsThatANewHouseIsSavedInTheDatabaseAndRetreivedAsExpected_VerifiesByOutput()
        {
            IPropertyApplicationService houseApplicationService = _kernel.Get<IPropertyApplicationService>();

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
            bool isShared = true;
            string rentUnit = House.GetAllRentUnits()[3];

            var house = new CreateHouseCommand(title, monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
                true, true, true, true, true, propertyType1, email, phoneNumber, houseNo, streetNo, area,
                dimensionType, dimensionString, 0, ownerName, description, genderRestriction1, isShared, rentUnit,
                null, null);
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
            bool isShared2 = true;
            string rentUnit2 = House.GetAllRentUnits()[0];

            var house2 = new CreateHouseCommand(title2, monthlyRent2, numberOfBedrooms2, numberOfKitchens2, numberofBathrooms2,
                true, true, true, true, true, propertyType2, email2, phoneNumber2, houseNo2, streetNo2, area2,
                dimensionType2, dimensionString2, 0, ownerName2, description2, genderRestriction2, isShared2, 
                rentUnit2, null, null);
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
            bool isShared3 = true;
            string rentUnit3 = House.GetAllRentUnits()[1];

            var house3 = new CreateHouseCommand(title3, monthlyRent3, numberOfBedrooms3, numberOfKitchens3, numberofBathrooms3,
                true, true, true, true, true, propertyType3, email3, phoneNumber3, houseNo3, streetNo3, area3,
                dimensionType3, dimensionString3, 0, ownerName3, description3, genderRestriction3, isShared3,
                rentUnit3, null, null);
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
            Assert.AreEqual(house2.RentPrice, retreivedHouse.RentPrice);
            Assert.AreEqual(house2.Title, retreivedHouse.Title);
            Assert.AreEqual(house2.Description, retreivedHouse.Description);
            Assert.AreEqual(house2.DimensionStringValue + " " + house2.DimensionType, retreivedHouse.Dimension);
            Assert.AreEqual(house2.OwnerName, retreivedHouse.OwnerName);
            Assert.AreEqual(house2.IsShared, retreivedHouse.IsShared);
            Assert.AreEqual(house2.GenderRestriction, retreivedHouse.GenderRestriction);
            Assert.AreEqual(house2.RentUnit, retreivedHouse.RentUnit);
        }
        
        [Test]
        public void SaveHouseAndGetHouseByIdTest_TestsThatANewHouseIsSavedInTheDatabaseAsExpected_VerifiesByOutput()
        {
            IPropertyApplicationService houseApplicationService = _kernel.Get<IPropertyApplicationService>();

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
            string rentUnit = House.GetAllRentUnits()[0];

            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService = _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();
            Tuple<decimal, decimal> coordinates = geocodingService.GetCoordinatesFromAddress(area);
            Assert.IsNotNull(coordinates);

            bool isShared = true;
            var house = new CreateHouseCommand(title, monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
                true, true, true, true, true, "Apartment", email, phoneNumber, houseNo, streetNo, area,
                dimensionType, dimensionString, 0, ownerName, description, genderRestriction1, isShared, rentUnit, null, null);
            string houseId = houseApplicationService.SaveNewHouseOffer(house);
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseId));

            HouseFullRepresentation retreivedHouse = (HouseFullRepresentation)houseApplicationService.GetPropertyById(houseId, Constants.House);

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
            Assert.AreEqual(house.IsShared, retreivedHouse.IsShared);
            Assert.AreEqual(house.GenderRestriction, retreivedHouse.GenderRestriction);
            Assert.AreEqual(house.RentUnit, retreivedHouse.RentUnit);
        }
    }
}
