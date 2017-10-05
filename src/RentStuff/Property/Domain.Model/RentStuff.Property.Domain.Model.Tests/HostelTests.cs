using System;
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
            string propertyType = "Apartment";
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
    }
}
