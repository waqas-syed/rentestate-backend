using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using NUnit.Framework;
using RentStuff.Common;
using RentStuff.Common.Ninject.Modules;
using RentStuff.Common.Services.LocationServices;
using RentStuff.Common.Utilities;
using RentStuff.Services.Domain.Model.ServiceAggregate;
using RentStuff.Services.Infrastructure.Persistence.Ninject;
using RentStuff.Services.Infrastructure.Persistence.Ninject.Modules;

namespace RentStuff.Services.Infrastructure.Persistence.Tests
{
    [TestFixture]
    public class ServicesRepositoryTests
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
            _kernel.Load<ServicePersistenceNinjectModule>();
            _kernel.Load<CommonNinjectModule>();
            
        }

        [TearDown]
        public void Teardown()
        {
            _databaseUtility.Create();
        }

        // Saves a Service while assigning values to all mandatory and non-mandatory fields and then retrieves
        // it
        [Test]
        public void ServiceSaveAndRetrieveTest_ChecksIfTheServiceInstanceIsSavedAsExpected_VerifiesByTheReturnedValue()
        {
            // Get the ServiceRepository instance using Ninejct DI
            var servicesRepository = _kernel.Get<IServiceRepository>();

            string name = "The Stone Chopper";
            string description = "We make swrods so sharp and strong, they can chop stones";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03168948486";
            string serviceEmail = "stone@chopper1234567.com";
            string uploaderEmail = "uploader@chopper1234567.com";

            // Get a profession somewhere from the middle of the dictionary
            IReadOnlyList<string> vehicleProfession;
            Service.GetProfessionsList().TryGetValue("Vehicle Services", out vehicleProfession);
            Assert.IsNotNull(vehicleProfession);
            string serviceProfessionType = vehicleProfession[3];
            string entity = "Organization";
            DateTime? dateEstablished = DateTime.Now;
            decimal latitude = 33.7M;
            decimal longitude = 73.1M;
            string facebookLink = "https://dummyfacebooklink-123456789-1.com";
            string instagramLink = "https://dummyinstagramlink-123456789-1.com";
            string twitterLink = "https://dummytwitterlink-123456789-1.com";
            string websiteLink = "https://dummywebsitelink-123456789-1.com";
            string secondaryMobileNumber = "00000000002";
            string landlinePhoneNumber = "0000000001";
            string fax = "0000000003";
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProfessionType)
                .ServiceEntityType(entity).DateEstablished(dateEstablished)
                .Latitude(latitude).Longitude(longitude).SecondaryMobileNumber(secondaryMobileNumber)
                .FacebookLink(facebookLink).TwitterLink(twitterLink).InstagramLink(instagramLink)
                .WebsiteLink(websiteLink).LandlinePhoneNumber(landlinePhoneNumber).Fax(fax)
                .Build();
            string image1Id = Guid.NewGuid().ToString();
            string image2Id = Guid.NewGuid().ToString();
            string image3Id = Guid.NewGuid().ToString();
            service.AddImage(image1Id);
            service.AddImage(image2Id);
            service.AddImage(image3Id);

            // Provide a review to save
            string authorName = "King Arthur";
            string authorEmail = "kingofthelingbling@kingdomcup12345678.com";
            string reviewDescription = "Off goes your head";
            service.AddReview(authorName, authorEmail, reviewDescription);
            // Save Service in the repository
            servicesRepository.SaveOrUpdate(service);

            // Retrieve the result
            var retrievedService = servicesRepository.GetServiceById(service.Id);
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name, retrievedService.Name);
            Assert.AreEqual(description, retrievedService.Description);
            Assert.AreEqual(location, retrievedService.Location);
            Assert.AreEqual(phoneNumber, retrievedService.MobileNumber);
            Assert.AreEqual(serviceEmail, retrievedService.ServiceEmail);
            Assert.AreEqual(uploaderEmail, retrievedService.UploaderEmail);
            Assert.AreEqual(serviceProfessionType, retrievedService.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), entity),
                retrievedService.ServiceEntityType);
            AssertDateTime(dateEstablished, retrievedService.DateEstablished);
            Assert.AreEqual(latitude, retrievedService.Latitude);
            Assert.AreEqual(longitude, retrievedService.Longitude);
            Assert.AreEqual(facebookLink, retrievedService.FacebookLink);
            Assert.AreEqual(twitterLink, retrievedService.TwitterLink);
            Assert.AreEqual(instagramLink, retrievedService.InstagramLink);
            Assert.AreEqual(websiteLink, retrievedService.WebsiteLink);
            Assert.AreEqual(secondaryMobileNumber, service.SecondaryMobileNumber);
            Assert.AreEqual(landlinePhoneNumber, service.LandlinePhoneNumber);
            Assert.AreEqual(fax, service.Fax);
            Assert.AreEqual(DateTime.Now.Date, service.DatePosted.Date);
            Assert.AreEqual(DateTime.Now.Date, service.DatePosted.Date);
            Assert.AreEqual(fax, service.Fax);

            // Check the Reviews
            Assert.IsNotNull(retrievedService.Reviews);
            Assert.AreEqual(1, retrievedService.Reviews.Count);
            Assert.AreEqual(authorName, retrievedService.Reviews[0].AuthorName);
            Assert.AreEqual(authorEmail, retrievedService.Reviews[0].AuthorEmail);
            Assert.AreEqual(reviewDescription, retrievedService.Reviews[0].ReviewDescription);
            Assert.AreEqual(service, retrievedService.Reviews[0].Service);

            // Check the Service Images
            Assert.NotNull(retrievedService.Images);
            Assert.AreEqual(3, retrievedService.Images.Count);
            Assert.AreEqual(image1Id, retrievedService.Images[0]);
            Assert.AreEqual(image2Id, retrievedService.Images[1]);
            Assert.AreEqual(image3Id, retrievedService.Images[2]);
        }

        // Service Images Save + Delete test
        [Test]
        public void ServiceImagesSaveAndDeleteTest_ChecksIfTheServiceImagesAreSavedAndDeetedAsExpected_VerifiesByTheReturnedValue()
        {
            // Get the ServiceRepository instance using Ninejct DI
            var servicesRepository = _kernel.Get<IServiceRepository>();

            string name = "The Stone Chopper";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03168948486";
            string uploaderEmail = "uploader@chopper1234567.com";
            
            string serviceProfessionType = Service.GetProfessionsList().First().Value.First();
            string entity = "Organization";
            decimal latitude = 33.7M;
            decimal longitude = 73.1M;
            Service service = new Service.ServiceBuilder().Name(name)
                .Location(location).PhoneNumber(phoneNumber)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProfessionType)
                .ServiceEntityType(entity)
                .Latitude(latitude).Longitude(longitude).Build();

            // Add some images to the Service
            string image1Id = Guid.NewGuid().ToString();
            string image2Id = Guid.NewGuid().ToString();
            string image3Id = Guid.NewGuid().ToString();
            service.AddImage(image1Id);
            service.AddImage(image2Id);
            service.AddImage(image3Id);
            // Save Service in the repository
            servicesRepository.SaveOrUpdate(service);

            // Retrieve the result
            var retrievedService = servicesRepository.GetServiceById(service.Id);
            Assert.IsNotNull(retrievedService);
            // Check the Service Images
            Assert.NotNull(retrievedService.Images);
            Assert.AreEqual(3, retrievedService.Images.Count);
            Assert.AreEqual(image1Id, retrievedService.Images[0]);
            Assert.AreEqual(image2Id, retrievedService.Images[1]);
            Assert.AreEqual(image3Id, retrievedService.Images[2]);

            retrievedService.DeleteImage(image2Id);
            servicesRepository.SaveOrUpdate(retrievedService);
            // Retrieve the result
            retrievedService = servicesRepository.GetServiceById(service.Id);
            Assert.IsNotNull(retrievedService);
            // Check the Service Images
            Assert.NotNull(retrievedService.Images);
            Assert.AreEqual(2, retrievedService.Images.Count);
            Assert.AreEqual(image1Id, retrievedService.Images[0]);
            Assert.AreEqual(image3Id, retrievedService.Images[1]);

            // Now provide an iamge id that doesn't exist in the Images List
            retrievedService.DeleteImage(image3Id + "1");
            servicesRepository.SaveOrUpdate(retrievedService);
            // Retrieve the result
            retrievedService = servicesRepository.GetServiceById(service.Id);
            Assert.IsNotNull(retrievedService);
            // Check the Service Images
            Assert.NotNull(retrievedService.Images);
            Assert.AreEqual(2, retrievedService.Images.Count);
            Assert.AreEqual(image1Id, retrievedService.Images[0]);
            Assert.AreEqual(image3Id, retrievedService.Images[1]);
        }

        // Service Save + update + delete test
        [Test]
        public void ServiceSaveUpdateAndRetrieveTest_ChecksIfTheServiceInstanceIsSavedAndThenUpdatedAsExpected_VerifiesByTheReturnedValue()
        {
            // Get the ServiceRepository instance using Ninejct DI
            var servicesRepository = _kernel.Get<IServiceRepository>();

            string name = "The Stone Chopper";
            string description = "We make swrods so sharp and strong, they can chop stones";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string serviceEmail = "stone@chopper1234567.com";
            string uploaderEmail = "uploader@chopper1234567.com";
            string serviceProfessionType = Service.GetProfessionsList().First().Value.First();
            string entity = "Organization";
            DateTime dateEstablished = DateTime.Now;
            decimal latitude = 33.7M;
            decimal longitude = 73.1M;
            string facebookLink = "https://dummyfacebooklink-123456789-1.com";
            string instagramLink = "https://dummyinstagramlink-123456789-1.com";
            string twitterLink = "https://dummytwitterlink-123456789-1.com";
            string websiteLink = "https://dummywebsitelink-123456789-1.com";
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProfessionType)
                .ServiceEntityType(entity).DateEstablished(dateEstablished)
                .Latitude(latitude).Longitude(longitude).FacebookLink(facebookLink)
                .TwitterLink(twitterLink).InstagramLink(instagramLink).WebsiteLink(websiteLink).Build();
            
            // Save the Service
            servicesRepository.SaveOrUpdate(service);

            // Retrieve the result
            var retrievedService = servicesRepository.GetServiceById(service.Id);         
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name, retrievedService.Name);
            Assert.AreEqual(description, retrievedService.Description);
            Assert.AreEqual(location, retrievedService.Location);
            Assert.AreEqual(phoneNumber, retrievedService.MobileNumber);
            Assert.AreEqual(serviceEmail, retrievedService.ServiceEmail);
            Assert.AreEqual(uploaderEmail, retrievedService.UploaderEmail);
            Assert.AreEqual(serviceProfessionType, retrievedService.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), entity),
                retrievedService.ServiceEntityType);
            AssertDateTime(dateEstablished, retrievedService.DateEstablished);
            Assert.AreEqual(latitude, retrievedService.Latitude);
            Assert.AreEqual(longitude, retrievedService.Longitude);
            Assert.AreEqual(facebookLink, retrievedService.FacebookLink);
            Assert.AreEqual(twitterLink, retrievedService.TwitterLink);
            Assert.AreEqual(instagramLink, retrievedService.InstagramLink);
            Assert.AreEqual(websiteLink, retrievedService.WebsiteLink);

            // Now update the Service
            string name2 = "The Grass Hopper";
            string description2 = "We make swords so sharp and strong, they can chop grass :D";
            string location2 = "Satellite Town, Rawalpindi, Pakistan";
            string phoneNumber2 = "03168948486";
            string serviceEmail2 = "grass@hopper1234567.com";
            string profession2 = Service.GetProfessionsList().Last().Value.First();
            string entity2 = "Individual";
            decimal latitude2 = 34.7M;
            decimal longitude2 = 74.1M;
            string facebookLink2 = "https://dummyfacebooklink-123456789-2.com";
            string instagramLink2 = "https://dummyinstagramlink-123456789-2.com";
            string twitterLink2 = "https://dummytwitterlink-123456789-2.com";
            string websiteLink2 = "https://dummywebsitelink-123456789-2.com";
            DateTime dateEstablished2 = DateTime.Now.AddDays(1);
            retrievedService.UpdateService(name2, description2, location2, phoneNumber2, serviceEmail2,
                profession2, entity2, dateEstablished2, latitude2, longitude2,
                facebookLink2, instagramLink2, twitterLink2, websiteLink2);

            retrievedService = servicesRepository.GetServiceById(service.Id);
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name2, retrievedService.Name);
            Assert.AreEqual(description2, retrievedService.Description);
            Assert.AreEqual(location2, retrievedService.Location);
            Assert.AreEqual(phoneNumber2, retrievedService.MobileNumber);
            Assert.AreEqual(serviceEmail2, retrievedService.ServiceEmail);
            Assert.AreEqual(uploaderEmail, retrievedService.UploaderEmail);
            Assert.AreEqual(profession2, retrievedService.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), entity2),
                retrievedService.ServiceEntityType);
            AssertDateTime(dateEstablished2, retrievedService.DateEstablished);
            Assert.AreEqual(latitude2, retrievedService.Latitude);
            Assert.AreEqual(longitude2, retrievedService.Longitude);
            Assert.AreEqual(facebookLink2, retrievedService.FacebookLink);
            Assert.AreEqual(twitterLink2, retrievedService.TwitterLink);
            Assert.AreEqual(instagramLink2, retrievedService.InstagramLink);
            Assert.AreEqual(websiteLink2, retrievedService.WebsiteLink);
        }

        // Saves the service wih all the mandatory fields leaving out non-mandatory ones
        [Test]
        public void ServiceSaveWithoutMandatoryFieldsTest_ChecksIfTheServiceInstanceIsSavedWithoutThemandatoryFieldsAsExpected_VerifiesByTheReturnedValue()
        {
            // Get the ServiceRepository instance using Ninejct DI
            var servicesRepository = _kernel.Get<IServiceRepository>();

            string name = "The Stone Chopper";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string serviceEmail = "stone@chopper1234567.com";
            string uploaderEmail = "uploader@chopper1234567.com";
            string profession = Service.GetProfessionsList().First().Value.First();
            string entity = "Organization";
            decimal latitude = 34.7M;
            decimal longitude = 74.1M;
            Service service = new Service.ServiceBuilder().Name(name)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(profession).ServiceEntityType(entity)
                .Latitude(latitude).Longitude(longitude).Build();
            
            // Save the Service
            servicesRepository.SaveOrUpdate(service);

            // Retrieve the result and just check that we got one
            var retrievedService = servicesRepository.GetServiceById(service.Id);
            Assert.IsNotNull(retrievedService);
        }

        // DateEstablished assigned null Test
        [Test]
        public void ServiceSaveWithDateEstablishedAsNull_ChecksIfTheServiceInstanceIsSavedWithTheDateAssignedNullAsExpected_VerifiesByTheReturnedValue()
        {
            // Get the ServiceRepository instance using Ninejct DI
            var servicesRepository = _kernel.Get<IServiceRepository>();

            string name = "The Stone Chopper";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string serviceEmail = "stone@chopper1234567.com";
            string uploaderEmail = "uploader@chopper1234567.com";
            string profession = Service.GetProfessionsList().First().Value.First();
            string entity = "Organization";
            decimal latitude = 34.7M;
            decimal longitude = 74.1M;
            DateTime? dateEstablished = null;
            Service service = new Service.ServiceBuilder().Name(name)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(profession).ServiceEntityType(entity)
                .Latitude(latitude).Longitude(longitude).DateEstablished(dateEstablished).Build();

            // Save the Service
            servicesRepository.SaveOrUpdate(service);

            // Retrieve the result and just check that we got one
            var retrievedService = servicesRepository.GetServiceById(service.Id);
            Assert.IsNotNull(retrievedService);
        }

        // Search using the coordinates
        [Test]
        public void SearchServiceUsingLocationCoordinatesTest_ChecksIfTheServiceInstanceIsSavedWithoutThemandatoryFieldsAsExpected_VerifiesByTheReturnedValue()
        {
            // This test saves 6 services at distant locations in Rawalpindi and Islamabad and then queries 
            // one location(Bahria Town). Services within the defined radius( approx 38 kilometers) are
            // retreived. Services outside of this radius(Sangada, KPK) are ignored
            // This test also includes the zero points, i.e., Islamabad, Pakistan & Rawalpindi, Pakistan 
            // directly

            // Get the ServiceRepository instance using Ninejct DI
            var servicesRepository = _kernel.Get<IServiceRepository>();

            // Get the GeocodingService from the Common module
            var geocodingService = _kernel.Get<IGeocodingService>();
            
            // House # 1
            string name = "The Stone Chopper";
            string description = "Our swords can chop stones. Easily!!!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string serviceEmail = "stone@chopper1234567.com";
            string uploaderEmail = "uploader@chopper1234567.com";
            string serviceProfessionType = Service.GetProfessionsList().First().Value.First();
            string serviceEntityType = "Organization";
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(location);
            decimal latitude = coordinatesFromAddress.Item1;
            decimal longitude = coordinatesFromAddress.Item2;
            DateTime dateEstablished = DateTime.Now.AddYears(-2);
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProfessionType)
                .ServiceEntityType(serviceEntityType)
                .Latitude(latitude).Longitude(longitude).DateEstablished(dateEstablished).Build();
            // Provide a review to save
            string reviewAuthorName = "King Arthur";
            string reviewAuthorEmail = "kingofthelingbling@kingdomcup12345678.com";
            string reviewDescription = "Off goes your head";
            service.AddReview(reviewAuthorName, reviewAuthorEmail, reviewDescription);
            // Save the Service
            servicesRepository.SaveOrUpdate(service);
            
            // House # 2
            string name2 = "The Lightning Bolt";
            string description2 = "Bolt, Electrify!";
            string location2 = "Islamabad, Pakistan";
            string phoneNumber2 = "03168948486";
            string serviceEmail2 = "bolt@chopper1234567.com";
            string uploaderEmail2 = "uploader@bolt1234567.com";
            string serviceProfessionType2 = Service.GetProfessionsList().First().Value.Last();
            string serviceEntityType2 = "Organization";
            var coordinatesFromAddress2 = geocodingService.GetCoordinatesFromAddress(location2);
            decimal latitude2 = coordinatesFromAddress2.Item1;
            decimal longitude2 = coordinatesFromAddress2.Item2;
            DateTime dateEstablished2 = DateTime.Now.AddYears(-2);
            Service service2 = new Service.ServiceBuilder().Name(name2).Description(description2)
                .Location(location2).PhoneNumber(phoneNumber2).ServiceEmail(serviceEmail2)
                .UploaderEmail(uploaderEmail2).ServiceProfessionType(serviceProfessionType2).ServiceEntityType(serviceEntityType2)
                .Latitude(latitude2).Longitude(longitude2).DateEstablished(dateEstablished2).Build();
            // Save the Service
            servicesRepository.SaveOrUpdate(service2);

            // House # 3
            string name3 = "The Grass Hopper";
            string description3 = "We make choppers, so they can chop grass :D";
            string location3 = "E-11, Islamabad, Islamabad Capital Territory, Pakistan";
            string phoneNumber3 = "03168948486";
            string serviceEmail3 = "grass@hopper1234567.com";
            string uploaderEmail3 = "uploader@hopper1234567.com";
            string serviceProfessionType3 = Service.GetProfessionsList().Last().Value.First();
            string serviceEntityType3 = "Individual";
            var coordinatesFromAddress3 = geocodingService.GetCoordinatesFromAddress(location3);
            decimal latitude3 = coordinatesFromAddress3.Item1;
            decimal longitude3 = coordinatesFromAddress3.Item2;
            DateTime dateEstablished3 = DateTime.Now.AddYears(-3);
            Service service3 = new Service.ServiceBuilder().Name(name3).Description(description3)
                .Location(location3).PhoneNumber(phoneNumber3).ServiceEmail(serviceEmail3)
                .UploaderEmail(uploaderEmail3).ServiceProfessionType(serviceProfessionType3)
                .ServiceEntityType(serviceEntityType3)
                .Latitude(latitude3).Longitude(longitude3).DateEstablished(dateEstablished3).Build();
            // Provide a review to save
            string reviewAuthorName3 = "King Arthur 3";
            string reviewAuthorEmail3 = "kingofthelingbling3@kingdomcup12345678.com";
            string reviewDescription3 = "Off goes your head 3 times";
            service3.AddReview(reviewAuthorName3, reviewAuthorEmail3, reviewDescription3);
            // Save the Service
            servicesRepository.SaveOrUpdate(service3);

            // House # 4
            string name4 = "The Onion Chopper";
            string description4 = "Meat, carbs and sauces. All combined in perfect proportions.";
            string location4 = "Bahria Town, Rawalpindi, Punjab, Pakistan";
            string phoneNumber4 = "03455138018";
            string serviceEmail4 = "onion@chopper1234567.com";
            string uploaderEmail4 = "uploader@onion1234567.com";
            string serviceProfessionType4 = Service.GetProfessionsList().Last().Value.Last();
            string serviceEntityType4 = "Organization";
            var coordinatesFromAddress4 = geocodingService.GetCoordinatesFromAddress(location4);
            decimal latitude4 = coordinatesFromAddress4.Item1;
            decimal longitude4 = coordinatesFromAddress4.Item2;
            DateTime dateEstablished4 = DateTime.Now.AddYears(-4);
            Service service4 = new Service.ServiceBuilder().Name(name4).Description(description4)
                .Location(location4).PhoneNumber(phoneNumber4).ServiceEmail(serviceEmail4)
                .UploaderEmail(uploaderEmail4).ServiceProfessionType(serviceProfessionType4)
                .ServiceEntityType(serviceEntityType4)
                .Latitude(latitude4).Longitude(longitude4).DateEstablished(dateEstablished4).Build();
            // Save the Service
            servicesRepository.SaveOrUpdate(service4);

            // House # 5
            string name5 = "The Lone Artist";
            string description5 = "Whatever colors you need will be painted. But by my imagination.";
            string location5 = "Khajut, Pakistan";
            string phoneNumber5 = "03455138018";
            string serviceEmail5 = "lone@cartist1234567.com";
            string uploaderEmail5 = "uploader@lone1234567.com";

            // Get a profession somewhere from the middle of the dictionary
            IReadOnlyList<string> vehicleProfession;
            Service.GetProfessionsList().TryGetValue("Vehicle Services", out vehicleProfession);
            Assert.IsNotNull(vehicleProfession);
            string serviceProfessionType5 = vehicleProfession[1];
            string serviceEntityType5 = "Individual";
            var coordinatesFromAddress5 = geocodingService.GetCoordinatesFromAddress(location5);
            decimal latitude5 = coordinatesFromAddress5.Item1;
            decimal longitude5 = coordinatesFromAddress5.Item2;
            DateTime dateEstablished5 = DateTime.Now.AddYears(-5);
            Service service5 = new Service.ServiceBuilder().Name(name5).Description(description5)
                .Location(location5).PhoneNumber(phoneNumber5).ServiceEmail(serviceEmail5)
                .UploaderEmail(uploaderEmail5).ServiceProfessionType(serviceProfessionType5)
                .ServiceEntityType(serviceEntityType5)
                .Latitude(latitude5).Longitude(longitude5).DateEstablished(dateEstablished5).Build();
            // Save the Service
            servicesRepository.SaveOrUpdate(service5);

            // House # 6
            string name6 = "Salsalo";
            string description6 = "You have never tasted such delights before!";
            string location6 = "Rawalpindi, Pakistan";
            string phoneNumber6 = "03455138018";
            string serviceEmail6 = "food@salsalo1234567.com";
            string uploaderEmail6 = "uploader@osalsalo1234567.com";

            // Get a profession somewhere from the middle of the dictionary
            IReadOnlyList<string> vehicleProfession2;
            Service.GetProfessionsList().TryGetValue("Vehicle Services", out vehicleProfession2);
            Assert.IsNotNull(vehicleProfession2);
            string serviceProfessionType6 = vehicleProfession2[2];
            string serviceEntityType6 = "Organization";
            var coordinatesFromAddress6 = geocodingService.GetCoordinatesFromAddress(location6);
            decimal latitude6 = coordinatesFromAddress6.Item1;
            decimal longitude6 = coordinatesFromAddress6.Item2;
            DateTime dateEstablished6 = DateTime.Now.AddYears(-6);
            Service service6 = new Service.ServiceBuilder().Name(name6).Description(description6)
                .Location(location6).PhoneNumber(phoneNumber6).ServiceEmail(serviceEmail6)
                .UploaderEmail(uploaderEmail6).ServiceProfessionType(serviceProfessionType6)
                .ServiceEntityType(serviceEntityType6)
                .Latitude(latitude6).Longitude(longitude6).DateEstablished(dateEstablished6).Build();
            // Save the Service
            servicesRepository.SaveOrUpdate(service6);

            // Search by giving 'Rawalpindi, Pakistan' as the location. It should include all of the above 
            // saved houses except House # 5(Khajut, Pakistan), because it is more than 35 kilometers away 
            // from the location 'Rawalpindi, Pakistan'(who's zero point is near GPO Saddar)
            var searchCriteria = geocodingService.GetCoordinatesFromAddress("Rawalpindi, Pakistan");
            var retrievedServices = servicesRepository.GetServicesByLocation(
                                    searchCriteria.Item1, searchCriteria.Item2);
            Assert.IsNotNull(retrievedServices);
            Assert.AreEqual(5, retrievedServices.Count);

            // Verify House no 6
            var retrievedService = retrievedServices[0];
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name6, retrievedService.Name);
            Assert.AreEqual(description6, retrievedService.Description);
            Assert.AreEqual(location6, retrievedService.Location);
            Assert.AreEqual(phoneNumber6, retrievedService.MobileNumber);
            Assert.AreEqual(serviceEmail6, retrievedService.ServiceEmail);
            Assert.AreEqual(uploaderEmail6, retrievedService.UploaderEmail);
            Assert.AreEqual(serviceProfessionType6, retrievedService.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), serviceEntityType6),
                retrievedService.ServiceEntityType);
            AssertDateTime(dateEstablished6, retrievedService.DateEstablished);
            Assert.AreEqual(latitude6, retrievedService.Latitude);
            Assert.AreEqual(longitude6, retrievedService.Longitude);
            // Check the Reviews of House no 6
            Assert.IsNotNull(retrievedService.Reviews);
            Assert.AreEqual(0, retrievedService.Reviews.Count);

            // Verify House no 1
            retrievedService = retrievedServices[1];
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name, retrievedService.Name);
            Assert.AreEqual(description, retrievedService.Description);
            Assert.AreEqual(location, retrievedService.Location);
            Assert.AreEqual(phoneNumber, retrievedService.MobileNumber);
            Assert.AreEqual(serviceEmail, retrievedService.ServiceEmail);
            Assert.AreEqual(uploaderEmail, retrievedService.UploaderEmail);
            Assert.AreEqual(serviceProfessionType, retrievedService.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), serviceEntityType),
                retrievedService.ServiceEntityType);
            AssertDateTime(dateEstablished, retrievedService.DateEstablished);
            Assert.AreEqual(latitude, retrievedService.Latitude);
            Assert.AreEqual(longitude, retrievedService.Longitude);
            // Check the Reviews of House no 1
            Assert.IsNotNull(retrievedService.Reviews);
            Assert.AreEqual(1, retrievedService.Reviews.Count);
            Assert.AreEqual(reviewAuthorName, retrievedService.Reviews[0].AuthorName);
            Assert.AreEqual(reviewAuthorEmail, retrievedService.Reviews[0].AuthorEmail);
            Assert.AreEqual(reviewDescription, retrievedService.Reviews[0].ReviewDescription);
            Assert.AreEqual(service, retrievedService.Reviews[0].Service);

            // Verify House no 3
            retrievedService = retrievedServices[2];
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name3, retrievedService.Name);
            Assert.AreEqual(description3, retrievedService.Description);
            Assert.AreEqual(location3, retrievedService.Location);
            Assert.AreEqual(phoneNumber3, retrievedService.MobileNumber);
            Assert.AreEqual(serviceEmail3, retrievedService.ServiceEmail);
            Assert.AreEqual(uploaderEmail3, retrievedService.UploaderEmail);
            Assert.AreEqual(serviceProfessionType3, retrievedService.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), serviceEntityType3),
                retrievedService.ServiceEntityType);
            AssertDateTime(dateEstablished3, retrievedService.DateEstablished);
            Assert.AreEqual(latitude3, retrievedService.Latitude);
            Assert.AreEqual(longitude3, retrievedService.Longitude);
            // Check the Reviews of House no 3
            Assert.IsNotNull(retrievedService.Reviews);
            Assert.AreEqual(1, retrievedService.Reviews.Count);
            Assert.AreEqual(reviewAuthorName3, retrievedService.Reviews[0].AuthorName);
            Assert.AreEqual(reviewAuthorEmail3, retrievedService.Reviews[0].AuthorEmail);
            Assert.AreEqual(reviewDescription3, retrievedService.Reviews[0].ReviewDescription);
            Assert.AreEqual(service3, retrievedService.Reviews[0].Service);

            // Verify House no 4
            retrievedService = retrievedServices[3];
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name4, retrievedService.Name);
            Assert.AreEqual(description4, retrievedService.Description);
            Assert.AreEqual(location4, retrievedService.Location);
            Assert.AreEqual(phoneNumber4, retrievedService.MobileNumber);
            Assert.AreEqual(serviceEmail4, retrievedService.ServiceEmail);
            Assert.AreEqual(uploaderEmail4, retrievedService.UploaderEmail);
            Assert.AreEqual(serviceProfessionType4, retrievedService.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), serviceEntityType4),
                retrievedService.ServiceEntityType);
            AssertDateTime(dateEstablished4, retrievedService.DateEstablished);
            Assert.AreEqual(latitude4, retrievedService.Latitude);
            Assert.AreEqual(longitude4, retrievedService.Longitude);
            // Check the Reviews of House no 6
            Assert.IsNotNull(retrievedService.Reviews);
            Assert.AreEqual(0, retrievedService.Reviews.Count);

            // Verify House no 2
            retrievedService = retrievedServices[4];
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name2, retrievedService.Name);
            Assert.AreEqual(description2, retrievedService.Description);
            Assert.AreEqual(location2, retrievedService.Location);
            Assert.AreEqual(phoneNumber2, retrievedService.MobileNumber);
            Assert.AreEqual(serviceEmail2, retrievedService.ServiceEmail);
            Assert.AreEqual(uploaderEmail2, retrievedService.UploaderEmail);
            Assert.AreEqual(serviceProfessionType2, retrievedService.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), serviceEntityType2),
                retrievedService.ServiceEntityType);
            AssertDateTime(dateEstablished2, retrievedService.DateEstablished);
            Assert.AreEqual(latitude2, retrievedService.Latitude);
            Assert.AreEqual(longitude2, retrievedService.Longitude);
            // Check the Reviews of House no 2
            Assert.IsNotNull(retrievedService.Reviews);
            Assert.AreEqual(0, retrievedService.Reviews.Count);
        }

        [Test]
        public void SearchAllServicesTest_ChecksIfNoSearchCriteriaIsPrvidedThenAllServicesAreReturnedAsExpected_VerifiesByTheReturnedValue()
        {
            // Save 4 houses is distant cities. Search for all services should return all the houses
            // Get the ServiceRepository instance using Ninejct DI
            var servicesRepository = _kernel.Get<IServiceRepository>();

            // Get the GeocodingService from the Common module
            var geocodingService = _kernel.Get<IGeocodingService>();

            // House # 1
            string name = "The Stone Chopper";
            string description = "Our swords can chop stones. Easily!!!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string serviceEmail = "stone@chopper1234567.com";
            string uploaderEmail = "uploader@chopper1234567.com";
            string serviceProfessionType = "Carpenter";
            string serviceEntityType = "Organization";
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(location);
            decimal latitude = coordinatesFromAddress.Item1;
            decimal longitude = coordinatesFromAddress.Item2;
            DateTime dateEstablished = DateTime.Now.AddYears(-2);
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProfessionType)
                .ServiceEntityType(serviceEntityType)
                .Latitude(latitude).Longitude(longitude).DateEstablished(dateEstablished).Build();
            // Provide a review to save
            string reviewAuthorName = "King Arthur";
            string reviewAuthorEmail = "kingofthelingbling@kingdomcup12345678.com";
            string reviewDescription = "Off goes your head";
            service.AddReview(reviewAuthorName, reviewAuthorEmail, reviewDescription);
            // Save the Service
            servicesRepository.SaveOrUpdate(service);

            // House # 2
            string name2 = "The Lightning Bolt";
            string description2 = "Bolt, Electrify!";
            string location2 = "Chitral, Khyber Pakhtunkhwa, Pakistan";
            string phoneNumber2 = "03168948486";
            string serviceEmail2 = "bolt@chopper1234567.com";
            string uploaderEmail2 = "uploader@bolt1234567.com";
            string serviceProfessionType2 = "Electrician";
            string serviceEntityType2 = "Organization";
            var coordinatesFromAddress2 = geocodingService.GetCoordinatesFromAddress(location2);
            decimal latitude2 = coordinatesFromAddress2.Item1;
            decimal longitude2 = coordinatesFromAddress2.Item2;
            string facebookLink2 = "https://dummyfacebooklink-123456789-1.com";
            string instagramLink2 = "https://dummyinstagramlink-123456789-1.com";
            string twitterLink2 = "https://dummytwitterlink-123456789-1.com";
            string websiteLink2 = "https://dummywebsitelink-123456789-1.com";
            DateTime dateEstablished2 = DateTime.Now.AddYears(-2);
            Service service2 = new Service.ServiceBuilder().Name(name2).Description(description2)
                .Location(location2).PhoneNumber(phoneNumber2).ServiceEmail(serviceEmail2)
                .UploaderEmail(uploaderEmail2).ServiceProfessionType(serviceProfessionType2).ServiceEntityType(serviceEntityType2)
                .Latitude(latitude2).Longitude(longitude2).DateEstablished(dateEstablished2)
                .FacebookLink(facebookLink2).InstagramLink(instagramLink2).TwitterLink(twitterLink2)
                .WebsiteLink(websiteLink2).Build();
            // Save the Service
            servicesRepository.SaveOrUpdate(service2);

            // House # 3
            string name3 = "The Grass Hopper";
            string description3 = "We make choppers, so they can chop grass :D";
            string location3 = "Karachi, Sindh, Pakistan";
            string phoneNumber3 = "03168948486";
            string serviceEmail3 = "grass@hopper1234567.com";
            string uploaderEmail3 = "uploader@hopper1234567.com";
            string serviceProfessionType3 = "Plumber";
            string serviceEntityType3 = "Individual";
            var coordinatesFromAddress3 = geocodingService.GetCoordinatesFromAddress(location3);
            decimal latitude3 = coordinatesFromAddress3.Item1;
            decimal longitude3 = coordinatesFromAddress3.Item2;
            DateTime dateEstablished3 = DateTime.Now.AddYears(-3);
            string facebookLink3 = "https://dummyfacebooklink-123456789-1.com";
            string instagramLink3 = "https://dummyinstagramlink-123456789-1.com";
            string twitterLink3 = "https://dummytwitterlink-123456789-1.com";
            string websiteLink3 = "https://dummywebsitelink-123456789-1.com";
            Service service3 = new Service.ServiceBuilder().Name(name3).Description(description3)
                .Location(location3).PhoneNumber(phoneNumber3).ServiceEmail(serviceEmail3)
                .UploaderEmail(uploaderEmail3).ServiceProfessionType(serviceProfessionType3)
                .ServiceEntityType(serviceEntityType3)
                .Latitude(latitude3).Longitude(longitude3).DateEstablished(dateEstablished3)
                .FacebookLink(facebookLink3).InstagramLink(instagramLink3).TwitterLink(twitterLink3)
                .WebsiteLink(websiteLink3).Build();
            // Provide a review to save
            string reviewAuthorName3 = "King Arthur 3";
            string reviewAuthorEmail3 = "kingofthelingbling3@kingdomcup12345678.com";
            string reviewDescription3 = "Off goes your head 3 times";
            service3.AddReview(reviewAuthorName3, reviewAuthorEmail3, reviewDescription3);
            // Save the Service
            servicesRepository.SaveOrUpdate(service3);

            // House # 4
            string name4 = "The Onion Chopper";
            string description4 = "Meat, carbs and sauces. All combined in perfect proportions.";
            string location4 = "Lahore, Punjab, Pakistan";
            string phoneNumber4 = "03455138018";
            string serviceEmail4 = "onion@chopper1234567.com";
            string uploaderEmail4 = "uploader@onion1234567.com";
            string serviceProfessionType4 = "Electrician";
            string serviceEntityType4 = "Organization";
            var coordinatesFromAddress4 = geocodingService.GetCoordinatesFromAddress(location4);
            decimal latitude4 = coordinatesFromAddress4.Item1;
            decimal longitude4 = coordinatesFromAddress4.Item2;
            DateTime dateEstablished4 = DateTime.Now.AddYears(-4);
            Service service4 = new Service.ServiceBuilder().Name(name4).Description(description4)
                .Location(location4).PhoneNumber(phoneNumber4).ServiceEmail(serviceEmail4)
                .UploaderEmail(uploaderEmail4).ServiceProfessionType(serviceProfessionType4)
                .ServiceEntityType(serviceEntityType4)
                .Latitude(latitude4).Longitude(longitude4).DateEstablished(dateEstablished4).Build();
            // Save the Service
            servicesRepository.SaveOrUpdate(service4);

            var retrievedServices = servicesRepository.GetAllServices();
            Assert.IsNotNull(retrievedServices);
            Assert.AreEqual(4, retrievedServices.Count);
            
            // Verify House no 1
            var retrievedService = retrievedServices[0];
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name, retrievedService.Name);
            Assert.AreEqual(description, retrievedService.Description);
            Assert.AreEqual(location, retrievedService.Location);
            Assert.AreEqual(phoneNumber, retrievedService.MobileNumber);
            Assert.AreEqual(serviceEmail, retrievedService.ServiceEmail);
            Assert.AreEqual(uploaderEmail, retrievedService.UploaderEmail);
            Assert.AreEqual(serviceProfessionType, retrievedService.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), serviceEntityType),
                retrievedService.ServiceEntityType);
            AssertDateTime(dateEstablished, retrievedService.DateEstablished);
            Assert.AreEqual(latitude, retrievedService.Latitude);
            Assert.AreEqual(longitude, retrievedService.Longitude);
            // Check the Reviews of House no 1
            Assert.IsNotNull(retrievedService.Reviews);
            Assert.AreEqual(1, retrievedService.Reviews.Count);
            Assert.AreEqual(reviewAuthorName, retrievedService.Reviews[0].AuthorName);
            Assert.AreEqual(reviewAuthorEmail, retrievedService.Reviews[0].AuthorEmail);
            Assert.AreEqual(reviewDescription, retrievedService.Reviews[0].ReviewDescription);
            Assert.AreEqual(service, retrievedService.Reviews[0].Service);

            // Verify House no 2
            retrievedService = retrievedServices[1];
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name2, retrievedService.Name);
            Assert.AreEqual(description2, retrievedService.Description);
            Assert.AreEqual(location2, retrievedService.Location);
            Assert.AreEqual(phoneNumber2, retrievedService.MobileNumber);
            Assert.AreEqual(serviceEmail2, retrievedService.ServiceEmail);
            Assert.AreEqual(uploaderEmail2, retrievedService.UploaderEmail);
            Assert.AreEqual(serviceProfessionType2, retrievedService.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), serviceEntityType2),
                retrievedService.ServiceEntityType);
            AssertDateTime(dateEstablished2, retrievedService.DateEstablished);
            Assert.AreEqual(latitude2, retrievedService.Latitude);
            Assert.AreEqual(longitude2, retrievedService.Longitude);
            Assert.AreEqual(facebookLink2, retrievedService.FacebookLink);
            Assert.AreEqual(instagramLink2, retrievedService.InstagramLink);
            Assert.AreEqual(twitterLink2, retrievedService.TwitterLink);
            Assert.AreEqual(websiteLink2, retrievedService.WebsiteLink);
            // Check the Reviews of House no 2
            Assert.IsNotNull(retrievedService.Reviews);
            Assert.AreEqual(0, retrievedService.Reviews.Count);

            // Verify House no 3
            retrievedService = retrievedServices[2];
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name3, retrievedService.Name);
            Assert.AreEqual(description3, retrievedService.Description);
            Assert.AreEqual(location3, retrievedService.Location);
            Assert.AreEqual(phoneNumber3, retrievedService.MobileNumber);
            Assert.AreEqual(serviceEmail3, retrievedService.ServiceEmail);
            Assert.AreEqual(uploaderEmail3, retrievedService.UploaderEmail);
            Assert.AreEqual(serviceProfessionType3, retrievedService.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), serviceEntityType3),
                retrievedService.ServiceEntityType);
            AssertDateTime(dateEstablished3, retrievedService.DateEstablished);
            Assert.AreEqual(latitude3, retrievedService.Latitude);
            Assert.AreEqual(longitude3, retrievedService.Longitude);
            Assert.AreEqual(facebookLink3, retrievedService.FacebookLink);
            Assert.AreEqual(instagramLink3, retrievedService.InstagramLink);
            Assert.AreEqual(twitterLink3, retrievedService.TwitterLink);
            Assert.AreEqual(websiteLink3, retrievedService.WebsiteLink);
            // Check the Reviews of House no 3
            Assert.IsNotNull(retrievedService.Reviews);
            Assert.AreEqual(1, retrievedService.Reviews.Count);
            Assert.AreEqual(reviewAuthorName3, retrievedService.Reviews[0].AuthorName);
            Assert.AreEqual(reviewAuthorEmail3, retrievedService.Reviews[0].AuthorEmail);
            Assert.AreEqual(reviewDescription3, retrievedService.Reviews[0].ReviewDescription);
            Assert.AreEqual(service3, retrievedService.Reviews[0].Service);

            // Verify House no 4
            retrievedService = retrievedServices[3];
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name4, retrievedService.Name);
            Assert.AreEqual(description4, retrievedService.Description);
            Assert.AreEqual(location4, retrievedService.Location);
            Assert.AreEqual(phoneNumber4, retrievedService.MobileNumber);
            Assert.AreEqual(serviceEmail4, retrievedService.ServiceEmail);
            Assert.AreEqual(uploaderEmail4, retrievedService.UploaderEmail);
            Assert.AreEqual(serviceProfessionType4, retrievedService.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), serviceEntityType4),
                retrievedService.ServiceEntityType);
            AssertDateTime(dateEstablished4, retrievedService.DateEstablished);
            Assert.AreEqual(latitude4, retrievedService.Latitude);
            Assert.AreEqual(longitude4, retrievedService.Longitude);
            // Check the Reviews of House no 4
            Assert.IsNotNull(retrievedService.Reviews);
            Assert.AreEqual(0, retrievedService.Reviews.Count);
        }

        // Searches through both coordinates and Service profession type
        [Test]
        public void
            SearchHousesByCoordinatesAndServiceProfessionType_ChecksIfValueAreReturnedAsExpected_VerifiesByTheReturnedValue
            ()
        {
            // This test saves 6 services all within the reach of the provided location. So all are lcoated 
            // within the ssearch radius

            // Thecatch is that there are three professions used in the below Service saves, two Services
            // saved for each profession.
            // The test is supposed ot find 2 services with the corresponding given ServiceProfessionType and 
            // leave the rest
            
            // Get the ServiceRepository instance using Ninejct DI
            var servicesRepository = _kernel.Get<IServiceRepository>();

            // Get the GeocodingService from the Common module
            var geocodingService = _kernel.Get<IGeocodingService>();

            // Define the 3 ServiceProfessionTypes that we will use in this test
            string serviceProfessionType1 = Service.GetProfessionsList().First().Value.First();
            string serviceProfessionType2 = Service.GetProfessionsList().First().Value.Last();
            string serviceProfessionType3 = Service.GetProfessionsList().Last().Value.First();

            // House # 1 - serviceProfessionType1
            string name = "The Stone Chopper";
            string description = "Our swords can chop stones. Easily!!!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string serviceEmail = "stone@chopper1234567.com";
            string uploaderEmail = "uploader@chopper1234567.com";
            string serviceEntityType = "Organization";
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(location);
            decimal latitude = coordinatesFromAddress.Item1;
            decimal longitude = coordinatesFromAddress.Item2;
            DateTime dateEstablished = DateTime.Now.AddYears(-2);
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProfessionType1)
                .ServiceEntityType(serviceEntityType)
                .Latitude(latitude).Longitude(longitude).DateEstablished(dateEstablished).Build();
            // Provide a review to save
            string reviewAuthorName = "King Arthur";
            string reviewAuthorEmail = "kingofthelingbling@kingdomcup12345678.com";
            string reviewDescription = "Off goes your head";
            service.AddReview(reviewAuthorName, reviewAuthorEmail, reviewDescription);
            // Save the Service
            servicesRepository.SaveOrUpdate(service);

            // House # 2 - serviceProfessionType2
            string name2 = "The Lightning Bolt";
            string description2 = "Bolt, Electrify!";
            string location2 = "Islamabad, Pakistan";
            string phoneNumber2 = "03168948486";
            string serviceEmail2 = "bolt@chopper1234567.com";
            string uploaderEmail2 = "uploader@bolt1234567.com";
            string serviceEntityType2 = "Organization";
            var coordinatesFromAddress2 = geocodingService.GetCoordinatesFromAddress(location2);
            decimal latitude2 = coordinatesFromAddress2.Item1;
            decimal longitude2 = coordinatesFromAddress2.Item2;
            DateTime dateEstablished2 = DateTime.Now.AddYears(-2);
            Service service2 = new Service.ServiceBuilder().Name(name2).Description(description2)
                .Location(location2).PhoneNumber(phoneNumber2).ServiceEmail(serviceEmail2)
                .UploaderEmail(uploaderEmail2).ServiceProfessionType(serviceProfessionType2)
                .ServiceEntityType(serviceEntityType2)
                .Latitude(latitude2).Longitude(longitude2).DateEstablished(dateEstablished2).Build();
            // Save the Service
            servicesRepository.SaveOrUpdate(service2);

            // House # 3 - serviceProfessionType2
            string name3 = "The Grass Hopper";
            string description3 = "We make choppers, so they can chop grass :D";
            string location3 = "Rawalpindi, Pakistan";
            string phoneNumber3 = "03168948486";
            string serviceEmail3 = "grass@hopper1234567.com";
            string uploaderEmail3 = "uploader@hopper1234567.com";
            string serviceEntityType3 = "Individual";
            var coordinatesFromAddress3 = geocodingService.GetCoordinatesFromAddress(location3);
            decimal latitude3 = coordinatesFromAddress3.Item1;
            decimal longitude3 = coordinatesFromAddress3.Item2;
            DateTime dateEstablished3 = DateTime.Now.AddYears(-3);
            Service service3 = new Service.ServiceBuilder().Name(name3).Description(description3)
                .Location(location3).PhoneNumber(phoneNumber3).ServiceEmail(serviceEmail3)
                .UploaderEmail(uploaderEmail3).ServiceProfessionType(serviceProfessionType2)
                .ServiceEntityType(serviceEntityType3)
                .Latitude(latitude3).Longitude(longitude3).DateEstablished(dateEstablished3).Build();
            // Save the Service
            servicesRepository.SaveOrUpdate(service3);

            // House # 4 - serviceProfessionType3
            string name4 = "The Onion Chopper";
            string description4 = "Meat, carbs and sauces. All combined in perfect proportions.";
            string location4 = "E-11, Islamabad, Islamabad Capital Territory, Pakistan";
            string phoneNumber4 = "03455138018";
            string serviceEmail4 = "onion@chopper1234567.com";
            string uploaderEmail4 = "uploader@onion1234567.com";
            string serviceEntityType4 = "Organization";
            var coordinatesFromAddress4 = geocodingService.GetCoordinatesFromAddress(location4);
            decimal latitude4 = coordinatesFromAddress4.Item1;
            decimal longitude4 = coordinatesFromAddress4.Item2;
            DateTime dateEstablished4 = DateTime.Now.AddYears(-4);
            Service service4 = new Service.ServiceBuilder().Name(name4).Description(description4)
                .Location(location4).PhoneNumber(phoneNumber4).ServiceEmail(serviceEmail4)
                .UploaderEmail(uploaderEmail4).ServiceProfessionType(serviceProfessionType3)
                .ServiceEntityType(serviceEntityType4)
                .Latitude(latitude4).Longitude(longitude4).DateEstablished(dateEstablished4).Build();
            // Provide a review to save
            string reviewAuthorName4 = "King Arthur 4";
            string reviewAuthorEmail4 = "kingofthelingbling4@kingdomcup12345678.com";
            string reviewDescription4 = "Off goes your head 4 times";
            service4.AddReview(reviewAuthorName4, reviewAuthorEmail4, reviewDescription4);
            // Save the Service
            servicesRepository.SaveOrUpdate(service4);

            // House # 5 - serviceProfessionType1
            string name5 = "The Lone Artist";
            string description5 = "Whatever colors you need will be painted. But by my imagination.";
            string location5 = "Saddar, Rawalpindi, Pakistan";
            string phoneNumber5 = "03455138018";
            string serviceEmail5 = "lone@cartist1234567.com";
            string uploaderEmail5 = "uploader@lone1234567.com";
            
            string serviceEntityType5 = "Individual";
            var coordinatesFromAddress5 = geocodingService.GetCoordinatesFromAddress(location5);
            decimal latitude5 = coordinatesFromAddress5.Item1;
            decimal longitude5 = coordinatesFromAddress5.Item2;
            DateTime dateEstablished5 = DateTime.Now.AddYears(-5);
            Service service5 = new Service.ServiceBuilder().Name(name5).Description(description5)
                .Location(location5).PhoneNumber(phoneNumber5).ServiceEmail(serviceEmail5)
                .UploaderEmail(uploaderEmail5).ServiceProfessionType(serviceProfessionType1)
                .ServiceEntityType(serviceEntityType5)
                .Latitude(latitude5).Longitude(longitude5).DateEstablished(dateEstablished5).Build();
            // Save the Service
            servicesRepository.SaveOrUpdate(service5);

            // House # 6 - serviceProfessionType3
            string name6 = "Salsalo";
            string description6 = "You have never tasted such delights before!";
            string location6 = "Bahria Town, Rawalpindi, Punjab, Pakistan";
            string phoneNumber6 = "03455138018";
            string serviceEmail6 = "food@salsalo1234567.com";
            string uploaderEmail6 = "uploader@osalsalo1234567.com";

            // Get a profession somewhere from the middle of the dictionary
            string serviceEntityType6 = "Organization";
            var coordinatesFromAddress6 = geocodingService.GetCoordinatesFromAddress(location6);
            decimal latitude6 = coordinatesFromAddress6.Item1;
            decimal longitude6 = coordinatesFromAddress6.Item2;
            DateTime dateEstablished6 = DateTime.Now.AddYears(-6);
            Service service6 = new Service.ServiceBuilder().Name(name6).Description(description6)
                .Location(location6).PhoneNumber(phoneNumber6).ServiceEmail(serviceEmail6)
                .UploaderEmail(uploaderEmail6).ServiceProfessionType(serviceProfessionType3)
                .ServiceEntityType(serviceEntityType6)
                .Latitude(latitude6).Longitude(longitude6).DateEstablished(dateEstablished6).Build();
            // Save the Service
            servicesRepository.SaveOrUpdate(service6);

            // Search by giving 'Rawalpindi, Pakistan' as the location and serviceProfession3 as the search
            // criteria. As a result, House no. 4 and House no. 6 should be retrieved
            var searchCriteria = geocodingService.GetCoordinatesFromAddress("Satellite Town, Rawalpindi, Pakistan");
            var retrievedServices = servicesRepository.GetServicesByLocationAndProfession(
                                    searchCriteria.Item1, searchCriteria.Item2, serviceProfessionType3);
            Assert.IsNotNull(retrievedServices);
            Assert.AreEqual(2, retrievedServices.Count);

            // Verify House no 4
            var retrievedService = retrievedServices[0];
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name4, retrievedService.Name);
            Assert.AreEqual(description4, retrievedService.Description);
            Assert.AreEqual(location4, retrievedService.Location);
            Assert.AreEqual(phoneNumber4, retrievedService.MobileNumber);
            Assert.AreEqual(serviceEmail4, retrievedService.ServiceEmail);
            Assert.AreEqual(uploaderEmail4, retrievedService.UploaderEmail);
            Assert.AreEqual(serviceProfessionType3, retrievedService.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), serviceEntityType4),
                retrievedService.ServiceEntityType);
            AssertDateTime(dateEstablished4, retrievedService.DateEstablished);
            Assert.AreEqual(latitude4, retrievedService.Latitude);
            Assert.AreEqual(longitude4, retrievedService.Longitude);
            // Check the Reviews of House no 4
            Assert.IsNotNull(retrievedService.Reviews);
            Assert.AreEqual(1, retrievedService.Reviews.Count);
            Assert.AreEqual(reviewAuthorName4, retrievedService.Reviews[0].AuthorName);
            Assert.AreEqual(reviewAuthorEmail4, retrievedService.Reviews[0].AuthorEmail);
            Assert.AreEqual(reviewDescription4, retrievedService.Reviews[0].ReviewDescription);
            Assert.AreEqual(service4, retrievedService.Reviews[0].Service);

            // Verify House no 6
            retrievedService = retrievedServices[1];
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name6, retrievedService.Name);
            Assert.AreEqual(description6, retrievedService.Description);
            Assert.AreEqual(location6, retrievedService.Location);
            Assert.AreEqual(phoneNumber6, retrievedService.MobileNumber);
            Assert.AreEqual(serviceEmail6, retrievedService.ServiceEmail);
            Assert.AreEqual(uploaderEmail6, retrievedService.UploaderEmail);
            Assert.AreEqual(serviceProfessionType3, retrievedService.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), serviceEntityType6),
                retrievedService.ServiceEntityType);
            AssertDateTime(dateEstablished6, retrievedService.DateEstablished);
            Assert.AreEqual(latitude6, retrievedService.Latitude);
            Assert.AreEqual(longitude6, retrievedService.Longitude);
            // Check the Reviews of House no 6
            Assert.IsNotNull(retrievedService.Reviews);
            Assert.AreEqual(0, retrievedService.Reviews.Count);
        }

        // Searches through both coordinates and Service profession type
        [Test]
        public void SearchHousesByServiceProfessionTypeOnly_ChecksIfSearchByProfessionReturnsValuesAsExpected_VerifiesByTheReturnedValues()
        {
            // This test saves 6 locations and 3 ServiceProfessionTypes, each ServiceProfessionType employs
            // 2 services. The ServiceProfessionType that is searched should return the 2 corresponding
            // services saved with that Profession and should ignore their location completely

            // Get the ServiceRepository instance using Ninejct DI
            var servicesRepository = _kernel.Get<IServiceRepository>();

            // Get the GeocodingService from the Common module
            var geocodingService = _kernel.Get<IGeocodingService>();

            // Define the 3 ServiceProfessionTypes that we will use in this test
            string serviceProfessionType1 = Service.GetProfessionsList().First().Value.First();
            string serviceProfessionType2 = Service.GetProfessionsList().First().Value.Last();
            string serviceProfessionType3 = Service.GetProfessionsList().Last().Value.First();

            // Service # 1 - serviceProfessionType1
            string name = "The Stone Chopper";
            string description = "Our swords can chop stones. Easily!!!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string serviceEmail = "stone@chopper1234567.com";
            string uploaderEmail = "uploader@chopper1234567.com";
            string serviceEntityType = "Organization";
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(location);
            decimal latitude = coordinatesFromAddress.Item1;
            decimal longitude = coordinatesFromAddress.Item2;
            DateTime dateEstablished = DateTime.Now.AddYears(-2);
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProfessionType1)
                .ServiceEntityType(serviceEntityType)
                .Latitude(latitude).Longitude(longitude).DateEstablished(dateEstablished).Build();
            // Provide a review to save
            string reviewAuthorName = "King Arthur";
            string reviewAuthorEmail = "kingofthelingbling@kingdomcup12345678.com";
            string reviewDescription = "Off goes your head";
            service.AddReview(reviewAuthorName, reviewAuthorEmail, reviewDescription);
            // Save the Service
            servicesRepository.SaveOrUpdate(service);

            // Service # 2 - serviceProfessionType2
            string name2 = "The Lightning Bolt";
            string description2 = "Bolt, Electrify!";
            string location2 = "Islamabad, Pakistan";
            string phoneNumber2 = "03168948486";
            string serviceEmail2 = "bolt@chopper1234567.com";
            string uploaderEmail2 = "uploader@bolt1234567.com";
            string serviceEntityType2 = "Organization";
            var coordinatesFromAddress2 = geocodingService.GetCoordinatesFromAddress(location2);
            decimal latitude2 = coordinatesFromAddress2.Item1;
            decimal longitude2 = coordinatesFromAddress2.Item2;
            DateTime dateEstablished2 = DateTime.Now.AddYears(-2);
            Service service2 = new Service.ServiceBuilder().Name(name2).Description(description2)
                .Location(location2).PhoneNumber(phoneNumber2).ServiceEmail(serviceEmail2)
                .UploaderEmail(uploaderEmail2).ServiceProfessionType(serviceProfessionType2)
                .ServiceEntityType(serviceEntityType2)
                .Latitude(latitude2).Longitude(longitude2).DateEstablished(dateEstablished2).Build();
            // Provide a review to save
            string reviewAuthorName2 = "King Arthur 2";
            string reviewAuthorEmail2 = "kingofthelingbling2@kingdomcup12345678.com";
            string reviewDescription2 = "Off goes your head 2 times";
            service2.AddReview(reviewAuthorName2, reviewAuthorEmail2, reviewDescription2);
            servicesRepository.SaveOrUpdate(service2);

            // Service # 3 - serviceProfessionType2
            string name3 = "The Grass Hopper";
            string description3 = "We make choppers, so they can chop grass :D";
            string location3 = "Rawalpindi, Pakistan";
            string phoneNumber3 = "03168948486";
            string serviceEmail3 = "grass@hopper1234567.com";
            string uploaderEmail3 = "uploader@hopper1234567.com";
            string serviceEntityType3 = "Individual";
            var coordinatesFromAddress3 = geocodingService.GetCoordinatesFromAddress(location3);
            decimal latitude3 = coordinatesFromAddress3.Item1;
            decimal longitude3 = coordinatesFromAddress3.Item2;
            DateTime dateEstablished3 = DateTime.Now.AddYears(-3);
            Service service3 = new Service.ServiceBuilder().Name(name3).Description(description3)
                .Location(location3).PhoneNumber(phoneNumber3).ServiceEmail(serviceEmail3)
                .UploaderEmail(uploaderEmail3).ServiceProfessionType(serviceProfessionType2)
                .ServiceEntityType(serviceEntityType3)
                .Latitude(latitude3).Longitude(longitude3).DateEstablished(dateEstablished3).Build();
            // Save the Service
            servicesRepository.SaveOrUpdate(service3);

            // Service # 4 - serviceProfessionType3
            string name4 = "The Onion Chopper";
            string description4 = "Meat, carbs and sauces. All combined in perfect proportions.";
            string location4 = "E-11, Islamabad, Islamabad Capital Territory, Pakistan";
            string phoneNumber4 = "03455138018";
            string serviceEmail4 = "onion@chopper1234567.com";
            string uploaderEmail4 = "uploader@onion1234567.com";
            string serviceEntityType4 = "Organization";
            var coordinatesFromAddress4 = geocodingService.GetCoordinatesFromAddress(location4);
            decimal latitude4 = coordinatesFromAddress4.Item1;
            decimal longitude4 = coordinatesFromAddress4.Item2;
            DateTime dateEstablished4 = DateTime.Now.AddYears(-4);
            Service service4 = new Service.ServiceBuilder().Name(name4).Description(description4)
                .Location(location4).PhoneNumber(phoneNumber4).ServiceEmail(serviceEmail4)
                .UploaderEmail(uploaderEmail4).ServiceProfessionType(serviceProfessionType3)
                .ServiceEntityType(serviceEntityType4)
                .Latitude(latitude4).Longitude(longitude4).DateEstablished(dateEstablished4).Build();
            // Provide a review to save
            string reviewAuthorName4 = "King Arthur 4";
            string reviewAuthorEmail4 = "kingofthelingbling4@kingdomcup12345678.com";
            string reviewDescription4 = "Off goes your head 4 times";
            service4.AddReview(reviewAuthorName4, reviewAuthorEmail4, reviewDescription4);
            // Save the Service
            servicesRepository.SaveOrUpdate(service4);

            // Service # 5 - serviceProfessionType1
            string name5 = "The Lone Artist";
            string description5 = "Whatever colors you need will be painted. But by my imagination.";
            string location5 = "Saddar, Rawalpindi, Pakistan";
            string phoneNumber5 = "03455138018";
            string serviceEmail5 = "lone@cartist1234567.com";
            string uploaderEmail5 = "uploader@lone1234567.com";

            string serviceEntityType5 = "Individual";
            var coordinatesFromAddress5 = geocodingService.GetCoordinatesFromAddress(location5);
            decimal latitude5 = coordinatesFromAddress5.Item1;
            decimal longitude5 = coordinatesFromAddress5.Item2;
            DateTime dateEstablished5 = DateTime.Now.AddYears(-5);
            Service service5 = new Service.ServiceBuilder().Name(name5).Description(description5)
                .Location(location5).PhoneNumber(phoneNumber5).ServiceEmail(serviceEmail5)
                .UploaderEmail(uploaderEmail5).ServiceProfessionType(serviceProfessionType1)
                .ServiceEntityType(serviceEntityType5)
                .Latitude(latitude5).Longitude(longitude5).DateEstablished(dateEstablished5).Build();
            // Save the Service
            servicesRepository.SaveOrUpdate(service5);

            // Service # 6 - serviceProfessionType3
            string name6 = "Salsalo";
            string description6 = "You have never tasted such delights before!";
            string location6 = "Bahria Town, Rawalpindi, Punjab, Pakistan";
            string phoneNumber6 = "03455138018";
            string serviceEmail6 = "food@salsalo1234567.com";
            string uploaderEmail6 = "uploader@osalsalo1234567.com";

            // Get a profession somewhere from the middle of the dictionary
            string serviceEntityType6 = "Organization";
            var coordinatesFromAddress6 = geocodingService.GetCoordinatesFromAddress(location6);
            decimal latitude6 = coordinatesFromAddress6.Item1;
            decimal longitude6 = coordinatesFromAddress6.Item2;
            DateTime dateEstablished6 = DateTime.Now.AddYears(-6);
            Service service6 = new Service.ServiceBuilder().Name(name6).Description(description6)
                .Location(location6).PhoneNumber(phoneNumber6).ServiceEmail(serviceEmail6)
                .UploaderEmail(uploaderEmail6).ServiceProfessionType(serviceProfessionType3)
                .ServiceEntityType(serviceEntityType6)
                .Latitude(latitude6).Longitude(longitude6).DateEstablished(dateEstablished6).Build();
            // Save the Service
            servicesRepository.SaveOrUpdate(service6);

            // House # 2 and house # 3 shoulf be retrieved as they are saved with serviceProfesstionType2
            var retrievedServices = servicesRepository.GetServicesByProfession(serviceProfessionType2);
            Assert.IsNotNull(retrievedServices);
            Assert.AreEqual(2, retrievedServices.Count);
            
            Service retrievedService;
            Service retrievedService2;
            // We dont know in what order wil the services be retrieved, a in this case we are searching only
            // using the profession tpye and not the location. So we will check in which order the services 
            // are retrieved and then assert them correspondingly.
            if (retrievedServices[0].Name.Equals(name2))
            {
                retrievedService = retrievedServices[0];
                retrievedService2 = retrievedServices[1];
            }
            else
            {
                retrievedService2 = retrievedServices[0];
                retrievedService = retrievedServices[1];
            }

            // Verify House # 2
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name2, retrievedService.Name);
            Assert.AreEqual(description2, retrievedService.Description);
            Assert.AreEqual(location2, retrievedService.Location);
            Assert.AreEqual(phoneNumber2, retrievedService.MobileNumber);
            Assert.AreEqual(serviceEmail2, retrievedService.ServiceEmail);
            Assert.AreEqual(uploaderEmail2, retrievedService.UploaderEmail);
            Assert.AreEqual(serviceProfessionType2, retrievedService.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), serviceEntityType2),
                retrievedService.ServiceEntityType);
            AssertDateTime(dateEstablished2, retrievedService.DateEstablished);
            Assert.AreEqual(latitude2, retrievedService.Latitude);
            Assert.AreEqual(longitude2, retrievedService.Longitude);
            // Check the Reviews of House no 2
            Assert.IsNotNull(retrievedService.Reviews);
            Assert.AreEqual(1, retrievedService.Reviews.Count);
            Assert.AreEqual(reviewAuthorName2, retrievedService.Reviews[0].AuthorName);
            Assert.AreEqual(reviewAuthorEmail2, retrievedService.Reviews[0].AuthorEmail);
            Assert.AreEqual(reviewDescription2, retrievedService.Reviews[0].ReviewDescription);
            Assert.AreEqual(service2, retrievedService.Reviews[0].Service);

            // Verify House # 3
            Assert.IsNotNull(retrievedService2);
            Assert.AreEqual(name3, retrievedService2.Name);
            Assert.AreEqual(description3, retrievedService2.Description);
            Assert.AreEqual(location3, retrievedService2.Location);
            Assert.AreEqual(phoneNumber3, retrievedService2.MobileNumber);
            Assert.AreEqual(serviceEmail3, retrievedService2.ServiceEmail);
            Assert.AreEqual(uploaderEmail3, retrievedService2.UploaderEmail);
            Assert.AreEqual(serviceProfessionType2, retrievedService2.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), serviceEntityType3),
                retrievedService2.ServiceEntityType);
            AssertDateTime(dateEstablished3, retrievedService2.DateEstablished);
            Assert.AreEqual(latitude3, retrievedService2.Latitude);
            Assert.AreEqual(longitude3, retrievedService2.Longitude);
            // Check the Reviews of House no 3
            Assert.IsNotNull(retrievedService2.Reviews);
            Assert.AreEqual(0, retrievedService2.Reviews.Count);
        }

        // Searches by providing the uplaoder's email
        [Test]
        public void SearchHousesByUploaderEmailTest_ChecksIfSearchByEmailReturnsValuesAsExpected_VerifiesByTheReturnedValues()
        {
            // Saves 3 houses, 2 with same email that is used for searching, and 1 with another one.
            // The 2 houses should be retrieved
            // Get the ServiceRepository instance using Ninejct DI
            var servicesRepository = _kernel.Get<IServiceRepository>();

            // Get the GeocodingService from the Common module
            var geocodingService = _kernel.Get<IGeocodingService>();

            string mainUploaderEmail = "uploader@chopper1234567.com";
            
            // Service # 1 - Saved with MainUploaderEmail
            string name = "The Stone Chopper";
            string description = "Our swords can chop stones. Easily!!!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string serviceEmail = "stone@chopper1234567.com";
            string serviceEntityType = "Organization";
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(location);
            decimal latitude = coordinatesFromAddress.Item1;
            decimal longitude = coordinatesFromAddress.Item2;
            DateTime dateEstablished = DateTime.Now.AddYears(-2);
            string serviceProfessionType = "Carpenter";
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(mainUploaderEmail).ServiceProfessionType(serviceProfessionType)
                .ServiceEntityType(serviceEntityType)
                .Latitude(latitude).Longitude(longitude).DateEstablished(dateEstablished).Build();
            // Provide a review to save
            string reviewAuthorName = "King Arthur";
            string reviewAuthorEmail = "kingofthelingbling@kingdomcup12345678.com";
            string reviewDescription = "Off goes your head";
            service.AddReview(reviewAuthorName, reviewAuthorEmail, reviewDescription);
            // Save the Service
            servicesRepository.SaveOrUpdate(service);

            // Service # 2 - Saved with MainUploaderEmail
            string name2 = "The Lightning Bolt";
            string description2 = "Bolt, Electrify!";
            string location2 = "Islamabad, Pakistan";
            string phoneNumber2 = "03168948486";
            string serviceEmail2 = "bolt@chopper1234567.com";
            string serviceEntityType2 = "Organization";
            string serviceProfessionType2 = "Electrician";
            var coordinatesFromAddress2 = geocodingService.GetCoordinatesFromAddress(location2);
            decimal latitude2 = coordinatesFromAddress2.Item1;
            decimal longitude2 = coordinatesFromAddress2.Item2;
            DateTime dateEstablished2 = DateTime.Now.AddYears(-2);
            Service service2 = new Service.ServiceBuilder().Name(name2).Description(description2)
                .Location(location2).PhoneNumber(phoneNumber2).ServiceEmail(serviceEmail2)
                .UploaderEmail(mainUploaderEmail).ServiceProfessionType(serviceProfessionType2)
                .ServiceEntityType(serviceEntityType2)
                .Latitude(latitude2).Longitude(longitude2).DateEstablished(dateEstablished2).Build();
            // Provide a review to save
            string reviewAuthorName2 = "King Arthur 2";
            string reviewAuthorEmail2 = "kingofthelingbling2@kingdomcup12345678.com";
            string reviewDescription2 = "Off goes your head 2 times";
            service2.AddReview(reviewAuthorName2, reviewAuthorEmail2, reviewDescription2);
            servicesRepository.SaveOrUpdate(service2);

            // Service # 3 - Saved with an email thaat won't be searched
            string name3 = "The Grass Hopper";
            string description3 = "We make choppers, so they can chop grass :D";
            string location3 = "Rawalpindi, Pakistan";
            string phoneNumber3 = "03168948486";
            string serviceEmail3 = "grass@hopper1234567.com";
            string uploaderEmail3 = "uploader@hopper1234567.com";
            string serviceEntityType3 = "Individual";
            string serviceProfessionType3 = "Carpenter";
            var coordinatesFromAddress3 = geocodingService.GetCoordinatesFromAddress(location3);
            decimal latitude3 = coordinatesFromAddress3.Item1;
            decimal longitude3 = coordinatesFromAddress3.Item2;
            DateTime dateEstablished3 = DateTime.Now.AddYears(-3);
            Service service3 = new Service.ServiceBuilder().Name(name3).Description(description3)
                .Location(location3).PhoneNumber(phoneNumber3).ServiceEmail(serviceEmail3)
                .UploaderEmail(uploaderEmail3).ServiceProfessionType(serviceProfessionType3)
                .ServiceEntityType(serviceEntityType3)
                .Latitude(latitude3).Longitude(longitude3).DateEstablished(dateEstablished3).Build();
            // Save the Service
            servicesRepository.SaveOrUpdate(service3);
            
            // House # 1 and house # 2 should be retrieved as they are saved with the main uploader email
            var retrievedServices = servicesRepository.GetServicesByEmail(mainUploaderEmail);
            Assert.IsNotNull(retrievedServices);
            Assert.AreEqual(2, retrievedServices.Count);

            // Verify Service # 1
            Service retrievedService = retrievedServices[0];
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name, retrievedService.Name);
            Assert.AreEqual(description, retrievedService.Description);
            Assert.AreEqual(location, retrievedService.Location);
            Assert.AreEqual(phoneNumber, retrievedService.MobileNumber);
            Assert.AreEqual(serviceEmail, retrievedService.ServiceEmail);
            Assert.AreEqual(mainUploaderEmail, retrievedService.UploaderEmail);
            Assert.AreEqual(serviceProfessionType, retrievedService.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), serviceEntityType),
                retrievedService.ServiceEntityType);
            AssertDateTime(dateEstablished, retrievedService.DateEstablished);
            Assert.AreEqual(latitude, retrievedService.Latitude);
            Assert.AreEqual(longitude, retrievedService.Longitude);
            // Check the Reviews of Service no 1
            Assert.IsNotNull(retrievedService.Reviews);
            Assert.AreEqual(1, retrievedService.Reviews.Count);
            Assert.AreEqual(reviewAuthorName, retrievedService.Reviews[0].AuthorName);
            Assert.AreEqual(reviewAuthorEmail, retrievedService.Reviews[0].AuthorEmail);
            Assert.AreEqual(reviewDescription, retrievedService.Reviews[0].ReviewDescription);
            Assert.AreEqual(service, retrievedService.Reviews[0].Service);

            // Verify Service # 2
            Service retrievedService2 = retrievedServices[1];
            Assert.IsNotNull(retrievedService2);
            Assert.AreEqual(name2, retrievedService2.Name);
            Assert.AreEqual(description2, retrievedService2.Description);
            Assert.AreEqual(location2, retrievedService2.Location);
            Assert.AreEqual(phoneNumber2, retrievedService2.MobileNumber);
            Assert.AreEqual(serviceEmail2, retrievedService2.ServiceEmail);
            Assert.AreEqual(mainUploaderEmail, retrievedService2.UploaderEmail);
            Assert.AreEqual(serviceProfessionType2, retrievedService2.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), serviceEntityType2),
                retrievedService2.ServiceEntityType);
            AssertDateTime(dateEstablished2, retrievedService2.DateEstablished);
            Assert.AreEqual(latitude2, retrievedService2.Latitude);
            Assert.AreEqual(longitude2, retrievedService2.Longitude);
            // Check the Reviews of Service no 2
            Assert.IsNotNull(retrievedService2.Reviews);
            Assert.AreEqual(1, retrievedService2.Reviews.Count);
            Assert.AreEqual(reviewAuthorName2, retrievedService2.Reviews[0].AuthorName);
            Assert.AreEqual(reviewAuthorEmail2, retrievedService2.Reviews[0].AuthorEmail);
            Assert.AreEqual(reviewDescription2, retrievedService2.Reviews[0].ReviewDescription);
            Assert.AreEqual(service2, retrievedService2.Reviews[0].Service);
        }

        [Test]
        public void
            DeleteServicetest_ChecksIfAnInstanceisDeletedFromTheDatabaseSuccessfully_VerifiesThroughTheValueNotReturned()
        {
            // Get the ServiceRepository instance using Ninejct DI
            var servicesRepository = _kernel.Get<IServiceRepository>();

            // Get the GeocodingService from the Common module
            var geocodingService = _kernel.Get<IGeocodingService>();

            // House # 1
            string name = "The Stone Chopper";
            string description = "Our swords can chop stones. Easily!!!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string serviceEmail = "stone@chopper1234567.com";
            string uploaderEmail = "uploader@chopper1234567.com";
            string serviceProfessionType = Service.GetProfessionsList().First().Value.First();
            string serviceEntityType = "Organization";
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(location);
            decimal latitude = coordinatesFromAddress.Item1;
            decimal longitude = coordinatesFromAddress.Item2;
            DateTime dateEstablished = DateTime.Now.AddYears(-2);
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProfessionType)
                .ServiceEntityType(serviceEntityType)
                .Latitude(latitude).Longitude(longitude).DateEstablished(dateEstablished).Build();
            // Provide a review to save
            string reviewAuthorName = "King Arthur";
            string reviewAuthorEmail = "kingofthelingbling@kingdomcup12345678.com";
            string reviewDescription = "Off goes your head";
            service.AddReview(reviewAuthorName, reviewAuthorEmail, reviewDescription);
            // Add some images to this service
            string image1Id = Guid.NewGuid().ToString();
            string image2Id = Guid.NewGuid().ToString();
            string image3Id = Guid.NewGuid().ToString();
            service.AddImage(image1Id);
            service.AddImage(image2Id);
            service.AddImage(image3Id);
            // Save the Service
            servicesRepository.SaveOrUpdate(service);

            // Retrieve Service to verify it exists
            var retrievedService = servicesRepository.GetServiceById(service.Id);
            Assert.NotNull(retrievedService);

            // Delete service
            servicesRepository.DeleteService(retrievedService);

            // Query the service again
            retrievedService = servicesRepository.GetServiceById(service.Id);
            // This time it should not be present
            Assert.IsNull(retrievedService);
        }

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
                actualDateTime = actualDateTime.Value.AddTicks(-actualDateTime.Value.Ticks%TimeSpan.TicksPerSecond);
            }
            if (expectedDateTime != null)
            {
                expectedDateTime = expectedDateTime.Value.AddTicks(-expectedDateTime.Value.Ticks%TimeSpan.TicksPerSecond);
            }
            Assert.AreEqual(expectedDateTime, actualDateTime);
        }
    }
}
