using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Infrastructure.Persistence.Repositories;
using Spring.Context.Support;

namespace RentStuff.Property.Persistence.IntegrationTests
{
    [TestFixture]
    public class HouseRepositoryTests
    {
        [Test]
        public void SaveHouseTest_TestsThatHouseUInstancesAreSavedToTheDatabaseAsExpected_VerifiesThroughDatabaseQuery()
        {
            IHouseRepository houseRepository = (IHouseRepository) ContextRegistry.GetContext()["HouseRepository"];
            ILocationRepository locationRepository = (ILocationRepository)ContextRegistry.GetContext()["LocationRepository"];
            string email = "w@12344321.com";
            
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long price = 90000;

            House house = new House.HouseBuilder().OwnerEmail(email)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).FamiliesOnly(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(price).Latitude(33.29M).Latitude(73.41M)
                .HouseNo("123").Area("Harley Street").StreetNo("13").Build();
            Location location = new Location(22M, 100M, "House # 818", "13", "Islamabad, Pakistan", house);
            locationRepository.SaveOrUpdate(location);
            //house.Location = location;
            houseRepository.SaveorUpdate(house);
            
            //House retreivedHouse = houseRepository.GetHouseByOwnerEmail(email);

            House retreivedHouse = houseRepository.GetHouseById(house.Id);

            Assert.NotNull(retreivedHouse);
            /*Assert.AreEqual(house.Location.StreetAddress, retreivedHouse.Location.StreetAddress);
            Assert.AreEqual(house.Location.City, retreivedHouse.Location.City);
            Assert.AreEqual(house.Location.Country, retreivedHouse.Location.Country);*/
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouse.NumberOfBedrooms);
            Assert.AreEqual(house.NumberOfKitchens, retreivedHouse.NumberOfKitchens);
            Assert.AreEqual(house.FamiliesOnly, retreivedHouse.FamiliesOnly);
            Assert.AreEqual(house.GarageAvailable, retreivedHouse.LandlinePhoneAvailable);
            Assert.AreEqual(house.SmokingAllowed, retreivedHouse.SmokingAllowed);
            Assert.AreEqual(house.InternetAvailable, retreivedHouse.InternetAvailable);
            Assert.AreEqual(house.PropertyType, retreivedHouse.PropertyType);

            RemoveHouseObject(houseRepository, house);
        }

        [Test]
        public void SaveHouseAndRetreiveByEmailTest_TestsThatHouseUInstancesAreSavedToTheDatabaseAsExpected_VerifiesThroughDatabaseQuery()
        {
            IHouseRepository houseRepository = (IHouseRepository)ContextRegistry.GetContext()["HouseRepository"];
            ILocationRepository locationRepository = (ILocationRepository)ContextRegistry.GetContext()["LocationRepository"];
            string email = "w@12344321.com";
            
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long price = 90000;

            House house = new House.HouseBuilder().OwnerEmail(email)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).FamiliesOnly(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(price).Build();

            Location location = new Location(22, 100, "House # 818", "141", "Islamabad, Pakistan", house);
            locationRepository.SaveOrUpdate(location);
            //house.Location = location;
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

            RemoveHouseObject(houseRepository, house);
        }

        [Test]
        public void RetrieveHouseByCoordinates_GetsTheHousesUsingTheirCoordinates_VerifiesThroughReturnValue()
        {
            decimal initialLatitude = 33.29M;
            decimal initialLongitude = 73.41M;
            IHouseRepository houseRepository = (IHouseRepository)ContextRegistry.GetContext()["HouseRepository"];
            SaveMultipleHouses(houseRepository, initialLatitude, initialLongitude);
            IList searchHousesByCoordinates = houseRepository.SearchHousesByCoordinates(33.29M, 73.41M);
            Assert.NotNull(searchHousesByCoordinates);
        }

        private void SaveMultipleHouses(IHouseRepository houseRepository, decimal latitude, decimal longitude)
        {
            decimal initialLatitude = latitude;
            decimal initialLongitude = longitude;
            for (int i = 0; i < 20; i++)
            {
                initialLatitude += .005M;
                initialLongitude += .005M;
                House house = new House.HouseBuilder().OwnerEmail("dummy@dumdum123.com" + i)
                .NumberOfBedrooms(1).NumberOfBathrooms(1)
                .NumberOfKitchens(1).CableTvAvailable(true).FamiliesOnly(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(50000).Latitude(initialLatitude).Longitude(initialLongitude)
                .HouseNo("123").Area("Harley Street").StreetNo("13").Build();
                houseRepository.SaveorUpdate(house);
            }
        }

        private void RemoveHouseObject(IHouseRepository houseRepository, House house)
        {
            houseRepository.Delete(house);
            Thread.Sleep(2000);
        }
    }
}