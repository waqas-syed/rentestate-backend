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
            bool elevator = true;
            
            Hostel hostel = new Hostel.HostelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).GarageAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Meals(meals)
                .PicknDrop(picknDrop).NumberOfSeats(numberOfSeats).Ironing(ironing).CctvCameras(cctvCameras)
                .BackupElectricity(backupElectricity).Elevator(elevator)
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
            Assert.AreEqual(parking, hostel.ParkingAvailable);
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
            Assert.AreEqual(elevator, hostel.Elevator);
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
            string landlineNumber = "0510000000";
            string fax = "0510000000";
            bool elevator = false;

            Hostel hostel = new Hostel.HostelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).GarageAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Meals(meals)
                .PicknDrop(picknDrop).NumberOfSeats(numberOfSeats).Ironing(ironing).CctvCameras(cctvCameras)
                .BackupElectricity(backupElectricity).LandlineNumber(landlineNumber).Fax(fax).Elevator(elevator)
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
            Assert.AreEqual(parking, hostel.ParkingAvailable);
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
            Assert.AreEqual(landlineNumber, hostel.LandlineNumber);
            Assert.AreEqual(fax, hostel.Fax);

            Assert.AreEqual(2, hostel.Images.Count);
            Assert.AreEqual(image1, hostel.Images[0]);
            Assert.AreEqual(image2, hostel.Images[1]);
            Assert.AreEqual(elevator, hostel.Elevator);
            
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
            string landlineNumber2 = "0510000001";
            string fax2 = "0510000001";
            bool elevator2 = true;

            hostel.Update(title2, monthlyRent2, email2, phoneNumber2, latitude2, longitude2, area2, name2, 
                description2, genderRestriction2, isShared2, rentUnit2, internet2, cableTv2, parking2, 
                propertyType2, laundry2, ac2, geyser2, fitnessCentre2, attachedBathroom2, ironing2, balcony2, lawn2, 
                cctvCameras2, backupElectricity2, meals2, picknDrop2, numberOfSeats2, heating2, landlineNumber2, 
                fax2, elevator2);
            hostel.AddImage(image3);
            hostel.AddImage(image4);

            Assert.IsNotNull(hostel);
            Assert.AreEqual(title2, hostel.Title);
            Assert.AreEqual(description2, hostel.Description);
            Assert.AreEqual(email2, hostel.OwnerEmail);
            Assert.AreEqual(name2, hostel.OwnerName);
            Assert.AreEqual(phoneNumber2, hostel.OwnerPhoneNumber);
            Assert.AreEqual(cableTv2, hostel.CableTvAvailable);
            Assert.AreEqual(internet2, hostel.InternetAvailable);
            Assert.AreEqual(parking2, hostel.ParkingAvailable);
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
            Assert.AreEqual(landlineNumber2, hostel.LandlineNumber);
            Assert.AreEqual(fax2, hostel.Fax);

            Assert.AreEqual(4, hostel.Images.Count);
            Assert.AreEqual(image1, hostel.Images[0]);
            Assert.AreEqual(image2, hostel.Images[1]);
            Assert.AreEqual(image3, hostel.Images[2]);
            Assert.AreEqual(image4, hostel.Images[3]);
            Assert.AreEqual(elevator2, hostel.Elevator);
        }

        // Only Hostel Property type should be able to get a Hostel instance created
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
            string propertyType = "House";
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
                .OwnerName(name).CableTvAvailable(cableTv).GarageAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Meals(meals)
                .PicknDrop(picknDrop).NumberOfSeats(numberOfSeats).Ironing(ironing).CctvCameras(cctvCameras)
                .BackupElectricity(backupElectricity)
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
            string landlineNumber = "0510000000";
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

            Hostel hostel = new Hostel.HostelBuilder().OwnerEmail(email).LandlineNumber(landlineNumber).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).GarageAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Meals(meals)
                .PicknDrop(picknDrop).NumberOfSeats(numberOfSeats).Ironing(ironing).CctvCameras(cctvCameras)
                .BackupElectricity(backupElectricity)
                .Build();
            Assert.IsNotNull(hostel);
            Assert.AreEqual(landlineNumber, hostel.LandlineNumber);
            Assert.IsTrue(string.IsNullOrWhiteSpace(hostel.OwnerPhoneNumber));
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHostelFailTest1_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            string title = "Title No 1";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            string propertyType = "Apartment";

            // No Latitude is given. So the house instance should not be created
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long monthlyRent = 90000;
            decimal longitude = 73.41M;

            Hostel house = new Hostel.HostelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title).OwnerName(name)
                .CableTvAvailable(true)
                .GarageAvailable(true).WithInternetAvailable(true)
                .PropertyType(propertyType).RentPrice(monthlyRent).Longitude(longitude)
                .Area("Pindora").Build();
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHouseFailTest2_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            string title = "Title No 1";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            string propertyType = "Apartment";

            // No Latitude is given. So the house instance should not be created
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long monthlyRent = 90000;
            decimal latitude = 73.41M;

            Hostel house = new Hostel.HostelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title).OwnerName(name)
                .CableTvAvailable(true)
                .GarageAvailable(true).WithInternetAvailable(true)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude)
                .Area("Pindora").Build();
        }
        
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHouseFailTest4_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            string title = "Title No 1";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            string propertyType = "Apartment";

            // No Rent is given. So the house instance should not be created
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            decimal latitude = 34.51M;
            decimal longitude = 73.41M;

            Hostel house = new Hostel.HostelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title).OwnerName(name)
                .CableTvAvailable(true)
                .GarageAvailable(true).WithInternetAvailable(true)
                .PropertyType(propertyType).Latitude(latitude).Longitude(longitude)
                .Area("Pindora").Build();
        }

        // No Owner Name is given. So the house instance should not be created
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHouseFailTest5_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            string title = "Title No 1";
            string email = "w@12344321.com";
            string phoneNumber = "03455138018";
            string propertyType = "Apartment";
            
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long rentPrice = 80000;
            decimal latitude = 34.51M;
            decimal longitude = 73.41M;

            Hostel house = new Hostel.HostelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title)
                .CableTvAvailable(true)
                .GarageAvailable(true).WithInternetAvailable(true)
                .PropertyType(propertyType).Latitude(latitude).Longitude(longitude).RentPrice(rentPrice)
                .Area("Pindora").Build();
        }

        // No OwnerEmail provided, so exception should be raised
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHouseFailTest7_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            string title = "Title No 1";
            string name = "Townsend";
            string phoneNumber = "03455138018";
            string propertyType = "Apartment";

            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long rentPrice = 80000;
            decimal latitude = 34.51M;
            decimal longitude = 73.41M;

            Hostel house = new Hostel.HostelBuilder().OwnerName(name).OwnerPhoneNumber(phoneNumber).Title(title)
                .CableTvAvailable(true)
                .GarageAvailable(true).WithInternetAvailable(true)
                .PropertyType(propertyType).Latitude(latitude).Longitude(longitude).RentPrice(rentPrice)
                .Area("Pindora").Build();
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHouseFailTest8_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            // No Title is provided, so exception should be raised
            string email = "w@12344321.com";
            string name = "Townsend";
            string phoneNumber = "03455138018";
            string propertyType = "Apartment";

            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long rentPrice = 80000;
            decimal latitude = 34.51M;
            decimal longitude = 73.41M;

            Hostel house = new Hostel.HostelBuilder().OwnerName(name).OwnerPhoneNumber(phoneNumber).OwnerEmail(email)
                .CableTvAvailable(true)
                .GarageAvailable(true).WithInternetAvailable(true)
                .PropertyType(propertyType).Latitude(latitude).Longitude(longitude).RentPrice(rentPrice)
                .Area("Pindora").Build();
        }
    }
}
