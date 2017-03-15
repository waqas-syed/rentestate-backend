using System.Collections.Generic;
using System.Configuration;
using NUnit.Framework;
using RentStuff.Common;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Domain.Model.Services;
using Spring.Context.Support;

namespace RentStuff.Property.Persistence.IntegrationTests
{
    [TestFixture]
    public class HouseRepositoryTests
    {
        private readonly decimal _latitudeIncrementForMultipleHouseSaves = 0.0005M;
        private readonly decimal _longitudeIncrementForMultipleHouseSaves = 0.0005M;
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

        #region Save and Get Houses By Id

        [Test]
        public void SaveHouseTest_TestsThatHouseUInstancesAreSavedToTheDatabaseAsExpected_VerifiesThroughDatabaseQuery()
        {
             //Save the house in the repository and retreive it. Primitive test
            IHouseRepository houseRepository = (IHouseRepository) ContextRegistry.GetContext()["HouseRepository"];
            string email = "w@12344321.com";
            string description = "It was a Hobbit Hole. Which means it had good food and a warm hearth.";
            string title = "MGM Grand";
            string phoneNumber = "123456789";
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

            House house = new House.HouseBuilder().Title(title).OwnerEmail(email).OwnerPhoneNumber(phoneNumber)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).FamiliesOnly(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(price).Latitude(latitude).Longitude(longitude)
                .HouseNo(houseNo).Area(area).StreetNo(streetNo).OwnerName(ownerName).Description(description).Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, null, 5, house);
            house.Dimension = dimension;
            houseRepository.SaveorUpdateDimension(dimension);
            houseRepository.SaveorUpdate(house);
            
            House retreivedHouse = houseRepository.GetHouseById(house.Id);

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

        #endregion Save and Get Houses By Id

        #region Save and Search Houses By Area and PropertyType

        [Test]
        [Category("Integration")]
        public void SearchHouseByCoordinatesAndPropertyTypeApartment_ChecksIfTheRepositoryCallReturnsTheExpectedResult_VerifiesByReturnValue()
        {
            // Save two houses with type House & Apartment. Search for houses with type Apartment only. Only the house
            // with type 'Apartment' should be retreived

            string area = "Pindora, Rawalpindi, Pakistan";

            IHouseRepository houseRepository = (IHouseRepository)ContextRegistry.GetContext()["HouseRepository"];
            IGeocodingService geocodingService = (IGeocodingService)ContextRegistry.GetContext()["GeocodingService"];
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(area);

            // Saving House # 1
            string houseNo = "S-123";
            string streetNo = "13";
            string title = "Special House";
            string description = "It was a Hobbit Hole. Which means it had good food and a warm hearth.";
            string phoneNumber = "123456789";
            string email = "special@spsp123456.com";
            int numberOfBathrooms = 1;
            int numberOfBedrooms = 1;
            int numberOfKitchens = 1;
            int rent = 50000;
            string ownerName = "Owner Name 1";
            PropertyType propertyType = PropertyType.Apartment;
            House house = new House.HouseBuilder().Title(title).OwnerEmail(email)
            .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberOfBathrooms).OwnerPhoneNumber(phoneNumber)
            .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).FamiliesOnly(true)
            .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(true).WithInternetAvailable(true)
            .PropertyType(propertyType).MonthlyRent(rent).Latitude(coordinatesFromAddress.Item1)
            .Longitude(coordinatesFromAddress.Item2)
            .HouseNo(houseNo).Area(area).OwnerName(ownerName).StreetNo(streetNo).Description(description).Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, "5", 0, house);
            house.Dimension = dimension;
            houseRepository.SaveorUpdateDimension(dimension);
            houseRepository.SaveorUpdate(house);

            // Saving House # 2
            string email2 = "special2@spsp123456.com";
            string description2 = "Erobor. Built deep within the mountain itself the beauty of this fortress was legend.";
            string houseNo2 = "S2-123";
            string streetNo2 = "2-13";
            string phoneNumber2 = "987654321";
            int numberOfBathrooms2 = 2;
            int numberOfBedrooms2 = 2;
            int numberOfKitchens2 = 3;
            int rent2 = 100000;
            string ownerName2 = "Owner Name 2";
            PropertyType propertyType2 = PropertyType.House;
            House house2 = new House.HouseBuilder().Title(title).OwnerEmail(email2)
            .NumberOfBedrooms(numberOfBedrooms2).NumberOfBathrooms(numberOfBathrooms2).OwnerPhoneNumber(phoneNumber2)
            .NumberOfKitchens(numberOfKitchens2).CableTvAvailable(false).FamiliesOnly(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType2).MonthlyRent(rent2).Latitude(coordinatesFromAddress.Item1)
            .Longitude(coordinatesFromAddress.Item2)
            .HouseNo(houseNo2).Area(area).OwnerName(ownerName2).StreetNo(streetNo2).Description(description2).Build();
            Dimension dimension2 = new Dimension(DimensionType.Marla, "20", 0, house2);
            house2.Dimension = dimension2;
            houseRepository.SaveorUpdateDimension(dimension2);
            houseRepository.SaveorUpdate(house2);

            var retreivedHouses = houseRepository.SearchHousesByCoordinatesAndPropertyType(coordinatesFromAddress.Item1,
                coordinatesFromAddress.Item2, PropertyType.Apartment);
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
            Assert.AreEqual(house.FamiliesOnly, retreivedHouses[0].FamiliesOnly);
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
        }

        [Test]
        [Category("Integration")]
        public void SearchHouseByCoordinatesAndPropertyTypeHouse_ChecksIfTheRepositoryCallReturnsTheExpectedResult_VerifiesByReturnValue()
        {
            // Save two houses with type House & Apartment. Search for houses with type House only. Only the house
            // with type 'House' should be retreived

            string area = "Pindora, Rawalpindi, Pakistan";

            IHouseRepository houseRepository = (IHouseRepository)ContextRegistry.GetContext()["HouseRepository"];
            IGeocodingService geocodingService = (IGeocodingService)ContextRegistry.GetContext()["GeocodingService"];
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(area);

            // Saving House # 1
            string houseNo = "S-123";
            string streetNo = "13";
            string description = "It was a Hobbit Hole. Which means it had good food and a warm hearth.";
            string title = "Special House";
            string phoneNumber = "123456789";
            string email = "special@spsp123456.com";
            int numberOfBathrooms = 1;
            int numberOfBedrooms = 1;
            int numberOfKitchens = 1;
            int rent = 50000;
            string ownerName = "Owner Name 1";
            PropertyType propertyType = PropertyType.Apartment;
            House house = new House.HouseBuilder().Title(title).OwnerEmail(email)
            .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberOfBathrooms).OwnerPhoneNumber(phoneNumber)
            .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).FamiliesOnly(true)
            .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(true).WithInternetAvailable(true)
            .PropertyType(propertyType).MonthlyRent(rent).Latitude(coordinatesFromAddress.Item1)
            .Longitude(coordinatesFromAddress.Item2)
            .HouseNo(houseNo).Area(area).StreetNo(streetNo).OwnerName(ownerName).Description(description).Build();
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
            string phoneNumber2 = "987654321";
            int numberOfBathrooms2 = 2;
            int numberOfBedrooms2 = 2;
            int numberOfKitchens2 = 3;
            int rent2 = 100000;
            string ownerName2 = "Owner Name 2";
            PropertyType propertyType2 = PropertyType.House;
            House house2 = new House.HouseBuilder().Title(title2).OwnerEmail(email2)
            .NumberOfBedrooms(numberOfBedrooms2).NumberOfBathrooms(numberOfBathrooms2).OwnerPhoneNumber(phoneNumber2)
            .NumberOfKitchens(numberOfKitchens2).CableTvAvailable(false).FamiliesOnly(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType2).MonthlyRent(rent2).Latitude(coordinatesFromAddress.Item1)
            .Longitude(coordinatesFromAddress.Item2)
            .HouseNo(houseNo2).Area(area).StreetNo(streetNo2).OwnerName(ownerName2).Description(description2).Build();
            Dimension dimension2 = new Dimension(DimensionType.Marla, "20", 0, house2);
            house2.Dimension = dimension2;
            houseRepository.SaveorUpdateDimension(dimension2);
            houseRepository.SaveorUpdate(house2);

            var retreivedHouses = houseRepository.SearchHousesByCoordinatesAndPropertyType(coordinatesFromAddress.Item1,
                coordinatesFromAddress.Item2, PropertyType.House);
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
            Assert.AreEqual(house2.FamiliesOnly, retreivedHouses[0].FamiliesOnly);
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
        }

        [Test]
        [Category("Integration")]
        public void SearchMultipleHousesByCoordinatesAndPropertyType_ChecksIfNearbyHousesAreReturned_VerifiesByReturnValue()
        {
            // Save 3 houses in locations nearby, 2 have same PropertyType but 1 has different
            // Save 2 houses that are in other places. 
            // Search should get the 2 houses located near the serched location and also the exact PropertyType

            IHouseRepository houseRepository = (IHouseRepository)ContextRegistry.GetContext()["HouseRepository"];
            IGeocodingService geocodingService = (IGeocodingService)ContextRegistry.GetContext()["GeocodingService"];

            // Saving House # 1: Should be in the search results
            string area = "Pindora, Rawalpindi, Pakistan";
            string description = "It was a Hobbit Hole. Which means it had good food and a warm hearth.";
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(area);
            string houseNo = "House # 1";
            string streetNo = "1";
            string title = "Title # 1";
            string phoneNumber = "1234567891";
            string email = "special@spsp123456-1.com";
            int numberOfBathrooms = 1;
            int numberOfBedrooms = 1;
            int numberOfKitchens = 1;
            int rent = 100;
            string ownerName = "Owner Name 1";
            PropertyType propertyType = PropertyType.House;
            House house = new House.HouseBuilder().Title(title).OwnerEmail(email)
            .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberOfBathrooms).OwnerPhoneNumber(phoneNumber)
            .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).FamiliesOnly(true)
            .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(true).WithInternetAvailable(true)
            .PropertyType(propertyType).MonthlyRent(rent).Latitude(coordinatesFromAddress.Item1)
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
            string phoneNumber2 = "1234567892";
            int numberOfBathrooms2 = 2;
            int numberOfBedrooms2 = 2;
            int numberOfKitchens2 = 2;
            int rent2 = 200;
            string ownerName2 = "Owner Name 2";
            PropertyType propertyType2 = PropertyType.Apartment;

            House house2 = new House.HouseBuilder().Title(title2).OwnerEmail(email2)
            .NumberOfBedrooms(numberOfBedrooms2).NumberOfBathrooms(numberOfBathrooms2).OwnerPhoneNumber(phoneNumber2)
            .NumberOfKitchens(numberOfKitchens2).CableTvAvailable(false).FamiliesOnly(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType2).MonthlyRent(rent2).Latitude(coordinatesFromAddress2.Item1)
            .Longitude(coordinatesFromAddress2.Item2)
            .HouseNo(houseNo2).Area(area2).StreetNo(streetNo2).OwnerName(ownerName2).Description(description2).Build();
            Dimension dimension2 = new Dimension(DimensionType.Kanal, "2", 0, house2);
            house2.Dimension = dimension2;
            houseRepository.SaveorUpdateDimension(dimension2);
            houseRepository.SaveorUpdate(house2);

            // Saving House # 3: Should NOT be in the search results, outside bounds of search location
            string area3 = "Kahuta, Pakistan";
            var coordinatesFromAddress3 = geocodingService.GetCoordinatesFromAddress(area3);
            string title3 = "Title # 3";
            string description3 = "It was a Hobbit Hole 3. Which means it had good food and a warm hearth.";
            string email3 = "special2@spsp123456-3.com";
            string houseNo3 = "House # 3";
            string streetNo3 = "3";
            string phoneNumber3 = "1234567893";
            int numberOfBathrooms3 = 3;
            int numberOfBedrooms3 = 3;
            int numberOfKitchens3 = 3;
            int rent3 = 300;
            string ownerName3 = "Owner Name 3";
            PropertyType propertyType3 = PropertyType.House;

            House house3 = new House.HouseBuilder().Title(title3).OwnerEmail(email3)
            .NumberOfBedrooms(numberOfBedrooms3).NumberOfBathrooms(numberOfBathrooms3).OwnerPhoneNumber(phoneNumber3)
            .NumberOfKitchens(numberOfKitchens3).CableTvAvailable(false).FamiliesOnly(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType3).MonthlyRent(rent3).Latitude(coordinatesFromAddress3.Item1)
            .Longitude(coordinatesFromAddress3.Item2)
            .HouseNo(houseNo3).Area(area3).StreetNo(streetNo3).OwnerName(ownerName3).Description(description3).Build();
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
            string phoneNumber4 = "1234567894";
            int numberOfBathrooms4 = 4;
            int numberOfBedrooms4 = 4;
            int numberOfKitchens4 = 4;
            int rent4 = 400;
            string ownerName4 = "Owner Name 4";
            PropertyType propertyType4 = PropertyType.House;

            House house4 = new House.HouseBuilder().Title(title4).OwnerEmail(email4)
            .NumberOfBedrooms(numberOfBedrooms4).NumberOfBathrooms(numberOfBathrooms4).OwnerPhoneNumber(phoneNumber4)
            .NumberOfKitchens(numberOfKitchens4).CableTvAvailable(false).FamiliesOnly(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType4).MonthlyRent(rent4).Latitude(coordinatesFromAddress4.Item1)
            .Longitude(coordinatesFromAddress4.Item2)
            .HouseNo(houseNo4).Area(area4).StreetNo(streetNo4).OwnerName(ownerName4).Description(description4).Build();
            Dimension dimension4 = new Dimension(DimensionType.Kanal, "4", 0, house4);
            house4.Dimension = dimension4;
            houseRepository.SaveorUpdateDimension(dimension4);
            houseRepository.SaveorUpdate(house4);

            // Saving House # 5: Should NOT be in the search results, outside bounds of search location
            string area5 = "Saddar, Rawalpindi, Punjab, Pakistan";
            var coordinatesFromAddress5 = geocodingService.GetCoordinatesFromAddress(area5);
            string title5 = "Title # 5";
            string description5 = "It was a Hobbit Hole 5. Which means it had good food and a warm hearth.";
            string email5 = "special2@spsp123456-5.com";
            string houseNo5 = "House # 5";
            string streetNo5 = "5";
            string phoneNumber5 = "1234567895";
            int numberOfBathrooms5 = 5;
            int numberOfBedrooms5 = 5;
            int numberOfKitchens5 = 5;
            int rent5 = 500;
            string ownerName5 = "Owner Name 5";
            PropertyType propertyType5 = PropertyType.House;

            House house5 = new House.HouseBuilder().Title(title5).OwnerEmail(email5)
            .NumberOfBedrooms(numberOfBedrooms5).NumberOfBathrooms(numberOfBathrooms5).OwnerPhoneNumber(phoneNumber5)
            .NumberOfKitchens(numberOfKitchens5).CableTvAvailable(false).FamiliesOnly(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType5).MonthlyRent(rent5).Latitude(coordinatesFromAddress5.Item1)
            .Longitude(coordinatesFromAddress5.Item2)
            .HouseNo(houseNo5).Area(area5).StreetNo(streetNo5).OwnerName(ownerName5).Description(description5).Build();
            Dimension dimension5 = new Dimension(DimensionType.Kanal, "5", 0, house5);
            house5.Dimension = dimension5;
            houseRepository.SaveorUpdateDimension(dimension5);
            houseRepository.SaveorUpdate(house5);

            var retreivedHouses = houseRepository.SearchHousesByCoordinatesAndPropertyType(coordinatesFromAddress.Item1,
                coordinatesFromAddress.Item2, PropertyType.House);
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
            Assert.AreEqual(house.FamiliesOnly, retreivedHouses[0].FamiliesOnly);
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

            // Verification of House # 4
            Assert.AreEqual(title4, retreivedHouses[1].Title);
            Assert.AreEqual(house4.Title, retreivedHouses[1].Title);
            Assert.AreEqual(description4, retreivedHouses[1].Description);
            Assert.AreEqual(house4.Description, retreivedHouses[1].Description);
            Assert.AreEqual(house4.NumberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(house4.NumberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(house4.NumberOfBedrooms, retreivedHouses[1].NumberOfBedrooms);
            Assert.AreEqual(house4.NumberOfKitchens, retreivedHouses[1].NumberOfKitchens);
            Assert.AreEqual(house4.FamiliesOnly, retreivedHouses[1].FamiliesOnly);
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
        }

        [Test]
        public void RetrieveHouseByCoordinatesAndPropertyType_GetsTheHousesUsingTheirCoordinates_VerifiesThroughReturnValue()
        {
            // Save multiple houses, retreive them by specifying coordinates and verify all of them
            decimal initialLatitude = 33.29M;
            decimal initialLongitude = 73.41M;
            PropertyType propertyType = PropertyType.House;
            IHouseRepository houseRepository = (IHouseRepository)ContextRegistry.GetContext()["HouseRepository"];
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

        #region Save and Search Houses By Area Only

        [Test]
        public void RetrieveSingleHouseByCoordinates_GetsTheHouseUsingItsCoordinates_VerifiesThroughReturnValue()
        {
            // Save two houses with the same coordinates and get both of them 
            string area = "Pindora, Rawalpindi, Pakistan";

            IHouseRepository houseRepository = (IHouseRepository)ContextRegistry.GetContext()["HouseRepository"];
            IGeocodingService geocodingService = (IGeocodingService)ContextRegistry.GetContext()["GeocodingService"];
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(area);

            // Saving House # 1
            string houseNo = "S-123";
            string streetNo = "13";
            string title = "Special House";
            string phoneNumber = "123456789";
            string email = "special@spsp123456.com";
            int numberOfBathrooms = 1;
            int numberOfBedrooms = 1;
            int numberOfKitchens = 1;
            int rent = 50000;
            string ownerName = "Owner Name 1";
            House house = new House.HouseBuilder().Title(title).OwnerEmail(email)
            .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberOfBathrooms).OwnerPhoneNumber(phoneNumber)
            .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).FamiliesOnly(true)
            .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(true).WithInternetAvailable(true)
            .PropertyType(PropertyType.Apartment).MonthlyRent(rent).Latitude(coordinatesFromAddress.Item1)
            .Longitude(coordinatesFromAddress.Item2)
            .HouseNo(houseNo).Area(area).StreetNo(streetNo).OwnerName(ownerName).Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, "5", 0, house);
            house.Dimension = dimension;
            houseRepository.SaveorUpdateDimension(dimension);
            houseRepository.SaveorUpdate(house);

            // Saving House # 2
            string email2 = "special2@spsp123456.com";
            string houseNo2 = "S2-123";
            string streetNo2 = "2-13";
            string phoneNumber2 = "987654321";
            int numberOfBathrooms2 = 2;
            int numberOfBedrooms2 = 2;
            int numberOfKitchens2 = 3;
            int rent2 = 100000;
            string ownerName2 = "Owner Name 2";
            House house2 = new House.HouseBuilder().Title(title).OwnerEmail(email2)
            .NumberOfBedrooms(numberOfBedrooms2).NumberOfBathrooms(numberOfBathrooms2).OwnerPhoneNumber(phoneNumber2)
            .NumberOfKitchens(numberOfKitchens2).CableTvAvailable(false).FamiliesOnly(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(PropertyType.House).MonthlyRent(rent2).Latitude(coordinatesFromAddress.Item1)
            .Longitude(coordinatesFromAddress.Item2)
            .HouseNo(houseNo2).Area(area).StreetNo(streetNo2).OwnerName(ownerName2).Build();
            Dimension dimension2 = new Dimension(DimensionType.Marla, "20", 0, house2);
            house2.Dimension = dimension2;
            houseRepository.SaveorUpdateDimension(dimension2);
            houseRepository.SaveorUpdate(house2);

            var retreivedHouses = houseRepository.SearchHousesByCoordinates(coordinatesFromAddress.Item1, coordinatesFromAddress.Item2);
            Assert.NotNull(retreivedHouses);

            // Verification of House # 1
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouses[0].NumberOfBedrooms);
            Assert.AreEqual(house.NumberOfKitchens, retreivedHouses[0].NumberOfKitchens);
            Assert.AreEqual(house.FamiliesOnly, retreivedHouses[0].FamiliesOnly);
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

            // Verification of House # 2
            Assert.AreEqual(house2.NumberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(house2.NumberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(house2.NumberOfBedrooms, retreivedHouses[1].NumberOfBedrooms);
            Assert.AreEqual(house2.NumberOfKitchens, retreivedHouses[1].NumberOfKitchens);
            Assert.AreEqual(house2.FamiliesOnly, retreivedHouses[1].FamiliesOnly);
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
        }

        [Test]
        public void RetrieveHouseByCoordinates_GetsTheHousesUsingTheirCoordinates_VerifiesThroughReturnValue()
        {
            // Save multiple houses, retreive them by specifying coordinates and verify all of them
            decimal initialLatitude = 33.29M;
            decimal initialLongitude = 73.41M;
            IHouseRepository houseRepository = (IHouseRepository)ContextRegistry.GetContext()["HouseRepository"];
            SaveMultipleHouses(houseRepository, initialLatitude, initialLongitude);
            IList<House> retreivedHouses = houseRepository.SearchHousesByCoordinates(initialLatitude, initialLongitude);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(20, retreivedHouses.Count);
            VerifyRetereivedHouses(retreivedHouses, initialLatitude, initialLongitude);
        }

        [Test]
        public void RetrieveHouseByCoordinatesFailTest_ChecksByProvidingTheWrongCoordinatesThatNoHouseIsRetreived_VerifiesThroughReturnValue()
        {
            // Save multiple houses and 
            decimal initialLatitude = 33.29M;
            decimal initialLongitude = 73.41M;
            IHouseRepository houseRepository = (IHouseRepository)ContextRegistry.GetContext()["HouseRepository"];
            SaveMultipleHouses(houseRepository, initialLatitude, initialLongitude);
            IList<House> retreivedHouses = houseRepository.SearchHousesByCoordinates(29.00M, 69.00M);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(0, retreivedHouses.Count);
        }

        [Test]
        [Category("Integration")]
        public void SearchMultipleHousesByCoordinates_ChecksIfNearbyHousesAreReturned_VerifiesByReturnValue()
        {
            // Save 3 houses in locations nearby and 2 houses that are in other places. 
            // Search should get the 3 houses located near theserched location

            IHouseRepository houseRepository = (IHouseRepository)ContextRegistry.GetContext()["HouseRepository"];
            IGeocodingService geocodingService = (IGeocodingService)ContextRegistry.GetContext()["GeocodingService"];

            // Saving House # 1: Near the search location, should be in the search results
            string area = "Pindora, Rawalpindi, Pakistan";
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(area);
            string houseNo = "House # 1";
            string streetNo = "1";
            string title = "Title # 1";
            string phoneNumber = "1234567891";
            string email = "special@spsp123456-1.com";
            int numberOfBathrooms = 1;
            int numberOfBedrooms = 1;
            int numberOfKitchens = 1;
            int rent = 100;
            string ownerName = "Owner Name 1";
            PropertyType propertyType = PropertyType.House;
            House house = new House.HouseBuilder().Title(title).OwnerEmail(email)
            .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberOfBathrooms).OwnerPhoneNumber(phoneNumber)
            .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).FamiliesOnly(true)
            .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(true).WithInternetAvailable(true)
            .PropertyType(propertyType).MonthlyRent(rent).Latitude(coordinatesFromAddress.Item1)
            .Longitude(coordinatesFromAddress.Item2)
            .HouseNo(houseNo).Area(area).StreetNo(streetNo).OwnerName(ownerName).Build();
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
            string phoneNumber2 = "1234567892";
            int numberOfBathrooms2 = 2;
            int numberOfBedrooms2 = 2;
            int numberOfKitchens2 = 2;
            int rent2 = 200;
            string ownerName2 = "Owner Name 2";
            PropertyType propertyType2 = PropertyType.House;

            House house2 = new House.HouseBuilder().Title(title2).OwnerEmail(email2)
            .NumberOfBedrooms(numberOfBedrooms2).NumberOfBathrooms(numberOfBathrooms2).OwnerPhoneNumber(phoneNumber2)
            .NumberOfKitchens(numberOfKitchens2).CableTvAvailable(false).FamiliesOnly(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType2).MonthlyRent(rent2).Latitude(coordinatesFromAddress2.Item1)
            .Longitude(coordinatesFromAddress2.Item2)
            .HouseNo(houseNo2).Area(area2).StreetNo(streetNo2).OwnerName(ownerName2).Build();
            Dimension dimension2 = new Dimension(DimensionType.Kanal, "2", 0, house2);
            house2.Dimension = dimension2;
            houseRepository.SaveorUpdateDimension(dimension2);
            houseRepository.SaveorUpdate(house2);

            // Saving House # 3: Outside the bounds of the search location, should not be in the search results
            string area3 = "Kahuta, Pakistan";
            var coordinatesFromAddress3 = geocodingService.GetCoordinatesFromAddress(area3);
            string title3 = "Title # 3";
            string email3 = "special2@spsp123456-3.com";
            string houseNo3 = "House # 3";
            string streetNo3 = "3";
            string phoneNumber3 = "1234567893";
            int numberOfBathrooms3 = 3;
            int numberOfBedrooms3 = 3;
            int numberOfKitchens3 = 3;
            int rent3 = 300;
            string ownerName3 = "Owner Name 3";
            PropertyType propertyType3 = PropertyType.House;

            House house3 = new House.HouseBuilder().Title(title3).OwnerEmail(email3)
            .NumberOfBedrooms(numberOfBedrooms3).NumberOfBathrooms(numberOfBathrooms3).OwnerPhoneNumber(phoneNumber3)
            .NumberOfKitchens(numberOfKitchens3).CableTvAvailable(false).FamiliesOnly(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType3).MonthlyRent(rent3).Latitude(coordinatesFromAddress3.Item1)
            .Longitude(coordinatesFromAddress3.Item2)
            .HouseNo(houseNo3).Area(area3).StreetNo(streetNo3).OwnerName(ownerName3).Build();
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
            string phoneNumber4 = "1234567894";
            int numberOfBathrooms4 = 4;
            int numberOfBedrooms4 = 4;
            int numberOfKitchens4 = 4;
            int rent4 = 400;
            string ownerName4 = "Owner Name 4";
            PropertyType propertyType4 = PropertyType.House;

            House house4 = new House.HouseBuilder().Title(title4).OwnerEmail(email4)
            .NumberOfBedrooms(numberOfBedrooms4).NumberOfBathrooms(numberOfBathrooms4).OwnerPhoneNumber(phoneNumber4)
            .NumberOfKitchens(numberOfKitchens4).CableTvAvailable(false).FamiliesOnly(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType4).MonthlyRent(rent4).Latitude(coordinatesFromAddress4.Item1)
            .Longitude(coordinatesFromAddress4.Item2)
            .HouseNo(houseNo4).Area(area4).StreetNo(streetNo4).OwnerName(ownerName4).Build();
            Dimension dimension4 = new Dimension(DimensionType.Kanal, "4", 0, house4);
            house4.Dimension = dimension4;
            houseRepository.SaveorUpdateDimension(dimension4);
            houseRepository.SaveorUpdate(house4);

            // Saving House # 5: Should NOT be in the search results
            string area5 = "Saddar, Rawalpindi, Punjab, Pakistan";
            var coordinatesFromAddress5 = geocodingService.GetCoordinatesFromAddress(area5);
            string title5 = "Title # 5";
            string email5 = "special2@spsp123456-5.com";
            string houseNo5 = "House # 5";
            string streetNo5 = "5";
            string phoneNumber5 = "1234567895";
            int numberOfBathrooms5 = 5;
            int numberOfBedrooms5 = 5;
            int numberOfKitchens5 = 5;
            int rent5 = 100000;
            string ownerName5 = "Owner Name 5";
            PropertyType propertyType5 = PropertyType.House;

            House house5 = new House.HouseBuilder().Title(title5).OwnerEmail(email5)
            .NumberOfBedrooms(numberOfBedrooms5).NumberOfBathrooms(numberOfBathrooms5).OwnerPhoneNumber(phoneNumber5)
            .NumberOfKitchens(numberOfKitchens5).CableTvAvailable(false).FamiliesOnly(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType5).MonthlyRent(rent5).Latitude(coordinatesFromAddress5.Item1)
            .Longitude(coordinatesFromAddress5.Item2)
            .HouseNo(houseNo5).Area(area5).StreetNo(streetNo5).OwnerName(ownerName5).Build();
            Dimension dimension5 = new Dimension(DimensionType.Kanal, "5", 0, house5);
            house5.Dimension = dimension5;
            houseRepository.SaveorUpdateDimension(dimension5);
            houseRepository.SaveorUpdate(house5);

            var retreivedHouses = houseRepository.SearchHousesByCoordinatesAndPropertyType(coordinatesFromAddress.Item1,
                coordinatesFromAddress.Item2, PropertyType.House);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(3, retreivedHouses.Count);

            // Verification of House # 1
            Assert.AreEqual(title, retreivedHouses[0].Title);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouses[0].NumberOfBedrooms);
            Assert.AreEqual(house.NumberOfKitchens, retreivedHouses[0].NumberOfKitchens);
            Assert.AreEqual(house.FamiliesOnly, retreivedHouses[0].FamiliesOnly);
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

            // Verification of House # 2
            Assert.AreEqual(title2, retreivedHouses[1].Title);
            Assert.AreEqual(house2.NumberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(house2.NumberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(house2.NumberOfBedrooms, retreivedHouses[1].NumberOfBedrooms);
            Assert.AreEqual(house2.NumberOfKitchens, retreivedHouses[1].NumberOfKitchens);
            Assert.AreEqual(house2.FamiliesOnly, retreivedHouses[1].FamiliesOnly);
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

            // Verification of House # 4
            Assert.AreEqual(title4, retreivedHouses[2].Title);
            Assert.AreEqual(house4.NumberOfBathrooms, retreivedHouses[2].NumberOfBathrooms);
            Assert.AreEqual(house4.NumberOfBathrooms, retreivedHouses[2].NumberOfBathrooms);
            Assert.AreEqual(house4.NumberOfBedrooms, retreivedHouses[2].NumberOfBedrooms);
            Assert.AreEqual(house4.NumberOfKitchens, retreivedHouses[2].NumberOfKitchens);
            Assert.AreEqual(house4.FamiliesOnly, retreivedHouses[2].FamiliesOnly);
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
        }
        
        #endregion Save and Search Houses By Area Only

        #region Save and Search Houses By PropertyType Only

        [Test]
        [Category("Integration")]
        public void SearchMultipleHousesByPropertyTypeOnly_ChecksIfNearbyHousesAreReturnedByPropertyTypeProperly_VerifiesByReturnValue()
        {
            // Save 5 houses in locations nearby, 2 have same PropertyType as the search criteria but 3 have different
            // Search should get the 2 houses with the same propertyType, and ignore the 3 who have a different propertyType, even
            // though they are located within the search radius (2 kilometers)

            IHouseRepository houseRepository = (IHouseRepository)ContextRegistry.GetContext()["HouseRepository"];
            IGeocodingService geocodingService = (IGeocodingService)ContextRegistry.GetContext()["GeocodingService"];

            // Saving House # 1: Should be in the search results
            string area = "Pindora, Rawalpindi, Pakistan";
            string description = "It was a Hobbit Hole. Which means it had good food and a warm hearth.";
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(area);
            string houseNo = "House # 1";
            string streetNo = "1";
            string title = "Title # 1";
            string phoneNumber = "1234567891";
            string email = "special@spsp123456-1.com";
            int numberOfBathrooms = 1;
            int numberOfBedrooms = 1;
            int numberOfKitchens = 1;
            int rent = 100;
            string ownerName = "Owner Name 1";
            PropertyType propertyType = PropertyType.House;
            House house = new House.HouseBuilder().Title(title).OwnerEmail(email)
            .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberOfBathrooms).OwnerPhoneNumber(phoneNumber)
            .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).FamiliesOnly(true)
            .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(true).WithInternetAvailable(true)
            .PropertyType(propertyType).MonthlyRent(rent).Latitude(coordinatesFromAddress.Item1)
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
            string phoneNumber2 = "1234567892";
            int numberOfBathrooms2 = 2;
            int numberOfBedrooms2 = 2;
            int numberOfKitchens2 = 2;
            int rent2 = 200;
            string ownerName2 = "Owner Name 2";
            PropertyType propertyType2 = PropertyType.Apartment;

            House house2 = new House.HouseBuilder().Title(title2).OwnerEmail(email2)
            .NumberOfBedrooms(numberOfBedrooms2).NumberOfBathrooms(numberOfBathrooms2).OwnerPhoneNumber(phoneNumber2)
            .NumberOfKitchens(numberOfKitchens2).CableTvAvailable(false).FamiliesOnly(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType2).MonthlyRent(rent2).Latitude(coordinatesFromAddress2.Item1)
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
            string phoneNumber3 = "1234567893";
            int numberOfBathrooms3 = 3;
            int numberOfBedrooms3 = 3;
            int numberOfKitchens3 = 3;
            int rent3 = 300;
            string ownerName3 = "Owner Name 3";
            PropertyType propertyType3 = PropertyType.House;

            House house3 = new House.HouseBuilder().Title(title3).OwnerEmail(email3)
            .NumberOfBedrooms(numberOfBedrooms3).NumberOfBathrooms(numberOfBathrooms3).OwnerPhoneNumber(phoneNumber3)
            .NumberOfKitchens(numberOfKitchens3).CableTvAvailable(false).FamiliesOnly(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType3).MonthlyRent(rent3).Latitude(coordinatesFromAddress3.Item1)
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
            string phoneNumber4 = "1234567894";
            int numberOfBathrooms4 = 4;
            int numberOfBedrooms4 = 4;
            int numberOfKitchens4 = 4;
            int rent4 = 400;
            string ownerName4 = "Owner Name 4";
            PropertyType propertyType4 = PropertyType.Hostel;

            House house4 = new House.HouseBuilder().Title(title4).OwnerEmail(email4)
            .NumberOfBedrooms(numberOfBedrooms4).NumberOfBathrooms(numberOfBathrooms4).OwnerPhoneNumber(phoneNumber4)
            .NumberOfKitchens(numberOfKitchens4).CableTvAvailable(false).FamiliesOnly(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType4).MonthlyRent(rent4).Latitude(coordinatesFromAddress4.Item1)
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
            string phoneNumber5 = "1234567895";
            int numberOfBathrooms5 = 5;
            int numberOfBedrooms5 = 5;
            int numberOfKitchens5 = 5;
            int rent5 = 500;
            string ownerName5 = "Owner Name 5";
            PropertyType propertyType5 = PropertyType.Hotel;

            House house5 = new House.HouseBuilder().Title(title5).OwnerEmail(email5)
            .NumberOfBedrooms(numberOfBedrooms5).NumberOfBathrooms(numberOfBathrooms5).OwnerPhoneNumber(phoneNumber5)
            .NumberOfKitchens(numberOfKitchens5).CableTvAvailable(false).FamiliesOnly(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType5).MonthlyRent(rent5).Latitude(coordinatesFromAddress5.Item1)
            .Longitude(coordinatesFromAddress5.Item2)
            .HouseNo(houseNo5).Area(area5).StreetNo(streetNo5).OwnerName(ownerName5).Description(description5).Build();
            Dimension dimension5 = new Dimension(DimensionType.Kanal, "5", 0, house5);
            house5.Dimension = dimension5;
            houseRepository.SaveorUpdateDimension(dimension5);
            houseRepository.SaveorUpdate(house5);

            var retreivedHouses = houseRepository.SearchHousesByPropertyType(PropertyType.House);
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
            Assert.AreEqual(house.FamiliesOnly, retreivedHouses[0].FamiliesOnly);
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

            // Verification of House # 3
            Assert.AreEqual(title3, retreivedHouses[1].Title);
            Assert.AreEqual(house3.Title, retreivedHouses[1].Title);
            Assert.AreEqual(description3, retreivedHouses[1].Description);
            Assert.AreEqual(house3.Description, retreivedHouses[1].Description);
            Assert.AreEqual(house3.NumberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(house3.NumberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(house3.NumberOfBedrooms, retreivedHouses[1].NumberOfBedrooms);
            Assert.AreEqual(house3.NumberOfKitchens, retreivedHouses[1].NumberOfKitchens);
            Assert.AreEqual(house3.FamiliesOnly, retreivedHouses[1].FamiliesOnly);
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
        }
        
        [Test]
        public void RetrieveHouseByPropertyTypeOnly_GetsTheHousesUsingTheirPropertyType_VerifiesThroughReturnValue()
        {
            // Save multiple houses, retreive them by specifying coordinates and verify all of them
            decimal initialLatitude = 33.29M;
            decimal initialLongitude = 73.41M;
            PropertyType propertyType = PropertyType.House;
            IHouseRepository houseRepository = (IHouseRepository)ContextRegistry.GetContext()["HouseRepository"];
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

        #region Save and Search Houses by Email

        [Test]
        public void SaveHouseAndRetreiveByEmailTest_TestsThatHouseUInstancesAreSavedToTheDatabaseAsExpected_VerifiesThroughDatabaseQuery()
        {
            IHouseRepository houseRepository = (IHouseRepository)ContextRegistry.GetContext()["HouseRepository"];
            string email = "w@12344321.com";
            string description = "It was a Hobbit Hole. Which means it had good food and a warm hearth.";
            string title = "MGM Grand";
            string phoneNumber = "123456789";

            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long price = 90000;
            string ownerName = "Owner Name 1";

            House house = new House.HouseBuilder().Title(title).OwnerPhoneNumber(phoneNumber).OwnerEmail(email)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).FamiliesOnly(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(price).Latitude(33.29M).Longitude(73.41M)
                .HouseNo("123").Area("Pindora").StreetNo("13").OwnerName(ownerName).Description(description).Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, "50", 0, house);
            house.Dimension = dimension;
            houseRepository.SaveorUpdateDimension(dimension);
            houseRepository.SaveorUpdate(house);

            IList<House> retreivedHouses = houseRepository.GetHouseByOwnerEmail(email);

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
            Assert.AreEqual(house.FamiliesOnly, retreivedHouse.FamiliesOnly);
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
            IHouseRepository houseRepository = (IHouseRepository)ContextRegistry.GetContext()["HouseRepository"];
            string email = "w@12344321.com";
            string title = "MGM Grand";
            string phoneNumber = "123456789";

            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long price = 90000;
            string ownerName = "Owner Name 1";

            House house = new House.HouseBuilder().Title(title).OwnerEmail(email).OwnerPhoneNumber(phoneNumber)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).FamiliesOnly(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(price).Latitude(33.29M).Longitude(73.41M)
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

            House retreivedHouse = houseRepository.GetHouseById(house.Id);
            Assert.IsNotNull(retreivedHouse);
            Assert.AreEqual(3, retreivedHouse.HouseImages.Count);

            Assert.AreEqual(title, retreivedHouse.Title);
            Assert.AreEqual(phoneNumber, retreivedHouse.OwnerPhoneNumber);
            Assert.AreEqual(email, retreivedHouse.OwnerEmail);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouse.NumberOfBedrooms);
            Assert.AreEqual(house.NumberOfKitchens, retreivedHouse.NumberOfKitchens);
            Assert.AreEqual(house.FamiliesOnly, retreivedHouse.FamiliesOnly);
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

            Assert.AreEqual(image1, retreivedHouse.HouseImages[0]);
            Assert.AreEqual(image2, retreivedHouse.HouseImages[1]);
            Assert.AreEqual(image3, retreivedHouse.HouseImages[2]);

            IList<House> allHouses = houseRepository.GetAllHouses();
            Assert.IsNotNull(allHouses);
            Assert.AreNotEqual(0, allHouses.Count);
        }

        #endregion Save and Search House Images

        #region Utility methods

        /// <summary>
        /// Values are saved in this method based on the loop number, so they can be verified in batches after they 
        /// are retreived
        /// </summary>
        /// <param name="houseRepository"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        private void SaveMultipleHouses(IHouseRepository houseRepository, decimal latitude, decimal longitude)
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
                string phoneNumber = "123456789" + i;
                string email = "dummy@dumdum123456-" + i + ".com";
                string houseNo = "123" + i;
                string area = "Harley Street" + i;
                string streetNo = i.ToString();
                DimensionType dimensionType = DimensionType.Kanal;
                string ownerName = "Owner Name " + i;

                var propertyType = i % 2 == 0 ? PropertyType.House : PropertyType.Apartment;

                House house = new House.HouseBuilder().Title(title).OwnerEmail(email)
                    .NumberOfBedrooms(i).NumberOfBathrooms(i).OwnerPhoneNumber(phoneNumber)
                    .NumberOfKitchens(i).CableTvAvailable(false).FamiliesOnly(true)
                    .GarageAvailable(false)
                    .LandlinePhoneAvailable(true)
                    .SmokingAllowed(false)
                    .WithInternetAvailable(true)
                    .PropertyType(propertyType)
                    .Latitude(initialLatitude)
                    .Longitude(initialLongitude)
                    .MonthlyRent(rentPrice)
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
            PropertyType propertyType)
        {
            decimal initialLatitude = latitude;
            decimal initialLongitude = longitude;
            for (int i = 0; i < retreivedHouses.Count; i++)
            {
                initialLatitude += _latitudeIncrementForMultipleHouseSaves;
                initialLongitude += _longitudeIncrementForMultipleHouseSaves;
                string title = "MGM Grand" + i;
                string description = "It was a Hobbit Hole " + i + ". Which means it had good food and a warm hearth.";
                string phoneNumber = "123456789" + i;
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
                    phoneNumber = "123456789" + (i + i);
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
                Assert.AreEqual(true, retreivedHouses[i].FamiliesOnly);
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
                string phoneNumber = "123456789" + i;
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
                Assert.AreEqual(true, retreivedHouses[i].FamiliesOnly);
                Assert.AreEqual(true, retreivedHouses[i].LandlinePhoneAvailable);
                Assert.AreEqual(false, retreivedHouses[i].SmokingAllowed);
                Assert.AreEqual(true, retreivedHouses[i].InternetAvailable);
                Assert.AreEqual(i % 2 == 0 ? PropertyType.House : PropertyType.Apartment, retreivedHouses[i].PropertyType);

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