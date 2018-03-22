using Newtonsoft.Json;
using Ninject;
using NUnit.Framework;
using RentStuff.Common.Ninject.Modules;
using RentStuff.Common.Utilities;
using RentStuff.Property.Application.Ninject.Modules;
using RentStuff.Property.Application.PropertyServices.Commands.CreateCommands;
using RentStuff.Property.Application.PropertyServices.Commands.UpdateCommands;
using RentStuff.Property.Application.PropertyServices.Representation;
using RentStuff.Property.Application.PropertyServices.Representation.AbstractRepresentations;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Domain.Model.PropertyAggregate;
using RentStuff.Property.Infrastructure.Persistence.Ninject.Modules;
using RentStuff.Property.Ports.Adapter.Rest.Ninject.Modules;
using RentStuff.Property.Ports.Adapter.Rest.Resources;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Results;
using RentStuff.Property.Domain.Model.HotelAggregate;

namespace RentStuff.Property.Ports.Tests
{
    [TestFixture]
    public class PropertyControllerTests
    {
        // Flag which allows tests to run or not, as these tests call Google APIs which has a threshold limie. So we want to be sure
        // that we are not exhausting resources by unknowingly running such tests
        private bool _runTests = true;
        private DatabaseUtility _databaseUtility;
        private StandardKernel _kernel;

        [SetUp]
        public void Setup()
        {
            var connection = StringCipher.DecipheredConnectionString;
            _databaseUtility = new DatabaseUtility(connection);
            _databaseUtility.Create();

            _kernel = new StandardKernel();
            _kernel.Load<PropertyPersistenceNinjectModule>();
            _kernel.Load<CommonNinjectModule>();
            _kernel.Load<PropertyApplicationNinjectModule>();
            _kernel.Load<PropertyPortsNinjectModule>();
            //_databaseUtility.Populate();
            //ShowNhibernateLogging();
            //NhConnectionDecipherService.SetupDecipheredConnectionString();
        }

        [TearDown]
        public void Teardown()
        {
            _databaseUtility.Create();
        }

        #region Save Tests

        // Save & Get House by Id
        [Category("Integration")]
        [Test]
        public void SaveAndGetHouseInstanceByIdTest_TestsThatHouseIsSavedThenUpdatedAndRetreivedAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);

                string title = "House For Rent";
                string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
                int rent = 105000;
                string ownerEmail = "thorin@oakenshield123.com";
                string ownerPhoneNumber = "01234567890";
                string houseNo = "CT-141/A";
                string streetNo = "14";
                int numberOfBathrooms = 1;
                int numberOfBedrooms = 1;
                int numberOfKitchens = 1;
                bool familiesOnly = false;
                bool boysOnly = false;
                bool girlsOnly = true;
                bool internetAvailable = true;
                bool landlinePhoneAvailable = true;
                bool cableTvAvailable = true;
                bool garageAvailable = true;
                bool smokingAllowed = true;
                string propertyType = Constants.House;
                string area = "Pindora, Rawalpindi, Pakistan";
                //string area = "Pindora, Rawalpindi, Pakistan";
                string dimensionType = DimensionType.Kanal.ToString();
                string dimensionString = "5";
                decimal dimensionDecimal = 0;
                string ownerName = "Owner Name 1";
                string genderRestriction = GenderRestriction.FamiliesOnly.ToString();
                bool isShared = true;
                string rentUnit = House.GetAllRentUnits()[0];
                string landlineNumber = "0510000000";
                string fax = "0510000000";

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail)
                    })
                });
                CreateHouseCommand house = new CreateHouseCommand(title, rent, numberOfBedrooms, numberOfKitchens,
                    numberOfBathrooms, internetAvailable, landlinePhoneAvailable,
                    cableTvAvailable, garageAvailable, smokingAllowed, propertyType, ownerEmail, ownerPhoneNumber,
                    houseNo, streetNo, area, dimensionType, dimensionString, dimensionDecimal, ownerName, description, genderRestriction,
                    isShared, rentUnit, landlineNumber, fax);
                IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(house));
                string houseId = ((OkNegotiatedContentResult<string>)houseSaveResult).Content;

                IHttpActionResult response = (IHttpActionResult)houseController.Get(houseId: houseId, propertyType: propertyType);
                dynamic retreivedHouse = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)response).Content;
                Assert.NotNull(retreivedHouse);
                Assert.AreEqual(houseId, retreivedHouse.Id);
                Assert.AreEqual(title, retreivedHouse.Title);
                Assert.AreEqual(description, retreivedHouse.Description);
                Assert.AreEqual(ownerPhoneNumber, retreivedHouse.OwnerPhoneNumber);
                Assert.AreEqual(ownerEmail, retreivedHouse.OwnerEmail);
                Assert.AreEqual(rent, retreivedHouse.RentPrice);
                Assert.AreEqual(numberOfBathrooms, retreivedHouse.NumberOfBathrooms);
                Assert.AreEqual(numberOfBedrooms, retreivedHouse.NumberOfBedrooms);
                Assert.AreEqual(numberOfKitchens, retreivedHouse.NumberOfKitchens);
                Assert.AreEqual(garageAvailable, retreivedHouse.GarageAvailable);
                Assert.AreEqual(landlinePhoneAvailable, retreivedHouse.LandlinePhoneAvailable);
                Assert.AreEqual(cableTvAvailable, retreivedHouse.CableTvAvailable);
                Assert.AreEqual(internetAvailable, retreivedHouse.InternetAvailable);
                Assert.AreEqual(smokingAllowed, retreivedHouse.SmokingAllowed);
                Assert.AreEqual(propertyType, retreivedHouse.PropertyType);
                Assert.AreEqual(houseNo, retreivedHouse.HouseNo);
                Assert.AreEqual(isShared, retreivedHouse.IsShared);
                Assert.AreEqual(area, retreivedHouse.Area);
                Assert.AreEqual(streetNo, retreivedHouse.StreetNo);
                Assert.AreEqual(dimensionString + " " + dimensionType, retreivedHouse.Dimension);
                Assert.AreEqual(ownerName, retreivedHouse.OwnerName);
                Assert.AreEqual(genderRestriction, retreivedHouse.GenderRestriction);
                Assert.AreEqual(rentUnit, retreivedHouse.RentUnit);
                Assert.AreEqual(landlineNumber, retreivedHouse.LandlineNumber);
                Assert.AreEqual(fax, retreivedHouse.Fax);
            }
        }

        // Save & Get Apartment by Id
        [Category("Integration")]
        [Test]
        public void SaveAndGetApartmentInstanceByIdTest_TestsThatApartmentIsSavedThenUpdatedAndRetreivedAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);

                string title = "House For Rent";
                string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
                int rent = 105000;
                string ownerEmail = "thorin@oakenshield123.com";
                string ownerPhoneNumber = "01234567890";
                string houseNo = "CT-141/A";
                string streetNo = "14";
                int numberOfBathrooms = 1;
                int numberOfBedrooms = 1;
                int numberOfKitchens = 1;
                bool familiesOnly = false;
                bool boysOnly = false;
                bool girlsOnly = true;
                bool internetAvailable = true;
                bool landlinePhoneAvailable = true;
                bool cableTvAvailable = true;
                bool garageAvailable = true;
                bool smokingAllowed = true;
                string propertyType = Constants.Apartment;
                string area = "Pindora, Rawalpindi, Pakistan";
                //string area = "Pindora, Rawalpindi, Pakistan";
                string dimensionType = DimensionType.Kanal.ToString();
                string dimensionString = "5";
                decimal dimensionDecimal = 0;
                string ownerName = "Owner Name 1";
                string genderRestriction = GenderRestriction.FamiliesOnly.ToString();
                bool isShared = true;
                string rentUnit = House.GetAllRentUnits()[0];
                string landlineNumber = "0510000000";
                string fax = "0510000000";

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail)
                    })
                });
                CreateHouseCommand house = new CreateHouseCommand(title, rent, numberOfBedrooms, numberOfKitchens,
                    numberOfBathrooms, internetAvailable, landlinePhoneAvailable,
                    cableTvAvailable, garageAvailable, smokingAllowed, propertyType, ownerEmail, ownerPhoneNumber,
                    houseNo, streetNo, area, dimensionType, dimensionString, dimensionDecimal, ownerName, description, genderRestriction,
                    isShared, rentUnit, landlineNumber, fax);
                IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(house));
                string houseId = ((OkNegotiatedContentResult<string>)houseSaveResult).Content;

                IHttpActionResult response = (IHttpActionResult)houseController.Get(houseId: houseId, propertyType: propertyType);
                dynamic retreivedHouse = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)response).Content;
                Assert.NotNull(retreivedHouse);
                Assert.AreEqual(houseId, retreivedHouse.Id);
                Assert.AreEqual(title, retreivedHouse.Title);
                Assert.AreEqual(description, retreivedHouse.Description);
                Assert.AreEqual(ownerPhoneNumber, retreivedHouse.OwnerPhoneNumber);
                Assert.AreEqual(ownerEmail, retreivedHouse.OwnerEmail);
                Assert.AreEqual(rent, retreivedHouse.RentPrice);
                Assert.AreEqual(numberOfBathrooms, retreivedHouse.NumberOfBathrooms);
                Assert.AreEqual(numberOfBedrooms, retreivedHouse.NumberOfBedrooms);
                Assert.AreEqual(numberOfKitchens, retreivedHouse.NumberOfKitchens);
                Assert.AreEqual(garageAvailable, retreivedHouse.GarageAvailable);
                Assert.AreEqual(landlinePhoneAvailable, retreivedHouse.LandlinePhoneAvailable);
                Assert.AreEqual(cableTvAvailable, retreivedHouse.CableTvAvailable);
                Assert.AreEqual(internetAvailable, retreivedHouse.InternetAvailable);
                Assert.AreEqual(smokingAllowed, retreivedHouse.SmokingAllowed);
                Assert.AreEqual(propertyType, retreivedHouse.PropertyType);
                Assert.AreEqual(houseNo, retreivedHouse.HouseNo);
                Assert.AreEqual(isShared, retreivedHouse.IsShared);
                Assert.AreEqual(area, retreivedHouse.Area);
                Assert.AreEqual(streetNo, retreivedHouse.StreetNo);
                Assert.AreEqual(dimensionString + " " + dimensionType, retreivedHouse.Dimension);
                Assert.AreEqual(ownerName, retreivedHouse.OwnerName);
                Assert.AreEqual(genderRestriction, retreivedHouse.GenderRestriction);
                Assert.AreEqual(rentUnit, retreivedHouse.RentUnit);
                Assert.AreEqual(landlineNumber, retreivedHouse.LandlineNumber);
                Assert.AreEqual(fax, retreivedHouse.Fax);
            }
        }

        // Save & Get Hostel by Id
        [Category("Integration")]
        [Test]
        public void SaveAndGetHostelByIdTest_TestsThatHouseIsSavedAndRetreivedAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);

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

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, email)
                    })
                });

                var postHttpActionResult = houseController.Post(JsonConvert.SerializeObject(createNewHostelCommand));
                string hostelId = ((OkNegotiatedContentResult<string>)postHttpActionResult).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hostelId));

                var retrievedHostelHttpResult = houseController.Get(houseId: hostelId, propertyType: propertyType);
                dynamic retrievedHostel = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)retrievedHostelHttpResult).Content;
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
        }

        /// <summary>
        /// Save and Get Hotel by Id
        /// </summary>
        [Test]
        public void SaveNewHotelTest_ChecksThatANewHotelIsSavedAndRetrievedAsExpected_VerifiesByTheReturnValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);
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
                int numberOfSingleBeds = 1;
                int numberOfDoubleBeds = 2;

                string landlineNumber = "0510000000";
                string fax = "0510000000";
                bool elevator = false;
                
                var createNewHotelCommand = new CreateHotelCommand(title, monthlyRent, internet,
                    cableTv, parking, propertyType, email, phoneNumber, area, name, description,
                    genderRestriction.ToString(),
                    isShared, rentUnit, laundry, ac, geyser, fitnessCentre, attachedBathroom, ironing, balcony,
                    lawn, cctvCameras, backupElectricity, heating, landlineNumber, fax, elevator, restaurant,
                    airportShuttle, breakfastIncluded, sittingArea, carRental, spa, salon, bathtub, swimmingPool,
                    kitchen, numberOfSingleBeds, numberOfDoubleBeds, new Occupants(numberOfAdults, numberOfChildren));

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, email)
                    })
                });

                var postHttpActionResult = houseController.Post(JsonConvert.SerializeObject(createNewHotelCommand));
                string hotelId = ((OkNegotiatedContentResult<string>)postHttpActionResult).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hotelId));

                var retrievedHostelHttpResult = houseController.Get(houseId: hotelId, propertyType: propertyType);
                dynamic retrievedHotel = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)retrievedHostelHttpResult).Content;
                Assert.IsNotNull(retrievedHotel);

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

                Assert.AreEqual(numberOfSingleBeds, retrievedHotel.NumberOfSingleBeds);
                Assert.AreEqual(numberOfDoubleBeds, retrievedHotel.NumberOfDoubleBeds);
                
                Assert.AreEqual(numberOfAdults, retrievedHotel.Occupants.Adults);
                Assert.AreEqual(numberOfChildren, retrievedHotel.Occupants.Children);
                Assert.AreEqual(numberOfAdults + numberOfChildren, retrievedHotel.Occupants.TotalOccupants);

                Assert.AreEqual(landlineNumber, retrievedHotel.LandlineNumber);
                Assert.AreEqual(fax, retrievedHotel.Fax);
                Assert.AreEqual(elevator, retrievedHotel.Elevator);
            }
        }

        // Save and Get Guest House by Id
        [Test]
        public void SaveNewGuestHouseTest_ChecksThatANewGuestHouseIsSavedAndRetrievedAsExpected_VerifiesByTheReturnValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);
                string title = "Title No 1";
                string description = "Description of house";
                string email = "w@12344321.com";
                string name = "OwnerName";
                string phoneNumber = "03455138018";
                string propertyType = Constants.GuestHouse;
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
                int numberOfSingleBeds = 3;
                int numberOfDoubleBeds = 2;

                string landlineNumber = "0510000000";
                string fax = "0510000000";
                bool elevator = false;
                
                var createNewHotelCommand = new CreateHotelCommand(title, monthlyRent, internet,
                    cableTv, parking, propertyType, email, phoneNumber, area, name, description,
                    genderRestriction.ToString(),
                    isShared, rentUnit, laundry, ac, geyser, fitnessCentre, attachedBathroom, ironing, balcony,
                    lawn, cctvCameras, backupElectricity, heating, landlineNumber, fax, elevator, restaurant,
                    airportShuttle, breakfastIncluded, sittingArea, carRental, spa, salon, bathtub, swimmingPool,
                    kitchen, numberOfSingleBeds, numberOfDoubleBeds, new Occupants(numberOfAdults, numberOfChildren));

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, email)
                    })
                });

                var postHttpActionResult = houseController.Post(JsonConvert.SerializeObject(createNewHotelCommand));
                string hotelId = ((OkNegotiatedContentResult<string>)postHttpActionResult).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hotelId));

                var retrievedHostelHttpResult = houseController.Get(houseId: hotelId, propertyType: propertyType);
                dynamic retrievedHotel = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)retrievedHostelHttpResult).Content;
                Assert.IsNotNull(retrievedHotel);

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

                Assert.AreEqual(numberOfSingleBeds, retrievedHotel.NumberOfSingleBeds);
                Assert.AreEqual(numberOfDoubleBeds, retrievedHotel.NumberOfDoubleBeds);

                Assert.AreEqual(numberOfAdults, retrievedHotel.Occupants.Adults);
                Assert.AreEqual(numberOfChildren, retrievedHotel.Occupants.Children);
                Assert.AreEqual(numberOfAdults + numberOfChildren, retrievedHotel.Occupants.TotalOccupants);

                Assert.AreEqual(landlineNumber, retrievedHotel.LandlineNumber);
                Assert.AreEqual(fax, retrievedHotel.Fax);
                Assert.AreEqual(elevator, retrievedHotel.Elevator);
            }
        }

        #endregion Save Tests

        #region Update Tests

        // Update House
        [Category("Integration")]
        [Test]
        public void UpdateAndGetHouseInstanceByIdTest_TestsThatHouseIsSavedAndRetreivedAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);

                string title = "House For Rent";
                string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
                int rent = 105000;
                string ownerEmail = "thorin@oakenshield123.com";
                string ownerPhoneNumber = "01234567890";
                string houseNo = "CT-141/A";
                string streetNo = "14";
                int numberOfBathrooms = 1;
                int numberOfBedrooms = 1;
                int numberOfKitchens = 1;
                bool familiesOnly = false;
                bool boysOnly = false;
                bool girlsOnly = true;
                bool internetAvailable = true;
                bool landlinePhoneAvailable = true;
                bool cableTvAvailable = true;
                bool garageAvailable = true;
                bool smokingAllowed = true;
                string propertyType = Constants.Apartment;
                string area = "Pindora, Rawalpindi, Pakistan";
                //string area = "Pindora, Rawalpindi, Pakistan";
                string dimensionType = DimensionType.Kanal.ToString();
                string dimensionString = "5";
                decimal dimensionDecimal = 0;
                string ownerName = "Owner Name 1";
                string genderRestriction = GenderRestriction.FamiliesOnly.ToString();
                bool isShared = true;
                string rentUnit = House.GetAllRentUnits()[0];
                string landlineNumber = "0510000000";
                string fax = "0510000000";

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail)
                    })
                });
                CreateHouseCommand house = new CreateHouseCommand(title, rent, numberOfBedrooms, numberOfKitchens,
                    numberOfBathrooms, internetAvailable, landlinePhoneAvailable,
                    cableTvAvailable, garageAvailable, smokingAllowed, propertyType, ownerEmail, ownerPhoneNumber,
                    houseNo, streetNo, area, dimensionType, dimensionString, dimensionDecimal, ownerName, description, genderRestriction,
                    isShared, rentUnit, landlineNumber, fax);
                IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(house));
                string houseId = ((OkNegotiatedContentResult<string>)houseSaveResult).Content;

                IHttpActionResult response = (IHttpActionResult)houseController.Get(houseId: houseId, propertyType: propertyType);
                dynamic retreivedHouse = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)response).Content;
                Assert.NotNull(retreivedHouse);

                // Update variables
                string updatedTitle = "Updated House For Rent";
                string updatedDescription = "updated Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
                int updatedRent = 195000;
                string updatedOwnerEmail = "thorin@oakenshield123.com";
                string updatedOwnerPhoneNumber = "01234567890";
                string updatedHouseNo = "CT-141/A";
                string updatedStreetNo = "14";
                int updatedNumberOfBathrooms = 9;
                int updatedNumberOfBedrooms = 8;
                int updatedNumberOfKitchens = 7;
                bool updatedInternetAvailable = true;
                bool updatedLandlinePhoneAvailable = true;
                bool updatedCableTvAvailable = true;
                bool updatedGarageAvailable = true;
                bool updatedSmokingAllowed = true;
                string updatedPropertyType = Constants.House;
                string updatedArea = "Saddar, Rawalpindi, Pakistan";
                //string area = "Pindora, Rawalpindi, Pakistan";
                string updatedDimensionType = DimensionType.Kanal.ToString();
                string updatedDimensionString = "5";
                decimal updatedDimensionDecimal = 0;
                string updatedOwnerName = "Owner Name 1";
                string updatedGenderRestriction = GenderRestriction.BoysOnly.ToString();
                bool updatedIsShared = true;
                string updatedRentUnit = House.GetAllRentUnits()[1];
                string updatedLandlineNumber = "0510000001";
                string updatedFax = "0510000001";

                UpdateHouseCommand updateHouseCommand = new UpdateHouseCommand(houseId, updatedTitle, updatedRent, updatedNumberOfBedrooms,
                    updatedNumberOfKitchens, updatedNumberOfBathrooms, updatedInternetAvailable, updatedLandlinePhoneAvailable,
                    updatedCableTvAvailable, updatedGarageAvailable, updatedSmokingAllowed, updatedPropertyType, updatedOwnerEmail,
                    updatedOwnerPhoneNumber, updatedHouseNo, updatedStreetNo, updatedArea, updatedDimensionType, updatedDimensionString,
                    updatedDimensionDecimal, updatedOwnerName, updatedDescription, updatedGenderRestriction, updatedIsShared,
                    updatedRentUnit, updatedLandlineNumber, updatedFax);

                houseController.Put(JsonConvert.SerializeObject(updateHouseCommand));
                response = (IHttpActionResult)houseController.Get(houseId: houseId, propertyType: updatedPropertyType);
                dynamic retreivedUpdatedHouse = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)response).Content;
                Assert.NotNull(retreivedUpdatedHouse);
                Assert.AreEqual(houseId, retreivedUpdatedHouse.Id);
                Assert.AreEqual(updatedTitle, retreivedUpdatedHouse.Title);
                Assert.AreEqual(updatedDescription, retreivedUpdatedHouse.Description);
                Assert.AreEqual(updatedOwnerPhoneNumber, retreivedUpdatedHouse.OwnerPhoneNumber);
                Assert.AreEqual(updatedOwnerEmail, retreivedUpdatedHouse.OwnerEmail);
                Assert.AreEqual(updatedRent, retreivedUpdatedHouse.RentPrice);
                Assert.AreEqual(updatedNumberOfBathrooms, retreivedUpdatedHouse.NumberOfBathrooms);
                Assert.AreEqual(updatedNumberOfBedrooms, retreivedUpdatedHouse.NumberOfBedrooms);
                Assert.AreEqual(updatedNumberOfKitchens, retreivedUpdatedHouse.NumberOfKitchens);
                Assert.AreEqual(updatedGarageAvailable, retreivedUpdatedHouse.GarageAvailable);
                Assert.AreEqual(updatedLandlinePhoneAvailable, retreivedUpdatedHouse.LandlinePhoneAvailable);
                Assert.AreEqual(updatedCableTvAvailable, retreivedUpdatedHouse.CableTvAvailable);
                Assert.AreEqual(updatedInternetAvailable, retreivedUpdatedHouse.InternetAvailable);
                Assert.AreEqual(updatedSmokingAllowed, retreivedUpdatedHouse.SmokingAllowed);
                Assert.AreEqual(updatedPropertyType, retreivedUpdatedHouse.PropertyType);
                Assert.AreEqual(updatedHouseNo, retreivedUpdatedHouse.HouseNo);
                Assert.AreEqual(updatedIsShared, retreivedUpdatedHouse.IsShared);
                Assert.AreEqual(updatedArea, retreivedUpdatedHouse.Area);
                Assert.AreEqual(updatedStreetNo, retreivedUpdatedHouse.StreetNo);
                Assert.AreEqual(updatedDimensionString + " " + updatedDimensionType, retreivedUpdatedHouse.Dimension);
                Assert.AreEqual(updatedOwnerName, retreivedUpdatedHouse.OwnerName);
                Assert.AreEqual(updatedGenderRestriction, retreivedUpdatedHouse.GenderRestriction);
                Assert.AreEqual(updatedRentUnit, retreivedUpdatedHouse.RentUnit);
                Assert.AreEqual(updatedLandlineNumber, retreivedUpdatedHouse.LandlineNumber);
                Assert.AreEqual(updatedFax, retreivedUpdatedHouse.Fax);
            }
        }

        // Update Apartment
        [Category("Integration")]
        [Test]
        public void UpdateAndGetApartmentInstanceByIdTest_TestsThatApartmentIsSavedAndRetreivedAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);

                string title = "House For Rent";
                string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
                int rent = 105000;
                string ownerEmail = "thorin@oakenshield123.com";
                string ownerPhoneNumber = "01234567890";
                string houseNo = "CT-141/A";
                string streetNo = "14";
                int numberOfBathrooms = 1;
                int numberOfBedrooms = 1;
                int numberOfKitchens = 1;
                bool familiesOnly = false;
                bool boysOnly = false;
                bool girlsOnly = true;
                bool internetAvailable = true;
                bool landlinePhoneAvailable = true;
                bool cableTvAvailable = true;
                bool garageAvailable = true;
                bool smokingAllowed = true;
                string propertyType = Constants.Apartment;
                string area = "Pindora, Rawalpindi, Pakistan";
                //string area = "Pindora, Rawalpindi, Pakistan";
                string dimensionType = DimensionType.Kanal.ToString();
                string dimensionString = "5";
                decimal dimensionDecimal = 0;
                string ownerName = "Owner Name 1";
                string genderRestriction = GenderRestriction.FamiliesOnly.ToString();
                bool isShared = true;
                string rentUnit = House.GetAllRentUnits()[0];
                string landlineNumber = "0510000000";
                string fax = "0510000000";

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail)
                    })
                });
                CreateHouseCommand house = new CreateHouseCommand(title, rent, numberOfBedrooms, numberOfKitchens,
                    numberOfBathrooms, internetAvailable, landlinePhoneAvailable,
                    cableTvAvailable, garageAvailable, smokingAllowed, propertyType, ownerEmail, ownerPhoneNumber,
                    houseNo, streetNo, area, dimensionType, dimensionString, dimensionDecimal, ownerName, description, genderRestriction,
                    isShared, rentUnit, landlineNumber, fax);
                IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(house));
                string houseId = ((OkNegotiatedContentResult<string>)houseSaveResult).Content;

                IHttpActionResult response = (IHttpActionResult)houseController.Get(houseId: houseId, propertyType: propertyType);
                dynamic retreivedHouse = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)response).Content;
                Assert.NotNull(retreivedHouse);

                // Update variables
                string updatedTitle = "Updated House For Rent";
                string updatedDescription = "updated Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
                int updatedRent = 195000;
                string updatedOwnerEmail = "thorin@oakenshield123.com";
                string updatedOwnerPhoneNumber = "01234567890";
                string updatedHouseNo = "CT-141/A";
                string updatedStreetNo = "14";
                int updatedNumberOfBathrooms = 9;
                int updatedNumberOfBedrooms = 8;
                int updatedNumberOfKitchens = 7;
                bool updatedInternetAvailable = true;
                bool updatedLandlinePhoneAvailable = true;
                bool updatedCableTvAvailable = true;
                bool updatedGarageAvailable = true;
                bool updatedSmokingAllowed = true;
                string updatedPropertyType = Constants.House;
                string updatedArea = "Saddar, Rawalpindi, Pakistan";
                //string area = "Pindora, Rawalpindi, Pakistan";
                string updatedDimensionType = DimensionType.Kanal.ToString();
                string updatedDimensionString = "5";
                decimal updatedDimensionDecimal = 0;
                string updatedOwnerName = "Owner Name 1";
                string updatedGenderRestriction = GenderRestriction.BoysOnly.ToString();
                bool updatedIsShared = true;
                string updatedRentUnit = House.GetAllRentUnits()[1];
                string updatedLandlineNumber = "0510000001";
                string updatedFax = "0510000001";

                UpdateHouseCommand updateHouseCommand = new UpdateHouseCommand(houseId, updatedTitle, updatedRent, updatedNumberOfBedrooms,
                    updatedNumberOfKitchens, updatedNumberOfBathrooms, updatedInternetAvailable, updatedLandlinePhoneAvailable,
                    updatedCableTvAvailable, updatedGarageAvailable, updatedSmokingAllowed, updatedPropertyType, updatedOwnerEmail,
                    updatedOwnerPhoneNumber, updatedHouseNo, updatedStreetNo, updatedArea, updatedDimensionType, updatedDimensionString,
                    updatedDimensionDecimal, updatedOwnerName, updatedDescription, updatedGenderRestriction, updatedIsShared,
                    updatedRentUnit, updatedLandlineNumber, updatedFax);

                houseController.Put(JsonConvert.SerializeObject(updateHouseCommand));
                response = (IHttpActionResult)houseController.Get(houseId: houseId, propertyType: updatedPropertyType);
                dynamic retreivedUpdatedHouse = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)response).Content;
                Assert.NotNull(retreivedUpdatedHouse);
                Assert.AreEqual(houseId, retreivedUpdatedHouse.Id);
                Assert.AreEqual(updatedTitle, retreivedUpdatedHouse.Title);
                Assert.AreEqual(updatedDescription, retreivedUpdatedHouse.Description);
                Assert.AreEqual(updatedOwnerPhoneNumber, retreivedUpdatedHouse.OwnerPhoneNumber);
                Assert.AreEqual(updatedOwnerEmail, retreivedUpdatedHouse.OwnerEmail);
                Assert.AreEqual(updatedRent, retreivedUpdatedHouse.RentPrice);
                Assert.AreEqual(updatedNumberOfBathrooms, retreivedUpdatedHouse.NumberOfBathrooms);
                Assert.AreEqual(updatedNumberOfBedrooms, retreivedUpdatedHouse.NumberOfBedrooms);
                Assert.AreEqual(updatedNumberOfKitchens, retreivedUpdatedHouse.NumberOfKitchens);
                Assert.AreEqual(updatedGarageAvailable, retreivedUpdatedHouse.GarageAvailable);
                Assert.AreEqual(updatedLandlinePhoneAvailable, retreivedUpdatedHouse.LandlinePhoneAvailable);
                Assert.AreEqual(updatedCableTvAvailable, retreivedUpdatedHouse.CableTvAvailable);
                Assert.AreEqual(updatedInternetAvailable, retreivedUpdatedHouse.InternetAvailable);
                Assert.AreEqual(updatedSmokingAllowed, retreivedUpdatedHouse.SmokingAllowed);
                Assert.AreEqual(updatedPropertyType, retreivedUpdatedHouse.PropertyType);
                Assert.AreEqual(updatedHouseNo, retreivedUpdatedHouse.HouseNo);
                Assert.AreEqual(updatedIsShared, retreivedUpdatedHouse.IsShared);
                Assert.AreEqual(updatedArea, retreivedUpdatedHouse.Area);
                Assert.AreEqual(updatedStreetNo, retreivedUpdatedHouse.StreetNo);
                Assert.AreEqual(updatedDimensionString + " " + updatedDimensionType, retreivedUpdatedHouse.Dimension);
                Assert.AreEqual(updatedOwnerName, retreivedUpdatedHouse.OwnerName);
                Assert.AreEqual(updatedGenderRestriction, retreivedUpdatedHouse.GenderRestriction);
                Assert.AreEqual(updatedRentUnit, retreivedUpdatedHouse.RentUnit);
                Assert.AreEqual(updatedLandlineNumber, retreivedUpdatedHouse.LandlineNumber);
                Assert.AreEqual(updatedFax, retreivedUpdatedHouse.Fax);
            }
        }

        // Update a Hostel
        [Category("Integration")]
        [Test]
        public void UpdateHostelTest_TestsThatHouseIsSavedThenUpdatedAndRetreivedAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);

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

                var createNewHostelCommand = new CreateHostelCommand(title, monthlyRent, internet,
                    cableTv, parking, propertyType, email, phoneNumber, area, name, description,
                    genderRestriction.ToString(),
                    isShared, rentUnit, laundry, ac, geyser, fitnessCentre, attachedBathroom, ironing, balcony, lawn,
                    cctvCameras, backupElectricity, heating, landlineNumber, fax, elevator, meals, picknDrop,
                    numberOfSeats);

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, email)
                    })
                });

                var postHttpActionResult = houseController.Post(JsonConvert.SerializeObject(createNewHostelCommand));
                string hostelId = ((OkNegotiatedContentResult<string>)postHttpActionResult).Content;
                
                var retrievedHostelHttpResult = houseController.Get(houseId: hostelId, propertyType: propertyType);
                dynamic retrievedHostel = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)retrievedHostelHttpResult).Content;
                Assert.IsNotNull(retrievedHostel);

                // Update variables
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

                houseController.Put(JsonConvert.SerializeObject(updateHostelCommand));
                var response = (IHttpActionResult)houseController.Get(houseId: hostelId, propertyType: updatedPropertyType);
                dynamic updatedRetrievedHostel = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)response).Content;
                Assert.NotNull(updatedRetrievedHostel);

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
        }

        // Update a hotel
        [Test]
        public void UpdateHotelTest_ChecksThatAHotelIsUpdatedAsExpected_VerifiesByTheReturnValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);
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
                int numberOfSingleBeds = 0;
                int numberOfDoubleBeds = 2;

                string landlineNumber = "0510000000";
                string fax = "0510000000";
                bool elevator = false;
                
                var createNewHotelCommand = new CreateHotelCommand(title, monthlyRent, internet,
                    cableTv, parking, propertyType, email, phoneNumber, area, name, description,
                    genderRestriction.ToString(),
                    isShared, rentUnit, laundry, ac, geyser, fitnessCentre, attachedBathroom, ironing, balcony,
                    lawn, cctvCameras, backupElectricity, heating, landlineNumber, fax, elevator, restaurant,
                    airportShuttle, breakfastIncluded, sittingArea, carRental, spa, salon, bathtub, swimmingPool,
                    kitchen, numberOfSingleBeds, numberOfDoubleBeds, new Occupants(numberOfAdults, numberOfChildren));

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, email)
                    })
                });

                var postHttpActionResult = houseController.Post(JsonConvert.SerializeObject(createNewHotelCommand));
                string hotelId = ((OkNegotiatedContentResult<string>)postHttpActionResult).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hotelId));

                var retrievedHostelHttpResult = houseController.Get(houseId: hotelId, propertyType: propertyType);
                dynamic retrievedHotel = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)retrievedHostelHttpResult).Content;
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
                int updatedNumberOfSingleBeds = 2;
                int updatedNumberOfDoubleBeds = 0;

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
                    updatedSalon, updatedBathtub, updatedSwimmingPool, updatedKitchen, updatedNumberOfSingleBeds,
                    updatedNumberOfDoubleBeds, new Occupants(updatedNumberOfAdults, updatedNumberOfChildren));

                houseController.Put(JsonConvert.SerializeObject(updateHotelCommand));
                var response = (IHttpActionResult)houseController.Get(houseId: hotelId, propertyType: updatedPropertyType);
                dynamic retrievedUpdatedHotel = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)response).Content;
                Assert.NotNull(retrievedUpdatedHotel);

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

                Assert.AreEqual(updatedNumberOfSingleBeds, retrievedUpdatedHotel.NumberOfSingleBeds);
                Assert.AreEqual(updatedNumberOfDoubleBeds, retrievedUpdatedHotel.NumberOfDoubleBeds);

                Assert.AreEqual(updatedNumberOfAdults, retrievedUpdatedHotel.Occupants.Adults);
                Assert.AreEqual(updatedNumberOfChildren, retrievedUpdatedHotel.Occupants.Children);
                Assert.AreEqual(updatedNumberOfAdults + updatedNumberOfChildren, retrievedUpdatedHotel.Occupants.TotalOccupants);

                Assert.AreEqual(updatedLandlineNumber, retrievedUpdatedHotel.LandlineNumber);
                Assert.AreEqual(updatedFax, retrievedUpdatedHotel.Fax);
                Assert.AreEqual(updatedElevator, retrievedUpdatedHotel.Elevator);
            }
        }

        // Update a Guest House
        [Test]
        public void UpdateGuestHouseTest_ChecksThatAGuestHouseIsUpdatedAsExpected_VerifiesByTheReturnValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);
                string title = "Title No 1";
                string description = "Description of house";
                string email = "w@12344321.com";
                string name = "OwnerName";
                string phoneNumber = "03455138018";
                string propertyType = Constants.GuestHouse;
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
                int numberOfSingleBeds = 0;
                int numberOfDoubleBeds = 0;

                string landlineNumber = "0510000000";
                string fax = "0510000000";
                bool elevator = false;
                
                var createNewHotelCommand = new CreateHotelCommand(title, monthlyRent, internet,
                    cableTv, parking, propertyType, email, phoneNumber, area, name, description,
                    genderRestriction.ToString(),
                    isShared, rentUnit, laundry, ac, geyser, fitnessCentre, attachedBathroom, ironing, balcony,
                    lawn, cctvCameras, backupElectricity, heating, landlineNumber, fax, elevator, restaurant,
                    airportShuttle, breakfastIncluded, sittingArea, carRental, spa, salon, bathtub, swimmingPool,
                    kitchen, numberOfSingleBeds, numberOfDoubleBeds, new Occupants(numberOfAdults, numberOfChildren));

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, email)
                    })
                });

                var postHttpActionResult = houseController.Post(JsonConvert.SerializeObject(createNewHotelCommand));
                string hotelId = ((OkNegotiatedContentResult<string>)postHttpActionResult).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hotelId));

                var retrievedHostelHttpResult = houseController.Get(houseId: hotelId, propertyType: propertyType);
                dynamic retrievedHotel = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)retrievedHostelHttpResult).Content;
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

                int updatedNumberOfSingleBeds = 1;
                int updatedNumberOfDoubleBeds = 0;

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
                    updatedSalon, updatedBathtub, updatedSwimmingPool, updatedKitchen, updatedNumberOfSingleBeds,
                    updatedNumberOfDoubleBeds, new Occupants(updatedNumberOfAdults, updatedNumberOfChildren));

                houseController.Put(JsonConvert.SerializeObject(updateHotelCommand));
                var response = (IHttpActionResult)houseController.Get(houseId: hotelId, propertyType: updatedPropertyType);
                dynamic retrievedUpdatedHotel = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)response).Content;
                Assert.NotNull(retrievedUpdatedHotel);

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

                Assert.AreEqual(updatedNumberOfSingleBeds, retrievedUpdatedHotel.NumberOfSingleBeds);
                Assert.AreEqual(updatedNumberOfDoubleBeds, retrievedUpdatedHotel.NumberOfDoubleBeds);
                Assert.AreEqual(updatedNumberOfAdults, retrievedUpdatedHotel.Occupants.Adults);
                Assert.AreEqual(updatedNumberOfChildren, retrievedUpdatedHotel.Occupants.Children);
                Assert.AreEqual(updatedNumberOfAdults + updatedNumberOfChildren, retrievedUpdatedHotel.Occupants.TotalOccupants);

                Assert.AreEqual(updatedLandlineNumber, retrievedUpdatedHotel.LandlineNumber);
                Assert.AreEqual(updatedFax, retrievedUpdatedHotel.Fax);
                Assert.AreEqual(updatedElevator, retrievedUpdatedHotel.Elevator);
            }
        }

        #endregion Update Tests

        // KNOWLEDGE POINT
        // # HttpResponseMessage's status and response can be checked like this:
        //Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        //Assert.That(response.Content, Is.EqualTo("No Permission"));
        // # For getting value from HttpResponseMessage, there are 3 ways to extract out objects.
        // # 1 is:
        //House retreivedHouse;
        //response.TryGetContentValue<House>(out retreivedHouse);
        // # 2 is:
        //ObjectContent objContent = response.Content as ObjectContent;
        //House retreivedHouse2 = objContent.Value as House;
        // # 3: But if it's a StreamContent, then we do it like this using async read
        //var retreivedHouse3 = response.Content.ReadAsAsync<House>().Result;

        #region Search Tests

        [Test]
        public void SearchByLocationAndPropertyTypeTest_TestsThatHousesAreSavedAndRetreivedAsExpected_VerfiesThroughInstanceValue()
        {
            // Save 2 properties for each property type, at distant locations from each other. 
            // Then search for one of those locations for each property type and verify that the search result
            // contains only one of the properties that are located within the searched location
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);

                // Saving Property # 1 - House
                string ownerEmail = "house@1234567-1.com";
                string area = "Nowshera";
                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail)
                    })
                });
                CreateHouseCommand house = new CreateHouseCommand("title", 22000000, 1, 1, 1, true, false, false, true,
                    false, Constants.House, ownerEmail, "03000000000", "1", "1", area, "Kanal", "1", 0, "Some name",
                    "", GenderRestriction.NoRestriction.ToString(), false, "Day", "", "");
                IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(house));
                string houseId = ((OkNegotiatedContentResult<string>) houseSaveResult).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(houseId));

                // Saving Property # 2 - House
                string ownerEmail2 = "house@1234567-2.com";
                string area2 = "Saddar, Rawalpindi";
                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail2)
                    })
                });
                CreateHouseCommand house2 = new CreateHouseCommand("Title # 2", 1400000, 1, 1,
                    1, true, false, false, false, true, Constants.House, ownerEmail2, "03000000000",
                    "", "", area2, "Kanal", "2", 0, "Name", "", GenderRestriction.GirlsOnly.ToString(), true, Constants.Daily, "", "");
                IHttpActionResult houseSaveResult2 = houseController.Post(JsonConvert.SerializeObject(house2));
                string houseId2 = ((OkNegotiatedContentResult<string>)houseSaveResult2).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(houseId2));

                // Saving Property # 3 - Apartment
                string ownerEmail3 = "house@1234567-3.com";
                string area3 = "Saddar, Rawalpindi, Pakistan";
                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail3)
                    })
                });
                CreateHouseCommand apartment3 = new CreateHouseCommand("Title # 3", 10000003, 1, 1,
                    1, false, true,
                    true, false, false, Constants.Apartment, ownerEmail3, "03000000000",
                    "", "", area3, DimensionType.Acre.ToString(), "2", 0, "Name 3", "", 
                    GenderRestriction.NoRestriction.ToString(), false, Constants.Daily, "", "");
                IHttpActionResult houseSaveResult3 = houseController.Post(JsonConvert.SerializeObject(apartment3));
                Assert.IsFalse(string.IsNullOrWhiteSpace(((OkNegotiatedContentResult<string>)houseSaveResult3).Content));
                string apartmentId3 = ((OkNegotiatedContentResult<string>)houseSaveResult3).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(apartmentId3));

                // Saving Property # 4 - Hotel
                string ownerEmail4 = "house@1234567-4.com";
                string area4 = "Risalpur";
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail4)
                    })
                });
                CreateHotelCommand hotel4 = new CreateHotelCommand("title 4", 1000, false, false, false, 
                    Constants.Hotel, ownerEmail4, "03000000000", area4, "someone", "", GenderRestriction.BoysOnly.ToString(),
                    false, Constants.Daily, true,true,true,true, true, true, false, false, false, false, false,
                    "", "", true, true, true, true, true, true, true, true, true, true, true, 0, 0, new Occupants(2, 0));
                IHttpActionResult houseSaveResult4 = houseController.Post(JsonConvert.SerializeObject(hotel4));
                string hotelId4 = ((OkNegotiatedContentResult<string>)houseSaveResult4).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hotelId4));

                // Saving Property # 5 - Hotel
                string ownerEmail5 = "house@1234567-5.com";
                string area5 = "Satellite Town, Rawalpindi";
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail5)
                    })
                });
                CreateHotelCommand hotel5 = new CreateHotelCommand("title 4", 1000, false, false, false,
                    Constants.Hotel, ownerEmail5, "03234567894", area5, "someone", "", GenderRestriction.BoysOnly.ToString(),
                    false, Constants.Daily, true, true, true, true, true, true, false, false, false, false, false,
                    "", "", true, true, true, true, true, true, true, true, true, true, true, 0, 0, new Occupants(2, 0));
                IHttpActionResult houseSaveResult5 = houseController.Post(JsonConvert.SerializeObject(hotel5));
                string hotelId5 = ((OkNegotiatedContentResult<string>)houseSaveResult5).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hotelId5));

                // Saving Property # 6 - Hostel
                string ownerEmail6 = "house@1234567-6.com";
                string area6 = "Nowshera";
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail6)
                    })
                });
                CreateHostelCommand hostel6 = new CreateHostelCommand("Title 6", 1000, false,
                    true, true, Constants.Hostel, ownerEmail6, "03990000000", area6, "SOmeone", "", GenderRestriction.BoysOnly.ToString(),
                    false, Constants.Daily, true, true, true, true, true, true, true, true, false, true, true,
                    "", "", false, false, false, 1);
                IHttpActionResult houseSaveResult6 = houseController.Post(JsonConvert.SerializeObject(hostel6));
                string hostelId6 = ((OkNegotiatedContentResult<string>)houseSaveResult6).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hostelId6));

                // Saving Property # 7 Hostel
                string ownerEmail7 = "house@1234567-7.com";
                string area7 = "Pindora, Rawalpindi";
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail7)
                    })
                });
                CreateHostelCommand hostel7 = new CreateHostelCommand("Title 7", 1000, false,
                    true, true, Constants.Hostel, ownerEmail7, "03990000000", area7, "SOmeone", "", GenderRestriction.BoysOnly.ToString(),
                    false, Constants.Daily, true, true, true, true, true, true, true, true, false, true, true,
                    "", "", false, false, false, 1);
                IHttpActionResult houseSaveResult7 = houseController.Post(JsonConvert.SerializeObject(hostel7));
                string hostelId7 = ((OkNegotiatedContentResult<string>)houseSaveResult7).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hostelId7));
                
                // Saving Property # 8 - Guest House
                string ownerEmail8 = "house@1234567-8.com";
                string ownerPhoneNumber8 = "01238567893";
                string area8 = "Risalpur";
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail8)
                    })
                });
                CreateHotelCommand guestHouse8 = new CreateHotelCommand("title 8", 1000, false, false, false,
                    Constants.GuestHouse, ownerEmail8, ownerPhoneNumber8, area8, "someone", "", GenderRestriction.BoysOnly.ToString(),
                    false, Constants.Daily, true, true, true, true, true, true, false, false, false, false, false,
                    "", "", true, true, true, true, true, true, true, true, true, true, true, 0, 0, new Occupants(2, 0));
                IHttpActionResult houseSaveResult8 = houseController.Post(JsonConvert.SerializeObject(guestHouse8));
                string guestHouseId8 = ((OkNegotiatedContentResult<string>)houseSaveResult8).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(guestHouseId8));

                // Saving Property # 9 - Guest House
                string ownerEmail9 = "house@1234967-9.com";
                string area9 = "Satellite Town, Rawalpindi";
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail9)
                    })
                });
                CreateHotelCommand guestHouse9 = new CreateHotelCommand("title 4", 1000, false, false, false,
                    Constants.GuestHouse, ownerEmail9, "03234967894", area9, "someone", "", GenderRestriction.BoysOnly.ToString(),
                    false, Constants.Daily, true, true, true, true, true, true, false, false, false, false, false,
                    "", "", true, true, true, true, true, true, true, true, true, true, true, 0, 0, new Occupants(2, 0));
                IHttpActionResult houseSaveResult9 = houseController.Post(JsonConvert.SerializeObject(guestHouse9));
                string guestHouseId9 = ((OkNegotiatedContentResult<string>)houseSaveResult9).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(guestHouseId9));

                // Saving Property # 10 - Apartment
                string ownerEmail10 = "house@12104567-10.com";
                string area10 = "Nowshera";
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail10)
                    })
                });
                CreateHouseCommand apartment10 = new CreateHouseCommand("Title 10", 10010, 1, 1,
                    1, false, true,
                    true, false, false, Constants.Apartment, ownerEmail10, "03000000000",
                    "", "", area10, DimensionType.Acre.ToString(), "2", 0, "Owner name", "",
                    GenderRestriction.GirlsOnly.ToString(), false, Constants.Daily, "", "");
                IHttpActionResult houseSaveResult10 = houseController.Post(JsonConvert.SerializeObject(apartment10));
                Assert.IsFalse(string.IsNullOrWhiteSpace(((OkNegotiatedContentResult<string>)houseSaveResult10).Content));
                string apartmentId10 = ((OkNegotiatedContentResult<string>)houseSaveResult10).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(apartmentId10));

                var searchLocation1 = "Daman-e-Koh, Daman -e- Koh Road, Islamabad";
                var searchLocation2 = "Mardan";

                // ###### When House is searched ######
                IHttpActionResult response = (IHttpActionResult)houseController.Get(
                    area: searchLocation1, propertyType: Constants.House);
                dynamic retreivedHouses = ((OkNegotiatedContentResult
                    <IList<ResidentialPropertyPartialBaseImplementation>>)response).Content;
                Assert.NotNull(retreivedHouses);
                Assert.AreEqual(1, retreivedHouses.Count);
                Assert.AreEqual(houseId2, retreivedHouses[0].Id);
                Assert.AreEqual(house2.Title, retreivedHouses[0].Title);
                Assert.AreEqual(Constants.House, retreivedHouses[0].PropertyType);

                // ###### When Apartment is searched ######
                IHttpActionResult apartmentResponse = (IHttpActionResult)houseController.Get(
                    area: searchLocation2, propertyType: Constants.Apartment);
                dynamic retreivedApartments = ((OkNegotiatedContentResult
                    <IList<ResidentialPropertyPartialBaseImplementation>>)apartmentResponse).Content;
                Assert.NotNull(retreivedApartments);
                Assert.AreEqual(1, retreivedApartments.Count);
                Assert.AreEqual(apartmentId10, retreivedApartments[0].Id);
                Assert.AreEqual(apartment10.Title, retreivedApartments[0].Title);
                Assert.AreEqual(Constants.Apartment, retreivedApartments[0].PropertyType);

                // ###### When Hotel is searched ######
                IHttpActionResult hotelResponse = (IHttpActionResult)houseController.Get(
                    area: searchLocation1, propertyType: Constants.Hotel);
                dynamic retreivedHotels = ((OkNegotiatedContentResult
                    <IList<ResidentialPropertyPartialBaseImplementation>>)hotelResponse).Content;
                Assert.NotNull(retreivedHotels);
                Assert.AreEqual(1, retreivedHotels.Count);
                Assert.AreEqual(hotelId5, retreivedHotels[0].Id);
                Assert.AreEqual(hotel5.Title, retreivedHotels[0].Title);
                Assert.AreEqual(Constants.Hotel, retreivedHotels[0].PropertyType);

                // ###### When Hostel is searched ######
                IHttpActionResult hostelResponse = (IHttpActionResult)houseController.Get(
                    area: searchLocation1, propertyType: Constants.Hostel);
                dynamic retreivedHostels = ((OkNegotiatedContentResult
                    <IList<ResidentialPropertyPartialBaseImplementation>>)hostelResponse).Content;
                Assert.NotNull(retreivedHostels);
                Assert.AreEqual(1, retreivedHostels.Count);
                Assert.AreEqual(hostelId7, retreivedHostels[0].Id);
                Assert.AreEqual(hostel7.Title, retreivedHostels[0].Title);
                Assert.AreEqual(Constants.Hostel, retreivedHostels[0].PropertyType);

                // ###### When Guest House is searched ######
                IHttpActionResult guestHouseResponse = (IHttpActionResult)houseController.Get(
                    area: searchLocation2, propertyType: Constants.GuestHouse);
                dynamic retreivedGuestHouses = ((OkNegotiatedContentResult
                    <IList<ResidentialPropertyPartialBaseImplementation>>)guestHouseResponse).Content;
                Assert.NotNull(retreivedGuestHouses);
                Assert.AreEqual(1, retreivedGuestHouses.Count);
                Assert.AreEqual(guestHouseId8, retreivedGuestHouses[0].Id);
                Assert.AreEqual(guestHouse8.Title, retreivedGuestHouses[0].Title);
                Assert.AreEqual(Constants.GuestHouse, retreivedGuestHouses[0].PropertyType);
            }
        }

        [Category("Integration")]
        [Test]
        public void SearchByPropertyTypeOnlyTest_ChecksThatTheRequestedPropertiesForThatPropertyTypeAreSearchedAndSavedAsExpcted_VerifiesThroughDatabaseRetrieval()
        {
            // Save 2 properties for each property type, at distant locations from each other. 
            // Then search for each property type and verify that the search result
            // contains all of the properties for the searched property type
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);

                // Saving Property # 1 - House
                string ownerEmail = "house@1234567-1.com";
                string area = "Nowshera";
                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail)
                    })
                });
                CreateHouseCommand house = new CreateHouseCommand("title", 22000000, 1, 1, 1, true, false, false, true,
                    false, Constants.House, ownerEmail, "03000000000", "1", "1", area, "Kanal", "1", 0, "Some name",
                    "", GenderRestriction.NoRestriction.ToString(), false, "Day", "", "");
                IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(house));
                string houseId = ((OkNegotiatedContentResult<string>)houseSaveResult).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(houseId));

                // Saving Property # 2 - House
                string ownerEmail2 = "house@1234567-2.com";
                string area2 = "Saddar, Rawalpindi";
                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail2)
                    })
                });
                CreateHouseCommand house2 = new CreateHouseCommand("Title # 2", 1400000, 1, 1,
                    1, true, false, false, false, true, Constants.House, ownerEmail2, "03000000000",
                    "", "", area2, "Kanal", "2", 0, "Name", "", GenderRestriction.GirlsOnly.ToString(), true, Constants.Daily, "", "");
                IHttpActionResult houseSaveResult2 = houseController.Post(JsonConvert.SerializeObject(house2));
                string houseId2 = ((OkNegotiatedContentResult<string>)houseSaveResult2).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(houseId2));

                // Saving Property # 3 - Apartment
                string ownerEmail3 = "house@1234567-3.com";
                string area3 = "Saddar, Rawalpindi, Pakistan";
                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail3)
                    })
                });
                CreateHouseCommand apartment3 = new CreateHouseCommand("Title # 3", 10000003, 1, 1,
                    1, false, true,
                    true, false, false, Constants.Apartment, ownerEmail3, "03000000000",
                    "", "", area3, DimensionType.Acre.ToString(), "2", 0, "Name 3", "",
                    GenderRestriction.NoRestriction.ToString(), false, Constants.Daily, "", "");
                IHttpActionResult houseSaveResult3 = houseController.Post(JsonConvert.SerializeObject(apartment3));
                Assert.IsFalse(string.IsNullOrWhiteSpace(((OkNegotiatedContentResult<string>)houseSaveResult3).Content));
                string apartmentId3 = ((OkNegotiatedContentResult<string>)houseSaveResult3).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(apartmentId3));

                // Saving Property # 4 - Hotel
                string ownerEmail4 = "house@1234567-4.com";
                string area4 = "Risalpur";
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail4)
                    })
                });
                CreateHotelCommand hotel4 = new CreateHotelCommand("title 4", 1000, false, false, false,
                    Constants.Hotel, ownerEmail4, "03000000000", area4, "someone", "", GenderRestriction.BoysOnly.ToString(),
                    false, Constants.Daily, true, true, true, true, true, true, false, false, false, false, false,
                    "", "", true, true, true, true, true, true, true, true, true, true, true, 0, 0, new Occupants(2, 0));
                IHttpActionResult houseSaveResult4 = houseController.Post(JsonConvert.SerializeObject(hotel4));
                string hotelId4 = ((OkNegotiatedContentResult<string>)houseSaveResult4).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hotelId4));

                // Saving Property # 5 - Hotel
                string ownerEmail5 = "house@1234567-5.com";
                string area5 = "Satellite Town, Rawalpindi";
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail5)
                    })
                });
                CreateHotelCommand hotel5 = new CreateHotelCommand("title 4", 1000, false, false, false,
                    Constants.Hotel, ownerEmail5, "03234567894", area5, "someone", "", GenderRestriction.BoysOnly.ToString(),
                    false, Constants.Daily, true, true, true, true, true, true, false, false, false, false, false,
                    "", "", true, true, true, true, true, true, true, true, true, true, true, 0, 0, new Occupants(2, 0));
                IHttpActionResult houseSaveResult5 = houseController.Post(JsonConvert.SerializeObject(hotel5));
                string hotelId5 = ((OkNegotiatedContentResult<string>)houseSaveResult5).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hotelId5));

                // Saving Property # 6 - Hostel
                string ownerEmail6 = "house@1234567-6.com";
                string area6 = "Nowshera";
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail6)
                    })
                });
                CreateHostelCommand hostel6 = new CreateHostelCommand("Title 6", 1000, false,
                    true, true, Constants.Hostel, ownerEmail6, "03990000000", area6, "SOmeone", "", GenderRestriction.BoysOnly.ToString(),
                    false, Constants.Daily, true, true, true, true, true, true, true, true, false, true, true,
                    "", "", false, false, false, 1);
                IHttpActionResult houseSaveResult6 = houseController.Post(JsonConvert.SerializeObject(hostel6));
                string hostelId6 = ((OkNegotiatedContentResult<string>)houseSaveResult6).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hostelId6));

                // Saving Property # 7 Hostel
                string ownerEmail7 = "house@1234567-7.com";
                string area7 = "Pindora, Rawalpindi";
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail7)
                    })
                });
                CreateHostelCommand hostel7 = new CreateHostelCommand("Title 7", 1000, false,
                    true, true, Constants.Hostel, ownerEmail7, "03990000000", area7, "SOmeone", "", GenderRestriction.BoysOnly.ToString(),
                    false, Constants.Daily, true, true, true, true, true, true, true, true, false, true, true,
                    "", "", false, false, false, 1);
                IHttpActionResult houseSaveResult7 = houseController.Post(JsonConvert.SerializeObject(hostel7));
                string hostelId7 = ((OkNegotiatedContentResult<string>)houseSaveResult7).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hostelId7));

                // Saving Property # 8 - Guest House
                string ownerEmail8 = "house@1234567-8.com";
                string ownerPhoneNumber8 = "01238567893";
                string area8 = "Risalpur";
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail8)
                    })
                });
                CreateHotelCommand guestHouse8 = new CreateHotelCommand("title 8", 1000, false, false, false,
                    Constants.GuestHouse, ownerEmail8, ownerPhoneNumber8, area8, "someone", "", GenderRestriction.BoysOnly.ToString(),
                    false, Constants.Daily, true, true, true, true, true, true, false, false, false, false, false,
                    "", "", true, true, true, true, true, true, true, true, true, true, true, 0, 0, new Occupants(2, 0));
                IHttpActionResult houseSaveResult8 = houseController.Post(JsonConvert.SerializeObject(guestHouse8));
                string guestHouseId8 = ((OkNegotiatedContentResult<string>)houseSaveResult8).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(guestHouseId8));

                // Saving Property # 9 - Guest House
                string ownerEmail9 = "house@1234967-9.com";
                string area9 = "Satellite Town, Rawalpindi";
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail9)
                    })
                });
                CreateHotelCommand guestHouse9 = new CreateHotelCommand("title 4", 1000, false, false, false,
                    Constants.GuestHouse, ownerEmail9, "03234967894", area9, "someone", "", GenderRestriction.BoysOnly.ToString(),
                    false, Constants.Daily, true, true, true, true, true, true, false, false, false, false, false,
                    "", "", true, true, true, true, true, true, true, true, true, true, true, 0, 0, new Occupants(2, 0));
                IHttpActionResult houseSaveResult9 = houseController.Post(JsonConvert.SerializeObject(guestHouse9));
                string guestHouseId9 = ((OkNegotiatedContentResult<string>)houseSaveResult9).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(guestHouseId9));

                // Saving Property # 10 - Apartment
                string ownerEmail10 = "house@12104567-10.com";
                string area10 = "Nowshera";
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail10)
                    })
                });
                CreateHouseCommand apartment10 = new CreateHouseCommand("Title 10", 10010, 1, 1,
                    1, false, true,
                    true, false, false, Constants.Apartment, ownerEmail10, "03000000000",
                    "", "", area10, DimensionType.Acre.ToString(), "2", 0, "Owner name", "",
                    GenderRestriction.GirlsOnly.ToString(), false, Constants.Daily, "", "");
                IHttpActionResult houseSaveResult10 = houseController.Post(JsonConvert.SerializeObject(apartment10));
                Assert.IsFalse(string.IsNullOrWhiteSpace(((OkNegotiatedContentResult<string>)houseSaveResult10).Content));
                string apartmentId10 = ((OkNegotiatedContentResult<string>)houseSaveResult10).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(apartmentId10));

                var searchLocation1 = "Pir Sohawa";
                var searchLocation2 = "Mardan";

                // ###### When House is searched ######
                IHttpActionResult response = (IHttpActionResult)houseController.Get(
                    propertyType: Constants.House);
                dynamic retreivedHouses = ((OkNegotiatedContentResult
                    <IList<ResidentialPropertyPartialBaseImplementation>>)response).Content;
                Assert.NotNull(retreivedHouses);
                Assert.AreEqual(2, retreivedHouses.Count);
                Assert.AreEqual(houseId, retreivedHouses[0].Id);
                Assert.AreEqual(house.Title, retreivedHouses[0].Title);
                Assert.AreEqual(Constants.House, retreivedHouses[0].PropertyType);
                Assert.AreEqual(houseId2, retreivedHouses[1].Id);
                Assert.AreEqual(house2.Title, retreivedHouses[1].Title);
                Assert.AreEqual(Constants.House, retreivedHouses[1].PropertyType);


                // ###### When Apartment is searched ######
                IHttpActionResult apartmentResponse = (IHttpActionResult)houseController.Get(
                    propertyType: Constants.Apartment);
                dynamic retreivedApartments = ((OkNegotiatedContentResult
                    <IList<ResidentialPropertyPartialBaseImplementation>>)apartmentResponse).Content;
                Assert.NotNull(retreivedApartments);
                Assert.AreEqual(2, retreivedApartments.Count);
                Assert.AreEqual(apartmentId3, retreivedApartments[0].Id);
                Assert.AreEqual(apartment3.Title, retreivedApartments[0].Title);
                Assert.AreEqual(Constants.Apartment, retreivedApartments[0].PropertyType);
                Assert.AreEqual(apartmentId10, retreivedApartments[1].Id);
                Assert.AreEqual(apartment10.Title, retreivedApartments[1].Title);
                Assert.AreEqual(Constants.Apartment, retreivedApartments[1].PropertyType);

                // ###### When Hotel is searched ######
                IHttpActionResult hotelResponse = (IHttpActionResult)houseController.Get(
                    propertyType: Constants.Hotel);
                dynamic retreivedHotels = ((OkNegotiatedContentResult
                    <IList<ResidentialPropertyPartialBaseImplementation>>)hotelResponse).Content;
                Assert.NotNull(retreivedHotels);
                Assert.AreEqual(2, retreivedHotels.Count);
                Assert.AreEqual(hotelId4, retreivedHotels[0].Id);
                Assert.AreEqual(hotel4.Title, retreivedHotels[0].Title);
                Assert.AreEqual(Constants.Hotel, retreivedHotels[0].PropertyType);
                Assert.AreEqual(hotelId5, retreivedHotels[1].Id);
                Assert.AreEqual(hotel5.Title, retreivedHotels[1].Title);
                Assert.AreEqual(Constants.Hotel, retreivedHotels[1].PropertyType);

                // ###### When Hostel is searched ######
                IHttpActionResult hostelResponse = (IHttpActionResult)houseController.Get(
                    propertyType: Constants.Hostel);
                dynamic retreivedHostels = ((OkNegotiatedContentResult
                    <IList<ResidentialPropertyPartialBaseImplementation>>)hostelResponse).Content;
                Assert.NotNull(retreivedHostels);
                Assert.AreEqual(2, retreivedHostels.Count);
                Assert.AreEqual(hostelId6, retreivedHostels[0].Id);
                Assert.AreEqual(hostel6.Title, retreivedHostels[0].Title);
                Assert.AreEqual(Constants.Hostel, retreivedHostels[0].PropertyType);
                Assert.AreEqual(hostelId7, retreivedHostels[1].Id);
                Assert.AreEqual(hostel7.Title, retreivedHostels[1].Title);
                Assert.AreEqual(Constants.Hostel, retreivedHostels[1].PropertyType);

                // ###### When Guest House is searched ######
                IHttpActionResult guestHouseResponse = (IHttpActionResult)houseController.Get(
                    propertyType: Constants.GuestHouse);
                dynamic retreivedGuestHouses = ((OkNegotiatedContentResult
                    <IList<ResidentialPropertyPartialBaseImplementation>>)guestHouseResponse).Content;
                Assert.NotNull(retreivedGuestHouses);
                Assert.AreEqual(2, retreivedGuestHouses.Count);
                Assert.AreEqual(guestHouseId8, retreivedGuestHouses[0].Id);
                Assert.AreEqual(guestHouse8.Title, retreivedGuestHouses[0].Title);
                Assert.AreEqual(Constants.GuestHouse, retreivedGuestHouses[0].PropertyType);
                Assert.AreEqual(guestHouseId9, retreivedGuestHouses[1].Id);
                Assert.AreEqual(guestHouse9.Title, retreivedGuestHouses[1].Title);
                Assert.AreEqual(Constants.GuestHouse, retreivedGuestHouses[1].PropertyType);
            }
        }

        [Category("Integration")]
        [Test]
        public void SearchByEmailTest_ChecksThatTheRequestPropertiesByEmailAreSearchedAndSavedAsExpcted_VerifiesThroughDatabaseRetrieval()
        {
            // Save 2 properties for each property type, at same location, with different emails. 
            // Only one email per each property type will be searched 
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);

                string propertySearchedEmail = "houseSearchedEmail@1234567-1.com";
                string propertyNonSearchedEmail = "houseNotSearchedEmail@1234567-1.com";

                // Saving Property # 1 - House
                string area = "Saddar, Rawalpindi";
                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, propertySearchedEmail)
                    })
                });
                CreateHouseCommand house = new CreateHouseCommand("title", 22000000, 1, 1, 1, true, false, false, true,
                    false, Constants.House, propertySearchedEmail, "03000000000", "1", "1", area, "Kanal", "1", 0, "Some name",
                    "", GenderRestriction.NoRestriction.ToString(), false, "Day", "", "");
                IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(house));
                string houseId = ((OkNegotiatedContentResult<string>)houseSaveResult).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(houseId));

                // Saving Property # 2 - House
                string area2 = "Saddar, Rawalpindi";
                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, propertyNonSearchedEmail)
                    })
                });
                CreateHouseCommand house2 = new CreateHouseCommand("Title # 2", 1400000, 1, 1,
                    1, true, false, false, false, true, Constants.House, propertyNonSearchedEmail, "03000000000",
                    "", "", area2, "Kanal", "2", 0, "Name", "", GenderRestriction.GirlsOnly.ToString(), true, Constants.Daily, "", "");
                IHttpActionResult houseSaveResult2 = houseController.Post(JsonConvert.SerializeObject(house2));
                string houseId2 = ((OkNegotiatedContentResult<string>)houseSaveResult2).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(houseId2));

                // Apartment emails
                // Saving Property # 3 - Apartment
                string area3 = "Saddar, Rawalpindi";
                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, propertyNonSearchedEmail)
                    })
                });
                CreateHouseCommand apartment3 = new CreateHouseCommand("Title # 3", 10000003, 1, 1,
                    1, false, true,
                    true, false, false, Constants.Apartment, propertyNonSearchedEmail, "03000000000",
                    "", "", area3, DimensionType.Acre.ToString(), "2", 0, "Name 3", "",
                    GenderRestriction.NoRestriction.ToString(), false, Constants.Daily, "", "");
                IHttpActionResult houseSaveResult3 = houseController.Post(JsonConvert.SerializeObject(apartment3));
                Assert.IsFalse(string.IsNullOrWhiteSpace(((OkNegotiatedContentResult<string>)houseSaveResult3).Content));
                string apartmentId3 = ((OkNegotiatedContentResult<string>)houseSaveResult3).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(apartmentId3));

                // Hotel Email Declaration. Covers Hotels
                // Saving Property # 4 - Hotel
                string area4 = "Saddar, Rawalpindi";
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, propertySearchedEmail)
                    })
                });
                CreateHotelCommand hotel4 = new CreateHotelCommand("title 4", 1000, false, false, false,
                    Constants.Hotel, propertySearchedEmail, "03000000000", area4, "someone", "", GenderRestriction.BoysOnly.ToString(),
                    false, Constants.Daily, true, true, true, true, true, true, false, false, false, false, false,
                    "", "", true, true, true, true, true, true, true, true, true, true, true, 0, 0, new Occupants(2, 0));
                IHttpActionResult houseSaveResult4 = houseController.Post(JsonConvert.SerializeObject(hotel4));
                string hotelId4 = ((OkNegotiatedContentResult<string>)houseSaveResult4).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hotelId4));

                // Saving Property # 5 - Hotel
                string area5 = "Satellite Town, Rawalpindi";
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, propertyNonSearchedEmail)
                    })
                });
                CreateHotelCommand hotel5 = new CreateHotelCommand("title 4", 1000, false, false, false,
                    Constants.Hotel, propertyNonSearchedEmail, "03234567894", area5, "someone", "", GenderRestriction.BoysOnly.ToString(),
                    false, Constants.Daily, true, true, true, true, true, true, false, false, false, false, false,
                    "", "", true, true, true, true, true, true, true, true, true, true, true, 0, 0, new Occupants(2, 0));
                IHttpActionResult houseSaveResult5 = houseController.Post(JsonConvert.SerializeObject(hotel5));
                string hotelId5 = ((OkNegotiatedContentResult<string>)houseSaveResult5).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hotelId5));
                
                // Saving Property # 6 - Hostel
                string area6 = "Saddar, Rawalpindi";
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, propertyNonSearchedEmail)
                    })
                });
                CreateHostelCommand hostel6 = new CreateHostelCommand("Title 6", 1000, false,
                    true, true, Constants.Hostel, propertyNonSearchedEmail, "03990000000", area6, "SOmeone", "", GenderRestriction.BoysOnly.ToString(),
                    false, Constants.Daily, true, true, true, true, true, true, true, true, false, true, true,
                    "", "", false, false, false, 1);
                IHttpActionResult houseSaveResult6 = houseController.Post(JsonConvert.SerializeObject(hostel6));
                string hostelId6 = ((OkNegotiatedContentResult<string>)houseSaveResult6).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hostelId6));

                // Saving Property # 7 Hostel
                string area7 = "Pindora, Rawalpindi";
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, propertySearchedEmail)
                    })
                });
                CreateHostelCommand hostel7 = new CreateHostelCommand("Title 7", 1000, false,
                    true, true, Constants.Hostel, propertySearchedEmail, "03990000000", area7, "SOmeone", "", GenderRestriction.BoysOnly.ToString(),
                    false, Constants.Daily, true, true, true, true, true, true, true, true, false, true, true,
                    "", "", false, false, false, 1);
                IHttpActionResult houseSaveResult7 = houseController.Post(JsonConvert.SerializeObject(hostel7));
                string hostelId7 = ((OkNegotiatedContentResult<string>)houseSaveResult7).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hostelId7));

                // Guest House Emails
                // Saving Property # 8 - Guest House
                string ownerPhoneNumber8 = "01238567893";
                string area8 = "Saddar, Rawalpindi";
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, propertyNonSearchedEmail)
                    })
                });
                CreateHotelCommand guestHouse8 = new CreateHotelCommand("title 8", 1000, false, false, false,
                    Constants.GuestHouse, propertyNonSearchedEmail, ownerPhoneNumber8, area8, "someone", "", GenderRestriction.BoysOnly.ToString(),
                    false, Constants.Daily, true, true, true, true, true, true, false, false, false, false, false,
                    "", "", true, true, true, true, true, true, true, true, true, true, true, 0, 0, new Occupants(2, 0));
                IHttpActionResult houseSaveResult8 = houseController.Post(JsonConvert.SerializeObject(guestHouse8));
                string guestHouseId8 = ((OkNegotiatedContentResult<string>)houseSaveResult8).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(guestHouseId8));

                // Saving Property # 9 - Guest House
                string area9 = "Saddar, Rawalpindi";
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, propertySearchedEmail)
                    })
                });
                CreateHotelCommand guestHouse9 = new CreateHotelCommand("title 4", 1000, false, false, false,
                    Constants.GuestHouse, propertySearchedEmail, "03234967894", area9, "someone", "", GenderRestriction.BoysOnly.ToString(),
                    false, Constants.Daily, true, true, true, true, true, true, false, false, false, false, false,
                    "", "", true, true, true, true, true, true, true, true, true, true, true, 0, 0, new Occupants(2, 0));
                IHttpActionResult houseSaveResult9 = houseController.Post(JsonConvert.SerializeObject(guestHouse9));
                string guestHouseId9 = ((OkNegotiatedContentResult<string>)houseSaveResult9).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(guestHouseId9));

                // Saving Property # 10 - Apartment
                string area10 = "Saddar, Rawalpindi";
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, propertySearchedEmail)
                    })
                });
                CreateHouseCommand apartment10 = new CreateHouseCommand("Title 10", 10010, 1, 1,
                    1, false, true,
                    true, false, false, Constants.Apartment, propertySearchedEmail, "03000000000",
                    "", "", area10, DimensionType.Acre.ToString(), "2", 0, "Owner name", "",
                    GenderRestriction.GirlsOnly.ToString(), false, Constants.Daily, "", "");
                IHttpActionResult houseSaveResult10 = houseController.Post(JsonConvert.SerializeObject(apartment10));
                Assert.IsFalse(string.IsNullOrWhiteSpace(((OkNegotiatedContentResult<string>)houseSaveResult10).Content));
                string apartmentId10 = ((OkNegotiatedContentResult<string>)houseSaveResult10).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(apartmentId10));

                // ###### When House is searched ######
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, propertySearchedEmail)
                    })
                });
                IHttpActionResult response = (IHttpActionResult)houseController.Get(email: propertySearchedEmail);
                dynamic retreivedHouses = ((OkNegotiatedContentResult
                    <IList<ResidentialPropertyPartialBaseImplementation>>)response).Content;
                Assert.NotNull(retreivedHouses);
                Assert.AreEqual(5, retreivedHouses.Count);
                
                // ###### Hotel ######
                Assert.AreEqual(hotelId4, retreivedHouses[0].Id);
                Assert.AreEqual(hotel4.Title, retreivedHouses[0].Title);
                Assert.AreEqual(Constants.Hotel, retreivedHouses[0].PropertyType);

                // ###### Guest House ######
                Assert.AreEqual(guestHouseId9, retreivedHouses[1].Id);
                Assert.AreEqual(guestHouse9.Title, retreivedHouses[1].Title);
                Assert.AreEqual(Constants.GuestHouse, retreivedHouses[1].PropertyType);

                // ###### Hostel ######
                Assert.AreEqual(hostelId7, retreivedHouses[2].Id);
                Assert.AreEqual(hostel7.Title, retreivedHouses[2].Title);
                Assert.AreEqual(Constants.Hostel, retreivedHouses[2].PropertyType);
                
                // ###### House ######
                Assert.AreEqual(houseId, retreivedHouses[3].Id);
                Assert.AreEqual(house.Title, retreivedHouses[3].Title);
                Assert.AreEqual(Constants.House, retreivedHouses[3].PropertyType);

                // ###### Apartment ######
                Assert.AreEqual(apartmentId10, retreivedHouses[4].Id);
                Assert.AreEqual(apartment10.Title, retreivedHouses[4].Title);
                Assert.AreEqual(Constants.Apartment, retreivedHouses[4].PropertyType);
            }
        }
        
        #endregion Search Tests

        #region GetByPropertyType: Single Partial Representation Evaluation Tests

        [Category("Integration")]
        [Test]
        public void GetHousesByPropertyTypeSingleInstanceEvaluationInstanceByIdTest_TestsThatHouseIsRetrievedByPropertyTypeAndAllFieldsAreAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);

                string title = "House For Rent";
                string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
                int rent = 105000;
                string ownerEmail = "thorin@oakenshield123.com";
                string ownerPhoneNumber = "01234567890";
                string houseNo = "CT-141/A";
                string streetNo = "14";
                int numberOfBathrooms = 1;
                int numberOfBedrooms = 1;
                int numberOfKitchens = 1;
                bool familiesOnly = false;
                bool boysOnly = false;
                bool girlsOnly = true;
                bool internetAvailable = true;
                bool landlinePhoneAvailable = true;
                bool cableTvAvailable = true;
                bool garageAvailable = true;
                bool smokingAllowed = true;
                string propertyType = Constants.House;
                string area = "Pindora, Rawalpindi, Pakistan";
                //string area = "Pindora, Rawalpindi, Pakistan";
                string dimensionType = DimensionType.Kanal.ToString();
                string dimensionString = "5";
                decimal dimensionDecimal = 0;
                string ownerName = "Owner Name 1";
                string genderRestriction = GenderRestriction.FamiliesOnly.ToString();
                bool isShared = true;
                string rentUnit = House.GetAllRentUnits()[0];
                string landlineNumber = "0510000000";
                string fax = "0510000000";

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail)
                    })
                });
                CreateHouseCommand house = new CreateHouseCommand(title, rent, numberOfBedrooms, numberOfKitchens,
                    numberOfBathrooms, internetAvailable, landlinePhoneAvailable,
                    cableTvAvailable, garageAvailable, smokingAllowed, propertyType, ownerEmail, ownerPhoneNumber,
                    houseNo, streetNo, area, dimensionType, dimensionString, dimensionDecimal, ownerName, description, genderRestriction,
                    isShared, rentUnit, landlineNumber, fax);
                IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(house));
                string houseId = ((OkNegotiatedContentResult<string>)houseSaveResult).Content;

                IHttpActionResult response = (IHttpActionResult)houseController.Get(propertyType: propertyType);
                dynamic retreivedHouse = ((OkNegotiatedContentResult<IList<ResidentialPropertyPartialBaseImplementation>>)response).Content;
                Assert.NotNull(retreivedHouse);
                Assert.AreEqual(houseId, retreivedHouse[0].Id);
                Assert.AreEqual(title, retreivedHouse[0].Title);
                Assert.AreEqual(ownerPhoneNumber, retreivedHouse[0].OwnerPhoneNumber);
                Assert.AreEqual(rent, retreivedHouse[0].RentPrice);
                Assert.AreEqual(numberOfBathrooms, retreivedHouse[0].NumberOfBathrooms);
                Assert.AreEqual(numberOfBedrooms, retreivedHouse[0].NumberOfBedrooms);
                Assert.AreEqual(numberOfKitchens, retreivedHouse[0].NumberOfKitchens);
                Assert.AreEqual(propertyType, retreivedHouse[0].PropertyType);
                Assert.AreEqual(isShared, retreivedHouse[0].IsShared);
                Assert.AreEqual(area, retreivedHouse[0].Area);
                Assert.AreEqual(dimensionString + " " + dimensionType, retreivedHouse[0].Dimension);
                Assert.AreEqual(ownerName, retreivedHouse[0].OwnerName);
                Assert.AreEqual(genderRestriction, retreivedHouse[0].GenderRestriction);
                Assert.AreEqual(rentUnit, retreivedHouse[0].RentUnit);
            }
        }

        [Category("Integration")]
        [Test]
        public void GetApartmentsByPropertyTypeSingleInstanceEvaluationInstanceByIdTest_TestsThatApartmentIsRetrievedByPropertyTypeAndAllFieldsAreAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);

                string title = "House For Rent";
                string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
                int rent = 105000;
                string ownerEmail = "thorin@oakenshield123.com";
                string ownerPhoneNumber = "01234567890";
                string houseNo = "CT-141/A";
                string streetNo = "14";
                int numberOfBathrooms = 1;
                int numberOfBedrooms = 1;
                int numberOfKitchens = 1;
                bool familiesOnly = false;
                bool boysOnly = false;
                bool girlsOnly = true;
                bool internetAvailable = true;
                bool landlinePhoneAvailable = true;
                bool cableTvAvailable = true;
                bool garageAvailable = true;
                bool smokingAllowed = true;
                string propertyType = Constants.Apartment;
                string area = "Pindora, Rawalpindi, Pakistan";
                //string area = "Pindora, Rawalpindi, Pakistan";
                string dimensionType = DimensionType.Kanal.ToString();
                string dimensionString = "5";
                decimal dimensionDecimal = 0;
                string ownerName = "Owner Name 1";
                string genderRestriction = GenderRestriction.FamiliesOnly.ToString();
                bool isShared = true;
                string rentUnit = House.GetAllRentUnits()[0];
                string landlineNumber = "0510000000";
                string fax = "0510000000";

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail)
                    })
                });
                CreateHouseCommand house = new CreateHouseCommand(title, rent, numberOfBedrooms, numberOfKitchens,
                    numberOfBathrooms, internetAvailable, landlinePhoneAvailable,
                    cableTvAvailable, garageAvailable, smokingAllowed, propertyType, ownerEmail, ownerPhoneNumber,
                    houseNo, streetNo, area, dimensionType, dimensionString, dimensionDecimal, ownerName, description, genderRestriction,
                    isShared, rentUnit, landlineNumber, fax);
                IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(house));
                string houseId = ((OkNegotiatedContentResult<string>)houseSaveResult).Content;

                IHttpActionResult response = (IHttpActionResult)houseController.Get(propertyType: propertyType);
                dynamic retreivedHouse = ((OkNegotiatedContentResult<IList<ResidentialPropertyPartialBaseImplementation>>)response).Content;
                Assert.NotNull(retreivedHouse);
                Assert.AreEqual(houseId, retreivedHouse[0].Id);
                Assert.AreEqual(title, retreivedHouse[0].Title);
                Assert.AreEqual(ownerPhoneNumber, retreivedHouse[0].OwnerPhoneNumber);
                Assert.AreEqual(rent, retreivedHouse[0].RentPrice);
                Assert.AreEqual(numberOfBathrooms, retreivedHouse[0].NumberOfBathrooms);
                Assert.AreEqual(numberOfBedrooms, retreivedHouse[0].NumberOfBedrooms);
                Assert.AreEqual(numberOfKitchens, retreivedHouse[0].NumberOfKitchens);
                Assert.AreEqual(propertyType, retreivedHouse[0].PropertyType);
                Assert.AreEqual(isShared, retreivedHouse[0].IsShared);
                Assert.AreEqual(area, retreivedHouse[0].Area);
                Assert.AreEqual(dimensionString + " " + dimensionType, retreivedHouse[0].Dimension);
                Assert.AreEqual(ownerName, retreivedHouse[0].OwnerName);
                Assert.AreEqual(genderRestriction, retreivedHouse[0].GenderRestriction);
                Assert.AreEqual(rentUnit, retreivedHouse[0].RentUnit);
            }
        }

        [Category("Integration")]
        [Test]
        public void GetHostelsByPropertyTypeSingleInstanceEvaluationInstanceByIdTest_TestsThatHostelIsRetrievedByPropertyTypeAndAllFieldsAreAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);

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

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, email)
                    })
                });

                var postHttpActionResult = houseController.Post(JsonConvert.SerializeObject(createNewHostelCommand));
                string hostelId = ((OkNegotiatedContentResult<string>)postHttpActionResult).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hostelId));

                var retrievedHostelHttpResult = houseController.Get(propertyType: propertyType);
                dynamic retrievedHostel = ((OkNegotiatedContentResult<IList<ResidentialPropertyPartialBaseImplementation>>)retrievedHostelHttpResult).Content;
                Assert.IsNotNull(retrievedHostel);

                Assert.AreEqual(hostelId, retrievedHostel[0].Id);
                Assert.AreEqual(title, retrievedHostel[0].Title);
                Assert.AreEqual(name, retrievedHostel[0].OwnerName);
                Assert.AreEqual(phoneNumber, retrievedHostel[0].OwnerPhoneNumber);
                Assert.AreEqual(propertyType, retrievedHostel[0].PropertyType);
                Assert.AreEqual(genderRestriction.ToString(), retrievedHostel[0].GenderRestriction);
                Assert.AreEqual(area, retrievedHostel[0].Area);
                Assert.AreEqual(monthlyRent, retrievedHostel[0].RentPrice);
                Assert.AreEqual(isShared, retrievedHostel[0].IsShared);
                Assert.AreEqual(rentUnit, retrievedHostel[0].RentUnit);
                Assert.AreEqual(laundry, retrievedHostel[0].Laundry);
                Assert.AreEqual(ac, retrievedHostel[0].AC);
                Assert.AreEqual(geyser, retrievedHostel[0].Geyser);
                Assert.AreEqual(attachedBathroom, retrievedHostel[0].AttachedBathroom);
                Assert.AreEqual(backupElectricity, retrievedHostel[0].BackupElectricity);
                Assert.AreEqual(meals, retrievedHostel[0].Meals);
                Assert.AreEqual(numberOfSeats, retrievedHostel[0].NumberOfSeats);
            }
        }

        [Test]
        public void GetHotelsByPropertyTypeSingleInstanceEvaluationInstanceByIdTest_TestsThatHotelIsRetrievedByPropertyTypeAndAllFieldsAreAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);
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
                int numberOfSingleBeds = 2;
                int numberOfDoubleBeds = 1;

                string landlineNumber = "0510000000";
                string fax = "0510000000";
                bool elevator = false;
                
                var createNewHotelCommand = new CreateHotelCommand(title, monthlyRent, internet,
                    cableTv, parking, propertyType, email, phoneNumber, area, name, description,
                    genderRestriction.ToString(),
                    isShared, rentUnit, laundry, ac, geyser, fitnessCentre, attachedBathroom, ironing, balcony,
                    lawn, cctvCameras, backupElectricity, heating, landlineNumber, fax, elevator, restaurant,
                    airportShuttle, breakfastIncluded, sittingArea, carRental, spa, salon, bathtub, swimmingPool,
                    kitchen, numberOfSingleBeds, numberOfDoubleBeds, new Occupants(numberOfAdults, numberOfChildren));

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, email)
                    })
                });

                var postHttpActionResult = houseController.Post(JsonConvert.SerializeObject(createNewHotelCommand));
                string hotelId = ((OkNegotiatedContentResult<string>)postHttpActionResult).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hotelId));

                var retrievedHostelHttpResult = houseController.Get(propertyType: propertyType);
                dynamic retrievedHotel = ((OkNegotiatedContentResult<IList<ResidentialPropertyPartialBaseImplementation>>)retrievedHostelHttpResult).Content;
                Assert.IsNotNull(retrievedHotel);

                Assert.IsNotNull(retrievedHotel);
                Assert.AreEqual(hotelId, retrievedHotel[0].Id);
                Assert.AreEqual(title, retrievedHotel[0].Title);
                Assert.AreEqual(name, retrievedHotel[0].OwnerName);
                Assert.AreEqual(phoneNumber, retrievedHotel[0].OwnerPhoneNumber);
                Assert.AreEqual(propertyType, retrievedHotel[0].PropertyType);
                Assert.AreEqual(genderRestriction.ToString(), retrievedHotel[0].GenderRestriction);
                Assert.AreEqual(area, retrievedHotel[0].Area);
                Assert.AreEqual(monthlyRent, retrievedHotel[0].RentPrice);
                Assert.AreEqual(rentUnit, retrievedHotel[0].RentUnit);
                Assert.AreEqual(isShared, retrievedHotel[0].IsShared);
                Assert.AreEqual(ac, retrievedHotel[0].AC);
                Assert.AreEqual(geyser, retrievedHotel[0].Geyser);
                Assert.AreEqual(attachedBathroom, retrievedHotel[0].AttachedBathroom);
                Assert.AreEqual(fitnessCentre, retrievedHotel[0].FitnessCentre);
                Assert.AreEqual(backupElectricity, retrievedHotel[0].BackupElectricity);
                Assert.AreEqual(heating, retrievedHotel[0].Heating);
                
                Assert.AreEqual(airportShuttle, retrievedHotel[0].AirportShuttle);
                Assert.AreEqual(breakfastIncluded, retrievedHotel[0].BreakfastIncluded);

                Assert.AreEqual(numberOfSingleBeds, retrievedHotel[0].NumberOfSingleBeds);
                Assert.AreEqual(numberOfDoubleBeds, retrievedHotel[0].NumberOfDoubleBeds);

                Assert.AreEqual(numberOfAdults, retrievedHotel[0].Occupants.Adults);
                Assert.AreEqual(numberOfChildren, retrievedHotel[0].Occupants.Children);
                Assert.AreEqual(numberOfAdults + numberOfChildren, retrievedHotel[0].Occupants.TotalOccupants);
            }
        }

        [Test]
        public void GetGuestHousesByPropertyTypeSingleInstanceEvaluationInstanceByIdTest_TestsThatGuestHouseIsRetrievedByPropertyTypeAndAllFieldsAreAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);
                string title = "Title No 1";
                string description = "Description of house";
                string email = "w@12344321.com";
                string name = "OwnerName";
                string phoneNumber = "03455138018";
                string propertyType = Constants.GuestHouse;
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

                int numberOfSingleBeds = 2;
                int numberOfDoubleBeds = 1;

                string landlineNumber = "0510000000";
                string fax = "0510000000";
                bool elevator = false;
                

                var createNewHotelCommand = new CreateHotelCommand(title, monthlyRent, internet,
                    cableTv, parking, propertyType, email, phoneNumber, area, name, description,
                    genderRestriction.ToString(),
                    isShared, rentUnit, laundry, ac, geyser, fitnessCentre, attachedBathroom, ironing, balcony,
                    lawn, cctvCameras, backupElectricity, heating, landlineNumber, fax, elevator, restaurant,
                    airportShuttle, breakfastIncluded, sittingArea, carRental, spa, salon, bathtub, swimmingPool,
                    kitchen, numberOfSingleBeds, numberOfDoubleBeds, new Occupants(numberOfAdults, numberOfChildren));

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, email)
                    })
                });

                var postHttpActionResult = houseController.Post(JsonConvert.SerializeObject(createNewHotelCommand));
                string hotelId = ((OkNegotiatedContentResult<string>)postHttpActionResult).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hotelId));

                var retrievedHostelHttpResult = houseController.Get(propertyType: propertyType);
                dynamic retrievedHotel = ((OkNegotiatedContentResult<IList<ResidentialPropertyPartialBaseImplementation>>)retrievedHostelHttpResult).Content;
                Assert.IsNotNull(retrievedHotel);

                Assert.IsNotNull(retrievedHotel);
                Assert.AreEqual(hotelId, retrievedHotel[0].Id);
                Assert.AreEqual(title, retrievedHotel[0].Title);
                Assert.AreEqual(name, retrievedHotel[0].OwnerName);
                Assert.AreEqual(phoneNumber, retrievedHotel[0].OwnerPhoneNumber);
                Assert.AreEqual(propertyType, retrievedHotel[0].PropertyType);
                Assert.AreEqual(genderRestriction.ToString(), retrievedHotel[0].GenderRestriction);
                Assert.AreEqual(area, retrievedHotel[0].Area);
                Assert.AreEqual(monthlyRent, retrievedHotel[0].RentPrice);
                Assert.AreEqual(rentUnit, retrievedHotel[0].RentUnit);
                Assert.AreEqual(isShared, retrievedHotel[0].IsShared);
                Assert.AreEqual(ac, retrievedHotel[0].AC);
                Assert.AreEqual(geyser, retrievedHotel[0].Geyser);
                Assert.AreEqual(attachedBathroom, retrievedHotel[0].AttachedBathroom);
                Assert.AreEqual(fitnessCentre, retrievedHotel[0].FitnessCentre);
                Assert.AreEqual(backupElectricity, retrievedHotel[0].BackupElectricity);
                Assert.AreEqual(heating, retrievedHotel[0].Heating);

                Assert.AreEqual(numberOfSingleBeds, retrievedHotel[0].NumberOfSingleBeds);
                Assert.AreEqual(numberOfDoubleBeds, retrievedHotel[0].NumberOfDoubleBeds);

                Assert.AreEqual(airportShuttle, retrievedHotel[0].AirportShuttle);
                Assert.AreEqual(breakfastIncluded, retrievedHotel[0].BreakfastIncluded);

                Assert.AreEqual(numberOfAdults, retrievedHotel[0].Occupants.Adults);
                Assert.AreEqual(numberOfChildren, retrievedHotel[0].Occupants.Children);
                Assert.AreEqual(numberOfAdults + numberOfChildren, retrievedHotel[0].Occupants.TotalOccupants);
            }
        }

        #endregion GetByPropertyType: SIngle Partial Representation Evaluation Tests

        #region Get By PropertyType and Location: Single Partial Representation Evaluation Tests

        [Category("Integration")]
        [Test]
        public void GetHousesByPropertyTypeAndLocationSingleInstanceEvaluationInstanceByIdTest_TestsThatHouseIsRetrievedByPropertyTypeAndLocationAndAllFieldsAreAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);

                string title = "House For Rent";
                string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
                int rent = 105000;
                string ownerEmail = "thorin@oakenshield123.com";
                string ownerPhoneNumber = "01234567890";
                string houseNo = "CT-141/A";
                string streetNo = "14";
                int numberOfBathrooms = 1;
                int numberOfBedrooms = 1;
                int numberOfKitchens = 1;
                bool familiesOnly = false;
                bool boysOnly = false;
                bool girlsOnly = true;
                bool internetAvailable = true;
                bool landlinePhoneAvailable = true;
                bool cableTvAvailable = true;
                bool garageAvailable = true;
                bool smokingAllowed = true;
                string propertyType = Constants.House;
                string area = "Pindora, Rawalpindi, Pakistan";
                //string area = "Pindora, Rawalpindi, Pakistan";
                string dimensionType = DimensionType.Kanal.ToString();
                string dimensionString = "5";
                decimal dimensionDecimal = 0;
                string ownerName = "Owner Name 1";
                string genderRestriction = GenderRestriction.FamiliesOnly.ToString();
                bool isShared = true;
                string rentUnit = House.GetAllRentUnits()[0];
                string landlineNumber = "0510000000";
                string fax = "0510000000";

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail)
                    })
                });
                CreateHouseCommand house = new CreateHouseCommand(title, rent, numberOfBedrooms, numberOfKitchens,
                    numberOfBathrooms, internetAvailable, landlinePhoneAvailable,
                    cableTvAvailable, garageAvailable, smokingAllowed, propertyType, ownerEmail, ownerPhoneNumber,
                    houseNo, streetNo, area, dimensionType, dimensionString, dimensionDecimal, ownerName, description, genderRestriction,
                    isShared, rentUnit, landlineNumber, fax);
                IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(house));
                string houseId = ((OkNegotiatedContentResult<string>)houseSaveResult).Content;

                IHttpActionResult response = (IHttpActionResult)houseController.Get(area: area, propertyType: propertyType);
                dynamic retreivedHouse = ((OkNegotiatedContentResult<IList<ResidentialPropertyPartialBaseImplementation>>)response).Content;
                Assert.NotNull(retreivedHouse);
                Assert.AreEqual(houseId, retreivedHouse[0].Id);
                Assert.AreEqual(title, retreivedHouse[0].Title);
                Assert.AreEqual(ownerPhoneNumber, retreivedHouse[0].OwnerPhoneNumber);
                Assert.AreEqual(rent, retreivedHouse[0].RentPrice);
                Assert.AreEqual(numberOfBathrooms, retreivedHouse[0].NumberOfBathrooms);
                Assert.AreEqual(numberOfBedrooms, retreivedHouse[0].NumberOfBedrooms);
                Assert.AreEqual(numberOfKitchens, retreivedHouse[0].NumberOfKitchens);
                Assert.AreEqual(propertyType, retreivedHouse[0].PropertyType);
                Assert.AreEqual(isShared, retreivedHouse[0].IsShared);
                Assert.AreEqual(area, retreivedHouse[0].Area);
                Assert.AreEqual(dimensionString + " " + dimensionType, retreivedHouse[0].Dimension);
                Assert.AreEqual(ownerName, retreivedHouse[0].OwnerName);
                Assert.AreEqual(genderRestriction, retreivedHouse[0].GenderRestriction);
                Assert.AreEqual(rentUnit, retreivedHouse[0].RentUnit);
            }
        }

        [Category("Integration")]
        [Test]
        public void GetApartmentsByLocationAndPropertyTypeSingleInstanceEvaluationInstanceByIdTest_TestsThatApartmentIsRetrievedByLocationAndPropertyTypeAndAllFieldsAreAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);

                string title = "House For Rent";
                string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
                int rent = 105000;
                string ownerEmail = "thorin@oakenshield123.com";
                string ownerPhoneNumber = "01234567890";
                string houseNo = "CT-141/A";
                string streetNo = "14";
                int numberOfBathrooms = 1;
                int numberOfBedrooms = 1;
                int numberOfKitchens = 1;
                bool familiesOnly = false;
                bool boysOnly = false;
                bool girlsOnly = true;
                bool internetAvailable = true;
                bool landlinePhoneAvailable = true;
                bool cableTvAvailable = true;
                bool garageAvailable = true;
                bool smokingAllowed = true;
                string propertyType = Constants.Apartment;
                string area = "Pindora, Rawalpindi, Pakistan";
                //string area = "Pindora, Rawalpindi, Pakistan";
                string dimensionType = DimensionType.Kanal.ToString();
                string dimensionString = "5";
                decimal dimensionDecimal = 0;
                string ownerName = "Owner Name 1";
                string genderRestriction = GenderRestriction.FamiliesOnly.ToString();
                bool isShared = true;
                string rentUnit = House.GetAllRentUnits()[0];
                string landlineNumber = "0510000000";
                string fax = "0510000000";

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail)
                    })
                });
                CreateHouseCommand house = new CreateHouseCommand(title, rent, numberOfBedrooms, numberOfKitchens,
                    numberOfBathrooms, internetAvailable, landlinePhoneAvailable,
                    cableTvAvailable, garageAvailable, smokingAllowed, propertyType, ownerEmail, ownerPhoneNumber,
                    houseNo, streetNo, area, dimensionType, dimensionString, dimensionDecimal, ownerName, description, genderRestriction,
                    isShared, rentUnit, landlineNumber, fax);
                IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(house));
                string houseId = ((OkNegotiatedContentResult<string>)houseSaveResult).Content;

                IHttpActionResult response = (IHttpActionResult)houseController.Get(area: area, propertyType: propertyType);
                dynamic retreivedHouse = ((OkNegotiatedContentResult<IList<ResidentialPropertyPartialBaseImplementation>>)response).Content;
                Assert.NotNull(retreivedHouse);
                Assert.AreEqual(houseId, retreivedHouse[0].Id);
                Assert.AreEqual(title, retreivedHouse[0].Title);
                Assert.AreEqual(ownerPhoneNumber, retreivedHouse[0].OwnerPhoneNumber);
                Assert.AreEqual(rent, retreivedHouse[0].RentPrice);
                Assert.AreEqual(numberOfBathrooms, retreivedHouse[0].NumberOfBathrooms);
                Assert.AreEqual(numberOfBedrooms, retreivedHouse[0].NumberOfBedrooms);
                Assert.AreEqual(numberOfKitchens, retreivedHouse[0].NumberOfKitchens);
                Assert.AreEqual(propertyType, retreivedHouse[0].PropertyType);
                Assert.AreEqual(isShared, retreivedHouse[0].IsShared);
                Assert.AreEqual(area, retreivedHouse[0].Area);
                Assert.AreEqual(dimensionString + " " + dimensionType, retreivedHouse[0].Dimension);
                Assert.AreEqual(ownerName, retreivedHouse[0].OwnerName);
                Assert.AreEqual(genderRestriction, retreivedHouse[0].GenderRestriction);
                Assert.AreEqual(rentUnit, retreivedHouse[0].RentUnit);
            }
        }

        [Category("Integration")]
        [Test]
        public void GetHostelsByLocationAndPropertyTypeSingleInstanceEvaluationInstanceByIdTest_TestsThatHostelIsRetrievedBylocationAndPropertyTypeAndAllFieldsAreAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);

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

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, email)
                    })
                });

                var postHttpActionResult = houseController.Post(JsonConvert.SerializeObject(createNewHostelCommand));
                string hostelId = ((OkNegotiatedContentResult<string>)postHttpActionResult).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hostelId));

                var retrievedHostelHttpResult = houseController.Get(area: area, propertyType: propertyType);
                dynamic retrievedHostel = ((OkNegotiatedContentResult<IList<ResidentialPropertyPartialBaseImplementation>>)retrievedHostelHttpResult).Content;
                Assert.IsNotNull(retrievedHostel);

                Assert.AreEqual(hostelId, retrievedHostel[0].Id);
                Assert.AreEqual(title, retrievedHostel[0].Title);
                Assert.AreEqual(name, retrievedHostel[0].OwnerName);
                Assert.AreEqual(phoneNumber, retrievedHostel[0].OwnerPhoneNumber);
                Assert.AreEqual(propertyType, retrievedHostel[0].PropertyType);
                Assert.AreEqual(genderRestriction.ToString(), retrievedHostel[0].GenderRestriction);
                Assert.AreEqual(area, retrievedHostel[0].Area);
                Assert.AreEqual(monthlyRent, retrievedHostel[0].RentPrice);
                Assert.AreEqual(isShared, retrievedHostel[0].IsShared);
                Assert.AreEqual(rentUnit, retrievedHostel[0].RentUnit);
                Assert.AreEqual(laundry, retrievedHostel[0].Laundry);
                Assert.AreEqual(ac, retrievedHostel[0].AC);
                Assert.AreEqual(geyser, retrievedHostel[0].Geyser);
                Assert.AreEqual(attachedBathroom, retrievedHostel[0].AttachedBathroom);
                Assert.AreEqual(backupElectricity, retrievedHostel[0].BackupElectricity);
                Assert.AreEqual(meals, retrievedHostel[0].Meals);
                Assert.AreEqual(numberOfSeats, retrievedHostel[0].NumberOfSeats);
            }
        }

        [Test]
        public void GetHotelsByLocationAndPropertyTypeSingleInstanceEvaluationInstanceByIdTest_TestsThatHotelIsRetrievedByLocationAndPropertyTypeAndAllFieldsAreAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);
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
                    kitchen, 0, 0, new Occupants(numberOfAdults, numberOfChildren));

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, email)
                    })
                });

                var postHttpActionResult = houseController.Post(JsonConvert.SerializeObject(createNewHotelCommand));
                string hotelId = ((OkNegotiatedContentResult<string>)postHttpActionResult).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hotelId));

                var retrievedHostelHttpResult = houseController.Get(area: area, propertyType: propertyType);
                dynamic retrievedHotel = ((OkNegotiatedContentResult<IList<ResidentialPropertyPartialBaseImplementation>>)retrievedHostelHttpResult).Content;
                Assert.IsNotNull(retrievedHotel);

                Assert.IsNotNull(retrievedHotel);
                Assert.AreEqual(hotelId, retrievedHotel[0].Id);
                Assert.AreEqual(title, retrievedHotel[0].Title);
                Assert.AreEqual(name, retrievedHotel[0].OwnerName);
                Assert.AreEqual(phoneNumber, retrievedHotel[0].OwnerPhoneNumber);
                Assert.AreEqual(propertyType, retrievedHotel[0].PropertyType);
                Assert.AreEqual(genderRestriction.ToString(), retrievedHotel[0].GenderRestriction);
                Assert.AreEqual(area, retrievedHotel[0].Area);
                Assert.AreEqual(monthlyRent, retrievedHotel[0].RentPrice);
                Assert.AreEqual(rentUnit, retrievedHotel[0].RentUnit);
                Assert.AreEqual(isShared, retrievedHotel[0].IsShared);
                Assert.AreEqual(ac, retrievedHotel[0].AC);
                Assert.AreEqual(geyser, retrievedHotel[0].Geyser);
                Assert.AreEqual(attachedBathroom, retrievedHotel[0].AttachedBathroom);
                Assert.AreEqual(fitnessCentre, retrievedHotel[0].FitnessCentre);
                Assert.AreEqual(backupElectricity, retrievedHotel[0].BackupElectricity);
                Assert.AreEqual(heating, retrievedHotel[0].Heating);

                Assert.AreEqual(airportShuttle, retrievedHotel[0].AirportShuttle);
                Assert.AreEqual(breakfastIncluded, retrievedHotel[0].BreakfastIncluded);

                Assert.AreEqual(numberOfAdults, retrievedHotel[0].Occupants.Adults);
                Assert.AreEqual(numberOfChildren, retrievedHotel[0].Occupants.Children);
                Assert.AreEqual(numberOfAdults + numberOfChildren, retrievedHotel[0].Occupants.TotalOccupants);
            }
        }

        [Test]
        public void GetGuestHousesByLocationAndPropertyTypeSingleInstanceEvaluationInstanceByIdTest_TestsThatGuestHouseIsRetrievedByLocationAndPropertyTypeAndAllFieldsAreAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);
                string title = "Title No 1";
                string description = "Description of house";
                string email = "w@12344321.com";
                string name = "OwnerName";
                string phoneNumber = "03455138018";
                string propertyType = Constants.GuestHouse;
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
                    kitchen, 0, 0, new Occupants(numberOfAdults, numberOfChildren));

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, email)
                    })
                });

                var postHttpActionResult = houseController.Post(JsonConvert.SerializeObject(createNewHotelCommand));
                string hotelId = ((OkNegotiatedContentResult<string>)postHttpActionResult).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hotelId));

                var retrievedHostelHttpResult = houseController.Get(area: area, propertyType: propertyType);
                dynamic retrievedHotel = ((OkNegotiatedContentResult<IList<ResidentialPropertyPartialBaseImplementation>>)retrievedHostelHttpResult).Content;
                Assert.IsNotNull(retrievedHotel);

                Assert.IsNotNull(retrievedHotel);
                Assert.AreEqual(hotelId, retrievedHotel[0].Id);
                Assert.AreEqual(title, retrievedHotel[0].Title);
                Assert.AreEqual(name, retrievedHotel[0].OwnerName);
                Assert.AreEqual(phoneNumber, retrievedHotel[0].OwnerPhoneNumber);
                Assert.AreEqual(propertyType, retrievedHotel[0].PropertyType);
                Assert.AreEqual(genderRestriction.ToString(), retrievedHotel[0].GenderRestriction);
                Assert.AreEqual(area, retrievedHotel[0].Area);
                Assert.AreEqual(monthlyRent, retrievedHotel[0].RentPrice);
                Assert.AreEqual(rentUnit, retrievedHotel[0].RentUnit);
                Assert.AreEqual(isShared, retrievedHotel[0].IsShared);
                Assert.AreEqual(ac, retrievedHotel[0].AC);
                Assert.AreEqual(geyser, retrievedHotel[0].Geyser);
                Assert.AreEqual(attachedBathroom, retrievedHotel[0].AttachedBathroom);
                Assert.AreEqual(fitnessCentre, retrievedHotel[0].FitnessCentre);
                Assert.AreEqual(backupElectricity, retrievedHotel[0].BackupElectricity);
                Assert.AreEqual(heating, retrievedHotel[0].Heating);

                Assert.AreEqual(airportShuttle, retrievedHotel[0].AirportShuttle);
                Assert.AreEqual(breakfastIncluded, retrievedHotel[0].BreakfastIncluded);

                Assert.AreEqual(numberOfAdults, retrievedHotel[0].Occupants.Adults);
                Assert.AreEqual(numberOfChildren, retrievedHotel[0].Occupants.Children);
                Assert.AreEqual(numberOfAdults + numberOfChildren, retrievedHotel[0].Occupants.TotalOccupants);
            }
        }

        #endregion GetByPropertyType: Single Partial Representation Evaluation Tests

        #region Get By PropertyType and Location: Single Partial Representation Evaluation Tests

        [Category("Integration")]
        [Test]
        public void GetHousesByEmail_TestsThatHouseIsRetrievedByPropertyTypeAndEmailAndAllFieldsInPartialRepresentationAreAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);

                string title = "House For Rent";
                string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
                int rent = 105000;
                string ownerEmail = "thorin@oakenshield123.com";
                string ownerPhoneNumber = "01234567890";
                string houseNo = "CT-141/A";
                string streetNo = "14";
                int numberOfBathrooms = 1;
                int numberOfBedrooms = 1;
                int numberOfKitchens = 1;
                bool familiesOnly = false;
                bool boysOnly = false;
                bool girlsOnly = true;
                bool internetAvailable = true;
                bool landlinePhoneAvailable = true;
                bool cableTvAvailable = true;
                bool garageAvailable = true;
                bool smokingAllowed = true;
                string propertyType = Constants.House;
                string area = "Pindora, Rawalpindi, Pakistan";
                //string area = "Pindora, Rawalpindi, Pakistan";
                string dimensionType = DimensionType.Kanal.ToString();
                string dimensionString = "5";
                decimal dimensionDecimal = 0;
                string ownerName = "Owner Name 1";
                string genderRestriction = GenderRestriction.FamiliesOnly.ToString();
                bool isShared = true;
                string rentUnit = House.GetAllRentUnits()[0];
                string landlineNumber = "0510000000";
                string fax = "0510000000";

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail)
                    })
                });
                CreateHouseCommand house = new CreateHouseCommand(title, rent, numberOfBedrooms, numberOfKitchens,
                    numberOfBathrooms, internetAvailable, landlinePhoneAvailable,
                    cableTvAvailable, garageAvailable, smokingAllowed, propertyType, ownerEmail, ownerPhoneNumber,
                    houseNo, streetNo, area, dimensionType, dimensionString, dimensionDecimal, ownerName, description, genderRestriction,
                    isShared, rentUnit, landlineNumber, fax);
                IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(house));
                string houseId = ((OkNegotiatedContentResult<string>)houseSaveResult).Content;

                IHttpActionResult response = (IHttpActionResult)houseController.Get(email: ownerEmail, propertyType: propertyType);
                dynamic retreivedHouse = ((OkNegotiatedContentResult<IList<ResidentialPropertyPartialBaseImplementation>>)response).Content;
                Assert.NotNull(retreivedHouse);
                Assert.AreEqual(houseId, retreivedHouse[0].Id);
                Assert.AreEqual(title, retreivedHouse[0].Title);
                Assert.AreEqual(ownerPhoneNumber, retreivedHouse[0].OwnerPhoneNumber);
                Assert.AreEqual(rent, retreivedHouse[0].RentPrice);
                Assert.AreEqual(numberOfBathrooms, retreivedHouse[0].NumberOfBathrooms);
                Assert.AreEqual(numberOfBedrooms, retreivedHouse[0].NumberOfBedrooms);
                Assert.AreEqual(numberOfKitchens, retreivedHouse[0].NumberOfKitchens);
                Assert.AreEqual(propertyType, retreivedHouse[0].PropertyType);
                Assert.AreEqual(isShared, retreivedHouse[0].IsShared);
                Assert.AreEqual(area, retreivedHouse[0].Area);
                Assert.AreEqual(dimensionString + " " + dimensionType, retreivedHouse[0].Dimension);
                Assert.AreEqual(ownerName, retreivedHouse[0].OwnerName);
                Assert.AreEqual(genderRestriction, retreivedHouse[0].GenderRestriction);
                Assert.AreEqual(rentUnit, retreivedHouse[0].RentUnit);
            }
        }

        [Category("Integration")]
        [Test]
        public void GetApartmentsByEmail_TestsThatApartmentIsRetrievedByPropertyTypeAndEmailAndAllFieldsInPartialRepresentationAreAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);

                string title = "House For Rent";
                string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
                int rent = 105000;
                string ownerEmail = "thorin@oakenshield123.com";
                string ownerPhoneNumber = "01234567890";
                string houseNo = "CT-141/A";
                string streetNo = "14";
                int numberOfBathrooms = 1;
                int numberOfBedrooms = 1;
                int numberOfKitchens = 1;
                bool familiesOnly = false;
                bool boysOnly = false;
                bool girlsOnly = true;
                bool internetAvailable = true;
                bool landlinePhoneAvailable = true;
                bool cableTvAvailable = true;
                bool garageAvailable = true;
                bool smokingAllowed = true;
                string propertyType = Constants.Apartment;
                string area = "Pindora, Rawalpindi, Pakistan";
                //string area = "Pindora, Rawalpindi, Pakistan";
                string dimensionType = DimensionType.Kanal.ToString();
                string dimensionString = "5";
                decimal dimensionDecimal = 0;
                string ownerName = "Owner Name 1";
                string genderRestriction = GenderRestriction.FamiliesOnly.ToString();
                bool isShared = true;
                string rentUnit = House.GetAllRentUnits()[0];
                string landlineNumber = "0510000000";
                string fax = "0510000000";

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail)
                    })
                });
                CreateHouseCommand house = new CreateHouseCommand(title, rent, numberOfBedrooms, numberOfKitchens,
                    numberOfBathrooms, internetAvailable, landlinePhoneAvailable,
                    cableTvAvailable, garageAvailable, smokingAllowed, propertyType, ownerEmail, ownerPhoneNumber,
                    houseNo, streetNo, area, dimensionType, dimensionString, dimensionDecimal, ownerName, description, genderRestriction,
                    isShared, rentUnit, landlineNumber, fax);
                IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(house));
                string houseId = ((OkNegotiatedContentResult<string>)houseSaveResult).Content;

                IHttpActionResult response = (IHttpActionResult)houseController.Get(email: ownerEmail, propertyType: propertyType);
                dynamic retreivedHouse = ((OkNegotiatedContentResult<IList<ResidentialPropertyPartialBaseImplementation>>)response).Content;
                Assert.NotNull(retreivedHouse);
                Assert.AreEqual(houseId, retreivedHouse[0].Id);
                Assert.AreEqual(title, retreivedHouse[0].Title);
                Assert.AreEqual(ownerPhoneNumber, retreivedHouse[0].OwnerPhoneNumber);
                Assert.AreEqual(rent, retreivedHouse[0].RentPrice);
                Assert.AreEqual(numberOfBathrooms, retreivedHouse[0].NumberOfBathrooms);
                Assert.AreEqual(numberOfBedrooms, retreivedHouse[0].NumberOfBedrooms);
                Assert.AreEqual(numberOfKitchens, retreivedHouse[0].NumberOfKitchens);
                Assert.AreEqual(propertyType, retreivedHouse[0].PropertyType);
                Assert.AreEqual(isShared, retreivedHouse[0].IsShared);
                Assert.AreEqual(area, retreivedHouse[0].Area);
                Assert.AreEqual(dimensionString + " " + dimensionType, retreivedHouse[0].Dimension);
                Assert.AreEqual(ownerName, retreivedHouse[0].OwnerName);
                Assert.AreEqual(genderRestriction, retreivedHouse[0].GenderRestriction);
                Assert.AreEqual(rentUnit, retreivedHouse[0].RentUnit);
            }
        }

        [Category("Integration")]
        [Test]
        public void GetHostelByEmail_TestsThatHostelIsRetrievedByPropertyTypeAndEmailAndAllFieldsInPartialRepresentationAreAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);

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

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, email)
                    })
                });

                var postHttpActionResult = houseController.Post(JsonConvert.SerializeObject(createNewHostelCommand));
                string hostelId = ((OkNegotiatedContentResult<string>)postHttpActionResult).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hostelId));

                var retrievedHostelHttpResult = houseController.Get(email: email, propertyType: propertyType);
                dynamic retrievedHostel = ((OkNegotiatedContentResult<IList<ResidentialPropertyPartialBaseImplementation>>)retrievedHostelHttpResult).Content;
                Assert.IsNotNull(retrievedHostel);

                Assert.AreEqual(hostelId, retrievedHostel[0].Id);
                Assert.AreEqual(title, retrievedHostel[0].Title);
                Assert.AreEqual(name, retrievedHostel[0].OwnerName);
                Assert.AreEqual(phoneNumber, retrievedHostel[0].OwnerPhoneNumber);
                Assert.AreEqual(propertyType, retrievedHostel[0].PropertyType);
                Assert.AreEqual(genderRestriction.ToString(), retrievedHostel[0].GenderRestriction);
                Assert.AreEqual(area, retrievedHostel[0].Area);
                Assert.AreEqual(monthlyRent, retrievedHostel[0].RentPrice);
                Assert.AreEqual(isShared, retrievedHostel[0].IsShared);
                Assert.AreEqual(rentUnit, retrievedHostel[0].RentUnit);
                Assert.AreEqual(laundry, retrievedHostel[0].Laundry);
                Assert.AreEqual(ac, retrievedHostel[0].AC);
                Assert.AreEqual(geyser, retrievedHostel[0].Geyser);
                Assert.AreEqual(attachedBathroom, retrievedHostel[0].AttachedBathroom);
                Assert.AreEqual(backupElectricity, retrievedHostel[0].BackupElectricity);
                Assert.AreEqual(meals, retrievedHostel[0].Meals);
                Assert.AreEqual(numberOfSeats, retrievedHostel[0].NumberOfSeats);
            }
        }

        [Category("Integration")]
        [Test]
        public void GetHotelByEmail_TestsThatHotelIsRetrievedByPropertyTypeAndEmailAndAllFieldsInPartialRepresentationAreAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);
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
                    kitchen, 0, 0, new Occupants(numberOfAdults, numberOfChildren));

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, email)
                    })
                });

                var postHttpActionResult = houseController.Post(JsonConvert.SerializeObject(createNewHotelCommand));
                string hotelId = ((OkNegotiatedContentResult<string>)postHttpActionResult).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hotelId));

                var retrievedHostelHttpResult = houseController.Get(email: email, propertyType: propertyType);
                dynamic retrievedHotel = ((OkNegotiatedContentResult<IList<ResidentialPropertyPartialBaseImplementation>>)retrievedHostelHttpResult).Content;
                Assert.IsNotNull(retrievedHotel);

                Assert.IsNotNull(retrievedHotel);
                Assert.AreEqual(hotelId, retrievedHotel[0].Id);
                Assert.AreEqual(title, retrievedHotel[0].Title);
                Assert.AreEqual(name, retrievedHotel[0].OwnerName);
                Assert.AreEqual(phoneNumber, retrievedHotel[0].OwnerPhoneNumber);
                Assert.AreEqual(propertyType, retrievedHotel[0].PropertyType);
                Assert.AreEqual(genderRestriction.ToString(), retrievedHotel[0].GenderRestriction);
                Assert.AreEqual(area, retrievedHotel[0].Area);
                Assert.AreEqual(monthlyRent, retrievedHotel[0].RentPrice);
                Assert.AreEqual(rentUnit, retrievedHotel[0].RentUnit);
                Assert.AreEqual(isShared, retrievedHotel[0].IsShared);
                Assert.AreEqual(ac, retrievedHotel[0].AC);
                Assert.AreEqual(geyser, retrievedHotel[0].Geyser);
                Assert.AreEqual(attachedBathroom, retrievedHotel[0].AttachedBathroom);
                Assert.AreEqual(fitnessCentre, retrievedHotel[0].FitnessCentre);
                Assert.AreEqual(backupElectricity, retrievedHotel[0].BackupElectricity);
                Assert.AreEqual(heating, retrievedHotel[0].Heating);

                Assert.AreEqual(airportShuttle, retrievedHotel[0].AirportShuttle);
                Assert.AreEqual(breakfastIncluded, retrievedHotel[0].BreakfastIncluded);

                Assert.AreEqual(numberOfAdults, retrievedHotel[0].Occupants.Adults);
                Assert.AreEqual(numberOfChildren, retrievedHotel[0].Occupants.Children);
                Assert.AreEqual(numberOfAdults + numberOfChildren, retrievedHotel[0].Occupants.TotalOccupants);
            }
        }

        [Category("Integration")]
        [Test]
        public void GetGuestHouseByEmail_TestsThatGuestHouseIsRetrievedByPropertyTypeAndEmailAndAllFieldsInPartialRepresentationAreAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                PropertyController houseController = _kernel.Get<PropertyController>();
                Assert.NotNull(houseController);
                string title = "Title No 1";
                string description = "Description of house";
                string email = "w@12344321.com";
                string name = "OwnerName";
                string phoneNumber = "03455138018";
                string propertyType = Constants.GuestHouse;
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
                    kitchen, 0, 0, new Occupants(numberOfAdults, numberOfChildren));

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, email)
                    })
                });

                var postHttpActionResult = houseController.Post(JsonConvert.SerializeObject(createNewHotelCommand));
                string hotelId = ((OkNegotiatedContentResult<string>)postHttpActionResult).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(hotelId));

                var retrievedHostelHttpResult = houseController.Get(email: email, propertyType: propertyType);
                dynamic retrievedHotel = ((OkNegotiatedContentResult<IList<ResidentialPropertyPartialBaseImplementation>>)retrievedHostelHttpResult).Content;
                Assert.IsNotNull(retrievedHotel);

                Assert.IsNotNull(retrievedHotel);
                Assert.AreEqual(hotelId, retrievedHotel[0].Id);
                Assert.AreEqual(title, retrievedHotel[0].Title);
                Assert.AreEqual(name, retrievedHotel[0].OwnerName);
                Assert.AreEqual(phoneNumber, retrievedHotel[0].OwnerPhoneNumber);
                Assert.AreEqual(propertyType, retrievedHotel[0].PropertyType);
                Assert.AreEqual(genderRestriction.ToString(), retrievedHotel[0].GenderRestriction);
                Assert.AreEqual(area, retrievedHotel[0].Area);
                Assert.AreEqual(monthlyRent, retrievedHotel[0].RentPrice);
                Assert.AreEqual(rentUnit, retrievedHotel[0].RentUnit);
                Assert.AreEqual(isShared, retrievedHotel[0].IsShared);
                Assert.AreEqual(ac, retrievedHotel[0].AC);
                Assert.AreEqual(geyser, retrievedHotel[0].Geyser);
                Assert.AreEqual(attachedBathroom, retrievedHotel[0].AttachedBathroom);
                Assert.AreEqual(fitnessCentre, retrievedHotel[0].FitnessCentre);
                Assert.AreEqual(backupElectricity, retrievedHotel[0].BackupElectricity);
                Assert.AreEqual(heating, retrievedHotel[0].Heating);

                Assert.AreEqual(airportShuttle, retrievedHotel[0].AirportShuttle);
                Assert.AreEqual(breakfastIncluded, retrievedHotel[0].BreakfastIncluded);

                Assert.AreEqual(numberOfAdults, retrievedHotel[0].Occupants.Adults);
                Assert.AreEqual(numberOfChildren, retrievedHotel[0].Occupants.Children);
                Assert.AreEqual(numberOfAdults + numberOfChildren, retrievedHotel[0].Occupants.TotalOccupants);
            }
        }

        #endregion Get By Email & PropertyType: Single Partial Representation Evaluation Tests

        #region Get All Tests for Types and Units

        [Test]
        public void GetAllRentUnitTest_ChecksThatAllRentUnitsAreReturnedProperly_VerifiesThroughReturnedValue()
        {
            PropertyController houseController = _kernel.Get<PropertyController>();
            var httpActionResult = houseController.GetRentUnits();
            var allRentUnits = ((OkNegotiatedContentResult<IList<string>>) httpActionResult).Content;
            Assert.Greater(allRentUnits.Count, 0);

            for (int i = 0; i < allRentUnits.Count - 1; i++)
            {
                Assert.AreEqual(House.GetAllRentUnits()[i], allRentUnits[i]);
            }
        }

        [Test]
        public void GetAllPropertyTypesTest_ChecksThatAllPropertyTypesAreReturnedProperly_VerifiesThroughReturnedValue()
        {
            PropertyController houseController = _kernel.Get<PropertyController>();
            var httpActionResult = houseController.GetPropertyTypes();
            var allPropertyTypes = ((OkNegotiatedContentResult<IList<string>>)httpActionResult).Content;
            Assert.Greater(allPropertyTypes.Count, 0);

            for (int i = 0; i < allPropertyTypes.Count - 1; i++)
            {
                Assert.AreEqual(House.GetAllPropertyTypes()[i], allPropertyTypes[i]);
            }
        }

        #endregion Get All Tests for Types and Units

        #region Delete Tests

        [Category("Integration")]
        [Test]
        public void DeleteHouseTest_ChecksThatAHouseIsDeletedAsExpected_VerifiesByDatabaseRetrieval()
        {
            PropertyController houseController = _kernel.Get<PropertyController>();
            Assert.NotNull(houseController);
            
            string ownerEmail = "thorin@oakenshield123.com";
            string ownerPhoneNumber = "01234567890";
            string propertyType = Constants.House;
            string area = "Pindora, Rawalpindi, Pakistan";

            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
            houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail)
                    })
                });
            CreateHouseCommand house = new CreateHouseCommand("Title", 45000, 1, 1, 1, false, false, false,
                false, false, propertyType, ownerEmail, ownerPhoneNumber, "", "", area, DimensionType.Acre.ToString(), "2",
                1, "None", "", GenderRestriction.GirlsOnly.ToString(), false, Constants.Hourly, "", "");
            IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(house));
            string houseId = ((OkNegotiatedContentResult<string>)houseSaveResult).Content;

            IHttpActionResult response = (IHttpActionResult)houseController.Get(houseId: houseId, propertyType: propertyType);
            dynamic retreivedHouse = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)response).Content;
            Assert.NotNull(retreivedHouse);
            Assert.AreEqual(houseId, retreivedHouse.Id);

            houseController.Delete(houseId);
            response = (IHttpActionResult)houseController.Get(houseId: houseId, propertyType: propertyType);
            retreivedHouse = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)response).Content;
            Assert.IsNull(retreivedHouse);
        }

        [Category("Integration")]
        [Test]
        public void DeleteApartmentTest_ChecksThatAnApartmentIsDeletedAsExpected_VerifiesByDatabaseRetrieval()
        {
            PropertyController houseController = _kernel.Get<PropertyController>();
            Assert.NotNull(houseController);

            string ownerEmail = "thorin@oakenshield123.com";
            string ownerPhoneNumber = "01234567890";
            string propertyType = Constants.Apartment;
            string area = "Pindora, Rawalpindi, Pakistan";

            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
            houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail)
                    })
                });
            CreateHouseCommand house = new CreateHouseCommand("Title", 45000, 1, 1, 1, false, false, false,
                false, false, propertyType, ownerEmail, ownerPhoneNumber, "", "", area, DimensionType.Acre.ToString(), "2",
                1, "None", "", GenderRestriction.GirlsOnly.ToString(), false, Constants.Hourly, "", "");
            IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(house));
            string apartmentId = ((OkNegotiatedContentResult<string>)houseSaveResult).Content;

            IHttpActionResult response = (IHttpActionResult)houseController.Get(houseId: apartmentId, propertyType: propertyType);
            dynamic retreivedHouse = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)response).Content;
            Assert.NotNull(retreivedHouse);
            Assert.AreEqual(apartmentId, retreivedHouse.Id);

            houseController.Delete(apartmentId);
            response = (IHttpActionResult)houseController.Get(houseId: apartmentId, propertyType: propertyType);
            retreivedHouse = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)response).Content;
            Assert.IsNull(retreivedHouse);
        }

        [Category("Integration")]
        [Test]
        public void DeleteHostelTest_ChecksThatAHostelIsDeletedAsExpected_VerifiesByDatabaseRetrieval()
        {
            PropertyController houseController = _kernel.Get<PropertyController>();
            Assert.NotNull(houseController);

            string ownerEmail = "thorin@oakenshield123.com";
            string ownerPhoneNumber = "01234567890";
            string propertyType = Constants.Hostel;
            string area = "Pindora, Rawalpindi, Pakistan";

            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
            houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail)
                    })
                });
            CreateHostelCommand hostel = new CreateHostelCommand("Title", 45000, false, false, false, propertyType,
                ownerEmail, ownerPhoneNumber, area, "None", "", GenderRestriction.GirlsOnly.ToString(), false,
                Constants.Hourly, false, false, false, false, false, false, false, false, false, false, false,
                "", "", false, false, false, 2);
            IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(hostel));
            string hostelId = ((OkNegotiatedContentResult<string>)houseSaveResult).Content;

            IHttpActionResult response = (IHttpActionResult)houseController.Get(houseId: hostelId, propertyType: propertyType);
            dynamic retreivedHouse = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)response).Content;
            Assert.NotNull(retreivedHouse);
            Assert.AreEqual(hostelId, retreivedHouse.Id);

            houseController.Delete(hostelId);
            response = (IHttpActionResult)houseController.Get(houseId: hostelId, propertyType: propertyType);
            retreivedHouse = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)response).Content;
            Assert.IsNull(retreivedHouse);
        }

        [Category("Integration")]
        [Test]
        public void DeleteHotelTest_ChecksThatAHotelIsDeletedAsExpected_VerifiesByDatabaseRetrieval()
        {
            PropertyController houseController = _kernel.Get<PropertyController>();
            Assert.NotNull(houseController);

            string ownerEmail = "thorin@oakenshield123.com";
            string ownerPhoneNumber = "01234567890";
            string propertyType = Constants.Hotel;
            string area = "Pindora, Rawalpindi, Pakistan";

            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
            houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail)
                    })
                });
            CreateHotelCommand hotel = new CreateHotelCommand("Title", 34000, false, false, false, propertyType, 
                ownerEmail, ownerPhoneNumber, area, "None", "", GenderRestriction.FamiliesOnly.ToString(),
                false, Constants.Daily, false, false, false, false, false, true, true, false, false, false, false,
                "", "", true, true, true, true, true, true, true, true, true, true, true, 0, 0, new Occupants(2, 0));
            IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(hotel));
            string hotelId = ((OkNegotiatedContentResult<string>)houseSaveResult).Content;

            IHttpActionResult response = (IHttpActionResult)houseController.Get(houseId: hotelId, propertyType: propertyType);
            dynamic retreivedHouse = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)response).Content;
            Assert.NotNull(retreivedHouse);
            Assert.AreEqual(hotelId, retreivedHouse.Id);

            houseController.Delete(hotelId);
            response = (IHttpActionResult)houseController.Get(houseId: hotelId, propertyType: propertyType);
            retreivedHouse = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)response).Content;
            Assert.IsNull(retreivedHouse);
        }


        [Category("Integration")]
        [Test]
        public void DeleteGuestHouseTest_ChecksThatAGuestHouseIsDeletedAsExpected_VerifiesByDatabaseRetrieval()
        {
            PropertyController houseController = _kernel.Get<PropertyController>();
            Assert.NotNull(houseController);

            string ownerEmail = "thorin@oakenshield123.com";
            string ownerPhoneNumber = "01234567890";
            string propertyType = Constants.GuestHouse;
            string area = "Pindora, Rawalpindi, Pakistan";

            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
            houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail)
                    })
                });
            CreateHotelCommand hotel = new CreateHotelCommand("Title", 34000, false, false, false, propertyType,
                ownerEmail, ownerPhoneNumber, area, "None", "", GenderRestriction.FamiliesOnly.ToString(),
                false, Constants.Daily, false, false, false, false, false, true, true, false, false, false, false,
                "", "", true, true, true, true, true, true, true, true, true, true, true, 0, 0, new Occupants(2, 0));
            IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(hotel));
            string hotelId = ((OkNegotiatedContentResult<string>)houseSaveResult).Content;

            IHttpActionResult response = (IHttpActionResult)houseController.Get(houseId: hotelId, propertyType: propertyType);
            dynamic retreivedHouse = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)response).Content;
            Assert.NotNull(retreivedHouse);
            Assert.AreEqual(hotelId, retreivedHouse.Id);

            houseController.Delete(hotelId);
            response = (IHttpActionResult)houseController.Get(houseId: hotelId, propertyType: propertyType);
            retreivedHouse = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)response).Content;
            Assert.IsNull(retreivedHouse);
        }

        #endregion Delete Tests
    }
}
