﻿using System;
using System.Configuration;
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
            string email = "stone@chopper1234567.com";
            string profession = "Welder";
            string entity = "Organization";
            DateTime dateEstablished = DateTime.Now;
            Service service = new Service(name, description, location, phoneNumber,
                        email, profession, entity, dateEstablished);

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
            Assert.AreEqual(email, retrievedService[0].Email);
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
    }
}
