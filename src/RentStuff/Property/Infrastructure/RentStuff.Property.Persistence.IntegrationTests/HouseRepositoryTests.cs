using System.Collections.Generic;
using System.Configuration;
using NUnit.Framework;
using RentStuff.Common;
using RentStuff.Property.Domain.Model.HouseAggregate;
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
        public void RetrieveHouseByCoordinates_GetsTheHousesUsingTheirCoordinates_VerifiesThroughReturnValue()
        {
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
        private void SaveImagesToHouse_ChecksThatAfterAddingImagesHouseIsSavedAsExpected_VerifiesByRetrievingAfterSaving()
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
    }
}