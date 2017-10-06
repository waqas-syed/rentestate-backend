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

            houseRepository.SaveorUpdate(hostel);

            // Now retrieve the Hostel from the database
            Hostel retrievedHostel = (Hostel)houseRepository.GetPropertyById(hostel.Id);
            Assert.IsNotNull(retrievedHostel);
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
        
    }
}
