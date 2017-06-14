using System;
using Ninject;
using NUnit.Framework;
using RentStuff.Common;
using RentStuff.Common.NinjectModules;
using RentStuff.Common.Services.LocationServices;
using RentStuff.Services.Application.ApplicationServices;
using RentStuff.Services.Application.Commands;
using RentStuff.Services.Application.Ninject.Modules;
using RentStuff.Services.Infrastructure.Persistence.NinjectModules;

namespace RentStuff.Services.Application.IntegrationTests
{
    [TestFixture]
    public class ServiceApplicationServiceTests
    {
        private DatabaseUtility _databaseUtility;
        private StandardKernel _kernel;

        [SetUp]
        public void Setup()
        {
            //var connection = StringCipher.DecipheredConnectionString;
            var connection = StringCipher.DecipheredConnectionString;
            _databaseUtility = new DatabaseUtility(connection);
            _databaseUtility.Create();
            //NhConnectionDecipherService.SetupDecipheredConnectionString();
            //_databaseUtility.Populate();
            _kernel = new StandardKernel();
            _kernel.Load<CommonNinjectModule>();
            _kernel.Load<ServicesPersistenceNinjectModule>();
            _kernel.Load<ServiceApplicationNinjectModule>();
        }

        [TearDown]
        public void Teardown()
        {
            _databaseUtility.Create();
        }

        // Service Save Test
        [Test]
        public void SaveServiceTest_VerifiesThatTheServiecIsSavedAsExpected_VerifiesByThEDatabaseRetrieval()
        {
            var serviceAppicationService = _kernel.Get<IServiceApplicationService>();
            Assert.NotNull(serviceAppicationService);

            string name = "The Stone Chopper";
            string description = "We make swrods so sharp and strong, they can chop stones";
            string location = "Pindora, Rawalpindi, Pakistan";
            string mobileNumber = "00001000001";
            string serviceEmail = "stone@chopper1234567.com";
            string uploaderEmail = "uploader@chopper1234567.com";
            string serviceProfessionType = "Plumber";
            string serviceEntityType = "Individual";
            DateTime? dateEstablished = DateTime.Now;
            string facebookLink = "https://dummyfacebooklink-123456789-1.com";
            string instagramLink = "https://dummyinstagramlink-123456789-1.com";
            string twitterLink = "https://dummytwitterlink-123456789-1.com";
            string websiteLink = "https://dummywebsitelink-123456789-1.com";

            var createServiceCommand = new CreateServiceCommand(name, description, location, mobileNumber,
                serviceEmail, uploaderEmail, serviceProfessionType, serviceEntityType, dateEstablished,
                facebookLink, instagramLink, twitterLink, websiteLink);
            Assert.IsNotNull(createServiceCommand);

            var savedServiceId = serviceAppicationService.SaveNewService(createServiceCommand);
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId));
            var serviceFullRepresentation = serviceAppicationService.GetServiceById(savedServiceId);
            Assert.IsNotNull(serviceFullRepresentation);
            Assert.AreEqual(name, serviceFullRepresentation.Name);
            Assert.AreEqual(description, serviceFullRepresentation.Description);
            Assert.AreEqual(location, serviceFullRepresentation.Location);
            Assert.AreEqual(mobileNumber, serviceFullRepresentation.MobileNumber);
            Assert.AreEqual(serviceEmail, serviceFullRepresentation.ServiceEmail);
            Assert.AreEqual(serviceProfessionType, serviceFullRepresentation.ServiceProfessionType);
            Assert.AreEqual(serviceEntityType, serviceFullRepresentation.ServiceEntityType);
            AssertDateTime(dateEstablished, serviceFullRepresentation.DateEstablished);
            Assert.AreEqual(facebookLink, serviceFullRepresentation.FacebookLink);
            Assert.AreEqual(twitterLink, serviceFullRepresentation.TwitterLink);
            Assert.AreEqual(instagramLink, serviceFullRepresentation.InstagramLink);
            Assert.AreEqual(websiteLink, serviceFullRepresentation.WebsiteLink);
        }

        // Service Save Test while using only the mandatory fields and providing optional fields as null
        [Test]
        public void SaveServiceWithMandatoryFieldsOnlyTest_VerifiesThatTheServiecIsSavedAsExpected_VerifiesByThEDatabaseRetrieval()
        {
            var serviceAppicationService = _kernel.Get<IServiceApplicationService>();
            Assert.NotNull(serviceAppicationService);

            string name = "The Stone Chopper";
            string description = null;
            string location = "Pindora, Rawalpindi, Pakistan";
            string mobileNumber = "00001000001";
            string serviceEmail = null;
            string uploaderEmail = "uploader@chopper1234567.com";
            string serviceProfessionType = "Interior Designing";
            string serviceEntityType = "Organization";
            DateTime? dateEstablished = null;
            string facebookLink = null;
            string instagramLink = null;
            string twitterLink = null;
            string websiteLink = null;

            var createServiceCommand = new CreateServiceCommand(name, description, location, mobileNumber,
                serviceEmail, uploaderEmail, serviceProfessionType, serviceEntityType, dateEstablished,
                facebookLink, instagramLink, twitterLink, websiteLink);
            Assert.IsNotNull(createServiceCommand);

            var savedServiceId = serviceAppicationService.SaveNewService(createServiceCommand);
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId));
            var serviceFullRepresentation = serviceAppicationService.GetServiceById(savedServiceId);
            Assert.IsNotNull(serviceFullRepresentation);
            Assert.AreEqual(savedServiceId, serviceFullRepresentation.Id);
            Assert.AreEqual(name, serviceFullRepresentation.Name);
            Assert.AreEqual(serviceProfessionType, serviceFullRepresentation.ServiceProfessionType);
            Assert.AreEqual(serviceEntityType, serviceFullRepresentation.ServiceEntityType);
            Assert.AreEqual(mobileNumber, serviceFullRepresentation.MobileNumber);
            Assert.AreEqual(location, serviceFullRepresentation.Location);
            Assert.IsNull(serviceFullRepresentation.Description);
            Assert.IsNull(serviceFullRepresentation.ServiceEmail);
            Assert.IsNull(serviceFullRepresentation.DateEstablished);
            Assert.IsNull(serviceFullRepresentation.FacebookLink);
            Assert.IsNull(serviceFullRepresentation.TwitterLink);
            Assert.IsNull(serviceFullRepresentation.InstagramLink);
            Assert.IsNull(serviceFullRepresentation.WebsiteLink);
        }

        // Service Save and Update test. Ofcourse retrieval is involved as well
        [Test]
        public void SaveAndUpdateServiceTest_VerifiesThatTheServiecIsSavedAsExpected_VerifiesByThEDatabaseRetrieval()
        {
            var serviceApplicationService = _kernel.Get<IServiceApplicationService>();
            Assert.NotNull(serviceApplicationService);
            var geocodingService = _kernel.Get<IGeocodingService>();
            Assert.NotNull(geocodingService);

            string name = "The Stone Chopper";
            string description = "We make swrods so sharp and strong, they can chop stones";
            string location = "Pindora, Rawalpindi, Pakistan";
            string mobileNumber = "00001000001";
            string serviceEmail = "stone@chopper1234567.com";
            string uploaderEmail = "uploader@chopper1234567.com";
            string serviceProfessionType = "Plumber";
            string serviceEntityType = "Individual";
            DateTime? dateEstablished = DateTime.Now;
            string facebookLink = "https://dummyfacebooklink-123456789-1.com";
            string instagramLink = "https://dummyinstagramlink-123456789-1.com";
            string twitterLink = "https://dummytwitterlink-123456789-1.com";
            string websiteLink = "https://dummywebsitelink-123456789-1.com";

            var createServiceCommand = new CreateServiceCommand(name, description, location, mobileNumber,
                serviceEmail, uploaderEmail, serviceProfessionType, serviceEntityType, dateEstablished,
                facebookLink, instagramLink, twitterLink, websiteLink);
            Assert.IsNotNull(createServiceCommand);

            var savedServiceId = serviceApplicationService.SaveNewService(createServiceCommand);
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId));
            var serviceFullRepresentation = serviceApplicationService.GetServiceById(savedServiceId);
            Assert.IsNotNull(serviceFullRepresentation);
            Assert.AreEqual(savedServiceId, serviceFullRepresentation.Id);
            Assert.AreEqual(name, serviceFullRepresentation.Name);
            Assert.AreEqual(description, serviceFullRepresentation.Description);
            Assert.AreEqual(location, serviceFullRepresentation.Location);
            Assert.AreEqual(mobileNumber, serviceFullRepresentation.MobileNumber);
            Assert.AreEqual(serviceEmail, serviceFullRepresentation.ServiceEmail);
            Assert.AreEqual(serviceProfessionType, serviceFullRepresentation.ServiceProfessionType);
            Assert.AreEqual(serviceEntityType, serviceFullRepresentation.ServiceEntityType);
            AssertDateTime(dateEstablished, serviceFullRepresentation.DateEstablished);
            Assert.AreEqual(facebookLink, serviceFullRepresentation.FacebookLink);
            Assert.AreEqual(twitterLink, serviceFullRepresentation.TwitterLink);
            Assert.AreEqual(instagramLink, serviceFullRepresentation.InstagramLink);
            Assert.AreEqual(websiteLink, serviceFullRepresentation.WebsiteLink);

            string name2 = "The Lightning Bolt";
            string description2 = "Bolt, Electrify!";
            string location2 = "Islamabad, Pakistan";
            string mobileNumber2 = "03168948486";
            string serviceEmail2 = null;
            string uploaderEmail2 = "uploader@bolt1234567.com";
            string serviceProfessionType2 = "Electrician";
            string serviceEntityType2 = "Organization";
            string facebookLink2 = "https://dummyfacebooklink-123456789-1.com";
            string twitterLink2 = "https://dummytwitterlink-123456789-1.com";

            // Update the Service
            serviceApplicationService.UpdateService(new UpdateServiceCommand(savedServiceId, name2,
                description2, location2, mobileNumber2, serviceEmail2, uploaderEmail2, serviceProfessionType2,
                serviceEntityType2, dateEstablished, facebookLink2, instagramLink, twitterLink2, websiteLink));
            
            // Retrieve the Service form the database again
            serviceFullRepresentation = serviceApplicationService.GetServiceById(savedServiceId);
            Assert.IsNotNull(serviceFullRepresentation);
            Assert.AreEqual(savedServiceId, serviceFullRepresentation.Id);
            Assert.AreEqual(name2, serviceFullRepresentation.Name);
            Assert.AreEqual(description2, serviceFullRepresentation.Description);
            Assert.AreEqual(location2, serviceFullRepresentation.Location);
            Assert.AreEqual(mobileNumber2, serviceFullRepresentation.MobileNumber);
            Assert.AreEqual(serviceEmail2, serviceFullRepresentation.ServiceEmail);
            Assert.AreEqual(serviceProfessionType2, serviceFullRepresentation.ServiceProfessionType);
            Assert.AreEqual(serviceEntityType2, serviceFullRepresentation.ServiceEntityType);
            AssertDateTime(dateEstablished, serviceFullRepresentation.DateEstablished);
            Assert.AreEqual(facebookLink2, serviceFullRepresentation.FacebookLink);
            Assert.AreEqual(twitterLink2, serviceFullRepresentation.TwitterLink);
            Assert.AreEqual(instagramLink, serviceFullRepresentation.InstagramLink);
            Assert.AreEqual(websiteLink, serviceFullRepresentation.WebsiteLink);
        }

        // Service Save and delete test
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveAndDeleteServiceTest_VerifiesThatTheServiecIsSavedAsExpected_VerifiesByThEDatabaseRetrieval()
        {
            var serviceApplicationService = _kernel.Get<IServiceApplicationService>();
            Assert.NotNull(serviceApplicationService);
            var geocodingService = _kernel.Get<IGeocodingService>();
            Assert.NotNull(geocodingService);

            string name = "The Stone Chopper";
            string description = "We make swrods so sharp and strong, they can chop stones";
            string location = "Pindora, Rawalpindi, Pakistan";
            string mobileNumber = "00001000001";
            string serviceEmail = "stone@chopper1234567.com";
            string uploaderEmail = "uploader@chopper1234567.com";
            string serviceProfessionType = "Plumber";
            string serviceEntityType = "Individual";
            DateTime? dateEstablished = DateTime.Now;
            string facebookLink = "https://dummyfacebooklink-123456789-1.com";
            string instagramLink = "https://dummyinstagramlink-123456789-1.com";
            string twitterLink = "https://dummytwitterlink-123456789-1.com";
            string websiteLink = "https://dummywebsitelink-123456789-1.com";

            var createServiceCommand = new CreateServiceCommand(name, description, location, mobileNumber,
                serviceEmail, uploaderEmail, serviceProfessionType, serviceEntityType, dateEstablished,
                facebookLink, instagramLink, twitterLink, websiteLink);
            Assert.IsNotNull(createServiceCommand);

            //Save the service
            var savedServiceId = serviceApplicationService.SaveNewService(createServiceCommand);
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId));
            var serviceFullRepresentation = serviceApplicationService.GetServiceById(savedServiceId);
            Assert.IsNotNull(serviceFullRepresentation);
            
            // Delete the service
            serviceApplicationService.DeleteService(savedServiceId);
            serviceApplicationService.GetServiceById(savedServiceId);
        }

        // Save multiple services using a single uploader email and then retrieve them using that email
        [Test]
        public void GetServicesByEmail_VerifiesThatTheServiecIsSavedAsExpected_VerifiesByThEDatabaseRetrieval()
        {
            var serviceApplicationService = _kernel.Get<IServiceApplicationService>();
            Assert.NotNull(serviceApplicationService);
            var geocodingService = _kernel.Get<IGeocodingService>();
            Assert.NotNull(geocodingService);
            
            // The email that we will use to search for the corresponding instances
            string mainEmail = "uploader@chopper1234567.com";
            string name = "The Stone Chopper";
            string description = "We make swrods so sharp and strong, they can chop stones";
            string location = "Pindora, Rawalpindi, Pakistan";
            string mobileNumber = "00001000001";
            string serviceEmail = "stone@chopper1234567.com";
            string serviceProfessionType = "Plumber";
            string serviceEntityType = "Individual";
            DateTime? dateEstablished = DateTime.Now;
            string facebookLink = "https://dummyfacebooklink-123456789-1.com";
            string instagramLink = "https://dummyinstagramlink-123456789-1.com";
            string twitterLink = "https://dummytwitterlink-123456789-1.com";
            string websiteLink = "https://dummywebsitelink-123456789-1.com";
            
            // Save the service
            var savedServiceId = serviceApplicationService.SaveNewService(new CreateServiceCommand(
                name, description, location, mobileNumber,
                serviceEmail, mainEmail, serviceProfessionType, serviceEntityType, dateEstablished,
                facebookLink, instagramLink, twitterLink, websiteLink));
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId));

            string name2 = "The Lightning Bolt";
            string description2 = "Bolt, Electrify!";
            string location2 = "Islamabad, Pakistan";
            string mobileNumber2 = "03168948486";
            string serviceEmail2 = "bolt@chopper1234567.com";
            string uploaderEmail2 = "uploader@bolt1234567.com";
            string serviceProfessionType2 = "Electrician";
            string serviceEntityType2 = "Organization";
            DateTime dateEstablished2 = DateTime.Now.AddYears(-2);
            string facebookLink2 = "https://dummyfacebooklink-123456789-2.com";
            string instagramLink2 = "https://dummyinstagramlink-123456789-2.com";
            string twitterLink2 = "https://dummytwitterlink-123456789-2.com";
            string websiteLink2 = "https://dummywebsitelink-123456789-2.com";
            
            //Save the service
            var savedServiceId2 = serviceApplicationService.SaveNewService(new CreateServiceCommand(
                name2, description2, location2, mobileNumber2,
                serviceEmail2, uploaderEmail2, serviceProfessionType2, serviceEntityType2, dateEstablished2,
                facebookLink2, instagramLink2, twitterLink2, websiteLink2));
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId2));

            string name3 = "The Grass Hopper";
            string description3 = "We make choppers, so they can chop grass :D";
            string location3 = "Rawalpindi, Pakistan";
            string mobileNumber3 = "03168948486";
            string serviceEmail3 = "grass@hopper1234567.com";
            string serviceProfessionType3 = "Carpenter";
            string serviceEntityType3 = "Individual";
            DateTime? dateEstablished3 = null;
            string facebookLink3 = "https://dummyfacebooklink-123456789-3.com";
            string instagramLink3 = null;
            string twitterLink3 = null;
            string websiteLink3 = null;
            
            // Save the service
            var savedServiceId3 = serviceApplicationService.SaveNewService(new CreateServiceCommand(
                name3, description3, location3, mobileNumber3,
                serviceEmail3, mainEmail, serviceProfessionType3, serviceEntityType3, dateEstablished3,
                facebookLink3, instagramLink3, twitterLink3, websiteLink3));
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId3));

            // Retrieve the services by email
            var retrievedServices = serviceApplicationService.GetServicesByUploaderEmail(mainEmail);
            Assert.IsNotNull(retrievedServices);
            Assert.AreEqual(2, retrievedServices.Count);
            // Verify House no 1
            var retrievedService = retrievedServices[0];
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name, retrievedService.Name);
            Assert.AreEqual(location, retrievedService.Location);
            Assert.AreEqual(mobileNumber, retrievedService.MobileNumber);
            Assert.AreEqual(serviceEmail, retrievedService.ServiceEmail);
            Assert.AreEqual(serviceProfessionType, retrievedService.ServiceProfessionType);
            Assert.AreEqual(serviceEntityType, retrievedService.ServiceEntityType);
            Assert.AreEqual(facebookLink, retrievedService.FacebookLink);
            Assert.AreEqual(twitterLink, retrievedService.TwitterLink);
            Assert.AreEqual(instagramLink, retrievedService.InstagramLink);
            Assert.AreEqual(websiteLink, retrievedService.WebsiteLink);

            // Verify Service no 3
            retrievedService = retrievedServices[1];
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name3, retrievedService.Name);
            Assert.AreEqual(location3, retrievedService.Location);
            Assert.AreEqual(mobileNumber3, retrievedService.MobileNumber);
            Assert.AreEqual(serviceEmail3, retrievedService.ServiceEmail);
            Assert.AreEqual(serviceProfessionType3, retrievedService.ServiceProfessionType);
            Assert.AreEqual(serviceEntityType3, retrievedService.ServiceEntityType);
            Assert.AreEqual(facebookLink3, retrievedService.FacebookLink);
            Assert.IsNull(retrievedService.TwitterLink);
            Assert.IsNull(retrievedService.InstagramLink);
            Assert.IsNull(retrievedService.WebsiteLink);
        }

        // Gets the Services by providing the location
        [Test]
        public void GetServicesByLocationTest_VerifiesThatTheServicesAreDiscoverableUsingLocationAsExpected_VerifiesByThEDatabaseRetrieval()
        {
            // House # 1 and 2 are saved in locations that are within the range of the searched location, so
            // should be retrieved. House # 3 should be left
            var serviceApplicationService = _kernel.Get<IServiceApplicationService>();
            Assert.NotNull(serviceApplicationService);
            var geocodingService = _kernel.Get<IGeocodingService>();
            Assert.NotNull(geocodingService);
            
            string name = "The Stone Chopper";
            string description = "We make swrods so sharp and strong, they can chop stones";
            string location = "Pindora, Rawalpindi, Pakistan";
            string mobileNumber = "00001000001";
            string serviceEmail = "stone@chopper1234567.com";
            string uploaderEmail = "uploader@chopper1234567.com";
            string serviceProfessionType = "Plumber";
            string serviceEntityType = "Individual";
            DateTime? dateEstablished = DateTime.Now;
            string facebookLink = "https://dummyfacebooklink-123456789-1.com";
            string instagramLink = "https://dummyinstagramlink-123456789-1.com";
            string twitterLink = "https://dummytwitterlink-123456789-1.com";
            string websiteLink = "https://dummywebsitelink-123456789-1.com";

            // Save the service
            var savedServiceId = serviceApplicationService.SaveNewService(new CreateServiceCommand(
                name, description, location, mobileNumber,
                serviceEmail, uploaderEmail, serviceProfessionType, serviceEntityType, dateEstablished,
                facebookLink, instagramLink, twitterLink, websiteLink));
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId));

            string name2 = "The Lightning Bolt";
            string description2 = "Bolt, Electrify!";
            string location2 = "Islamabad, Pakistan";
            string mobileNumber2 = "03168948486";
            string serviceEmail2 = "bolt@chopper1234567.com";
            string uploaderEmail2 = "uploader@bolt1234567.com";
            string serviceProfessionType2 = "Electrician";
            string serviceEntityType2 = "Organization";
            DateTime dateEstablished2 = DateTime.Now.AddYears(-2);
            string facebookLink2 = "https://dummyfacebooklink-123456789-2.com";
            string instagramLink2 = null;
            string twitterLink2 = "https://dummytwitterlink-123456789-2.com";
            string websiteLink2 = null;

            //Save the service
            var savedServiceId2 = serviceApplicationService.SaveNewService(new CreateServiceCommand(
                name2, description2, location2, mobileNumber2,
                serviceEmail2, uploaderEmail2, serviceProfessionType2, serviceEntityType2, dateEstablished2,
                facebookLink2, instagramLink2, twitterLink2, websiteLink2));
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId2));

            string name3 = "The Grass Hopper";
            string description3 = "We make choppers, so they can chop grass :D";
            string location3 = "Kotli Sattian, Punjab, Pakistan";
            string mobileNumber3 = "03168948486";
            string serviceEmail3 = "grass@hopper1234567.com";
            string uploaderEmail3 = "uploader@hop1234567.com";
            string serviceProfessionType3 = "Carpenter";
            string serviceEntityType3 = "Individual";
            DateTime? dateEstablished3 = null;
            string facebookLink3 = "https://dummyfacebooklink-123456789-3.com";
            string instagramLink3 = null;
            string twitterLink3 = "https://dummytwitterlink-123456789-3.com";
            string websiteLink3 = null;

            // Save the service
            var savedServiceId3 = serviceApplicationService.SaveNewService(new CreateServiceCommand(
                name3, description3, location3, mobileNumber3,
                serviceEmail3, uploaderEmail3, serviceProfessionType3, serviceEntityType3, dateEstablished3,
                facebookLink3, instagramLink3, twitterLink3, websiteLink3));
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId3));

            // Retrieve the services by email
            string searchedLocation = "Bahria Town, Rawalpindi, Pakistan";
            var retrievedServices = serviceApplicationService.SearchServicesByLocation(searchedLocation);
            Assert.IsNotNull(retrievedServices);
            Assert.AreEqual(2, retrievedServices.Count);
            // Verify House # 1
            var retrievedService = retrievedServices[0];
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name, retrievedService.Name);
            Assert.AreEqual(location, retrievedService.Location);
            Assert.AreEqual(mobileNumber, retrievedService.MobileNumber);
            Assert.AreEqual(serviceEmail, retrievedService.ServiceEmail);
            Assert.AreEqual(serviceProfessionType, retrievedService.ServiceProfessionType);
            Assert.AreEqual(serviceEntityType, retrievedService.ServiceEntityType);
            Assert.AreEqual(facebookLink, retrievedService.FacebookLink);
            Assert.AreEqual(twitterLink, retrievedService.TwitterLink);
            Assert.AreEqual(instagramLink, retrievedService.InstagramLink);
            Assert.AreEqual(websiteLink, retrievedService.WebsiteLink);

            // Verify Service # 2
            retrievedService = retrievedServices[1];
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name2, retrievedService.Name);
            Assert.AreEqual(location2, retrievedService.Location);
            Assert.AreEqual(mobileNumber2, retrievedService.MobileNumber);
            Assert.AreEqual(serviceEmail2, retrievedService.ServiceEmail);
            Assert.AreEqual(serviceProfessionType2, retrievedService.ServiceProfessionType);
            Assert.AreEqual(serviceEntityType2, retrievedService.ServiceEntityType);
            Assert.AreEqual(facebookLink2, retrievedService.FacebookLink);
            Assert.AreEqual(twitterLink2, retrievedService.TwitterLink);
            Assert.IsNull(retrievedService.InstagramLink);
            Assert.IsNull(retrievedService.WebsiteLink);
        }

        // Gets the Services by providing the location and profession
        [Test]
        public void GetServicesByLocationAndProfessionTypeTest_VerifiesThatTheServicesAreDiscoverableUsingLocationAndProfessionAsExpected_VerifiesByThEDatabaseRetrieval()
        {
            // All 3 locations are within the range of the search location, but only Service no 1 has the 
            // corresponding ServiceProfessionType, so only Service no 1 should be fetched
            var serviceApplicationService = _kernel.Get<IServiceApplicationService>();
            Assert.NotNull(serviceApplicationService);
            var geocodingService = _kernel.Get<IGeocodingService>();
            Assert.NotNull(geocodingService);
            
            // The profession that will be searched
            string searchedProfession = "Carpenter";

            // Service # 1
            string name = "The Stone Chopper";
            string description = "We make swrods so sharp and strong, they can chop stones";
            string location = "F-8, Islamabad, Pakistan";
            string mobileNumber = "00001000001";
            string serviceEmail = "stone@chopper1234567.com";
            string uploaderEmail = "uploader@chopper1234567.com";
            string serviceProfessionType = searchedProfession;
            string serviceEntityType = "Individual";
            DateTime? dateEstablished = DateTime.Now;
            string facebookLink = "https://dummyfacebooklink-123456789-1.com";
            string instagramLink = "https://dummyinstagramlink-123456789-1.com";
            string twitterLink = "https://dummytwitterlink-123456789-1.com";
            string websiteLink = "https://dummywebsitelink-123456789-1.com";

            // Save the service
            var savedServiceId = serviceApplicationService.SaveNewService(new CreateServiceCommand(
                name, description, location, mobileNumber,
                serviceEmail, uploaderEmail, serviceProfessionType, serviceEntityType, dateEstablished,
                facebookLink, instagramLink, twitterLink, websiteLink));
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId));

            // Service # 2
            string name2 = "The Lightning Bolt";
            string description2 = "Bolt, Electrify!";
            string location2 = "Islamabad, Pakistan";
            string mobileNumber2 = "03168948486";
            string serviceEmail2 = "bolt@chopper1234567.com";
            string uploaderEmail2 = "uploader@bolt1234567.com";
            string serviceProfessionType2 = "Electrician";
            string serviceEntityType2 = "Organization";
            DateTime dateEstablished2 = DateTime.Now.AddYears(-2);
            string facebookLink2 = "https://dummyfacebooklink-123456789-2.com";
            string instagramLink2 = null;
            string twitterLink2 = "https://dummytwitterlink-123456789-2.com";
            string websiteLink2 = null;

            //Save the service
            var savedServiceId2 = serviceApplicationService.SaveNewService(new CreateServiceCommand(
                name2, description2, location2, mobileNumber2,
                serviceEmail2, uploaderEmail2, serviceProfessionType2, serviceEntityType2, dateEstablished2,
                facebookLink2, instagramLink2, twitterLink2, websiteLink2));
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId2));

            // Service # 3
            string name3 = "The Grass Hopper";
            string description3 = "We make choppers, so they can chop grass :D";
            string location3 = "F-7, Islamabad, Pakistan";
            string mobileNumber3 = "03168948486";
            string serviceEmail3 = "grass@hopper1234567.com";
            string uploaderEmail3 = "uploader@hop1234567.com";
            string serviceProfessionType3 = "Plumber";
            string serviceEntityType3 = "Individual";
            DateTime? dateEstablished3 = null;
            string facebookLink3 = "https://dummyfacebooklink-123456789-3.com";
            string instagramLink3 = null;
            string twitterLink3 = "https://dummytwitterlink-123456789-3.com";
            string websiteLink3 = null;

            // Save the service
            var savedServiceId3 = serviceApplicationService.SaveNewService(new CreateServiceCommand(
                name3, description3, location3, mobileNumber3,
                serviceEmail3, uploaderEmail3, serviceProfessionType3, serviceEntityType3, dateEstablished3,
                facebookLink3, instagramLink3, twitterLink3, websiteLink3));
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId3));

            // Retrieve the services by email
            string searchedLocation = "Islamabad, Pakistan";
            var retrievedServices = serviceApplicationService.SearchServicesByLocationAndProfession(
                searchedLocation, searchedProfession);
            Assert.IsNotNull(retrievedServices);
            Assert.AreEqual(1, retrievedServices.Count);
            // Verify House # 1
            var retrievedService = retrievedServices[0];
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name, retrievedService.Name);
            Assert.AreEqual(location, retrievedService.Location);
            Assert.AreEqual(mobileNumber, retrievedService.MobileNumber);
            Assert.AreEqual(serviceEmail, retrievedService.ServiceEmail);
            Assert.AreEqual(serviceProfessionType, retrievedService.ServiceProfessionType);
            Assert.AreEqual(serviceEntityType, retrievedService.ServiceEntityType);
            Assert.AreEqual(facebookLink, retrievedService.FacebookLink);
            Assert.AreEqual(twitterLink, retrievedService.TwitterLink);
            Assert.AreEqual(instagramLink, retrievedService.InstagramLink);
            Assert.AreEqual(websiteLink, retrievedService.WebsiteLink);
        }

        // Gets the Services by providing the location
        [Test]
        public void GetServicesByProfessionTypeTest_VerifiesThatTheServicesAreDiscoverableUsingProfessionAsExpected_VerifiesByThEDatabaseRetrieval()
        {
            // 2 Services are stored with the same profession that will be used to search. Service # 2 & 3.
            var serviceApplicationService = _kernel.Get<IServiceApplicationService>();
            Assert.NotNull(serviceApplicationService);
            var geocodingService = _kernel.Get<IGeocodingService>();
            Assert.NotNull(geocodingService);

            // The profession that will be searched
            string searchedProfession = "Carpenter";

            // Service # 1
            string name = "The Stone Chopper";
            string description = "We make swrods so sharp and strong, they can chop stones";
            string location = "F-8, Islamabad, Pakistan";
            string mobileNumber = "00001000001";
            string serviceEmail = "stone@chopper1234567.com";
            string uploaderEmail = "uploader@chopper1234567.com";
            string serviceProfessionType = "Electrician";
            string serviceEntityType = "Individual";
            DateTime? dateEstablished = DateTime.Now;
            string facebookLink = "https://dummyfacebooklink-123456789-1.com";
            string instagramLink = "https://dummyinstagramlink-123456789-1.com";
            string twitterLink = "https://dummytwitterlink-123456789-1.com";
            string websiteLink = "https://dummywebsitelink-123456789-1.com";

            // Save the service
            var savedServiceId = serviceApplicationService.SaveNewService(new CreateServiceCommand(
                name, description, location, mobileNumber,
                serviceEmail, uploaderEmail, serviceProfessionType, serviceEntityType, dateEstablished,
                facebookLink, instagramLink, twitterLink, websiteLink));
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId));

            // Service # 2
            string name2 = "The Lightning Bolt";
            string description2 = "Bolt, Electrify!";
            string location2 = "Islamabad, Pakistan";
            string mobileNumber2 = "03168948486";
            string serviceEmail2 = "bolt@chopper1234567.com";
            string uploaderEmail2 = "uploader@bolt1234567.com";
            string serviceProfessionType2 = searchedProfession;
            string serviceEntityType2 = "Organization";
            DateTime dateEstablished2 = DateTime.Now.AddYears(-2);
            string facebookLink2 = "https://dummyfacebooklink-123456789-2.com";
            string instagramLink2 = null;
            string twitterLink2 = "https://dummytwitterlink-123456789-2.com";
            string websiteLink2 = null;

            //Save the service
            var savedServiceId2 = serviceApplicationService.SaveNewService(new CreateServiceCommand(
                name2, description2, location2, mobileNumber2,
                serviceEmail2, uploaderEmail2, serviceProfessionType2, serviceEntityType2, dateEstablished2,
                facebookLink2, instagramLink2, twitterLink2, websiteLink2));
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId2));

            // Service # 3
            string name3 = "The Grass Hopper";
            string description3 = "We make choppers, so they can chop grass :D";
            string location3 = "F-7, Islamabad, Pakistan";
            string mobileNumber3 = "03168948486";
            string serviceEmail3 = "grass@hopper1234567.com";
            string uploaderEmail3 = "uploader@hop1234567.com";
            string serviceProfessionType3 = searchedProfession;
            string serviceEntityType3 = "Individual";
            DateTime? dateEstablished3 = null;
            string facebookLink3 = "https://dummyfacebooklink-123456789-3.com";
            string instagramLink3 = "https://dummyinstagramlink-123456789-3.com";
            string twitterLink3 = "https://dummytwitterlink-123456789-3.com";
            string websiteLink3 = null;

            // Save the service
            var savedServiceId3 = serviceApplicationService.SaveNewService(new CreateServiceCommand(
                name3, description3, location3, mobileNumber3,
                serviceEmail3, uploaderEmail3, serviceProfessionType3, serviceEntityType3, dateEstablished3,
                facebookLink3, instagramLink3, twitterLink3, websiteLink3));
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId3));

            // Retrieve the services by ServiceProfessionType
            var retrievedServices = serviceApplicationService.SearchServicesByProfession(searchedProfession);
            Assert.IsNotNull(retrievedServices);
            Assert.AreEqual(2, retrievedServices.Count);
            // Verify Service # 2
            var retrievedService = retrievedServices[0];
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name2, retrievedService.Name);
            Assert.AreEqual(location2, retrievedService.Location);
            Assert.AreEqual(mobileNumber2, retrievedService.MobileNumber);
            Assert.AreEqual(serviceEmail2, retrievedService.ServiceEmail);
            Assert.AreEqual(serviceProfessionType2, retrievedService.ServiceProfessionType);
            Assert.AreEqual(serviceEntityType2, retrievedService.ServiceEntityType);
            Assert.AreEqual(facebookLink2, retrievedService.FacebookLink);
            Assert.AreEqual(twitterLink2, retrievedService.TwitterLink);
            Assert.IsNull(retrievedService.InstagramLink);
            Assert.IsNull(retrievedService.WebsiteLink);

            // Verify Service # 3
            retrievedService = retrievedServices[1];
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name3, retrievedService.Name);
            Assert.AreEqual(location3, retrievedService.Location);
            Assert.AreEqual(mobileNumber3, retrievedService.MobileNumber);
            Assert.AreEqual(serviceEmail3, retrievedService.ServiceEmail);
            Assert.AreEqual(serviceProfessionType3, retrievedService.ServiceProfessionType);
            Assert.AreEqual(serviceEntityType3, retrievedService.ServiceEntityType);
            Assert.AreEqual(facebookLink3, retrievedService.FacebookLink);
            Assert.AreEqual(twitterLink3, retrievedService.TwitterLink);
            Assert.AreEqual(instagramLink3, retrievedService.InstagramLink);
            Assert.IsNull(retrievedService.WebsiteLink);
        }

        // ToDo: GetAllServices
        // ToDo: DeleteImages
        // ToDo: AddImage or AddImages???

        #region Private Methods

        /// <summary>
        /// Ignores miliseconds in the datetime provided and then asserts it against the expected DateTime
        /// value
        /// </summary>
        /// <param name="expectedDateTime"></param>
        /// <param name="actualDateTime"></param>
        private void AssertDateTime(DateTime? expectedDateTime, DateTime? actualDateTime)
        {
            if (actualDateTime != null)
            {
                actualDateTime = actualDateTime.Value.AddTicks(-actualDateTime.Value.Ticks % TimeSpan.TicksPerSecond);
            }
            if (expectedDateTime != null)
            {
                expectedDateTime = expectedDateTime.Value.AddTicks(-expectedDateTime.Value.Ticks % TimeSpan.TicksPerSecond);
            }
            Assert.AreEqual(expectedDateTime, actualDateTime);
        }

        #endregion Private Methods
    }
}
