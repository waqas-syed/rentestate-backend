﻿using Ninject;
using NUnit.Framework;
using RentStuff.Common.Ninject.Modules;
using RentStuff.Common.Utilities;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Domain.Model.PropertyAggregate;
using RentStuff.Property.Infrastructure.Persistence.Ninject.Modules;
using System;
using System.Collections.Generic;

namespace RentStuff.Property.Persistence.IntegrationTests
{
    [TestFixture]
    public class HouseRepositoryTests
    {
        private readonly decimal _latitudeIncrementForMultipleHouseSaves = 0.0005M;
        private readonly decimal _longitudeIncrementForMultipleHouseSaves = 0.0005M;
        private DatabaseUtility _databaseUtility;
        private IKernel _kernel;

        [SetUp]
        public void Setup()
        {
            var connection = StringCipher.DecipheredConnectionString;
            _databaseUtility = new DatabaseUtility(connection);
            _databaseUtility.Create();
            _kernel = new StandardKernel();
            _kernel.Load<PropertyPersistenceNinjectModule>();
            _kernel.Load<CommonNinjectModule>();
            //NhConnectionDecipherService.SetupDecipheredConnectionString();
            //_databaseUtility.Populate();
        }

        [TearDown]
        public void Teardown()
        {
            _databaseUtility.Create();
        }

        #region Save and Get Houses By Id

        [Test]
        public void SaveHouseTest_TestsThatHouseUInstancesAreSavedToTheDatabaseAsExpected_VerifiesThroughDatabaseQuery()
        {
            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();
            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService =
                _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();

            // APARTMENT
            // Save the Apartment in the repository and retreive it
            string area = "Rawalpindi, Punjab, Pakistan";
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(area);
            decimal latitude = coordinatesFromAddress.Item1;
            decimal longitude = coordinatesFromAddress.Item2;
            string houseNo = "S-123";
            string streetNo = "13";
            string title = "Special House";
            string description = "It was a Hobbit Hole. Which means it had good food and a warm hearth.";
            string phoneNumber = "01234567890";
            string email = "special@spsp123456.com";
            int numberOfBathrooms = 1;
            int numberOfBedrooms = 1;
            int numberOfKitchens = 1;
            int rent = 50000;
            string ownerName = "Owner Name 1";
            string propertyType = Constants.Apartment;
            bool isShared = false;
            string rentUnit = Constants.Hourly;
            string landlineNumber = "0510000000";
            string fax = "0510000000";
            bool cable = false;
            bool garage = false;
            bool landlinePhoneAvailable = false;
            bool smokingAllowed = false;
            bool internet = true;
            GenderRestriction genderRestriction = GenderRestriction.FamiliesOnly;
            House house = new House.HouseBuilder().Title(title).OwnerEmail(email)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberOfBathrooms).OwnerPhoneNumber(phoneNumber)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(cable)
                .GarageAvailable(garage).LandlinePhoneAvailable(landlinePhoneAvailable).SmokingAllowed(smokingAllowed)
                .WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(rent).Latitude(latitude)
                .Longitude(longitude).GenderRestriction(genderRestriction)
                .HouseNo(houseNo).Area(area).OwnerName(ownerName).StreetNo(streetNo).Description(description)
                .IsShared(isShared).RentUnit(rentUnit).LandlineNumber(landlineNumber).Fax(fax).Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, "5", 0, house);
            house.Dimension = dimension;
            houseRepository.SaveorUpdateDimension(dimension);
            houseRepository.SaveorUpdate(house);

            House retreivedHouse = (House)houseRepository.GetPropertyById(house.Id);

            Assert.NotNull(retreivedHouse);
            Assert.AreEqual(title, retreivedHouse.Title);
            Assert.AreEqual(description, retreivedHouse.Description);
            Assert.AreEqual(phoneNumber, retreivedHouse.OwnerPhoneNumber);
            Assert.AreEqual(email, retreivedHouse.OwnerEmail);
            Assert.AreEqual(rent, retreivedHouse.RentPrice);
            Assert.AreEqual(numberOfBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(numberOfBedrooms, retreivedHouse.NumberOfBedrooms);
            Assert.AreEqual(numberOfKitchens, retreivedHouse.NumberOfKitchens);
            Assert.AreEqual(garage, retreivedHouse.GarageAvailable);
            Assert.AreEqual(landlinePhoneAvailable, retreivedHouse.LandlinePhoneAvailable);
            Assert.AreEqual(cable, retreivedHouse.CableTvAvailable);
            Assert.AreEqual(internet, retreivedHouse.InternetAvailable);
            Assert.AreEqual(smokingAllowed, retreivedHouse.SmokingAllowed);
            Assert.AreEqual(propertyType, retreivedHouse.PropertyType);
            Assert.AreEqual(latitude, retreivedHouse.Latitude);
            Assert.AreEqual(longitude, retreivedHouse.Longitude);
            Assert.AreEqual(houseNo, retreivedHouse.HouseNo);
            Assert.AreEqual(area, retreivedHouse.Area);
            Assert.AreEqual(streetNo, retreivedHouse.StreetNo);
            Assert.AreEqual(phoneNumber, retreivedHouse.OwnerPhoneNumber);
            Assert.AreEqual(dimension.DimensionType, retreivedHouse.Dimension.DimensionType);
            Assert.AreEqual(dimension.DecimalValue, retreivedHouse.Dimension.DecimalValue);
            Assert.AreEqual(dimension.StringValue, retreivedHouse.Dimension.StringValue);
            Assert.AreEqual(ownerName, retreivedHouse.OwnerName);
            Assert.AreEqual(genderRestriction, retreivedHouse.GenderRestriction);
            Assert.AreEqual(rentUnit, retreivedHouse.RentUnit);
            Assert.AreEqual(landlineNumber, retreivedHouse.LandlineNumber);
            Assert.AreEqual(fax, retreivedHouse.Fax);

            // HOUSE
            string area2 = "Islamabad, Islamabad Capital Territory, Pakistan";
            var coordinatesFromAddress2 = geocodingService.GetCoordinatesFromAddress(area2);
            decimal latitude2 = coordinatesFromAddress2.Item1;
            decimal longitude2 = coordinatesFromAddress2.Item2;
            string title2 = "Special House 2";
            string email2 = "special2@spsp123456-2.com";
            string description2 = "Erobor. Built deep within the mountain itself the beauty of this fortress was legend. 2";
            string houseNo2 = "S2-123";
            string streetNo2 = "2-13";
            string phoneNumber2 = "03990000002";
            int numberOfBathrooms2 = 2;
            int numberOfBedrooms2 = 2;
            int numberOfKitchens2 = 3;
            int rent2 = 100000;
            string ownerName2 = "Owner Name 2";
            string propertyType2 = Constants.House;
            bool isShared2 = true;
            string rentUnit2 = Constants.Weekly;
            string landlineNumber2 = "0510000002";
            string fax2 = "0510000002";
            bool cable2 = false;
            bool garage2 = true;
            bool landlinePhoneAvailable2 = true;
            bool smokingAllowed2 = false;
            bool internet2 = false;
            GenderRestriction genderRestriction2 = GenderRestriction.FamiliesOnly;
            House house2 = new House.HouseBuilder().Title(title2).OwnerEmail(email2)
                .NumberOfBedrooms(numberOfBedrooms2).NumberOfBathrooms(numberOfBathrooms2).OwnerPhoneNumber(phoneNumber2)
                .NumberOfKitchens(numberOfKitchens2).CableTvAvailable(cable2).GarageAvailable(garage2)
                .LandlinePhoneAvailable(landlinePhoneAvailable2).SmokingAllowed(smokingAllowed2).Fax(fax2)
                .WithInternetAvailable(internet2).HouseNo(houseNo2).PropertyType(propertyType2).RentPrice(rent2)
                .Latitude(latitude2).Longitude(longitude2).Area(area2).OwnerName(ownerName2).StreetNo(streetNo2)
                .Description(description2).IsShared(isShared2).RentUnit(rentUnit2).LandlineNumber(landlineNumber2)
                .GenderRestriction(genderRestriction2)
                .Build();
            Dimension dimension2 = new Dimension(DimensionType.Marla, "20", 0, house2);
            house2.Dimension = dimension2;
            houseRepository.SaveorUpdateDimension(dimension2);
            houseRepository.SaveorUpdate(house2);

            retreivedHouse = (House)houseRepository.GetPropertyById(house2.Id);

            Assert.AreEqual(title2, retreivedHouse.Title);
            Assert.AreEqual(description2, retreivedHouse.Description);
            Assert.AreEqual(phoneNumber2, retreivedHouse.OwnerPhoneNumber);
            Assert.AreEqual(email2, retreivedHouse.OwnerEmail);
            Assert.AreEqual(rent2, retreivedHouse.RentPrice);
            Assert.AreEqual(numberOfBathrooms2, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(numberOfBedrooms2, retreivedHouse.NumberOfBedrooms);
            Assert.AreEqual(numberOfKitchens2, retreivedHouse.NumberOfKitchens);
            Assert.AreEqual(garage2, retreivedHouse.GarageAvailable);
            Assert.AreEqual(landlinePhoneAvailable2, retreivedHouse.LandlinePhoneAvailable);
            Assert.AreEqual(cable2, retreivedHouse.CableTvAvailable);
            Assert.AreEqual(internet2, retreivedHouse.InternetAvailable);
            Assert.AreEqual(smokingAllowed2, retreivedHouse.SmokingAllowed);
            Assert.AreEqual(propertyType2, retreivedHouse.PropertyType);
            Assert.AreEqual(latitude2, retreivedHouse.Latitude);
            Assert.AreEqual(longitude2, retreivedHouse.Longitude);
            Assert.AreEqual(houseNo2, retreivedHouse.HouseNo);
            Assert.AreEqual(area2, retreivedHouse.Area);
            Assert.AreEqual(streetNo2, retreivedHouse.StreetNo);
            Assert.AreEqual(phoneNumber2, retreivedHouse.OwnerPhoneNumber);
            Assert.AreEqual(dimension2.DimensionType, retreivedHouse.Dimension.DimensionType);
            Assert.AreEqual(dimension2.DecimalValue, retreivedHouse.Dimension.DecimalValue);
            Assert.AreEqual(dimension2.StringValue, retreivedHouse.Dimension.StringValue);
            Assert.AreEqual(ownerName2, retreivedHouse.OwnerName);
            Assert.AreEqual(genderRestriction2, retreivedHouse.GenderRestriction);
            Assert.AreEqual(rentUnit2, retreivedHouse.RentUnit);
            Assert.AreEqual(landlineNumber2, retreivedHouse.LandlineNumber);
            Assert.AreEqual(fax2, retreivedHouse.Fax);
        }

        [Test]
        public void SaveAndUpdateHouseTest_TestsThatHouseUInstancesAreSavedAndUpdatedToTheDatabaseAsExpected_VerifiesThroughDatabaseQuery()
        {
            //Save the house in the repository and retreive it. Primitive test
            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();
            string email = "w@12344321.com";
            string description = "It was a Hobbit Hole. Which means it had good food and a warm hearth.";
            string title = "MGM Grand";
            string phoneNumber = "01234567890";
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long price = 90000;
            string houseNo = "123";
            string area = "Pindora";
            string streetNo = "13";
            decimal latitude = 33.29M;
            decimal longitude = 73.41M;
            string ownerName = "Owner Name 1";
            string propertyType = "Apartment";
            GenderRestriction genderRestriction = GenderRestriction.FamiliesOnly;
            bool isShared = true;
            string landlineNumber = "0510000000";
            string fax = "0510000000";
            bool ac = true;
            bool geyser = true;
            bool balcony = true;
            bool lawn = true;
            bool heating = false;
            bool cctvCameras = true;
            bool backupElectricity = true;
            bool bathtub = true;
            bool elevator = true;

            House house = new House.HouseBuilder().Title(title).OwnerEmail(email).OwnerPhoneNumber(phoneNumber)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(propertyType).RentPrice(price).Latitude(latitude).Longitude(longitude)
                .HouseNo(houseNo).Area(area).StreetNo(streetNo).OwnerName(ownerName).Description(description)
                .GenderRestriction(genderRestriction).IsShared(isShared).LandlineNumber(landlineNumber).Fax(fax)
                .AC(ac).Geyser(geyser).Balcony(balcony).Lawn(lawn).Heating(heating)
                .CctvCameras(cctvCameras).BackupElectricity(backupElectricity).Bathtub(bathtub)
                .Elevator(elevator).Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, null, 5, house);
            house.Dimension = dimension;
            //houseRepository.SaveorUpdateDimension(dimension);
            houseRepository.SaveorUpdate(house);

            House retreivedHouse = (House)houseRepository.GetPropertyById(house.Id);

            Assert.NotNull(retreivedHouse);
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
            Assert.AreEqual(house.Latitude, retreivedHouse.Latitude);
            Assert.AreEqual(house.Longitude, retreivedHouse.Longitude);
            Assert.AreEqual(house.HouseNo, retreivedHouse.HouseNo);
            Assert.AreEqual(house.Area, retreivedHouse.Area);
            Assert.AreEqual(house.StreetNo, retreivedHouse.StreetNo);
            Assert.AreEqual(house.Dimension.DimensionType, retreivedHouse.Dimension.DimensionType);
            Assert.AreEqual(house.Dimension.DecimalValue, retreivedHouse.Dimension.DecimalValue);
            Assert.AreEqual(house.Dimension.StringValue, retreivedHouse.Dimension.StringValue);
            Assert.AreEqual(house.OwnerName, retreivedHouse.OwnerName);
            Assert.AreEqual(house.GenderRestriction, retreivedHouse.GenderRestriction);
            Assert.IsNotNull(retreivedHouse.DateCreated);
            Assert.AreEqual(house.DateCreated, retreivedHouse.DateCreated);
            Assert.IsNotNull(retreivedHouse.LastModified);
            Assert.AreEqual(house.LastModified, retreivedHouse.LastModified);
            Assert.AreEqual(landlineNumber, retreivedHouse.LandlineNumber);
            Assert.AreEqual(fax, retreivedHouse.Fax);

            Assert.AreEqual(ac, retreivedHouse.AC);
            Assert.AreEqual(geyser, retreivedHouse.Geyser);
            Assert.AreEqual(balcony, retreivedHouse.Balcony);
            Assert.AreEqual(lawn, retreivedHouse.Lawn);
            Assert.AreEqual(cctvCameras, retreivedHouse.CctvCameras);
            Assert.AreEqual(backupElectricity, retreivedHouse.BackupElectricity);
            Assert.AreEqual(heating, retreivedHouse.Heating);
            Assert.AreEqual(bathtub, retreivedHouse.Bathtub);
            Assert.AreEqual(elevator, retreivedHouse.Elevator);

            // Get the value of last modified
            DateTime? lastModifiedInitialValue = house.LastModified;

            // Defaut value of Month should be used
            string rentUnit = "Month";
            Assert.AreEqual(rentUnit, retreivedHouse.RentUnit);

            // NOW UDPATE THE VALUES
            // Create and save the instance and then update it with new values
            string title2 = "Title No 2";
            string description2 = "Description of house update";
            string email2 = "w@12344321-3.com";
            string name2 = "OwnerName2";
            string phoneNumber2 = "03455138018";

            // No Latitude is given. So the house instance should not be created
            int numberOfBedrooms2 = 3;
            int numberofBathrooms2 = 3;
            int numberOfKitchens2 = 2;
            bool smokingAllowed2 = false;
            bool landline2 = true;
            bool cableTv2 = false;
            bool internet2 = true;
            bool garage2 = false;
            decimal latitude2 = 25.13M;
            decimal longitude2 = 73.11M;
            string propertyType2 = Constants.House;
            GenderRestriction genderRestriction2 = GenderRestriction.BoysOnly;
            string area2 = "Pindora, Rawalpindi, Pakistan";
            long monthlyRent2 = 199000;
            string rentUnit2 = "Day";
            bool isShared2 = false;
            string landlineNumber2 = "0510000001";
            string fax2 = "0510000001";

            bool ac2 = true;
            bool geyser2 = true;
            bool balcony2 = true;
            bool lawn2 = true;
            bool heating2 = false;
            bool cctvCameras2 = true;
            bool backupElectricity2 = true;
            bool bathtub2 = true;
            bool elevator2 = true;

            Dimension dimension2 = new Dimension(DimensionType.Marla, "20", 0, house);
            house.UpdateHouse(title2, monthlyRent2, numberOfBedrooms2, numberOfKitchens2, numberofBathrooms2, internet2, landline2,
                cableTv2, dimension2, garage2, smokingAllowed2, propertyType2, email2, phoneNumber2, null, null, area2, name2,
                description2, genderRestriction2, latitude2, longitude2, isShared2, rentUnit2, landlineNumber2, fax2,
                ac2, geyser2, balcony2, lawn2, cctvCameras2, backupElectricity2, heating2, bathtub2, elevator2);
            houseRepository.SaveorUpdate(house);
            House retreivedHouse2 = (House)houseRepository.GetPropertyById(house.Id);

            Assert.AreEqual(title2, retreivedHouse2.Title);
            Assert.AreEqual(description2, retreivedHouse2.Description);
            Assert.AreEqual(email2, retreivedHouse2.OwnerEmail);
            Assert.AreEqual(name2, retreivedHouse2.OwnerName);
            Assert.AreEqual(phoneNumber2, retreivedHouse2.OwnerPhoneNumber);
            Assert.AreEqual(numberOfBedrooms2, retreivedHouse2.NumberOfBedrooms);
            Assert.AreEqual(numberofBathrooms2, retreivedHouse2.NumberOfBathrooms);
            Assert.AreEqual(numberOfKitchens2, retreivedHouse2.NumberOfKitchens);
            Assert.AreEqual(smokingAllowed2, retreivedHouse2.SmokingAllowed);
            Assert.AreEqual(landline2, retreivedHouse2.LandlinePhoneAvailable);
            Assert.AreEqual(cableTv2, retreivedHouse2.CableTvAvailable);
            Assert.AreEqual(internet2, retreivedHouse2.InternetAvailable);
            Assert.AreEqual(garage2, retreivedHouse2.GarageAvailable);
            Assert.AreEqual(latitude2, retreivedHouse2.Latitude);
            Assert.AreEqual(longitude2, retreivedHouse2.Longitude);
            Assert.AreEqual(propertyType2, retreivedHouse2.PropertyType);
            Assert.AreEqual(genderRestriction2, retreivedHouse2.GenderRestriction);
            Assert.AreEqual(area2, retreivedHouse2.Area);
            Assert.AreEqual(monthlyRent2, retreivedHouse2.RentPrice);

            // Date Created should be the same
            Assert.AreEqual(retreivedHouse2.DateCreated, retreivedHouse.DateCreated);
            // Last Modified should be changed as we update the instance
            Assert.Greater(retreivedHouse2.LastModified, lastModifiedInitialValue);

            Assert.AreEqual(dimension2.DimensionType, retreivedHouse2.Dimension.DimensionType);
            Assert.AreEqual(dimension2.StringValue, retreivedHouse2.Dimension.StringValue);
            Assert.AreEqual(dimension2.DecimalValue, retreivedHouse2.Dimension.DecimalValue);

            Assert.AreEqual(isShared2, retreivedHouse2.IsShared);
            Assert.AreEqual(rentUnit2, retreivedHouse2.RentUnit);

            Assert.AreEqual(landlineNumber2, retreivedHouse.LandlineNumber);
            Assert.AreEqual(fax2, retreivedHouse.Fax);

            Assert.AreEqual(ac2, retreivedHouse.AC);
            Assert.AreEqual(geyser2, retreivedHouse.Geyser);
            Assert.AreEqual(balcony2, retreivedHouse.Balcony);
            Assert.AreEqual(lawn2, retreivedHouse.Lawn);
            Assert.AreEqual(cctvCameras2, retreivedHouse.CctvCameras);
            Assert.AreEqual(backupElectricity2, retreivedHouse.BackupElectricity);
            Assert.AreEqual(heating2, retreivedHouse.Heating);
            Assert.AreEqual(bathtub2, retreivedHouse.Bathtub);
            Assert.AreEqual(elevator2, retreivedHouse.Elevator);
        }

        #endregion Save and Get Houses By Id

        #region Delete House

        [Test]
        public void SaveAndDeleteHouseTest_TestsThatHouseInstancesAreDeletedFromTheDatabaseAsExpected_VerifiesThroughDatabaseQuery()
        {
            // Save the house in the repository and retreive it. Primitive test
            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();
            string email = "w@12344321.com";
            string description = "It was a Hobbit Hole. Which means it had good food and a warm hearth.";
            string title = "MGM Grand";
            string phoneNumber = "01234567890";
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long price = 90000;
            string houseNo = "123";
            string area = "Pindora";
            string streetNo = "13";
            decimal latitude = 33.29M;
            decimal longitude = 73.41M;
            string ownerName = "Owner Name 1";
            string propertyType = "Apartment";
            GenderRestriction genderRestriction = GenderRestriction.FamiliesOnly;
            bool isShared = true;
            string landlineNumbe = "03510000000";
            string fax = "0510000000";

            House house = new House.HouseBuilder().Title(title).OwnerEmail(email).OwnerPhoneNumber(phoneNumber)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(propertyType).RentPrice(price).Latitude(latitude).Longitude(longitude)
                .HouseNo(houseNo).Area(area).StreetNo(streetNo).OwnerName(ownerName).Description(description)
                .GenderRestriction(genderRestriction).IsShared(isShared).LandlineNumber(landlineNumbe).Fax(fax).Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, null, 5, house);
            house.Dimension = dimension;
            houseRepository.SaveorUpdateDimension(dimension);
            houseRepository.SaveorUpdate(house);

            House retreivedHouse = (House)houseRepository.GetPropertyById(house.Id);

            Assert.NotNull(retreivedHouse);
            
            // Now Delete the house

            houseRepository.Delete(house);
            retreivedHouse = (House)houseRepository.GetPropertyById(house.Id);

            Assert.Null(retreivedHouse);
        }

        #endregion Delete House

        #region Save and Search Houses By Area

        [Test]
        [Category("Integration")]
        public void SearchHouseAndApartmentByCoordinates_ChecksIfTheRepositoryCallReturnsTheExpectedResult_VerifiesByReturnValue()
        {
            // Save 4 properties, 2 houses and 2 apartments. 1 house and 1 apartment should come within the 
            // search radius and returned
            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();
            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService = 
                _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();
            
            // Saving House # 1
            string area = "Rawalpindi, Punjab";
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(area);
            decimal latitude = coordinatesFromAddress.Item1;
            decimal longitude = coordinatesFromAddress.Item2;
            string houseNo = "S-123";
            string streetNo = "13";
            string title = "Special House";
            string description = "It was a Hobbit Hole. Which means it had good food and a warm hearth.";
            string phoneNumber = "01234567890";
            string email = "special@spsp123456.com";
            int numberOfBathrooms = 1;
            int numberOfBedrooms = 1;
            int numberOfKitchens = 1;
            int rent = 50000;
            string ownerName = "Owner Name 1";
            string propertyType = Constants.Apartment;
            bool isShared = false;
            string rentUnit = Constants.Hourly;
            string landlineNumber = "0510000000";
            string fax = "0510000000";
            bool cable = false;
            bool garage = false;
            bool landlinePhoneAvailable = false;
            bool smokingAllowed = false;
            bool internet = true;
            GenderRestriction genderRestriction = GenderRestriction.FamiliesOnly;
            House house = new House.HouseBuilder().Title(title).OwnerEmail(email)
            .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberOfBathrooms).OwnerPhoneNumber(phoneNumber)
            .NumberOfKitchens(numberOfKitchens).CableTvAvailable(cable)
            .GarageAvailable(garage).LandlinePhoneAvailable(landlinePhoneAvailable).SmokingAllowed(smokingAllowed)
            .WithInternetAvailable(internet)
            .PropertyType(propertyType).RentPrice(rent).Latitude(latitude)
            .Longitude(longitude).GenderRestriction(genderRestriction)
            .HouseNo(houseNo).Area(area).OwnerName(ownerName).StreetNo(streetNo).Description(description)
            .IsShared(isShared).RentUnit(rentUnit).LandlineNumber(landlineNumber).Fax(fax).Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, "5", 0, house);
            house.Dimension = dimension;
            houseRepository.SaveorUpdateDimension(dimension);
            houseRepository.SaveorUpdate(house);

            // Saving House # 2
            string area2 = "Islamabad";
            var coordinatesFromAddress2 = geocodingService.GetCoordinatesFromAddress(area2);
            decimal latitude2 = coordinatesFromAddress2.Item1;
            decimal longitude2 = coordinatesFromAddress2.Item2;
            string title2 = "Special House 2";
            string email2 = "special2@spsp123456-2.com";
            string description2 = "Erobor. Built deep within the mountain itself the beauty of this fortress was legend. 2";
            string houseNo2 = "S2-123";
            string streetNo2 = "2-13";
            string phoneNumber2 = "03990000002";
            int numberOfBathrooms2 = 2;
            int numberOfBedrooms2 = 2;
            int numberOfKitchens2 = 3;
            int rent2 = 100000;
            string ownerName2 = "Owner Name 2";
            string propertyType2 = Constants.House;
            bool isShared2 = true;
            string rentUnit2 = Constants.Weekly;
            string landlineNumber2 = "0510000002";
            string fax2 = "0510000002";
            bool cable2 = false;
            bool garage2 = true;
            bool landlinePhoneAvailable2 = true;
            bool smokingAllowed2 = false;
            bool internet2 = false;
            GenderRestriction genderRestriction2 = GenderRestriction.FamiliesOnly;
            House house2 = new House.HouseBuilder().Title(title2).OwnerEmail(email2)
            .NumberOfBedrooms(numberOfBedrooms2).NumberOfBathrooms(numberOfBathrooms2).OwnerPhoneNumber(phoneNumber2)
            .NumberOfKitchens(numberOfKitchens2).CableTvAvailable(cable2).GarageAvailable(garage2)
            .LandlinePhoneAvailable(landlinePhoneAvailable2).SmokingAllowed(smokingAllowed2).Fax(fax2)
            .WithInternetAvailable(internet2).HouseNo(houseNo2).PropertyType(propertyType2).RentPrice(rent2)
            .Latitude(latitude2).Longitude(longitude2).Area(area2).OwnerName(ownerName2).StreetNo(streetNo2)
            .Description(description2).IsShared(isShared2).RentUnit(rentUnit2).LandlineNumber(landlineNumber2)
            .GenderRestriction(genderRestriction2)
            .Build();
            Dimension dimension2 = new Dimension(DimensionType.Marla, "20", 0, house2);
            house2.Dimension = dimension2;
            houseRepository.SaveorUpdateDimension(dimension2);
            houseRepository.SaveorUpdate(house2);

            // Saving House # 3
            string area3 = "Lahore, Punjab, Pakistan";
            var coordinatesFromAddress3 = geocodingService.GetCoordinatesFromAddress(area3);
            decimal latitude3 = coordinatesFromAddress3.Item1;
            decimal longitude3 = coordinatesFromAddress3.Item2;
            string email3 = "special2@spsp123456-3.com";
            string title3 = "Special House 3";
            string description3 = "Erobor. Built deep within the mountain itself the beauty of this fortress was legend. 3";
            string houseNo3 = "S2-1233";
            string streetNo3 = "2-133";
            string phoneNumber3 = "03990000003";
            int numberOfBathrooms3 = 3;
            int numberOfBedrooms3 = 3;
            int numberOfKitchens3 = 1;
            int rent3 = 100000;
            string ownerName3 = "Owner Name 3";
            string propertyType3 = Constants.House;
            bool isShared3 = true;
            string rentUnit3 = Constants.Monthly;
            string landlineNumber3 = "0510000003";
            string fax3 = "0510000003";
            bool cable3 = true;
            bool garage3 = true;
            bool landlinePhoneAvailable3 = true;
            bool smokingAllowed3 = true;
            bool internet3 = false;
            GenderRestriction genderRestriction3 = GenderRestriction.FamiliesOnly;
            House house3 = new House.HouseBuilder().Title(title).OwnerEmail(email3)
                .NumberOfBedrooms(numberOfBedrooms3).NumberOfBathrooms(numberOfBathrooms3).OwnerPhoneNumber(phoneNumber3)
                .NumberOfKitchens(numberOfKitchens3).CableTvAvailable(cable3)
                .GarageAvailable(garage3).LandlinePhoneAvailable(landlinePhoneAvailable3).SmokingAllowed(smokingAllowed3)
                .WithInternetAvailable(internet3)
                .PropertyType(propertyType3).RentPrice(rent3).Latitude(latitude3)
                .Longitude(longitude3).GenderRestriction(genderRestriction3)
                .HouseNo(houseNo3).Area(area).OwnerName(ownerName3).StreetNo(streetNo3).Description(description3).IsShared(isShared3)
                .RentUnit(rentUnit3).LandlineNumber(landlineNumber3).Fax(fax3)
                .Build();
            Dimension dimension3 = new Dimension(DimensionType.Marla, "30", 0, house3);
            house3.Dimension = dimension3;
            houseRepository.SaveorUpdateDimension(dimension3);
            houseRepository.SaveorUpdate(house3);

            // Saving House # 4
            string area4 = "Karachi, Sindh, Pakistan";
            var coordinatesFromAddress4 = geocodingService.GetCoordinatesFromAddress(area4);
            decimal latitude4 = coordinatesFromAddress4.Item1;
            decimal longitude4 = coordinatesFromAddress4.Item2;
            string email4 = "special2@spsp124456-4.com";
            string title4 = "Special House 4";
            string description4 = "Erobor. Built deep within the mountain itself the beauty of this fortress was legend. 4";
            string houseNo4 = "S2-1244";
            string streetNo4 = "2-144";
            string phoneNumber4 = "04990000004";
            int numberOfBathrooms4 = 4;
            int numberOfBedrooms4 = 4;
            int numberOfKitchens4 = 1;
            int rent4 = 100000;
            string ownerName4 = "Owner Name 4";
            string propertyType4 = Constants.Apartment;
            bool isShared4 = true;
            string rentUnit4 = Constants.Daily;
            string landlineNumber4 = "0510000004";
            string fax4 = "0510000004";
            bool cable4 = true;
            bool garage4 = true;
            bool landlinePhoneAvailable4 = true;
            bool smokingAllowed4 = true;
            bool internet4 = false;
            GenderRestriction genderRestriction4 = GenderRestriction.FamiliesOnly;
            House house4 = new House.HouseBuilder().Title(title).OwnerEmail(email4)
                .NumberOfBedrooms(numberOfBedrooms4).NumberOfBathrooms(numberOfBathrooms4).OwnerPhoneNumber(phoneNumber4)
                .NumberOfKitchens(numberOfKitchens4).CableTvAvailable(cable4)
                .GarageAvailable(garage4).LandlinePhoneAvailable(landlinePhoneAvailable4).SmokingAllowed(smokingAllowed4)
                .WithInternetAvailable(internet4)
                .PropertyType(propertyType4).RentPrice(rent4).Latitude(latitude4)
                .Longitude(longitude4).GenderRestriction(genderRestriction4)
                .HouseNo(houseNo4).Area(area).OwnerName(ownerName4).StreetNo(streetNo4).Description(description4).IsShared(isShared4)
                .RentUnit(rentUnit4).LandlineNumber(landlineNumber4).Fax(fax4)
                .Build();
            Dimension dimension4 = new Dimension(DimensionType.Marla, "40", 0, house4);
            house4.Dimension = dimension4;
            houseRepository.SaveorUpdateDimension(dimension4);
            houseRepository.SaveorUpdate(house4);

            var retreivedHouses = houseRepository.SearchHousesByCoordinates(coordinatesFromAddress.Item1,
                coordinatesFromAddress.Item2);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(2, retreivedHouses.Count);
            
            // Verification of House # 1
            Assert.AreEqual(title, retreivedHouses[0].Title);
            Assert.AreEqual(description, retreivedHouses[0].Description);
            Assert.AreEqual(phoneNumber, retreivedHouses[0].OwnerPhoneNumber);
            Assert.AreEqual(email, retreivedHouses[0].OwnerEmail);
            Assert.AreEqual(rent, retreivedHouses[0].RentPrice);
            Assert.AreEqual(numberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(numberOfBedrooms, retreivedHouses[0].NumberOfBedrooms);
            Assert.AreEqual(numberOfKitchens, retreivedHouses[0].NumberOfKitchens);
            Assert.AreEqual(garage, retreivedHouses[0].GarageAvailable);
            Assert.AreEqual(landlinePhoneAvailable, retreivedHouses[0].LandlinePhoneAvailable);
            Assert.AreEqual(cable, retreivedHouses[0].CableTvAvailable);
            Assert.AreEqual(internet, retreivedHouses[0].InternetAvailable);
            Assert.AreEqual(smokingAllowed, retreivedHouses[0].SmokingAllowed);
            Assert.AreEqual(propertyType, retreivedHouses[0].PropertyType);
            Assert.AreEqual(latitude, retreivedHouses[0].Latitude);
            Assert.AreEqual(longitude, retreivedHouses[0].Longitude);
            Assert.AreEqual(houseNo, retreivedHouses[0].HouseNo);
            Assert.AreEqual(area, retreivedHouses[0].Area);
            Assert.AreEqual(streetNo, retreivedHouses[0].StreetNo);
            Assert.AreEqual(phoneNumber, retreivedHouses[0].OwnerPhoneNumber);
            Assert.AreEqual(dimension.DimensionType, retreivedHouses[0].Dimension.DimensionType);
            Assert.AreEqual(dimension.DecimalValue, retreivedHouses[0].Dimension.DecimalValue);
            Assert.AreEqual(dimension.StringValue, retreivedHouses[0].Dimension.StringValue);
            Assert.AreEqual(ownerName, retreivedHouses[0].OwnerName);
            Assert.AreEqual(genderRestriction, retreivedHouses[0].GenderRestriction);
            Assert.AreEqual(rentUnit, retreivedHouses[0].RentUnit);
            Assert.AreEqual(landlineNumber, retreivedHouses[0].LandlineNumber);
            Assert.AreEqual(fax, retreivedHouses[0].Fax);

            // Verification of House # 2
            Assert.AreEqual(title2, retreivedHouses[1].Title);
            Assert.AreEqual(description2, retreivedHouses[1].Description);
            Assert.AreEqual(phoneNumber2, retreivedHouses[1].OwnerPhoneNumber);
            Assert.AreEqual(email2, retreivedHouses[1].OwnerEmail);
            Assert.AreEqual(rent2, retreivedHouses[1].RentPrice);
            Assert.AreEqual(numberOfBathrooms2, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(numberOfBedrooms2, retreivedHouses[1].NumberOfBedrooms);
            Assert.AreEqual(numberOfKitchens2, retreivedHouses[1].NumberOfKitchens);
            Assert.AreEqual(garage2, retreivedHouses[1].GarageAvailable);
            Assert.AreEqual(landlinePhoneAvailable2, retreivedHouses[1].LandlinePhoneAvailable);
            Assert.AreEqual(cable2, retreivedHouses[1].CableTvAvailable);
            Assert.AreEqual(internet2, retreivedHouses[1].InternetAvailable);
            Assert.AreEqual(smokingAllowed2, retreivedHouses[1].SmokingAllowed);
            Assert.AreEqual(propertyType2, retreivedHouses[1].PropertyType);
            Assert.AreEqual(latitude2, retreivedHouses[1].Latitude);
            Assert.AreEqual(longitude2, retreivedHouses[1].Longitude);
            Assert.AreEqual(houseNo2, retreivedHouses[1].HouseNo);
            Assert.AreEqual(area2, retreivedHouses[1].Area);
            Assert.AreEqual(streetNo2, retreivedHouses[1].StreetNo);
            Assert.AreEqual(phoneNumber2, retreivedHouses[1].OwnerPhoneNumber);
            Assert.AreEqual(dimension2.DimensionType, retreivedHouses[1].Dimension.DimensionType);
            Assert.AreEqual(dimension2.DecimalValue, retreivedHouses[1].Dimension.DecimalValue);
            Assert.AreEqual(dimension2.StringValue, retreivedHouses[1].Dimension.StringValue);
            Assert.AreEqual(ownerName2, retreivedHouses[1].OwnerName);
            Assert.AreEqual(genderRestriction2, retreivedHouses[1].GenderRestriction);
            Assert.AreEqual(rentUnit2, retreivedHouses[1].RentUnit);
            Assert.AreEqual(landlineNumber2, retreivedHouses[1].LandlineNumber);
            Assert.AreEqual(fax2, retreivedHouses[1].Fax);
        }
        
        #endregion Save and Search Houses By Area and PropertyType

        #region House Pagination Tests

        [Test]
        public void SearchAllHousesTest_TestsThatAllHOusesAreRetrievedAsExpected_VerifiesBYReturnValue() {
            // Save 4 properties, 2 houses and 2 apartments. 1 house and 1 apartment should come within the 
            // search radius and returned
            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();
            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService =
                _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();

            // Saving House # 1
            string area = "Rawalpindi, Punjab, Pakistan";
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(area);
            decimal latitude = coordinatesFromAddress.Item1;
            decimal longitude = coordinatesFromAddress.Item2;
            string houseNo = "S-123";
            string streetNo = "13";
            string title = "Special House";
            string description = "It was a Hobbit Hole. Which means it had good food and a warm hearth.";
            string phoneNumber = "01234567890";
            string email = "special@spsp123456.com";
            int numberOfBathrooms = 1;
            int numberOfBedrooms = 1;
            int numberOfKitchens = 1;
            int rent = 50000;
            string ownerName = "Owner Name 1";
            string propertyType = Constants.Apartment;
            bool isShared = false;
            string rentUnit = Constants.Hourly;
            string landlineNumber = "0510000000";
            string fax = "0510000000";
            bool cable = false;
            bool garage = false;
            bool landlinePhoneAvailable = false;
            bool smokingAllowed = false;
            bool internet = true;
            GenderRestriction genderRestriction = GenderRestriction.FamiliesOnly;
            House house = new House.HouseBuilder().Title(title).OwnerEmail(email)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberOfBathrooms).OwnerPhoneNumber(phoneNumber)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(cable)
                .GarageAvailable(garage).LandlinePhoneAvailable(landlinePhoneAvailable).SmokingAllowed(smokingAllowed)
                .WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(rent).Latitude(latitude)
                .Longitude(longitude).GenderRestriction(genderRestriction)
                .HouseNo(houseNo).Area(area).OwnerName(ownerName).StreetNo(streetNo).Description(description)
                .IsShared(isShared).RentUnit(rentUnit).LandlineNumber(landlineNumber).Fax(fax).Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, "5", 0, house);
            house.Dimension = dimension;
            houseRepository.SaveorUpdateDimension(dimension);
            houseRepository.SaveorUpdate(house);

            // Saving House # 2
            string area2 = "Islamabad, Islamabad Capital Territory, Pakistan";
            var coordinatesFromAddress2 = geocodingService.GetCoordinatesFromAddress(area2);
            decimal latitude2 = coordinatesFromAddress2.Item1;
            decimal longitude2 = coordinatesFromAddress2.Item2;
            string title2 = "Special House 2";
            string email2 = "special2@spsp123456-2.com";
            string description2 = "Erobor. Built deep within the mountain itself the beauty of this fortress was legend. 2";
            string houseNo2 = "S2-123";
            string streetNo2 = "2-13";
            string phoneNumber2 = "03990000002";
            int numberOfBathrooms2 = 2;
            int numberOfBedrooms2 = 2;
            int numberOfKitchens2 = 3;
            int rent2 = 100000;
            string ownerName2 = "Owner Name 2";
            string propertyType2 = Constants.House;
            bool isShared2 = true;
            string rentUnit2 = Constants.Weekly;
            string landlineNumber2 = "0510000002";
            string fax2 = "0510000002";
            bool cable2 = false;
            bool garage2 = true;
            bool landlinePhoneAvailable2 = true;
            bool smokingAllowed2 = false;
            bool internet2 = false;
            GenderRestriction genderRestriction2 = GenderRestriction.FamiliesOnly;
            House house2 = new House.HouseBuilder().Title(title2).OwnerEmail(email2)
                .NumberOfBedrooms(numberOfBedrooms2).NumberOfBathrooms(numberOfBathrooms2).OwnerPhoneNumber(phoneNumber2)
                .NumberOfKitchens(numberOfKitchens2).CableTvAvailable(cable2).GarageAvailable(garage2)
                .LandlinePhoneAvailable(landlinePhoneAvailable2).SmokingAllowed(smokingAllowed2).Fax(fax2)
                .WithInternetAvailable(internet2).HouseNo(houseNo2).PropertyType(propertyType2).RentPrice(rent2)
                .Latitude(latitude2).Longitude(longitude2).Area(area2).OwnerName(ownerName2).StreetNo(streetNo2)
                .Description(description2).IsShared(isShared2).RentUnit(rentUnit2).LandlineNumber(landlineNumber2)
                .GenderRestriction(genderRestriction2)
                .Build();
            Dimension dimension2 = new Dimension(DimensionType.Marla, "20", 0, house2);
            house2.Dimension = dimension2;
            houseRepository.SaveorUpdateDimension(dimension2);
            houseRepository.SaveorUpdate(house2);

            // Retrieve the Houses
            IList<House> retreivedHouses = houseRepository.GetAllHouses();
            Assert.AreEqual(2, retreivedHouses.Count);
            
            House verificationHouse1 = null;
            House verificationHouse2 = null;

            // There is no priority on which entity will be retrieved first, so we check which house came in first and 
            // assert in that order of houses
            if (retreivedHouses[0].Area.Equals(house.Area))
            {
                verificationHouse1 = house;
                verificationHouse2 = house2;
            }
            else
            {
                verificationHouse1 = house2;
                verificationHouse2 = house;
            }

            // Verification of House # 2
            Assert.AreEqual(verificationHouse1.Title, retreivedHouses[0].Title);
            Assert.AreEqual(verificationHouse1.Description, retreivedHouses[0].Description);
            Assert.AreEqual(verificationHouse1.OwnerPhoneNumber, retreivedHouses[0].OwnerPhoneNumber);
            Assert.AreEqual(verificationHouse1.OwnerEmail, retreivedHouses[0].OwnerEmail);
            Assert.AreEqual(verificationHouse1.RentPrice, retreivedHouses[0].RentPrice);
            Assert.AreEqual(verificationHouse1.NumberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(verificationHouse1.NumberOfBedrooms, retreivedHouses[0].NumberOfBedrooms);
            Assert.AreEqual(verificationHouse1.NumberOfKitchens, retreivedHouses[0].NumberOfKitchens);
            Assert.AreEqual(verificationHouse1.GarageAvailable, retreivedHouses[0].GarageAvailable);
            Assert.AreEqual(verificationHouse1.LandlinePhoneAvailable, retreivedHouses[0].LandlinePhoneAvailable);
            Assert.AreEqual(verificationHouse1.CableTvAvailable, retreivedHouses[0].CableTvAvailable);
            Assert.AreEqual(verificationHouse1.InternetAvailable, retreivedHouses[0].InternetAvailable);
            Assert.AreEqual(verificationHouse1.SmokingAllowed, retreivedHouses[0].SmokingAllowed);
            Assert.AreEqual(verificationHouse1.PropertyType, retreivedHouses[0].PropertyType);
            Assert.AreEqual(verificationHouse1.Latitude, retreivedHouses[0].Latitude);
            Assert.AreEqual(verificationHouse1.Longitude, retreivedHouses[0].Longitude);
            Assert.AreEqual(verificationHouse1.HouseNo, retreivedHouses[0].HouseNo);
            Assert.AreEqual(verificationHouse1.Area, retreivedHouses[0].Area);
            Assert.AreEqual(verificationHouse1.StreetNo, retreivedHouses[0].StreetNo);
            Assert.AreEqual(verificationHouse1.Dimension.DimensionType, retreivedHouses[0].Dimension.DimensionType);
            Assert.AreEqual(verificationHouse1.Dimension.DecimalValue, retreivedHouses[0].Dimension.DecimalValue);
            Assert.AreEqual(verificationHouse1.Dimension.StringValue, retreivedHouses[0].Dimension.StringValue);
            Assert.AreEqual(verificationHouse1.OwnerName, retreivedHouses[0].OwnerName);
            Assert.AreEqual(verificationHouse1.GenderRestriction, retreivedHouses[0].GenderRestriction);
            Assert.AreEqual(verificationHouse1.RentUnit, retreivedHouses[0].RentUnit);
            Assert.AreEqual(verificationHouse1.LandlineNumber, retreivedHouses[0].LandlineNumber);
            Assert.AreEqual(verificationHouse1.Fax, retreivedHouses[0].Fax);

            // Verification of House # 1
            Assert.AreEqual(verificationHouse2.Title, retreivedHouses[1].Title);
            Assert.AreEqual(verificationHouse2.Description, retreivedHouses[1].Description);
            Assert.AreEqual(verificationHouse2.OwnerPhoneNumber, retreivedHouses[1].OwnerPhoneNumber);
            Assert.AreEqual(verificationHouse2.OwnerEmail, retreivedHouses[1].OwnerEmail);
            Assert.AreEqual(verificationHouse2.RentPrice, retreivedHouses[1].RentPrice);
            Assert.AreEqual(verificationHouse2.NumberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(verificationHouse2.NumberOfBedrooms, retreivedHouses[1].NumberOfBedrooms);
            Assert.AreEqual(verificationHouse2.NumberOfKitchens, retreivedHouses[1].NumberOfKitchens);
            Assert.AreEqual(verificationHouse2.GarageAvailable, retreivedHouses[1].GarageAvailable);
            Assert.AreEqual(verificationHouse2.LandlinePhoneAvailable, retreivedHouses[1].LandlinePhoneAvailable);
            Assert.AreEqual(verificationHouse2.CableTvAvailable, retreivedHouses[1].CableTvAvailable);
            Assert.AreEqual(verificationHouse2.InternetAvailable, retreivedHouses[1].InternetAvailable);
            Assert.AreEqual(verificationHouse2.SmokingAllowed, retreivedHouses[1].SmokingAllowed);
            Assert.AreEqual(verificationHouse2.PropertyType, retreivedHouses[1].PropertyType);
            Assert.AreEqual(verificationHouse2.Latitude, retreivedHouses[1].Latitude);
            Assert.AreEqual(verificationHouse2.Longitude, retreivedHouses[1].Longitude);
            Assert.AreEqual(verificationHouse2.HouseNo, retreivedHouses[1].HouseNo);
            Assert.AreEqual(verificationHouse2.Area, retreivedHouses[1].Area);
            Assert.AreEqual(verificationHouse2.StreetNo, retreivedHouses[1].StreetNo);
            Assert.AreEqual(verificationHouse2.Dimension.DimensionType, retreivedHouses[1].Dimension.DimensionType);
            Assert.AreEqual(verificationHouse2.Dimension.DecimalValue, retreivedHouses[1].Dimension.DecimalValue);
            Assert.AreEqual(verificationHouse2.Dimension.StringValue, retreivedHouses[1].Dimension.StringValue);
            Assert.AreEqual(verificationHouse2.OwnerName, retreivedHouses[1].OwnerName);
            Assert.AreEqual(verificationHouse2.GenderRestriction, retreivedHouses[1].GenderRestriction);
            Assert.AreEqual(verificationHouse2.RentUnit, retreivedHouses[1].RentUnit);
            Assert.AreEqual(verificationHouse2.LandlineNumber, retreivedHouses[1].LandlineNumber);
            Assert.AreEqual(verificationHouse2.Fax, retreivedHouses[1].Fax);
        }

        [Test]
        public void RetreiveAllHousesPaginationTest_ChecksThatThePaginationIsWorkingFine_VerifiesThroughReturnedOutput()
        {
            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();

            // Save more than 10 houses using the same property type
            SaveMultipleHousesUsingGivenIterations(houseRepository, 21);

            // Now search by property type and check we only retreived 10 houses
            string searchedPropertyType = "House";
            var retreivedHouses = houseRepository.GetAllHouses();
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(10, retreivedHouses.Count);
        }

        [Test]
        public void RetreiveHousesByCoordinatesPaginationTest_ChecksThatThePaginationIsWorkingFine_VerifiesThroughReturnedOutput()
        {
            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();
            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService =
                _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();

            string searchArea = "Pindora, Rawalpindi, Pakistan";
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(searchArea);
            // Save more than 10 houses using the same property type
            SaveMultipleHousesUsingGivenIterations(houseRepository, 21, searchArea,
                coordinatesFromAddress.Item1, coordinatesFromAddress.Item2);

            // Now search by property type and check we only retreived 10 houses
            var retreivedHouses = houseRepository.SearchHousesByCoordinates(coordinatesFromAddress.Item1, 
                coordinatesFromAddress.Item2);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(10, retreivedHouses.Count);
        }

        [Test]
        public void RetreiveHousesByEmailPaginationTest_ChecksThatThePaginationIsWorkingFine_VerifiesThroughReturnedOutput()
        {
            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();
            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService =
                _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();

            string ownerEemail = "special@spsp123456-1.com";
            // Save more than 10 houses using the same property type
            SaveMultipleHousesUsingGivenIterations(houseRepository, 21, email: ownerEemail);
            
            var retreivedHouses = houseRepository.GetHouseByOwnerEmail(ownerEemail);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(10, retreivedHouses.Count);
        }

        #endregion House Pagination Tests

        #region Save and Search Houses by Email

        [Test]
        public void SaveHouseAndRetreiveByEmailTest_TestsThatHouseUInstancesAreSavedToTheDatabaseAsExpected_VerifiesThroughDatabaseQuery()
        {
            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();

            // House # 1: Same as the searched email
            string email = "w@12344321.com";
            string description = "It was a Hobbit Hole. Which means it had good food and a warm hearth.";
            string title = "MGM Grand";
            string phoneNumber = "01234567891";

            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long price = 90000;
            string ownerName = "Owner Name 1";
            string propertyType = Constants.House;
            string area = "Pindora, Rawalpindi, Punjab, Pakistan";

            House house = new House.HouseBuilder().Title(title).OwnerPhoneNumber(phoneNumber).OwnerEmail(email)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(propertyType).RentPrice(price).Latitude(33.29M).Longitude(73.41M)
                .Area(area).OwnerName(ownerName).Description(description).Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, "50", 0, house);
            house.Dimension = dimension;
            houseRepository.SaveorUpdateDimension(dimension);
            houseRepository.SaveorUpdate(house);

            // House # 2: Same as the searched email
            string email2 = "w@12344321.com";
            string description2 = "It was a Hobbit Hole. Which means it had good food and a warm hearth. 2";
            string title2 = "MGM Grand 2";
            string phoneNumber2 = "03990000001";
            
            long price2 = 90000;
            string ownerName2 = "Owner Name 2";
            string propertyType2 = Constants.Apartment;
            string area2 = "Lahore, Punjab, Pakistan";

            House house2 = new House.HouseBuilder().Title(title2).OwnerPhoneNumber(phoneNumber2).OwnerEmail(email2)                
                .PropertyType(propertyType2).RentPrice(price).Latitude(33.29M).Longitude(73.41M).Area(area)
                .OwnerName(ownerName2).Description(description2).Build();
            
            houseRepository.SaveorUpdate(house2);

            // House # 3: Different from the searched email
            string email3 = "w@12344321-2.com";
            string description3 = "It was a Hobbit Hole. Which means it had good food and a warm hearth. 3";
            string title3 = "MGM Grand 3";
            string phoneNumber3 = "03990000001";
            
            long price3 = 90000;
            string ownerName3 = "Owner Name 3";
            string propertyType3 = Constants.Apartment;
            string area3 = "Karachi, Sindh, Pakistan";

            House house3 = new House.HouseBuilder().Title(title3).OwnerPhoneNumber(phoneNumber3).OwnerEmail(email3)
                .PropertyType(propertyType3).RentPrice(price).Latitude(33.39M).Longitude(73.41M).Area(area3)
                .OwnerName(ownerName3).Description(description3).Build();

            houseRepository.SaveorUpdate(house3);

            // Retrieve the Houses by email
            IList<Domain.Model.PropertyAggregate.Property> retreivedHouses =  houseRepository.GetHouseByOwnerEmail(email);
            Assert.AreEqual(2, retreivedHouses.Count);
            House retreivedHouse = (House)retreivedHouses[0];

            // Verfication of House # 1
            Assert.AreEqual(title, retreivedHouse.Title);
            Assert.AreEqual(house.Title, retreivedHouse.Title);
            Assert.AreEqual(description, retreivedHouse.Description);
            Assert.AreEqual(house.Description, retreivedHouse.Description);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouse.NumberOfBedrooms);
            Assert.AreEqual(house.NumberOfKitchens, retreivedHouse.NumberOfKitchens);
            Assert.AreEqual(house.GarageAvailable, retreivedHouse.LandlinePhoneAvailable);
            Assert.AreEqual(house.SmokingAllowed, retreivedHouse.SmokingAllowed);
            Assert.AreEqual(house.InternetAvailable, retreivedHouse.InternetAvailable);
            Assert.AreEqual(house.PropertyType, retreivedHouse.PropertyType);
            Assert.AreEqual(house.Latitude, retreivedHouse.Latitude);
            Assert.AreEqual(house.Longitude, retreivedHouse.Longitude);
            Assert.AreEqual(house.HouseNo, retreivedHouse.HouseNo);
            Assert.AreEqual(house.Area, retreivedHouse.Area);
            Assert.AreEqual(house.StreetNo, retreivedHouse.StreetNo);
            Assert.AreEqual(house.Dimension.DimensionType, retreivedHouse.Dimension.DimensionType);
            Assert.AreEqual(house.Dimension.DecimalValue, retreivedHouse.Dimension.DecimalValue);
            Assert.AreEqual(house.Dimension.StringValue, retreivedHouse.Dimension.StringValue);
            Assert.AreEqual(house.OwnerName, retreivedHouse.OwnerName);

            // Verfication of House # 2
            retreivedHouse = (House)retreivedHouses[1];
            Assert.AreEqual(house2.Title, retreivedHouse.Title);
            Assert.AreEqual(house2.Description, retreivedHouse.Description);
            Assert.AreEqual(house2.PropertyType, retreivedHouse.PropertyType);
            Assert.AreEqual(house2.Latitude, retreivedHouse.Latitude);
            Assert.AreEqual(house2.Longitude, retreivedHouse.Longitude);
            Assert.AreEqual(house2.StreetNo, retreivedHouse.StreetNo);
            Assert.AreEqual(house2.OwnerName, retreivedHouse.OwnerName);

            // Retrieve the Apartment by Owner Email
            /*retreivedHouses = houseRepository.GetApartmentByOwnerEmail(email);
            Assert.AreEqual(1, retreivedHouses.Count);
            retreivedHouse = retreivedHouses[0];

            */
        }

        #endregion Save and Search Houses by Email

        #region Save and Search House Images

        [Category("Integration")]
        [Test]
        public void SaveImagesToHouse_ChecksThatAfterAddingImagesHouseIsSavedAsExpected_VerifiesByRetrievingAfterSaving()
        {
            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();
            string email = "w@12344321.com";
            string title = "MGM Grand";
            string phoneNumber = "01234567890";

            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long price = 90000;
            string ownerName = "Owner Name 1";
            string propertyType = "Apartment";

            House house = new House.HouseBuilder().Title(title).OwnerEmail(email).OwnerPhoneNumber(phoneNumber)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(propertyType).RentPrice(price).Latitude(33.29M).Longitude(73.41M)
                .HouseNo("123").Area("Pindora").StreetNo("13").OwnerName(ownerName).Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, null, 5, house);
            house.Dimension = dimension;

            houseRepository.SaveorUpdateDimension(dimension);
            houseRepository.SaveorUpdate(house);

            var image1 = "123";
            var image2 = "1234";
            var image3 = "12345";
            house.AddImage(image1);
            house.AddImage(image2);
            house.AddImage(image3);

            houseRepository.SaveorUpdate(house);

            House retreivedHouse = (House)houseRepository.GetPropertyById(house.Id);
            Assert.IsNotNull(retreivedHouse);
            Assert.AreEqual(3, retreivedHouse.Images.Count);

            Assert.AreEqual(title, retreivedHouse.Title);
            Assert.AreEqual(phoneNumber, retreivedHouse.OwnerPhoneNumber);
            Assert.AreEqual(email, retreivedHouse.OwnerEmail);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouse.NumberOfBedrooms);
            Assert.AreEqual(house.NumberOfKitchens, retreivedHouse.NumberOfKitchens);
            Assert.AreEqual(house.GarageAvailable, retreivedHouse.LandlinePhoneAvailable);
            Assert.AreEqual(house.SmokingAllowed, retreivedHouse.SmokingAllowed);
            Assert.AreEqual(house.InternetAvailable, retreivedHouse.InternetAvailable);
            Assert.AreEqual(house.PropertyType, retreivedHouse.PropertyType);
            Assert.AreEqual(house.Latitude, retreivedHouse.Latitude);
            Assert.AreEqual(house.Longitude, retreivedHouse.Longitude);
            Assert.AreEqual(house.HouseNo, retreivedHouse.HouseNo);
            Assert.AreEqual(house.Area, retreivedHouse.Area);
            Assert.AreEqual(house.StreetNo, retreivedHouse.StreetNo);
            Assert.AreEqual(house.Dimension.DimensionType, retreivedHouse.Dimension.DimensionType);
            Assert.AreEqual(house.Dimension.DecimalValue, retreivedHouse.Dimension.DecimalValue);
            Assert.AreEqual(house.Dimension.StringValue, retreivedHouse.Dimension.StringValue);
            Assert.AreEqual(house.OwnerName, retreivedHouse.OwnerName);
            Assert.AreEqual(house.IsShared, retreivedHouse.IsShared);
            Assert.AreEqual(house.RentUnit, retreivedHouse.RentUnit);

            Assert.AreEqual(image1, retreivedHouse.Images[0]);
            Assert.AreEqual(image2, retreivedHouse.Images[1]);
            Assert.AreEqual(image3, retreivedHouse.Images[2]);
        }

        #endregion Save and Search House Images
        
        #region Utility methods

        /// <summary>
        /// We can use this method to create data for pagination tests. Values are simple and repetitive
        /// </summary>
        /// <param name="houseRepository"></param>
        /// <param name="numberOfIterations"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="email"></param>
        private void SaveMultipleHousesUsingGivenIterations(IResidentialPropertyRepository houseRepository, 
            int numberOfIterations, string area = "Pindora, Rawalpindi, Pakistan", decimal latitude = 25.74M, 
            decimal longitude = 73.91M, string email = "special@spsp123456-1.com")
        {
            for (int i = 0; i < numberOfIterations; i++)
            {
                string description = "It was a Hobbit Hole. Which means it had good food and a warm hearth.";
                string houseNo = "House # 1";
                string streetNo = "1";
                string title = "Title # 1";
                string phoneNumber = "01234567890";
                int numberOfBathrooms = 1;
                int numberOfBedrooms = 1;
                int numberOfKitchens = 1;
                int rent = 100;
                string ownerName = "Owner Name 1";
                string propertyType = "House";
                House house = new House.HouseBuilder().Title(title).OwnerEmail(email)
                    .NumberOfBedrooms(numberOfBedrooms)
                    .NumberOfBathrooms(numberOfBathrooms)
                    .OwnerPhoneNumber(phoneNumber)
                    .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
                    .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(true).WithInternetAvailable(true)
                    .PropertyType(propertyType).RentPrice(rent).Latitude(latitude)
                    .Longitude(longitude)
                    .HouseNo(houseNo)
                    .Area(area)
                    .StreetNo(streetNo)
                    .OwnerName(ownerName)
                    .Description(description)
                    .Build();
                Dimension dimension = new Dimension(DimensionType.Kanal, "1", 0, house);
                house.Dimension = dimension;
                houseRepository.SaveorUpdateDimension(dimension);
                houseRepository.SaveorUpdate(house);
            }
        }
        
        private void VerifyRetereivedHouses(IList<House> retreivedHouses, decimal latitude, decimal longitude)
        {
            decimal initialLatitude = latitude;
            decimal initialLongitude = longitude;
            for (int i = 0; i < retreivedHouses.Count; i++)
            {
                initialLatitude += _latitudeIncrementForMultipleHouseSaves;
                initialLongitude += _longitudeIncrementForMultipleHouseSaves;
                string title = "MGM Grand" + i;
                string description = "It was a Hobbit Hole " + i + ". Which means it had good food and a warm hearth.";
                string phoneNumber =null;
                if (i < 10)
                {
                    phoneNumber = "0123456789" + i;
                }
                else if (i >= 10)
                {
                    phoneNumber = "012345678" + i;
                }
                string email = "dummy@dumdum123456-" + i + ".com";
                string houseNo = "123" + i;
                string area = "Harley Street" + i;
                string streetNo = i.ToString();
                string ownerName = "Owner Name " + i;

                DimensionType dimensionType = DimensionType.Kanal;

                Assert.AreEqual(initialLatitude, retreivedHouses[i].Latitude);
                Assert.AreEqual(initialLongitude, retreivedHouses[i].Longitude);
                Assert.AreEqual(title, retreivedHouses[i].Title);
                Assert.AreEqual(description, retreivedHouses[i].Description);
                Assert.AreEqual(phoneNumber, retreivedHouses[i].OwnerPhoneNumber);
                Assert.AreEqual(email, retreivedHouses[i].OwnerEmail);
                Assert.AreEqual(i, retreivedHouses[i].NumberOfBathrooms);
                Assert.AreEqual(i, retreivedHouses[i].NumberOfBathrooms);
                Assert.AreEqual(i, retreivedHouses[i].NumberOfBedrooms);
                Assert.AreEqual(i, retreivedHouses[i].NumberOfKitchens);
                Assert.AreEqual(true, retreivedHouses[i].LandlinePhoneAvailable);
                Assert.AreEqual(false, retreivedHouses[i].SmokingAllowed);
                Assert.AreEqual(true, retreivedHouses[i].InternetAvailable);
                Assert.AreEqual(i % 2 == 0 ? "House" : "Apartment", retreivedHouses[i].PropertyType);

                Assert.AreEqual(houseNo, retreivedHouses[i].HouseNo);
                Assert.AreEqual(area, retreivedHouses[i].Area);
                Assert.AreEqual(streetNo, retreivedHouses[i].StreetNo);
                Assert.AreEqual(dimensionType, retreivedHouses[i].Dimension.DimensionType);
                Assert.AreEqual(i.ToString(), retreivedHouses[i].Dimension.StringValue);
                Assert.AreEqual(i.ToString(), retreivedHouses[i].Dimension.StringValue);
                Assert.AreEqual(ownerName, retreivedHouses[i].OwnerName);
            }
        }

        #endregion Utility Methods
    }
}