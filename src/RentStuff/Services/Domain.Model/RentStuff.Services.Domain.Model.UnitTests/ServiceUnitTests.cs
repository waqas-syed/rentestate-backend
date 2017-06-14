﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RentStuff.Services.Domain.Model.ServiceAggregate;

namespace RentStuff.Services.Domain.Model.UnitTests
{
    [TestFixture]
    public class ServiceUnitTests
    {
        #region Service Tests

        // Successful instance creation with all the fields; mandatory and non-mandatory
        [Test]
        public void SuccessfulInstanceWithAllFieldsCreation_ChecksThatAnInstanceIsCreatedSuccessfully_VerifiesThroughReturnedValue()
        {
            string name = "Black Smith Inc";
            string description = "We create the worlds best swords, spears and hammers!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "00000000001";
            string serviceEmail = "smithy@smithereene1234567.com";
            string uploaderEmail = "provider@smithereene1234567.com";

            // Get a profession somewhere from the middle of the dictionary
            IReadOnlyList<string> vehicleProfession;
            Service.GetProfessionsList().TryGetValue("Vehicle Services", out vehicleProfession);
            Assert.IsNotNull(vehicleProfession);
            string serviceProfessionType = vehicleProfession[3];
            string serviceEntityType = ServiceEntityType.Organization.ToString();
            DateTime dateEstablished = DateTime.Today.AddYears(-101);
            string facebookLink = "https://dummyfacebooklink-123456789-1.com";
            string instagramLink = "https://dummyinstagramlink-123456789-1.com";
            string twitterLink = "https://dummytwitterlink-123456789-1.com";
            string websiteLink = "https://dummywebsitelink-123456789-1.com";
            decimal latitude = 33.7M;
            decimal longitude = 73.1M;
            string secondaryMobileNumber = "00000000002";
            string landlinePhoneNumber = "0000000001";
            string fax = "0000000003";
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProfessionType)
                .ServiceEntityType(serviceEntityType).DateEstablished(dateEstablished)
                .Latitude(latitude).Longitude(longitude).FacebookLink(facebookLink)
                .InstagramLink(instagramLink).TwitterLink(twitterLink).WebsiteLink(websiteLink)
                .SecondaryMobileNumber(secondaryMobileNumber).LandlinePhoneNumber(landlinePhoneNumber)
                .Fax(fax)
                .Build();

            Assert.IsNotNull(service);
            Assert.AreEqual(name, service.Name);
            Assert.AreEqual(description, service.Description);
            Assert.AreEqual(location, service.Location);
            Assert.AreEqual(phoneNumber, service.MobileNumber);
            Assert.AreEqual(serviceEmail, service.ServiceEmail);
            Assert.AreEqual(uploaderEmail, service.UploaderEmail);
            Assert.AreEqual(serviceProfessionType, service.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), serviceEntityType),
                            service.ServiceEntityType);
            Assert.AreEqual(dateEstablished, service.DateEstablished);
            Assert.AreEqual(latitude, service.Latitude);
            Assert.AreEqual(longitude, service.Longitude);
            Assert.AreEqual(facebookLink, service.FacebookLink);
            Assert.AreEqual(instagramLink, service.InstagramLink);
            Assert.AreEqual(twitterLink, service.TwitterLink);
            Assert.AreEqual(websiteLink, service.WebsiteLink);
            Assert.AreEqual(secondaryMobileNumber, service.SecondaryMobileNumber);
            Assert.AreEqual(landlinePhoneNumber, service.LandlinePhoneNumber);
            Assert.AreEqual(fax, service.Fax);
        }

        // Successful instance creation with all mandatory fields, excluding non-mandatory ones
        [Test]
        public void SuccessfulInstanceCreation_ChecksThatAnInstanceIsCreatedSuccessfully_VerifiesThroughReturnedValue()
        {
            string name = "Black Smith Inc";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string uploaderEmail = "provider@smithereene1234567.com";
            string serviceProfessionType = Service.GetProfessionsList().First().Value.First();
            string serviceEntityType = ServiceEntityType.Organization.ToString();
            decimal latitude = 33.7M;
            decimal longitude = 73.1M;
            Service service = new Service.ServiceBuilder().Name(name)
                .Location(location).PhoneNumber(phoneNumber)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProfessionType)
                .ServiceEntityType(serviceEntityType)
                .Latitude(latitude).Longitude(longitude).Build();

            Assert.IsNotNull(service);
            Assert.AreEqual(name, service.Name);
            Assert.AreEqual(location, service.Location);
            Assert.AreEqual(phoneNumber, service.MobileNumber);
            Assert.AreEqual(uploaderEmail, service.UploaderEmail);
            Assert.AreEqual(serviceProfessionType, service.ServiceProfessionType);
            Assert.AreEqual((ServiceEntityType)Enum.Parse(typeof(ServiceEntityType), serviceEntityType),
                            service.ServiceEntityType);
            Assert.AreEqual(latitude, service.Latitude);
            Assert.AreEqual(longitude, service.Longitude);
        }

        // DateEstablished assigned as null test
        [Test]
        public void ServiceSaveWithDateEstablishedAssignedNullTest_ChecksIfTheServiceInstanceisBuiltAsExpectedWhenTheDateTimeIsNull_VerifiesByTheReturnedValue()
        {
            string name = "Black Smith Inc";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string uploaderEmail = "provider@smithereene1234567.com";
            string serviceProfessionType = Service.GetProfessionsList().First().Value.First();
            string serviceEntityType = ServiceEntityType.Organization.ToString();
            decimal latitude = 33.7M;
            decimal longitude = 73.1M;
            Service service = new Service.ServiceBuilder().Name(name)
                .Location(location).PhoneNumber(phoneNumber)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProfessionType)
                .ServiceEntityType(serviceEntityType)
                .Latitude(latitude).Longitude(longitude).DateEstablished(null).Build();

            Assert.IsNotNull(service);
        }

        public void
            ServiceImagesSavAndDeleteTest_ChecksIfImagesAerSavedAndDeletedAsExpected_VerifiesThroughTheReturnedValue()
        {
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
            
            // Check the Service and its images are populated
            Assert.IsNotNull(service);
            Assert.NotNull(service.Images);
            Assert.AreEqual(3, service.Images.Count);
            Assert.AreEqual(image1Id, service.Images[0]);
            Assert.AreEqual(image2Id, service.Images[1]);
            Assert.AreEqual(image3Id, service.Images[2]);

            // Delete an image from the collection and check if the results reflect that
            service.DeleteImage(image2Id);
            Assert.AreEqual(2, service.Images.Count);
            Assert.AreEqual(image1Id, service.Images[0]);
            Assert.AreEqual(image3Id, service.Images[1]);

            // Now provide an image id that doesn't exist in the Images List. image count should be the same
            service.DeleteImage(image3Id + "1");
            Assert.AreEqual(2, service.Images.Count);
            Assert.AreEqual(image1Id, service.Images[0]);
            Assert.AreEqual(image3Id, service.Images[1]);
        }
        
        // No name provided
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ServiceInstanceCreationNoNameFailTest_ChecksThatAnInstanceIsNotCreatedSuccessfully_VerifiesThroughReturnedValue()
        {
            string description = "We create the worlds best swords, spears and hammers!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string serviceEmail = "smithy@smithereene1234567.com";
            string uploaderEmail = "provider@smithereene1234567.com";
            string serviceProfessionType = Service.GetProfessionsList().First().Value.First();
            string serviceEntityType = ServiceEntityType.Organization.ToString();
            DateTime dateEstablished = DateTime.Today.AddYears(-101);
            decimal latitude = 33.7M;
            decimal longitude = 73.1M;
            Service service = new Service.ServiceBuilder().Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProfessionType)
                .ServiceEntityType(serviceEntityType).DateEstablished(dateEstablished)
                .Latitude(latitude).Longitude(longitude).Build();
        }

        // No Location Provided
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ServiceInstanceCreationNoLocationFailTest_ChecksThatAnInstanceIsNotCreatedSuccessfully_VerifiesThroughReturnedValue()
        {
            string name = "Black Smith Inc";
            string description = "We create the worlds best swords, spears and hammers!";
            string phoneNumber = "03455138018";
            string serviceEmail = "smithy@smithereene1234567.com";
            string uploaderEmail = "provider@smithereene1234567.com";
            string serviceProfessionType = Service.GetProfessionsList().First().Value.First();
            string serviceEntityType = ServiceEntityType.Organization.ToString();
            DateTime dateEstablished = DateTime.Today.AddYears(-101);
            decimal latitude = 33.7M;
            decimal longitude = 73.1M;
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProfessionType)
                .ServiceEntityType(serviceEntityType).DateEstablished(dateEstablished)
                .Latitude(latitude).Longitude(longitude).Build();
        }

        // No Phone Number Provided
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ServiceInstanceCreationNoPhoneNumberFailTest_ChecksThatAnInstanceIsNotCreatedSuccessfully_VerifiesThroughReturnedValue()
        {
            string name = "Black Smith Inc";
            string description = "We create the worlds best swords, spears and hammers!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string serviceEmail = "smithy@smithereene1234567.com";
            string uploaderEmail = "provider@smithereene1234567.com";
            string serviceProfessionType = Service.GetProfessionsList().First().Value.First();
            string serviceEntityType = ServiceEntityType.Organization.ToString();
            DateTime dateEstablished = DateTime.Today.AddYears(-101);
            decimal latitude = 33.7M;
            decimal longitude = 73.1M;
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProfessionType)
                .ServiceEntityType(serviceEntityType).DateEstablished(dateEstablished)
                .Latitude(latitude).Longitude(longitude).Build();
        }

        // No UploaderEmail Provided
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ServiceInstanceCreationNoEmailFailTest_ChecksThatAnInstanceIsNotCreatedSuccessfully_VerifiesThroughReturnedValue()
        {
            string name = "Black Smith Inc";
            string description = "We create the worlds best swords, spears and hammers!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string serviceEmail = "smithy@smithereene1234567.com";
            string serviceProfessionType = Service.GetProfessionsList().First().Value.First();
            string serviceEntityType = ServiceEntityType.Organization.ToString();
            DateTime dateEstablished = DateTime.Today.AddYears(-101);
            decimal latitude = 33.7M;
            decimal longitude = 73.1M;
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .ServiceProfessionType(serviceProfessionType)
                .ServiceEntityType(serviceEntityType).DateEstablished(dateEstablished)
                .Latitude(latitude).Longitude(longitude).Build();
        }

        // No ServiceProfessionType provided
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ServiceInstanceCreationNoServiceProviderTypeFailTest_ChecksThatAnInstanceIsNotCreatedSuccessfully_VerifiesThroughReturnedValue()
        {
            string name = "Black Smith Inc";
            string description = "We create the worlds best swords, spears and hammers!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string serviceEmail = "smithy@smithereene1234567.com";
            string uploaderEmail = "provider@smithereene1234567.com";
            string serviceEntityType = ServiceEntityType.Organization.ToString();
            DateTime dateEstablished = DateTime.Today.AddYears(-101);
            decimal latitude = 33.7M;
            decimal longitude = 73.1M;
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail)
                .ServiceEntityType(serviceEntityType).DateEstablished(dateEstablished)
                .Latitude(latitude).Longitude(longitude).Build();
        }

        // No ServiceEntityType provided
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ServiceInstanceCreationNoServiceEntityTypeFailTest_ChecksThatAnInstanceIsNotCreatedSuccessfully_VerifiesThroughReturnedValue()
        {
            string name = "Black Smith Inc";
            string description = "We create the worlds best swords, spears and hammers!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string serviceEmail = "smithy@smithereene1234567.com";
            string uploaderEmail = "provider@smithereene1234567.com";
            string serviceProfessionType = Service.GetProfessionsList().First().Value.First();
            DateTime dateEstablished = DateTime.Today.AddYears(-101);
            decimal latitude = 33.7M;
            decimal longitude = 73.1M;
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProfessionType)
                .DateEstablished(dateEstablished)
                .Latitude(latitude).Longitude(longitude).Build();
        }

        // No Latitude provided
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ServiceInstanceCreationNoLatitudeFailTest_ChecksThatAnInstanceIsNotCreatedSuccessfully_VerifiesThroughReturnedValue()
        {
            string name = "Black Smith Inc";
            string description = "We create the worlds best swords, spears and hammers!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string serviceEmail = "smithy@smithereene1234567.com";
            string uploaderEmail = "provider@smithereene1234567.com";
            string serviceProfessionType = Service.GetProfessionsList().First().Value.First();
            string serviceEntityType = ServiceEntityType.Organization.ToString();
            DateTime dateEstablished = DateTime.Today.AddYears(-101);
            decimal longitude = 73.1M;
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProfessionType)
                .ServiceEntityType(serviceEntityType).DateEstablished(dateEstablished)
                .Longitude(longitude).Build();
        }

        // No Longitude provided
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ServiceInstanceCreationNoLongitudeFailTest_ChecksThatAnInstanceIsNotCreatedSuccessfully_VerifiesThroughReturnedValue()
        {
            string name = "Black Smith Inc";
            string description = "We create the worlds best swords, spears and hammers!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string serviceEmail = "smithy@smithereene1234567.com";
            string uploaderEmail = "provider@smithereene1234567.com";
            string serviceProfessionType = Service.GetProfessionsList().First().Value.First();
            string serviceEntityType = ServiceEntityType.Organization.ToString();
            DateTime dateEstablished = DateTime.Today.AddYears(-101);
            decimal latitude = 33.7M;
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProfessionType)
                .ServiceEntityType(serviceEntityType).DateEstablished(dateEstablished)
                .Latitude(latitude).Build();
        }

        //We provide a false profession to the service builder and the service will raise exception a result
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WrongServiceProfessionTypeFailTest_ChecksThatTheSystemThrowsErrorIfAnNonRecognizableProfessionisSupplied_VerifiesThroughRasedException()
        {
            string name = "Black Smith Inc";
            string description = "We create the worlds best swords, spears and hammers!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string serviceEmail = "smithy@smithereene1234567.com";
            string uploaderEmail = "provider@smithereene1234567.com";
            // Provide a false profession here by adding just a digit to an existing profession
            string serviceProfessionType = Service.GetProfessionsList().First().Value.First() + "1";
            string serviceEntityType = ServiceEntityType.Organization.ToString();
            DateTime dateEstablished = DateTime.Today.AddYears(-101);
            decimal latitude = 33.7M;
            decimal longitude = 73.1M;
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProfessionType)
                .ServiceEntityType(serviceEntityType).DateEstablished(dateEstablished)
                .Latitude(latitude).Longitude(longitude).Build();
        }

        #endregion Service Tests

        #region Reviews Tests

        [Test]
        public void ReviewAdditionTest_ChecksIfTheExpectedResultIsAsExpected_VerifiesByTheReturnValue()
        {
            string name = "Black Smith Inc";
            string description = "We create the worlds best swords, spears and hammers!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string serviceEmail = "smithy@smithereene1234567.com";
            string uploaderEmail = "provider@smithereene1234567.com";
            string serviceProfessionType = Service.GetProfessionsList().First().Value.First();
            string serviceEntityType = ServiceEntityType.Organization.ToString();
            DateTime dateEstablished = DateTime.Today.AddYears(-101);
            decimal latitude = 33.7M;
            decimal longitude = 73.1M;
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProfessionType)
                .ServiceEntityType(serviceEntityType).DateEstablished(dateEstablished)
                .Latitude(latitude).Longitude(longitude).Build();
            
            // Initial Count of reviews
            Assert.AreEqual(0, service.Reviews.Count);

            string authorName = "Bailin Of Ibelin";
            string authorEmail = "bailin@ofibelin1234567.com";
            string reviewDescription = "I have purchased some swords from this place and found them " +
                                       "to be quite balanced, sharp and agile. I cannot recommend any other" +
                                       " place more";
            
            service.AddReview(authorName, authorEmail, reviewDescription);

            string authorName2 = "Bailin Of Ibelin 2";
            string authorEmail2 = "bailin2@ofibelin1234567.com";
            string reviewDescription2 = "My second review for them. I have purchased some swords from this " +
                                       "place and found them to be quite balanced, sharp and agile. I cannot " +
                                       "recommend any other place more";
            service.AddReview(authorName2, authorEmail2, reviewDescription2);

            // 2 reviews have been added
            Assert.AreEqual(2, service.Reviews.Count);

            // First review
            Assert.AreEqual(authorName, service.Reviews[0].Authorname);
            Assert.AreEqual(authorName, service.Reviews[0].Authorname);
            Assert.AreEqual(reviewDescription, service.Reviews[0].ReviewDescription);

            // Second review
            Assert.AreEqual(authorName2, service.Reviews[1].Authorname);
            Assert.AreEqual(authorName2, service.Reviews[1].Authorname);
            Assert.AreEqual(reviewDescription2, service.Reviews[1].ReviewDescription);
        }
        
        #endregion Reviews Tests

        // Ratings code: commented out but let it stay for possible future reference
        #region Ratings Tests

        // A few 5 star ratings initially, then a few 4 star ratings, followed by many 5 star ratings
        /*[Test]
        public void RatingCalculationTest1_ChecksIfTheExpectedResultIsAsExpected_VerifiesByTheReturnValue()
        {
            string name = "Black Smith Inc";
            string description = "We create the worlds best swords, spears and hammers!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string serviceEmail = "smithy@smithereene1234567.com";
            string uploaderEmail = "provider@smithereene1234567.com";
            string serviceProviderType = ServiceProfessions.List.First();
            string serviceEntityType = ServiceEntityType.Organization.ToString();
            DateTime dateEstablished = DateTime.Today.AddYears(-101);
            decimal latitude = 33.7M;
            decimal longitude = 73.1M;
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProviderType)
                .ServiceEntityType(serviceEntityType).DateEstablished(dateEstablished)
                .Latitude(latitude).Longitude(longitude).Build();

            // Initial Rating is 0
            Assert.AreEqual(0, service.Ratings.RatingStars);

            // One rating of 5
            service.AddNewRating(5);
            Assert.AreEqual(5, service.Ratings.RatingStars);

            // Another Rating of 5
            service.AddNewRating(5);
            Assert.AreEqual(5, service.Ratings.RatingStars);

            // Lightning struck thrice
            service.AddNewRating(5);
            Assert.AreEqual(5, service.Ratings.RatingStars);

            // First 4 star rating. Two more will follow
            service.AddNewRating(4);
            Assert.AreEqual(4.8, service.Ratings.RatingStars);

            service.AddNewRating(4);
            Assert.AreEqual(4.6, service.Ratings.RatingStars);

            service.AddNewRating(4);
            Assert.AreEqual(4.5, service.Ratings.RatingStars);

            // Started getting the 5 star ratings again
            service.AddNewRating(5);
            Assert.AreEqual(4.6, service.Ratings.RatingStars);

            service.AddNewRating(5);
            Assert.AreEqual(4.6, service.Ratings.RatingStars);

            service.AddNewRating(5);
            Assert.AreEqual(4.7, service.Ratings.RatingStars);

            service.AddNewRating(5);
            Assert.AreEqual(4.7, service.Ratings.RatingStars);

            service.AddNewRating(5);
            Assert.AreEqual(4.7, service.Ratings.RatingStars);

            service.AddNewRating(5);
            Assert.AreEqual(4.8, service.Ratings.RatingStars);

            service.AddNewRating(5);
            Assert.AreEqual(4.8, service.Ratings.RatingStars);

            service.AddNewRating(5);
            Assert.AreEqual(4.8, service.Ratings.RatingStars);

            service.AddNewRating(5);
            Assert.AreEqual(4.8, service.Ratings.RatingStars);

            service.AddNewRating(5);
            Assert.AreEqual(4.8, service.Ratings.RatingStars);

            service.AddNewRating(5);
            Assert.AreEqual(4.8, service.Ratings.RatingStars);

            service.AddNewRating(5);
            Assert.AreEqual(4.8, service.Ratings.RatingStars);

            // It's not possible to go to 5 once a lower rating star has been given
            service.AddNewRating(5);
            Assert.AreEqual(4.8, service.Ratings.RatingStars);
        }

        // Random ratings
        [Test]
        public void RatingCalculationTest2_ChecksIfTheExpectedResultIsAsExpected_VerifiesByTheReturnValue()
        {
            string name = "Black Smith Inc";
            string description = "We create the worlds best swords, spears and hammers!";
            string location = "Pindora, Rawalpindi, Pakistan";
            string phoneNumber = "03455138018";
            string serviceEmail = "smithy@smithereene1234567.com";
            string uploaderEmail = "provider@smithereene1234567.com";
            string serviceProviderType = ServiceProfessions.List.First();
            string serviceEntityType = ServiceEntityType.Organization.ToString();
            DateTime dateEstablished = DateTime.Today.AddYears(-101);
            decimal latitude = 33.7M;
            decimal longitude = 73.1M;
            Service service = new Service.ServiceBuilder().Name(name).Description(description)
                .Location(location).PhoneNumber(phoneNumber).ServiceEmail(serviceEmail)
                .UploaderEmail(uploaderEmail).ServiceProfessionType(serviceProviderType)
                .ServiceEntityType(serviceEntityType).DateEstablished(dateEstablished)
                .Latitude(latitude).Longitude(longitude).Build();

            // Initial Rating is 0
            Assert.AreEqual(0, service.Ratings.RatingStars);

            // One rating of 5
            service.AddNewRating(5);
            Assert.AreEqual(5, service.Ratings.RatingStars);

            // First 4 star rating. Two more will follow
            service.AddNewRating(1);
            Assert.AreEqual(3, service.Ratings.RatingStars);

            service.AddNewRating(5);
            Assert.AreEqual(3.7, service.Ratings.RatingStars);

            service.AddNewRating(5);
            Assert.AreEqual(4, service.Ratings.RatingStars);

            // Started getting the 5 star ratings again
            service.AddNewRating(5);
            Assert.AreEqual(4.2, service.Ratings.RatingStars);

            service.AddNewRating(5);
            Assert.AreEqual(4.3, service.Ratings.RatingStars);

            service.AddNewRating(4);
            Assert.AreEqual(4.3, service.Ratings.RatingStars);

            service.AddNewRating(4);
            Assert.AreEqual(4.2, service.Ratings.RatingStars);

            service.AddNewRating(5);
            Assert.AreEqual(4.3, service.Ratings.RatingStars);

            service.AddNewRating(5);
            Assert.AreEqual(4.4, service.Ratings.RatingStars);

            service.AddNewRating(2);
            Assert.AreEqual(4.2, service.Ratings.RatingStars);

            service.AddNewRating(3);
            Assert.AreEqual(4.1, service.Ratings.RatingStars);
        }*/

        #endregion Ratings Tests
    }
}
