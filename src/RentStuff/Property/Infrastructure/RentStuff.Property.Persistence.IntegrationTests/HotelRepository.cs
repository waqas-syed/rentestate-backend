﻿using System;
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
using RentStuff.Property.Domain.Model.PropertyAggregate;
using RentStuff.Property.Infrastructure.Persistence.Ninject.Modules;

namespace RentStuff.Property.Persistence.IntegrationTests
{
    [TestFixture]
    class HotelRepository
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

        // Create and then Update a Hotel
        [Test]
        public void CreateAndUpdateHotelInstanceSuccessTest_TestsThatAHotelInstanceIsCreatedAndUpdatedAsExpected_VerifiesByTheReturnValue()
        {
            IResidentialPropertyRepository propertyRepository = _kernel.Get<IResidentialPropertyRepository>();
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            decimal latitude = 25.43M;
            decimal longitude = 73.41M;
            string propertyType = Constants.Hotel;
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
            IList<Bed> beds = new List<Bed>()
            {
                new Bed(2, BedType.Single)
            };
            Occupants occupants = new Occupants(2, 1);

            string landlineNumber = "0510000000";
            string fax = "0510000000";
            bool elevator = false;

            Hotel hotel = new Hotel.HotelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).GarageAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Ironing(ironing)
                .CctvCameras(cctvCameras).BackupElectricity(backupElectricity)
                .Restaurant(restaurant).AirportShuttle(airportShuttle).BreakfastIncluded(breakfastIncluded)
                .SittingArea(sittingArea).CarRental(carRental).Spa(spa).Salon(salon).Bathtub(bathtub)
                .SwimmingPool(swimmingPool).Kitchen(kitchen).Beds(beds).Occupants(occupants)
                .LandlineNumber(landlineNumber).Fax(fax).Elevator(elevator)
                .Build();
            hotel.AddImage(image1);
            hotel.AddImage(image2);

            //Save the Hotel
            propertyRepository.SaveorUpdate(hotel);

            //Retreive the Hotel
            Hotel retrievedHotel = (Hotel)propertyRepository.GetPropertyById(hotel.Id);

            Assert.IsNotNull(retrievedHotel);
            Assert.AreEqual(title, retrievedHotel.Title);
            Assert.AreEqual(description, retrievedHotel.Description);
            Assert.AreEqual(email, retrievedHotel.OwnerEmail);
            Assert.AreEqual(name, retrievedHotel.OwnerName);
            Assert.AreEqual(phoneNumber, retrievedHotel.OwnerPhoneNumber);
            Assert.AreEqual(cableTv, retrievedHotel.CableTvAvailable);
            Assert.AreEqual(internet, retrievedHotel.InternetAvailable);
            Assert.AreEqual(parking, retrievedHotel.ParkingAvailable);
            Assert.AreEqual(latitude, retrievedHotel.Latitude);
            Assert.AreEqual(longitude, retrievedHotel.Longitude);
            Assert.AreEqual(propertyType, retrievedHotel.PropertyType);
            Assert.AreEqual(genderRestriction, retrievedHotel.GenderRestriction);
            Assert.AreEqual(area, retrievedHotel.Area);
            Assert.AreEqual(monthlyRent, retrievedHotel.RentPrice);
            Assert.AreEqual(isShared, retrievedHotel.IsShared);
            Assert.AreEqual(rentUnit, retrievedHotel.RentUnit);
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
            Assert.AreEqual(beds.Count, retrievedHotel.Beds.Count);
            Assert.AreEqual(beds[0].BedCount, retrievedHotel.Beds[0].BedCount);
            Assert.AreEqual(beds[0].BedType, retrievedHotel.Beds[0].BedType);

            Assert.AreEqual(occupants.Adults, retrievedHotel.Occupants.Adults);
            Assert.AreEqual(occupants.Children, retrievedHotel.Occupants.Children);
            Assert.AreEqual(occupants.TotalOccupants, retrievedHotel.Occupants.TotalOccupants);

            Assert.AreEqual(2, retrievedHotel.Images.Count);
            Assert.AreEqual(image1, retrievedHotel.Images[0]);
            Assert.AreEqual(image2, retrievedHotel.Images[1]);

            Assert.AreEqual(elevator, retrievedHotel.Elevator);

            string title2 = "Title No 2";
            string description2 = "Description of hotel 2";
            string email2 = "w@12344321-2.com";
            string name2 = "OwnerName2";
            string phoneNumber2 = "03990000002";
            decimal latitude2 = 25.43M;
            decimal longitude2 = 73.41M;
            string propertyType2 = Constants.Hotel;
            GenderRestriction genderRestriction2 = GenderRestriction.BoysOnly;
            string area2 = "Satellite Town, Rawalpindi, Pakistan";
            long monthlyRent2 = 100000;
            bool cableTv2 = true;
            bool internet2 = false;
            bool parking2 = false;
            string image3 = "Image3.jpg";
            string image4 = "Image4.png";
            string rentUnit2 = Constants.Monthly;
            bool isShared2 = false;
            bool laundry2 = false;
            bool ac2 = false;
            bool geyser2 = false;
            bool attachedBathroom2 = false;
            bool fitnessCentre2 = true;
            bool balcony2 = true;
            bool lawn2 = false;
            bool heating2 = false;
            bool ironing2 = false;
            bool cctvCameras2 = false;
            bool backupElectricity2 = false;

            bool restaurant2 = false;
            bool airportShuttle2 = true;
            bool breakfastIncluded2 = false;
            bool sittingArea2 = true;
            bool carRental2 = false;
            bool spa2 = false;
            bool salon2 = false;
            bool bathtub2 = true;
            bool swimmingPool2 = false;
            bool kitchen2 = false;
            IList<Bed> beds2 = new List<Bed>()
            {
                new Bed(1, BedType.Double)
            };
            Occupants occupants2 = new Occupants(3, 0);

            string landlineNumber2 = "0510000001";
            string fax2 = "0510000001";
            bool elevator2 = true;

            retrievedHotel.Update(title2, monthlyRent2, email2, phoneNumber2, latitude2, longitude2, area2, name2,
                description2, genderRestriction2, isShared2, rentUnit2, internet2, cableTv2, parking2,
                propertyType2, laundry2, ac2, geyser2, fitnessCentre2, attachedBathroom2, ironing2, balcony2, lawn2,
                cctvCameras2, backupElectricity2, heating2, restaurant2, airportShuttle2, breakfastIncluded2,
                sittingArea2, carRental2, spa2, salon2, bathtub2, swimmingPool2, kitchen2, beds2, occupants2,
                landlineNumber2, fax2, elevator2);
            hotel.AddImage(image3);
            hotel.AddImage(image4);

            propertyRepository.SaveorUpdate(retrievedHotel);
            Hotel retrievedHotel2 = (Hotel)propertyRepository.GetPropertyById(retrievedHotel.Id);

            Assert.IsNotNull(retrievedHotel2);
            Assert.AreEqual(title2, retrievedHotel2.Title);
            Assert.AreEqual(description2, retrievedHotel2.Description);
            Assert.AreEqual(email2, retrievedHotel2.OwnerEmail);
            Assert.AreEqual(name2, retrievedHotel2.OwnerName);
            Assert.AreEqual(phoneNumber2, retrievedHotel2.OwnerPhoneNumber);
            Assert.AreEqual(cableTv2, retrievedHotel2.CableTvAvailable);
            Assert.AreEqual(internet2, retrievedHotel2.InternetAvailable);
            Assert.AreEqual(parking2, retrievedHotel2.ParkingAvailable);
            Assert.AreEqual(latitude2, retrievedHotel2.Latitude);
            Assert.AreEqual(longitude2, retrievedHotel2.Longitude);
            Assert.AreEqual(propertyType2, retrievedHotel2.PropertyType);
            Assert.AreEqual(genderRestriction2, retrievedHotel2.GenderRestriction);
            Assert.AreEqual(area2, retrievedHotel2.Area);
            Assert.AreEqual(monthlyRent2, retrievedHotel2.RentPrice);
            Assert.AreEqual(isShared2, retrievedHotel2.IsShared);
            Assert.AreEqual(rentUnit2, retrievedHotel2.RentUnit);
            Assert.AreEqual(laundry2, retrievedHotel2.Laundry);
            Assert.AreEqual(ac2, retrievedHotel2.AC);
            Assert.AreEqual(geyser2, retrievedHotel2.Geyser);
            Assert.AreEqual(attachedBathroom2, retrievedHotel2.AttachedBathroom);
            Assert.AreEqual(fitnessCentre2, retrievedHotel2.FitnessCentre);
            Assert.AreEqual(balcony2, retrievedHotel2.Balcony);
            Assert.AreEqual(lawn2, retrievedHotel2.Lawn);
            Assert.AreEqual(ironing2, retrievedHotel2.Ironing);
            Assert.AreEqual(cctvCameras2, retrievedHotel2.CctvCameras);
            Assert.AreEqual(backupElectricity2, retrievedHotel2.BackupElectricity);
            Assert.AreEqual(heating2, retrievedHotel2.Heating);

            Assert.AreEqual(restaurant2, retrievedHotel2.Restaurant);
            Assert.AreEqual(airportShuttle2, retrievedHotel2.AirportShuttle);
            Assert.AreEqual(breakfastIncluded2, retrievedHotel2.BreakfastIncluded);
            Assert.AreEqual(sittingArea2, retrievedHotel2.SittingArea);
            Assert.AreEqual(carRental2, retrievedHotel2.CarRental);
            Assert.AreEqual(spa2, retrievedHotel2.Spa);
            Assert.AreEqual(salon2, retrievedHotel2.Salon);
            Assert.AreEqual(bathtub2, retrievedHotel2.Bathtub);
            Assert.AreEqual(swimmingPool2, retrievedHotel2.SwimmingPool);
            Assert.AreEqual(kitchen2, retrievedHotel2.Kitchen);
            Assert.AreEqual(beds2.Count, retrievedHotel2.Beds.Count);
            Assert.AreEqual(beds2[0].BedCount, retrievedHotel2.Beds[0].BedCount);
            Assert.AreEqual(beds2[0].BedType, retrievedHotel2.Beds[0].BedType);

            Assert.AreEqual(occupants2.Adults, retrievedHotel2.Occupants.Adults);
            Assert.AreEqual(occupants2.Children, retrievedHotel2.Occupants.Children);
            Assert.AreEqual(occupants2.TotalOccupants, retrievedHotel2.Occupants.TotalOccupants);

            Assert.AreEqual(4, retrievedHotel2.Images.Count);
            Assert.AreEqual(image1, retrievedHotel2.Images[0]);
            Assert.AreEqual(image2, retrievedHotel2.Images[1]);
            Assert.AreEqual(image3, retrievedHotel2.Images[2]);
            Assert.AreEqual(image4, retrievedHotel2.Images[3]);
            Assert.AreEqual(elevator2, retrievedHotel2.Elevator);
        }

        // Create and then Delete a Hotel
        [Test]
        public void DeleteHotelInstanceSuccessTest_TestsThatAHotelInstanceIsCreatedAndDeletedAsExpected_VerifiesByTheReturnValue()
        {
            IResidentialPropertyRepository propertyRepository = _kernel.Get<IResidentialPropertyRepository>();
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            decimal latitude = 25.43M;
            decimal longitude = 73.41M;
            string propertyType = Constants.Hotel;
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
            IList<Bed> beds = new List<Bed>()
            {
                new Bed(2, BedType.Single)
            };
            Occupants occupants = new Occupants(2, 1);

            string landlineNumber = "0510000000";
            string fax = "0510000000";
            bool elevator = false;

            Hotel hotel = new Hotel.HotelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).GarageAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Ironing(ironing)
                .CctvCameras(cctvCameras).BackupElectricity(backupElectricity)
                .Restaurant(restaurant).AirportShuttle(airportShuttle).BreakfastIncluded(breakfastIncluded)
                .SittingArea(sittingArea).CarRental(carRental).Spa(spa).Salon(salon).Bathtub(bathtub)
                .SwimmingPool(swimmingPool).Kitchen(kitchen).Beds(beds).Occupants(occupants)
                .LandlineNumber(landlineNumber).Fax(fax).Elevator(elevator)
                .Build();
            hotel.AddImage(image1);
            hotel.AddImage(image2);

            //Save the Hotel
            propertyRepository.SaveorUpdate(hotel);

            //Retreive the Hotel
            Hotel retrievedHotel = (Hotel)propertyRepository.GetPropertyById(hotel.Id);

            Assert.IsNotNull(retrievedHotel);
            
            // Now delete the Hotel
            propertyRepository.Delete(retrievedHotel);

            // Retrieve the Hotel again and see if it is present
            Hotel retrievedHotel2 = (Hotel)propertyRepository.GetPropertyById(hotel.Id);

            Assert.IsNull(retrievedHotel2);
        }

        // Gets all the Hotels and Guest Houses only given the Owner's email
        [Test]
        public void GetAllHotelsByOwnerEmail_ChecksIfExpectedRecordsAreReturned_VerifiesByReturnedValued()
        {
            IResidentialPropertyRepository propertyRepository = _kernel.Get<IResidentialPropertyRepository>();

            // Hotel # 1
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            decimal latitude = 25.43M;
            decimal longitude = 73.41M;
            string propertyType = Constants.Hotel;
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
            IList<Bed> beds = new List<Bed>()
            {
                new Bed(2, BedType.Single)
            };
            Occupants occupants = new Occupants(2, 1);

            string landlineNumber = "0510000000";
            string fax = "0510000000";
            bool elevator = false;

            Hotel hotel = new Hotel.HotelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).GarageAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Ironing(ironing)
                .CctvCameras(cctvCameras).BackupElectricity(backupElectricity)
                .Restaurant(restaurant).AirportShuttle(airportShuttle).BreakfastIncluded(breakfastIncluded)
                .SittingArea(sittingArea).CarRental(carRental).Spa(spa).Salon(salon).Bathtub(bathtub)
                .SwimmingPool(swimmingPool).Kitchen(kitchen).Beds(beds).Occupants(occupants)
                .LandlineNumber(landlineNumber).Fax(fax).Elevator(elevator)
                .Build();
            hotel.AddImage(image1);
            hotel.AddImage(image2);

            //Save the Hotel
            propertyRepository.SaveorUpdate(hotel);
            
            // Hotel # 2. Different Email
            string title2 = "Title No 2";
            string description2 = "Description of house 2";
            string email2 = "w@12344321-2.com";
            string name2 = "OwnerName 2";
            string phoneNumber2 = "03990000002";
            decimal latitude2 = 28.53M;
            decimal longitude2 = 74.11M;
            string propertyType2 = Constants.Hotel;
            GenderRestriction genderRestriction2 = GenderRestriction.GirlsOnly;
            string area2 = "Pindora, Rawalpindi, Pakistan";
            long monthlyRent2 = 92000;
            string rentUnit2 = Constants.Monthly;
            
            Hotel hotel2 = new Hotel.HotelBuilder().OwnerEmail(email2).OwnerPhoneNumber(phoneNumber2).Title(title2)
                .OwnerName(name2)
                .PropertyType(propertyType2).RentPrice(monthlyRent2).Latitude(latitude2).Longitude(longitude2)
                .Area(area2).GenderRestriction(genderRestriction2).Description(description2).RentUnit(rentUnit2)
                .Build();

            //Save the Hotel
            propertyRepository.SaveorUpdate(hotel2);

            // Hotel # 3(Guest House). Same Email
            string title3 = "Title No 3";
            string description3 = "Description of house 3";
            string email3 = "w@12344321.com";
            string name3 = "OwnerName 3";
            string phoneNumber3 = "03990000003";
            decimal latitude3 = 31.48M;
            decimal longitude3 = 72.40M;
            string propertyType3 = Constants.GuestHouse;
            GenderRestriction genderRestriction3 = GenderRestriction.NoRestriction;
            string area3 = "Satellite Town, Rawalpindi, Pakistan";
            long monthlyRent3 = 93000;
            string rentUnit3 = Constants.Daily;

            Hotel hotel3 = new Hotel.HotelBuilder().OwnerEmail(email3).OwnerPhoneNumber(phoneNumber3).Title(title3)
                .OwnerName(name3)
                .PropertyType(propertyType3).RentPrice(monthlyRent3).Latitude(latitude3).Longitude(longitude3)
                .Area(area3).GenderRestriction(genderRestriction3).Description(description3).RentUnit(rentUnit3)
                .Build();

            //Save the Guest Houses
            propertyRepository.SaveorUpdate(hotel3);

            IList<Hotel> retrievedHotel = propertyRepository.GetHotelsByOwnerEmail(email);

            Assert.AreEqual(2, retrievedHotel.Count);
            Assert.IsNotNull(retrievedHotel);

            // Verification of Hotel # 1
            Assert.AreEqual(title, retrievedHotel[0].Title);
            Assert.AreEqual(description, retrievedHotel[0].Description);
            Assert.AreEqual(email, retrievedHotel[0].OwnerEmail);
            Assert.AreEqual(name, retrievedHotel[0].OwnerName);
            Assert.AreEqual(phoneNumber, retrievedHotel[0].OwnerPhoneNumber);
            Assert.AreEqual(cableTv, retrievedHotel[0].CableTvAvailable);
            Assert.AreEqual(internet, retrievedHotel[0].InternetAvailable);
            Assert.AreEqual(parking, retrievedHotel[0].ParkingAvailable);
            Assert.AreEqual(latitude, retrievedHotel[0].Latitude);
            Assert.AreEqual(longitude, retrievedHotel[0].Longitude);
            Assert.AreEqual(propertyType, retrievedHotel[0].PropertyType);
            Assert.AreEqual(genderRestriction, retrievedHotel[0].GenderRestriction);
            Assert.AreEqual(area, retrievedHotel[0].Area);
            Assert.AreEqual(monthlyRent, retrievedHotel[0].RentPrice);
            Assert.AreEqual(isShared, retrievedHotel[0].IsShared);
            Assert.AreEqual(rentUnit, retrievedHotel[0].RentUnit);
            Assert.AreEqual(laundry, retrievedHotel[0].Laundry);
            Assert.AreEqual(ac, retrievedHotel[0].AC);
            Assert.AreEqual(geyser, retrievedHotel[0].Geyser);
            Assert.AreEqual(attachedBathroom, retrievedHotel[0].AttachedBathroom);
            Assert.AreEqual(fitnessCentre, retrievedHotel[0].FitnessCentre);
            Assert.AreEqual(balcony, retrievedHotel[0].Balcony);
            Assert.AreEqual(lawn, retrievedHotel[0].Lawn);
            Assert.AreEqual(ironing, retrievedHotel[0].Ironing);
            Assert.AreEqual(cctvCameras, retrievedHotel[0].CctvCameras);
            Assert.AreEqual(backupElectricity, retrievedHotel[0].BackupElectricity);
            Assert.AreEqual(heating, retrievedHotel[0].Heating);

            Assert.AreEqual(restaurant, retrievedHotel[0].Restaurant);
            Assert.AreEqual(airportShuttle, retrievedHotel[0].AirportShuttle);
            Assert.AreEqual(breakfastIncluded, retrievedHotel[0].BreakfastIncluded);
            Assert.AreEqual(sittingArea, retrievedHotel[0].SittingArea);
            Assert.AreEqual(carRental, retrievedHotel[0].CarRental);
            Assert.AreEqual(spa, retrievedHotel[0].Spa);
            Assert.AreEqual(salon, retrievedHotel[0].Salon);
            Assert.AreEqual(bathtub, retrievedHotel[0].Bathtub);
            Assert.AreEqual(swimmingPool, retrievedHotel[0].SwimmingPool);
            Assert.AreEqual(kitchen, retrievedHotel[0].Kitchen);
            Assert.AreEqual(beds.Count, retrievedHotel[0].Beds.Count);
            Assert.AreEqual(beds[0].BedCount, retrievedHotel[0].Beds[0].BedCount);
            Assert.AreEqual(beds[0].BedType, retrievedHotel[0].Beds[0].BedType);

            Assert.AreEqual(occupants.Adults, retrievedHotel[0].Occupants.Adults);
            Assert.AreEqual(occupants.Children, retrievedHotel[0].Occupants.Children);
            Assert.AreEqual(occupants.TotalOccupants, retrievedHotel[0].Occupants.TotalOccupants);

            Assert.AreEqual(2, retrievedHotel[0].Images.Count);
            Assert.AreEqual(image1, retrievedHotel[0].Images[0]);
            Assert.AreEqual(image2, retrievedHotel[0].Images[1]);

            Assert.AreEqual(elevator, retrievedHotel[0].Elevator);

            // Verification of Hotel(Guest House) # 3
            Assert.AreEqual(title3, retrievedHotel[1].Title);
            Assert.AreEqual(description3, retrievedHotel[1].Description);
            Assert.AreEqual(email3, retrievedHotel[1].OwnerEmail);
            Assert.AreEqual(name3, retrievedHotel[1].OwnerName);
            Assert.AreEqual(latitude3, retrievedHotel[1].Latitude);
            Assert.AreEqual(longitude3, retrievedHotel[1].Longitude);
            Assert.AreEqual(propertyType3, retrievedHotel[1].PropertyType);
            Assert.AreEqual(genderRestriction3, retrievedHotel[1].GenderRestriction);
            Assert.AreEqual(area3, retrievedHotel[1].Area);
            Assert.AreEqual(monthlyRent3, retrievedHotel[1].RentPrice);
            Assert.AreEqual(rentUnit3, retrievedHotel[1].RentUnit);
        }

        // Gets all the Hotels and Guest Houses
        [Test]
        public void GetAllHotelsTest_ChecksIfExpectedRecordsAreReturned_VerifiesByReturnedValued()
        {
            IResidentialPropertyRepository propertyRepository = _kernel.Get<IResidentialPropertyRepository>();

            // Hotel # 1
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            decimal latitude = 25.43M;
            decimal longitude = 73.41M;
            string propertyType = Constants.Hotel;
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
            string area = "Lahore, Pakistan";
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
            IList<Bed> beds = new List<Bed>()
            {
                new Bed(2, BedType.Single)
            };
            Occupants occupants = new Occupants(2, 1);

            string landlineNumber = "0510000000";
            string fax = "0510000000";
            bool elevator = false;

            Hotel hotel = new Hotel.HotelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).GarageAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Ironing(ironing)
                .CctvCameras(cctvCameras).BackupElectricity(backupElectricity)
                .Restaurant(restaurant).AirportShuttle(airportShuttle).BreakfastIncluded(breakfastIncluded)
                .SittingArea(sittingArea).CarRental(carRental).Spa(spa).Salon(salon).Bathtub(bathtub)
                .SwimmingPool(swimmingPool).Kitchen(kitchen).Beds(beds).Occupants(occupants)
                .LandlineNumber(landlineNumber).Fax(fax).Elevator(elevator)
                .Build();
            hotel.AddImage(image1);
            hotel.AddImage(image2);

            //Save the Hotel
            propertyRepository.SaveorUpdate(hotel);

            // Hotel # 2
            string title2 = "Title No 2";
            string description2 = "Description of house 2";
            string email2 = "w@12344321-2.com";
            string name2 = "OwnerName 2";
            string phoneNumber2 = "03990000002";
            decimal latitude2 = 28.53M;
            decimal longitude2 = 74.11M;
            string propertyType2 = Constants.Hotel;
            GenderRestriction genderRestriction2 = GenderRestriction.GirlsOnly;
            string area2 = "Rawalpindi, Pakistan";
            long monthlyRent2 = 92000;
            string rentUnit2 = Constants.Monthly;

            Hotel hotel2 = new Hotel.HotelBuilder().OwnerEmail(email2).OwnerPhoneNumber(phoneNumber2).Title(title2)
                .OwnerName(name2)
                .PropertyType(propertyType2).RentPrice(monthlyRent2).Latitude(latitude2).Longitude(longitude2)
                .Area(area2).GenderRestriction(genderRestriction2).Description(description2).RentUnit(rentUnit2)
                .Build();

            // Save the Hotel
            propertyRepository.SaveorUpdate(hotel2);

            // Hotel # 3(Guest House)
            string title3 = "Title No 3";
            string description3 = "Description of house 3";
            string email3 = "w@12344321-3.com";
            string name3 = "OwnerName 3";
            string phoneNumber3 = "03990000003";
            decimal latitude3 = 31.48M;
            decimal longitude3 = 72.40M;
            string propertyType3 = Constants.GuestHouse;
            GenderRestriction genderRestriction3 = GenderRestriction.NoRestriction;
            string area3 = "Karachi, Pakistan";
            long monthlyRent3 = 93000;
            string rentUnit3 = Constants.Daily;

            Hotel hotel3 = new Hotel.HotelBuilder().OwnerEmail(email3).OwnerPhoneNumber(phoneNumber3).Title(title3)
                .OwnerName(name3)
                .PropertyType(propertyType3).RentPrice(monthlyRent3).Latitude(latitude3).Longitude(longitude3)
                .Area(area3).GenderRestriction(genderRestriction3).Description(description3).RentUnit(rentUnit3)
                .Build();

            //Save the Guest House
            propertyRepository.SaveorUpdate(hotel3);

            IList<Hotel> retrievedHotel = propertyRepository.GetAllHotels();

            Assert.AreEqual(3, retrievedHotel.Count);
            Assert.IsNotNull(retrievedHotel);

            // Verification of Hotel # 1
            Assert.AreEqual(title, retrievedHotel[0].Title);
            Assert.AreEqual(description, retrievedHotel[0].Description);
            Assert.AreEqual(email, retrievedHotel[0].OwnerEmail);
            Assert.AreEqual(name, retrievedHotel[0].OwnerName);
            Assert.AreEqual(phoneNumber, retrievedHotel[0].OwnerPhoneNumber);
            Assert.AreEqual(cableTv, retrievedHotel[0].CableTvAvailable);
            Assert.AreEqual(internet, retrievedHotel[0].InternetAvailable);
            Assert.AreEqual(parking, retrievedHotel[0].ParkingAvailable);
            Assert.AreEqual(latitude, retrievedHotel[0].Latitude);
            Assert.AreEqual(longitude, retrievedHotel[0].Longitude);
            Assert.AreEqual(propertyType, retrievedHotel[0].PropertyType);
            Assert.AreEqual(genderRestriction, retrievedHotel[0].GenderRestriction);
            Assert.AreEqual(area, retrievedHotel[0].Area);
            Assert.AreEqual(monthlyRent, retrievedHotel[0].RentPrice);
            Assert.AreEqual(isShared, retrievedHotel[0].IsShared);
            Assert.AreEqual(rentUnit, retrievedHotel[0].RentUnit);
            Assert.AreEqual(laundry, retrievedHotel[0].Laundry);
            Assert.AreEqual(ac, retrievedHotel[0].AC);
            Assert.AreEqual(geyser, retrievedHotel[0].Geyser);
            Assert.AreEqual(attachedBathroom, retrievedHotel[0].AttachedBathroom);
            Assert.AreEqual(fitnessCentre, retrievedHotel[0].FitnessCentre);
            Assert.AreEqual(balcony, retrievedHotel[0].Balcony);
            Assert.AreEqual(lawn, retrievedHotel[0].Lawn);
            Assert.AreEqual(ironing, retrievedHotel[0].Ironing);
            Assert.AreEqual(cctvCameras, retrievedHotel[0].CctvCameras);
            Assert.AreEqual(backupElectricity, retrievedHotel[0].BackupElectricity);
            Assert.AreEqual(heating, retrievedHotel[0].Heating);

            Assert.AreEqual(restaurant, retrievedHotel[0].Restaurant);
            Assert.AreEqual(airportShuttle, retrievedHotel[0].AirportShuttle);
            Assert.AreEqual(breakfastIncluded, retrievedHotel[0].BreakfastIncluded);
            Assert.AreEqual(sittingArea, retrievedHotel[0].SittingArea);
            Assert.AreEqual(carRental, retrievedHotel[0].CarRental);
            Assert.AreEqual(spa, retrievedHotel[0].Spa);
            Assert.AreEqual(salon, retrievedHotel[0].Salon);
            Assert.AreEqual(bathtub, retrievedHotel[0].Bathtub);
            Assert.AreEqual(swimmingPool, retrievedHotel[0].SwimmingPool);
            Assert.AreEqual(kitchen, retrievedHotel[0].Kitchen);
            Assert.AreEqual(beds.Count, retrievedHotel[0].Beds.Count);
            Assert.AreEqual(beds[0].BedCount, retrievedHotel[0].Beds[0].BedCount);
            Assert.AreEqual(beds[0].BedType, retrievedHotel[0].Beds[0].BedType);

            Assert.AreEqual(occupants.Adults, retrievedHotel[0].Occupants.Adults);
            Assert.AreEqual(occupants.Children, retrievedHotel[0].Occupants.Children);
            Assert.AreEqual(occupants.TotalOccupants, retrievedHotel[0].Occupants.TotalOccupants);

            Assert.AreEqual(2, retrievedHotel[0].Images.Count);
            Assert.AreEqual(image1, retrievedHotel[0].Images[0]);
            Assert.AreEqual(image2, retrievedHotel[0].Images[1]);

            Assert.AreEqual(elevator, retrievedHotel[0].Elevator);

            // Verification of Hotel # 2
            Assert.AreEqual(title2, retrievedHotel[1].Title);
            Assert.AreEqual(description2, retrievedHotel[1].Description);
            Assert.AreEqual(email2, retrievedHotel[1].OwnerEmail);
            Assert.AreEqual(name2, retrievedHotel[1].OwnerName);
            Assert.AreEqual(latitude2, retrievedHotel[1].Latitude);
            Assert.AreEqual(longitude2, retrievedHotel[1].Longitude);
            Assert.AreEqual(propertyType2, retrievedHotel[1].PropertyType);
            Assert.AreEqual(genderRestriction2, retrievedHotel[1].GenderRestriction);
            Assert.AreEqual(area2, retrievedHotel[1].Area);
            Assert.AreEqual(monthlyRent2, retrievedHotel[1].RentPrice);
            Assert.AreEqual(rentUnit2, retrievedHotel[1].RentUnit);

            // Verification of Hotel(Guest House) # 3
            Assert.AreEqual(title3, retrievedHotel[2].Title);
            Assert.AreEqual(description3, retrievedHotel[2].Description);
            Assert.AreEqual(email3, retrievedHotel[2].OwnerEmail);
            Assert.AreEqual(name3, retrievedHotel[2].OwnerName);
            Assert.AreEqual(latitude3, retrievedHotel[2].Latitude);
            Assert.AreEqual(longitude3, retrievedHotel[2].Longitude);
            Assert.AreEqual(propertyType3, retrievedHotel[2].PropertyType);
            Assert.AreEqual(genderRestriction3, retrievedHotel[2].GenderRestriction);
            Assert.AreEqual(area3, retrievedHotel[2].Area);
            Assert.AreEqual(monthlyRent3, retrievedHotel[2].RentPrice);
            Assert.AreEqual(rentUnit3, retrievedHotel[2].RentUnit);
        }

        // Search by Coordinates. This test verifies all the attributes have the corect values rather than having
        // the search functionality tested. For search functionality, check SearchResidentialProperty Tests
        [Test]
        public void SearchHotelsByCoordinatesTest_ChecksIfExpectedRecordsAreReturnedByProvidingTheCoordinates_VerifiesByReturnedValued()
        {
            IResidentialPropertyRepository propertyRepository = _kernel.Get<IResidentialPropertyRepository>();
            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService =
                _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();

            // Hotel # 1
            string area = "Lahore, Punjab, Pakistan";
            var coordinates = geocodingService.GetCoordinatesFromAddress(area);
            decimal latitude = coordinates.Item1;
            decimal longitude = coordinates.Item2;
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            string propertyType = Constants.Hotel;
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
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
            IList<Bed> beds = new List<Bed>()
            {
                new Bed(2, BedType.Single)
            };
            Occupants occupants = new Occupants(2, 1);

            string landlineNumber = "0510000000";
            string fax = "0510000000";
            bool elevator = false;

            Hotel hotel = new Hotel.HotelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).GarageAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Ironing(ironing)
                .CctvCameras(cctvCameras).BackupElectricity(backupElectricity)
                .Restaurant(restaurant).AirportShuttle(airportShuttle).BreakfastIncluded(breakfastIncluded)
                .SittingArea(sittingArea).CarRental(carRental).Spa(spa).Salon(salon).Bathtub(bathtub)
                .SwimmingPool(swimmingPool).Kitchen(kitchen).Beds(beds).Occupants(occupants)
                .LandlineNumber(landlineNumber).Fax(fax).Elevator(elevator)
                .Build();
            hotel.AddImage(image1);
            hotel.AddImage(image2);

            //Save the Hotel
            propertyRepository.SaveorUpdate(hotel);

            // Hotel # 2
            string area2 = "Rawalpindi, Punjab, Pakistan";
            var coordinates2 = geocodingService.GetCoordinatesFromAddress(area2);
            decimal latitude2 = coordinates2.Item1;
            decimal longitude2 = coordinates2.Item2;
            string title2 = "Title No 2";
            string description2 = "Description of house 2";
            string email2 = "w@12344321-2.com";
            string name2 = "OwnerName 2";
            string phoneNumber2 = "03990000002";
            string propertyType2 = Constants.Hotel;
            GenderRestriction genderRestriction2 = GenderRestriction.GirlsOnly;
            long monthlyRent2 = 92000;
            string rentUnit2 = Constants.Monthly;

            Hotel hotel2 = new Hotel.HotelBuilder().OwnerEmail(email2).OwnerPhoneNumber(phoneNumber2).Title(title2)
                .OwnerName(name2)
                .PropertyType(propertyType2).RentPrice(monthlyRent2).Latitude(latitude2).Longitude(longitude2)
                .Area(area2).GenderRestriction(genderRestriction2).Description(description2).RentUnit(rentUnit2)
                .Build();

            // Save the Hotel
            propertyRepository.SaveorUpdate(hotel2);

            // Hotel # 3(Guest House)
            string area3 = "Karachi, Sindh, Pakistan";
            var coordinates3 = geocodingService.GetCoordinatesFromAddress(area3);
            decimal latitude3 = coordinates3.Item1;
            decimal longitude3 = coordinates3.Item2;
            string title3 = "Title No 3";
            string description3 = "Description of house 3";
            string email3 = "w@12344321-3.com";
            string name3 = "OwnerName 3";
            string phoneNumber3 = "03990000003";
            string propertyType3 = Constants.GuestHouse;
            GenderRestriction genderRestriction3 = GenderRestriction.NoRestriction;
            long monthlyRent3 = 93000;
            string rentUnit3 = Constants.Daily;

            Hotel hotel3 = new Hotel.HotelBuilder().OwnerEmail(email3).OwnerPhoneNumber(phoneNumber3).Title(title3)
                .OwnerName(name3)
                .PropertyType(propertyType3).RentPrice(monthlyRent3).Latitude(latitude3).Longitude(longitude3)
                .Area(area3).GenderRestriction(genderRestriction3).Description(description3).RentUnit(rentUnit3)
                .Build();

            //Save the Guest House
            propertyRepository.SaveorUpdate(hotel3);

            IList<Hotel> retrievedHotel = propertyRepository.GetAllHotels();

            Assert.AreEqual(3, retrievedHotel.Count);
            Assert.IsNotNull(retrievedHotel);

            // Verification of Hotel # 1
            Assert.AreEqual(title, retrievedHotel[0].Title);
            Assert.AreEqual(description, retrievedHotel[0].Description);
            Assert.AreEqual(email, retrievedHotel[0].OwnerEmail);
            Assert.AreEqual(name, retrievedHotel[0].OwnerName);
            Assert.AreEqual(phoneNumber, retrievedHotel[0].OwnerPhoneNumber);
            Assert.AreEqual(cableTv, retrievedHotel[0].CableTvAvailable);
            Assert.AreEqual(internet, retrievedHotel[0].InternetAvailable);
            Assert.AreEqual(parking, retrievedHotel[0].ParkingAvailable);
            Assert.AreEqual(latitude, retrievedHotel[0].Latitude);
            Assert.AreEqual(longitude, retrievedHotel[0].Longitude);
            Assert.AreEqual(propertyType, retrievedHotel[0].PropertyType);
            Assert.AreEqual(genderRestriction, retrievedHotel[0].GenderRestriction);
            Assert.AreEqual(area, retrievedHotel[0].Area);
            Assert.AreEqual(monthlyRent, retrievedHotel[0].RentPrice);
            Assert.AreEqual(isShared, retrievedHotel[0].IsShared);
            Assert.AreEqual(rentUnit, retrievedHotel[0].RentUnit);
            Assert.AreEqual(laundry, retrievedHotel[0].Laundry);
            Assert.AreEqual(ac, retrievedHotel[0].AC);
            Assert.AreEqual(geyser, retrievedHotel[0].Geyser);
            Assert.AreEqual(attachedBathroom, retrievedHotel[0].AttachedBathroom);
            Assert.AreEqual(fitnessCentre, retrievedHotel[0].FitnessCentre);
            Assert.AreEqual(balcony, retrievedHotel[0].Balcony);
            Assert.AreEqual(lawn, retrievedHotel[0].Lawn);
            Assert.AreEqual(ironing, retrievedHotel[0].Ironing);
            Assert.AreEqual(cctvCameras, retrievedHotel[0].CctvCameras);
            Assert.AreEqual(backupElectricity, retrievedHotel[0].BackupElectricity);
            Assert.AreEqual(heating, retrievedHotel[0].Heating);

            Assert.AreEqual(restaurant, retrievedHotel[0].Restaurant);
            Assert.AreEqual(airportShuttle, retrievedHotel[0].AirportShuttle);
            Assert.AreEqual(breakfastIncluded, retrievedHotel[0].BreakfastIncluded);
            Assert.AreEqual(sittingArea, retrievedHotel[0].SittingArea);
            Assert.AreEqual(carRental, retrievedHotel[0].CarRental);
            Assert.AreEqual(spa, retrievedHotel[0].Spa);
            Assert.AreEqual(salon, retrievedHotel[0].Salon);
            Assert.AreEqual(bathtub, retrievedHotel[0].Bathtub);
            Assert.AreEqual(swimmingPool, retrievedHotel[0].SwimmingPool);
            Assert.AreEqual(kitchen, retrievedHotel[0].Kitchen);
            Assert.AreEqual(beds.Count, retrievedHotel[0].Beds.Count);
            Assert.AreEqual(beds[0].BedCount, retrievedHotel[0].Beds[0].BedCount);
            Assert.AreEqual(beds[0].BedType, retrievedHotel[0].Beds[0].BedType);

            Assert.AreEqual(occupants.Adults, retrievedHotel[0].Occupants.Adults);
            Assert.AreEqual(occupants.Children, retrievedHotel[0].Occupants.Children);
            Assert.AreEqual(occupants.TotalOccupants, retrievedHotel[0].Occupants.TotalOccupants);

            Assert.AreEqual(2, retrievedHotel[0].Images.Count);
            Assert.AreEqual(image1, retrievedHotel[0].Images[0]);
            Assert.AreEqual(image2, retrievedHotel[0].Images[1]);

            Assert.AreEqual(elevator, retrievedHotel[0].Elevator);
        }
    }
}
