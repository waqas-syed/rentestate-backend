using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using NUnit.Framework;
using RentStuff.Common.Ninject.Modules;
using RentStuff.Common.Utilities;
using RentStuff.Property.Domain.Model.HostelAggregate;
using RentStuff.Property.Domain.Model.HotelAggregate;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Domain.Model.PropertyAggregate;
using RentStuff.Property.Infrastructure.Persistence.Ninject.Modules;

namespace RentStuff.Property.Persistence.IntegrationTests
{
    [TestFixture]
    class SearchPropertyTests
    {
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

        // Different propertyies with different types, and we will search by Location and property type
        [Test]
        [Category("Integration")]
        public void SearchMultipleHousesByCoordinatesAndPropertyType2_ChecksIfNearbyHousesAreReturned_VerifiesByReturnValue()
        {
            Console.WriteLine("Start");
            DateTime startTime = DateTime.Now;

            // Save 3 houses in locations nearby and 2 houses that are in other places. 
            // Search should get the 3 houses located near theserched location

            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();
            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService =
                _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();

            // Saving House # 1: House. Should be in the search results
            string area = "Pindora, Rawalpindi, Pakistan";
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(area);
            string title = "Title # 1";
            string phoneNumber = "03990000001";
            string email = "special@spsp123456-1.com";
            int rent = 100;
            string ownerName = "Owner Name 1";
            string propertyType = Constants.House;
            string rentUnit = Constants.Hourly;
            House house = new House.HouseBuilder().Title(title).OwnerEmail(email)
                .OwnerPhoneNumber(phoneNumber)
                .PropertyType(propertyType).RentPrice(rent).Latitude(coordinatesFromAddress.Item1)
                .Longitude(coordinatesFromAddress.Item2)
                .Area(area).OwnerName(ownerName).RentUnit(rentUnit)
                .Build();
            houseRepository.SaveorUpdate(house);

            // Saving Property # 2: Hotel. Near the search location, should be in the search results
            string area2 = "Satellite Town, Rawalpindi, Pakistan";
            var coordinatesFromAddress2 = geocodingService.GetCoordinatesFromAddress(area2);
            string title2 = "Title No 2";
            string email2 = "w@12344321-2.com";
            string name2 = "OwnerName 2";
            string phoneNumber2 = "03990000002";
            decimal latitude2 =  coordinatesFromAddress2.Item1;
            decimal longitude2 = coordinatesFromAddress2.Item2;
            string propertyType2 = Constants.Hotel;
            string rentUnit2 = Constants.Weekly;
            long monthlyRent2 = 92000;
            GenderRestriction genderRestriction2 = GenderRestriction.GirlsOnly;

            Hotel hotel = new Hotel.HotelBuilder().OwnerEmail(email2).OwnerPhoneNumber(phoneNumber2).Title(title2)
                .OwnerName(name2)
                .PropertyType(propertyType2).RentPrice(monthlyRent2).Latitude(latitude2).Longitude(longitude2)
                .Area(area2).GenderRestriction(genderRestriction2).RentUnit(rentUnit2)
                .Build();
            houseRepository.SaveorUpdate(hotel);

            // Saving Property # 3: Apartment. Should be in the search results
            string area3 = "Bahria Town, Rawalpindi, Punjab, Pakistan";
            var coordinatesFromAddress3 = geocodingService.GetCoordinatesFromAddress(area3);
            string title3 = "Title # 3";
            string email3 = "special2@spsp123456-3.com";
            string streetNo3 = "3";
            string phoneNumber3 = "03990000003";
            int rent3 = 93000;
            string ownerName3 = "Owner Name 3";
            string propertyType3 = Constants.Apartment;
            string rentUnit3 = Constants.Monthly;

            House apartment = new House.HouseBuilder().Title(title3).OwnerEmail(email3)
                .OwnerPhoneNumber(phoneNumber3)
                .PropertyType(propertyType3).RentPrice(rent3).Latitude(coordinatesFromAddress3.Item1)
                .Longitude(coordinatesFromAddress3.Item2).Area(area3).StreetNo(streetNo3).OwnerName(ownerName3).RentUnit(rentUnit3).Build();
            houseRepository.SaveorUpdate(apartment);

            // Saving House # 4: Guest House. Should be in the search results
            string area4 = "Lohi Bhair, Islamabad Capital Territory, Pakistan";
            var coordinatesFromAddress4 = geocodingService.GetCoordinatesFromAddress(area4);
            string title4 = "Title No 4";
            string email4 = "w@12344321-4.com";
            string name4 = "OwnerName 4";
            string phoneNumber4 = "03990000004";
            decimal latitude4 = coordinatesFromAddress4.Item1;
            decimal longitude4 = coordinatesFromAddress4.Item2;
            string propertyType4 = Constants.GuestHouse;
            string rentUnit4 = Constants.Monthly;
            long monthlyRent4 = 94000;
            GenderRestriction genderRestriction4 = GenderRestriction.NoRestriction;

            Hotel guestHouse = new Hotel.HotelBuilder().OwnerEmail(email4).OwnerPhoneNumber(phoneNumber4)
                .Title(title4)
                .OwnerName(name4)
                .PropertyType(propertyType4).RentPrice(monthlyRent4).Latitude(latitude4).Longitude(longitude4)
                .Area(area4).GenderRestriction(genderRestriction4).RentUnit(rentUnit4)
                .Build();
            houseRepository.SaveorUpdate(guestHouse);

            // Saving House # 5: Hostel. Should be in the search results
            string area5 = "Gulberg Greens, Gulberg, Islamabad Capital Territory, Pakistan";
            var coordinatesFromAddress5 = geocodingService.GetCoordinatesFromAddress(area5);
            string title5 = "Title No 5";
            string email5 = "w@12344321-5.com";
            string name5 = "OwnerName 5";
            string phoneNumber5 = "03990000005";
            decimal latitude5 = coordinatesFromAddress5.Item1;
            decimal longitude5 = coordinatesFromAddress5.Item2;
            string propertyType5 = Constants.Hostel;
            string rentUnit5 = Constants.Daily;
            long monthlyRent5 = 95000;
            GenderRestriction genderRestriction5 = GenderRestriction.NoRestriction;

            Hostel hostel = new Hostel.HostelBuilder().OwnerEmail(email5).OwnerPhoneNumber(phoneNumber5)
                .Title(title5)
                .OwnerName(name5)
                .PropertyType(propertyType5).RentPrice(monthlyRent5).Latitude(latitude5).Longitude(longitude5)
                .Area(area5).GenderRestriction(genderRestriction5).RentUnit(rentUnit5)
                .Build();
            houseRepository.SaveorUpdate(hostel);

            // Saving Property # 6: House. Outside Bounds, should NOT be in search results
            string area6 = "Nara, Rawalpindi, Pakistan";
            var coordinatesFromAddress6 = geocodingService.GetCoordinatesFromAddress(area6);
            string title6 = "Title # 6";
            string phoneNumber6 = "03990000006";
            string email6 = "special@spsp123456-6.com";
            int rent6 = 96000;
            string ownerName6 = "Owner Name 6";
            string propertyType6 = Constants.House;
            string rentUnit6 = Constants.Daily;
            decimal latitude6 = coordinatesFromAddress6.Item1;
            decimal longitude6 = coordinatesFromAddress6.Item2;
            House house2 = new House.HouseBuilder().Title(title6).OwnerEmail(email6)
                .OwnerPhoneNumber(phoneNumber6)
                .PropertyType(propertyType6).RentPrice(rent6).Latitude(latitude6)
                .Longitude(longitude6)
                .Area(area6).OwnerName(ownerName6).RentUnit(rentUnit6)
                .Build();
            houseRepository.SaveorUpdate(house2);

            // Saving Property # 7: Hotel. Outside bounds, should NOT be in the search results
            string area7 = "Choha Khalsa, Punjab, Pakistan";
            var coordinatesFromAddress7 = geocodingService.GetCoordinatesFromAddress(area7);
            string title7 = "Title No 7";
            string email7 = "w@12344321-7.com";
            string name7 = "OwnerName 7";
            string phoneNumber7 = "03990000007";
            decimal latitude7 = coordinatesFromAddress7.Item1;
            decimal longitude7 = coordinatesFromAddress7.Item2;
            string propertyType7 = Constants.Hotel;
            string rentUnit7 = Constants.Weekly;
            long monthlyRent7 = 97000;
            GenderRestriction genderRestriction7 = GenderRestriction.FamiliesOnly;

            Hotel hotel2 = new Hotel.HotelBuilder().OwnerEmail(email7).OwnerPhoneNumber(phoneNumber7).Title(title7)
                .OwnerName(name7)
                .PropertyType(propertyType7).RentPrice(monthlyRent7).Latitude(latitude7).Longitude(longitude7)
                .Area(area7).GenderRestriction(genderRestriction7).RentUnit(rentUnit7)
                .Build();
            houseRepository.SaveorUpdate(hotel2);

            // Saving Property # 8: Guest House. Outside bounds, should NOT be in the search results
            string area8 = "Patriata, Punjab, Pakistan";
            var coordinatesFromAddress8 = geocodingService.GetCoordinatesFromAddress(area8);
            string title8 = "Title No 8";
            string email8 = "w@12344321-8.com";
            string name8 = "OwnerName 8";
            string phoneNumber8 = "03990000008";
            decimal latitude8 = coordinatesFromAddress8.Item1;
            decimal longitude8 = coordinatesFromAddress8.Item2;
            string propertyType8 = Constants.GuestHouse;
            string rentUnit8 = Constants.Weekly;
            long monthlyRent8 = 98000;
            GenderRestriction genderRestriction8 = GenderRestriction.FamiliesOnly;

            Hotel guestHouse2 = new Hotel.HotelBuilder().OwnerEmail(email8).OwnerPhoneNumber(phoneNumber8).Title(title8)
                .OwnerName(name8)
                .PropertyType(propertyType8).RentPrice(monthlyRent8).Latitude(latitude8).Longitude(longitude8)
                .Area(area8).GenderRestriction(genderRestriction8).RentUnit(rentUnit8)
                .Build();
            houseRepository.SaveorUpdate(guestHouse2);

            // Saving Property # 9: Aparment. Outside bounds, should NOT be in the search results
            string area9 = "Murree, Punjab, Pakistan";
            var coordinatesFromAddress9 = geocodingService.GetCoordinatesFromAddress(area9);
            string title9 = "Title # 9";
            string email9 = "special2@spsp123456-9.com";
            string phoneNumber9 = "03990000009";
            int rent9 = 99000;
            string ownerName9 = "Owner Name 9";
            string propertyType9 = Constants.Apartment;
            string rentUnit9 = Constants.Monthly;

            House apartment2 = new House.HouseBuilder().Title(title9).OwnerEmail(email9)
                .OwnerPhoneNumber(phoneNumber9)
                .PropertyType(propertyType9).RentPrice(rent9).Latitude(coordinatesFromAddress9.Item1)
                .Longitude(coordinatesFromAddress9.Item2).Area(area9).OwnerName(ownerName9).RentUnit(rentUnit9)
                .Build();
            houseRepository.SaveorUpdate(apartment2);

            // Saving Property # 10: Hostel. Should be in the search results, again
            string area10 = "6th Road, Rawalpindi, Pakistan";
            var coordinatesFromAddress10 = geocodingService.GetCoordinatesFromAddress(area10);
            string title10 = "Title No 10";
            string email10 = "w@12344321-5.com";
            string name10 = "OwnerName 5";
            string phoneNumber10 = "03990000010";
            decimal latitude10 = coordinatesFromAddress10.Item1;
            decimal longitude10 = coordinatesFromAddress10.Item2;
            string propertyType10 = Constants.Hostel;
            string rentUnit10 = Constants.Daily;
            long monthlyRent10 = 100000;
            GenderRestriction genderRestriction10 = GenderRestriction.GirlsOnly;

            Hostel hostel2 = new Hostel.HostelBuilder().OwnerEmail(email10).OwnerPhoneNumber(phoneNumber10)
                .Title(title10)
                .OwnerName(name10)
                .PropertyType(propertyType10).RentPrice(monthlyRent5).Latitude(latitude10).Longitude(longitude10)
                .Area(area10).GenderRestriction(genderRestriction10).RentUnit(rentUnit10)
                .Build();
            houseRepository.SaveorUpdate(hostel2);

            string searchLocation = "Pindora, Rawalpindi, Pakistan";
            var searchCoordinates = geocodingService.GetCoordinatesFromAddress(searchLocation);

            // Verification of House & Apartment
            var retreivedHouses = houseRepository.SearchHousesByCoordinates(searchCoordinates.Item1,
                searchCoordinates.Item2);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(2, retreivedHouses.Count);
            // House
            Assert.AreEqual(house.Id, retreivedHouses[0].Id);
            Assert.AreEqual(title, retreivedHouses[0].Title);
            Assert.AreEqual(Constants.House, retreivedHouses[0].PropertyType);
            // Apartment
            Assert.AreEqual(apartment.Id, retreivedHouses[1].Id);
            Assert.AreEqual(apartment.Title, retreivedHouses[1].Title);
            Assert.AreEqual(Constants.Apartment, retreivedHouses[1].PropertyType);

            // Verification of 2 Hostels
            var retreivedHostels = houseRepository.SearchHostelByLocation(searchCoordinates.Item1,
                searchCoordinates.Item2);
            Assert.NotNull(retreivedHostels);
            Assert.AreEqual(2, retreivedHostels.Count);
            // Hostel # 1
            Assert.AreEqual(hostel2.Id, retreivedHostels[0].Id);
            Assert.AreEqual(hostel2.Title, retreivedHostels[0].Title);
            Assert.AreEqual(Constants.Hostel, retreivedHostels[0].PropertyType);
            // Hostel # 2
            Assert.AreEqual(hostel.Id, retreivedHostels[1].Id);
            Assert.AreEqual(hostel.Title, retreivedHostels[1].Title);
            Assert.AreEqual(Constants.Hostel, retreivedHostels[1].PropertyType);

            // Verification of Hotel & Guest House
            var retreivedHotels = houseRepository.SearchHotelByCoordinates(searchCoordinates.Item1,
                searchCoordinates.Item2);
            Assert.NotNull(retreivedHotels);
            Assert.AreEqual(2, retreivedHotels.Count);
            // Hotel
            Assert.AreEqual(hotel.Id, retreivedHotels[0].Id);
            Assert.AreEqual(hotel.Title, retreivedHotels[0].Title);
            Assert.AreEqual(Constants.Hotel, retreivedHotels[0].PropertyType);
            // Guest House
            Assert.AreEqual(guestHouse.Id, retreivedHotels[1].Id);
            Assert.AreEqual(guestHouse.Title, retreivedHotels[1].Title);
            Assert.AreEqual(Constants.GuestHouse, retreivedHotels[1].PropertyType);
        }
    }
}
