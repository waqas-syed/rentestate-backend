using System;
using System.Configuration;
using Ninject;
using NUnit.Framework;
using RentStuff.Common;
using RentStuff.Common.NinjectModules;
using RentStuff.Common.Services.LocationServices;
using RentStuff.Services.Domain.Model.ServiceAggregate;
using RentStuff.Services.Infrastructure.Persistence.NHibernateCompound;
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
            var connection = ConfigurationManager.ConnectionStrings["MySql"].ConnectionString;
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
            string profession = "Welder";
            string entity = "Organization";
            DateTime dateEstablished = DateTime.Now;
            decimal latitude = 33.7M;
            decimal longitude = 73.1M;
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(profession)
                .ServiceEntityType(entity).DateEstablished(dateEstablished)
                .Latitude(latitude).Longitude(longitude).Build();

            // Provide a review to save
            string authorName = "King Arthur";
            string authorEmail = "kingofthelingbling@kingdomcup12345678.com";
            string reviewDescription = "Off goes your head";
            service.AddReview(authorName, authorEmail, reviewDescription);
            
            servicesRepository.SaveOrUpdate(service);
            servicesRepository.Commit();

            // Retrieve the result
            var retrievedService = servicesRepository.GetServiceById(service.Id);
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name, retrievedService.Name);
            Assert.AreEqual(description, retrievedService.Description);
            Assert.AreEqual(location, retrievedService.Location);
            Assert.AreEqual(phoneNumber, retrievedService.PhoneNumber);
            Assert.AreEqual(serviceEmail, retrievedService.ServiceEmail);
            Assert.AreEqual(uploaderEmail, retrievedService.UploaderEmail);
            Assert.AreEqual((ServiceProfessionType)Enum.Parse(typeof(ServiceProfessionType), profession),
                retrievedService.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), entity),
                retrievedService.ServiceEntityType);
            AssertDateTime(dateEstablished, retrievedService.DateEstablished);
            Assert.AreEqual(latitude, retrievedService.Latitude);
            Assert.AreEqual(longitude, retrievedService.Longitude);

            // Check the Ratings
            Assert.IsNotNull(retrievedService.Ratings.Service);
            Assert.AreEqual(service, retrievedService.Ratings.Service);

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
            string profession = "Welder";
            string entity = "Organization";
            DateTime dateEstablished = DateTime.Now;
            decimal latitude = 33.7M;
            decimal longitude = 73.1M;
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(profession)
                .ServiceEntityType(entity).DateEstablished(dateEstablished)
                .Latitude(latitude).Longitude(longitude).Build();
            
            // Save the Service
            servicesRepository.SaveOrUpdate(service);
            servicesRepository.Commit();

            // Retrieve the result
            var retrievedService = servicesRepository.GetServiceById(service.Id);         
            Assert.IsNotNull(retrievedService);
            Assert.AreEqual(name, retrievedService.Name);
            Assert.AreEqual(description, retrievedService.Description);
            Assert.AreEqual(location, retrievedService.Location);
            Assert.AreEqual(phoneNumber, retrievedService.PhoneNumber);
            Assert.AreEqual(serviceEmail, retrievedService.ServiceEmail);
            Assert.AreEqual(uploaderEmail, retrievedService.UploaderEmail);
            Assert.AreEqual((ServiceProfessionType)Enum.Parse(typeof(ServiceProfessionType), profession),
                retrievedService.ServiceProfessionType);
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
            string profession2 = "Carpenter";
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
            Assert.AreEqual((ServiceProfessionType)Enum.Parse(typeof(ServiceProfessionType), profession2),
                retrievedService.ServiceProfessionType);
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
            string profession = "Welder";
            string entity = "Organization";
            decimal latitude = 34.7M;
            decimal longitude = 74.1M;
            Service service = new Service.ServiceBuilder().Name(name)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(profession).ServiceEntityType(entity)
                .Latitude(latitude).Longitude(longitude).Build();
            
            // Save the Service
            servicesRepository.SaveOrUpdate(service);
            servicesRepository.Commit();

            // Retrieve the result and just check that we got one
            var retrievedService = servicesRepository.GetServiceById(service.Id);
            Assert.IsNotNull(retrievedService);
        }

        [Test]
        public void SearchServiceUsingLocationCoordinatesTest_ChecksIfTheServiceInstanceIsSavedWithoutThemandatoryFieldsAsExpected_VerifiesByTheReturnedValue()
        {
            Console.WriteLine("Start");
            DateTime startTime = DateTime.Now;
            
            // This test saves multiple services at distant locations and then queries one location. Services 
            // within the defined radius( approx 38 kilometers) are retreived. Services outside of this radius
            // are ignored

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
            string serviceProfessionType = "Welder";
            string serviceEntityType = "Organization";
            var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(location);
            decimal latitude = coordinatesFromAddress.Item1;
            decimal longitude = coordinatesFromAddress.Item2;
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProfessionType)
                .ServiceEntityType(serviceEntityType)
                .Latitude(latitude).Longitude(longitude).Build();
            // Save the Service
            servicesRepository.SaveOrUpdate(service);

            // House # 2
            string name2 = "The Lightning Bolt";
            string description2 = "Bolt, Electrify!";
            string location2 = "Islamabad, Pakistan";
            string phoneNumber2 = "03168948486";
            string serviceEmail2 = "bolt@chopper1234567.com";
            string uploaderEmail2 = "uploader@bolt1234567.com";
            string serviceProfessionType2 = "Electrician";
            string serviceEntityType2 = "Organization";
            var coordinatesFromAddress2 = geocodingService.GetCoordinatesFromAddress(location2);
            decimal latitude2 = coordinatesFromAddress2.Item1;
            decimal longitude2 = coordinatesFromAddress2.Item2;
            Service service2 = new Service.ServiceBuilder().Name(name2).Description(description2)
                .Location(location2).PhoneNumber(phoneNumber2).ServiceEmail(serviceEmail2)
                .UploaderEmail(uploaderEmail2).ServiceProfessionType(serviceProfessionType2).ServiceEntityType(serviceEntityType2)
                .Latitude(latitude2).Longitude(longitude2).Build();
            // Save the Service
            servicesRepository.SaveOrUpdate(service2);

            // House # 3
            string name3 = "The Grass Hopper";
            string description3 = "We make choppers, so they can chop grass :D";
            string location3 = "E-11, Islamabad, Islamabad Capital Territory, Pakistan";
            string phoneNumber3 = "03168948486";
            string serviceEmail3 = "grass@hopper1234567.com";
            string uploaderEmail3 = "uploader@hopper1234567.com";
            string serviceProfessionType3 = "Carpenter";
            string serviceEntityType3 = "Individual";
            var coordinatesFromAddress3 = geocodingService.GetCoordinatesFromAddress(location3);
            decimal latitude3 = coordinatesFromAddress3.Item1;
            decimal longitude3 = coordinatesFromAddress3.Item2;
            Service service3 = new Service.ServiceBuilder().Name(name3).Description(description3)
                .Location(location3).PhoneNumber(phoneNumber3).ServiceEmail(serviceEmail3)
                .UploaderEmail(uploaderEmail3).ServiceProfessionType(serviceProfessionType3)
                .ServiceEntityType(serviceEntityType3)
                .Latitude(latitude3).Longitude(longitude3).Build();
            // Save the Service
            servicesRepository.SaveOrUpdate(service3);

            // House # 4
            string name4 = "The Onion Chopper";
            string description4 = "Meat, carbs and sauces. All combined in perfect proportions.";
            string location4 = "Bahria Town, Rawalpindi, Punjab, Pakistan";
            string phoneNumber4 = "03455138018";
            string serviceEmail4 = "food@chopper1234567.com";
            string uploaderEmail4 = "uploader@onion1234567.com";
            string serviceProfessionType4 = "FoodCaterer";
            string serviceEntityType4 = "Organization";
            var coordinatesFromAddress4 = geocodingService.GetCoordinatesFromAddress(location4);
            decimal latitude4 = coordinatesFromAddress4.Item1;
            decimal longitude4 = coordinatesFromAddress4.Item2;
            Service service4 = new Service.ServiceBuilder().Name(name4).Description(description4)
                .Location(location4).PhoneNumber(phoneNumber4).ServiceEmail(serviceEmail4)
                .UploaderEmail(uploaderEmail4).ServiceProfessionType(serviceProfessionType4)
                .ServiceEntityType(serviceEntityType4)
                .Latitude(latitude4).Longitude(longitude4).Build();
            // Save the Service
            servicesRepository.SaveOrUpdate(service4);

            // House # 5
            string name5 = "The Onion Chopper";
            string description5 = "Meat, carbs and sauces. All combined in perfect proportions.";
            string location5 = "Rawalpindi, Pakistan";
            string phoneNumber5 = "03455138018";
            string serviceEmail5 = "food@chopper1234567.com";
            string uploaderEmail5 = "uploader@onion1234567.com";
            string serviceProfessionType5 = "FoodCaterer";
            string serviceEntityType5 = "Organization";
            var coordinatesFromAddress5 = geocodingService.GetCoordinatesFromAddress(location4);
            decimal latitude5 = coordinatesFromAddress5.Item1;
            decimal longitude5 = coordinatesFromAddress5.Item2;
            Service service5 = new Service.ServiceBuilder().Name(name5).Description(description5)
                .Location(location5).PhoneNumber(phoneNumber5).ServiceEmail(serviceEmail5)
                .UploaderEmail(uploaderEmail5).ServiceProfessionType(serviceProfessionType5)
                .ServiceEntityType(serviceEntityType5)
                .Latitude(latitude5).Longitude(longitude5).Build();
            // Save the Service
            servicesRepository.SaveOrUpdate(service5);

            // House # 6
            string name6 = "The Onion Chopper";
            string description6 = "Meat, carbs and sauces. All combined in perfect proportions.";
            string location6 = "Talhar, Talhaar, Islamabad Capital Territory, Pakistan";
            string phoneNumber6 = "03455138018";
            string serviceEmail6 = "food@chopper1234567.com";
            string uploaderEmail6 = "uploader@onion1234567.com";
            string serviceProfessionType6 = "FoodCaterer";
            string serviceEntityType6 = "Organization";
            var coordinatesFromAddress6 = geocodingService.GetCoordinatesFromAddress(location4);
            decimal latitude6 = coordinatesFromAddress6.Item1;
            decimal longitude6 = coordinatesFromAddress6.Item2;
            Service service6 = new Service.ServiceBuilder().Name(name6).Description(description6)
                .Location(location6).PhoneNumber(phoneNumber6).ServiceEmail(serviceEmail6)
                .UploaderEmail(uploaderEmail6).ServiceProfessionType(serviceProfessionType6)
                .ServiceEntityType(serviceEntityType6)
                .Latitude(latitude6).Longitude(longitude6).Build();
            // Save the Service
            servicesRepository.SaveOrUpdate(service6);
            servicesRepository.Commit();

            // Search by giving Bahroa Town as the location. It should include all of the above saved houses
            // except House#6, because it is more than 38 kilometers away from Bahria Town
            var retrievedServices = servicesRepository.GetServicesByLocation(latitude4, longitude4);
            Assert.IsNotNull(retrievedServices);
            
            int timeTaken = (DateTime.Now - startTime).Milliseconds;
            Console.WriteLine("Total Time Taken: " + timeTaken);

            
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
