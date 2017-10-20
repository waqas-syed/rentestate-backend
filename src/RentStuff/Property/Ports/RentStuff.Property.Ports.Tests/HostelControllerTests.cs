using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json;
using Ninject;
using NUnit.Framework;
using RentStuff.Common.Ninject.Modules;
using RentStuff.Common.Utilities;
using RentStuff.Property.Application.Ninject.Modules;
using RentStuff.Property.Application.PropertyServices.Commands.CreateCommands;
using RentStuff.Property.Application.PropertyServices.Representation;
using RentStuff.Property.Domain.Model.PropertyAggregate;
using RentStuff.Property.Infrastructure.Persistence.Ninject.Modules;
using RentStuff.Property.Ports.Adapter.Rest.Ninject.Modules;
using RentStuff.Property.Ports.Adapter.Rest.Resources;

namespace RentStuff.Property.Ports.Tests
{
    [TestFixture]
    class HostelControllerTests
    {
        private DatabaseUtility _databaseUtility;
        private StandardKernel _kernel;

        [SetUp]
        public void Setup()
        {
            var connection = StringCipher.DecipheredConnectionString;
            _databaseUtility = new DatabaseUtility(connection);
            _databaseUtility.Create();

            _kernel = new StandardKernel();
            _kernel.Load<PropertyPersistenceNinjectModule>();
            _kernel.Load<CommonNinjectModule>();
            _kernel.Load<PropertyApplicationNinjectModule>();
            _kernel.Load<PropertyPortsNinjectModule>();
            //_databaseUtility.Populate();
            //ShowNhibernateLogging();
            //NhConnectionDecipherService.SetupDecipheredConnectionString();
        }

        [TearDown]
        public void Teardown()
        {
            _databaseUtility.Create();
        }

        #region Save and Get Hostel By Id

        [Test]
        public void SaveAndGetHostelByIdTest_ChecksThatAHostelIsSavedAndRetrievedAsExpected_VerifiesByReturnValue()
        {
            HouseController houseController = _kernel.Get<HouseController>();
            Assert.NotNull(houseController);

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

            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
            houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, email)
                })
            });
            CreateHostelCommand createHostelCommand = new CreateHostelCommand(title, monthlyRent, internet,
                cableTv, parking, propertyType, email, phoneNumber, area, name, description, genderRestriction.ToString(),
                isShared, rentUnit, laundry, ac, geyser, fitnessCentre, attachedBathroom, ironing, balcony, lawn,
                cctvCameras, backupElectricity, heating, landlineNumber, fax, elevator, meals, picknDrop,
                numberOfSeats);
            IHttpActionResult hostelSaveResult = houseController.Post(JsonConvert.SerializeObject(createHostelCommand));
            string hostelId = ((OkNegotiatedContentResult<string>)hostelSaveResult).Content;

            IHttpActionResult response = (IHttpActionResult)houseController.GetHouse(houseId: hostelId,
                propertyType: Constants.House);
            dynamic retrievedHostel = ((OkNegotiatedContentResult<PropertyBaseRepresentation>)response).Content;

            Assert.IsNotNull(retrievedHostel);
            Assert.AreEqual(hostelId, retrievedHostel.Id);
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
            Assert.AreEqual(genderRestriction.ToString(), retrievedHostel.GenderRestriction);
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
        }

        #endregion Save and Get Hostel By Id
    }
}
