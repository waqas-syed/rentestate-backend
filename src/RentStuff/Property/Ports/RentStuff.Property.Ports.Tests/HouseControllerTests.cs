﻿using Newtonsoft.Json;
using Ninject;
using NUnit.Framework;
using RentStuff.Common.Ninject.Modules;
using RentStuff.Common.Utilities;
using RentStuff.Property.Application.Ninject.Modules;
using RentStuff.Property.Application.PropertyServices.Commands.CreateCommands;
using RentStuff.Property.Application.PropertyServices.Commands.UpdateCommands;
using RentStuff.Property.Application.PropertyServices.Representation;
using RentStuff.Property.Application.PropertyServices.Representation.FullRepresentations;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Domain.Model.PropertyAggregate;
using RentStuff.Property.Infrastructure.Persistence.Ninject.Modules;
using RentStuff.Property.Ports.Adapter.Rest.Ninject.Modules;
using RentStuff.Property.Ports.Adapter.Rest.Resources;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Results;
using RentStuff.Property.Application.PropertyServices.Representation.AbstractRepresentations;

namespace RentStuff.Property.Ports.Tests
{
    [TestFixture]
    public class HouseControllerTests
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

        // Save, Update & Get House by Id
        [Category("Integration")]
        [Test]
        public void SaveUpdateAndGetHouseInstanceByIdTest_TestsThatHouseIsSavedThenUpdatedAndRetreivedAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                HouseController houseController = _kernel.Get<HouseController>();
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
                string propertyType = "Apartment";
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

                IHttpActionResult response = (IHttpActionResult)houseController.GetHouse(houseId: houseId, propertyType: propertyType);
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
                HouseController houseController = _kernel.Get<HouseController>();
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

                var retrievedHostelHttpResult = houseController.GetHouse(houseId: hostelId, propertyType: propertyType);
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

        [Test]
        public void SaveNewHotelTest_ChecksThatANewHotelIsSavedAndRetrievedAsExpected_VerifiesByTheReturnValue()
        {
            if (_runTests)
            {
                HouseController houseController = _kernel.Get<HouseController>();
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
                    kitchen, numberOfAdults, numberOfChildren);

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

                var retrievedHostelHttpResult = houseController.GetHouse(houseId: hotelId, propertyType: propertyType);
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
                HouseController houseController = _kernel.Get<HouseController>();
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
                string propertyType = "Apartment";
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

                IHttpActionResult response = (IHttpActionResult)houseController.GetHouse(houseId: houseId, propertyType: propertyType);
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
                response = (IHttpActionResult)houseController.GetHouse(houseId: houseId, propertyType: updatedPropertyType);
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
                HouseController houseController = _kernel.Get<HouseController>();
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
                
                var retrievedHostelHttpResult = houseController.GetHouse(houseId: hostelId, propertyType: propertyType);
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
                var response = (IHttpActionResult)houseController.GetHouse(houseId: hostelId, propertyType: updatedPropertyType);
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
                HouseController houseController = _kernel.Get<HouseController>();
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
                    kitchen, numberOfAdults, numberOfChildren);

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

                var retrievedHostelHttpResult = houseController.GetHouse(houseId: hotelId, propertyType: propertyType);
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

                houseController.Put(JsonConvert.SerializeObject(updateHotelCommand));
                var response = (IHttpActionResult)houseController.GetHouse(houseId: hotelId, propertyType: updatedPropertyType);
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

        [Test]
        public void SaveAndGetAllHousesTest_TestsThatHousesAreSavedAndRetreivedAsExpected_VerfiesThroughInstanceValue()
        {
            // Save 2 properties for each property type, at distant locations from each other. 
            // Then search for one of those locations for each property type and verify that the search result
            // contains only one of the properties that are located within the searched location
            if (_runTests)
            {
                HouseController houseController = _kernel.Get<HouseController>();
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
                    "", "", true, true, true, true, true, true, true, true, true, true, true, 2, 0);
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
                    "", "", true, true, true, true, true, true, true, true, true, true, true, 2, 0);
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
                    "", "", true, true, true, true, true, true, true, true, true, true, true, 2, 0);
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
                    "", "", true, true, true, true, true, true, true, true, true, true, true, 2, 0);
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
                IHttpActionResult response = (IHttpActionResult)houseController.GetHouse(
                    area: searchLocation1, propertyType: Constants.House);
                dynamic retreivedHouses = ((OkNegotiatedContentResult
                    <IList<ResidentialPropertyPartialBaseImplementation>>)response).Content;
                Assert.NotNull(retreivedHouses);
                Assert.AreEqual(1, retreivedHouses.Count);
                Assert.AreEqual(houseId2, retreivedHouses[0].Id);
                Assert.AreEqual(house2.Title, retreivedHouses[0].Title);
                Assert.AreEqual(Constants.House, retreivedHouses[0].PropertyType);

                // ###### When Apartment is searched ######
                IHttpActionResult apartmentResponse = (IHttpActionResult)houseController.GetHouse(
                    area: searchLocation2, propertyType: Constants.Apartment);
                dynamic retreivedApartments = ((OkNegotiatedContentResult
                    <IList<ResidentialPropertyPartialBaseImplementation>>)apartmentResponse).Content;
                Assert.NotNull(retreivedApartments);
                Assert.AreEqual(1, retreivedApartments.Count);
                Assert.AreEqual(apartmentId10, retreivedApartments[0].Id);
                Assert.AreEqual(apartment10.Title, retreivedApartments[0].Title);
                Assert.AreEqual(Constants.Apartment, retreivedApartments[0].PropertyType);

                // ###### When Hotel is searched ######
                IHttpActionResult hotelResponse = (IHttpActionResult)houseController.GetHouse(
                    area: searchLocation1, propertyType: Constants.Hotel);
                dynamic retreivedHotels = ((OkNegotiatedContentResult
                    <IList<ResidentialPropertyPartialBaseImplementation>>)hotelResponse).Content;
                Assert.NotNull(retreivedHotels);
                Assert.AreEqual(1, retreivedHotels.Count);
                Assert.AreEqual(hotelId5, retreivedHotels[0].Id);
                Assert.AreEqual(hotel5.Title, retreivedHotels[0].Title);
                Assert.AreEqual(Constants.Hotel, retreivedHotels[0].PropertyType);

                // ###### When Hostel is searched ######
                IHttpActionResult hostelResponse = (IHttpActionResult)houseController.GetHouse(
                    area: searchLocation1, propertyType: Constants.Hostel);
                dynamic retreivedHostels = ((OkNegotiatedContentResult
                    <IList<ResidentialPropertyPartialBaseImplementation>>)hostelResponse).Content;
                Assert.NotNull(retreivedHostels);
                Assert.AreEqual(1, retreivedHostels.Count);
                Assert.AreEqual(hostelId7, retreivedHostels[0].Id);
                Assert.AreEqual(hostel7.Title, retreivedHostels[0].Title);
                Assert.AreEqual(Constants.Hostel, retreivedHostels[0].PropertyType);

                // ###### When Guest House is searched ######
                IHttpActionResult guestHouseResponse = (IHttpActionResult)houseController.GetHouse(
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
        public void
            SearchHousesByPropertyTypeOnly_TestsThatHouseIsSavedAndRetreivedAsExpected_VerfiesThroughInstanceValue()
        {
            HouseController houseController = _kernel.Get<HouseController>();
            Assert.NotNull(houseController);

            // Saving House # 1: Should appear in search results with PropertyType = Apartment
            string title = "Title # 1";
            string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
            int rent = 100001;
            string ownerEmail = "house@1234567-1.com";
            string ownerPhoneNumber = "01234567890";
            string houseNo = "House # 1";
            string streetNo = "1";
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
            string propertyType = "Apartment";
            string area = "Pindora, Rawalpindi, Pakistan";
            string dimensionType = DimensionType.Kanal.ToString();
            string dimensionString = "1";
            decimal dimensionDecimal = 0;
            string ownerName = "Owner Name 1";
            string genderRestriction = GenderRestriction.FamiliesOnly.ToString();
            bool isShared = true;
            string rentUnit = House.GetAllRentUnits()[1];

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
                isShared, rentUnit, null, null);
            IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(house));
            string houseId = ((OkNegotiatedContentResult<string>) houseSaveResult).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseId));

            // Saving House # 2 - SET 1: Should NOT appear in search results with PropertyType = House
            string title2 = "Title # 2";
            int rent2 = 100002;
            string ownerEmail2 = "house@1234567-2.com";
            string description2 =
                "Erebor 2. Built deep within the mountain itself the beauty of this fortress was legend.";
            string ownerPhoneNumber2 = "01234567892";
            string houseNo2 = "House # 2";
            string streetNo2 = "2";
            int numberOfBathrooms2 = 2;
            int numberOfBedrooms2 = 2;
            int numberOfKitchens2 = 2;
            bool familiesOnly2 = false;
            bool boysOnly2 = false;
            bool girlsOnly2 = true;
            bool internetAvailable2 = true;
            bool landlinePhoneAvailable2 = true;
            bool cableTvAvailable2 = true;
            bool garageAvailable2 = true;
            bool smokingAllowed2 = true;
            string propertyType2 = "House";
            string area2 = "I-9, Islamabad, Pakistan";
            string dimensionType2 = DimensionType.Kanal.ToString();
            string dimensionString2 = "2";
            decimal dimensionDecimal2 = 0;
            string ownerName2 = "Owner Name 2";
            string genderRestriction2 = GenderRestriction.FamiliesOnly.ToString();
            bool isShared2 = true;
            string rentUnit2 = House.GetAllRentUnits()[0];

            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
            houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, ownerEmail2)
                })
            });
            CreateHouseCommand house2 = new CreateHouseCommand(title2, rent2, numberOfBedrooms2, numberOfKitchens2,
                numberOfBathrooms2, internetAvailable2, landlinePhoneAvailable2,
                cableTvAvailable2, garageAvailable2, smokingAllowed2, propertyType2, ownerEmail2, ownerPhoneNumber2,
                houseNo2, streetNo2, area2, dimensionType2, dimensionString2, dimensionDecimal2, ownerName2,
                description2, genderRestriction2, isShared2, rentUnit2, null, null);
            IHttpActionResult houseSaveResult2 = houseController.Post(JsonConvert.SerializeObject(house2));
            string houseId2 = ((OkNegotiatedContentResult<string>) houseSaveResult2).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseId2));

            // Saving House # 3: Should appear in search results with PropertyType = Apartment
            string title3 = "Title # 3";
            string description3 =
                "Erebor 3. Built deep within the mountain itself the beauty of this fortress was legend.";
            int rent3 = 100003;
            string ownerEmail3 = "house@1234567-3.com";
            string ownerPhoneNumber3 = "01234567893";
            string houseNo3 = "House # 3";
            string streetNo3 = "3";
            int numberOfBathrooms3 = 3;
            int numberOfBedrooms3 = 3;
            int numberOfKitchens3 = 3;
            bool familiesOnly3 = false;
            bool boysOnly3 = false;
            bool girlsOnly3 = true;
            bool internetAvailable3 = true;
            bool landlinePhoneAvailable3 = true;
            bool cableTvAvailable3 = true;
            bool garageAvailable3 = true;
            bool smokingAllowed3 = true;
            string propertyType3 = "Apartment";
            string area3 = "Saddar, Rawalpindi, Pakistan";
            string dimensionType3 = DimensionType.Kanal.ToString();
            string dimensionString3 = "3";
            decimal dimensionDecimal3 = 0;
            string ownerName3 = "Owner Name 3";
            string genderRestriction3 = GenderRestriction.FamiliesOnly.ToString();
            bool isShared3 = true;
            string rentUnit3 = House.GetAllRentUnits()[0];

            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
            houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, ownerEmail3)
                })
            });
            CreateHouseCommand house3 = new CreateHouseCommand(title3, rent3, numberOfBedrooms3, numberOfKitchens3,
                numberOfBathrooms3, internetAvailable3, landlinePhoneAvailable3,
                cableTvAvailable3, garageAvailable3, smokingAllowed3, propertyType3, ownerEmail3, ownerPhoneNumber3,
                houseNo3, streetNo3, area3, dimensionType3, dimensionString3, dimensionDecimal3, ownerName3,
                description3, genderRestriction3, isShared3, rentUnit3, null, null);
            IHttpActionResult houseSaveResult3 = houseController.Post(JsonConvert.SerializeObject(house3));
            Assert.IsFalse(string.IsNullOrWhiteSpace(((OkNegotiatedContentResult<string>) houseSaveResult3).Content));
            string houseId3 = ((OkNegotiatedContentResult<string>) houseSaveResult3).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseId3));
            
            IHttpActionResult response = (IHttpActionResult)houseController.GetHouse(propertyType: propertyType);
            IList<HousePartialRepresentation> retreivedHouses = ((OkNegotiatedContentResult<IList<HousePartialRepresentation>>)response).Content;

            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(2, retreivedHouses.Count);

            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(2, retreivedHouses.Count);
            // Verification of House No 1
            Assert.AreEqual(houseId, retreivedHouses[0].Id);
            Assert.AreEqual(title, retreivedHouses[0].Title);
            Assert.AreEqual(house.Title, retreivedHouses[0].Title);
            Assert.AreEqual(rent, retreivedHouses[0].RentPrice);
            Assert.AreEqual(house.RentPrice, retreivedHouses[0].RentPrice);
            Assert.AreEqual(numberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(numberOfBedrooms, retreivedHouses[0].NumberOfBedrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouses[0].NumberOfBedrooms);
            Assert.AreEqual(numberOfKitchens, retreivedHouses[0].NumberOfKitchens);
            Assert.AreEqual(house.NumberOfKitchens, retreivedHouses[0].NumberOfKitchens);
            Assert.AreEqual(dimensionString + " " + dimensionType, retreivedHouses[0].Dimension);
            Assert.AreEqual(house.DimensionStringValue + " " + house.DimensionType, retreivedHouses[0].Dimension);

            Assert.AreEqual(propertyType, retreivedHouses[0].PropertyType);
            Assert.AreEqual(house.PropertyType, retreivedHouses[0].PropertyType);
            Assert.AreEqual(area, retreivedHouses[0].Area);
            Assert.AreEqual(house.Area, retreivedHouses[0].Area);
            Assert.AreEqual(ownerPhoneNumber, retreivedHouses[0].OwnerPhoneNumber);
            Assert.AreEqual(house.OwnerPhoneNumber, retreivedHouses[0].OwnerPhoneNumber);
            Assert.AreEqual(ownerName, retreivedHouses[0].OwnerName);
            Assert.AreEqual(house.OwnerName, retreivedHouses[0].OwnerName);
            Assert.AreEqual(house.IsShared, retreivedHouses[0].IsShared);
            Assert.AreEqual(house.GenderRestriction.ToString(), retreivedHouses[0].GenderRestriction);
            Assert.AreEqual(rentUnit, retreivedHouses[0].RentUnit);


            // Verification of House No 3 (in order of saving houses above)
            Assert.AreEqual(houseId3, retreivedHouses[1].Id);
            Assert.AreEqual(title3, retreivedHouses[1].Title);
            Assert.AreEqual(house3.Title, retreivedHouses[1].Title);
            Assert.AreEqual(rent3, retreivedHouses[1].RentPrice);
            Assert.AreEqual(house3.RentPrice, retreivedHouses[1].RentPrice);
            Assert.AreEqual(numberOfBathrooms3, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(house3.NumberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(numberOfBedrooms3, retreivedHouses[1].NumberOfBedrooms);
            Assert.AreEqual(house3.NumberOfBedrooms, retreivedHouses[1].NumberOfBedrooms);
            Assert.AreEqual(numberOfKitchens3, retreivedHouses[1].NumberOfKitchens);
            Assert.AreEqual(house3.NumberOfKitchens, retreivedHouses[1].NumberOfKitchens);
            Assert.AreEqual(dimensionString3 + " " + dimensionType2, retreivedHouses[1].Dimension);
            Assert.AreEqual(house3.DimensionStringValue + " " + house2.DimensionType, retreivedHouses[1].Dimension);

            Assert.AreEqual(propertyType3, retreivedHouses[1].PropertyType);
            Assert.AreEqual(house3.PropertyType, retreivedHouses[1].PropertyType);
            Assert.AreEqual(area3, retreivedHouses[1].Area);
            Assert.AreEqual(house3.Area, retreivedHouses[1].Area);
            Assert.AreEqual(ownerPhoneNumber3, retreivedHouses[1].OwnerPhoneNumber);
            Assert.AreEqual(house3.OwnerPhoneNumber, retreivedHouses[1].OwnerPhoneNumber);
            Assert.AreEqual(ownerName3, retreivedHouses[1].OwnerName);
            Assert.AreEqual(house3.OwnerName, retreivedHouses[1].OwnerName);
            Assert.AreEqual(house3.IsShared, retreivedHouses[1].IsShared);
            Assert.AreEqual(house3.GenderRestriction.ToString(), retreivedHouses[1].GenderRestriction);
            Assert.AreEqual(house3.RentUnit, retreivedHouses[1].RentUnit);
        }

        [Category("Integration")]
        [Test]
        public void
            SearchHousesByAreaOnly_TestsThatHouseIsSavedAndRetreivedAsExpected_VerfiesThroughInstanceValue()
        {
            HouseController houseController = _kernel.Get<HouseController>();
            Assert.NotNull(houseController);

            // Saving House # 1: Should appear in search results with area = Pindora (near I-9, Islamabad)
            string title = "Title # 1";
            string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
            int rent = 100001;
            string ownerEmail = "house@1234567-1.com";
            string ownerPhoneNumber = "01234567890";
            string houseNo = "House # 1";
            string streetNo = "1";
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
            string propertyType = "Apartment";
            string area = "Pindora, Rawalpindi, Pakistan";
            string dimensionType = DimensionType.Kanal.ToString();
            string dimensionString = "1";
            decimal dimensionDecimal = 0;
            string ownerName = "Owner Name 1";
            string genderRestriction = GenderRestriction.NoRestriction.ToString();
            bool isShared = false;
            string rentUnit = House.GetAllRentUnits()[3];

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
                isShared, rentUnit, null, null);
            IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(house));
            string houseId = ((OkNegotiatedContentResult<string>)houseSaveResult).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseId));

            // Saving House # 2 - SET 1: Should appear in search results with area = I-9, Islamabad, which would be the search criteria
            string title2 = "Title # 2";
            int rent2 = 100002;
            string ownerEmail2 = "house@1234567-2.com";
            string description2 =
                "Erebor 2. Built deep within the mountain itself the beauty of this fortress was legend.";
            string ownerPhoneNumber2 = "01234567891";
            string houseNo2 = "House # 2";
            string streetNo2 = "2";
            int numberOfBathrooms2 = 2;
            int numberOfBedrooms2 = 2;
            int numberOfKitchens2 = 2;
            bool familiesOnly2 = false;
            bool boysOnly2 = false;
            bool girlsOnly2 = true;
            bool internetAvailable2 = true;
            bool landlinePhoneAvailable2 = true;
            bool cableTvAvailable2 = true;
            bool garageAvailable2 = true;
            bool smokingAllowed2 = true;
            string propertyType2 = "House";
            string area2 = "I-9, Islamabad, Pakistan";
            string dimensionType2 = DimensionType.Kanal.ToString();
            string dimensionString2 = "2";
            decimal dimensionDecimal2 = 0;
            string ownerName2 = "Owner Name 2";
            string genderRestriction2 = GenderRestriction.FamiliesOnly.ToString();
            bool isShared2 = true;
            string rentUnit2 = House.GetAllRentUnits()[2];

            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
            houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, ownerEmail2)
                })
            });
            CreateHouseCommand house2 = new CreateHouseCommand(title2, rent2, numberOfBedrooms2, numberOfKitchens2,
                numberOfBathrooms2, internetAvailable2, landlinePhoneAvailable2,
                cableTvAvailable2, garageAvailable2, smokingAllowed2, propertyType2, ownerEmail2, ownerPhoneNumber2,
                houseNo2, streetNo2, area2, dimensionType2, dimensionString2, dimensionDecimal2, ownerName2,
                description2, genderRestriction2, isShared2, rentUnit2, null, null);
            IHttpActionResult houseSaveResult2 = houseController.Post(JsonConvert.SerializeObject(house2));
            string houseId2 = ((OkNegotiatedContentResult<string>)houseSaveResult2).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseId2));

            // Saving House # 3: Should NOT appear in search results with area = Saddar, Rawalpindi, which is more than 2 kilometers 
            // away from I-9, Islamabad. Remember the search radius is 2 Kilometer
            string title3 = "Title # 3";
            string description3 =
                "Erebor 3. Built deep within the mountain itself the beauty of this fortress was legend.";
            int rent3 = 100003;
            string ownerEmail3 = "house@1234567-3.com";
            string ownerPhoneNumber3 = "01234567892";
            string houseNo3 = "House # 3";
            string streetNo3 = "3";
            int numberOfBathrooms3 = 3;
            int numberOfBedrooms3 = 3;
            int numberOfKitchens3 = 3;
            bool familiesOnly3 = false;
            bool boysOnly3 = false;
            bool girlsOnly3 = true;
            bool internetAvailable3 = true;
            bool landlinePhoneAvailable3 = true;
            bool cableTvAvailable3 = true;
            bool garageAvailable3 = true;
            bool smokingAllowed3 = true;
            string propertyType3 = "Apartment";
            string area3 = "Nara, Punjab, Pakistan";
            string dimensionType3 = DimensionType.Kanal.ToString();
            string dimensionString3 = "3";
            decimal dimensionDecimal3 = 0;
            string ownerName3 = "Owner Name 3";
            string genderRestriction3 = GenderRestriction.FamiliesOnly.ToString();
            bool isShared3 = false;
            string rentUnit3 = House.GetAllRentUnits()[3];

            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
            houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, ownerEmail3)
                })
            });
            CreateHouseCommand house3 = new CreateHouseCommand(title3, rent3, numberOfBedrooms3, numberOfKitchens3,
                numberOfBathrooms3, internetAvailable3, landlinePhoneAvailable3,
                cableTvAvailable3, garageAvailable3, smokingAllowed3, propertyType3, ownerEmail3, ownerPhoneNumber3,
                houseNo3, streetNo3, area3, dimensionType3, dimensionString3, dimensionDecimal3, ownerName3,
                description3, genderRestriction3, isShared3, rentUnit3, null, null);
            IHttpActionResult houseSaveResult3 = houseController.Post(JsonConvert.SerializeObject(house3));
            Assert.IsFalse(string.IsNullOrWhiteSpace(((OkNegotiatedContentResult<string>)houseSaveResult3).Content));
            string houseId3 = ((OkNegotiatedContentResult<string>)houseSaveResult3).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseId3));

            // Search the 
            IHttpActionResult response = (IHttpActionResult)houseController.GetHouse(area: area2);
            IList<HousePartialRepresentation> retreivedHouses = ((OkNegotiatedContentResult<IList<HousePartialRepresentation>>)response).Content;

            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(2, retreivedHouses.Count);

            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(2, retreivedHouses.Count);
            // Verification of House No 2 (nearest to farthest from the searched location)
            Assert.AreEqual(houseId2, retreivedHouses[0].Id);
            Assert.AreEqual(title2, retreivedHouses[0].Title);
            Assert.AreEqual(house2.Title, retreivedHouses[0].Title);
            Assert.AreEqual(rent2, retreivedHouses[0].RentPrice);
            Assert.AreEqual(house2.RentPrice, retreivedHouses[0].RentPrice);
            Assert.AreEqual(numberOfBathrooms2, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(house2.NumberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(numberOfBedrooms2, retreivedHouses[0].NumberOfBedrooms);
            Assert.AreEqual(house2.NumberOfBedrooms, retreivedHouses[0].NumberOfBedrooms);
            Assert.AreEqual(numberOfKitchens2, retreivedHouses[0].NumberOfKitchens);
            Assert.AreEqual(house2.NumberOfKitchens, retreivedHouses[0].NumberOfKitchens);
            Assert.AreEqual(dimensionString2 + " " + dimensionType2, retreivedHouses[0].Dimension);
            Assert.AreEqual(house2.DimensionStringValue + " " + house2.DimensionType, retreivedHouses[0].Dimension);

            Assert.AreEqual(propertyType2, retreivedHouses[0].PropertyType);
            Assert.AreEqual(house2.PropertyType, retreivedHouses[0].PropertyType);
            Assert.AreEqual(area2, retreivedHouses[0].Area);
            Assert.AreEqual(house2.Area, retreivedHouses[0].Area);
            Assert.AreEqual(ownerPhoneNumber2, retreivedHouses[0].OwnerPhoneNumber);
            Assert.AreEqual(house2.OwnerPhoneNumber, retreivedHouses[0].OwnerPhoneNumber);
            Assert.AreEqual(ownerName2, retreivedHouses[0].OwnerName);
            Assert.AreEqual(house2.OwnerName, retreivedHouses[0].OwnerName);
            Assert.AreEqual(house2.IsShared, retreivedHouses[0].IsShared);
            Assert.AreEqual(house2.GenderRestriction.ToString(), retreivedHouses[0].GenderRestriction);
            Assert.AreEqual(house2.RentUnit, retreivedHouses[0].RentUnit);

            // Verification of House No 2 (in order of saving houses above)
            Assert.AreEqual(houseId, retreivedHouses[1].Id);
            Assert.AreEqual(title, retreivedHouses[1].Title);
            Assert.AreEqual(house.Title, retreivedHouses[1].Title);
            Assert.AreEqual(rent, retreivedHouses[1].RentPrice);
            Assert.AreEqual(house.RentPrice, retreivedHouses[1].RentPrice);
            Assert.AreEqual(numberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(numberOfBedrooms, retreivedHouses[1].NumberOfBedrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouses[1].NumberOfBedrooms);
            Assert.AreEqual(numberOfKitchens, retreivedHouses[1].NumberOfKitchens);
            Assert.AreEqual(house.NumberOfKitchens, retreivedHouses[1].NumberOfKitchens);
            Assert.AreEqual(dimensionString + " " + dimensionType2, retreivedHouses[1].Dimension);
            Assert.AreEqual(house.DimensionStringValue + " " + house2.DimensionType, retreivedHouses[1].Dimension);

            Assert.AreEqual(propertyType, retreivedHouses[1].PropertyType);
            Assert.AreEqual(house.PropertyType, retreivedHouses[1].PropertyType);
            Assert.AreEqual(area, retreivedHouses[1].Area);
            Assert.AreEqual(house.Area, retreivedHouses[1].Area);
            Assert.AreEqual(ownerPhoneNumber, retreivedHouses[1].OwnerPhoneNumber);
            Assert.AreEqual(house.OwnerPhoneNumber, retreivedHouses[1].OwnerPhoneNumber);
            Assert.AreEqual(ownerName, retreivedHouses[1].OwnerName);
            Assert.AreEqual(house.OwnerName, retreivedHouses[1].OwnerName);
            Assert.AreEqual(house.IsShared, retreivedHouses[1].IsShared);
            Assert.AreEqual(house.GenderRestriction, retreivedHouses[1].GenderRestriction);
            Assert.AreEqual(rentUnit, retreivedHouses[1].RentUnit);
        }

        [Test]
        public void GetAllRentUnitTest_ChecksThatAllRentUnitsAreReturnedProperly_VerifiesThroughReturnedValue()
        {
            HouseController houseController = _kernel.Get<HouseController>();
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
            HouseController houseController = _kernel.Get<HouseController>();
            var httpActionResult = houseController.GetPropertyTypes();
            var allPropertyTypes = ((OkNegotiatedContentResult<IList<string>>)httpActionResult).Content;
            Assert.Greater(allPropertyTypes.Count, 0);

            for (int i = 0; i < allPropertyTypes.Count - 1; i++)
            {
                Assert.AreEqual(House.GetAllPropertyTypes()[i], allPropertyTypes[i]);
            }
        }
    }
}
