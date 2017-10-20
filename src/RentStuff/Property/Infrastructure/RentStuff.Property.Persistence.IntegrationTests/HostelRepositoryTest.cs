using System.Collections.Generic;
using Ninject;
using NUnit.Framework;
using RentStuff.Common.Ninject.Modules;
using RentStuff.Common.Utilities;
using RentStuff.Property.Domain.Model.HostelAggregate;
using RentStuff.Property.Domain.Model.PropertyAggregate;
using RentStuff.Property.Infrastructure.Persistence.Ninject.Modules;

namespace RentStuff.Property.Persistence.IntegrationTests
{
    [TestFixture]
    class HostelRepositoryTest
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

        [Test]
        public void SaveAndUpdateHostelTest_TestsThatHostelInstanceIsSavedAndUpdatedToTheDatabaseAsExpected_VerifiesThroughDatabaseQuery()
        {
            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();

            //Save the Hostel in the repository and retreive it. Primitive test
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
            bool elevator = true;

            Hostel hostel = new Hostel.HostelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).ParkingAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Meals(meals)
                .PicknDrop(picknDrop).NumberOfSeats(numberOfSeats).Ironing(ironing).CctvCameras(cctvCameras)
                .BackupElectricity(backupElectricity).LandlineNumber(landlineNumber).Fax(fax).Elevator(elevator)
                .Build();
            hostel.AddImage(image1);
            hostel.AddImage(image2);

            houseRepository.SaveorUpdate(hostel);

            // Now retrieve the Hostel from the database
            Hostel retrievedHostel = (Hostel)houseRepository.GetPropertyById(hostel.Id);
            Assert.IsNotNull(retrievedHostel);
            Assert.AreEqual(hostel.Id, retrievedHostel.Id);
            Assert.AreEqual(title, retrievedHostel.Title);
            Assert.AreEqual(description, retrievedHostel.Description);
            Assert.AreEqual(email, retrievedHostel.OwnerEmail);
            Assert.AreEqual(name, retrievedHostel.OwnerName);
            Assert.AreEqual(phoneNumber, retrievedHostel.OwnerPhoneNumber);
            Assert.AreEqual(cableTv, retrievedHostel.CableTvAvailable);
            Assert.AreEqual(internet, retrievedHostel.InternetAvailable);
            Assert.AreEqual(parking, retrievedHostel.ParkingAvailable);
            Assert.AreEqual(latitude, retrievedHostel.Latitude);
            Assert.AreEqual(longitude, retrievedHostel.Longitude);
            Assert.AreEqual(propertyType, retrievedHostel.PropertyType);
            Assert.AreEqual(genderRestriction, retrievedHostel.GenderRestriction);
            Assert.AreEqual(area, retrievedHostel.Area);
            Assert.AreEqual(monthlyRent, retrievedHostel.RentPrice);
            Assert.AreEqual(isShared, retrievedHostel.IsShared);
            Assert.AreEqual(rentUnit, retrievedHostel.RentUnit);
            Assert.AreEqual(laundry, retrievedHostel.Laundry);
            Assert.AreEqual(ac, retrievedHostel.AC);
            Assert.AreEqual(geyser, retrievedHostel.Geyser);
            Assert.AreEqual(attachedBathroom, retrievedHostel.AttachedBathroom);
            Assert.AreEqual(fitnessCentre, retrievedHostel.FitnessCentre);
            Assert.AreEqual(balcony, retrievedHostel.Balcony);
            Assert.AreEqual(lawn, retrievedHostel.Lawn);
            Assert.AreEqual(ironing, retrievedHostel.Ironing);
            Assert.AreEqual(cctvCameras, retrievedHostel.CctvCameras);
            Assert.AreEqual(backupElectricity, retrievedHostel.BackupElectricity);
            Assert.AreEqual(heating, retrievedHostel.Heating);
            Assert.AreEqual(meals, retrievedHostel.Meals);
            Assert.AreEqual(picknDrop, retrievedHostel.PicknDrop);
            Assert.AreEqual(numberOfSeats, retrievedHostel.NumberOfSeats);
            Assert.AreEqual(landlineNumber, retrievedHostel.LandlineNumber);
            Assert.AreEqual(fax, retrievedHostel.Fax);

            Assert.AreEqual(2, retrievedHostel.Images.Count);
            Assert.AreEqual(image1, retrievedHostel.Images[0]);
            Assert.AreEqual(image2, retrievedHostel.Images[1]);
            Assert.AreEqual(elevator, retrievedHostel.Elevator);

            // Update the Hostel now with new values and then save in the database
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
            bool elevator2 = false;

            // Call Update now 
            retrievedHostel.Update(title2, monthlyRent2, email2, phoneNumber2, latitude2, longitude2, area2, name2,
                description2, genderRestriction2, isShared2, rentUnit2, internet2, cableTv2, parking2,
                propertyType2, laundry2, ac2, geyser2, fitnessCentre2, attachedBathroom2, ironing2, balcony2, lawn2,
                cctvCameras2, backupElectricity2, meals2, picknDrop2, numberOfSeats2, heating2, landlineNumber2,
                fax2, elevator2);
            retrievedHostel.AddImage(image3);
            retrievedHostel.AddImage(image4);

            houseRepository.SaveorUpdate(retrievedHostel);

            // Now retrieve the Updated Hostel from the database
            Hostel retrievedHostel2 = (Hostel)houseRepository.GetPropertyById(hostel.Id);
            // Check the updated values
            Assert.IsNotNull(retrievedHostel2);
            Assert.AreEqual(title2, retrievedHostel2.Title);
            Assert.AreEqual(description2, retrievedHostel2.Description);
            Assert.AreEqual(email2, retrievedHostel2.OwnerEmail);
            Assert.AreEqual(name2, retrievedHostel2.OwnerName);
            Assert.AreEqual(phoneNumber2, retrievedHostel2.OwnerPhoneNumber);
            Assert.AreEqual(cableTv2, retrievedHostel2.CableTvAvailable);
            Assert.AreEqual(internet2, retrievedHostel2.InternetAvailable);
            Assert.AreEqual(parking2, retrievedHostel2.ParkingAvailable);
            Assert.AreEqual(latitude2, retrievedHostel2.Latitude);
            Assert.AreEqual(longitude2, retrievedHostel2.Longitude);
            Assert.AreEqual(propertyType2, retrievedHostel2.PropertyType);
            Assert.AreEqual(genderRestriction2, retrievedHostel2.GenderRestriction);
            Assert.AreEqual(area2, retrievedHostel2.Area);
            Assert.AreEqual(monthlyRent2, retrievedHostel2.RentPrice);
            Assert.AreEqual(isShared2, retrievedHostel2.IsShared);
            Assert.AreEqual(rentUnit2, retrievedHostel2.RentUnit);
            Assert.AreEqual(laundry2, retrievedHostel2.Laundry);
            Assert.AreEqual(ac2, retrievedHostel2.AC);
            Assert.AreEqual(geyser2, retrievedHostel2.Geyser);
            Assert.AreEqual(attachedBathroom2, retrievedHostel2.AttachedBathroom);
            Assert.AreEqual(fitnessCentre2, retrievedHostel2.FitnessCentre);
            Assert.AreEqual(balcony2, retrievedHostel2.Balcony);
            Assert.AreEqual(lawn2, retrievedHostel2.Lawn);
            Assert.AreEqual(ironing2, retrievedHostel2.Ironing);
            Assert.AreEqual(cctvCameras2, retrievedHostel2.CctvCameras);
            Assert.AreEqual(backupElectricity2, retrievedHostel2.BackupElectricity);
            Assert.AreEqual(heating2, retrievedHostel2.Heating);
            Assert.AreEqual(meals2, retrievedHostel2.Meals);
            Assert.AreEqual(picknDrop2, retrievedHostel2.PicknDrop);
            Assert.AreEqual(numberOfSeats2, retrievedHostel2.NumberOfSeats);
            Assert.AreEqual(landlineNumber2, retrievedHostel2.LandlineNumber);
            Assert.AreEqual(fax2, retrievedHostel2.Fax);

            Assert.AreEqual(4, retrievedHostel2.Images.Count);
            Assert.AreEqual(image1, retrievedHostel2.Images[0]);
            Assert.AreEqual(image2, retrievedHostel2.Images[1]);
            Assert.AreEqual(image3, retrievedHostel2.Images[2]);
            Assert.AreEqual(image4, retrievedHostel2.Images[3]);
            Assert.AreEqual(elevator2, retrievedHostel.Elevator);
        }

        // Save the Hostel instance and then Delete it 
        [Test]
        public void DeleteHostelTest_TestsThatHostelInstanceIsSavedAndDeletedToTheDatabaseAsExpected_VerifiesThroughDatabaseQuery()
        {
            IResidentialPropertyRepository propertyRepository = _kernel.Get<IResidentialPropertyRepository>();

            //Save the Hostel in the repository and retreive it. Primitive test
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
            bool elevator = true;

            Hostel hostel = new Hostel.HostelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).ParkingAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Meals(meals)
                .PicknDrop(picknDrop).NumberOfSeats(numberOfSeats).Ironing(ironing).CctvCameras(cctvCameras)
                .BackupElectricity(backupElectricity).LandlineNumber(landlineNumber).Fax(fax).Elevator(elevator)
                .Build();
            hostel.AddImage(image1);
            hostel.AddImage(image2);

            propertyRepository.SaveorUpdate(hostel);

            // Now retrieve the Hostel from the database
            Hostel retrievedHostel = (Hostel)propertyRepository.GetPropertyById(hostel.Id);
            Assert.IsNotNull(retrievedHostel);
           
            // Now delete the isntance
            propertyRepository.Delete(retrievedHostel);

            // Now retrieve the Hostel from the database again
            Hostel retrievedHostel2 = (Hostel)propertyRepository.GetPropertyById(hostel.Id);
            Assert.IsNull(retrievedHostel2);
        }

        [Test]
        public void
            GetHostelsByEmailTest_TestsThatHostelsAreRetrievedByThePostersEmailAsExpected_VerifiesByReturnedValue()
        {
            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();

            // Hostel # 1: Same Email as the searched one
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
            bool elevator = true;

            Hostel hostel = new Hostel.HostelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).ParkingAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Meals(meals)
                .PicknDrop(picknDrop).NumberOfSeats(numberOfSeats).Ironing(ironing).CctvCameras(cctvCameras)
                .BackupElectricity(backupElectricity).LandlineNumber(landlineNumber).Fax(fax).Elevator(elevator)
                .Build();
            hostel.AddImage(image1);
            hostel.AddImage(image2);

            houseRepository.SaveorUpdate(hostel);
            
            // Hostel # 2: Same email as the searched one
            string title2 = "Title No 2";
            string description2 = "Description of house 2";
            string email2 = "w@12344321.com";
            string name2 = "OwnerName 2";
            string phoneNumber2 = "03990000002";
            decimal latitude2 = 26.43M;
            decimal longitude2 = 74.41M;
            string propertyType2 = Constants.Hostel;
            GenderRestriction genderRestriction2 = GenderRestriction.FamiliesOnly;
            string area2 = "Lahore, Pakistan";
            long monthlyRent2 = 92000;
            string rentUnit2 = Constants.Hourly;

            Hostel hostel2 = new Hostel.HostelBuilder().OwnerEmail(email2).OwnerPhoneNumber(phoneNumber2).Title(title2)
                .OwnerName(name2)
                .PropertyType(propertyType2).RentPrice(monthlyRent2).Latitude(latitude2).Longitude(longitude2)
                .Area(area2).GenderRestriction(genderRestriction2).RentUnit(rentUnit2)
                .Build();

            houseRepository.SaveorUpdate(hostel2);

            // Hostel # 3: Different email from the searched one
            string title3 = "Title No 3";
            string description3 = "Description of house 3";
            string email3 = "w@13344331.com";
            string name3 = "OwnerName 3";
            string phoneNumber3 = "03990000003";
            decimal latitude3 = 36.43M;
            decimal longitude3 = 74.41M;
            string propertyType3 = Constants.Hostel;
            GenderRestriction genderRestriction3 = GenderRestriction.FamiliesOnly;
            string area3 = "Lahore, Pakistan";
            long monthlyRent3 = 93000;
            string rentUnit3 = Constants.Hourly;

            Hostel hostel3 = new Hostel.HostelBuilder().OwnerEmail(email3).OwnerPhoneNumber(phoneNumber).Title(title3)
                .OwnerName(name3)
                .PropertyType(propertyType3).RentPrice(monthlyRent3).Latitude(latitude3).Longitude(longitude3)
                .Area(area3).GenderRestriction(genderRestriction3).RentUnit(rentUnit3)
                .Build();

            houseRepository.SaveorUpdate(hostel3);

            // Search using the owner of the first and second Hostel
            IList<Hostel> retrievedHostel = houseRepository.GetHostelsByOwnerEmail(hostel.OwnerEmail);
            Assert.IsNotNull(retrievedHostel);
            Assert.AreEqual(2, retrievedHostel.Count);
            Assert.AreEqual(title, retrievedHostel[0].Title);
            Assert.AreEqual(description, retrievedHostel[0].Description);
            Assert.AreEqual(email, retrievedHostel[0].OwnerEmail);
            Assert.AreEqual(name, retrievedHostel[0].OwnerName);
            Assert.AreEqual(phoneNumber, retrievedHostel[0].OwnerPhoneNumber);
            Assert.AreEqual(cableTv, retrievedHostel[0].CableTvAvailable);
            Assert.AreEqual(internet, retrievedHostel[0].InternetAvailable);
            Assert.AreEqual(parking, retrievedHostel[0].ParkingAvailable);
            Assert.AreEqual(latitude, retrievedHostel[0].Latitude);
            Assert.AreEqual(longitude, retrievedHostel[0].Longitude);
            Assert.AreEqual(propertyType, retrievedHostel[0].PropertyType);
            Assert.AreEqual(genderRestriction, retrievedHostel[0].GenderRestriction);
            Assert.AreEqual(area, retrievedHostel[0].Area);
            Assert.AreEqual(monthlyRent, retrievedHostel[0].RentPrice);
            Assert.AreEqual(isShared, retrievedHostel[0].IsShared);
            Assert.AreEqual(rentUnit, retrievedHostel[0].RentUnit);
            Assert.AreEqual(laundry, retrievedHostel[0].Laundry);
            Assert.AreEqual(ac, retrievedHostel[0].AC);
            Assert.AreEqual(geyser, retrievedHostel[0].Geyser);
            Assert.AreEqual(attachedBathroom, retrievedHostel[0].AttachedBathroom);
            Assert.AreEqual(fitnessCentre, retrievedHostel[0].FitnessCentre);
            Assert.AreEqual(balcony, retrievedHostel[0].Balcony);
            Assert.AreEqual(lawn, retrievedHostel[0].Lawn);
            Assert.AreEqual(ironing, retrievedHostel[0].Ironing);
            Assert.AreEqual(cctvCameras, retrievedHostel[0].CctvCameras);
            Assert.AreEqual(backupElectricity, retrievedHostel[0].BackupElectricity);
            Assert.AreEqual(heating, retrievedHostel[0].Heating);
            Assert.AreEqual(meals, retrievedHostel[0].Meals);
            Assert.AreEqual(picknDrop, retrievedHostel[0].PicknDrop);
            Assert.AreEqual(numberOfSeats, retrievedHostel[0].NumberOfSeats);
            Assert.AreEqual(landlineNumber, retrievedHostel[0].LandlineNumber);
            Assert.AreEqual(fax, retrievedHostel[0].Fax);

            Assert.AreEqual(2, retrievedHostel[0].Images.Count);
            Assert.AreEqual(image1, retrievedHostel[0].Images[0]);
            Assert.AreEqual(image2, retrievedHostel[0].Images[1]);
            Assert.AreEqual(elevator, retrievedHostel[0].Elevator);

            // Verification of Hostel # 2
            Assert.AreEqual(title2, retrievedHostel[1].Title);
            Assert.AreEqual(email2, retrievedHostel[1].OwnerEmail);
            Assert.AreEqual(name2, retrievedHostel[1].OwnerName);
            Assert.AreEqual(phoneNumber2, retrievedHostel[1].OwnerPhoneNumber);
            Assert.AreEqual(latitude2, retrievedHostel[1].Latitude);
            Assert.AreEqual(longitude2, retrievedHostel[1].Longitude);
            Assert.AreEqual(propertyType2, retrievedHostel[1].PropertyType);
            Assert.AreEqual(genderRestriction2, retrievedHostel[1].GenderRestriction);
            Assert.AreEqual(area2, retrievedHostel[1].Area);
            Assert.AreEqual(monthlyRent2, retrievedHostel[1].RentPrice);
        }

        // Gets the Hostels
        [Test]
        public void GetAllHostels_ChecksThatHostelsAreRetrievedAsExpected_VerifiesByReturnedValue()
        {
            IResidentialPropertyRepository propertyRepository = _kernel.Get<IResidentialPropertyRepository>();

            // Hostel # 1
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            decimal latitude = 25.43M;
            decimal longitude = 73.41M;
            string propertyType = "Hostel";
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
            string area = "Rawalpindi, Pakistan";
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
            bool elevator = true;

            Hostel hostel = new Hostel.HostelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).ParkingAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Meals(meals)
                .PicknDrop(picknDrop).NumberOfSeats(numberOfSeats).Ironing(ironing).CctvCameras(cctvCameras)
                .BackupElectricity(backupElectricity).LandlineNumber(landlineNumber).Fax(fax).Elevator(elevator)
                .Build();
            hostel.AddImage(image1);
            hostel.AddImage(image2);

            propertyRepository.SaveorUpdate(hostel);

            // Hostel # 2
            string title2 = "Title No 2";
            string description2 = "Description of house 2";
            string email2 = "w@12344321-2.com";
            string name2 = "OwnerName 2";
            string phoneNumber2 = "03990000002";
            decimal latitude2 = 26.43M;
            decimal longitude2 = 74.41M;
            string propertyType2 = Constants.Hostel;
            GenderRestriction genderRestriction2 = GenderRestriction.FamiliesOnly;
            string area2 = "Lahore, Pakistan";
            long monthlyRent2 = 92000;
            string rentUnit2 = Constants.Hourly;

            Hostel hostel2 = new Hostel.HostelBuilder().OwnerEmail(email2).OwnerPhoneNumber(phoneNumber2).Title(title2)
                .OwnerName(name2)
                .PropertyType(propertyType2).RentPrice(monthlyRent2).Latitude(latitude2).Longitude(longitude2)
                .Area(area2).GenderRestriction(genderRestriction2).RentUnit(rentUnit2)
                .Build();

            propertyRepository.SaveorUpdate(hostel2);

            IList<Hostel> retrievedHostel = propertyRepository.GetAllHostels();
            Assert.IsNotNull(retrievedHostel);
            Assert.AreEqual(2, retrievedHostel.Count);
            Assert.AreEqual(title, retrievedHostel[0].Title);
            Assert.AreEqual(description, retrievedHostel[0].Description);
            Assert.AreEqual(email, retrievedHostel[0].OwnerEmail);
            Assert.AreEqual(name, retrievedHostel[0].OwnerName);
            Assert.AreEqual(phoneNumber, retrievedHostel[0].OwnerPhoneNumber);
            Assert.AreEqual(cableTv, retrievedHostel[0].CableTvAvailable);
            Assert.AreEqual(internet, retrievedHostel[0].InternetAvailable);
            Assert.AreEqual(parking, retrievedHostel[0].ParkingAvailable);
            Assert.AreEqual(latitude, retrievedHostel[0].Latitude);
            Assert.AreEqual(longitude, retrievedHostel[0].Longitude);
            Assert.AreEqual(propertyType, retrievedHostel[0].PropertyType);
            Assert.AreEqual(genderRestriction, retrievedHostel[0].GenderRestriction);
            Assert.AreEqual(area, retrievedHostel[0].Area);
            Assert.AreEqual(monthlyRent, retrievedHostel[0].RentPrice);
            Assert.AreEqual(isShared, retrievedHostel[0].IsShared);
            Assert.AreEqual(rentUnit, retrievedHostel[0].RentUnit);
            Assert.AreEqual(laundry, retrievedHostel[0].Laundry);
            Assert.AreEqual(ac, retrievedHostel[0].AC);
            Assert.AreEqual(geyser, retrievedHostel[0].Geyser);
            Assert.AreEqual(attachedBathroom, retrievedHostel[0].AttachedBathroom);
            Assert.AreEqual(fitnessCentre, retrievedHostel[0].FitnessCentre);
            Assert.AreEqual(balcony, retrievedHostel[0].Balcony);
            Assert.AreEqual(lawn, retrievedHostel[0].Lawn);
            Assert.AreEqual(ironing, retrievedHostel[0].Ironing);
            Assert.AreEqual(cctvCameras, retrievedHostel[0].CctvCameras);
            Assert.AreEqual(backupElectricity, retrievedHostel[0].BackupElectricity);
            Assert.AreEqual(heating, retrievedHostel[0].Heating);
            Assert.AreEqual(meals, retrievedHostel[0].Meals);
            Assert.AreEqual(picknDrop, retrievedHostel[0].PicknDrop);
            Assert.AreEqual(numberOfSeats, retrievedHostel[0].NumberOfSeats);
            Assert.AreEqual(landlineNumber, retrievedHostel[0].LandlineNumber);
            Assert.AreEqual(fax, retrievedHostel[0].Fax);

            Assert.AreEqual(2, retrievedHostel[0].Images.Count);
            Assert.AreEqual(image1, retrievedHostel[0].Images[0]);
            Assert.AreEqual(image2, retrievedHostel[0].Images[1]);
            Assert.AreEqual(elevator, retrievedHostel[0].Elevator);

            // Verification of Hostel # 2
            Assert.AreEqual(title2, retrievedHostel[1].Title);
            Assert.AreEqual(email2, retrievedHostel[1].OwnerEmail);
            Assert.AreEqual(name2, retrievedHostel[1].OwnerName);
            Assert.AreEqual(phoneNumber2, retrievedHostel[1].OwnerPhoneNumber);
            Assert.AreEqual(latitude2, retrievedHostel[1].Latitude);
            Assert.AreEqual(longitude2, retrievedHostel[1].Longitude);
            Assert.AreEqual(propertyType2, retrievedHostel[1].PropertyType);
            Assert.AreEqual(genderRestriction2, retrievedHostel[1].GenderRestriction);
            Assert.AreEqual(area2, retrievedHostel[1].Area);
            Assert.AreEqual(monthlyRent2, retrievedHostel[1].RentPrice);
        }

        // Search by Coordinates. This test verifies all the attributes have the corect values rather than having
        // the search functionality tested. For search functionality, check SearchResidentialProperty Tests
        [Test]
        public void SearchHostelsByCoordinates_ChecksThatHostelsAreRetrievedAExpected_VerifiesByTheReturnValue()
        {
            IResidentialPropertyRepository propertyRepository = _kernel.Get<IResidentialPropertyRepository>();
            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService =
                _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();

            // Hostel # 1
            string area = "Rawalpindi, Punjab, Pakistan";
            var coordinates = geocodingService.GetCoordinatesFromAddress(area);
            decimal latitude = coordinates.Item1;
            decimal longitude = coordinates.Item2;
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            string propertyType = "Hostel";
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
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
            bool ironing = true;
            bool cctvCameras = true;
            bool backupElectricity = true;
            int numberOfSeats = 3;
            string landlineNumber = "0510000000";
            string fax = "0510000000";
            bool elevator = true;

            Hostel hostel = new Hostel.HostelBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title)
                .OwnerName(name).CableTvAvailable(cableTv).ParkingAvailable(parking).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).Laundry(laundry).AC(ac).Geyser(geyser).AttachedBathroom(attachedBathroom)
                .FitnessCentre(fitnessCentre).Balcony(balcony).Lawn(lawn).Heating(heating).Meals(meals)
                .PicknDrop(picknDrop).NumberOfSeats(numberOfSeats).Ironing(ironing).CctvCameras(cctvCameras)
                .BackupElectricity(backupElectricity).LandlineNumber(landlineNumber).Fax(fax).Elevator(elevator)
                .Build();
            hostel.AddImage(image1);
            hostel.AddImage(image2);

            propertyRepository.SaveorUpdate(hostel);

            // Hostel # 2
            string area2 = "Lahore, Punjab, Pakistan";
            var coordinates2 = geocodingService.GetCoordinatesFromAddress(area2);
            decimal latitude2 = coordinates2.Item1;
            decimal longitude2 = coordinates2.Item2;
            string title2 = "Title No 2";
            string description2 = "Description of house 2";
            string email2 = "w@12344321-2.com";
            string name2 = "OwnerName 2";
            string phoneNumber2 = "03990000002";
            string propertyType2 = Constants.Hostel;
            GenderRestriction genderRestriction2 = GenderRestriction.FamiliesOnly;
            long monthlyRent2 = 92000;
            string rentUnit2 = Constants.Hourly;

            Hostel hostel2 = new Hostel.HostelBuilder().OwnerEmail(email2).OwnerPhoneNumber(phoneNumber2).Title(title2)
                .OwnerName(name2)
                .PropertyType(propertyType2).RentPrice(monthlyRent2).Latitude(latitude2).Longitude(longitude2)
                .Area(area2).GenderRestriction(genderRestriction2).RentUnit(rentUnit2)
                .Build();

            propertyRepository.SaveorUpdate(hostel2);
            IList<Hostel> retrievedHostel = propertyRepository.SearchHostelByCoordinates(latitude,
                 longitude);
            Assert.IsNotNull(retrievedHostel);
            Assert.AreEqual(1, retrievedHostel.Count);
            Assert.AreEqual(title, retrievedHostel[0].Title);
            Assert.AreEqual(description, retrievedHostel[0].Description);
            Assert.AreEqual(email, retrievedHostel[0].OwnerEmail);
            Assert.AreEqual(name, retrievedHostel[0].OwnerName);
            Assert.AreEqual(phoneNumber, retrievedHostel[0].OwnerPhoneNumber);
            Assert.AreEqual(cableTv, retrievedHostel[0].CableTvAvailable);
            Assert.AreEqual(internet, retrievedHostel[0].InternetAvailable);
            Assert.AreEqual(parking, retrievedHostel[0].ParkingAvailable);
            Assert.AreEqual(latitude, retrievedHostel[0].Latitude);
            Assert.AreEqual(longitude, retrievedHostel[0].Longitude);
            Assert.AreEqual(propertyType, retrievedHostel[0].PropertyType);
            Assert.AreEqual(genderRestriction, retrievedHostel[0].GenderRestriction);
            Assert.AreEqual(area, retrievedHostel[0].Area);
            Assert.AreEqual(monthlyRent, retrievedHostel[0].RentPrice);
            Assert.AreEqual(isShared, retrievedHostel[0].IsShared);
            Assert.AreEqual(rentUnit, retrievedHostel[0].RentUnit);
            Assert.AreEqual(laundry, retrievedHostel[0].Laundry);
            Assert.AreEqual(ac, retrievedHostel[0].AC);
            Assert.AreEqual(geyser, retrievedHostel[0].Geyser);
            Assert.AreEqual(attachedBathroom, retrievedHostel[0].AttachedBathroom);
            Assert.AreEqual(fitnessCentre, retrievedHostel[0].FitnessCentre);
            Assert.AreEqual(balcony, retrievedHostel[0].Balcony);
            Assert.AreEqual(lawn, retrievedHostel[0].Lawn);
            Assert.AreEqual(ironing, retrievedHostel[0].Ironing);
            Assert.AreEqual(cctvCameras, retrievedHostel[0].CctvCameras);
            Assert.AreEqual(backupElectricity, retrievedHostel[0].BackupElectricity);
            Assert.AreEqual(heating, retrievedHostel[0].Heating);
            Assert.AreEqual(meals, retrievedHostel[0].Meals);
            Assert.AreEqual(picknDrop, retrievedHostel[0].PicknDrop);
            Assert.AreEqual(numberOfSeats, retrievedHostel[0].NumberOfSeats);
            Assert.AreEqual(landlineNumber, retrievedHostel[0].LandlineNumber);
            Assert.AreEqual(fax, retrievedHostel[0].Fax);
        }

        [Test]
        public void RetreiveAllHousesPaginationTest_ChecksThatThePaginationIsWorkingFine_VerifiesThroughReturnedOutput()
        {
            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();

            // Save more than 10 houses using the same property type
            SaveMultipleHousesUsingGivenIterations(houseRepository, 21);
            
            var retreivedHouses = houseRepository.GetAllHostels();
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(10, retreivedHouses.Count);
        }

        [Test]
        public void RetreiveHousesByCoordinatesPaginationTest_ChecksThatThePaginationIsWorkingFine_VerifiesThroughReturnedOutput()
        {
            IResidentialPropertyRepository houseRepository = _kernel.Get<IResidentialPropertyRepository>();
            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService =
                _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();

            string searchArea = "Pindora, Rawalpindi, Pakistan";
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(searchArea);
            // Save more than 10 houses using the same property type
            SaveMultipleHousesUsingGivenIterations(houseRepository, 21, searchArea,
                coordinatesFromAddress.Item1, coordinatesFromAddress.Item2);
            
            var retreivedHouses = houseRepository.SearchHostelByCoordinates(coordinatesFromAddress.Item1,
                coordinatesFromAddress.Item2);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(10, retreivedHouses.Count);
        }

        [Test]
        public void RetreiveHousesByEmailPaginationTest_ChecksThatThePaginationIsWorkingFine_VerifiesThroughReturnedOutput()
        {
            IResidentialPropertyRepository residentialRepository = _kernel.Get<IResidentialPropertyRepository>();
            RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService =
                _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();

            string ownerEemail = "special@spsp123456-1.com";
            // Save more than 10 houses using the same property type
            SaveMultipleHousesUsingGivenIterations(residentialRepository, 21, email: ownerEemail);

            var retreivedHouses = residentialRepository.GetHostelsByOwnerEmail(ownerEemail);
            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(10, retreivedHouses.Count);
        }

        /// <summary>
        /// We can use this method to create data for pagination tests. Values are simple and repetitive
        /// </summary>
        /// <param name="houseRepository"></param>
        /// <param name="numberOfIterations"></param>
        /// <param name="area"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="email"></param>
        private void SaveMultipleHousesUsingGivenIterations(IResidentialPropertyRepository houseRepository,
            int numberOfIterations, string area = "Pindora, Rawalpindi, Pakistan", decimal latitude = 25.74M,
            decimal longitude = 73.91M, string email = "special@spsp123456-1.com")
        {
            for (int i = 0; i < numberOfIterations; i++)
            {
                string description = "It was a Hobbit Hole. Which means it had good food and a warm hearth.";
                string title = "Title # 1";
                string phoneNumber = "01234567890";
                int rent = 100;
                string ownerName = "Owner Name 1";
                string propertyType = Constants.Hostel;
                Hostel hostel = new Hostel.HostelBuilder().Title(title).OwnerEmail(email)
                    .OwnerPhoneNumber(phoneNumber)
                    .PropertyType(propertyType).RentPrice(rent).Latitude(latitude)
                    .Longitude(longitude)
                    .Area(area)
                    .OwnerName(ownerName)
                    .Description(description)
                    .Build();
                houseRepository.SaveorUpdate(hostel);
            }
        }
    }
}
