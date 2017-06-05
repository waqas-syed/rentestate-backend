using System;
using System.Configuration;
using System.Linq;
using FluentNHibernate.Data;
using Ninject;
using NUnit.Framework;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using RentStuff.Common;
using RentStuff.Services.Domain.Model.ServiceAggregate;
using RentStuff.Services.Infrastructure.Persistence.NinjectModules;

namespace RentStuff.Services.Infrastructure.Persistence.Tests
{
    [TestFixture]
    public class ServicesRepositoryTests
    {
        private DatabaseUtility _databaseUtility;

        [SetUp]
        public void Setup()
        {
            //var connection = StringCipher.DecipheredConnectionString;
            var connection = ConfigurationManager.ConnectionStrings["MySql"].ConnectionString;
            _databaseUtility = new DatabaseUtility(connection);
            _databaseUtility.Create();
            //NhConnectionDecipherService.SetupDecipheredConnectionString();
            //_databaseUtility.Populate();
        }

        [TearDown]
        public void Teardown()
        {
            _databaseUtility.Create();
        }

        [Test]
        public void ServiceSaveTest_ChecksIfTheServiceInstanceIsSavedAsExpected_VerifiesByTheReturnedValue()
        {
            string name = "The Stone Chopper";
            string description = "We make swrods so sharp and strong, they can chop stones";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03325329974";
            string serviceEmail = "stone@chopper1234567.com";
            string uploaderEmail = "uploader@chopper1234567.com";
            string profession = "Welder";
            string entity = "Organization";
            DateTime dateEstablished = DateTime.Now;
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(profession)
                .ServiceEntityType(entity).DateEstablished(dateEstablished).Build();

            // Provide a review to save
            string authorName = "King Arthur";
            string authorEmail = "kingofthelingbling@kingdomcup12345678.com";
            string reviewDescription = "Off goes your head";
            service.AddReview(authorName, authorEmail, reviewDescription);

            var kernel = new StandardKernel();
            kernel.Load<NHibernateModule>();
            var servicesRepository = kernel.Get<IServicesRepository>();
            servicesRepository.SaveOrUpdate(service);

            // Retrieve the result
            var retrievedService = servicesRepository.GetServiceByName(name);
            Assert.AreEqual(1, retrievedService.Count);
            Assert.AreEqual(name, retrievedService[0].Name);
            Assert.AreEqual(description, retrievedService[0].Description);
            Assert.AreEqual(location, retrievedService[0].Location);
            Assert.AreEqual(phoneNumber, retrievedService[0].PhoneNumber);
            Assert.AreEqual(serviceEmail, retrievedService[0].ServiceEmail);
            Assert.AreEqual(uploaderEmail, retrievedService[0].UploaderEmail);
            Assert.AreEqual((ServiceProfessionType)Enum.Parse(typeof(ServiceProfessionType), profession),
                retrievedService[0].ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), entity),
                retrievedService[0].ServiceEntityType);

            // Check the Ratings
            Assert.IsNotNull(retrievedService[0].Ratings.Service);
            Assert.AreEqual(service, retrievedService[0].Ratings.Service);

            // Check the Reviews
            Assert.IsNotNull(retrievedService[0].Reviews);
            Assert.AreEqual(1, retrievedService[0].Reviews.Count);
            Assert.AreEqual(authorName, retrievedService[0].Reviews[0].Authorname);
            Assert.AreEqual(authorEmail, retrievedService[0].Reviews[0].AuthorEmail);
            Assert.AreEqual(reviewDescription, retrievedService[0].Reviews[0].ReviewDescription);
            Assert.AreEqual(service, retrievedService[0].Reviews[0].Service);
        }

        [Test]
        public void ServiceSaveAndUpdateTest_ChecksIfTheServiceInstanceIsSavedAndThenupdatedAsExpected_VerifiesByTheReturnedValue()
        {
            string name = "The Stone Chopper";
            string description = "We make swords so sharp and strong, they can chop stones";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03325329974";
            string serviceEmail = "stone@chopper1234567.com";
            string uploaderEmail = "uploader@chopper1234567.com";
            string profession = "Welder";
            string entity = "Organization";
            DateTime dateEstablished = DateTime.Now;
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(profession)
                .ServiceEntityType(entity).DateEstablished(dateEstablished).Build();

            var kernel = new StandardKernel();
            kernel.Load<NHibernateModule>();
            var servicesRepository = kernel.Get<IServicesRepository>();
            // Save the Service
            servicesRepository.SaveOrUpdate(service);

            // Retrieve the result
            var retrievedServices = servicesRepository.GetServiceByName(name);
            Assert.AreEqual(1, retrievedServices.Count);
            var retrievedService = retrievedServices.FirstOrDefault();            
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

            // Now update the Service
            string name2 = "The Grass Hopper";
            string description2 = "We make swords so sharp and strong, they can chop grass :D";
            string location2 = "Satellite Town, Rawalpindi, Pakistan";
            string phoneNumber2 = "03455138018";
            string serviceEmail2 = "grass@hopper1234567.com";
            string uploaderEmail2 = "uplaoder@hopper1234567.com";
            string profession2 = "Carpenter";
            string entity2 = "Individual";
            DateTime dateEstablished2 = DateTime.Now.AddDays(1);
            retrievedService.UpdateService(name2, description2, location2, phoneNumber2, serviceEmail2,
                uploaderEmail2, profession2, entity2, dateEstablished2);

            retrievedServices = servicesRepository.GetServiceByName(name);
            Assert.AreEqual(1, retrievedServices.Count);
            retrievedService = retrievedServices.FirstOrDefault();
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
        }
    }
}
