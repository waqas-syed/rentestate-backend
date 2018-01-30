using Ninject;
using NUnit.Framework;
using RentStuff.Common.Ninject.Modules;
using RentStuff.Common.Utilities;
using RentStuff.Property.Application.Ninject.Modules;
using RentStuff.Property.Domain.Model.PropertyAggregate;
using RentStuff.Property.Infrastructure.Persistence.Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RentStuff.Property.Application.PropertyServices;
using RentStuff.Property.Application.PropertyServices.Commands.CreateCommands;
using RentStuff.Property.Application.PropertyServices.Commands.UpdateCommands;
using RentStuff.Property.Application.PropertyServices.Representation;
using RentStuff.Property.Application.PropertyServices.Representation.FullRepresentations;
using RentStuff.Property.Application.PropertyServices.Representation.PartialRepresentations;

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

        #region Save and retrieve Full Representations

        // Save and retrieve a full House Representation
        // GET BY ID
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
            string propertyType = Constants.House;
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
            string houseId = houseApplicationService.SaveNewProperty(JsonConvert.SerializeObject(createNewHouseCommand), email);

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

        // Save and retrieve a full Apartment Representation
        // GET BY ID
        [Test]
        public void SaveApartmentTest_TestsThatANewHouseIsSavedInTheDatabaseAsExpected_VerifiesByOutput()
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
            string propertyType = Constants.Apartment;
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
            string houseId = houseApplicationService.SaveNewProperty(JsonConvert.SerializeObject(createNewHouseCommand), email);

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

        // Save and retrieve a full Hostel Representation
        // GET BY ID
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

            var createNewHostelCommand = new CreateHostelCommand(title, monthlyRent, internet,
                cableTv, parking, propertyType, email, phoneNumber, area, name, description,
                genderRestriction.ToString(),
                isShared, rentUnit, laundry, ac, geyser, fitnessCentre, attachedBathroom, ironing, balcony, lawn,
                cctvCameras, backupElectricity, heating, landlineNumber, fax, elevator, meals, picknDrop,
                numberOfSeats);
            var hostelId = propertyApplicationService.SaveNewProperty(JsonConvert.SerializeObject(createNewHostelCommand), email);
            
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

        // Save and retrieve a full Hotel Representation
        // GET BY ID
        [Test]
        public void SaveNewHotelTest_ChecksThatANewHostelIsSavedAndRetrievedAsExpected_VerifiesByTheReturnValue()
        {
            IPropertyApplicationService propertyApplicationService = _kernel.Get<IPropertyApplicationService>();

            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            string propertyType = Constants.Hotel;
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
            string area = "Pindora, Rawalpindi, Pakistan";
            long monthlyRent = 90000;
            bool cableTv = false;
            bool internet = true;
            bool parking = true;
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

            var createNewHotelCommand = new CreateHotelCommand(title, monthlyRent, internet,
                cableTv, parking, propertyType, email, phoneNumber, area, name, description,
                genderRestriction.ToString(),
                isShared, rentUnit, laundry, ac, geyser, fitnessCentre, attachedBathroom, ironing, balcony,
                lawn, cctvCameras, backupElectricity, heating, landlineNumber, fax, elevator, restaurant,
                airportShuttle, breakfastIncluded, sittingArea, carRental, spa, salon, bathtub, swimmingPool,
                kitchen, numberOfAdults, numberOfChildren);
            // Save the Hotel
            var hotelId = propertyApplicationService.SaveNewProperty(JsonConvert.SerializeObject(createNewHotelCommand), email);

            // Retreive the Hotel
            HotelFullRepresentation retrievedHotel = 
                (HotelFullRepresentation)propertyApplicationService.GetPropertyById(hotelId, Constants.Hotel);

            Assert.IsNotNull(retrievedHotel);
            Assert.AreEqual(hotelId, retrievedHotel.Id);
            Assert.AreEqual(title, retrievedHotel.Title);
            Assert.AreEqual(description, retrievedHotel.Description);
            Assert.AreEqual(email, retrievedHotel.OwnerEmail);
            Assert.AreEqual(name, retrievedHotel.OwnerName);
            Assert.AreEqual(phoneNumber, retrievedHotel.OwnerPhoneNumber);
            Assert.AreEqual(propertyType, retrievedHotel.PropertyType);
            Assert.AreEqual(genderRestriction.ToString(), retrievedHotel.GenderRestriction);
            Assert.AreEqual(area, retrievedHotel.Area);
            Assert.AreEqual(monthlyRent, retrievedHotel.RentPrice);
            Assert.AreEqual(cableTv, retrievedHotel.CableTvAvailable);
            Assert.AreEqual(internet, retrievedHotel.InternetAvailable);
            Assert.AreEqual(parking, retrievedHotel.ParkingAvailable);
            Assert.AreEqual(rentUnit, retrievedHotel.RentUnit);
            Assert.AreEqual(isShared, retrievedHotel.IsShared);
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

            Assert.AreEqual(numberOfAdults, retrievedHotel.Occupants.Adults);
            Assert.AreEqual(numberOfChildren, retrievedHotel.Occupants.Children);
            Assert.AreEqual(numberOfAdults + numberOfChildren, retrievedHotel.Occupants.TotalOccupants);
            
            Assert.AreEqual(landlineNumber, retrievedHotel.LandlineNumber);
            Assert.AreEqual(fax, retrievedHotel.Fax);
            Assert.AreEqual(elevator, retrievedHotel.Elevator);
        }

        // Save and retrieve a full Guest House Representation
        // GET BY ID
        [Test]
        public void SaveNewGuestHouseTest_ChecksThatANewHostelIsSavedAndRetrievedAsExpected_VerifiesByTheReturnValue()
        {
            IPropertyApplicationService propertyApplicationService = _kernel.Get<IPropertyApplicationService>();

            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            decimal latitude = 25.43M;
            decimal longitude = 73.41M;
            string propertyType = Constants.GuestHouse;
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

            var createHotelCommand = new CreateHotelCommand(title, monthlyRent, internet,
                cableTv, parking, propertyType, email, phoneNumber, area, name, description,
                genderRestriction.ToString(),
                isShared, rentUnit, laundry, ac, geyser, fitnessCentre, attachedBathroom, ironing, balcony,
                lawn, cctvCameras, backupElectricity, heating, landlineNumber, fax, elevator, restaurant,
                airportShuttle, breakfastIncluded, sittingArea, carRental, spa, salon, bathtub, swimmingPool,
                kitchen, numberOfAdults, numberOfChildren);
            // Save the Hotel
            var guestHouseId = propertyApplicationService.SaveNewProperty(JsonConvert.SerializeObject(createHotelCommand), email);

            // Retreive the Hotel
            HotelFullRepresentation retrievedGuestHouse =
                (HotelFullRepresentation)propertyApplicationService.GetPropertyById(guestHouseId, Constants.Hotel);

            Assert.IsNotNull(retrievedGuestHouse);
            Assert.AreEqual(guestHouseId, retrievedGuestHouse.Id);
            Assert.AreEqual(title, retrievedGuestHouse.Title);
            Assert.AreEqual(description, retrievedGuestHouse.Description);
            Assert.AreEqual(email, retrievedGuestHouse.OwnerEmail);
            Assert.AreEqual(name, retrievedGuestHouse.OwnerName);
            Assert.AreEqual(phoneNumber, retrievedGuestHouse.OwnerPhoneNumber);
            Assert.AreEqual(propertyType, retrievedGuestHouse.PropertyType);
            Assert.AreEqual(genderRestriction.ToString(), retrievedGuestHouse.GenderRestriction);
            Assert.AreEqual(area, retrievedGuestHouse.Area);
            Assert.AreEqual(monthlyRent, retrievedGuestHouse.RentPrice);
            Assert.AreEqual(cableTv, retrievedGuestHouse.CableTvAvailable);
            Assert.AreEqual(internet, retrievedGuestHouse.InternetAvailable);
            Assert.AreEqual(parking, retrievedGuestHouse.ParkingAvailable);
            Assert.AreEqual(rentUnit, retrievedGuestHouse.RentUnit);
            Assert.AreEqual(isShared, retrievedGuestHouse.IsShared);
            Assert.AreEqual(laundry, retrievedGuestHouse.Laundry);
            Assert.AreEqual(ac, retrievedGuestHouse.AC);
            Assert.AreEqual(geyser, retrievedGuestHouse.Geyser);
            Assert.AreEqual(attachedBathroom, retrievedGuestHouse.AttachedBathroom);
            Assert.AreEqual(fitnessCentre, retrievedGuestHouse.FitnessCentre);
            Assert.AreEqual(balcony, retrievedGuestHouse.Balcony);
            Assert.AreEqual(lawn, retrievedGuestHouse.Lawn);
            Assert.AreEqual(ironing, retrievedGuestHouse.Ironing);
            Assert.AreEqual(cctvCameras, retrievedGuestHouse.CctvCameras);
            Assert.AreEqual(backupElectricity, retrievedGuestHouse.BackupElectricity);
            Assert.AreEqual(heating, retrievedGuestHouse.Heating);

            Assert.AreEqual(restaurant, retrievedGuestHouse.Restaurant);
            Assert.AreEqual(airportShuttle, retrievedGuestHouse.AirportShuttle);
            Assert.AreEqual(breakfastIncluded, retrievedGuestHouse.BreakfastIncluded);
            Assert.AreEqual(sittingArea, retrievedGuestHouse.SittingArea);
            Assert.AreEqual(carRental, retrievedGuestHouse.CarRental);
            Assert.AreEqual(spa, retrievedGuestHouse.Spa);
            Assert.AreEqual(salon, retrievedGuestHouse.Salon);
            Assert.AreEqual(bathtub, retrievedGuestHouse.Bathtub);
            Assert.AreEqual(swimmingPool, retrievedGuestHouse.SwimmingPool);
            Assert.AreEqual(kitchen, retrievedGuestHouse.Kitchen);

            Assert.AreEqual(numberOfAdults, retrievedGuestHouse.Occupants.Adults);
            Assert.AreEqual(numberOfChildren, retrievedGuestHouse.Occupants.Children);
            Assert.AreEqual(numberOfAdults + numberOfChildren, retrievedGuestHouse.Occupants.TotalOccupants);

            Assert.AreEqual(landlineNumber, retrievedGuestHouse.LandlineNumber);
            Assert.AreEqual(fax, retrievedGuestHouse.Fax);
            Assert.AreEqual(elevator, retrievedGuestHouse.Elevator);
        }

        #endregion Save and retrieve Full Representations

        #region Save House and retrieve Partial Representations

        // Save and retrieve Partial House Representations
        // SEARCH HOUSES BY PROPERTY TYPE
        // SEARCH HOUSES BY PROPERTY TYPE + EMAIL
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
            string propertyType = Constants.House;
            string genderRestriction = GenderRestriction.GirlsOnly.ToString();
            bool isShared = false;
            // Null value should never be assigned, defalt value i going to be assigned instead. So pass NULL deliberately
            string rentUnit = Constants.Monthly;

            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService = _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();
            Tuple<decimal, decimal> coordinates = geocodingService.GetCoordinatesFromAddress(area);
            Assert.IsNotNull(coordinates);
            
            var house = new CreateHouseCommand(title, monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
                true, true, true, true, true, propertyType, email, phoneNumber, houseNo, streetNo, area,
                dimensionType, dimensionString, 0, ownerName, description, genderRestriction,isShared, rentUnit,
                null, null);
            string houseCreated = houseApplicationService.SaveNewProperty(JsonConvert.SerializeObject(house),email);
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseCreated));

            // BY EMAIL & PROPERTY TYPE
            IList<HousePartialRepresentation> houses = 
                houseApplicationService.GetPropertiesByEmail(Constants.House, email).Cast<HousePartialRepresentation>().ToList();
            HousePartialRepresentation retreivedHouse = houses[0];

            Assert.NotNull(retreivedHouse);
            Assert.AreEqual(houseCreated, retreivedHouse.Id);
            Assert.AreEqual(house.PropertyType, retreivedHouse.PropertyType);
            Assert.AreEqual(house.Area, retreivedHouse.Area);
            Assert.AreEqual(house.RentPrice, retreivedHouse.RentPrice);
            Assert.AreEqual(house.Title, retreivedHouse.Title);
            Assert.AreEqual(house.DimensionStringValue + " " + house.DimensionType, retreivedHouse.Dimension);
            Assert.AreEqual(house.OwnerName, retreivedHouse.OwnerName);
            Assert.AreEqual(house.OwnerPhoneNumber, retreivedHouse.OwnerPhoneNumber);
            Assert.AreEqual(house.LandlineNumber, retreivedHouse.OwnerLandlineNumber);
            Assert.AreEqual(house.IsShared, retreivedHouse.IsShared);
            Assert.AreEqual(house.GenderRestriction, retreivedHouse.GenderRestriction);
            Assert.AreEqual(house.CableTvAvailable, retreivedHouse.CableTv);
            Assert.AreEqual(house.InternetAvailable, retreivedHouse.Internet);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouse.NumberOfBedrooms);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfKitchens, retreivedHouse.NumberOfKitchens);
            Assert.AreEqual(rentUnit, retreivedHouse.RentUnit);

            // BY PROPERTY TYPE ONLY
            houses =
                houseApplicationService.SearchPropertiesByPropertyType(Constants.House).Cast<HousePartialRepresentation>().ToList();
            retreivedHouse = houses[0];

            Assert.NotNull(retreivedHouse);
            Assert.AreEqual(houseCreated, retreivedHouse.Id);
            Assert.AreEqual(house.PropertyType, retreivedHouse.PropertyType);
            Assert.AreEqual(house.Area, retreivedHouse.Area);
            Assert.AreEqual(house.RentPrice, retreivedHouse.RentPrice);
            Assert.AreEqual(house.Title, retreivedHouse.Title);
            Assert.AreEqual(house.DimensionStringValue + " " + house.DimensionType, retreivedHouse.Dimension);
            Assert.AreEqual(house.OwnerName, retreivedHouse.OwnerName);
            Assert.AreEqual(house.OwnerPhoneNumber, retreivedHouse.OwnerPhoneNumber);
            Assert.AreEqual(house.LandlineNumber, retreivedHouse.OwnerLandlineNumber);
            Assert.AreEqual(house.IsShared, retreivedHouse.IsShared);
            Assert.AreEqual(house.GenderRestriction, retreivedHouse.GenderRestriction);
            Assert.AreEqual(house.CableTvAvailable, retreivedHouse.CableTv);
            Assert.AreEqual(house.InternetAvailable, retreivedHouse.Internet);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouse.NumberOfBedrooms);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfKitchens, retreivedHouse.NumberOfKitchens);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouse.NumberOfBedrooms);
            Assert.AreEqual(rentUnit, retreivedHouse.RentUnit);
        }

        // Save and retrieve Partial Hostels Representations
        // SEARCH HOSTELS BY PROPERTY TYPE
        // SEARCH HOSTELS BY PROPERTY TYPE + EMAIL
        [Test]
        public void SaveHostelAndRetrieveTest_TestsThatANewHostelIsSavedInTheDatabaseAsExpected_VerifiesByOutput()
        {
            IPropertyApplicationService propertyApplicationService = _kernel.Get<IPropertyApplicationService>();

            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            string propertyType = Constants.Hostel;
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

            var createNewHostelCommand = new CreateHostelCommand(title, monthlyRent,
                internet,
                cableTv, parking, propertyType, email, phoneNumber, area, name, description,
                genderRestriction.ToString(),
                isShared, rentUnit, laundry, ac, geyser, fitnessCentre, attachedBathroom, ironing, balcony, lawn,
                cctvCameras, backupElectricity, heating, landlineNumber, fax, elevator, meals, picknDrop,
                numberOfSeats);
            var hostelId = propertyApplicationService.SaveNewProperty(JsonConvert.SerializeObject(createNewHostelCommand), email);

            Assert.IsFalse(string.IsNullOrWhiteSpace(hostelId));

            // BY EMAIL & PROPERTY TYPE
            // Now retrieve the Hostel from the database
            IList<HostelPartialRepresentation> retrievedHostels =
                propertyApplicationService.GetPropertiesByEmail(Constants.Hostel, email).Cast<HostelPartialRepresentation>().ToList();
            HostelPartialRepresentation retrievedHostel = retrievedHostels[0];

            Assert.IsNotNull(retrievedHostel);
            Assert.AreEqual(hostelId, retrievedHostel.Id);
            Assert.AreEqual(title, retrievedHostel.Title);
            Assert.AreEqual(name, retrievedHostel.OwnerName);
            Assert.AreEqual(phoneNumber, retrievedHostel.OwnerPhoneNumber);
            Assert.AreEqual(landlineNumber, retrievedHostel.OwnerLandlineNumber);
            Assert.AreEqual(propertyType, retrievedHostel.PropertyType);
            Assert.AreEqual(genderRestriction.ToString(), retrievedHostel.GenderRestriction);
            Assert.AreEqual(area, retrievedHostel.Area);
            Assert.AreEqual(monthlyRent, retrievedHostel.RentPrice);
            Assert.AreEqual(internet, retrievedHostel.Internet);
            Assert.AreEqual(cableTv, retrievedHostel.CableTv);
            Assert.AreEqual(parking, retrievedHostel.Parking);
            Assert.AreEqual(isShared, retrievedHostel.IsShared);
            Assert.AreEqual(rentUnit, retrievedHostel.RentUnit);
            Assert.AreEqual(laundry, retrievedHostel.Laundry);
            Assert.AreEqual(ac, retrievedHostel.AC);
            Assert.AreEqual(geyser, retrievedHostel.Geyser);
            Assert.AreEqual(attachedBathroom, retrievedHostel.AttachedBathroom);
            Assert.AreEqual(backupElectricity, retrievedHostel.BackupElectricity);
            Assert.AreEqual(meals, retrievedHostel.Meals);
            Assert.AreEqual(numberOfSeats, retrievedHostel.NumberOfSeats);

            // BY PROPERTY TYPE ONLY
            // Now retrieve the Hostel from the database
            retrievedHostels =
                propertyApplicationService.SearchPropertiesByPropertyType(Constants.Hostel).Cast<HostelPartialRepresentation>().ToList();
            retrievedHostel = retrievedHostels[0];

            Assert.IsNotNull(retrievedHostel);
            Assert.AreEqual(hostelId, retrievedHostel.Id);
            Assert.AreEqual(title, retrievedHostel.Title);
            Assert.AreEqual(name, retrievedHostel.OwnerName);
            Assert.AreEqual(phoneNumber, retrievedHostel.OwnerPhoneNumber);
            Assert.AreEqual(landlineNumber, retrievedHostel.OwnerLandlineNumber);
            Assert.AreEqual(propertyType, retrievedHostel.PropertyType);
            Assert.AreEqual(genderRestriction.ToString(), retrievedHostel.GenderRestriction);
            Assert.AreEqual(area, retrievedHostel.Area);
            Assert.AreEqual(monthlyRent, retrievedHostel.RentPrice);
            Assert.AreEqual(internet, retrievedHostel.Internet);
            Assert.AreEqual(cableTv, retrievedHostel.CableTv);
            Assert.AreEqual(parking, retrievedHostel.Parking);
            Assert.AreEqual(isShared, retrievedHostel.IsShared);
            Assert.AreEqual(rentUnit, retrievedHostel.RentUnit);
            Assert.AreEqual(laundry, retrievedHostel.Laundry);
            Assert.AreEqual(ac, retrievedHostel.AC);
            Assert.AreEqual(geyser, retrievedHostel.Geyser);
            Assert.AreEqual(attachedBathroom, retrievedHostel.AttachedBathroom);
            Assert.AreEqual(backupElectricity, retrievedHostel.BackupElectricity);
            Assert.AreEqual(meals, retrievedHostel.Meals);
            Assert.AreEqual(numberOfSeats, retrievedHostel.NumberOfSeats);
        }

        // Save and retrieve Partial Hotels
        // SEARCH HOTELS BY PROPERTY TYPE
        // SEARCH HOTELS BY PROPERTY TYPE + EMAIL
        [Test]
        public void SaveHotelAndRetrieveTest_TestsThatANewHotelIsSavedInTheDatabaseAsExpected_VerifiesByOutput()
        {
            IPropertyApplicationService propertyApplicationService = _kernel.Get<IPropertyApplicationService>();

            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            string propertyType = Constants.Hotel;
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
            string area = "Pindora, Rawalpindi, Pakistan";
            long monthlyRent = 90000;
            bool cableTv = false;
            bool internet = true;
            bool parking = true;
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

           var createNewHotelCommand = new CreateHotelCommand(title, monthlyRent, internet,
                cableTv, parking, propertyType, email, phoneNumber, area, name, description,
                genderRestriction.ToString(),
                isShared, rentUnit, laundry, ac, geyser, fitnessCentre, attachedBathroom, ironing, balcony,
                lawn, cctvCameras, backupElectricity, heating, landlineNumber, fax, elevator, restaurant,
                airportShuttle, breakfastIncluded, sittingArea, carRental, spa, salon, bathtub, swimmingPool,
                kitchen, numberOfAdults, numberOfChildren);
            // Save the Hotel
            var hotelId = propertyApplicationService.SaveNewProperty(JsonConvert.SerializeObject(createNewHotelCommand), email);

            // BY EMAIL & PROPERTY TYPE
            // Retreive the Hotel
            List<HotelPartialRepresentation> retrievedHotels =
                propertyApplicationService.GetPropertiesByEmail(Constants.Hotel, email).Cast<HotelPartialRepresentation>().ToList();

            Assert.IsNotNull(retrievedHotels);
            Assert.AreEqual(1, retrievedHotels.Count);
            HotelPartialRepresentation retrievedHotel = retrievedHotels[0];
            Assert.IsNotNull(retrievedHotel);
            Assert.AreEqual(hotelId, retrievedHotel.Id);
            Assert.AreEqual(title, retrievedHotel.Title);
            Assert.AreEqual(name, retrievedHotel.OwnerName);
            Assert.AreEqual(phoneNumber, retrievedHotel.OwnerPhoneNumber);
            Assert.AreEqual(landlineNumber, retrievedHotel.OwnerLandlineNumber);
            Assert.AreEqual(propertyType, retrievedHotel.PropertyType);
            Assert.AreEqual(genderRestriction.ToString(), retrievedHotel.GenderRestriction);
            Assert.AreEqual(area, retrievedHotel.Area);
            Assert.AreEqual(monthlyRent, retrievedHotel.RentPrice);
            Assert.AreEqual(isShared, retrievedHotel.IsShared);
            Assert.AreEqual(rentUnit, retrievedHotel.RentUnit);
            Assert.AreEqual(internet, retrievedHotel.Internet);
            Assert.AreEqual(cableTv, retrievedHotel.CableTv);
            Assert.AreEqual(parking, retrievedHotel.Parking);
            Assert.AreEqual(ac, retrievedHotel.AC);
            Assert.AreEqual(geyser, retrievedHotel.Geyser);
            Assert.AreEqual(attachedBathroom, retrievedHotel.AttachedBathroom);
            Assert.AreEqual(fitnessCentre, retrievedHotel.FitnessCentre);
            Assert.AreEqual(backupElectricity, retrievedHotel.BackupElectricity);
            Assert.AreEqual(heating, retrievedHotel.Heating);
            Assert.AreEqual(airportShuttle, retrievedHotel.AirportShuttle);
            Assert.AreEqual(breakfastIncluded, retrievedHotel.BreakfastIncluded);
            Assert.AreEqual(numberOfAdults, retrievedHotel.Occupants.Adults);
            Assert.AreEqual(numberOfChildren, retrievedHotel.Occupants.Children);
            Assert.AreEqual(numberOfAdults + numberOfChildren, retrievedHotel.Occupants.TotalOccupants);

            // BY PROPERTY TYPE ONLY
            // Retreive the Hotel
            retrievedHotels =
                propertyApplicationService.SearchPropertiesByPropertyType(Constants.Hotel).Cast<HotelPartialRepresentation>().ToList();

            Assert.IsNotNull(retrievedHotels);
            Assert.AreEqual(1, retrievedHotels.Count);
            retrievedHotel = retrievedHotels[0];
            Assert.IsNotNull(retrievedHotel);
            Assert.AreEqual(hotelId, retrievedHotel.Id);
            Assert.AreEqual(title, retrievedHotel.Title);
            Assert.AreEqual(name, retrievedHotel.OwnerName);
            Assert.AreEqual(phoneNumber, retrievedHotel.OwnerPhoneNumber);
            Assert.AreEqual(landlineNumber, retrievedHotel.OwnerLandlineNumber);
            Assert.AreEqual(propertyType, retrievedHotel.PropertyType);
            Assert.AreEqual(genderRestriction.ToString(), retrievedHotel.GenderRestriction);
            Assert.AreEqual(area, retrievedHotel.Area);
            Assert.AreEqual(monthlyRent, retrievedHotel.RentPrice);
            Assert.AreEqual(isShared, retrievedHotel.IsShared);
            Assert.AreEqual(rentUnit, retrievedHotel.RentUnit);
            Assert.AreEqual(internet, retrievedHotel.Internet);
            Assert.AreEqual(cableTv, retrievedHotel.CableTv);
            Assert.AreEqual(parking, retrievedHotel.Parking);
            Assert.AreEqual(ac, retrievedHotel.AC);
            Assert.AreEqual(geyser, retrievedHotel.Geyser);
            Assert.AreEqual(attachedBathroom, retrievedHotel.AttachedBathroom);
            Assert.AreEqual(fitnessCentre, retrievedHotel.FitnessCentre);
            Assert.AreEqual(backupElectricity, retrievedHotel.BackupElectricity);
            Assert.AreEqual(heating, retrievedHotel.Heating);
            Assert.AreEqual(airportShuttle, retrievedHotel.AirportShuttle);
            Assert.AreEqual(breakfastIncluded, retrievedHotel.BreakfastIncluded);
            Assert.AreEqual(numberOfAdults, retrievedHotel.Occupants.Adults);
            Assert.AreEqual(numberOfChildren, retrievedHotel.Occupants.Children);
            Assert.AreEqual(numberOfAdults + numberOfChildren, retrievedHotel.Occupants.TotalOccupants);
        }

        // Save multiple property types and check that different property types are retrievable when prompted
        // SEARCH BY PROPERTY TYPE
        // SEARCH BY AREA + PROPERTY TYPE
        // SEARCH BY EMAIL
        [Test]
        public void SearchByMultipleCriteriaTest_TestsThatSearchisPerformedAsExpected_VerifiesByOutput()
        {
            IPropertyApplicationService propertyApplicationService = _kernel.Get<IPropertyApplicationService>();

            // Saving Property # 1: House. Withing the search radius
            string area = "Pindora, Rawalpindi, Pakistan";
            string title = "Title # 1";
            string phoneNumber = "03990000001";
            string email = "special@spsp123456-1.com";
            int rent = 100;
            string ownerName = "Owner Name 1";
            string propertyType = Constants.House;
            string rentUnit = Constants.Hourly;
            CreateHouseCommand house = new CreateHouseCommand(title, rent, 3, 2, 3, true, true, true,
                false, false, propertyType, email, phoneNumber, null, null, area, null, null, 0, ownerName,
                null, null, false, rentUnit, null, null);
            var hosue1Id = propertyApplicationService.SaveNewProperty(JsonConvert.SerializeObject(house), email);

            // Saving Property # 2: Hotel. Near the search location, should be in the search results
            string area2 = "Satellite Town, Rawalpindi, Pakistan";
            string title2 = "Title No 2";
            string email2 = "w@12344321-2.com";
            string name2 = "OwnerName 2";
            string phoneNumber2 = "03990000002";
            string propertyType2 = Constants.Hotel;
            string rentUnit2 = Constants.Weekly;
            long monthlyRent2 = 92000;
            GenderRestriction genderRestriction2 = GenderRestriction.GirlsOnly;

            CreateHotelCommand hotel1 = new CreateHotelCommand(title2, monthlyRent2, false, false, false,
                propertyType2, email2, phoneNumber2, area2, name2, null, genderRestriction2.ToString(), false, rentUnit2, false,
                false, false, false, false, false, false, false, false, false, false, null, null, false, false, false,
                false, false, false, false, false, false, false, false, 2, 1);
            var hotel1Id = propertyApplicationService.SaveNewProperty(JsonConvert.SerializeObject(hotel1), email2);

            // Saving Property # 3: Apartment. Should be in the search results
            string area3 = "Bahria Town, Rawalpindi, Punjab, Pakistan";
            string title3 = "Title # 3";
            string email3 = "special2@spsp123456-3.com";
            string streetNo3 = "3";
            string phoneNumber3 = "03990000003";
            int rent3 = 93000;
            string ownerName3 = "Owner Name 3";
            string propertyType3 = Constants.Apartment;
            string rentUnit3 = Constants.Monthly;

            CreateHouseCommand apartment = new CreateHouseCommand(title2, rent3, 3, 2, 3, true, true, true,
                false, false, propertyType3, email3, phoneNumber3, null, null, area3, null, null, 0, ownerName3,
                null, null, false, rentUnit3, null, null);
            var apartment1Id = propertyApplicationService.SaveNewProperty(JsonConvert.SerializeObject(apartment), email3);

            // Saving House # 4: Guest House. Should be in the search results
            string area4 = "Lohi Bhair, Islamabad Capital Territory, Pakistan";
            string title4 = "Title No 4";
            string email4 = "w@12344321-4.com";
            string name4 = "OwnerName 4";
            string phoneNumber4 = "03990000004";
            string propertyType4 = Constants.GuestHouse;
            string rentUnit4 = Constants.Monthly;
            long monthlyRent4 = 94000;
            GenderRestriction genderRestriction4 = GenderRestriction.NoRestriction;

            CreateHotelCommand guestHouse1 = new CreateHotelCommand(title4, monthlyRent4, false, false, false,
                propertyType4, email4, phoneNumber4, area4, name4, null, genderRestriction4.ToString(), false, 
                rentUnit4, false,
                false, false, false, false, false, false, false, false, false, false, null, null, false, false, false,
                false, false, false, false, false, false, false, false, 2, 1);
            var guestHouse1Id = propertyApplicationService.SaveNewProperty(JsonConvert.SerializeObject(guestHouse1), email4);

            // Saving House # 5: Hostel. Should be in the search results
            string area5 = "Gulberg Greens, Gulberg, Islamabad Capital Territory, Pakistan";
            string title5 = "Title No 5";
            string email5 = "special@spsp123456-5.com";
            string name5 = "OwnerName 5";
            string phoneNumber5 = "03990000005";
            string propertyType5 = Constants.Hostel;
            string rentUnit5 = Constants.Daily;
            long monthlyRent5 = 95000;
            GenderRestriction genderRestriction5 = GenderRestriction.NoRestriction;
            CreateHostelCommand hostel = new CreateHostelCommand(title5, monthlyRent5, false, false, false,
                propertyType5, email5, phoneNumber5, area5, name5, null, genderRestriction5.ToString(), false,
                rentUnit5, false,
                false, false, false, false, false, false, false, false, false, false, null, null, false, false,
                false, 3);
            var hostel1Id = propertyApplicationService.SaveNewProperty(JsonConvert.SerializeObject(hostel), email5);

            // Saving Property # 6: House. Outside Bounds, should NOT be in search results
            string area6 = "Nara, Rawalpindi, Pakistan";
            string title6 = "Title # 6";
            string phoneNumber6 = "03990000006";
            string email6 = "special@spsp123456.com";
            int rent6 = 96000;
            string ownerName6 = "Owner Name 6";
            string propertyType6 = Constants.House;
            string rentUnit6 = Constants.Daily;
            CreateHouseCommand house2 = new CreateHouseCommand(title6, rent6, 3, 2, 3, true, true, true,
                false, false, propertyType6, email6, phoneNumber6, null, null, area6, null, null, 0, ownerName6,
                null, null, false, rentUnit6, null, null);
            var hosue2Id = propertyApplicationService.SaveNewProperty(JsonConvert.SerializeObject(house2), email6);

            // Saving Property # 7: Hotel. Outside bounds, should NOT be in the search results
            string area7 = "Choha Khalsa, Punjab, Pakistan";
            string title7 = "Title No 7";
            string email7 = "special@spsp123456.com";
            string name7 = "OwnerName 7";
            string phoneNumber7 = "03990000007";
            string propertyType7 = Constants.Hotel;
            string rentUnit7 = Constants.Weekly;
            long monthlyRent7 = 97000;
            GenderRestriction genderRestriction7 = GenderRestriction.FamiliesOnly;

            CreateHotelCommand hotel2 = new CreateHotelCommand(title7, monthlyRent7, false, false, false,
                propertyType7, email7, phoneNumber7, area7, name7, null, genderRestriction7.ToString(), false, 
                rentUnit7, false,
                false, false, false, false, false, false, false, false, false, false, null, null, false, false, false,
                false, false, false, false, false, false, false, false, 2, 1);
            var hotel2Id = propertyApplicationService.SaveNewProperty(JsonConvert.SerializeObject(hotel2), email7);

            // Saving Property # 8: Guest House. Outside bounds, should NOT be in the search results
            string area8 = "Patriata, Punjab, Pakistan";
            string title8 = "Title No 8";
            string email8 = "special@spsp123456.com";
            string name8 = "OwnerName 8";
            string phoneNumber8 = "03990000008";
            string propertyType8 = Constants.GuestHouse;
            string rentUnit8 = Constants.Weekly;
            long monthlyRent8 = 98000;
            GenderRestriction genderRestriction8 = GenderRestriction.FamiliesOnly;

            CreateHotelCommand guestHouse2 = new CreateHotelCommand(title8, monthlyRent8, false, false, false,
                propertyType8, email8, phoneNumber8, area8, name8, null, genderRestriction8.ToString(), false,
                rentUnit8, false,
                false, false, false, false, false, false, false, false, false, false, null, null, false, false, false,
                false, false, false, false, false, false, false, false, 2, 1);
            var guestHouse2Id = propertyApplicationService.SaveNewProperty(JsonConvert.SerializeObject(guestHouse2), email8);

            // Saving Property # 9: Aparment. Outside bounds, should NOT be in the search results
            string area9 = "Murree, Punjab, Pakistan";
            string title9 = "Title # 9";
            string email9 = "special@spsp123456.com";
            string phoneNumber9 = "03990000009";
            int rent9 = 99000;
            string ownerName9 = "Owner Name 9";
            string propertyType9 = Constants.Apartment;
            string rentUnit9 = Constants.Monthly;

            CreateHouseCommand apartment2 = new CreateHouseCommand(title9, rent9, 3, 2, 3, true, true, true,
                false, false, propertyType9, email9, phoneNumber9, null, null, area9, null, null, 0, ownerName9,
                null, null, false, rentUnit9, null, null);
            var apartment2Id = propertyApplicationService.SaveNewProperty(JsonConvert.SerializeObject(apartment2), email9);

            // Saving Property # 10: Hostel. Should be in the search results, again
            string area10 = "6th Road, Rawalpindi, Pakistan";
            string title10 = "Title No 10";
            string email10 = "special@spsp123456.com";
            string name10 = "OwnerName 5";
            string phoneNumber10 = "03990000010";
            string propertyType10 = Constants.Hostel;
            string rentUnit10 = Constants.Daily;
            long monthlyRent10 = 100000;
            GenderRestriction genderRestriction10 = GenderRestriction.GirlsOnly;

            CreateHostelCommand hostel2 = new CreateHostelCommand(title10, monthlyRent10, false, false, false,
                propertyType10, email10, phoneNumber10, area10, name10, null, genderRestriction10.ToString(), false,
                rentUnit10, false,
                false, false, false, false, false, false, false, false, false, false, null, null, false, false,
                false, 3);
            var hostel2Id = propertyApplicationService.SaveNewProperty(JsonConvert.SerializeObject(hostel2), email10);

            #region PROPERTY TYPE ONLY SEARCH

            // SEARCH BY PROPERTY TYPE ONLY
            // Verification of House & Apartments
            var retreivedHouses = propertyApplicationService.SearchPropertiesByPropertyType(Constants.House);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(4, retreivedHouses.Count);
            // House # 1
            Assert.AreEqual(hosue1Id, retreivedHouses[0].Id);
            Assert.AreEqual(title, retreivedHouses[0].Title);
            Assert.AreEqual(Constants.House, retreivedHouses[0].PropertyType);
            // Apartment # 1
            Assert.AreEqual(apartment1Id, retreivedHouses[1].Id);
            Assert.AreEqual(apartment.Title, retreivedHouses[1].Title);
            Assert.AreEqual(Constants.Apartment, retreivedHouses[1].PropertyType);
            // House # 2
            Assert.AreEqual(hosue2Id, retreivedHouses[2].Id);
            Assert.AreEqual(house2.Title, retreivedHouses[2].Title);
            Assert.AreEqual(Constants.House, retreivedHouses[2].PropertyType);
            // Apartment # 2
            Assert.AreEqual(apartment2Id, retreivedHouses[3].Id);
            Assert.AreEqual(apartment2.Title, retreivedHouses[3].Title);
            Assert.AreEqual(Constants.Apartment, retreivedHouses[3].PropertyType);

            // Verification of 2 Hostels
            var retreivedHostels = propertyApplicationService.SearchPropertiesByPropertyType(Constants.Hostel);
            Assert.NotNull(retreivedHostels);
            Assert.AreEqual(2, retreivedHostels.Count);
            // Hostel # 1
            Assert.AreEqual(hostel1Id, retreivedHostels[0].Id);
            Assert.AreEqual(hostel.Title, retreivedHostels[0].Title);
            Assert.AreEqual(Constants.Hostel, retreivedHostels[0].PropertyType);
            // Hostel # 2
            Assert.AreEqual(hostel2Id, retreivedHostels[1].Id);
            Assert.AreEqual(hostel2.Title, retreivedHostels[1].Title);
            Assert.AreEqual(Constants.Hostel, retreivedHostels[1].PropertyType);

            // Verification of Hotel & Guest House
            var retreivedHotels = propertyApplicationService.SearchPropertiesByPropertyType(Constants.Hotel);
            Assert.NotNull(retreivedHotels);
            Assert.AreEqual(4, retreivedHotels.Count);
            // Hotel
            Assert.AreEqual(hotel1Id, retreivedHotels[0].Id);
            Assert.AreEqual(hotel1.Title, retreivedHotels[0].Title);
            Assert.AreEqual(Constants.Hotel, retreivedHotels[0].PropertyType);
            // Guest House
            Assert.AreEqual(guestHouse1Id, retreivedHotels[1].Id);
            Assert.AreEqual(guestHouse1.Title, retreivedHotels[1].Title);
            Assert.AreEqual(Constants.GuestHouse, retreivedHotels[1].PropertyType);
            // Hotel
            Assert.AreEqual(hotel2Id, retreivedHotels[2].Id);
            Assert.AreEqual(hotel2.Title, retreivedHotels[2].Title);
            Assert.AreEqual(Constants.Hotel, retreivedHotels[2].PropertyType);
            // Guest House
            Assert.AreEqual(guestHouse2Id, retreivedHotels[3].Id);
            Assert.AreEqual(guestHouse2.Title, retreivedHotels[3].Title);
            Assert.AreEqual(Constants.GuestHouse, retreivedHotels[3].PropertyType);

            #endregion PROPERTY TYPE ONLY SEARCH

            #region AREA + PROPERTY TYPE ONLY SEARCH

            string searchLocation = "Pindora, Rawalpindi, Pakistan";

            // Verification of House & Apartments
            retreivedHouses = propertyApplicationService.SearchPropertiesByAreaAndPropertyType(searchLocation, 
                Constants.House);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(2, retreivedHouses.Count);
            // House # 1
            Assert.AreEqual(hosue1Id, retreivedHouses[0].Id);
            Assert.AreEqual(title, retreivedHouses[0].Title);
            Assert.AreEqual(Constants.House, retreivedHouses[0].PropertyType);
            // Apartment # 1
            Assert.AreEqual(apartment1Id, retreivedHouses[1].Id);
            Assert.AreEqual(apartment.Title, retreivedHouses[1].Title);
            Assert.AreEqual(Constants.Apartment, retreivedHouses[1].PropertyType);

            // Verification of 2 Hostels
            retreivedHostels = propertyApplicationService.SearchPropertiesByAreaAndPropertyType(searchLocation, 
                Constants.Hostel);
            Assert.NotNull(retreivedHostels);
            Assert.AreEqual(2, retreivedHostels.Count);
            // Hostel # 2 (because this location is near to search location)
            Assert.AreEqual(hostel2Id, retreivedHostels[0].Id);
            Assert.AreEqual(hostel2.Title, retreivedHostels[0].Title);
            Assert.AreEqual(Constants.Hostel, retreivedHostels[0].PropertyType);
            // Hostel # 1
            Assert.AreEqual(hostel1Id, retreivedHostels[1].Id);
            Assert.AreEqual(hostel.Title, retreivedHostels[1].Title);
            Assert.AreEqual(Constants.Hostel, retreivedHostels[1].PropertyType);

            // Verification of Hotel & Guest House
            retreivedHotels = propertyApplicationService.SearchPropertiesByAreaAndPropertyType(searchLocation, 
                Constants.Hotel);
            Assert.NotNull(retreivedHotels);
            Assert.AreEqual(2, retreivedHotels.Count);
            // Hotel
            Assert.AreEqual(hotel1Id, retreivedHotels[0].Id);
            Assert.AreEqual(hotel1.Title, retreivedHotels[0].Title);
            Assert.AreEqual(Constants.Hotel, retreivedHotels[0].PropertyType);
            // Guest House
            Assert.AreEqual(guestHouse1Id, retreivedHotels[1].Id);
            Assert.AreEqual(guestHouse1.Title, retreivedHotels[1].Title);
            Assert.AreEqual(Constants.GuestHouse, retreivedHotels[1].PropertyType);

            #endregion AREA + PROPERTY TYPE ONLY SEARCH

            #region EMAIL ONLY SEARCH

            // This email is shared by 5 properties
            string searchedEmail = "special@spsp123456.com";

            // Verification of House & Apartments
            retreivedHouses = propertyApplicationService.GetPropertiesByEmail(Constants.House, searchedEmail);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(2, retreivedHouses.Count);
            // House # 2
            Assert.AreEqual(hosue2Id, retreivedHouses[0].Id);
            Assert.AreEqual(house2.Title, retreivedHouses[0].Title);
            Assert.AreEqual(Constants.House, retreivedHouses[0].PropertyType);
            // Apartment # 2
            Assert.AreEqual(apartment2Id, retreivedHouses[1].Id);
            Assert.AreEqual(apartment2.Title, retreivedHouses[1].Title);
            Assert.AreEqual(Constants.Apartment, retreivedHouses[1].PropertyType);

            // Verification of Hostel # 2
            retreivedHostels = propertyApplicationService.GetPropertiesByEmail(Constants.Hostel, searchedEmail);
            Assert.NotNull(retreivedHostels);
            Assert.AreEqual(1, retreivedHostels.Count);
            Assert.AreEqual(hostel2Id, retreivedHostels[0].Id);
            Assert.AreEqual(hostel2.Title, retreivedHostels[0].Title);
            Assert.AreEqual(Constants.Hostel, retreivedHostels[0].PropertyType);

            // Verification of Hotel & Guest House
            retreivedHotels = propertyApplicationService.GetPropertiesByEmail(Constants.Hotel, searchedEmail);
            Assert.NotNull(retreivedHotels);
            Assert.AreEqual(2, retreivedHotels.Count);
            // Hotel # 2
            Assert.AreEqual(hotel2Id, retreivedHotels[0].Id);
            Assert.AreEqual(hotel2.Title, retreivedHotels[0].Title);
            Assert.AreEqual(Constants.Hotel, retreivedHotels[0].PropertyType);
            // Guest House # 2
            Assert.AreEqual(guestHouse2Id, retreivedHotels[1].Id);
            Assert.AreEqual(guestHouse2.Title, retreivedHotels[1].Title);
            Assert.AreEqual(Constants.GuestHouse, retreivedHotels[1].PropertyType);

            #endregion EMAIL ONLY SEARCH
        }

        #endregion Save House and retrieve Partial Representations

        #region Delete Tests

        // DELETE HOUSE
        [Test]
        public void DeleteHouseTest_ChecksThatAHouseIsDeletedFromTheDatabaseSuccessfully_VerifiesByOutputResult()
        {
            IPropertyApplicationService propertyApplicationService = _kernel.Get<IPropertyApplicationService>();
            
            string area = "Pindora, Rawalpindi, Pakistan";
            string title = "Title # 1";
            string phoneNumber = "03990000001";
            string email = "special@spsp123456-1.com";
            int rent = 100;
            string ownerName = "Owner Name 1";
            string propertyType = Constants.House;
            string rentUnit = Constants.Hourly;
            CreateHouseCommand house = new CreateHouseCommand(title, rent, 3, 2, 3, true, true, true,
                false, false, propertyType, email, phoneNumber, null, null, area, null, null, 0, ownerName,
                null, null, false, rentUnit, null, null);
            var hosue1Id = propertyApplicationService.SaveNewProperty(JsonConvert.SerializeObject(house), email);

            var retrievedHouse = propertyApplicationService.GetPropertyById(hosue1Id, propertyType);
            Assert.IsNotNull(retrievedHouse);

            propertyApplicationService.DeleteHouse(hosue1Id, email);

            retrievedHouse = propertyApplicationService.GetPropertyById(hosue1Id, propertyType);
            Assert.IsNull(retrievedHouse);
        }

        // DELETE HOSTEL
        [Test]
        public void DeleteHostelTest_ChecksThatAHostelIsDeletedFromTheDatabaseSuccessfully_VerifiesByOutputResult()
        {
            IPropertyApplicationService propertyApplicationService = _kernel.Get<IPropertyApplicationService>();

            string area5 = "Gulberg Greens, Gulberg, Islamabad Capital Territory, Pakistan";
            string title5 = "Title No 5";
            string email5 = "special@spsp123456-5.com";
            string name5 = "OwnerName 5";
            string phoneNumber5 = "03990000005";
            string propertyType5 = Constants.Hostel;
            string rentUnit5 = Constants.Daily;
            long monthlyRent5 = 95000;
            GenderRestriction genderRestriction5 = GenderRestriction.NoRestriction;
            CreateHostelCommand hostel = new CreateHostelCommand(title5, monthlyRent5, false, false, false,
                propertyType5, email5, phoneNumber5, area5, name5, null, genderRestriction5.ToString(), false,
                rentUnit5, false,
                false, false, false, false, false, false, false, false, false, false, null, null, false, false,
                false, 3);
            var hostelId = propertyApplicationService.SaveNewProperty(JsonConvert.SerializeObject(hostel), email5);

            var retrievedHouse = propertyApplicationService.GetPropertyById(hostelId, propertyType5);
            Assert.IsNotNull(retrievedHouse);

            propertyApplicationService.DeleteHouse(hostelId, email5);

            retrievedHouse = propertyApplicationService.GetPropertyById(hostelId, propertyType5);
            Assert.IsNull(retrievedHouse);
        }

        // DELETE HOTEL
        [Test]
        public void DeleteHotelTest_ChecksThatAHotelIsDeletedFromTheDatabaseSuccessfully_VerifiesByOutputResult()
        {
            IPropertyApplicationService propertyApplicationService = _kernel.Get<IPropertyApplicationService>();

            string area2 = "Satellite Town, Rawalpindi, Pakistan";
            string title2 = "Title No 2";
            string email2 = "w@12344321-2.com";
            string name2 = "OwnerName 2";
            string phoneNumber2 = "03990000002";
            string propertyType2 = Constants.Hotel;
            string rentUnit2 = Constants.Weekly;
            long monthlyRent2 = 92000;
            GenderRestriction genderRestriction2 = GenderRestriction.GirlsOnly;

            CreateHotelCommand hotel1 = new CreateHotelCommand(title2, monthlyRent2, false, false, false,
                propertyType2, email2, phoneNumber2, area2, name2, null, genderRestriction2.ToString(), false, rentUnit2, false,
                false, false, false, false, false, false, false, false, false, false, null, null, false, false, false,
                false, false, false, false, false, false, false, false, 2, 1);
            var hotel1Id = propertyApplicationService.SaveNewProperty(JsonConvert.SerializeObject(hotel1), email2);

            var retrievedHouse = propertyApplicationService.GetPropertyById(hotel1Id, propertyType2);
            Assert.IsNotNull(retrievedHouse);

            propertyApplicationService.DeleteHouse(hotel1Id, email2);

            retrievedHouse = propertyApplicationService.GetPropertyById(hotel1Id, propertyType2);
            Assert.IsNull(retrievedHouse);
        }

        #endregion Delete Tests

        #region Update Tests

        // Save and then Update a House
        [Test]
        public void UpdateHouseTest_TestsThatANewHouseIsSavedInTheDatabaseAndThenUpdatedAsExpected_VerifiesByOutput()
        {
            IPropertyApplicationService propertyApplicationService = _kernel.Get<IPropertyApplicationService>();

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
            string propertyType = Constants.House;
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
            string houseId = propertyApplicationService.SaveNewProperty(JsonConvert.SerializeObject(createNewHouseCommand), email);

            HouseFullRepresentation retreivedHouse =
                (HouseFullRepresentation)propertyApplicationService.GetPropertyById(houseId, Constants.House);

            Assert.NotNull(retreivedHouse);

            // Update variables
            string updatedPhoneNumber = "01234567891";
            string updatedDescription = "Erebor Updated. Built deep within the mountain itself the beauty of this fortress was legend.";
            long updatedMonthlyRent = 130001;
            int updatedNumberOfBedrooms = 4;
            int updatedNumberofBathrooms = 2;
            int updatedNumberOfKitchens = 2;
            string updatedHouseNo = "747-2";
            string updatedStreetNo = "13-2";
            string updatedTitle = "Bellagio Updated";
            string updatedArea = "Pindora, Rawalpindi, Pakistan";
            string updatedDimensionType = "Marla";
            string updatedDimensionString = "100";
            int updatedDimensionIntValue = 0;
            string updatedOwnerName = "Owner Name 1 Updated";
            string updatedPropertyType = Constants.House;
            string updatedGenderRestriction = GenderRestriction.BoysOnly.ToString();
            bool updatedSmokingAllowed = false;
            bool updatedLandline = true;
            bool updatedCableTv = false;
            bool updatedInternet = true;
            bool updatedGarage = true;
            bool updatedIsShared = true;
            string updatedRentUnit = "Hour";
            string updatedLandlineNumber = "0510000000";
            string updatedFax = "0510000000";
            var updateHouseCommand = new UpdateHouseCommand(retreivedHouse.Id, updatedTitle, updatedMonthlyRent,
                updatedNumberOfBedrooms, updatedNumberOfKitchens, updatedNumberofBathrooms, updatedInternet,
                updatedLandline, updatedCableTv, updatedGarage, updatedSmokingAllowed, updatedPropertyType,
                email, updatedPhoneNumber, updatedHouseNo, updatedStreetNo, updatedArea,
                updatedDimensionType, updatedDimensionString, updatedDimensionIntValue, updatedOwnerName,
                updatedDescription, updatedGenderRestriction, updatedIsShared, updatedRentUnit, 
                updatedLandlineNumber, updatedFax);
            propertyApplicationService.UpdateProperty(JsonConvert.SerializeObject(updateHouseCommand), email);

            // Now Update the House
            var updatedRetrievedHouse = (HouseFullRepresentation)propertyApplicationService.GetPropertyById(retreivedHouse.Id, propertyType);
            Assert.IsNotNull(updatedRetrievedHouse);

            Assert.AreEqual(retreivedHouse.Id, updatedRetrievedHouse.Id);
            Assert.AreEqual(email, updatedRetrievedHouse.OwnerEmail);
            Assert.AreEqual(updatedPhoneNumber, updatedRetrievedHouse.OwnerPhoneNumber);
            Assert.AreEqual(updatedOwnerName, updatedRetrievedHouse.OwnerName);
            Assert.AreEqual(updatedDescription, updatedRetrievedHouse.Description);
            Assert.AreEqual(updatedHouseNo, updatedRetrievedHouse.HouseNo);
            Assert.AreEqual(updatedStreetNo, updatedRetrievedHouse.StreetNo);
            Assert.AreEqual(updatedTitle, updatedRetrievedHouse.Title);
            Assert.AreEqual(updatedArea, updatedRetrievedHouse.Area);
            Assert.AreEqual(updatedDimensionString + " " + updatedDimensionType, updatedRetrievedHouse.Dimension);
            Assert.AreEqual(updatedNumberofBathrooms, updatedRetrievedHouse.NumberOfBathrooms);
            Assert.AreEqual(updatedNumberOfBedrooms, updatedRetrievedHouse.NumberOfBedrooms);
            Assert.AreEqual(updatedNumberOfKitchens, updatedRetrievedHouse.NumberOfKitchens);
            Assert.AreEqual(updatedPropertyType, updatedRetrievedHouse.PropertyType);
            Assert.AreEqual(updatedMonthlyRent, updatedRetrievedHouse.RentPrice);
            Assert.AreEqual(updatedGenderRestriction, updatedRetrievedHouse.GenderRestriction);
            Assert.AreEqual(updatedInternet, updatedRetrievedHouse.InternetAvailable);
            Assert.AreEqual(updatedGarage, updatedRetrievedHouse.GarageAvailable);
            Assert.AreEqual(updatedLandline, updatedRetrievedHouse.LandlinePhoneAvailable);
            Assert.AreEqual(updatedCableTv, updatedRetrievedHouse.CableTvAvailable);
            Assert.AreEqual(updatedSmokingAllowed, updatedRetrievedHouse.SmokingAllowed);
            Assert.AreEqual(updatedIsShared, updatedRetrievedHouse.IsShared);
            Assert.AreEqual(updatedRentUnit, updatedRetrievedHouse.RentUnit);
            Assert.AreEqual(updatedLandlineNumber, updatedRetrievedHouse.LandlineNumber);
            Assert.AreEqual(updatedFax, updatedRetrievedHouse.Fax);
        }

        // Update a Hostel
        [Test]
        public void UpdateHostelTest_ChecksThatANewHostelIsSavedAndThenUpdatedAsExpected_VerifiesByTheReturnValue()
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
            string rentUnit = Constants.Hourly;
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

            var createNewHostelCommand = new CreateHostelCommand(title, monthlyRent, internet,
                cableTv, parking, propertyType, email, phoneNumber, area, name, description,
                genderRestriction.ToString(),
                isShared, rentUnit, laundry, ac, geyser, fitnessCentre, attachedBathroom, ironing, balcony, lawn,
                cctvCameras, backupElectricity, heating, landlineNumber, fax, elevator, meals, picknDrop,
                numberOfSeats);
            var hostelId = propertyApplicationService.SaveNewProperty(JsonConvert.SerializeObject(createNewHostelCommand), email);

            Assert.IsFalse(string.IsNullOrWhiteSpace(hostelId));
            // Now retrieve the Hostel from the database
            HostelFullRepresentation retrievedHostel =
                (HostelFullRepresentation)propertyApplicationService.GetPropertyById(hostelId, Constants.Hostel);
            Assert.IsNotNull(retrievedHostel);

            string updatedTitle = "Title No 1 - updated";
            string updatedDescription = "Description of Hostel - updated";
            string updatedName = "OwnerName";
            string updatedPhoneNumber = "03123456789";
            string updatedPropertyType = "Hostel";
            GenderRestriction updatedGenderRestriction = GenderRestriction.NoRestriction;
            string updatedArea = "Satellite Town, Rawalpindi, Pakistan";
            long updatedMonthlyRent = 90001;
            bool updatedCableTv = true;
            bool updatedInternet = false;
            bool updatedParking = false;
            string updatedRentUnit = Constants.Daily;
            bool updatedIsShared = false;
            bool updatedLaundry = false;
            bool updatedAc = false;
            bool updatedGeyser = false;
            bool updatedAttachedBathroom = false;
            bool updatedFitnessCentre = true;
            bool updatedBalcony = true;
            bool updatedLawn = false;
            bool updatedHeating = true;
            bool updatedMeals = false;
            bool updatedPicknDrop = true;
            bool updatedIroning = true;
            bool updatedCctvCameras = false;
            bool updatedBackupElectricity = false;
            int updatedNumberOfSeats = 2;
            string updatedLandlineNumber = "0510000001";
            string updatedFax = "0510000001";
            bool updatedElevator = true;

            var updateHostelCommand = new UpdateHostelCommand(retrievedHostel.Id, updatedTitle, updatedMonthlyRent,
                updatedInternet, updatedCableTv, updatedParking, updatedPropertyType, email, 
                updatedPhoneNumber, updatedArea, updatedName, updatedDescription, updatedGenderRestriction.ToString(), 
                updatedIsShared, updatedRentUnit, updatedLaundry, updatedAc, updatedGeyser, updatedFitnessCentre,
                updatedAttachedBathroom, updatedIroning, updatedBalcony, updatedLawn, updatedCctvCameras, 
                updatedBackupElectricity, updatedHeating, updatedLandlineNumber, updatedFax, updatedElevator,
                updatedMeals, updatedPicknDrop, updatedNumberOfSeats);
            // Now update the Hostel
            propertyApplicationService.UpdateProperty(JsonConvert.SerializeObject(updateHostelCommand), email);
            
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

        // Update a Hotel
        [Test]
        public void UpdateHotelTest_ChecksThatANewHostelIsSavedAndThenUpdatedAsExpected_VerifiesByTheReturnValue()
        {
            IPropertyApplicationService propertyApplicationService = _kernel.Get<IPropertyApplicationService>();

            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            string propertyType = Constants.Hotel;
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
            string area = "Pindora, Rawalpindi, Pakistan";
            long monthlyRent = 90000;
            bool cableTv = false;
            bool internet = true;
            bool parking = true;
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

            var createNewHotelCommand = new CreateHotelCommand(title, monthlyRent, internet,
                cableTv, parking, propertyType, email, phoneNumber, area, name, description,
                genderRestriction.ToString(),
                isShared, rentUnit, laundry, ac, geyser, fitnessCentre, attachedBathroom, ironing, balcony,
                lawn, cctvCameras, backupElectricity, heating, landlineNumber, fax, elevator, restaurant,
                airportShuttle, breakfastIncluded, sittingArea, carRental, spa, salon, bathtub, swimmingPool,
                kitchen, numberOfAdults, numberOfChildren);
            // Save the Hotel
            var hotelId = propertyApplicationService.SaveNewProperty(JsonConvert.SerializeObject(createNewHotelCommand), email);

            // Retreive the Hotel
            HotelFullRepresentation retrievedHotel =
                (HotelFullRepresentation)propertyApplicationService.GetPropertyById(hotelId, Constants.Hotel);

            Assert.IsNotNull(retrievedHotel);

            // Declare update variables
            string updatedTitle = "Title No 2";
            string updatedDescription = "Description of Hotel - Updated";
            string updatedName = "OwnerName updated";
            string updatedPhoneNumber = "03123456789";
            string updatedPropertyType = Constants.Hotel;
            GenderRestriction updatedGenderRestriction = GenderRestriction.NoRestriction;
            string updatedArea = "Bahria Town, Rawalpindi, Pakistan";
            long updatedMonthlyRent = 90001;
            bool updatedCableTv = true;
            bool updatedInternet = false;
            bool updatedParking = false;
            string updatedRentUnit = Constants.Daily;
            bool updatedIsShared = false;
            bool updatedLaundry = false;
            bool updatedAc = false;
            bool updatedGeyser = false;
            bool updatedAttachedBathroom = false;
            bool updatedFitnessCentre = true;
            bool updatedBalcony = true;
            bool updatedLawn = false;
            bool updatedHeating = true;
            bool updatedIroning = true;
            bool updatedCctvCameras = false;
            bool updatedBackupElectricity = false;

            bool updatedRestaurant = true;
            bool updatedAirportShuttle = true;
            bool updatedBreakfastIncluded = true;
            bool updatedSittingArea = true;
            bool updatedCarRental = true;
            bool updatedSpa = false;
            bool updatedSalon = false;
            bool updatedBathtub = false;
            bool updatedSwimmingPool = false;
            bool updatedKitchen = false;
            int updatedNumberOfAdults = 3;
            int updatedNumberOfChildren = 0;

            string updatedLandlineNumber = "0510000001";
            string updatedFax = "0510000001";
            bool updatedElevator = true;

            var updateHotelCommand = new UpdateHotelCommand(hotelId, updatedTitle, updatedMonthlyRent, updatedInternet,
                updatedCableTv, updatedParking, updatedPropertyType, email, updatedPhoneNumber, updatedArea,
                updatedName, updatedDescription, updatedGenderRestriction.ToString(), updatedIsShared, updatedRentUnit,
                updatedLaundry, updatedAc, updatedGeyser, updatedFitnessCentre, updatedAttachedBathroom,
                updatedIroning, updatedBalcony, updatedLawn, updatedCctvCameras, updatedBackupElectricity,
                updatedHeating, updatedLandlineNumber, updatedFax, updatedElevator, updatedRestaurant,
                updatedAirportShuttle, updatedBreakfastIncluded, updatedSittingArea, updatedCarRental, updatedSpa,
                updatedSalon, updatedBathtub, updatedSwimmingPool, updatedKitchen, updatedNumberOfAdults,
                updatedNumberOfChildren);

            // Update the Hotel
            propertyApplicationService.UpdateProperty(JsonConvert.SerializeObject(updateHotelCommand), email);

            // Retreive the Hotel
            HotelFullRepresentation retrievedUpdatedHotel =
                (HotelFullRepresentation)propertyApplicationService.GetPropertyById(hotelId, Constants.Hotel);

            Assert.AreEqual(hotelId, retrievedUpdatedHotel.Id);
            Assert.AreEqual(updatedTitle, retrievedUpdatedHotel.Title);
            Assert.AreEqual(updatedDescription, retrievedUpdatedHotel.Description);
            Assert.AreEqual(email, retrievedUpdatedHotel.OwnerEmail);
            Assert.AreEqual(updatedName, retrievedUpdatedHotel.OwnerName);
            Assert.AreEqual(updatedPhoneNumber, retrievedUpdatedHotel.OwnerPhoneNumber);
            Assert.AreEqual(updatedPropertyType, retrievedUpdatedHotel.PropertyType);
            Assert.AreEqual(updatedGenderRestriction.ToString(), retrievedUpdatedHotel.GenderRestriction);
            Assert.AreEqual(updatedArea, retrievedUpdatedHotel.Area);
            Assert.AreEqual(updatedMonthlyRent, retrievedUpdatedHotel.RentPrice);
            Assert.AreEqual(updatedCableTv, retrievedUpdatedHotel.CableTvAvailable);
            Assert.AreEqual(updatedInternet, retrievedUpdatedHotel.InternetAvailable);
            Assert.AreEqual(updatedParking, retrievedUpdatedHotel.ParkingAvailable);
            Assert.AreEqual(updatedRentUnit, retrievedUpdatedHotel.RentUnit);
            Assert.AreEqual(updatedIsShared, retrievedUpdatedHotel.IsShared);
            Assert.AreEqual(updatedLaundry, retrievedUpdatedHotel.Laundry);
            Assert.AreEqual(updatedAc, retrievedUpdatedHotel.AC);
            Assert.AreEqual(updatedGeyser, retrievedUpdatedHotel.Geyser);
            Assert.AreEqual(updatedAttachedBathroom, retrievedUpdatedHotel.AttachedBathroom);
            Assert.AreEqual(updatedFitnessCentre, retrievedUpdatedHotel.FitnessCentre);
            Assert.AreEqual(updatedBalcony, retrievedUpdatedHotel.Balcony);
            Assert.AreEqual(updatedLawn, retrievedUpdatedHotel.Lawn);
            Assert.AreEqual(updatedIroning, retrievedUpdatedHotel.Ironing);
            Assert.AreEqual(updatedCctvCameras, retrievedUpdatedHotel.CctvCameras);
            Assert.AreEqual(updatedBackupElectricity, retrievedUpdatedHotel.BackupElectricity);
            Assert.AreEqual(updatedHeating, retrievedUpdatedHotel.Heating);

            Assert.AreEqual(updatedRestaurant, retrievedUpdatedHotel.Restaurant);
            Assert.AreEqual(updatedAirportShuttle, retrievedUpdatedHotel.AirportShuttle);
            Assert.AreEqual(updatedBreakfastIncluded, retrievedUpdatedHotel.BreakfastIncluded);
            Assert.AreEqual(updatedSittingArea, retrievedUpdatedHotel.SittingArea);
            Assert.AreEqual(updatedCarRental, retrievedUpdatedHotel.CarRental);
            Assert.AreEqual(updatedSpa, retrievedUpdatedHotel.Spa);
            Assert.AreEqual(updatedSalon, retrievedUpdatedHotel.Salon);
            Assert.AreEqual(updatedBathtub, retrievedUpdatedHotel.Bathtub);
            Assert.AreEqual(updatedSwimmingPool, retrievedUpdatedHotel.SwimmingPool);
            Assert.AreEqual(updatedKitchen, retrievedUpdatedHotel.Kitchen);

            Assert.AreEqual(updatedNumberOfAdults, retrievedUpdatedHotel.Occupants.Adults);
            Assert.AreEqual(updatedNumberOfChildren, retrievedUpdatedHotel.Occupants.Children);
            Assert.AreEqual(updatedNumberOfAdults + updatedNumberOfChildren, retrievedUpdatedHotel.Occupants.TotalOccupants);

            Assert.AreEqual(updatedLandlineNumber, retrievedUpdatedHotel.LandlineNumber);
            Assert.AreEqual(updatedFax, retrievedUpdatedHotel.Fax);
            Assert.AreEqual(updatedElevator, retrievedUpdatedHotel.Elevator);
        }

        #endregion Update Tests
    }
}
