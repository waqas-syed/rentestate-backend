﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RentStuff.Common.Utilities;
using RentStuff.Property.Domain.Model.HostelAggregate;
using RentStuff.Property.Domain.Model.HotelAggregate;
using RentStuff.Property.Domain.Model.PropertyAggregate;

namespace RentStuff.Property.Domain.Model.Tests
{
    [TestFixture]
    class HotelTests
    {
        // Create a Hotel Instance successfully with all the attributes
        [Test]
        public void CreateHotelInstanceSuccessTest_TestsThatAHotelInstanceIsCreatedAsExpected_VerifiesByTheReturnValue()
        {
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
            bool elevator = false;
            int numberOfSingleBeds = 2;
            int numberOfDoubleBeds = 1;
            
            Occupants occupants = new Occupants(2, 1);

            string landlineNumber = "0510000000";
            string fax = "0510000000";

            Hotel hotel = new Hotel.HotelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).ParkingAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Ironing(ironing)
                .CctvCameras(cctvCameras).BackupElectricity(backupElectricity)
                .Restaurant(restaurant).AirportShuttle(airportShuttle).BreakfastIncluded(breakfastIncluded)
                .SittingArea(sittingArea).CarRental(carRental).Spa(spa).Salon(salon).Bathtub(bathtub)
                .SwimmingPool(swimmingPool).Kitchen(kitchen).Occupants(occupants).Elevator(elevator)
                .LandlineNumber(landlineNumber).Fax(fax).NumberOfSingleBeds(numberOfSingleBeds)
                .NumberOfDoubleBeds(numberOfDoubleBeds)
                .Build();
            hotel.AddImage(image1);
            hotel.AddImage(image2);
            

            Assert.IsNotNull(hotel);
            Assert.AreEqual(title, hotel.Title);
            Assert.AreEqual(description, hotel.Description);
            Assert.AreEqual(email, hotel.OwnerEmail);
            Assert.AreEqual(name, hotel.OwnerName);
            Assert.AreEqual(phoneNumber, hotel.OwnerPhoneNumber);
            Assert.AreEqual(cableTv, hotel.CableTvAvailable);
            Assert.AreEqual(internet, hotel.InternetAvailable);
            Assert.AreEqual(parking, hotel.ParkingAvailable);
            Assert.AreEqual(latitude, hotel.Latitude);
            Assert.AreEqual(longitude, hotel.Longitude);
            Assert.AreEqual(propertyType, hotel.PropertyType);
            Assert.AreEqual(genderRestriction, hotel.GenderRestriction);
            Assert.AreEqual(area, hotel.Area);
            Assert.AreEqual(monthlyRent, hotel.RentPrice);
            Assert.AreEqual(isShared, hotel.IsShared);
            Assert.AreEqual(rentUnit, hotel.RentUnit);
            Assert.AreEqual(laundry, hotel.Laundry);
            Assert.AreEqual(ac, hotel.AC);
            Assert.AreEqual(geyser, hotel.Geyser);
            Assert.AreEqual(attachedBathroom, hotel.AttachedBathroom);
            Assert.AreEqual(fitnessCentre, hotel.FitnessCentre);
            Assert.AreEqual(balcony, hotel.Balcony);
            Assert.AreEqual(lawn, hotel.Lawn);
            Assert.AreEqual(ironing, hotel.Ironing);
            Assert.AreEqual(cctvCameras, hotel.CctvCameras);
            Assert.AreEqual(backupElectricity, hotel.BackupElectricity);
            Assert.AreEqual(heating, hotel.Heating);
            Assert.AreEqual(restaurant, hotel.Restaurant);
            Assert.AreEqual(airportShuttle, hotel.AirportShuttle);
            Assert.AreEqual(breakfastIncluded, hotel.BreakfastIncluded);
            Assert.AreEqual(sittingArea, hotel.SittingArea);
            Assert.AreEqual(carRental, hotel.CarRental);
            Assert.AreEqual(spa, hotel.Spa);
            Assert.AreEqual(salon, hotel.Salon);
            Assert.AreEqual(bathtub, hotel.Bathtub);
            Assert.AreEqual(swimmingPool, hotel.SwimmingPool);
            Assert.AreEqual(kitchen, hotel.Kitchen);
            Assert.AreEqual(elevator, hotel.Elevator);
            Assert.AreEqual(landlineNumber, hotel.LandlineNumber);
            Assert.AreEqual(fax, hotel.Fax);
            Assert.AreEqual(numberOfSingleBeds, hotel.NumberOfSingleBeds);
            Assert.AreEqual(numberOfDoubleBeds, hotel.NumberOfDoubleBeds);

            Assert.AreEqual(occupants.Adults, hotel.Occupants.Adults);
            Assert.AreEqual(occupants.Children, hotel.Occupants.Children);
            Assert.AreEqual(occupants.TotalOccupants, hotel.Occupants.TotalOccupants);

            Assert.AreEqual(image1, hotel.Images[0]);
            Assert.AreEqual(image2, hotel.Images[1]);
        }

        // Create a Guest House Instance  and update successfully with all the attributes
        [Test]
        public void CreateGuestHouseInstanceSuccessTest_TestsThatAGuestHouseInstanceIsCreatedAsExpected_VerifiesByTheReturnValue()
        {
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            decimal latitude = 25.43M;
            decimal longitude = 73.41M;
            string propertyType = Constants.GuestHouse;
            GenderRestriction genderRestriction = GenderRestriction.NoRestriction;
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
            
            Occupants occupants = new Occupants(2, 1);
            bool elevator = true;

            string landlineNumber = "0510000000";
            string fax = "0510000000";
            int numberOfSingleBeds = 1;
            int numberOfDoubleBeds = 1;

            Hotel hotel = new Hotel.HotelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).ParkingAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Ironing(ironing)
                .CctvCameras(cctvCameras).BackupElectricity(backupElectricity)
                .Restaurant(restaurant).AirportShuttle(airportShuttle).BreakfastIncluded(breakfastIncluded)
                .SittingArea(sittingArea).CarRental(carRental).Spa(spa).Salon(salon).Bathtub(bathtub)
                .SwimmingPool(swimmingPool).Kitchen(kitchen).Occupants(occupants).Elevator(elevator)
                .LandlineNumber(landlineNumber).Fax(fax).NumberOfSingleBeds(numberOfSingleBeds)
                .NumberOfDoubleBeds(numberOfDoubleBeds)
                .Build();
            hotel.AddImage(image1);
            hotel.AddImage(image2);
            
            Assert.IsNotNull(hotel);
            Assert.AreEqual(title, hotel.Title);
            Assert.AreEqual(description, hotel.Description);
            Assert.AreEqual(email, hotel.OwnerEmail);
            Assert.AreEqual(name, hotel.OwnerName);
            Assert.AreEqual(phoneNumber, hotel.OwnerPhoneNumber);
            Assert.AreEqual(cableTv, hotel.CableTvAvailable);
            Assert.AreEqual(internet, hotel.InternetAvailable);
            Assert.AreEqual(parking, hotel.ParkingAvailable);
            Assert.AreEqual(latitude, hotel.Latitude);
            Assert.AreEqual(longitude, hotel.Longitude);
            Assert.AreEqual(propertyType, hotel.PropertyType);
            Assert.AreEqual(genderRestriction, hotel.GenderRestriction);
            Assert.AreEqual(area, hotel.Area);
            Assert.AreEqual(monthlyRent, hotel.RentPrice);
            Assert.AreEqual(isShared, hotel.IsShared);
            Assert.AreEqual(rentUnit, hotel.RentUnit);
            Assert.AreEqual(laundry, hotel.Laundry);
            Assert.AreEqual(ac, hotel.AC);
            Assert.AreEqual(geyser, hotel.Geyser);
            Assert.AreEqual(attachedBathroom, hotel.AttachedBathroom);
            Assert.AreEqual(fitnessCentre, hotel.FitnessCentre);
            Assert.AreEqual(balcony, hotel.Balcony);
            Assert.AreEqual(lawn, hotel.Lawn);
            Assert.AreEqual(ironing, hotel.Ironing);
            Assert.AreEqual(cctvCameras, hotel.CctvCameras);
            Assert.AreEqual(backupElectricity, hotel.BackupElectricity);
            Assert.AreEqual(heating, hotel.Heating);
            Assert.AreEqual(restaurant, hotel.Restaurant);
            Assert.AreEqual(airportShuttle, hotel.AirportShuttle);
            Assert.AreEqual(breakfastIncluded, hotel.BreakfastIncluded);
            Assert.AreEqual(sittingArea, hotel.SittingArea);
            Assert.AreEqual(carRental, hotel.CarRental);
            Assert.AreEqual(spa, hotel.Spa);
            Assert.AreEqual(salon, hotel.Salon);
            Assert.AreEqual(bathtub, hotel.Bathtub);
            Assert.AreEqual(swimmingPool, hotel.SwimmingPool);
            Assert.AreEqual(kitchen, hotel.Kitchen);

            Assert.AreEqual(numberOfSingleBeds, hotel.NumberOfSingleBeds);
            Assert.AreEqual(numberOfDoubleBeds, hotel.NumberOfDoubleBeds);

            Assert.AreEqual(occupants.Adults, hotel.Occupants.Adults);
            Assert.AreEqual(occupants.Children, hotel.Occupants.Children);
            Assert.AreEqual(occupants.TotalOccupants, hotel.Occupants.TotalOccupants);

            Assert.AreEqual(image1, hotel.Images[0]);
            Assert.AreEqual(image2, hotel.Images[1]);
            Assert.AreEqual(elevator, hotel.Elevator);
            
            string title2 = "Title No 2";
            string description2 = "Description of hotel 2";
            string email2 = "w@12344321-2.com";
            string name2 = "OwnerName2";
            string phoneNumber2 = "03990000002";
            decimal latitude2 = 25.43M;
            decimal longitude2 = 73.41M;
            string propertyType2 = Constants.GuestHouse;
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
            int numberOfSingleBeds2 = 1;
            int numberOfDoubleBeds2 = 1;
            Occupants occupants2 = new Occupants(3, 0);

            string landlineNumber2 = "0510000001";
            string fax2 = "0510000001";
            bool elevator2 = false;

            hotel.Update(title2, monthlyRent2, email2, phoneNumber2, latitude2, longitude2, area2, name2,
                description2, genderRestriction2, isShared2, rentUnit2, internet2, cableTv2, parking2,
                propertyType2, laundry2, ac2, geyser2, fitnessCentre2, attachedBathroom2, ironing2, balcony2, lawn2,
                cctvCameras2, backupElectricity2, heating2, restaurant2, airportShuttle2, breakfastIncluded2,
                sittingArea2, carRental2, spa2, salon2, bathtub2, swimmingPool2, kitchen2, numberOfSingleBeds2,
                numberOfDoubleBeds2, occupants2, landlineNumber2, fax2, elevator2);
            hotel.AddImage(image3);
            hotel.AddImage(image4);

            Assert.IsNotNull(hotel);
            Assert.AreEqual(title2, hotel.Title);
            Assert.AreEqual(description2, hotel.Description);
            Assert.AreEqual(email2, hotel.OwnerEmail);
            Assert.AreEqual(name2, hotel.OwnerName);
            Assert.AreEqual(phoneNumber2, hotel.OwnerPhoneNumber);
            Assert.AreEqual(cableTv2, hotel.CableTvAvailable);
            Assert.AreEqual(internet2, hotel.InternetAvailable);
            Assert.AreEqual(parking2, hotel.ParkingAvailable);
            Assert.AreEqual(latitude2, hotel.Latitude);
            Assert.AreEqual(longitude2, hotel.Longitude);
            Assert.AreEqual(propertyType2, hotel.PropertyType);
            Assert.AreEqual(genderRestriction2, hotel.GenderRestriction);
            Assert.AreEqual(area2, hotel.Area);
            Assert.AreEqual(monthlyRent2, hotel.RentPrice);
            Assert.AreEqual(isShared2, hotel.IsShared);
            Assert.AreEqual(rentUnit2, hotel.RentUnit);
            Assert.AreEqual(laundry2, hotel.Laundry);
            Assert.AreEqual(ac2, hotel.AC);
            Assert.AreEqual(geyser2, hotel.Geyser);
            Assert.AreEqual(attachedBathroom2, hotel.AttachedBathroom);
            Assert.AreEqual(fitnessCentre2, hotel.FitnessCentre);
            Assert.AreEqual(balcony2, hotel.Balcony);
            Assert.AreEqual(lawn2, hotel.Lawn);
            Assert.AreEqual(ironing2, hotel.Ironing);
            Assert.AreEqual(cctvCameras2, hotel.CctvCameras);
            Assert.AreEqual(backupElectricity2, hotel.BackupElectricity);
            Assert.AreEqual(heating2, hotel.Heating);

            Assert.AreEqual(restaurant2, hotel.Restaurant);
            Assert.AreEqual(airportShuttle2, hotel.AirportShuttle);
            Assert.AreEqual(breakfastIncluded2, hotel.BreakfastIncluded);
            Assert.AreEqual(sittingArea2, hotel.SittingArea);
            Assert.AreEqual(carRental2, hotel.CarRental);
            Assert.AreEqual(spa2, hotel.Spa);
            Assert.AreEqual(salon2, hotel.Salon);
            Assert.AreEqual(bathtub2, hotel.Bathtub);
            Assert.AreEqual(swimmingPool2, hotel.SwimmingPool);
            Assert.AreEqual(kitchen2, hotel.Kitchen);
            Assert.AreEqual(landlineNumber2, hotel.LandlineNumber);
            Assert.AreEqual(fax2, hotel.Fax);
            Assert.AreEqual(numberOfSingleBeds2, hotel.NumberOfSingleBeds);
            Assert.AreEqual(numberOfDoubleBeds2, hotel.NumberOfDoubleBeds);

            Assert.AreEqual(occupants2.Adults, hotel.Occupants.Adults);
            Assert.AreEqual(occupants2.Children, hotel.Occupants.Children);
            Assert.AreEqual(occupants2.TotalOccupants, hotel.Occupants.TotalOccupants);

            Assert.AreEqual(4, hotel.Images.Count);
            Assert.AreEqual(image1, hotel.Images[0]);
            Assert.AreEqual(image2, hotel.Images[1]);
            Assert.AreEqual(image3, hotel.Images[2]);
            Assert.AreEqual(image4, hotel.Images[3]);

            Assert.AreEqual(elevator2, hotel.Elevator);
        }

        // Create and then Update a Hotel
        [Test]
        public void CreateAndUpdateHotelInstanceSuccessTest_TestsThatAHotelInstanceIsCreatedAndUpdatedAsExpected_VerifiesByTheReturnValue()
        {
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
            
            Occupants occupants = new Occupants(2, 1);

            string landlineNumber = "0510000000";
            string fax = "0510000000";
            bool elevator = false;

            Hotel hotel = new Hotel.HotelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).ParkingAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Ironing(ironing)
                .CctvCameras(cctvCameras).BackupElectricity(backupElectricity)
                .Restaurant(restaurant).AirportShuttle(airportShuttle).BreakfastIncluded(breakfastIncluded)
                .SittingArea(sittingArea).CarRental(carRental).Spa(spa).Salon(salon).Bathtub(bathtub)
                .SwimmingPool(swimmingPool).Kitchen(kitchen).Occupants(occupants)
                .LandlineNumber(landlineNumber).Fax(fax).Elevator(elevator)
                .Build();
            hotel.AddImage(image1);
            hotel.AddImage(image2);
            
            Assert.IsNotNull(hotel);
            Assert.AreEqual(title, hotel.Title);
            Assert.AreEqual(description, hotel.Description);
            Assert.AreEqual(email, hotel.OwnerEmail);
            Assert.AreEqual(name, hotel.OwnerName);
            Assert.AreEqual(phoneNumber, hotel.OwnerPhoneNumber);
            Assert.AreEqual(cableTv, hotel.CableTvAvailable);
            Assert.AreEqual(internet, hotel.InternetAvailable);
            Assert.AreEqual(parking, hotel.ParkingAvailable);
            Assert.AreEqual(latitude, hotel.Latitude);
            Assert.AreEqual(longitude, hotel.Longitude);
            Assert.AreEqual(propertyType, hotel.PropertyType);
            Assert.AreEqual(genderRestriction, hotel.GenderRestriction);
            Assert.AreEqual(area, hotel.Area);
            Assert.AreEqual(monthlyRent, hotel.RentPrice);
            Assert.AreEqual(isShared, hotel.IsShared);
            Assert.AreEqual(rentUnit, hotel.RentUnit);
            Assert.AreEqual(laundry, hotel.Laundry);
            Assert.AreEqual(ac, hotel.AC);
            Assert.AreEqual(geyser, hotel.Geyser);
            Assert.AreEqual(attachedBathroom, hotel.AttachedBathroom);
            Assert.AreEqual(fitnessCentre, hotel.FitnessCentre);
            Assert.AreEqual(balcony, hotel.Balcony);
            Assert.AreEqual(lawn, hotel.Lawn);
            Assert.AreEqual(ironing, hotel.Ironing);
            Assert.AreEqual(cctvCameras, hotel.CctvCameras);
            Assert.AreEqual(backupElectricity, hotel.BackupElectricity);
            Assert.AreEqual(heating, hotel.Heating);

            Assert.AreEqual(restaurant, hotel.Restaurant);
            Assert.AreEqual(airportShuttle, hotel.AirportShuttle);
            Assert.AreEqual(breakfastIncluded, hotel.BreakfastIncluded);
            Assert.AreEqual(sittingArea, hotel.SittingArea);
            Assert.AreEqual(carRental, hotel.CarRental);
            Assert.AreEqual(spa, hotel.Spa);
            Assert.AreEqual(salon, hotel.Salon);
            Assert.AreEqual(bathtub, hotel.Bathtub);
            Assert.AreEqual(swimmingPool, hotel.SwimmingPool);
            Assert.AreEqual(kitchen, hotel.Kitchen);

            Assert.AreEqual(occupants.Adults, hotel.Occupants.Adults);
            Assert.AreEqual(occupants.Children, hotel.Occupants.Children);
            Assert.AreEqual(occupants.TotalOccupants, hotel.Occupants.TotalOccupants);

            Assert.AreEqual(2, hotel.Images.Count);
            Assert.AreEqual(image1, hotel.Images[0]);
            Assert.AreEqual(image2, hotel.Images[1]);

            Assert.AreEqual(elevator, hotel.Elevator);

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
            Occupants occupants2 = new Occupants(3, 0);

            string landlineNumber2 = "0510000001";
            string fax2 = "0510000001";
            bool elevator2 = true;

            hotel.Update(title2, monthlyRent2, email2, phoneNumber2, latitude2, longitude2, area2, name2,
                description2, genderRestriction2, isShared2, rentUnit2, internet2, cableTv2, parking2,
                propertyType2, laundry2, ac2, geyser2, fitnessCentre2, attachedBathroom2, ironing2, balcony2, lawn2,
                cctvCameras2, backupElectricity2, heating2, restaurant2, airportShuttle2, breakfastIncluded2, 
                sittingArea2, carRental2, spa2, salon2, bathtub2, swimmingPool2, kitchen2, 0, 0, occupants2,
                landlineNumber2, fax2, elevator2);
            hotel.AddImage(image3);
            hotel.AddImage(image4);

            Assert.IsNotNull(hotel);
            Assert.AreEqual(title2, hotel.Title);
            Assert.AreEqual(description2, hotel.Description);
            Assert.AreEqual(email2, hotel.OwnerEmail);
            Assert.AreEqual(name2, hotel.OwnerName);
            Assert.AreEqual(phoneNumber2, hotel.OwnerPhoneNumber);
            Assert.AreEqual(cableTv2, hotel.CableTvAvailable);
            Assert.AreEqual(internet2, hotel.InternetAvailable);
            Assert.AreEqual(parking2, hotel.ParkingAvailable);
            Assert.AreEqual(latitude2, hotel.Latitude);
            Assert.AreEqual(longitude2, hotel.Longitude);
            Assert.AreEqual(propertyType2, hotel.PropertyType);
            Assert.AreEqual(genderRestriction2, hotel.GenderRestriction);
            Assert.AreEqual(area2, hotel.Area);
            Assert.AreEqual(monthlyRent2, hotel.RentPrice);
            Assert.AreEqual(isShared2, hotel.IsShared);
            Assert.AreEqual(rentUnit2, hotel.RentUnit);
            Assert.AreEqual(laundry2, hotel.Laundry);
            Assert.AreEqual(ac2, hotel.AC);
            Assert.AreEqual(geyser2, hotel.Geyser);
            Assert.AreEqual(attachedBathroom2, hotel.AttachedBathroom);
            Assert.AreEqual(fitnessCentre2, hotel.FitnessCentre);
            Assert.AreEqual(balcony2, hotel.Balcony);
            Assert.AreEqual(lawn2, hotel.Lawn);
            Assert.AreEqual(ironing2, hotel.Ironing);
            Assert.AreEqual(cctvCameras2, hotel.CctvCameras);
            Assert.AreEqual(backupElectricity2, hotel.BackupElectricity);
            Assert.AreEqual(heating2, hotel.Heating);

            Assert.AreEqual(restaurant2, hotel.Restaurant);
            Assert.AreEqual(airportShuttle2, hotel.AirportShuttle);
            Assert.AreEqual(breakfastIncluded2, hotel.BreakfastIncluded);
            Assert.AreEqual(sittingArea2, hotel.SittingArea);
            Assert.AreEqual(carRental2, hotel.CarRental);
            Assert.AreEqual(spa2, hotel.Spa);
            Assert.AreEqual(salon2, hotel.Salon);
            Assert.AreEqual(bathtub2, hotel.Bathtub);
            Assert.AreEqual(swimmingPool2, hotel.SwimmingPool);
            Assert.AreEqual(kitchen2, hotel.Kitchen);
            Assert.AreEqual(0, hotel.NumberOfSingleBeds);
            Assert.AreEqual(0, hotel.NumberOfDoubleBeds);

            Assert.AreEqual(occupants2.Adults, hotel.Occupants.Adults);
            Assert.AreEqual(occupants2.Children, hotel.Occupants.Children);
            Assert.AreEqual(occupants2.TotalOccupants, hotel.Occupants.TotalOccupants);
            
            Assert.AreEqual(4, hotel.Images.Count);
            Assert.AreEqual(image1, hotel.Images[0]);
            Assert.AreEqual(image2, hotel.Images[1]);
            Assert.AreEqual(image3, hotel.Images[2]);
            Assert.AreEqual(image4, hotel.Images[3]);
            Assert.AreEqual(elevator2, hotel.Elevator);
        }

        // Only Hotel Property type should be able to get a Hostel instance created
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateHouseFailTest_HostelInstanceShouldNotGetCreatedWhenOtherTypeIsProvided_VerifiesByTheReturnValue()
        {
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            decimal latitude = 25.43M;
            decimal longitude = 73.41M;
            string propertyType = Constants.Hostel;
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

            Occupants occupants = new Occupants(2, 1);

            string landlineNumber = "0510000000";
            string fax = "0510000000";

            Hotel hotel = new Hotel.HotelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).ParkingAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Ironing(ironing)
                .CctvCameras(cctvCameras).BackupElectricity(backupElectricity)
                .Restaurant(restaurant).AirportShuttle(airportShuttle).BreakfastIncluded(breakfastIncluded)
                .SittingArea(sittingArea).CarRental(carRental).Spa(spa).Salon(salon).Bathtub(bathtub)
                .SwimmingPool(swimmingPool).Kitchen(kitchen).Occupants(occupants)
                .LandlineNumber(landlineNumber).Fax(fax)
                .Build();
        }

        // Only landline number is provided, without mobile phone number
        [Test]
        public void CreateHouseWithLandlineNumberTest_HostelInstanceShouldGetCreatedWhenOnlyLandlineNumbersProvided_VerifiesByTheReturnValue()
        {
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
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

            Occupants occupants = new Occupants(2, 1);

            string landlineNumber = "0510000000";
            string fax = "0510000000";

            Hotel hotel = new Hotel.HotelBuilder().OwnerEmail(email).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).ParkingAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Ironing(ironing)
                .CctvCameras(cctvCameras).BackupElectricity(backupElectricity)
                .Restaurant(restaurant).AirportShuttle(airportShuttle).BreakfastIncluded(breakfastIncluded)
                .SittingArea(sittingArea).CarRental(carRental).Spa(spa).Salon(salon).Bathtub(bathtub)
                .SwimmingPool(swimmingPool).Kitchen(kitchen).Occupants(occupants)
                .LandlineNumber(landlineNumber).Fax(fax)
                .Build();
            Assert.IsNotNull(hotel);
            Assert.AreEqual(landlineNumber, hotel.LandlineNumber);
            Assert.IsTrue(string.IsNullOrWhiteSpace(hotel.OwnerPhoneNumber));
        }

        // No Latitude is given, so the test should fail
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHostelFailTest1_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
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

            Occupants occupants = new Occupants(2, 1);

            string landlineNumber = "0510000000";
            string fax = "0510000000";

            Hotel hotel = new Hotel.HotelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).ParkingAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Ironing(ironing)
                .CctvCameras(cctvCameras).BackupElectricity(backupElectricity)
                .Restaurant(restaurant).AirportShuttle(airportShuttle).BreakfastIncluded(breakfastIncluded)
                .SittingArea(sittingArea).CarRental(carRental).Spa(spa).Salon(salon).Bathtub(bathtub)
                .SwimmingPool(swimmingPool).Kitchen(kitchen).Occupants(occupants)
                .LandlineNumber(landlineNumber).Fax(fax)
                .Build();
        }


        // No Longitude is given so the test should fail
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHouseFailTest2_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            decimal latitude = 25.43M;
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

            Occupants occupants = new Occupants(2, 1);

            string landlineNumber = "0510000000";
            string fax = "0510000000";

            Hotel hotel = new Hotel.HotelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).ParkingAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Ironing(ironing)
                .CctvCameras(cctvCameras).BackupElectricity(backupElectricity)
                .Restaurant(restaurant).AirportShuttle(airportShuttle).BreakfastIncluded(breakfastIncluded)
                .SittingArea(sittingArea).CarRental(carRental).Spa(spa).Salon(salon).Bathtub(bathtub)
                .SwimmingPool(swimmingPool).Kitchen(kitchen).Occupants(occupants)
                .LandlineNumber(landlineNumber).Fax(fax)
                .Build();
        }

        // No rent Price is given so the instance creation should fail
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHouseFailTest4_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
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

            Occupants occupants = new Occupants(2, 1);

            string landlineNumber = "0510000000";
            string fax = "0510000000";

            Hotel hotel = new Hotel.HotelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).ParkingAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Ironing(ironing)
                .CctvCameras(cctvCameras).BackupElectricity(backupElectricity)
                .Restaurant(restaurant).AirportShuttle(airportShuttle).BreakfastIncluded(breakfastIncluded)
                .SittingArea(sittingArea).CarRental(carRental).Spa(spa).Salon(salon).Bathtub(bathtub)
                .SwimmingPool(swimmingPool).Kitchen(kitchen).Occupants(occupants)
                .LandlineNumber(landlineNumber).Fax(fax)
                .Build();
        }

        // No Owner Name is given. So the house instance should not be created
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHouseFailTest5_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
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

            Occupants occupants = new Occupants(2, 1);

            string landlineNumber = "0510000000";
            string fax = "0510000000";

            Hotel hotel = new Hotel.HotelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title)
                .CableTvAvailable(cableTv).ParkingAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Ironing(ironing)
                .CctvCameras(cctvCameras).BackupElectricity(backupElectricity)
                .Restaurant(restaurant).AirportShuttle(airportShuttle).BreakfastIncluded(breakfastIncluded)
                .SittingArea(sittingArea).CarRental(carRental).Spa(spa).Salon(salon).Bathtub(bathtub)
                .SwimmingPool(swimmingPool).Kitchen(kitchen).Occupants(occupants)
                .LandlineNumber(landlineNumber).Fax(fax)
                .Build();
        }

        // No OwnerEmail provided, so exception should be raised
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHouseFailTest7_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            string title = "Title No 1";
            string description = "Description of house";
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

            Occupants occupants = new Occupants(2, 1);

            string landlineNumber = "0510000000";
            string fax = "0510000000";

            Hotel hotel = new Hotel.HotelBuilder().OwnerPhoneNumber(phoneNumber).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).ParkingAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Ironing(ironing)
                .CctvCameras(cctvCameras).BackupElectricity(backupElectricity)
                .Restaurant(restaurant).AirportShuttle(airportShuttle).BreakfastIncluded(breakfastIncluded)
                .SittingArea(sittingArea).CarRental(carRental).Spa(spa).Salon(salon).Bathtub(bathtub)
                .SwimmingPool(swimmingPool).Kitchen(kitchen).Occupants(occupants)
                .LandlineNumber(landlineNumber).Fax(fax)
                .Build();
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHouseFailTest8_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            // No Title is provided, so exception should be raised
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

            Occupants occupants = new Occupants(2, 1);

            string landlineNumber = "0510000000";
            string fax = "0510000000";

            Hotel hotel = new Hotel.HotelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber)
                .OwnerName(name).CableTvAvailable(cableTv).ParkingAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Ironing(ironing)
                .CctvCameras(cctvCameras).BackupElectricity(backupElectricity)
                .Restaurant(restaurant).AirportShuttle(airportShuttle).BreakfastIncluded(breakfastIncluded)
                .SittingArea(sittingArea).CarRental(carRental).Spa(spa).Salon(salon).Bathtub(bathtub)
                .SwimmingPool(swimmingPool).Kitchen(kitchen).Occupants(occupants)
                .LandlineNumber(landlineNumber).Fax(fax)
                .Build();
        }
    }
}
