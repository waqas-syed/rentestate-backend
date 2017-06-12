using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Ninject;
using NUnit.Framework;
using RentStuff.Common;
using RentStuff.Common.NinjectModules;
using RentStuff.Common.Services.LocationServices;
using RentStuff.Services.Domain.Model.ServiceAggregate;
using RentStuff.Services.Infrastructure.Persistence.NinjectModules;

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
            _kernel.Load<ServicesPersistenceNinjectModule>();
            _kernel.Load<CommonNinjectModule>();
        }

        [TearDown]
        public void Teardown()
        {
            _databaseUtility.Create();
        }

        [Test]
        public void ServiceSaveAndRetrieveTest_ChecksIfTheServiceInstanceIsSavedAsExpected_VerifiesByTheReturnedValue()
        {
            // Get the ServiceRepository instance using Ninejct DI
            var servicesRepository = _kernel.Get<IServicesRepository>();

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
            DateTime dateEstablished = DateTime.Now;
            decimal latitude = 33.7M;
            decimal longitude = 73.1M;
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProfessionType)
                .ServiceEntityType(entity).DateEstablished(dateEstablished)
                .Latitude(latitude).Longitude(longitude).Build();

            // Provide a review to save
            string authorName = "King Arthur";
            string authorEmail = "kingofthelingbling@kingdomcup12345678.com";
            string reviewDescription = "Off goes your head";
            service.AddReview(authorName, authorEmail, reviewDescription);
            
            servicesRepository.SaveOrUpdate(service);

            // Retrieve the result
            var retrievedService = servicesRepository.GetServiceById(service.Id);
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name, retrievedService.Name);
            Assert.AreEqual(description, retrievedService.Description);
            Assert.AreEqual(location, retrievedService.Location);
            Assert.AreEqual(phoneNumber, retrievedService.PhoneNumber);
            Assert.AreEqual(serviceEmail, retrievedService.ServiceEmail);
            Assert.AreEqual(uploaderEmail, retrievedService.UploaderEmail);
            Assert.AreEqual(serviceProfessionType, retrievedService.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), entity),
                retrievedService.ServiceEntityType);
            AssertDateTime(dateEstablished, retrievedService.DateEstablished);
            Assert.AreEqual(latitude, retrievedService.Latitude);
            Assert.AreEqual(longitude, retrievedService.Longitude);

            // Check the Reviews
            Assert.IsNotNull(retrievedService.Reviews);
            Assert.AreEqual(1, retrievedService.Reviews.Count);
            Assert.AreEqual(authorName, retrievedService.Reviews[0].Authorname);
            Assert.AreEqual(authorEmail, retrievedService.Reviews[0].AuthorEmail);
            Assert.AreEqual(reviewDescription, retrievedService.Reviews[0].ReviewDescription);
            Assert.AreEqual(service, retrievedService.Reviews[0].Service);
        }

        [Test]
        public void ServiceSaveUpdateAndRetrieveTest_ChecksIfTheServiceInstanceIsSavedAndThenUpdatedAsExpected_VerifiesByTheReturnedValue()
        {
            // Get the ServiceRepository instance using Ninejct DI
            var servicesRepository = _kernel.Get<IServicesRepository>();

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
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProfessionType)
                .ServiceEntityType(entity).DateEstablished(dateEstablished)
                .Latitude(latitude).Longitude(longitude).Build();
            
            // Save the Service
            servicesRepository.SaveOrUpdate(service);

            // Retrieve the result
            var retrievedService = servicesRepository.GetServiceById(service.Id);         
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name, retrievedService.Name);
            Assert.AreEqual(description, retrievedService.Description);
            Assert.AreEqual(location, retrievedService.Location);
            Assert.AreEqual(phoneNumber, retrievedService.PhoneNumber);
            Assert.AreEqual(serviceEmail, retrievedService.ServiceEmail);
            Assert.AreEqual(uploaderEmail, retrievedService.UploaderEmail);
            Assert.AreEqual(serviceProfessionType, retrievedService.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), entity),
                retrievedService.ServiceEntityType);
            AssertDateTime(dateEstablished, retrievedService.DateEstablished);
            Assert.AreEqual(latitude, retrievedService.Latitude);
            Assert.AreEqual(longitude, retrievedService.Longitude);

            // Now update the Service
            string name2 = "The Grass Hopper";
            string description2 = "We make swords so sharp and strong, they can chop grass :D";
            string location2 = "Satellite Town, Rawalpindi, Pakistan";
            string phoneNumber2 = "03168948486";
            string serviceEmail2 = "grass@hopper1234567.com";
            string uploaderEmail2 = "uplaoder@hopper1234567.com";
            string profession2 = Service.GetProfessionsList().Last().Value.First();
            string entity2 = "Individual";
            decimal latitude2 = 34.7M;
            decimal longitude2 = 74.1M;
            DateTime dateEstablished2 = DateTime.Now.AddDays(1);
            retrievedService.UpdateService(name2, description2, location2, phoneNumber2, serviceEmail2,
                uploaderEmail2, profession2, entity2, dateEstablished2, latitude2, longitude2);

            retrievedService = servicesRepository.GetServiceById(service.Id);
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name2, retrievedService.Name);
            Assert.AreEqual(description2, retrievedService.Description);
            Assert.AreEqual(location2, retrievedService.Location);
            Assert.AreEqual(phoneNumber2, retrievedService.PhoneNumber);
            Assert.AreEqual(serviceEmail2, retrievedService.ServiceEmail);
            Assert.AreEqual(profession2, retrievedService.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), entity2),
                retrievedService.ServiceEntityType);
            AssertDateTime(dateEstablished2, retrievedService.DateEstablished);
            Assert.AreEqual(latitude2, retrievedService.Latitude);
            Assert.AreEqual(longitude2, retrievedService.Longitude);
        }

        [Test]
        public void ServiceSaveWithoutMandatoryFieldsTest_ChecksIfTheServiceInstanceIsSavedWithoutThemandatoryFieldsAsExpected_VerifiesByTheReturnedValue()
        {
            // Get the ServiceRepository instance using Ninejct DI
            var servicesRepository = _kernel.Get<IServicesRepository>();

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

        // Search using the coordinates
        [Test]
        public void SearchServiceUsingLocationCoordinatesTest_ChecksIfTheServiceInstanceIsSavedWithoutThemandatoryFieldsAsExpected_VerifiesByTheReturnedValue()
        {
            Console.WriteLine("Start");
            DateTime startTime = DateTime.Now;

            // This test saves 6 services at distant locations in Rawalpindi and Islamabad and then queries 
            // one location(Bahria Town). Services within the defined radius( approx 38 kilometers) are
            // retreived. Services outside of this radius(Sangada, KPK) are ignored
            // This test also includes the zero points, i.e., Islamabad, Pakistan & Rawalpindi, Pakistan 
            // directly

            // Get the ServiceRepository instance using Ninejct DI
            var servicesRepository = _kernel.Get<IServicesRepository>();

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
            Assert.AreEqual(phoneNumber6, retrievedService.PhoneNumber);
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
            Assert.AreEqual(phoneNumber, retrievedService.PhoneNumber);
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
            Assert.AreEqual(reviewAuthorName, retrievedService.Reviews[0].Authorname);
            Assert.AreEqual(reviewAuthorEmail, retrievedService.Reviews[0].AuthorEmail);
            Assert.AreEqual(reviewDescription, retrievedService.Reviews[0].ReviewDescription);
            Assert.AreEqual(service, retrievedService.Reviews[0].Service);

            // Verify House no 3
            retrievedService = retrievedServices[2];
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name3, retrievedService.Name);
            Assert.AreEqual(description3, retrievedService.Description);
            Assert.AreEqual(location3, retrievedService.Location);
            Assert.AreEqual(phoneNumber3, retrievedService.PhoneNumber);
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
            Assert.AreEqual(reviewAuthorName3, retrievedService.Reviews[0].Authorname);
            Assert.AreEqual(reviewAuthorEmail3, retrievedService.Reviews[0].AuthorEmail);
            Assert.AreEqual(reviewDescription3, retrievedService.Reviews[0].ReviewDescription);
            Assert.AreEqual(service3, retrievedService.Reviews[0].Service);

            // Verify House no 4
            retrievedService = retrievedServices[3];
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name4, retrievedService.Name);
            Assert.AreEqual(description4, retrievedService.Description);
            Assert.AreEqual(location4, retrievedService.Location);
            Assert.AreEqual(phoneNumber4, retrievedService.PhoneNumber);
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
            Assert.AreEqual(phoneNumber2, retrievedService.PhoneNumber);
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
            

            

            int timeTaken = (DateTime.Now - startTime).Milliseconds;
            Console.WriteLine("Total Time Taken: " + timeTaken);
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
            var servicesRepository = _kernel.Get<IServicesRepository>();

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
            Assert.AreEqual(phoneNumber4, retrievedService.PhoneNumber);
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
            Assert.AreEqual(reviewAuthorName4, retrievedService.Reviews[0].Authorname);
            Assert.AreEqual(reviewAuthorEmail4, retrievedService.Reviews[0].AuthorEmail);
            Assert.AreEqual(reviewDescription4, retrievedService.Reviews[0].ReviewDescription);
            Assert.AreEqual(service4, retrievedService.Reviews[0].Service);

            // Verify House no 6
            retrievedService = retrievedServices[1];
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name6, retrievedService.Name);
            Assert.AreEqual(description6, retrievedService.Description);
            Assert.AreEqual(location6, retrievedService.Location);
            Assert.AreEqual(phoneNumber6, retrievedService.PhoneNumber);
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

        /// <summary>
        /// Ignores miliseconds in the datetime provided and then asserts it against the expected DateTime
        /// value
        /// </summary>
        /// <param name="expectedDateTime"></param>
        /// <param name="actualDateTime"></param>
        private void AssertDateTime(DateTime expectedDateTime, DateTime actualDateTime)
        {
            actualDateTime = actualDateTime.AddTicks(-actualDateTime.Ticks % TimeSpan.TicksPerSecond);
            expectedDateTime = expectedDateTime.AddTicks(-expectedDateTime.Ticks % TimeSpan.TicksPerSecond);
            Assert.AreEqual(expectedDateTime, actualDateTime);
        }
    }
}
