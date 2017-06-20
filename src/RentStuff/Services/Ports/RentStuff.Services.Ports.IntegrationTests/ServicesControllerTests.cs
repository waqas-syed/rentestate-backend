using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http.Results;
using Newtonsoft.Json;
using Ninject;
using NUnit.Framework;
using RentStuff.Common.Domain.Model;
using RentStuff.Common.Ninject.Modules;
using RentStuff.Common.Utilities;
using RentStuff.Services.Application.ApplicationServices.Commands;
using RentStuff.Services.Application.ApplicationServices.Representations;
using RentStuff.Services.Application.Ninject.Modules;
using RentStuff.Services.Infrastructure.Persistence.Ninject;
using RentStuff.Services.Infrastructure.Persistence.Ninject.Modules;
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
            kernel.Load<ServicePersistenceNinjectModule>();
            kernel.Load<CommonNinjectModule>();
            kernel.Load<ServiceApplicationNinjectModule>();
            kernel.Load<ServicePortsNinjectModule>();

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
            var getHttpResponse = serviceController.Get(serviceId:savedServiceId);
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

        // Save + retrieve then Update + retrieve 
        [Test]
        public void SaveAndUpdateServiceTest_ChecksIfTheServiceIsSavedUpdatedAndRetrieedAsExpected_VerifiesFromTheDatabaseReturnValue()
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
            var savedServiceId = ((OkNegotiatedContentResult<string>)postHttpResponse).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId));
            
            // Get the service by providing the ServiceId
            var getHttpResponse = serviceController.Get(serviceId: savedServiceId);
            var serviceFullRepresentation = ((OkNegotiatedContentResult<ServiceFullRepresentation>)getHttpResponse).Content;
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

            // Update the Service with the following values
            string name2 = "The Lightning Bolt";
            string description2 = "Bolt, Electrify!";
            string location2 = "Islamabad, Pakistan";
            string mobileNumber2 = "03168948486";
            string serviceEmail2 = "bolt@chopper1234567.com";
            string serviceProfessionType2 = "Electrician";
            string serviceEntityType2 = "Organization";
            DateTime dateEstablished2 = DateTime.Now.AddYears(-2);
            string facebookLink2 = "https://dummyfacebooklink-123456789-2.com";
            string instagramLink2 = "https://dummyinstagramlink-123456789-2.com";
            string twitterLink2 = "https://dummytwitterlink-123456789-2.com";
            string websiteLink2 = "https://dummywebsitelink-123456789-2.com";
            var updateServiceCommand = new UpdateServiceCommand(savedServiceId, name2, description2, location2,
                mobileNumber2, serviceEmail2, serviceProfessionType2, serviceEntityType2, dateEstablished2,
                facebookLink2, instagramLink2, twitterLink2, websiteLink2);
            serviceController.Put(JsonConvert.SerializeObject(updateServiceCommand));

            // Add a Review
            string authorName = "King Arthur";
            string authorEmail = "kingofthelingbling@kingdomcup12345678.com";
            string reviewDescription = "Off goes your head";
            var addReview = new AddReviewCommand(authorName, authorEmail, reviewDescription, savedServiceId);
            serviceController.PostReview(JsonConvert.SerializeObject(addReview));

            // Get the service by providing the ServiceId
            getHttpResponse = serviceController.Get(serviceId: savedServiceId);
            serviceFullRepresentation = ((OkNegotiatedContentResult<ServiceFullRepresentation>)getHttpResponse).Content;
            Assert.IsNotNull(serviceFullRepresentation);
            Assert.AreEqual(savedServiceId, serviceFullRepresentation.Id);
            Assert.AreEqual(name2, serviceFullRepresentation.Name);
            Assert.AreEqual(description2, serviceFullRepresentation.Description);
            Assert.AreEqual(location2, serviceFullRepresentation.Location);
            Assert.AreEqual(mobileNumber2, serviceFullRepresentation.MobileNumber);
            Assert.AreEqual(serviceEmail2, serviceFullRepresentation.ServiceEmail);
            Assert.AreEqual(serviceProfessionType2, serviceFullRepresentation.ServiceProfessionType);
            Assert.AreEqual(serviceEntityType2, serviceFullRepresentation.ServiceEntityType);
            Assertion.AssertNullableDateTime(dateEstablished2, serviceFullRepresentation.DateEstablished);
            Assert.AreEqual(facebookLink2, serviceFullRepresentation.FacebookLink);
            Assert.AreEqual(twitterLink2, serviceFullRepresentation.TwitterLink);
            Assert.AreEqual(instagramLink2, serviceFullRepresentation.InstagramLink);
            Assert.AreEqual(websiteLink2, serviceFullRepresentation.WebsiteLink);

            // Review the Review
            Assert.AreEqual(1, serviceFullRepresentation.Reviews.Count);
            Assert.AreEqual(authorName, serviceFullRepresentation.Reviews[0].Authorname);
            Assert.AreEqual(authorEmail, serviceFullRepresentation.Reviews[0].AuthorEmail);
            Assert.AreEqual(reviewDescription, serviceFullRepresentation.Reviews[0].ReviewDescription);
            Assert.AreEqual(savedServiceId, serviceFullRepresentation.Reviews[0].ServiceId);
        }

        // Save + Retrive and then Delete Test
        [Test]
        public void SaveAndDeleteTest_ChecksIfTheServiceIsSavedAndRetrieedAsExpected_VerifiesFromTheDatabaseReturnValue()
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
            var savedServiceId = ((OkNegotiatedContentResult<string>)postHttpResponse).Content;
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
            var getHttpResponse = serviceController.Get(serviceId: savedServiceId);
            var serviceFullRepresentation = ((OkNegotiatedContentResult<ServiceFullRepresentation>)getHttpResponse).Content;
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

            // DELETE THE SERVICE
            serviceController.Delete(savedServiceId);
            // Response should be null
            getHttpResponse = serviceController.Get(serviceId: savedServiceId);
            Assert.AreEqual(getHttpResponse.GetType().Name, typeof(BadRequestErrorMessageResult).Name);
        }

        // Search by ServiceProfessionType
        [Test]
        public void GetServicesbyProfessionTest_ChecksIfTheServiceIsRetrievedUsingProfessionTypeAsExpected_VerifiesFromTheDatabaseReturnValue()
        {
            // Save 2 services, both with different ServiceProfessionTypes. Search for only 1. Thus only 1
            // Service should be retrieved
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
            var savedServiceId = ((OkNegotiatedContentResult<string>)postHttpResponse).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId));

            // Service no 2
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
            createServiceCommand = new CreateServiceCommand(name2, description2, location2, mobileNumber2,
                serviceEmail2, uploaderEmail2, serviceProfessionType2, serviceEntityType2, dateEstablished2,
                facebookLink2, instagramLink2, twitterLink2, websiteLink2);
            Assert.IsNotNull(createServiceCommand);
            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new service will fail
            serviceController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, uploaderEmail2)
                })
            });
            // Save the Service no 2
            postHttpResponse = serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            var savedServiceId2 = ((OkNegotiatedContentResult<string>)postHttpResponse).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId2));

            // Get the services by providing the ServiceProfessionType.
            // Should yield only the 1st Service that corresponds to the ServiceProfessionType
            var getHttpResponse = serviceController.Get(serviceProfessionType: serviceProfessionType);
            var servicePartialRepresentations = ((OkNegotiatedContentResult<IList<ServicePartialRepresentation>>)getHttpResponse).Content;
            Assert.NotNull(servicePartialRepresentations);
            Assert.AreEqual(1, servicePartialRepresentations.Count);
            // Verify Service no 1
            var servicePartialRepresentation = servicePartialRepresentations[0];
            Assert.IsNotNull(servicePartialRepresentation);
            Assert.AreEqual(savedServiceId, servicePartialRepresentation.Id);
            Assert.AreEqual(name, servicePartialRepresentation.Name);
            Assert.AreEqual(location, servicePartialRepresentation.Location);
            Assert.AreEqual(mobileNumber, servicePartialRepresentation.MobileNumber);
            Assert.AreEqual(serviceEmail, servicePartialRepresentation.ServiceEmail);
            Assert.AreEqual(serviceProfessionType, servicePartialRepresentation.ServiceProfessionType);
            Assert.AreEqual(serviceEntityType, servicePartialRepresentation.ServiceEntityType);
            Assert.AreEqual(facebookLink, servicePartialRepresentation.FacebookLink);
            Assert.AreEqual(twitterLink, servicePartialRepresentation.TwitterLink);
            Assert.AreEqual(instagramLink, servicePartialRepresentation.InstagramLink);
            Assert.AreEqual(websiteLink, servicePartialRepresentation.WebsiteLink);
        }

        // Search Services by Location
        [Test]
        public void GetServicesByLocationTest_ChecksIfTheServiceIsRetrievedUsingLocationAsExpected_VerifiesFromTheDatabaseReturnValue()
        {
            // Save 2 Services with different professions but located within the search radius, so a search 
            // for a location would yield both of them
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
            var savedServiceId = ((OkNegotiatedContentResult<string>)postHttpResponse).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId));

            // Service no 2
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
            createServiceCommand = new CreateServiceCommand(name2, description2, location2, mobileNumber2,
                serviceEmail2, uploaderEmail2, serviceProfessionType2, serviceEntityType2, dateEstablished2,
                facebookLink2, instagramLink2, twitterLink2, websiteLink2);
            Assert.IsNotNull(createServiceCommand);
            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new service will fail
            serviceController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, uploaderEmail2)
                })
            });
            // Save the Service no 2
            postHttpResponse = serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            var savedServiceId2 = ((OkNegotiatedContentResult<string>)postHttpResponse).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId2));

            // Get the services by providing the Location
            string searchLocation = "Bahria Town, Rawalpindi, Pakistan";
            var getHttpResponse = serviceController.Get(location: searchLocation);
            var servicePartialRepresentations = ((OkNegotiatedContentResult<IList<ServicePartialRepresentation>>)getHttpResponse).Content;
            Assert.NotNull(servicePartialRepresentations);
            Assert.AreEqual(2, servicePartialRepresentations.Count);
            // Verify Service no 1
            var servicePartialRepresentation = servicePartialRepresentations[0];
            Assert.IsNotNull(servicePartialRepresentation);
            Assert.AreEqual(savedServiceId, servicePartialRepresentation.Id);
            Assert.AreEqual(name, servicePartialRepresentation.Name);
            Assert.AreEqual(location, servicePartialRepresentation.Location);
            Assert.AreEqual(mobileNumber, servicePartialRepresentation.MobileNumber);
            Assert.AreEqual(serviceEmail, servicePartialRepresentation.ServiceEmail);
            Assert.AreEqual(serviceProfessionType, servicePartialRepresentation.ServiceProfessionType);
            Assert.AreEqual(serviceEntityType, servicePartialRepresentation.ServiceEntityType);
            Assert.AreEqual(facebookLink, servicePartialRepresentation.FacebookLink);
            Assert.AreEqual(twitterLink, servicePartialRepresentation.TwitterLink);
            Assert.AreEqual(instagramLink, servicePartialRepresentation.InstagramLink);
            Assert.AreEqual(websiteLink, servicePartialRepresentation.WebsiteLink);

            // Verify Service no 2
            servicePartialRepresentation = servicePartialRepresentations[1];
            Assert.IsNotNull(servicePartialRepresentation);
            Assert.AreEqual(savedServiceId2, servicePartialRepresentation.Id);
            Assert.AreEqual(name2, servicePartialRepresentation.Name);
            Assert.AreEqual(location2, servicePartialRepresentation.Location);
            Assert.AreEqual(mobileNumber2, servicePartialRepresentation.MobileNumber);
            Assert.AreEqual(serviceEmail2, servicePartialRepresentation.ServiceEmail);
            Assert.AreEqual(serviceProfessionType2, servicePartialRepresentation.ServiceProfessionType);
            Assert.AreEqual(serviceEntityType2, servicePartialRepresentation.ServiceEntityType);
            Assert.AreEqual(facebookLink2, servicePartialRepresentation.FacebookLink);
            Assert.AreEqual(twitterLink2, servicePartialRepresentation.TwitterLink);
            Assert.AreEqual(instagramLink2, servicePartialRepresentation.InstagramLink);
            Assert.AreEqual(websiteLink2, servicePartialRepresentation.WebsiteLink);
        }

        // Search Services by Location and Profession
        [Test]
        public void GetServicesByLocationAndProfessionTest_ChecksIfTheServiceIsRetrievedUsingLocationAndProfessionTypeAsExpected_VerifiesFromTheDatabaseReturnValue()
        {
            // Save 2 Services within search radius, but with different professions. Only the 2nd service 
            // should be yielded in the result as it matches both the location and profession
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
            var savedServiceId = ((OkNegotiatedContentResult<string>)postHttpResponse).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId));

            // Service no 2
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
            createServiceCommand = new CreateServiceCommand(name2, description2, location2, mobileNumber2,
                serviceEmail2, uploaderEmail2, serviceProfessionType2, serviceEntityType2, dateEstablished2,
                facebookLink2, instagramLink2, twitterLink2, websiteLink2);
            Assert.IsNotNull(createServiceCommand);
            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new service will fail
            serviceController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, uploaderEmail2)
                })
            });
            // Save the Service no 2
            postHttpResponse = serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            var savedServiceId2 = ((OkNegotiatedContentResult<string>)postHttpResponse).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId2));

            // Get the services by providing the Location
            string searchLocation = "Bahria Town, Rawalpindi, Pakistan";
            var getHttpResponse = serviceController.Get(location: searchLocation, 
                                    serviceProfessionType:serviceProfessionType2);
            var servicePartialRepresentations = ((OkNegotiatedContentResult<IList<ServicePartialRepresentation>>)getHttpResponse).Content;
            Assert.NotNull(servicePartialRepresentations);
            Assert.AreEqual(1, servicePartialRepresentations.Count);
           
            // Verify Service no 2
            var servicePartialRepresentation = servicePartialRepresentations[0];
            Assert.IsNotNull(servicePartialRepresentation);
            Assert.AreEqual(savedServiceId2, servicePartialRepresentation.Id);
            Assert.AreEqual(name2, servicePartialRepresentation.Name);
            Assert.AreEqual(location2, servicePartialRepresentation.Location);
            Assert.AreEqual(mobileNumber2, servicePartialRepresentation.MobileNumber);
            Assert.AreEqual(serviceEmail2, servicePartialRepresentation.ServiceEmail);
            Assert.AreEqual(serviceProfessionType2, servicePartialRepresentation.ServiceProfessionType);
            Assert.AreEqual(serviceEntityType2, servicePartialRepresentation.ServiceEntityType);
            Assert.AreEqual(facebookLink2, servicePartialRepresentation.FacebookLink);
            Assert.AreEqual(twitterLink2, servicePartialRepresentation.TwitterLink);
            Assert.AreEqual(instagramLink2, servicePartialRepresentation.InstagramLink);
            Assert.AreEqual(websiteLink2, servicePartialRepresentation.WebsiteLink);
        }

        // Get Services By Email
        [Test]
        public void GetServicesByEmailTest_ChecksIfTheServicesAreRetrievedUsingEmailAsExpected_VerifiesFromTheDatabaseReturnValue()
        {
            // Save 2 Services with different professions but located within the search radius, so a search 
            // for a location would yield both of them
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
            var savedServiceId = ((OkNegotiatedContentResult<string>)postHttpResponse).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId));

            // Retrieve Service no 1 via email
            var getHttpResponse = serviceController.Get(email: uploaderEmail);
            var servicePartialRepresentations = ((OkNegotiatedContentResult<IList<ServicePartialRepresentation>>)getHttpResponse).Content;
            Assert.NotNull(servicePartialRepresentations);
            Assert.AreEqual(1, servicePartialRepresentations.Count);
            // Verify Service no 1
            var servicePartialRepresentation = servicePartialRepresentations[0];
            Assert.IsNotNull(servicePartialRepresentation);
            Assert.AreEqual(savedServiceId, servicePartialRepresentation.Id);
            Assert.AreEqual(name, servicePartialRepresentation.Name);
            Assert.AreEqual(location, servicePartialRepresentation.Location);
            Assert.AreEqual(mobileNumber, servicePartialRepresentation.MobileNumber);
            Assert.AreEqual(serviceEmail, servicePartialRepresentation.ServiceEmail);
            Assert.AreEqual(serviceProfessionType, servicePartialRepresentation.ServiceProfessionType);
            Assert.AreEqual(serviceEntityType, servicePartialRepresentation.ServiceEntityType);
            Assert.AreEqual(facebookLink, servicePartialRepresentation.FacebookLink);
            Assert.AreEqual(twitterLink, servicePartialRepresentation.TwitterLink);
            Assert.AreEqual(instagramLink, servicePartialRepresentation.InstagramLink);
            Assert.AreEqual(websiteLink, servicePartialRepresentation.WebsiteLink);

            // Service no 2
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
            createServiceCommand = new CreateServiceCommand(name2, description2, location2, mobileNumber2,
                serviceEmail2, uploaderEmail2, serviceProfessionType2, serviceEntityType2, dateEstablished2,
                facebookLink2, instagramLink2, twitterLink2, websiteLink2);
            Assert.IsNotNull(createServiceCommand);
            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new service will fail
            serviceController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, uploaderEmail2)
                })
            });
            // Save the Service no 2
            postHttpResponse = serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            var savedServiceId2 = ((OkNegotiatedContentResult<string>)postHttpResponse).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId2));

            // Retrieve Service no 2 via email
            getHttpResponse = serviceController.Get(email: uploaderEmail2);
            servicePartialRepresentations = ((OkNegotiatedContentResult<IList<ServicePartialRepresentation>>)getHttpResponse).Content;
            Assert.NotNull(servicePartialRepresentations);
            Assert.AreEqual(1, servicePartialRepresentations.Count);
            // Verify Service no 2
            servicePartialRepresentation = servicePartialRepresentations[0];
            Assert.IsNotNull(servicePartialRepresentation);
            Assert.AreEqual(savedServiceId2, servicePartialRepresentation.Id);
            Assert.AreEqual(name2, servicePartialRepresentation.Name);
            Assert.AreEqual(location2, servicePartialRepresentation.Location);
            Assert.AreEqual(mobileNumber2, servicePartialRepresentation.MobileNumber);
            Assert.AreEqual(serviceEmail2, servicePartialRepresentation.ServiceEmail);
            Assert.AreEqual(serviceProfessionType2, servicePartialRepresentation.ServiceProfessionType);
            Assert.AreEqual(serviceEntityType2, servicePartialRepresentation.ServiceEntityType);
            Assert.AreEqual(facebookLink2, servicePartialRepresentation.FacebookLink);
            Assert.AreEqual(twitterLink2, servicePartialRepresentation.TwitterLink);
            Assert.AreEqual(instagramLink2, servicePartialRepresentation.InstagramLink);
            Assert.AreEqual(websiteLink2, servicePartialRepresentation.WebsiteLink);
        }

        // Get Services By Email Fails because we ask for a service associated to another email while
        // being logged in with another email. Only the original uploader can summon a Service by it's email
        [Test]
        public void GetServicesByEmailFailTest_ChecksIfTheServicesAreRetrievedUsingEmailAsExpected_VerifiesFromTheDatabaseReturnValue()
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
            var savedServiceId = ((OkNegotiatedContentResult<string>)postHttpResponse).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId));
            
            // Service no 2
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
            createServiceCommand = new CreateServiceCommand(name2, description2, location2, mobileNumber2,
                serviceEmail2, uploaderEmail2, serviceProfessionType2, serviceEntityType2, dateEstablished2,
                facebookLink2, instagramLink2, twitterLink2, websiteLink2);
            Assert.IsNotNull(createServiceCommand);
            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new service will fail
            serviceController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, uploaderEmail2)
                })
            });
            // Save the Service no 2
            postHttpResponse = serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            var savedServiceId2 = ((OkNegotiatedContentResult<string>)postHttpResponse).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId2));

            // Retrieve Service no 1 via email
            var getHttpResponse = serviceController.Get(email: uploaderEmail);
            Assert.AreEqual(getHttpResponse.GetType().Name, typeof(BadRequestErrorMessageResult).Name);
        }

        // Get All Services
        [Test]
        public void GeAllServicesTest_ChecksIfServicesAreReturnedWithoutSpecifyingAnySearchCriteriaAsExpected_VerifiesFromTheDatabaseReturnValue()
        {
            // 2 Services are stored in distant areas and with different professions. But both should be 
            // retrieved and verified
            var kernel = InitiateLiveDependencies();

            var serviceController = kernel.Get<ServiceController>();

            string name = "The Stone Chopper";
            string description = "We make swrods so sharp and strong, they can chop stones";
            string location = "Bahria Town, Lahore, Punjab, Pakistan";
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
            var savedServiceId = ((OkNegotiatedContentResult<string>)postHttpResponse).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId));

            // Service no 2
            string name2 = "The Lightning Bolt";
            string description2 = "Bolt, Electrify!";
            string location2 = "Bahria Town, Rawalpindi, Punjab, Pakistan";
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
            createServiceCommand = new CreateServiceCommand(name2, description2, location2, mobileNumber2,
                serviceEmail2, uploaderEmail2, serviceProfessionType2, serviceEntityType2, dateEstablished2,
                facebookLink2, instagramLink2, twitterLink2, websiteLink2);
            Assert.IsNotNull(createServiceCommand);
            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new service will fail
            serviceController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, uploaderEmail2)
                })
            });
            // Save the Service no 2
            postHttpResponse = serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            var savedServiceId2 = ((OkNegotiatedContentResult<string>)postHttpResponse).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedServiceId2));
            
            // Get All Services
            var getHttpResponse = serviceController.Get();
            var servicePartialRepresentations = ((OkNegotiatedContentResult<IList<ServicePartialRepresentation>>)getHttpResponse).Content;
            Assert.NotNull(servicePartialRepresentations);
            Assert.AreEqual(2, servicePartialRepresentations.Count);

            // Verify Service no 1
            var servicePartialRepresentation = servicePartialRepresentations[0];
            Assert.IsNotNull(servicePartialRepresentation);
            Assert.AreEqual(savedServiceId, servicePartialRepresentation.Id);
            Assert.AreEqual(name, servicePartialRepresentation.Name);
            Assert.AreEqual(location, servicePartialRepresentation.Location);
            Assert.AreEqual(mobileNumber, servicePartialRepresentation.MobileNumber);
            Assert.AreEqual(serviceEmail, servicePartialRepresentation.ServiceEmail);
            Assert.AreEqual(serviceProfessionType, servicePartialRepresentation.ServiceProfessionType);
            Assert.AreEqual(serviceEntityType, servicePartialRepresentation.ServiceEntityType);
            Assert.AreEqual(facebookLink, servicePartialRepresentation.FacebookLink);
            Assert.AreEqual(twitterLink, servicePartialRepresentation.TwitterLink);
            Assert.AreEqual(instagramLink, servicePartialRepresentation.InstagramLink);
            Assert.AreEqual(websiteLink, servicePartialRepresentation.WebsiteLink);

            // Verify Service no 2
            servicePartialRepresentation = servicePartialRepresentations[1];
            Assert.IsNotNull(servicePartialRepresentation);
            Assert.AreEqual(savedServiceId2, servicePartialRepresentation.Id);
            Assert.AreEqual(name2, servicePartialRepresentation.Name);
            Assert.AreEqual(location2, servicePartialRepresentation.Location);
            Assert.AreEqual(mobileNumber2, servicePartialRepresentation.MobileNumber);
            Assert.AreEqual(serviceEmail2, servicePartialRepresentation.ServiceEmail);
            Assert.AreEqual(serviceProfessionType2, servicePartialRepresentation.ServiceProfessionType);
            Assert.AreEqual(serviceEntityType2, servicePartialRepresentation.ServiceEntityType);
            Assert.AreEqual(facebookLink2, servicePartialRepresentation.FacebookLink);
            Assert.AreEqual(twitterLink2, servicePartialRepresentation.TwitterLink);
            Assert.AreEqual(instagramLink2, servicePartialRepresentation.InstagramLink);
            Assert.AreEqual(websiteLink2, servicePartialRepresentation.WebsiteLink);
        }

        // Retrieves all of the Profession Types defined by the backend Domain Model
        [Test]
        public void GetAllProfessionTypesTest_ChecksIfTHeProfessionTypesAreAllReturnedAsExpected_VerifiesByTheRetrievedValue()
        {
            var kernel = InitiateLiveDependencies();

            var serviceController = kernel.Get<ServiceController>();
            var httpActionResult = serviceController.Get(getAllProfessionTypes:true);
            var httpResult = ((OkNegotiatedContentResult<IReadOnlyDictionary<string, IReadOnlyList<string>>>) httpActionResult).Content;
            Assert.IsNotNull(httpResult);
            // Count of the items in the Dictionary
            Assert.Greater(httpResult.Count, 9);
            // Count of the items in the List that is contained within the dictionary
            Assert.Greater(httpResult.First().Value.Count, 1);
        }

        // Checks the number of records fetched by location
        [Test]
        public void GetCountOfRecordsByLocationTest_ChecksIfNumberOfRecordsToUseForPaginationIsReturnedAsExpected_VerifiesByTheRetrievedValue()
        {
            // 4 Services with one Location that will requested, and 1 Location
            // that is different and will not be counted in theresult count
            var kernel = InitiateLiveDependencies();

            // Service that will be inserted multipe time
            var serviceController = kernel.Get<ServiceController>();
            string name = "The Stone Chopper";
            string description = "We make swrods so sharp and strong, they can chop stones";
            string location = "Pindora, Rawalpindi, Pakistan";
            string mobileNumber = "03770000000";
            string serviceEmail = "stone@chopper1234567.com";
            string uploaderEmail = "uploader@chopper1234567.com";
            string serviceProfessionType = "Plumber";
            string serviceEntityType = "Individual";

            var createServiceCommand = new CreateServiceCommand(name, description, location, mobileNumber,
                serviceEmail, uploaderEmail, serviceProfessionType, serviceEntityType, null,
                null, null, null, null);

            var location2 = "Lahore, Pakistan";
            var createServiceCommand2 = new CreateServiceCommand(name, description, location2, mobileNumber,
                serviceEmail, uploaderEmail, serviceProfessionType, serviceEntityType, null,
                null, null, null, null);
            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new service will fail
            serviceController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, uploaderEmail)
                })
            });
            // Save the Service multiple times so we have enough ercords to test
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            // Service with a different location
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand2));

            // Get the count of the Services by the given location.
            var httpActionResult = serviceController.Get(location: location, getCount: true);
            var recordsCount = ((OkNegotiatedContentResult<ServiceCountRepresentation>) httpActionResult).Content;
            Assert.AreEqual(4, recordsCount.RecordCount);
        }

        // Checks the number of records fetched by Profession type
        [Test]
        public void GetCountOfRecordsByProfessionTypeTest_ChecksIfNumberOfRecordsToUseForPaginationIsReturnedAsExpected_VerifiesByTheRetrievedValue()
        {
            // 4 Services with one ServiceProfessionType that will requested, and 1 ServiceProfessionType
            // that is different and will not be counted in theresult count
            var kernel = InitiateLiveDependencies();

            // Service that will be inserted multipe time
            var serviceController = kernel.Get<ServiceController>();
            string name = "The Stone Chopper";
            string description = "We make swrods so sharp and strong, they can chop stones";
            string location = "Pindora, Rawalpindi, Pakistan";
            string mobileNumber = "03770000000";
            string serviceEmail = "stone@chopper1234567.com";
            string uploaderEmail = "uploader@chopper1234567.com";
            string serviceProfessionType = "Plumber";
            string serviceEntityType = "Individual";

            var createServiceCommand = new CreateServiceCommand(name, description, location, mobileNumber,
                serviceEmail, uploaderEmail, serviceProfessionType, serviceEntityType, null,
                null, null, null, null);

            var serviceProfessionType2 = "Electrician";
            var createServiceCommand2 = new CreateServiceCommand(name, description, location, mobileNumber,
                serviceEmail, uploaderEmail, serviceProfessionType2, serviceEntityType, null,
                null, null, null, null);
            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new service will fail
            serviceController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, uploaderEmail)
                })
            });
            // Save the Service multiple times so we have enough records to test
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            // Service with a different Profession
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand2));

            // Get the count of the Services by the given profession.
            var httpActionResult = serviceController.Get(serviceProfessionType: serviceProfessionType, getCount: true);
            var recordsCount = ((OkNegotiatedContentResult<ServiceCountRepresentation>)httpActionResult).Content;
            Assert.AreEqual(4, recordsCount.RecordCount);
        }

        // Checks the number of records fetched by Profession and location. Profession is different in the 
        // used instances
        [Test]
        public void GetCountOfRecordsByProfessionAndLocationTest_ChecksIfNumberOfRecordsToUseForPaginationIsReturnedAsExpected_VerifiesByTheRetrievedValue()
        {
            // 4 Services with one profession and location that will be requested, and 1 service 
            // with a different profession only(same location) and will not be counted in the result count
            var kernel = InitiateLiveDependencies();

            // Service that will be inserted multipe time
            var serviceController = kernel.Get<ServiceController>();
            string name = "The Stone Chopper";
            string description = "We make swrods so sharp and strong, they can chop stones";
            string location = "Pindora, Rawalpindi, Pakistan";
            string mobileNumber = "03770000000";
            string serviceEmail = "stone@chopper1234567.com";
            string uploaderEmail = "uploader@chopper1234567.com";
            string serviceProfessionType = "Plumber";
            string serviceEntityType = "Individual";

            var createServiceCommand = new CreateServiceCommand(name, description, location, mobileNumber,
                serviceEmail, uploaderEmail, serviceProfessionType, serviceEntityType, null,
                null, null, null, null);

            var serviceProfessionType2 = "Electrician";
            var createServiceCommand2 = new CreateServiceCommand(name, description, location, mobileNumber,
                serviceEmail, uploaderEmail, serviceProfessionType2, serviceEntityType, null,
                null, null, null, null);
            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new service will fail
            serviceController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, uploaderEmail)
                })
            });
            // Save the Service multiple times so we have enough records to test
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            // Service with a different Profession
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand2));

            // Get the count of the Services by the given profession.
            var httpActionResult = serviceController.Get(
                serviceProfessionType: serviceProfessionType, location: location, getCount: true);
            var recordsCount = ((OkNegotiatedContentResult<ServiceCountRepresentation>)httpActionResult).Content;
            Assert.AreEqual(4, recordsCount.RecordCount);
        }

        // Checks the number of records fetched by Profession and location. Location is different in the 
        // used instances
        [Test]
        public void GetCountOfRecordsByProfessionAndLocationTest2_ChecksIfNumberOfRecordsToUseForPaginationIsReturnedAsExpected_VerifiesByTheRetrievedValue()
        {
            // 4 Services with one profession and location that will be requested, and 1 service 
            // with a different location only(same profession) and will not be counted in the result count
            var kernel = InitiateLiveDependencies();

            // Service that will be inserted multipe time
            var serviceController = kernel.Get<ServiceController>();
            string name = "The Stone Chopper";
            string description = "We make swrods so sharp and strong, they can chop stones";
            string location = "Pindora, Rawalpindi, Pakistan";
            string mobileNumber = "03770000000";
            string serviceEmail = "stone@chopper1234567.com";
            string uploaderEmail = "uploader@chopper1234567.com";
            string serviceProfessionType = "Plumber";
            string serviceEntityType = "Individual";

            var createServiceCommand = new CreateServiceCommand(name, description, location, mobileNumber,
                serviceEmail, uploaderEmail, serviceProfessionType, serviceEntityType, null,
                null, null, null, null);

            var location2 = "Lahore, Pakistan";
            var createServiceCommand2 = new CreateServiceCommand(name, description, location2, mobileNumber,
                serviceEmail, uploaderEmail, serviceProfessionType, serviceEntityType, null,
                null, null, null, null);
            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new service will fail
            serviceController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, uploaderEmail)
                })
            });
            // Save the Service multiple times so we have enough records to test
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            // Service with a different Profession
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand2));
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand2));

            // Get the count of the Services by the given profession.
            var httpActionResult = serviceController.Get(
                serviceProfessionType: serviceProfessionType, location: location, getCount: true);
            var recordsCount = ((OkNegotiatedContentResult<ServiceCountRepresentation>)httpActionResult).Content;
            Assert.AreEqual(3, recordsCount.RecordCount);
        }

        // Get the record count for all the services present in the database
        [Test]
        public void GetCountOfRecordsOfAllServicesTest_ChecksIfNumberOfRecordsToUseForPaginationIsReturnedAsExpected_VerifiesByTheRetrievedValue()
        {
            // 4 Services with one profession and location that will be requested, and 1 service 
            // with a different location only(same profession)
            var kernel = InitiateLiveDependencies();

            // Service that will be inserted multipe time
            var serviceController = kernel.Get<ServiceController>();
            string name = "The Stone Chopper";
            string description = "We make swrods so sharp and strong, they can chop stones";
            string location = "Pindora, Rawalpindi, Pakistan";
            string mobileNumber = "03770000000";
            string serviceEmail = "stone@chopper1234567.com";
            string uploaderEmail = "uploader@chopper1234567.com";
            string serviceProfessionType = "Plumber";
            string serviceEntityType = "Individual";

            var createServiceCommand = new CreateServiceCommand(name, description, location, mobileNumber,
                serviceEmail, uploaderEmail, serviceProfessionType, serviceEntityType, null,
                null, null, null, null);

            var location2 = "Lahore, Pakistan";
            var createServiceCommand2 = new CreateServiceCommand(name, description, location2, mobileNumber,
                serviceEmail, uploaderEmail, serviceProfessionType, serviceEntityType, null,
                null, null, null, null);
            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new service will fail
            serviceController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, uploaderEmail)
                })
            });
            // Save the Service multiple times so we have enough records to test
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand));
            // Service with a different Profession
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand2));
            serviceController.Post(JsonConvert.SerializeObject(createServiceCommand2));

            // Get the count of the Services by the given profession.
            var httpActionResult = serviceController.Get(getCount: true);
            var recordsCount = ((OkNegotiatedContentResult<ServiceCountRepresentation>)httpActionResult).Content;
            Assert.AreEqual(5, recordsCount.RecordCount);
        }
    }
}
