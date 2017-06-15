using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Newtonsoft.Json;
using Ninject;
using NUnit.Framework;
using RentStuff.Common.Domain.Model;
using RentStuff.Common.NinjectModules;
using RentStuff.Common.Utilities;
using RentStuff.Services.Application.ApplicationServices.Commands;
using RentStuff.Services.Application.ApplicationServices.Representations;
using RentStuff.Services.Application.Ninject.Modules;
using RentStuff.Services.Infrastructure.Persistence.NinjectModules;
using RentStuff.Services.Ports.Adapter.Rest.Ninject.Modules;
using RentStuff.Services.Ports.Adapter.Rest.Resources;

namespace RentStuff.Services.Ports.IntegrationTests
{
    [TestFixture]
    public class ServicesControllerTests
    {
        private DatabaseUtility _databaseUtility;

        [SetUp]
        public void Setup()
        {
            var connection = StringCipher.DecipheredConnectionString;
            _databaseUtility = new DatabaseUtility(connection);
            _databaseUtility.Create();

            
            //_databaseUtility.Populate();
            //ShowNhibernateLogging();
            //NhConnectionDecipherService.SetupDecipheredConnectionString();
        }

        [TearDown]
        public void Teardown()
        {
            _databaseUtility.Create();
        }

        /// <summary>
        /// Initiates the actual instances that will be used in the production environment
        /// </summary>
        /// <returns></returns>
        private IKernel InitiateLiveDependencies()
        {
             var kernel = new StandardKernel();
            kernel.Load<ServicesPersistenceNinjectModule>();
            kernel.Load<CommonNinjectModule>();
            kernel.Load<ServiceApplicationNinjectModule>();
            kernel.Load<ServicesPortsNinjectModule>();

            return kernel;
        }

        // Saves a service, adds a review and retrieves the result for veirfication
        [Test]
        public void SaveServiceAndAddReviewTest_ChecksIfTheServiceIsSavedAndRetrieedAsExpected_VerifiesFromTheDatabaseReturnValue()
        {
            var kernel = InitiateLiveDependencies();

            var serviceController = kernel.Get<ServiceController>();

            string name = "The Stone Chopper";
            string description = "We make swrods so sharp and strong, they can chop stones";
            string location = "Pindora, Rawalpindi, Pakistan";
            string mobileNumber = "03770000000";
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

            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new service will fail
            serviceController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, uploaderEmail)
                })
            });
            // Save the Service
            var postHttpResponse = serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            var savedServiceId = ((OkNegotiatedContentResult<string>) postHttpResponse).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId));

            // Add a coupele of reviews
            // Review # 1
            string authorName = "King Arthur";
            string authorEmail = "kingofthelingbling@kingdomcup12345678.com";
            string reviewDescription = "Off goes your head";
            var addReview = new AddReviewCommand(authorName, authorEmail, reviewDescription, savedServiceId);
            serviceController.PostReview(JsonConvert.SerializeObject(addReview));
            // Review # 2
            string authorName2 = "King Arthur 2";
            string authorEmail2 = "kingofthelingbling2@kingdomcup12345678.com";
            string reviewDescription2 = "Off goes your head 2";
            var addReview2 = new AddReviewCommand(authorName2, authorEmail2, reviewDescription2, savedServiceId);
            serviceController.PostReview(JsonConvert.SerializeObject(addReview2));
            // Get the service by providing the ServiceId
            var getHttpResponse = serviceController.GetHouse(serviceId:savedServiceId);
            var serviceFullRepresentation = ((OkNegotiatedContentResult<ServiceFullRepresentation>) getHttpResponse).Content;
            Assert.IsNotNull(serviceFullRepresentation);
            Assert.AreEqual(savedServiceId, serviceFullRepresentation.Id);
            Assert.AreEqual(name, serviceFullRepresentation.Name);
            Assert.AreEqual(description, serviceFullRepresentation.Description);
            Assert.AreEqual(location, serviceFullRepresentation.Location);
            Assert.AreEqual(mobileNumber, serviceFullRepresentation.MobileNumber);
            Assert.AreEqual(serviceEmail, serviceFullRepresentation.ServiceEmail);
            Assert.AreEqual(serviceProfessionType, serviceFullRepresentation.ServiceProfessionType);
            Assert.AreEqual(serviceEntityType, serviceFullRepresentation.ServiceEntityType);
            Assertion.AssertNullableDateTime(dateEstablished, serviceFullRepresentation.DateEstablished);
            Assert.AreEqual(facebookLink, serviceFullRepresentation.FacebookLink);
            Assert.AreEqual(twitterLink, serviceFullRepresentation.TwitterLink);
            Assert.AreEqual(instagramLink, serviceFullRepresentation.InstagramLink);
            Assert.AreEqual(websiteLink, serviceFullRepresentation.WebsiteLink);

            // Review the Reviews
            Assert.AreEqual(2, serviceFullRepresentation.Reviews.Count);
            // Review # 1
            Assert.AreEqual(authorName, serviceFullRepresentation.Reviews[0].Authorname);
            Assert.AreEqual(authorEmail, serviceFullRepresentation.Reviews[0].AuthorEmail);
            Assert.AreEqual(reviewDescription, serviceFullRepresentation.Reviews[0].ReviewDescription);
            Assert.AreEqual(savedServiceId, serviceFullRepresentation.Reviews[0].ServiceId);
            // Review # 2
            Assert.AreEqual(authorName2, serviceFullRepresentation.Reviews[1].Authorname);
            Assert.AreEqual(authorEmail2, serviceFullRepresentation.Reviews[1].AuthorEmail);
            Assert.AreEqual(reviewDescription2, serviceFullRepresentation.Reviews[1].ReviewDescription);
            Assert.AreEqual(savedServiceId, serviceFullRepresentation.Reviews[1].ServiceId);
        }
    }
}
