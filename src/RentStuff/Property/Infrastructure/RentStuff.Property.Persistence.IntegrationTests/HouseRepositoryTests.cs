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
            //_databaseUtility.Create();
        }

        [Test]
        public void SaveHouseTest_TestsThatHouseUInstancesAreSavedToTheDatabaseAsExpected_VerifiesThroughDatabaseQuery()
        {
             //Save the house in the repository and retreive it. Primitive test
            IHouseRepository houseRepository = (IHouseRepository) ContextRegistry.GetContext()["HouseRepository"];
            string email = "w@12344321.com";
            string title = "MGM Grand";
            string phoneNumber = "123456789";
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long price = 90000;
            
            House house = new House.HouseBuilder().Title(title).OwnerEmail(email).OwnerPhoneNumber(phoneNumber)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).FamiliesOnly(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(price).Latitude(33.29M).Longitude(73.41M)
                .HouseNo("123").Area("Pindora").StreetNo("13").Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, null, 5, house);
            house.Dimension = dimension;
            houseRepository.SaveorUpdateDimension(dimension);
            houseRepository.SaveorUpdate(house);
            
            House retreivedHouse = houseRepository.GetHouseById(house.Id);

            Assert.NotNull(retreivedHouse);
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
        }

        [Test]
        public void SaveHouseAndRetreiveByEmailTest_TestsThatHouseUInstancesAreSavedToTheDatabaseAsExpected_VerifiesThroughDatabaseQuery()
        {
            IHouseRepository houseRepository = (IHouseRepository)ContextRegistry.GetContext()["HouseRepository"];
            string email = "w@12344321.com";
            string title = "MGM Grand";
            string phoneNumber = "123456789";

            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long price = 90000;
            
            House house = new House.HouseBuilder().Title(title).OwnerPhoneNumber(phoneNumber).OwnerEmail(email)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).FamiliesOnly(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(price).Latitude(33.29M).Longitude(73.41M)
                .HouseNo("123").Area("Pindora").StreetNo("13").Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, "50", 0, house);
            house.Dimension = dimension;
            houseRepository.SaveorUpdateDimension(dimension);
            houseRepository.SaveorUpdate(house);

            IList<House> retreivedHouses = houseRepository.GetHouseByOwnerEmail(email);

            House retreivedHouse = retreivedHouses[0];
            Assert.NotNull(retreivedHouse);
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
        }

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
            House house = new House.HouseBuilder().Title(title).OwnerEmail(email)
            .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberOfBathrooms).OwnerPhoneNumber(phoneNumber)
            .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).FamiliesOnly(true)
            .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(true).WithInternetAvailable(true)
            .PropertyType(PropertyType.Apartment).MonthlyRent(rent).Latitude(coordinatesFromAddress.Item1)
            .Longitude(coordinatesFromAddress.Item2)
            .HouseNo(houseNo).Area(area).StreetNo(streetNo).Build();
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
            House house2 = new House.HouseBuilder().Title(title).OwnerEmail(email2)
            .NumberOfBedrooms(numberOfBedrooms2).NumberOfBathrooms(numberOfBathrooms2).OwnerPhoneNumber(phoneNumber2)
            .NumberOfKitchens(numberOfKitchens2).CableTvAvailable(false).FamiliesOnly(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(PropertyType.House).MonthlyRent(rent2).Latitude(coordinatesFromAddress.Item1)
            .Longitude(coordinatesFromAddress.Item2)
            .HouseNo(houseNo2).Area(area).StreetNo(streetNo2).Build();
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

        private void SaveMultipleHouses(IHouseRepository houseRepository, decimal latitude, decimal longitude)
        {
            decimal initialLatitude = latitude;
            decimal initialLongitude = longitude;
            int rentPrice = 50000;
            for (int i = 0; i < 20; i++)
            {
                initialLatitude += .005M;
                initialLongitude += .005M;
                string title = "MGM Grand" + i;
                string phoneNumber = "123456789";
                
                House house = new House.HouseBuilder().Title(title).OwnerEmail("dummy@dumdum123456" + i + ".com")
                .NumberOfBedrooms(1).NumberOfBathrooms(1).OwnerPhoneNumber(phoneNumber)
                .NumberOfKitchens(1).CableTvAvailable(true).FamiliesOnly(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(rentPrice).Latitude(initialLatitude).Longitude(initialLongitude)
                .HouseNo("123" + i).Area("Harley Street" + i).StreetNo("13" + i).Build();
                Dimension dimension = new Dimension(DimensionType.Kanal, null, 5, house);
                house.Dimension = dimension;
                houseRepository.SaveorUpdateDimension(dimension);
                houseRepository.SaveorUpdate(house);
                rentPrice++;
            }
        }

        private void VerifyRetereivedHouses(IList<House> retreivedHouses, decimal latitude, decimal longitude)
        {
            decimal initialLatitude = latitude;
            decimal initialLongitude = longitude;
            foreach (var retreivedHouse in retreivedHouses)
            {
                initialLatitude += .005M;
                initialLongitude += .005M;
                Assert.AreEqual(initialLatitude, retreivedHouse.Latitude);
                Assert.AreEqual(initialLongitude, retreivedHouse.Longitude);
            }
        }

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
            
            House house = new House.HouseBuilder().Title(title).OwnerEmail(email).OwnerPhoneNumber(phoneNumber)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).FamiliesOnly(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(price).Latitude(33.29M).Longitude(73.41M)
                .HouseNo("123").Area("Pindora").StreetNo("13").Build();
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

            Assert.AreEqual(image1, retreivedHouse.HouseImages[0]);
            Assert.AreEqual(image2, retreivedHouse.HouseImages[1]);
            Assert.AreEqual(image3, retreivedHouse.HouseImages[2]);

            IList<House> allHouses = houseRepository.GetAllHouses();
            Assert.IsNotNull(allHouses);
            Assert.AreNotEqual(0, allHouses.Count);
        }

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
            string phoneNumber = "123456789";
            string email = "special@spsp123456.com";
            int numberOfBathrooms = 1;
            int numberOfBedrooms = 1;
            int numberOfKitchens = 1;
            int rent = 50000;
            PropertyType propertyType = PropertyType.Apartment;
            House house = new House.HouseBuilder().Title(title).OwnerEmail(email)
            .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberOfBathrooms).OwnerPhoneNumber(phoneNumber)
            .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).FamiliesOnly(true)
            .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(true).WithInternetAvailable(true)
            .PropertyType(propertyType).MonthlyRent(rent).Latitude(coordinatesFromAddress.Item1)
            .Longitude(coordinatesFromAddress.Item2)
            .HouseNo(houseNo).Area(area).StreetNo(streetNo).Build();
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
            PropertyType propertyType2 = PropertyType.House;
            House house2 = new House.HouseBuilder().Title(title).OwnerEmail(email2)
            .NumberOfBedrooms(numberOfBedrooms2).NumberOfBathrooms(numberOfBathrooms2).OwnerPhoneNumber(phoneNumber2)
            .NumberOfKitchens(numberOfKitchens2).CableTvAvailable(false).FamiliesOnly(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType2).MonthlyRent(rent2).Latitude(coordinatesFromAddress.Item1)
            .Longitude(coordinatesFromAddress.Item2)
            .HouseNo(houseNo2).Area(area).StreetNo(streetNo2).Build();
            Dimension dimension2 = new Dimension(DimensionType.Marla, "20", 0, house2);
            house2.Dimension = dimension2;
            houseRepository.SaveorUpdateDimension(dimension2);
            houseRepository.SaveorUpdate(house2);

            var retreivedHouses = houseRepository.SearchHousesByCoordinatesAndPropertyType(coordinatesFromAddress.Item1, 
                coordinatesFromAddress.Item2, PropertyType.Apartment);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(1, retreivedHouses.Count);

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
            string title = "Special House";
            string phoneNumber = "123456789";
            string email = "special@spsp123456.com";
            int numberOfBathrooms = 1;
            int numberOfBedrooms = 1;
            int numberOfKitchens = 1;
            int rent = 50000;
            PropertyType propertyType = PropertyType.Apartment;
            House house = new House.HouseBuilder().Title(title).OwnerEmail(email)
            .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberOfBathrooms).OwnerPhoneNumber(phoneNumber)
            .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).FamiliesOnly(true)
            .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(true).WithInternetAvailable(true)
            .PropertyType(propertyType).MonthlyRent(rent).Latitude(coordinatesFromAddress.Item1)
            .Longitude(coordinatesFromAddress.Item2)
            .HouseNo(houseNo).Area(area).StreetNo(streetNo).Build();
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
            PropertyType propertyType2 = PropertyType.House;
            House house2 = new House.HouseBuilder().Title(title).OwnerEmail(email2)
            .NumberOfBedrooms(numberOfBedrooms2).NumberOfBathrooms(numberOfBathrooms2).OwnerPhoneNumber(phoneNumber2)
            .NumberOfKitchens(numberOfKitchens2).CableTvAvailable(false).FamiliesOnly(false)
            .GarageAvailable(false).LandlinePhoneAvailable(false).SmokingAllowed(false).WithInternetAvailable(false)
            .PropertyType(propertyType2).MonthlyRent(rent2).Latitude(coordinatesFromAddress.Item1)
            .Longitude(coordinatesFromAddress.Item2)
            .HouseNo(houseNo2).Area(area).StreetNo(streetNo2).Build();
            Dimension dimension2 = new Dimension(DimensionType.Marla, "20", 0, house2);
            house2.Dimension = dimension2;
            houseRepository.SaveorUpdateDimension(dimension2);
            houseRepository.SaveorUpdate(house2);

            var retreivedHouses = houseRepository.SearchHousesByCoordinatesAndPropertyType(coordinatesFromAddress.Item1,
                coordinatesFromAddress.Item2, PropertyType.House);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(1, retreivedHouses.Count);

            // Verification of House # 2
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
        }
    }
}