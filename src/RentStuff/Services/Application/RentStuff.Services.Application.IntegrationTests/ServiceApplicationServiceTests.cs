using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using NUnit.Framework;
using RentStuff.Common;
using RentStuff.Common.NinjectModules;
using RentStuff.Services.Application.ApplicationServices;
using RentStuff.Services.Application.Commands;
using RentStuff.Services.Application.Ninject.Modules;
using RentStuff.Services.Domain.Model.ServiceAggregate;
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
            string serviceProfessionType = Service.GetProfessionsList().First().Value.First();
            string serviceEntityType = "Organization";
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
        }
    }
}
