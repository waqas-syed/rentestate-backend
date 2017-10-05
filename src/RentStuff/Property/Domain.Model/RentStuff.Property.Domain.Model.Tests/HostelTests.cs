﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RentStuff.Property.Domain.Model.HostelAggregate;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Domain.Model.PropertyAggregate;

namespace RentStuff.Property.Domain.Model.Tests
{
    [TestFixture]
    class HostelTests
    {
        [Test]
        public void CreateHostelInstanceSuccessTest_TestsThatAHostelInstanceIsCreatedAsExpected_VerifiesByTheReturnValue()
        {
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            decimal latitude = 25.43M;
            decimal longitude = 73.41M;
            string propertyType = "Hostel";
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
            string area = "Pindora, Rawalpindi, Pakistan";
            long monthlyRent = 90000;
            bool cableTv = false;
            bool internet = true;
            bool parking = true;
            string image1 = "Image1.jpg";
            string image2 = "Image2.png";
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
            
            Hostel hostel = new Hostel.HostelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).Parking(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Meals(meals)
                .PicknDrop(picknDrop).NumberOfSeats(numberOfSeats).Ironing(ironing).CctvCameras(cctvCameras)
                .BackupElectricity(backupElectricity)
                .Build();
            hostel.AddImage(image1);
            hostel.AddImage(image2);

            Assert.IsNotNull(hostel);
            Assert.AreEqual(title, hostel.Title);
            Assert.AreEqual(description, hostel.Description);
            Assert.AreEqual(email, hostel.OwnerEmail);
            Assert.AreEqual(name, hostel.OwnerName);
            Assert.AreEqual(phoneNumber, hostel.OwnerPhoneNumber);
            Assert.AreEqual(cableTv, hostel.CableTvAvailable);
            Assert.AreEqual(internet, hostel.InternetAvailable);
            Assert.AreEqual(parking, hostel.GarageAvailable);
            Assert.AreEqual(latitude, hostel.Latitude);
            Assert.AreEqual(longitude, hostel.Longitude);
            Assert.AreEqual(propertyType, hostel.PropertyType);
            Assert.AreEqual(genderRestriction, hostel.GenderRestriction);
            Assert.AreEqual(area, hostel.Area);
            Assert.AreEqual(monthlyRent, hostel.RentPrice);
            Assert.AreEqual(isShared, hostel.IsShared);
            Assert.AreEqual(rentUnit, hostel.RentUnit);
            Assert.AreEqual(laundry, hostel.Laundry);
            Assert.AreEqual(ac, hostel.AC);
            Assert.AreEqual(geyser, hostel.Geyser);
            Assert.AreEqual(attachedBathroom, hostel.AttachedBathroom);
            Assert.AreEqual(fitnessCentre, hostel.FitnessCentre);
            Assert.AreEqual(balcony, hostel.Balcony);
            Assert.AreEqual(lawn, hostel.Lawn);
            Assert.AreEqual(ironing, hostel.Ironing);
            Assert.AreEqual(cctvCameras, hostel.CctvCameras);
            Assert.AreEqual(backupElectricity, hostel.BackupElectricity);
            Assert.AreEqual(heating, hostel.Heating);
            Assert.AreEqual(meals, hostel.Meals);
            Assert.AreEqual(picknDrop, hostel.PicknDrop);
            Assert.AreEqual(numberOfSeats, hostel.NumberOfSeats);

            Assert.AreEqual(image1, hostel.Images[0]);
            Assert.AreEqual(image2, hostel.Images[1]);
        }

        [Test]
        public void CreateAndUpdateHostelInstanceSuccessTest_TestsThatAHostelInstanceIsCreatedAndUpdatedAsExpected_VerifiesByTheReturnValue()
        {
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            decimal latitude = 25.43M;
            decimal longitude = 73.41M;
            string propertyType = "Hostel";
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
            string area = "Pindora, Rawalpindi, Pakistan";
            long monthlyRent = 90000;
            bool cableTv = false;
            bool internet = true;
            bool parking = true;
            string image1 = "Image1.jpg";
            string image2 = "Image2.png";
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

            Hostel hostel = new Hostel.HostelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).Parking(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Meals(meals)
                .PicknDrop(picknDrop).NumberOfSeats(numberOfSeats).Ironing(ironing).CctvCameras(cctvCameras)
                .BackupElectricity(backupElectricity)
                .Build();
            hostel.AddImage(image1);
            hostel.AddImage(image2);

            Assert.IsNotNull(hostel);
            Assert.AreEqual(title, hostel.Title);
            Assert.AreEqual(description, hostel.Description);
            Assert.AreEqual(email, hostel.OwnerEmail);
            Assert.AreEqual(name, hostel.OwnerName);
            Assert.AreEqual(phoneNumber, hostel.OwnerPhoneNumber);
            Assert.AreEqual(cableTv, hostel.CableTvAvailable);
            Assert.AreEqual(internet, hostel.InternetAvailable);
            Assert.AreEqual(parking, hostel.GarageAvailable);
            Assert.AreEqual(latitude, hostel.Latitude);
            Assert.AreEqual(longitude, hostel.Longitude);
            Assert.AreEqual(propertyType, hostel.PropertyType);
            Assert.AreEqual(genderRestriction, hostel.GenderRestriction);
            Assert.AreEqual(area, hostel.Area);
            Assert.AreEqual(monthlyRent, hostel.RentPrice);
            Assert.AreEqual(isShared, hostel.IsShared);
            Assert.AreEqual(rentUnit, hostel.RentUnit);
            Assert.AreEqual(laundry, hostel.Laundry);
            Assert.AreEqual(ac, hostel.AC);
            Assert.AreEqual(geyser, hostel.Geyser);
            Assert.AreEqual(attachedBathroom, hostel.AttachedBathroom);
            Assert.AreEqual(fitnessCentre, hostel.FitnessCentre);
            Assert.AreEqual(balcony, hostel.Balcony);
            Assert.AreEqual(lawn, hostel.Lawn);
            Assert.AreEqual(ironing, hostel.Ironing);
            Assert.AreEqual(cctvCameras, hostel.CctvCameras);
            Assert.AreEqual(backupElectricity, hostel.BackupElectricity);
            Assert.AreEqual(heating, hostel.Heating);
            Assert.AreEqual(meals, hostel.Meals);
            Assert.AreEqual(picknDrop, hostel.PicknDrop);
            Assert.AreEqual(numberOfSeats, hostel.NumberOfSeats);

            Assert.AreEqual(2, hostel.Images.Count);
            Assert.AreEqual(image1, hostel.Images[0]);
            Assert.AreEqual(image2, hostel.Images[1]);


            string title2 = "Title No 2";
            string description2 = "Description of hostel";
            string email2 = "w@12344321-2.com";
            string name2 = "OwnerName2";
            string phoneNumber2 = "03990000002";
            decimal latitude2 = 25.43M;
            decimal longitude2 = 73.41M;
            string propertyType2 = "Hostel";
            GenderRestriction genderRestriction2 = GenderRestriction.BoysOnly;
            string area2 = "Satellite Town, Rawalpindi, Pakistan";
            long monthlyRent2 = 100000;
            bool cableTv2 = true;
            bool internet2 = false;
            bool parking2 = false;
            string image3 = "Image3.jpg";
            string image4 = "Image4.png";
            string rentUnit2 = "Month";
            bool isShared2 = false;
            bool laundry2 = false;
            bool ac2 = false;
            bool geyser2 = false;
            bool attachedBathroom2 = false;
            bool fitnessCentre2 = true;
            bool balcony2 = true;
            bool lawn2 = false;
            bool heating2 = false;
            bool meals2 = true;
            bool picknDrop2 = false;
            bool ironing2 = false;
            bool cctvCameras2 = false;
            bool backupElectricity2 = false;
            int numberOfSeats2 = 5;

            hostel.Update(title2, monthlyRent2, email2, phoneNumber2, latitude2, longitude2, area2, name2, 
                description2, genderRestriction2, isShared2, rentUnit2, internet2, cableTv2, parking2, 
                propertyType2, laundry2, ac2, geyser2, fitnessCentre2, attachedBathroom2, ironing2, balcony2, lawn2, 
                cctvCameras2, backupElectricity2, meals2, picknDrop2, numberOfSeats2, heating2, null, null);
            hostel.AddImage(image1);
            hostel.AddImage(image2);

            Assert.IsNotNull(hostel);
            Assert.AreEqual(title2, hostel.Title);
            Assert.AreEqual(description2, hostel.Description);
            Assert.AreEqual(email2, hostel.OwnerEmail);
            Assert.AreEqual(name2, hostel.OwnerName);
            Assert.AreEqual(phoneNumber2, hostel.OwnerPhoneNumber);
            Assert.AreEqual(cableTv2, hostel.CableTvAvailable);
            Assert.AreEqual(internet2, hostel.InternetAvailable);
            Assert.AreEqual(parking2, hostel.GarageAvailable);
            Assert.AreEqual(latitude2, hostel.Latitude);
            Assert.AreEqual(longitude2, hostel.Longitude);
            Assert.AreEqual(propertyType2, hostel.PropertyType);
            Assert.AreEqual(genderRestriction2, hostel.GenderRestriction);
            Assert.AreEqual(area2, hostel.Area);
            Assert.AreEqual(monthlyRent2, hostel.RentPrice);
            Assert.AreEqual(isShared2, hostel.IsShared);
            Assert.AreEqual(rentUnit2, hostel.RentUnit);
            Assert.AreEqual(laundry2, hostel.Laundry);
            Assert.AreEqual(ac2, hostel.AC);
            Assert.AreEqual(geyser2, hostel.Geyser);
            Assert.AreEqual(attachedBathroom2, hostel.AttachedBathroom);
            Assert.AreEqual(fitnessCentre2, hostel.FitnessCentre);
            Assert.AreEqual(balcony2, hostel.Balcony);
            Assert.AreEqual(lawn2, hostel.Lawn);
            Assert.AreEqual(ironing2, hostel.Ironing);
            Assert.AreEqual(cctvCameras2, hostel.CctvCameras);
            Assert.AreEqual(backupElectricity2, hostel.BackupElectricity);
            Assert.AreEqual(heating2, hostel.Heating);
            Assert.AreEqual(meals2, hostel.Meals);
            Assert.AreEqual(picknDrop2, hostel.PicknDrop);
            Assert.AreEqual(numberOfSeats2, hostel.NumberOfSeats);

            Assert.AreEqual(4, hostel.Images.Count);
            Assert.AreEqual(image1, hostel.Images[0]);
            Assert.AreEqual(image2, hostel.Images[1]);
        }
    }
}
