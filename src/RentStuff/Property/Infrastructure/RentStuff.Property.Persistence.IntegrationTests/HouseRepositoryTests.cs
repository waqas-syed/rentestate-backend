using Ninject;
using NUnit.Framework;
using RentStuff.Common.Ninject.Modules;
using RentStuff.Common.Utilities;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Domain.Model.PropertyAggregate;
using RentStuff.Property.Infrastructure.Persistence.Ninject.Modules;
using System;
using System.Collections.Generic;

//using Spring.Context.Support;

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
            string landlineNumbe = "0510000000";
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

            // Defaut value of Month should be used
            string rentUnit = "Month";
            Assert.AreEqual(rentUnit, retreivedHouse.RentUnit);
            Assert.AreEqual(house.LastModified, retreivedHouse.LastModified);
            Assert.AreEqual(landlineNumbe, retreivedHouse.LandlineNumber);
            Assert.AreEqual(fax, retreivedHouse.Fax);
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

            House house = new House.HouseBuilder().Title(title).OwnerEmail(email).OwnerPhoneNumber(phoneNumber)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(propertyType).RentPrice(price).Latitude(latitude).Longitude(longitude)
                .HouseNo(houseNo).Area(area).StreetNo(streetNo).OwnerName(ownerName).Description(description)
                .GenderRestriction(genderRestriction).IsShared(isShared).LandlineNumber(landlineNumber).Fax(fax)
                .Build();
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

            Dimension dimension2 = new Dimension(DimensionType.Marla, "20", 0, house);
            house.UpdateHouse(title2, monthlyRent2, numberOfBedrooms2, numberOfKitchens2, numberofBathrooms2, internet2, landline2,
                cableTv2, dimension2, garage2, smokingAllowed2, propertyType2, email2, phoneNumber2, null, null, area2, name2,
                description2, genderRestriction2, latitude2, longitude2, isShared2, rentUnit2, landlineNumber2, fax2);
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

        #region Save and Search Houses By Area and PropertyType

        [Test]
        [Category("Integration")]
        public void SearchHouseByCoordinatesAndPropertyTypeApartment_ChecksIfTheRepositoryCallReturnsTheExpectedResult_VerifiesByReturnValue()
        {
            // Save two houses with type House & Apartment. Search for houses with type Apartment only. Only the house
            // with type 'Apartment' should be retreived

            string area = "Pindora, Rawalpindi, Pakistan";

            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();
            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService = 
                _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(area);

            // Saving House # 1
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
            string propertyType = "Apartment";
            bool isShared = false;
            string rentUnit = "Hour";
            GenderRestriction genderRestriction = GenderRestriction.FamiliesOnly;
            House house = new House.HouseBuilder().Title(title).OwnerEmail(email)
            .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberOfBathrooms).OwnerPhoneNumber(phoneNumber)
            .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
            .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(true).WithInternetAvailable(true)
            .PropertyType(propertyType).RentPrice(rent).Latitude(coordinatesFromAddress.Item1)
            .Longitude(coordinatesFromAddress.Item2).GenderRestriction(genderRestriction)
            .HouseNo(houseNo).Area(area).OwnerName(ownerName).StreetNo(streetNo).Description(description)
            .IsShared(isShared).RentUnit(rentUnit).Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, "5", 0, house);
            house.Dimension = dimension;
            houseRepository.SaveorUpdateDimension(dimension);
            houseRepository.SaveorUpdate(house);

            // Saving House # 2
            string email2 = "special2@spsp123456.com";
            string description2 = "Erobor. Built deep within the mountain itself the beauty of this fortress was legend.";
            string houseNo2 = "S2-123";
            string streetNo2 = "2-13";
            string phoneNumber2 = "01234567891";
            int numberOfBathrooms2 = 2;
            int numberOfBedrooms2 = 2;
            int numberOfKitchens2 = 3;
            int rent2 = 100000;
            string ownerName2 = "Owner Name 2";
            string propertyType2 = "House";
            bool isShared2 = true;
            string rentUnit2 = "Day";
            GenderRestriction genderRestriction2 = GenderRestriction.FamiliesOnly;
            House house2 = new House.HouseBuilder().Title(title).OwnerEmail(email2)
            .NumberOfBedrooms(numberOfBedrooms2).NumberOfBathrooms(numberOfBathrooms2).OwnerPhoneNumber(phoneNumber2)
            .NumberOfKitchens(numberOfKitchens2).CableTvAvailable(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType2).RentPrice(rent2).Latitude(coordinatesFromAddress.Item1)
            .Longitude(coordinatesFromAddress.Item2).GenderRestriction(genderRestriction2)
            .HouseNo(houseNo2).Area(area).OwnerName(ownerName2).StreetNo(streetNo2).Description(description2).IsShared(isShared2)
            .RentUnit(rentUnit2)
            .Build();
            Dimension dimension2 = new Dimension(DimensionType.Marla, "20", 0, house2);
            house2.Dimension = dimension2;
            houseRepository.SaveorUpdateDimension(dimension2);
            houseRepository.SaveorUpdate(house2);

            var retreivedHouses = houseRepository.SearchHousesByCoordinatesAndPropertyType(coordinatesFromAddress.Item1,
                coordinatesFromAddress.Item2, propertyType);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(1, retreivedHouses.Count);

            // Verification of House # 1
            Assert.AreEqual(title, retreivedHouses[0].Title);
            Assert.AreEqual(house.Title, retreivedHouses[0].Title);
            Assert.AreEqual(description, retreivedHouses[0].Description);
            Assert.AreEqual(house.Description, retreivedHouses[0].Description);
            Assert.AreEqual(phoneNumber, retreivedHouses[0].OwnerPhoneNumber);
            Assert.AreEqual(email, retreivedHouses[0].OwnerEmail);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouses[0].NumberOfBedrooms);
            Assert.AreEqual(house.NumberOfKitchens, retreivedHouses[0].NumberOfKitchens);
            Assert.AreEqual(house.GarageAvailable, retreivedHouses[0].LandlinePhoneAvailable);
            Assert.AreEqual(house.SmokingAllowed, retreivedHouses[0].SmokingAllowed);
            Assert.AreEqual(house.InternetAvailable, retreivedHouses[0].InternetAvailable);
            Assert.AreEqual(house.PropertyType, retreivedHouses[0].PropertyType);
            Assert.AreEqual(house.Latitude, retreivedHouses[0].Latitude);
            Assert.AreEqual(house.Longitude, retreivedHouses[0].Longitude);
            Assert.AreEqual(house.HouseNo, retreivedHouses[0].HouseNo);
            Assert.AreEqual(house.Area, retreivedHouses[0].Area);
            Assert.AreEqual(house.StreetNo, retreivedHouses[0].StreetNo);
            Assert.AreEqual(email, house.OwnerEmail);
            Assert.AreEqual(house.OwnerEmail, retreivedHouses[0].OwnerEmail);
            Assert.AreEqual(house.OwnerPhoneNumber, retreivedHouses[0].OwnerPhoneNumber);
            Assert.AreEqual(dimension.DimensionType, retreivedHouses[0].Dimension.DimensionType);
            Assert.AreEqual(dimension.DecimalValue, retreivedHouses[0].Dimension.DecimalValue);
            Assert.AreEqual(dimension.StringValue, retreivedHouses[0].Dimension.StringValue);
            Assert.AreEqual(house.OwnerName, retreivedHouses[0].OwnerName);
            Assert.AreEqual(house.GenderRestriction, retreivedHouses[0].GenderRestriction);
            Assert.AreEqual(house.RentUnit, retreivedHouses[0].RentUnit);
        }

        [Test]
        [Category("Integration")]
        public void SearchHouseByCoordinatesAndPropertyTypeHouse_ChecksIfTheRepositoryCallReturnsTheExpectedResult_VerifiesByReturnValue()
        {
            // Save two houses with type House & Apartment. Search for houses with type House only. Only the house
            // with type 'House' should be retreived

            string area = "Pindora, Rawalpindi, Pakistan";

            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();
            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService = 
                _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(area);

            // Saving House # 1
            string houseNo = "S-123";
            string streetNo = "13";
            string description = "It was a Hobbit Hole. Which means it had good food and a warm hearth.";
            string title = "Special House";
            string phoneNumber = "01234567890";
            string email = "special@spsp123456.com";
            int numberOfBathrooms = 1;
            int numberOfBedrooms = 1;
            int numberOfKitchens = 1;
            int rent = 50000;
            string ownerName = "Owner Name 1";
            string propertyType = "Apartment";
            string rentUnit = "Week";
            GenderRestriction genderRestriction = GenderRestriction.FamiliesOnly;

            House house = new House.HouseBuilder().Title(title).OwnerEmail(email)
            .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberOfBathrooms).OwnerPhoneNumber(phoneNumber)
            .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
            .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(true).WithInternetAvailable(true)
            .PropertyType(propertyType).RentPrice(rent).Latitude(coordinatesFromAddress.Item1)
            .Longitude(coordinatesFromAddress.Item2).GenderRestriction(genderRestriction)
            .HouseNo(houseNo).Area(area).StreetNo(streetNo).OwnerName(ownerName).Description(description).RentUnit(rentUnit).Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, "5", 0, house);
            house.Dimension = dimension;
            houseRepository.SaveorUpdateDimension(dimension);
            houseRepository.SaveorUpdate(house);

            // Saving House # 2
            string title2 = "Title # 2";
            string description2 = "Erobor. Built deep within the mountain itself the beauty of this place was legend.";
            string email2 = "special2@spsp123456.com";
            string houseNo2 = "S2-123";
            string streetNo2 = "2-13";
            string phoneNumber2 = "01234567891";
            int numberOfBathrooms2 = 2;
            int numberOfBedrooms2 = 2;
            int numberOfKitchens2 = 3;
            int rent2 = 100000;
            string ownerName2 = "Owner Name 2";
            string propertyType2 = "House";
            string rentUnit2 = "Hour";
            GenderRestriction genderRestriction2 = GenderRestriction.FamiliesOnly;
            House house2 = new House.HouseBuilder().Title(title2).OwnerEmail(email2)
            .NumberOfBedrooms(numberOfBedrooms2).NumberOfBathrooms(numberOfBathrooms2).OwnerPhoneNumber(phoneNumber2)
            .NumberOfKitchens(numberOfKitchens2).CableTvAvailable(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType2).RentPrice(rent2).Latitude(coordinatesFromAddress.Item1)
            .Longitude(coordinatesFromAddress.Item2).GenderRestriction(genderRestriction2)
            .HouseNo(houseNo2).Area(area).StreetNo(streetNo2).OwnerName(ownerName2).Description(description2).RentUnit(rentUnit2).Build();
            Dimension dimension2 = new Dimension(DimensionType.Marla, "20", 0, house2);
            house2.Dimension = dimension2;
            houseRepository.SaveorUpdateDimension(dimension2);
            houseRepository.SaveorUpdate(house2);

            var retreivedHouses = houseRepository.SearchHousesByCoordinatesAndPropertyType(coordinatesFromAddress.Item1,
                coordinatesFromAddress.Item2, propertyType2);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(1, retreivedHouses.Count);

            // Verification of House # 2
            Assert.AreEqual(title2, retreivedHouses[0].Title);
            Assert.AreEqual(house2.Title, retreivedHouses[0].Title);
            Assert.AreEqual(description2, retreivedHouses[0].Description);
            Assert.AreEqual(house2.Description, retreivedHouses[0].Description);
            Assert.AreEqual(house2.NumberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(house2.NumberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(house2.NumberOfBedrooms, retreivedHouses[0].NumberOfBedrooms);
            Assert.AreEqual(house2.NumberOfKitchens, retreivedHouses[0].NumberOfKitchens);
            Assert.AreEqual(house2.GarageAvailable, retreivedHouses[0].LandlinePhoneAvailable);
            Assert.AreEqual(house2.SmokingAllowed, retreivedHouses[0].SmokingAllowed);
            Assert.AreEqual(house2.InternetAvailable, retreivedHouses[0].InternetAvailable);
            Assert.AreEqual(house2.PropertyType, retreivedHouses[0].PropertyType);
            Assert.AreEqual(house2.Latitude, retreivedHouses[0].Latitude);
            Assert.AreEqual(house2.Longitude, retreivedHouses[0].Longitude);
            Assert.AreEqual(house2.HouseNo, retreivedHouses[0].HouseNo);
            Assert.AreEqual(house2.Area, retreivedHouses[0].Area);
            Assert.AreEqual(house2.StreetNo, retreivedHouses[0].StreetNo);
            Assert.AreEqual(email2, house2.OwnerEmail);
            Assert.AreEqual(house2.OwnerEmail, retreivedHouses[0].OwnerEmail);
            Assert.AreEqual(house2.OwnerPhoneNumber, retreivedHouses[0].OwnerPhoneNumber);
            Assert.AreEqual(dimension2.DimensionType, retreivedHouses[0].Dimension.DimensionType);
            Assert.AreEqual(dimension2.DecimalValue, retreivedHouses[0].Dimension.DecimalValue);
            Assert.AreEqual(dimension2.StringValue, retreivedHouses[0].Dimension.StringValue);
            Assert.AreEqual(house2.OwnerName, retreivedHouses[0].OwnerName);
            Assert.AreEqual(house2.GenderRestriction, retreivedHouses[0].GenderRestriction);
            Assert.AreEqual(house2.RentUnit, retreivedHouses[0].RentUnit);
        }

        [Test]
        [Category("Integration")]
        public void SearchMultipleHousesByCoordinatesAndPropertyType_ChecksIfNearbyHousesAreReturned_VerifiesByReturnValue()
        {
            // Save 3 houses in locations nearby, 2 have same PropertyType but 1 has different
            // Save 2 houses that are in other places. 
            // Search should get the 2 houses located near the serched location and also the exact PropertyType

            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();
            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService = 
                _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();

            // Saving House # 1: Should be in the search results
            string area = "Pindora, Rawalpindi, Pakistan";
            string description = "It was a Hobbit Hole. Which means it had good food and a warm hearth.";
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(area);
            string houseNo = "House # 1";
            string streetNo = "1";
            string title = "Title # 1";
            string phoneNumber = "01234567890";
            string email = "special@spsp123456-1.com";
            int numberOfBathrooms = 1;
            int numberOfBedrooms = 1;
            int numberOfKitchens = 1;
            int rent = 100;
            string ownerName = "Owner Name 1";
            string propertyType = "House";
            string rentUnit = "Month";
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
            House house = new House.HouseBuilder().Title(title).OwnerEmail(email)
            .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberOfBathrooms).OwnerPhoneNumber(phoneNumber)
            .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
            .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(true).WithInternetAvailable(true)
            .PropertyType(propertyType).RentPrice(rent).Latitude(coordinatesFromAddress.Item1)
            .Longitude(coordinatesFromAddress.Item2).GenderRestriction(genderRestriction)
            .HouseNo(houseNo).Area(area).StreetNo(streetNo).OwnerName(ownerName).Description(description).RentUnit(rentUnit).Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, "1", 0, house);
            house.Dimension = dimension;
            houseRepository.SaveorUpdateDimension(dimension);
            houseRepository.SaveorUpdate(house);

            // Saving House # 2: Should NOT be in the search results, different property type
            string title2 = "Title # 2";
            string area2 = "Satellite Town, Rawalpindi, Pakistan";
            string description2 = "It was a Hobbit Hole 2. Which means it had good food and a warm hearth.";
            var coordinatesFromAddress2 = geocodingService.GetCoordinatesFromAddress(area2);
            string email2 = "special2@spsp12345-2.com";
            string houseNo2 = "House # 2";
            string streetNo2 = "2";
            string phoneNumber2 = "01234567891";
            int numberOfBathrooms2 = 2;
            int numberOfBedrooms2 = 2;
            int numberOfKitchens2 = 2;
            int rent2 = 200;
            string ownerName2 = "Owner Name 2";
            string propertyType2 = "Apartment";
            string rentUnit2 = "Month";
            GenderRestriction genderRestriction2 = GenderRestriction.FamiliesOnly;

            House house2 = new House.HouseBuilder().Title(title2).OwnerEmail(email2)
            .NumberOfBedrooms(numberOfBedrooms2).NumberOfBathrooms(numberOfBathrooms2).OwnerPhoneNumber(phoneNumber2)
            .NumberOfKitchens(numberOfKitchens2).CableTvAvailable(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType2).RentPrice(rent2).Latitude(coordinatesFromAddress2.Item1)
            .Longitude(coordinatesFromAddress2.Item2).GenderRestriction(genderRestriction2)
            .HouseNo(houseNo2).Area(area2).StreetNo(streetNo2).OwnerName(ownerName2).Description(description2).RentUnit(rentUnit2)
            .Build();
            Dimension dimension2 = new Dimension(DimensionType.Kanal, "2", 0, house2);
            house2.Dimension = dimension2;
            houseRepository.SaveorUpdateDimension(dimension2);
            houseRepository.SaveorUpdate(house2);

            // Saving House # 3: Should NOT be in the search results, outside bounds of search location
            string area3 = "Nara, Punjab, Pakistan";
            var coordinatesFromAddress3 = geocodingService.GetCoordinatesFromAddress(area3);
            string title3 = "Title # 3";
            string description3 = "It was a Hobbit Hole 3. Which means it had good food and a warm hearth.";
            string email3 = "special2@spsp123456-3.com";
            string houseNo3 = "House # 3";
            string streetNo3 = "3";
            string phoneNumber3 = "01234567892";
            int numberOfBathrooms3 = 3;
            int numberOfBedrooms3 = 3;
            int numberOfKitchens3 = 3;
            int rent3 = 300;
            string ownerName3 = "Owner Name 3";
            string propertyType3 = "House";
            string rentUnit3 = "Month";
            GenderRestriction genderRestriction3 = GenderRestriction.BoysOnly;

            House house3 = new House.HouseBuilder().Title(title3).OwnerEmail(email3)
            .NumberOfBedrooms(numberOfBedrooms3).NumberOfBathrooms(numberOfBathrooms3).OwnerPhoneNumber(phoneNumber3)
            .NumberOfKitchens(numberOfKitchens3).CableTvAvailable(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType3).RentPrice(rent3).Latitude(coordinatesFromAddress3.Item1)
            .Longitude(coordinatesFromAddress3.Item2).GenderRestriction(genderRestriction3)
            .HouseNo(houseNo3).Area(area3).StreetNo(streetNo3).OwnerName(ownerName3).Description(description3)
            .RentUnit(rentUnit3).Build();
            Dimension dimension3 = new Dimension(DimensionType.Kanal, "3", 0, house3);
            house3.Dimension = dimension3;
            houseRepository.SaveorUpdateDimension(dimension3);
            houseRepository.SaveorUpdate(house3);

            // Saving House # 4: Should be in the search results
            string area4 = "I-9, Islamabad, Pakistan";
            var coordinatesFromAddress4 = geocodingService.GetCoordinatesFromAddress(area4);
            string title4 = "Title # 4";
            string description4 = "It was a Hobbit Hole 4. Which means it had good food and a warm hearth.";
            string email4 = "special2@spsp123456-4.com";
            string houseNo4 = "House # 4";
            string streetNo4 = "4";
            string phoneNumber4 = "01234567893";
            int numberOfBathrooms4 = 4;
            int numberOfBedrooms4 = 4;
            int numberOfKitchens4 = 4;
            int rent4 = 400;
            string ownerName4 = "Owner Name 4";
            string propertyType4 = "House";
            string rentUnit4 = "Month";
            GenderRestriction genderRestriction4 = GenderRestriction.GirlsOnly;

            House house4 = new House.HouseBuilder().Title(title4).OwnerEmail(email4)
            .NumberOfBedrooms(numberOfBedrooms4).NumberOfBathrooms(numberOfBathrooms4).OwnerPhoneNumber(phoneNumber4)
            .NumberOfKitchens(numberOfKitchens4).CableTvAvailable(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType4).RentPrice(rent4).Latitude(coordinatesFromAddress4.Item1)
            .Longitude(coordinatesFromAddress4.Item2).GenderRestriction(genderRestriction4)
            .HouseNo(houseNo4).Area(area4).StreetNo(streetNo4).OwnerName(ownerName4).Description(description4).RentUnit(rentUnit4)
            .Build();
            Dimension dimension4 = new Dimension(DimensionType.Kanal, "4", 0, house4);
            house4.Dimension = dimension4;
            houseRepository.SaveorUpdateDimension(dimension4);
            houseRepository.SaveorUpdate(house4);

            // Saving House # 5: Should NOT be in the search results, outside bounds of search location
            string area5 = "Qurtaba City, Punjab, Pakistan";
            var coordinatesFromAddress5 = geocodingService.GetCoordinatesFromAddress(area5);
            string title5 = "Title # 5";
            string description5 = "It was a Hobbit Hole 5. Which means it had good food and a warm hearth.";
            string email5 = "special2@spsp123456-5.com";
            string houseNo5 = "House # 5";
            string streetNo5 = "5";
            string phoneNumber5 = "01234567895";
            int numberOfBathrooms5 = 5;
            int numberOfBedrooms5 = 5;
            int numberOfKitchens5 = 5;
            int rent5 = 500;
            string ownerName5 = "Owner Name 5";
            string propertyType5 = "House";
            string rentUnit5 = "Month";
            GenderRestriction genderRestriction5 = GenderRestriction.GirlsOnly;

            House house5 = new House.HouseBuilder().Title(title5).OwnerEmail(email5)
            .NumberOfBedrooms(numberOfBedrooms5).NumberOfBathrooms(numberOfBathrooms5).OwnerPhoneNumber(phoneNumber5)
            .NumberOfKitchens(numberOfKitchens5).CableTvAvailable(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType5).RentPrice(rent5).Latitude(coordinatesFromAddress5.Item1)
            .Longitude(coordinatesFromAddress5.Item2).GenderRestriction(genderRestriction5)
            .HouseNo(houseNo5).Area(area5).StreetNo(streetNo5).OwnerName(ownerName5).Description(description5).RentUnit(rentUnit5)
            .Build();
            Dimension dimension5 = new Dimension(DimensionType.Kanal, "5", 0, house5);
            house5.Dimension = dimension5;
            houseRepository.SaveorUpdateDimension(dimension5);
            houseRepository.SaveorUpdate(house5);

            var retreivedHouses = houseRepository.SearchHousesByCoordinatesAndPropertyType(coordinatesFromAddress.Item1,
                coordinatesFromAddress.Item2, propertyType);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(2, retreivedHouses.Count);

            // Verification of House # 1
            Assert.AreEqual(title, retreivedHouses[0].Title);
            Assert.AreEqual(house.Title, retreivedHouses[0].Title);
            Assert.AreEqual(description, retreivedHouses[0].Description);
            Assert.AreEqual(house.Description, retreivedHouses[0].Description);
            Assert.AreEqual(phoneNumber, retreivedHouses[0].OwnerPhoneNumber);
            Assert.AreEqual(email, retreivedHouses[0].OwnerEmail);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouses[0].NumberOfBedrooms);
            Assert.AreEqual(house.NumberOfKitchens, retreivedHouses[0].NumberOfKitchens);
            Assert.AreEqual(house.GarageAvailable, retreivedHouses[0].LandlinePhoneAvailable);
            Assert.AreEqual(house.SmokingAllowed, retreivedHouses[0].SmokingAllowed);
            Assert.AreEqual(house.InternetAvailable, retreivedHouses[0].InternetAvailable);
            Assert.AreEqual(house.PropertyType, retreivedHouses[0].PropertyType);
            Assert.AreEqual(house.Latitude, retreivedHouses[0].Latitude);
            Assert.AreEqual(house.Longitude, retreivedHouses[0].Longitude);
            Assert.AreEqual(house.HouseNo, retreivedHouses[0].HouseNo);
            Assert.AreEqual(house.Area, retreivedHouses[0].Area);
            Assert.AreEqual(house.StreetNo, retreivedHouses[0].StreetNo);
            Assert.AreEqual(email, house.OwnerEmail);
            Assert.AreEqual(house.OwnerEmail, retreivedHouses[0].OwnerEmail);
            Assert.AreEqual(house.OwnerPhoneNumber, retreivedHouses[0].OwnerPhoneNumber);
            Assert.AreEqual(dimension.DimensionType, retreivedHouses[0].Dimension.DimensionType);
            Assert.AreEqual(dimension.DecimalValue, retreivedHouses[0].Dimension.DecimalValue);
            Assert.AreEqual(dimension.StringValue, retreivedHouses[0].Dimension.StringValue);
            Assert.AreEqual(house.OwnerName, retreivedHouses[0].OwnerName);
            Assert.AreEqual(house.GenderRestriction, retreivedHouses[0].GenderRestriction);
            Assert.AreEqual(house.IsShared, retreivedHouses[0].IsShared);
            Assert.AreEqual(house.RentUnit, retreivedHouses[0].RentUnit);

            // Verification of House # 4
            Assert.AreEqual(title4, retreivedHouses[1].Title);
            Assert.AreEqual(house4.Title, retreivedHouses[1].Title);
            Assert.AreEqual(description4, retreivedHouses[1].Description);
            Assert.AreEqual(house4.Description, retreivedHouses[1].Description);
            Assert.AreEqual(house4.NumberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(house4.NumberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(house4.NumberOfBedrooms, retreivedHouses[1].NumberOfBedrooms);
            Assert.AreEqual(house4.NumberOfKitchens, retreivedHouses[1].NumberOfKitchens);
            Assert.AreEqual(house4.GarageAvailable, retreivedHouses[1].LandlinePhoneAvailable);
            Assert.AreEqual(house4.SmokingAllowed, retreivedHouses[1].SmokingAllowed);
            Assert.AreEqual(house4.InternetAvailable, retreivedHouses[1].InternetAvailable);
            Assert.AreEqual(house4.PropertyType, retreivedHouses[1].PropertyType);
            Assert.AreEqual(house4.Latitude, retreivedHouses[1].Latitude);
            Assert.AreEqual(house4.Longitude, retreivedHouses[1].Longitude);
            Assert.AreEqual(house4.HouseNo, retreivedHouses[1].HouseNo);
            Assert.AreEqual(house4.Area, retreivedHouses[1].Area);
            Assert.AreEqual(house4.StreetNo, retreivedHouses[1].StreetNo);
            Assert.AreEqual(email4, house4.OwnerEmail);
            Assert.AreEqual(house4.OwnerEmail, retreivedHouses[1].OwnerEmail);
            Assert.AreEqual(house4.OwnerPhoneNumber, retreivedHouses[1].OwnerPhoneNumber);
            Assert.AreEqual(dimension4.DimensionType, retreivedHouses[1].Dimension.DimensionType);
            Assert.AreEqual(dimension4.DecimalValue, retreivedHouses[1].Dimension.DecimalValue);
            Assert.AreEqual(dimension4.StringValue, retreivedHouses[1].Dimension.StringValue);
            Assert.AreEqual(house4.OwnerName, retreivedHouses[1].OwnerName);
            Assert.AreEqual(house4.GenderRestriction, retreivedHouses[1].GenderRestriction);
            Assert.AreEqual(house4.IsShared, retreivedHouses[1].IsShared);
            Assert.AreEqual(house4.RentUnit, retreivedHouses[1].RentUnit);
        }

        [Test]
        public void RetrieveHouseByCoordinatesAndPropertyType_GetsTheHousesUsingTheirCoordinates_VerifiesThroughReturnValue()
        {
            // Save multiple houses, retreive them by specifying coordinates and verify all of them
            decimal initialLatitude = 33.29M;
            decimal initialLongitude = 73.41M;
            string propertyType = "House";
            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();
            SaveMultipleHouses(houseRepository, initialLatitude, initialLongitude);
            IList<House> retreivedHouses = houseRepository.SearchHousesByCoordinatesAndPropertyType(initialLatitude,
                initialLongitude, propertyType);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(10, retreivedHouses.Count);
            // This method will check the houses that are multiple of 2, as the provided PropertyType was saved in the
            // SaveMultipleHouses() method when the loop was a multiple of 2, and the values correspond to the loop number
            // (which was a multiple of 2)
            VerifyRetereivedHousesWithAreaAndPropertyType(retreivedHouses, initialLatitude, initialLongitude, propertyType);
        }

        #endregion Save and Search Houses By Area and PropertyType

        #region Save and Search Houses By PropertyType Only

        [Test]
        [Category("Integration")]
        public void SearchMultipleHousesByPropertyTypeOnly_ChecksIfNearbyHousesAreReturnedByPropertyTypeProperly_VerifiesByReturnValue()
        {
            // Save 5 houses in locations nearby, 2 have same PropertyType as the search criteria but 3 have different
            // Search should get the 2 houses with the same propertyType, and ignore the 3 who have a different propertyType, even
            // though they are located within the search radius (2 kilometers)

            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();
            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService =
                _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();

            // Saving House # 1: Should be in the search results
            string area = "Pindora, Rawalpindi, Pakistan";
            string description = "It was a Hobbit Hole. Which means it had good food and a warm hearth.";
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(area);
            string houseNo = "House # 1";
            string streetNo = "1";
            string title = "Title # 1";
            string phoneNumber = "01234567890";
            string email = "special@spsp123456-1.com";
            int numberOfBathrooms = 1;
            int numberOfBedrooms = 1;
            int numberOfKitchens = 1;
            int rent = 100;
            string ownerName = "Owner Name 1";
            string propertyType = "House";
            House house = new House.HouseBuilder().Title(title).OwnerEmail(email)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberOfBathrooms).OwnerPhoneNumber(phoneNumber)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(true).WithInternetAvailable(true)
                .PropertyType(propertyType).RentPrice(rent).Latitude(coordinatesFromAddress.Item1)
                .Longitude(coordinatesFromAddress.Item2)
                .HouseNo(houseNo).Area(area).StreetNo(streetNo).OwnerName(ownerName).Description(description).Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, "1", 0, house);
            house.Dimension = dimension;
            houseRepository.SaveorUpdateDimension(dimension);
            houseRepository.SaveorUpdate(house);

            // Saving House # 2: Should NOT be in the search results, different property type
            string title2 = "Title # 2";
            string area2 = "Satellite Town, Rawalpindi, Pakistan";
            string description2 = "It was a Hobbit Hole 2. Which means it had good food and a warm hearth.";
            var coordinatesFromAddress2 = geocodingService.GetCoordinatesFromAddress(area2);
            string email2 = "special2@spsp12345-2.com";
            string houseNo2 = "House # 2";
            string streetNo2 = "2";
            string phoneNumber2 = "01234567891";
            int numberOfBathrooms2 = 2;
            int numberOfBedrooms2 = 2;
            int numberOfKitchens2 = 2;
            int rent2 = 200;
            string ownerName2 = "Owner Name 2";
            string propertyType2 = "Apartment";

            House house2 = new House.HouseBuilder().Title(title2).OwnerEmail(email2)
                .NumberOfBedrooms(numberOfBedrooms2).NumberOfBathrooms(numberOfBathrooms2).OwnerPhoneNumber(phoneNumber2)
                .NumberOfKitchens(numberOfKitchens2).CableTvAvailable(false)
                .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
                .PropertyType(propertyType2).RentPrice(rent2).Latitude(coordinatesFromAddress2.Item1)
                .Longitude(coordinatesFromAddress2.Item2)
                .HouseNo(houseNo2).Area(area2).StreetNo(streetNo2).OwnerName(ownerName2).Description(description2).Build();
            Dimension dimension2 = new Dimension(DimensionType.Kanal, "2", 0, house2);
            house2.Dimension = dimension2;
            houseRepository.SaveorUpdateDimension(dimension2);
            houseRepository.SaveorUpdate(house2);

            // Saving House # 3: Should be in the search results, outside bounds of search location
            string area3 = "Satellite Town, Rawalpindi, Pakistan";
            var coordinatesFromAddress3 = geocodingService.GetCoordinatesFromAddress(area3);
            string title3 = "Title # 3";
            string description3 = "It was a Hobbit Hole 3. Which means it had good food and a warm hearth.";
            string email3 = "special2@spsp123456-3.com";
            string houseNo3 = "House # 3";
            string streetNo3 = "3";
            string phoneNumber3 = "01234567892";
            int numberOfBathrooms3 = 3;
            int numberOfBedrooms3 = 3;
            int numberOfKitchens3 = 3;
            int rent3 = 300;
            string ownerName3 = "Owner Name 3";
            string propertyType3 = "House";
            bool isShared3 = true;
            string rentUnit3 = "Day";

            House house3 = new House.HouseBuilder().Title(title3).OwnerEmail(email3)
                .NumberOfBedrooms(numberOfBedrooms3).NumberOfBathrooms(numberOfBathrooms3).OwnerPhoneNumber(phoneNumber3)
                .NumberOfKitchens(numberOfKitchens3).CableTvAvailable(false)
                .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
                .PropertyType(propertyType3).RentPrice(rent3).Latitude(coordinatesFromAddress3.Item1)
                .Longitude(coordinatesFromAddress3.Item2)
                .HouseNo(houseNo3).Area(area3).StreetNo(streetNo3).OwnerName(ownerName3).Description(description3).Build();
            Dimension dimension3 = new Dimension(DimensionType.Kanal, "3", 0, house3);
            house3.Dimension = dimension3;
            houseRepository.SaveorUpdateDimension(dimension3);
            houseRepository.SaveorUpdate(house3);

            // Saving House # 4: Should NOT be in the search results, different PropertyType
            string area4 = "I-9, Islamabad, Pakistan";
            var coordinatesFromAddress4 = geocodingService.GetCoordinatesFromAddress(area4);
            string title4 = "Title # 4";
            string description4 = "It was a Hobbit Hole 4. Which means it had good food and a warm hearth.";
            string email4 = "special2@spsp123456-4.com";
            string houseNo4 = "House # 4";
            string streetNo4 = "4";
            string phoneNumber4 = "01234567893";
            int numberOfBathrooms4 = 4;
            int numberOfBedrooms4 = 4;
            int numberOfKitchens4 = 4;
            int rent4 = 400;
            string ownerName4 = "Owner Name 4";
            string propertyType4 = "Hostel";

            House house4 = new House.HouseBuilder().Title(title4).OwnerEmail(email4)
                .NumberOfBedrooms(numberOfBedrooms4).NumberOfBathrooms(numberOfBathrooms4).OwnerPhoneNumber(phoneNumber4)
                .NumberOfKitchens(numberOfKitchens4).CableTvAvailable(false)
                .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
                .PropertyType(propertyType4).RentPrice(rent4).Latitude(coordinatesFromAddress4.Item1)
                .Longitude(coordinatesFromAddress4.Item2)
                .HouseNo(houseNo4).Area(area4).StreetNo(streetNo4).OwnerName(ownerName4).Description(description4).Build();
            Dimension dimension4 = new Dimension(DimensionType.Kanal, "4", 0, house4);
            house4.Dimension = dimension4;
            houseRepository.SaveorUpdateDimension(dimension4);
            houseRepository.SaveorUpdate(house4);

            // Saving House # 5: Should NOT be in the search results, different PropertyType
            string area5 = "I-10, Islamabad, Pakistan";
            var coordinatesFromAddress5 = geocodingService.GetCoordinatesFromAddress(area5);
            string title5 = "Title # 5";
            string description5 = "It was a Hobbit Hole 5. Which means it had good food and a warm hearth.";
            string email5 = "special2@spsp123456-5.com";
            string houseNo5 = "House # 5";
            string streetNo5 = "5";
            string phoneNumber5 = "01234567894";
            int numberOfBathrooms5 = 5;
            int numberOfBedrooms5 = 5;
            int numberOfKitchens5 = 5;
            int rent5 = 500;
            string ownerName5 = "Owner Name 5";
            string propertyType5 = "Hotel";

            House house5 = new House.HouseBuilder().Title(title5).OwnerEmail(email5)
                .NumberOfBedrooms(numberOfBedrooms5).NumberOfBathrooms(numberOfBathrooms5).OwnerPhoneNumber(phoneNumber5)
                .NumberOfKitchens(numberOfKitchens5).CableTvAvailable(false)
                .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
                .PropertyType(propertyType5).RentPrice(rent5).Latitude(coordinatesFromAddress5.Item1)
                .Longitude(coordinatesFromAddress5.Item2)
                .HouseNo(houseNo5).Area(area5).StreetNo(streetNo5).OwnerName(ownerName5).Description(description5).Build();
            Dimension dimension5 = new Dimension(DimensionType.Kanal, "5", 0, house5);
            house5.Dimension = dimension5;
            houseRepository.SaveorUpdateDimension(dimension5);
            houseRepository.SaveorUpdate(house5);

            string searchedPropertyType = "House";
            var retreivedHouses = houseRepository.SearchHousesByPropertyType(searchedPropertyType);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(2, retreivedHouses.Count);

            // Verification of House # 1
            Assert.AreEqual(title, retreivedHouses[0].Title);
            Assert.AreEqual(house.Title, retreivedHouses[0].Title);
            Assert.AreEqual(description, retreivedHouses[0].Description);
            Assert.AreEqual(house.Description, retreivedHouses[0].Description);
            Assert.AreEqual(phoneNumber, retreivedHouses[0].OwnerPhoneNumber);
            Assert.AreEqual(email, retreivedHouses[0].OwnerEmail);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouses[0].NumberOfBedrooms);
            Assert.AreEqual(house.NumberOfKitchens, retreivedHouses[0].NumberOfKitchens);
            Assert.AreEqual(house.GarageAvailable, retreivedHouses[0].LandlinePhoneAvailable);
            Assert.AreEqual(house.SmokingAllowed, retreivedHouses[0].SmokingAllowed);
            Assert.AreEqual(house.InternetAvailable, retreivedHouses[0].InternetAvailable);
            Assert.AreEqual(house.PropertyType, retreivedHouses[0].PropertyType);
            Assert.AreEqual(house.Latitude, retreivedHouses[0].Latitude);
            Assert.AreEqual(house.Longitude, retreivedHouses[0].Longitude);
            Assert.AreEqual(house.HouseNo, retreivedHouses[0].HouseNo);
            Assert.AreEqual(house.Area, retreivedHouses[0].Area);
            Assert.AreEqual(house.StreetNo, retreivedHouses[0].StreetNo);
            Assert.AreEqual(email, house.OwnerEmail);
            Assert.AreEqual(house.OwnerEmail, retreivedHouses[0].OwnerEmail);
            Assert.AreEqual(house.OwnerPhoneNumber, retreivedHouses[0].OwnerPhoneNumber);
            Assert.AreEqual(dimension.DimensionType, retreivedHouses[0].Dimension.DimensionType);
            Assert.AreEqual(dimension.DecimalValue, retreivedHouses[0].Dimension.DecimalValue);
            Assert.AreEqual(dimension.StringValue, retreivedHouses[0].Dimension.StringValue);
            Assert.AreEqual(house.OwnerName, retreivedHouses[0].OwnerName);
            Assert.AreEqual(house.IsShared, retreivedHouses[0].IsShared);
            Assert.AreEqual(house.RentUnit, retreivedHouses[0].RentUnit);

            // Verification of House # 3
            Assert.AreEqual(title3, retreivedHouses[1].Title);
            Assert.AreEqual(house3.Title, retreivedHouses[1].Title);
            Assert.AreEqual(description3, retreivedHouses[1].Description);
            Assert.AreEqual(house3.Description, retreivedHouses[1].Description);
            Assert.AreEqual(house3.NumberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(house3.NumberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(house3.NumberOfBedrooms, retreivedHouses[1].NumberOfBedrooms);
            Assert.AreEqual(house3.NumberOfKitchens, retreivedHouses[1].NumberOfKitchens);
            Assert.AreEqual(house3.GarageAvailable, retreivedHouses[1].LandlinePhoneAvailable);
            Assert.AreEqual(house3.SmokingAllowed, retreivedHouses[1].SmokingAllowed);
            Assert.AreEqual(house3.InternetAvailable, retreivedHouses[1].InternetAvailable);
            Assert.AreEqual(house3.PropertyType, retreivedHouses[1].PropertyType);
            Assert.AreEqual(house3.Latitude, retreivedHouses[1].Latitude);
            Assert.AreEqual(house3.Longitude, retreivedHouses[1].Longitude);
            Assert.AreEqual(house3.HouseNo, retreivedHouses[1].HouseNo);
            Assert.AreEqual(house3.Area, retreivedHouses[1].Area);
            Assert.AreEqual(house3.StreetNo, retreivedHouses[1].StreetNo);
            Assert.AreEqual(email3, house3.OwnerEmail);
            Assert.AreEqual(house3.OwnerEmail, retreivedHouses[1].OwnerEmail);
            Assert.AreEqual(house3.OwnerPhoneNumber, retreivedHouses[1].OwnerPhoneNumber);
            Assert.AreEqual(dimension3.DimensionType, retreivedHouses[1].Dimension.DimensionType);
            Assert.AreEqual(dimension3.DecimalValue, retreivedHouses[1].Dimension.DecimalValue);
            Assert.AreEqual(dimension3.StringValue, retreivedHouses[1].Dimension.StringValue);
            Assert.AreEqual(house3.OwnerName, retreivedHouses[1].OwnerName);
            Assert.AreEqual(house3.IsShared, retreivedHouses[1].IsShared);
            Assert.AreEqual(house3.RentUnit, retreivedHouses[1].RentUnit);
        }

        [Test]
        public void RetreiveHousesByPropertyTypePaginationTest_ChecksThatThePaginationIsWorkingFine_VerifiesThroughReturnedOutput()
        {
            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();

            // Save more than 10 houses using the same property type
            SaveMultipleHousesUsingGivenIterations(houseRepository, 21);

            // Now search by property type and check we only retreived 10 houses
            string searchedPropertyType = "House";
            var retreivedHouses = houseRepository.SearchHousesByPropertyType(searchedPropertyType);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(10, retreivedHouses.Count);
        }

        [Test]
        public void RetrieveHouseByPropertyTypeOnly_GetsTheHousesUsingTheirPropertyType_VerifiesThroughReturnValue()
        {
            // Save multiple houses, retreive them by specifying coordinates and verify all of them
            decimal initialLatitude = 33.29M;
            decimal initialLongitude = 73.41M;
            string propertyType = "House";
            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();
            SaveMultipleHouses(houseRepository, initialLatitude, initialLongitude);
            IList<House> retreivedHouses = houseRepository.SearchHousesByPropertyType(propertyType);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(10, retreivedHouses.Count);
            // This method will check the houses that are multiple of 2, as the provided PropertyType was saved in the
            // SaveMultipleHouses() method when the loop was a multiple of 2, and the values correspond to the loop number
            // (which was a multiple of 2)
            VerifyRetereivedHousesWithAreaAndPropertyType(retreivedHouses, initialLatitude, initialLongitude, propertyType);
        }

        #endregion Save and Search Houses By PropertyType Only

        #region Save and Search Houses By Area Only

        [Test]
        public void RetrieveSingleHouseByCoordinates_GetsTheHouseUsingItsCoordinates_VerifiesThroughReturnValue()
        {
            // Save two houses with the same coordinates and get both of them 
            string area = "Pindora, Rawalpindi, Pakistan";

            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();
            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService = _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(area);

            // Saving House # 1
            string houseNo = "S-123";
            string streetNo = "13";
            string title = "Special House";
            string phoneNumber = "01234567890";
            string email = "special@spsp123456.com";
            int numberOfBathrooms = 1;
            int numberOfBedrooms = 1;
            int numberOfKitchens = 1;
            int rent = 50000;
            string ownerName = "Owner Name 1";
            string propertyType = "Apartment";
            string rentUnit = "Hour";
            House house = new House.HouseBuilder().Title(title).OwnerEmail(email)
            .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberOfBathrooms).OwnerPhoneNumber(phoneNumber)
            .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
            .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(true).WithInternetAvailable(true)
            .PropertyType(propertyType).RentPrice(rent).Latitude(coordinatesFromAddress.Item1)
            .Longitude(coordinatesFromAddress.Item2)
            .HouseNo(houseNo).Area(area).StreetNo(streetNo).OwnerName(ownerName).RentUnit(rentUnit).Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, "5", 0, house);
            house.Dimension = dimension;
            houseRepository.SaveorUpdateDimension(dimension);
            houseRepository.SaveorUpdate(house);

            // Saving House # 2
            string email2 = "special2@spsp123456.com";
            string houseNo2 = "S2-123";
            string streetNo2 = "2-13";
            string phoneNumber2 = "01234567891";
            int numberOfBathrooms2 = 2;
            int numberOfBedrooms2 = 2;
            int numberOfKitchens2 = 3;
            int rent2 = 100000;
            string ownerName2 = "Owner Name 2";
            string propertyType2 = "House";
            bool isShared2 = true;
            string rentUnit2 = "Month";
            House house2 = new House.HouseBuilder().Title(title).OwnerEmail(email2)
            .NumberOfBedrooms(numberOfBedrooms2).NumberOfBathrooms(numberOfBathrooms2).OwnerPhoneNumber(phoneNumber2)
            .NumberOfKitchens(numberOfKitchens2).CableTvAvailable(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType2).RentPrice(rent2).Latitude(coordinatesFromAddress.Item1)
            .Longitude(coordinatesFromAddress.Item2)
            .HouseNo(houseNo2).Area(area).StreetNo(streetNo2).OwnerName(ownerName2).RentUnit(rentUnit2).IsShared(isShared2)
            .Build();
            Dimension dimension2 = new Dimension(DimensionType.Marla, "20", 0, house2);
            house2.Dimension = dimension2;
            houseRepository.SaveorUpdateDimension(dimension2);
            houseRepository.SaveorUpdate(house2);

            // Saving House # 3
            string area2 = "Saddar, Rawalpindi, Pakistan";
            var coordinatesFromAddress2 = geocodingService.GetCoordinatesFromAddress(area2);
            string email3 = "special2@spsp123456.com";
            string houseNo3 = "S2-123";
            string streetNo3 = "2-13";
            string phoneNumber3 = "01234567891";
            int numberOfBathrooms3 = 2;
            int numberOfBedrooms3 = 2;
            int numberOfKitchens3 = 3;
            int rent3 = 100000;
            string ownerName3 = "Owner Name 3";
            string propertyType3 = "House";
            bool isShared3 = true;
            string rentUnit3 = "Month";
            House house3 = new House.HouseBuilder().Title(title).OwnerEmail(email3)
            .NumberOfBedrooms(numberOfBedrooms3).NumberOfBathrooms(numberOfBathrooms3).OwnerPhoneNumber(phoneNumber3)
            .NumberOfKitchens(numberOfKitchens3).CableTvAvailable(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType3).RentPrice(rent3).Latitude(coordinatesFromAddress2.Item1)
            .Longitude(coordinatesFromAddress2.Item2)
            .HouseNo(houseNo3).Area(area).StreetNo(streetNo3).OwnerName(ownerName3).IsShared(isShared3).RentUnit(rentUnit3).Build();
            Dimension dimension3 = new Dimension(DimensionType.Acre, "2", 0, house3);
            house3.Dimension = dimension3;
            houseRepository.SaveorUpdateDimension(dimension3);
            houseRepository.SaveorUpdate(house3);

            var retreivedHouses = houseRepository.SearchHousesByCoordinates(coordinatesFromAddress.Item1, coordinatesFromAddress.Item2);
            Assert.NotNull(retreivedHouses);

            // Verification of House # 1
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouses[0].NumberOfBedrooms);
            Assert.AreEqual(house.NumberOfKitchens, retreivedHouses[0].NumberOfKitchens);
            Assert.AreEqual(house.GarageAvailable, retreivedHouses[0].LandlinePhoneAvailable);
            Assert.AreEqual(house.SmokingAllowed, retreivedHouses[0].SmokingAllowed);
            Assert.AreEqual(house.InternetAvailable, retreivedHouses[0].InternetAvailable);
            Assert.AreEqual(house.PropertyType, retreivedHouses[0].PropertyType);
            Assert.AreEqual(house.Latitude, retreivedHouses[0].Latitude);
            Assert.AreEqual(house.Longitude, retreivedHouses[0].Longitude);
            Assert.AreEqual(house.HouseNo, retreivedHouses[0].HouseNo);
            Assert.AreEqual(house.Area, retreivedHouses[0].Area);
            Assert.AreEqual(house.StreetNo, retreivedHouses[0].StreetNo);
            Assert.AreEqual(email, house.OwnerEmail);
            Assert.AreEqual(house.OwnerEmail, retreivedHouses[0].OwnerEmail);
            Assert.AreEqual(house.OwnerPhoneNumber, retreivedHouses[0].OwnerPhoneNumber);
            Assert.AreEqual(dimension.DimensionType, retreivedHouses[0].Dimension.DimensionType);
            Assert.AreEqual(dimension.DecimalValue, retreivedHouses[0].Dimension.DecimalValue);
            Assert.AreEqual(dimension.StringValue, retreivedHouses[0].Dimension.StringValue);
            Assert.AreEqual(house.OwnerName, retreivedHouses[0].OwnerName);
            Assert.AreEqual(house.IsShared, retreivedHouses[0].IsShared);
            Assert.AreEqual(house.RentUnit, retreivedHouses[0].RentUnit);

            // Verification of House # 2
            Assert.AreEqual(house2.NumberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(house2.NumberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(house2.NumberOfBedrooms, retreivedHouses[1].NumberOfBedrooms);
            Assert.AreEqual(house2.NumberOfKitchens, retreivedHouses[1].NumberOfKitchens);
            Assert.AreEqual(house2.GarageAvailable, retreivedHouses[1].LandlinePhoneAvailable);
            Assert.AreEqual(house2.SmokingAllowed, retreivedHouses[1].SmokingAllowed);
            Assert.AreEqual(house2.InternetAvailable, retreivedHouses[1].InternetAvailable);
            Assert.AreEqual(house2.PropertyType, retreivedHouses[1].PropertyType);
            Assert.AreEqual(house2.Latitude, retreivedHouses[1].Latitude);
            Assert.AreEqual(house2.Longitude, retreivedHouses[1].Longitude);
            Assert.AreEqual(house2.HouseNo, retreivedHouses[1].HouseNo);
            Assert.AreEqual(house2.Area, retreivedHouses[1].Area);
            Assert.AreEqual(house2.StreetNo, retreivedHouses[1].StreetNo);
            Assert.AreEqual(email2, house2.OwnerEmail);
            Assert.AreEqual(house2.OwnerEmail, retreivedHouses[1].OwnerEmail);
            Assert.AreEqual(house2.OwnerPhoneNumber, retreivedHouses[1].OwnerPhoneNumber);
            Assert.AreEqual(dimension2.DimensionType, retreivedHouses[1].Dimension.DimensionType);
            Assert.AreEqual(dimension2.DecimalValue, retreivedHouses[1].Dimension.DecimalValue);
            Assert.AreEqual(dimension2.StringValue, retreivedHouses[1].Dimension.StringValue);
            Assert.AreEqual(house2.OwnerName, retreivedHouses[1].OwnerName);
            Assert.AreEqual(house2.IsShared, retreivedHouses[1].IsShared);
            Assert.AreEqual(house2.RentUnit, retreivedHouses[1].RentUnit);
        }

        [Test]
        public void RetrieveHouseByCoordinates_GetsTheHousesUsingTheirCoordinates_VerifiesThroughReturnValue()
        {
            // Save multiple houses, retreive them by specifying coordinates and verify all of them
            decimal initialLatitude = 33.29M;
            decimal initialLongitude = 73.41M;
            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();
            SaveMultipleHouses(houseRepository, initialLatitude, initialLongitude);
            IList<House> retreivedHouses = houseRepository.SearchHousesByCoordinates(initialLatitude, initialLongitude);
            Assert.NotNull(retreivedHouses);
            // Only 10 houses are retreived because of the result size limit
            Assert.AreEqual(10, retreivedHouses.Count);
            VerifyRetereivedHouses(retreivedHouses, initialLatitude, initialLongitude);
        }

        [Test]
        public void RetrieveHouseByCoordinatesFailTest_ChecksByProvidingTheWrongCoordinatesThatNoHouseIsRetreived_VerifiesThroughReturnValue()
        {
            // Save multiple houses and 
            decimal initialLatitude = 33.29M;
            decimal initialLongitude = 73.41M;
            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();
            SaveMultipleHouses(houseRepository, initialLatitude, initialLongitude);
            IList<House> retreivedHouses = houseRepository.SearchHousesByCoordinates(29.00M, 69.00M);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(0, retreivedHouses.Count);
        }

        [Test]
        [Category("Integration")]
        public void SearchMultipleHousesByCoordinates_ChecksIfNearbyHousesAreReturned_VerifiesByReturnValue()
        {
            Console.WriteLine("Start");
            DateTime startTime = DateTime.Now;

            // Save 3 houses in locations nearby and 2 houses that are in other places. 
            // Search should get the 3 houses located near theserched location

            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();
            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService = 
                _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();

            // Saving House # 1: Near the search location, should be in the search results
            string area = "Pindora, Rawalpindi, Pakistan";
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(area);
            string houseNo = "House # 1";
            string streetNo = "1";
            string title = "Title # 1";
            string phoneNumber = "01234567890";
            string email = "special@spsp123456-1.com";
            int numberOfBathrooms = 1;
            int numberOfBedrooms = 1;
            int numberOfKitchens = 1;
            int rent = 100;
            string ownerName = "Owner Name 1";
            string propertyType = "House";
            bool isShared = false;
            string rentUnit = "Hour";
            House house = new House.HouseBuilder().Title(title).OwnerEmail(email)
            .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberOfBathrooms).OwnerPhoneNumber(phoneNumber)
            .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
            .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(true).WithInternetAvailable(true)
            .PropertyType(propertyType).RentPrice(rent).Latitude(coordinatesFromAddress.Item1)
            .Longitude(coordinatesFromAddress.Item2)
            .HouseNo(houseNo).Area(area).StreetNo(streetNo).OwnerName(ownerName).IsShared(isShared).RentUnit(rentUnit)
            .Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, "1", 0, house);
            house.Dimension = dimension;
            houseRepository.SaveorUpdateDimension(dimension);
            houseRepository.SaveorUpdate(house);

            // Saving House # 2: Near the search location, should be in the search results
            string title2 = "Title # 2";
            string area2 = "Satellite Town, Rawalpindi, Pakistan";
            var coordinatesFromAddress2 = geocodingService.GetCoordinatesFromAddress(area2);
            string email2 = "special2@spsp12345-2.com";
            string houseNo2 = "House # 2";
            string streetNo2 = "2";
            string phoneNumber2 = "01234567891";
            int numberOfBathrooms2 = 2;
            int numberOfBedrooms2 = 2;
            int numberOfKitchens2 = 2;
            int rent2 = 200;
            string ownerName2 = "Owner Name 2";
            string propertyType2 = "House";
            bool isShared2 = false;
            string rentUnit2 = "Month";

            House house2 = new House.HouseBuilder().Title(title2).OwnerEmail(email2)
            .NumberOfBedrooms(numberOfBedrooms2).NumberOfBathrooms(numberOfBathrooms2).OwnerPhoneNumber(phoneNumber2)
            .NumberOfKitchens(numberOfKitchens2).CableTvAvailable(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType2).RentPrice(rent2).Latitude(coordinatesFromAddress2.Item1)
            .Longitude(coordinatesFromAddress2.Item2)
            .HouseNo(houseNo2).Area(area2).StreetNo(streetNo2).OwnerName(ownerName2).IsShared(isShared2).RentUnit(rentUnit2).Build();
            Dimension dimension2 = new Dimension(DimensionType.Kanal, "2", 0, house2);
            house2.Dimension = dimension2;
            houseRepository.SaveorUpdateDimension(dimension2);
            houseRepository.SaveorUpdate(house2);

            // Saving House # 3: Outside the bounds of the search location, should not be in the search results
            string area3 = "Nara, Punjab, Pakistan";
            var coordinatesFromAddress3 = geocodingService.GetCoordinatesFromAddress(area3);
            string title3 = "Title # 3";
            string email3 = "special2@spsp123456-3.com";
            string houseNo3 = "House # 3";
            string streetNo3 = "3";
            string phoneNumber3 = "01234567892";
            int numberOfBathrooms3 = 3;
            int numberOfBedrooms3 = 3;
            int numberOfKitchens3 = 3;
            int rent3 = 300;
            string ownerName3 = "Owner Name 3";
            string propertyType3 = "House";
            bool isShared3 = true;
            string rentUnit3 = "Week";

            House house3 = new House.HouseBuilder().Title(title3).OwnerEmail(email3)
            .NumberOfBedrooms(numberOfBedrooms3).NumberOfBathrooms(numberOfBathrooms3).OwnerPhoneNumber(phoneNumber3)
            .NumberOfKitchens(numberOfKitchens3).CableTvAvailable(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType3).RentPrice(rent3).Latitude(coordinatesFromAddress3.Item1)
            .Longitude(coordinatesFromAddress3.Item2)
            .HouseNo(houseNo3).Area(area3).StreetNo(streetNo3).OwnerName(ownerName3).IsShared(isShared3).RentUnit(rentUnit3).Build();
            Dimension dimension3 = new Dimension(DimensionType.Kanal, "3", 0, house3);
            house3.Dimension = dimension3;
            houseRepository.SaveorUpdateDimension(dimension3);
            houseRepository.SaveorUpdate(house3);

            // Saving House # 4: Should be in the search results
            string area4 = "I-9, Islamabad, Pakistan";
            var coordinatesFromAddress4 = geocodingService.GetCoordinatesFromAddress(area4);
            string title4 = "Title # 4";
            string email4 = "special2@spsp123456-4.com";
            string houseNo4 = "House # 4";
            string streetNo4 = "4";
            string phoneNumber4 = "01234567893";
            int numberOfBathrooms4 = 4;
            int numberOfBedrooms4 = 4;
            int numberOfKitchens4 = 4;
            int rent4 = 400;
            string ownerName4 = "Owner Name 4";
            string propertyType4 = "House";
            bool isShared4 = true;
            string rentUnit4 = "Day";

            House house4 = new House.HouseBuilder().Title(title4).OwnerEmail(email4)
            .NumberOfBedrooms(numberOfBedrooms4).NumberOfBathrooms(numberOfBathrooms4).OwnerPhoneNumber(phoneNumber4)
            .NumberOfKitchens(numberOfKitchens4).CableTvAvailable(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType4).RentPrice(rent4).Latitude(coordinatesFromAddress4.Item1)
            .Longitude(coordinatesFromAddress4.Item2)
            .HouseNo(houseNo4).Area(area4).StreetNo(streetNo4).OwnerName(ownerName4).IsShared(isShared4).RentUnit(rentUnit4)
            .Build();
            Dimension dimension4 = new Dimension(DimensionType.Kanal, "4", 0, house4);
            house4.Dimension = dimension4;
            houseRepository.SaveorUpdateDimension(dimension4);
            houseRepository.SaveorUpdate(house4);

            // Saving House # 5: Should NOT be in the search results
            string area5 = "Qurtaba City, Punjab, Pakistan";
            var coordinatesFromAddress5 = geocodingService.GetCoordinatesFromAddress(area5);
            string title5 = "Title # 5";
            string email5 = "special2@spsp123456-5.com";
            string houseNo5 = "House # 5";
            string streetNo5 = "5";
            string phoneNumber5 = "01234567894";
            int numberOfBathrooms5 = 5;
            int numberOfBedrooms5 = 5;
            int numberOfKitchens5 = 5;
            int rent5 = 100000;
            string ownerName5 = "Owner Name 5";
            string propertyType5 = "House";
            bool isShared5 = false;
            string rentUnit5 = "Day";

            House house5 = new House.HouseBuilder().Title(title5).OwnerEmail(email5)
            .NumberOfBedrooms(numberOfBedrooms5).NumberOfBathrooms(numberOfBathrooms5).OwnerPhoneNumber(phoneNumber5)
            .NumberOfKitchens(numberOfKitchens5).CableTvAvailable(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType5).RentPrice(rent5).Latitude(coordinatesFromAddress5.Item1)
            .Longitude(coordinatesFromAddress5.Item2)
            .HouseNo(houseNo5).Area(area5).StreetNo(streetNo5).OwnerName(ownerName5).IsShared(isShared5).RentUnit(rentUnit5)
            .Build();
            Dimension dimension5 = new Dimension(DimensionType.Kanal, "5", 0, house5);
            house5.Dimension = dimension5;
            houseRepository.SaveorUpdateDimension(dimension5);
            houseRepository.SaveorUpdate(house5);

            var retreivedHouses = houseRepository.SearchHousesByCoordinatesAndPropertyType(coordinatesFromAddress.Item1,
                coordinatesFromAddress.Item2, propertyType);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(3, retreivedHouses.Count);
            int timeTaken = (DateTime.Now - startTime).Milliseconds;
            Console.WriteLine("Total Time Taken: " + timeTaken);

            // Verification of House # 1
            Assert.AreEqual(title, retreivedHouses[0].Title);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouses[0].NumberOfBedrooms);
            Assert.AreEqual(house.NumberOfKitchens, retreivedHouses[0].NumberOfKitchens);
            Assert.AreEqual(house.GarageAvailable, retreivedHouses[0].LandlinePhoneAvailable);
            Assert.AreEqual(house.SmokingAllowed, retreivedHouses[0].SmokingAllowed);
            Assert.AreEqual(house.InternetAvailable, retreivedHouses[0].InternetAvailable);
            Assert.AreEqual(house.PropertyType, retreivedHouses[0].PropertyType);
            Assert.AreEqual(house.Latitude, retreivedHouses[0].Latitude);
            Assert.AreEqual(house.Longitude, retreivedHouses[0].Longitude);
            Assert.AreEqual(house.HouseNo, retreivedHouses[0].HouseNo);
            Assert.AreEqual(house.Area, retreivedHouses[0].Area);
            Assert.AreEqual(house.Area, retreivedHouses[0].Area);
            Assert.AreEqual(house.StreetNo, retreivedHouses[0].StreetNo);
            Assert.AreEqual(email, house.OwnerEmail);
            Assert.AreEqual(house.OwnerEmail, retreivedHouses[0].OwnerEmail);
            Assert.AreEqual(house.OwnerPhoneNumber, retreivedHouses[0].OwnerPhoneNumber);
            Assert.AreEqual(dimension.DimensionType, retreivedHouses[0].Dimension.DimensionType);
            Assert.AreEqual(dimension.DecimalValue, retreivedHouses[0].Dimension.DecimalValue);
            Assert.AreEqual(dimension.StringValue, retreivedHouses[0].Dimension.StringValue);
            Assert.AreEqual(house.OwnerName, retreivedHouses[0].OwnerName);
            Assert.AreEqual(house.IsShared, retreivedHouses[0].IsShared);
            Assert.AreEqual(house.RentUnit, retreivedHouses[0].RentUnit);

            // Verification of House # 2
            Assert.AreEqual(title2, retreivedHouses[1].Title);
            Assert.AreEqual(house2.NumberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(house2.NumberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(house2.NumberOfBedrooms, retreivedHouses[1].NumberOfBedrooms);
            Assert.AreEqual(house2.NumberOfKitchens, retreivedHouses[1].NumberOfKitchens);
            Assert.AreEqual(house2.GarageAvailable, retreivedHouses[1].LandlinePhoneAvailable);
            Assert.AreEqual(house2.SmokingAllowed, retreivedHouses[1].SmokingAllowed);
            Assert.AreEqual(house2.InternetAvailable, retreivedHouses[1].InternetAvailable);
            Assert.AreEqual(house2.PropertyType, retreivedHouses[1].PropertyType);
            Assert.AreEqual(house2.Latitude, retreivedHouses[1].Latitude);
            Assert.AreEqual(house2.Longitude, retreivedHouses[1].Longitude);
            Assert.AreEqual(house2.HouseNo, retreivedHouses[1].HouseNo);
            Assert.AreEqual(house2.Area, retreivedHouses[1].Area);
            Assert.AreEqual(house2.StreetNo, retreivedHouses[1].StreetNo);
            Assert.AreEqual(email2, house2.OwnerEmail);
            Assert.AreEqual(house2.OwnerEmail, retreivedHouses[1].OwnerEmail);
            Assert.AreEqual(house2.OwnerPhoneNumber, retreivedHouses[1].OwnerPhoneNumber);
            Assert.AreEqual(dimension2.DimensionType, retreivedHouses[1].Dimension.DimensionType);
            Assert.AreEqual(dimension2.DecimalValue, retreivedHouses[1].Dimension.DecimalValue);
            Assert.AreEqual(dimension2.StringValue, retreivedHouses[1].Dimension.StringValue);
            Assert.AreEqual(house2.OwnerName, retreivedHouses[1].OwnerName);
            Assert.AreEqual(house2.IsShared, retreivedHouses[1].IsShared);
            Assert.AreEqual(house2.RentUnit, retreivedHouses[1].RentUnit);

            // Verification of House # 4
            Assert.AreEqual(title4, retreivedHouses[2].Title);
            Assert.AreEqual(house4.NumberOfBathrooms, retreivedHouses[2].NumberOfBathrooms);
            Assert.AreEqual(house4.NumberOfBathrooms, retreivedHouses[2].NumberOfBathrooms);
            Assert.AreEqual(house4.NumberOfBedrooms, retreivedHouses[2].NumberOfBedrooms);
            Assert.AreEqual(house4.NumberOfKitchens, retreivedHouses[2].NumberOfKitchens);
            Assert.AreEqual(house4.GarageAvailable, retreivedHouses[2].LandlinePhoneAvailable);
            Assert.AreEqual(house4.SmokingAllowed, retreivedHouses[2].SmokingAllowed);
            Assert.AreEqual(house4.InternetAvailable, retreivedHouses[2].InternetAvailable);
            Assert.AreEqual(house4.PropertyType, retreivedHouses[2].PropertyType);
            Assert.AreEqual(house4.Latitude, retreivedHouses[2].Latitude);
            Assert.AreEqual(house4.Longitude, retreivedHouses[2].Longitude);
            Assert.AreEqual(house4.HouseNo, retreivedHouses[2].HouseNo);
            Assert.AreEqual(house4.Area, retreivedHouses[2].Area);
            Assert.AreEqual(house4.StreetNo, retreivedHouses[2].StreetNo);
            Assert.AreEqual(email4, house4.OwnerEmail);
            Assert.AreEqual(house4.OwnerEmail, retreivedHouses[2].OwnerEmail);
            Assert.AreEqual(house4.OwnerPhoneNumber, retreivedHouses[2].OwnerPhoneNumber);
            Assert.AreEqual(dimension4.DimensionType, retreivedHouses[2].Dimension.DimensionType);
            Assert.AreEqual(dimension4.DecimalValue, retreivedHouses[2].Dimension.DecimalValue);
            Assert.AreEqual(dimension4.StringValue, retreivedHouses[2].Dimension.StringValue);
            Assert.AreEqual(house4.OwnerName, retreivedHouses[2].OwnerName);
            Assert.AreEqual(house4.IsShared, retreivedHouses[2].IsShared);
            Assert.AreEqual(house4.RentUnit, retreivedHouses[2].RentUnit);
        }
        
        #endregion Save and Search Houses By Area Only
        
        #region Save and Search Houses by Email

        [Test]
        public void SaveHouseAndRetreiveByEmailTest_TestsThatHouseUInstancesAreSavedToTheDatabaseAsExpected_VerifiesThroughDatabaseQuery()
        {
            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();
            string email = "w@12344321.com";
            string description = "It was a Hobbit Hole. Which means it had good food and a warm hearth.";
            string title = "MGM Grand";
            string phoneNumber = "01234567891";

            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long price = 90000;
            string ownerName = "Owner Name 1";
            string propertyType = "Apartment";

            House house = new House.HouseBuilder().Title(title).OwnerPhoneNumber(phoneNumber).OwnerEmail(email)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(propertyType).RentPrice(price).Latitude(33.29M).Longitude(73.41M)
                .HouseNo("123").Area("Pindora").StreetNo("13").OwnerName(ownerName).Description(description).Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, "50", 0, house);
            house.Dimension = dimension;
            houseRepository.SaveorUpdateDimension(dimension);
            houseRepository.SaveorUpdate(house);

            IList<House> retreivedHouses =  houseRepository.GetHouseByOwnerEmail(email);

            House retreivedHouse = retreivedHouses[0];
            Assert.NotNull(retreivedHouse);
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

            IList<House> allHouses = houseRepository.GetAllHouses();
            Assert.IsNotNull(allHouses);
            Assert.AreNotEqual(0, allHouses.Count);
        }

        #endregion Save and Search House Images
        
        #region Utility methods

        /// <summary>
        /// We can use this method to create data for pagination tests. Values are simple and repetitive
        /// </summary>
        /// <param name="houseRepository"></param>
        /// <param name="numberOfIterations"></param>
        private void SaveMultipleHousesUsingGivenIterations(IResidentialPropertyRepository houseRepository, int numberOfIterations)
        {
            for (int i = 0; i < numberOfIterations; i++)
            {
                string area = "Pindora, Rawalpindi, Pakistan";
                string description = "It was a Hobbit Hole. Which means it had good food and a warm hearth.";
                string houseNo = "House # 1";
                string streetNo = "1";
                string title = "Title # 1";
                string phoneNumber = "01234567890";
                string email = "special@spsp123456-1.com";
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
                    .PropertyType(propertyType).RentPrice(rent).Latitude(i + 1)
                    .Longitude(i + 1)
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

        /// <summary>
        /// Values are saved in this method based on the loop number, so they can be verified in batches after they 
        /// are retreived
        /// </summary>
        /// <param name="houseRepository"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        private void SaveMultipleHouses(IResidentialPropertyRepository houseRepository, decimal latitude, decimal longitude)
        {
            decimal initialLatitude = latitude;
            decimal initialLongitude = longitude;
            int rentPrice = 50000;
            for (int i = 0; i < 20; i++)
            {
                initialLatitude += _latitudeIncrementForMultipleHouseSaves;
                initialLongitude += _longitudeIncrementForMultipleHouseSaves;
                string title = "MGM Grand" + i;
                string description = "It was a Hobbit Hole " + i + ". Which means it had good food and a warm hearth.";
                string phoneNumber = null;
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
                DimensionType dimensionType = DimensionType.Kanal;
                string ownerName = "Owner Name " + i;

                var propertyType = i % 2 == 0 ? "House" : "Apartment";

                House house = new House.HouseBuilder().Title(title).OwnerEmail(email)
                    .NumberOfBedrooms(i).NumberOfBathrooms(i).OwnerPhoneNumber(phoneNumber)
                    .NumberOfKitchens(i).CableTvAvailable(false)
                    .GarageAvailable(false)
                    .LandlinePhoneAvailable(true)
                    .SmokingAllowed(false)
                    .WithInternetAvailable(true)
                    .PropertyType(propertyType)
                    .Latitude(initialLatitude)
                    .Longitude(initialLongitude)
                    .RentPrice(rentPrice)
                    .HouseNo(houseNo).Area(area).StreetNo(streetNo).OwnerName(ownerName)
                    .Description(description).Build();
                Dimension dimension = new Dimension(dimensionType, i.ToString(), 0, house);
                house.Dimension = dimension;
                houseRepository.SaveorUpdateDimension(dimension);
                houseRepository.SaveorUpdate(house);
                rentPrice++;
            }
        }

        /// <summary>
        /// Loops through houses. These houses contain values that are multiple of 2, as these are only for the selected 
        /// PropertyType, which were populated in the SaveMultipleHouses() method when the loop number was multiple of 2
        /// </summary>
        /// <param name="retreivedHouses"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="propertyType"></param>
        private void VerifyRetereivedHousesWithAreaAndPropertyType(IList<House> retreivedHouses, decimal latitude, decimal longitude,
            string propertyType)
        {
            decimal initialLatitude = latitude;
            decimal initialLongitude = longitude;
            for (int i = 0; i < retreivedHouses.Count; i++)
            {
                initialLatitude += _latitudeIncrementForMultipleHouseSaves;
                initialLongitude += _longitudeIncrementForMultipleHouseSaves;
                string title = "MGM Grand" + i;
                string description = "It was a Hobbit Hole " + i + ". Which means it had good food and a warm hearth.";
                string phoneNumber = "0123456789" + i;
                
                string email = "dummy@dumdum123456-" + i + ".com";
                string houseNo = "123" + i;
                string area = "Harley Street" + i;
                string streetNo = i.ToString();
                string ownerName = "Owner Name " + (i + i);

                // Here we use (i + i) because these houses wer saved in the SaveMultipleHouses() method when the loop number 
                // was a multiple of 2. And the values are corresponding to the loop number(0, 2, 4, 6 ...)
                if (i != 0)
                {
                    initialLatitude += .0005M;
                    initialLongitude += .0005M;

                    title = "MGM Grand" + (i + i);
                    description = "It was a Hobbit Hole " + (i + i) + ". Which means it had good food and a warm hearth.";
                    //phoneNumber = "012345678" + (i + i);
                    if (i < 5)
                    {
                        phoneNumber = "0123456789" + (i + i);
                    }
                    else if (i >= 5)
                    {
                        phoneNumber = "012345678" + (i + i);
                    }
                    email = "dummy@dumdum123456-" + (i + i) + ".com";
                    houseNo = "123" + (i + i);
                    area = "Harley Street" + (i + i);
                    streetNo = (i + i).ToString();
                    ownerName = "Owner Name " + (i + i);
                }


                DimensionType dimensionType = DimensionType.Kanal;

                Assert.AreEqual(initialLatitude, retreivedHouses[i].Latitude);
                Assert.AreEqual(initialLongitude, retreivedHouses[i].Longitude);
                Assert.AreEqual(title, retreivedHouses[i].Title);
                Assert.AreEqual(description, retreivedHouses[i].Description);
                Assert.AreEqual(phoneNumber, retreivedHouses[i].OwnerPhoneNumber);
                Assert.AreEqual(email, retreivedHouses[i].OwnerEmail);
                Assert.AreEqual(i + i, retreivedHouses[i].NumberOfBathrooms);
                Assert.AreEqual(i + i, retreivedHouses[i].NumberOfBathrooms);
                Assert.AreEqual(i + i, retreivedHouses[i].NumberOfBedrooms);
                Assert.AreEqual(i + i, retreivedHouses[i].NumberOfKitchens);
                Assert.AreEqual(true, retreivedHouses[i].LandlinePhoneAvailable);
                Assert.AreEqual(false, retreivedHouses[i].SmokingAllowed);
                Assert.AreEqual(true, retreivedHouses[i].InternetAvailable);
                Assert.AreEqual(propertyType, retreivedHouses[i].PropertyType);

                Assert.AreEqual(houseNo, retreivedHouses[i].HouseNo);
                Assert.AreEqual(area, retreivedHouses[i].Area);
                Assert.AreEqual(streetNo, retreivedHouses[i].StreetNo);
                Assert.AreEqual(dimensionType, retreivedHouses[i].Dimension.DimensionType);
                Assert.AreEqual((i + i).ToString(), retreivedHouses[i].Dimension.StringValue);
                Assert.AreEqual(ownerName, retreivedHouses[i].OwnerName);
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