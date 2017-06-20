using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Results;
using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Newtonsoft.Json;
using Ninject;
using NUnit.Framework;
using RentStuff.Common;
using RentStuff.Common.Ninject.Modules;
using RentStuff.Common.Utilities;
using RentStuff.Property.Application.HouseServices.Commands;
using RentStuff.Property.Application.HouseServices.Representation;
using RentStuff.Property.Application.Ninject.Modules;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Infrastructure.Persistence.Ninject.Modules;
using RentStuff.Property.Ports.Adapter.Rest.Ninject.Modules;
using RentStuff.Property.Ports.Adapter.Rest.Resources;

namespace RentStuff.Property.Ports.Tests
{
    [TestFixture]
    public class HouseControllerTests
    {
        // Flag which allows tests to run or not, as these tests call Google APIs which has a threshold limie. So we want to be sure
        // that we are not exhausting resources by unknowingly running such tests
        private bool _runTests = true;
        private DatabaseUtility _databaseUtility;
        private StandardKernel _kernel;

        [SetUp]
        public void Setup()
        {
            var connection = StringCipher.DecipheredConnectionString;
            _databaseUtility = new DatabaseUtility(connection);
            _databaseUtility.Create();

            _kernel = new StandardKernel();
            _kernel.Load<PropertyPersistenceNinjectModule>();
            _kernel.Load<CommonNinjectModule>();
            _kernel.Load<PropertyApplicationNinjectModule>();
            _kernel.Load<PropertyPortsNinjectModule>();
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
        /// Shows the SQL output in the Output window.
        /// </summary>
        private void ShowNhibernateLogging()
        {
            var hierarchy = (Hierarchy)LogManager.GetRepository();
            var logger = (Logger)hierarchy.GetLogger("NHibernate.SQL");
            logger.AddAppender(new TraceAppender() { Layout = new SimpleLayout() });
            hierarchy.Configured = true;
        }

        [Category("Integration")]
        [Test]
        public void SaveUpdateAndGetHouseInstanceByIdTest_TestsThatHouseIsSavedThenUpdatedAndRetreivedAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                HouseController houseController = _kernel.Get<HouseController>();
                Assert.NotNull(houseController);

                string title = "House For Rent";
                string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
                int rent = 105000;
                string ownerEmail = "thorin@oakenshield123.com";
                string ownerPhoneNumber = "01234567890";
                string houseNo = "CT-141/A";
                string streetNo = "14";
                int numberOfBathrooms = 1;
                int numberOfBedrooms = 1;
                int numberOfKitchens = 1;
                bool familiesOnly = false;
                bool boysOnly = false;
                bool girlsOnly = true;
                bool internetAvailable = true;
                bool landlinePhoneAvailable = true;
                bool cableTvAvailable = true;
                bool garageAvailable = true;
                bool smokingAllowed = true;
                string propertyType = PropertyType.Apartment.ToString();
                string area = "Pindora, Rawalpindi, Pakistan";
                //string area = "Pindora, Rawalpindi, Pakistan";
                string dimensionType = DimensionType.Kanal.ToString();
                string dimensionString = "5";
                decimal dimensionDecimal = 0;
                string ownerName = "Owner Name 1";
                string genderRestriction = GenderRestriction.FamiliesOnly.ToString();

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail)
                    })
                });
                CreateHouseCommand house = new CreateHouseCommand(title, rent,numberOfBedrooms, numberOfKitchens, 
                    numberOfBathrooms, internetAvailable, landlinePhoneAvailable,
                    cableTvAvailable, garageAvailable, smokingAllowed, propertyType, ownerEmail, ownerPhoneNumber, 
                    houseNo, streetNo, area, dimensionType, dimensionString, dimensionDecimal, ownerName, description, genderRestriction);
                IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(house));
                string houseId = ((OkNegotiatedContentResult<string>) houseSaveResult).Content;
                
                IHttpActionResult response = (IHttpActionResult)houseController.GetHouse(houseId:houseId);
                HouseFullRepresentation retreivedHouse = ((OkNegotiatedContentResult<HouseFullRepresentation>) response).Content;
                Assert.NotNull(retreivedHouse);
                Assert.AreEqual(houseId, retreivedHouse.Id);
                Assert.AreEqual(title, retreivedHouse.Title);
                Assert.AreEqual(house.Title, retreivedHouse.Title);
                Assert.AreEqual(description, retreivedHouse.Description);
                Assert.AreEqual(house.Description, retreivedHouse.Description);
                Assert.AreEqual(rent, retreivedHouse.MonthlyRent);
                Assert.AreEqual(house.MonthlyRent, retreivedHouse.MonthlyRent);
                Assert.AreEqual(numberOfBathrooms, retreivedHouse.NumberOfBathrooms);
                Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
                Assert.AreEqual(numberOfBedrooms, retreivedHouse.NumberOfBedrooms);
                Assert.AreEqual(house.NumberOfBedrooms, retreivedHouse.NumberOfBedrooms);
                Assert.AreEqual(numberOfKitchens, retreivedHouse.NumberOfKitchens);
                Assert.AreEqual(house.NumberOfKitchens, retreivedHouse.NumberOfKitchens);
                Assert.AreEqual(landlinePhoneAvailable, retreivedHouse.LandlinePhoneAvailable);
                Assert.AreEqual(house.LandlinePhoneAvailable, retreivedHouse.LandlinePhoneAvailable);
                Assert.AreEqual(garageAvailable, retreivedHouse.GarageAvailable);
                Assert.AreEqual(house.GarageAvailable, retreivedHouse.GarageAvailable);
                Assert.AreEqual(smokingAllowed, retreivedHouse.SmokingAllowed);
                Assert.AreEqual(house.SmokingAllowed, retreivedHouse.SmokingAllowed);
                Assert.AreEqual(internetAvailable, retreivedHouse.InternetAvailable);
                Assert.AreEqual(house.InternetAvailable, retreivedHouse.InternetAvailable);                
                Assert.AreEqual(propertyType, retreivedHouse.PropertyType);
                Assert.AreEqual(house.PropertyType, retreivedHouse.PropertyType);
                Assert.AreEqual(genderRestriction, retreivedHouse.GenderRestriction);
                Assert.AreEqual(house.GenderRestriction, retreivedHouse.GenderRestriction);
                Assert.AreEqual(genderRestriction, retreivedHouse.GenderRestriction);
                Assert.AreEqual(house.GenderRestriction, retreivedHouse.GenderRestriction);

                // Get the coordinates to verify we have stored the correct ones
                RentStuff.Common.Services.LocationServices.IGeocodingService geocodingService = _kernel.Get<RentStuff.Common.Services.LocationServices.IGeocodingService>();
                var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(area);
                Assert.AreEqual(coordinatesFromAddress.Item1, retreivedHouse.Latitude);
                Assert.AreEqual(coordinatesFromAddress.Item2, retreivedHouse.Longitude);

                Assert.AreEqual(houseNo, retreivedHouse.HouseNo);
                Assert.AreEqual(house.HouseNo, retreivedHouse.HouseNo);
                Assert.AreEqual(area, retreivedHouse.Area);
                Assert.AreEqual(house.Area, retreivedHouse.Area);
                Assert.AreEqual(streetNo, retreivedHouse.StreetNo);
                Assert.AreEqual(house.StreetNo, retreivedHouse.StreetNo);
                Assert.AreEqual(ownerName, retreivedHouse.OwnerName);
                Assert.AreEqual(house.OwnerName, retreivedHouse.OwnerName);

                string updatedTitle = "Updated House For Rent";
                string updatedDescription = "updated Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
                int updatedRent = 195000;
                string updatedOwnerEmail = "thorin@oakenshield123.com";
                string updatedOwnerPhoneNumber = "01234567890";
                string updatedHouseNo = "CT-141/A";
                string updatedStreetNo = "14";
                int updatedNumberOfBathrooms = 9;
                int updatedNumberOfBedrooms = 8;
                int updatedNumberOfKitchens = 7;
                bool updatedInternetAvailable = true;
                bool updatedLandlinePhoneAvailable = true;
                bool updatedCableTvAvailable = true;
                bool updatedGarageAvailable = true;
                bool updatedSmokingAllowed = true;
                string updatedPropertyType = PropertyType.Apartment.ToString();
                string updatedArea = "Saddar, Rawalpindi, Pakistan";
                //string area = "Pindora, Rawalpindi, Pakistan";
                string updatedDimensionType = DimensionType.Kanal.ToString();
                string updatedDimensionString = "5";
                decimal updatedDimensionDecimal = 0;
                string updatedOwnerName = "Owner Name 1";
                string updatedGenderRestriction = GenderRestriction.BoysOnly.ToString();

                UpdateHouseCommand updateHouseCommand = new UpdateHouseCommand(houseId, updatedTitle, updatedRent, updatedNumberOfBedrooms,
                    updatedNumberOfKitchens, updatedNumberOfBathrooms, updatedInternetAvailable, updatedLandlinePhoneAvailable,
                    updatedCableTvAvailable, updatedGarageAvailable, updatedSmokingAllowed, updatedPropertyType, updatedOwnerEmail,
                    updatedOwnerPhoneNumber, updatedHouseNo, updatedStreetNo, updatedArea, updatedDimensionType, updatedDimensionString,
                    updatedDimensionDecimal, updatedOwnerName, updatedDescription, updatedGenderRestriction);

                IHttpActionResult houseUpdateResult = houseController.Put(JsonConvert.SerializeObject(updateHouseCommand));
                response = (IHttpActionResult)houseController.GetHouse(houseId: houseId);
                retreivedHouse = ((OkNegotiatedContentResult<HouseFullRepresentation>)response).Content;
                Assert.NotNull(retreivedHouse);
                Assert.AreEqual(houseId, retreivedHouse.Id);
                Assert.AreEqual(updatedTitle, retreivedHouse.Title);
                Assert.AreEqual(updateHouseCommand.Title, retreivedHouse.Title);
                Assert.AreEqual(updatedDescription, retreivedHouse.Description);
                Assert.AreEqual(updateHouseCommand.Description, retreivedHouse.Description);
                Assert.AreEqual(updatedRent, retreivedHouse.MonthlyRent);
                Assert.AreEqual(updateHouseCommand.MonthlyRent, retreivedHouse.MonthlyRent);
                Assert.AreEqual(updatedNumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
                Assert.AreEqual(updateHouseCommand.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
                Assert.AreEqual(updatedNumberOfBedrooms, retreivedHouse.NumberOfBedrooms);
                Assert.AreEqual(updateHouseCommand.NumberOfBedrooms, retreivedHouse.NumberOfBedrooms);
                Assert.AreEqual(updatedNumberOfKitchens, retreivedHouse.NumberOfKitchens);
                Assert.AreEqual(updateHouseCommand.NumberOfKitchens, retreivedHouse.NumberOfKitchens);
                Assert.AreEqual(updatedLandlinePhoneAvailable, retreivedHouse.LandlinePhoneAvailable);
                Assert.AreEqual(updateHouseCommand.LandlinePhoneAvailable, retreivedHouse.LandlinePhoneAvailable);
                Assert.AreEqual(updatedGarageAvailable, retreivedHouse.GarageAvailable);
                Assert.AreEqual(updateHouseCommand.GarageAvailable, retreivedHouse.GarageAvailable);
                Assert.AreEqual(updatedSmokingAllowed, retreivedHouse.SmokingAllowed);
                Assert.AreEqual(updateHouseCommand.SmokingAllowed, retreivedHouse.SmokingAllowed);
                Assert.AreEqual(updatedInternetAvailable, retreivedHouse.InternetAvailable);
                Assert.AreEqual(updateHouseCommand.InternetAvailable, retreivedHouse.InternetAvailable);
                Assert.AreEqual(updatedPropertyType, retreivedHouse.PropertyType);
                Assert.AreEqual(updateHouseCommand.PropertyType, retreivedHouse.PropertyType);
                Assert.AreEqual(updatedGenderRestriction, retreivedHouse.GenderRestriction);
                Assert.AreEqual(updateHouseCommand.GenderRestriction, retreivedHouse.GenderRestriction);

                // Get the coordinates to verify we have stored the correct ones
                geocodingService = _kernel.Get< RentStuff.Common.Services.LocationServices.IGeocodingService> ();
                coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(updatedArea);
                Assert.AreEqual(coordinatesFromAddress.Item1, retreivedHouse.Latitude);
                Assert.AreEqual(coordinatesFromAddress.Item2, retreivedHouse.Longitude);

                Assert.AreEqual(updatedHouseNo, retreivedHouse.HouseNo);
                Assert.AreEqual(updateHouseCommand.HouseNo, retreivedHouse.HouseNo);
                Assert.AreEqual(updatedArea, retreivedHouse.Area);
                Assert.AreEqual(updateHouseCommand.Area, retreivedHouse.Area);
                Assert.AreEqual(updatedStreetNo, retreivedHouse.StreetNo);
                Assert.AreEqual(updateHouseCommand.StreetNo, retreivedHouse.StreetNo);
                Assert.AreEqual(updatedOwnerName, retreivedHouse.OwnerName);
                Assert.AreEqual(updateHouseCommand.OwnerName, retreivedHouse.OwnerName);
            }
        }
        
        // KNOWLEDGE POINT
        // # HttpResponseMessage's status and response can be checked like this:
        //Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        //Assert.That(response.Content, Is.EqualTo("No Permission"));
        // # For getting value from HttpResponseMessage, there are 3 ways to extract out objects.
        // # 1 is:
        //House retreivedHouse;
        //response.TryGetContentValue<House>(out retreivedHouse);
        // # 2 is:
        //ObjectContent objContent = response.Content as ObjectContent;
        //House retreivedHouse2 = objContent.Value as House;
        // # 3: But if it's a StreamContent, then we do it like this using async read
        //var retreivedHouse3 = response.Content.ReadAsAsync<House>().Result;

        [Test]
        public void SaveAndGetAllHousesTest_TestsThatHousesAreSavedAndRetreivedAsExpected_VerfiesThroughInstanceValue()
        {
            // Save 5 Houses; there are two sets among them, one set containing 2 houses and the other set containing 
            // 2 houses
            // In the first set, all 3 houses should have the same location, but 2 among these 3 should have the same 
            // property type (which is being searched) while 1 of them should have a different property type
            // The second set that contains 2 houses, both of their area/location should be different (and outside the 
            // thirty kilometer radius of the searched location). One of these has the property type that is being 
            // searched while the other has a different property type
            if (_runTests)
            {
                HouseController houseController = _kernel.Get<HouseController>();
                Assert.NotNull(houseController);

                // Saving House # 1 - SET 1: Should appear in search results
                string title = "Title # 1";
                string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
                int rent = 100001;
                string ownerEmail = "house@1234567-1.com";
                string ownerPhoneNumber = "01234567890";
                string houseNo = "House # 1";
                string streetNo = "1";
                int numberOfBathrooms = 1;
                int numberOfBedrooms = 1;
                int numberOfKitchens = 1;
                bool familiesOnly = false;
                bool boysOnly = false;
                bool girlsOnly = true;
                bool internetAvailable = true;
                bool landlinePhoneAvailable = true;
                bool cableTvAvailable = true;
                bool garageAvailable = true;
                bool smokingAllowed = true;
                string propertyType = PropertyType.House.ToString();                
                string area = "Pindora, Rawalpindi, Pakistan";
                string dimensionType = DimensionType.Kanal.ToString();
                string dimensionString = "1";
                decimal dimensionDecimal = 0;
                string ownerName = "Owner Name 1";
                string genderRestriction = GenderRestriction.GirlsOnly.ToString();

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail)
                    })
                });
                CreateHouseCommand house = new CreateHouseCommand(title, rent, numberOfBedrooms, numberOfKitchens,
                    numberOfBathrooms, internetAvailable, landlinePhoneAvailable,
                    cableTvAvailable, garageAvailable, smokingAllowed, propertyType, ownerEmail, ownerPhoneNumber,
                    houseNo, streetNo, area, dimensionType, dimensionString, dimensionDecimal, ownerName, description, genderRestriction);
                IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(house));
                string houseId = ((OkNegotiatedContentResult<string>) houseSaveResult).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(houseId));

                // Saving House # 2 - SET 1: Should appear in search results
                string title2 = "Title # 2";
                int rent2 = 100002;
                string ownerEmail2 = "house@1234567-2.com";
                string description2 = "Erebor 2. Built deep within the mountain itself the beauty of this fortress was legend.";
                string ownerPhoneNumber2 = "01234567891";
                string houseNo2 = "House # 2";
                string streetNo2 = "2";
                int numberOfBathrooms2 = 2;
                int numberOfBedrooms2 = 2;
                int numberOfKitchens2 = 2;
                bool familiesOnly2 = false;
                bool boysOnly2 = false;
                bool girlsOnly2 = true;
                bool internetAvailable2 = true;
                bool landlinePhoneAvailable2 = true;
                bool cableTvAvailable2 = true;
                bool garageAvailable2 = true;
                bool smokingAllowed2 = true;
                string propertyType2 = PropertyType.House.ToString();
                string area2 = "I-9, Islamabad, Pakistan";
                string dimensionType2 = DimensionType.Kanal.ToString();
                string dimensionString2 = "2";
                decimal dimensionDecimal2 = 0;
                string ownerName2 = "Owner Name 2";
                string genderRestriction2 = GenderRestriction.FamiliesOnly.ToString();

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail2)
                    })
                });
                CreateHouseCommand house2 = new CreateHouseCommand(title2, rent2, numberOfBedrooms2, numberOfKitchens2,
                    numberOfBathrooms2, internetAvailable2, landlinePhoneAvailable2,
                    cableTvAvailable2, garageAvailable2, smokingAllowed2, propertyType2, ownerEmail2, ownerPhoneNumber2,
                    houseNo2, streetNo2, area2, dimensionType2, dimensionString2, dimensionDecimal2, ownerName2, description2, genderRestriction2);
                IHttpActionResult houseSaveResult2 = houseController.Post(JsonConvert.SerializeObject(house2));
                string houseId2 = ((OkNegotiatedContentResult<string>)houseSaveResult2).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(houseId2));

                // Saving House # 3 - SET 1: Should NOT appear in search results
                string title3 = "Title # 3";
                string description3 = "Erebor 3. Built deep within the mountain itself the beauty of this fortress was legend.";
                int rent3 = 100003;
                string ownerEmail3 = "house@1234567-3.com";
                string ownerPhoneNumber3 = "01234567892";
                string houseNo3 = "House # 3";
                string streetNo3 = "3";
                int numberOfBathrooms3 = 3;
                int numberOfBedrooms3 = 3;
                int numberOfKitchens3 = 3;
                bool familiesOnly3 = false;
                bool boysOnly3 = false;
                bool girlsOnly3 = true;
                bool internetAvailable3 = true;
                bool landlinePhoneAvailable3 = true;
                bool cableTvAvailable3 = true;
                bool garageAvailable3 = true;
                bool smokingAllowed3 = true;
                string propertyType3 = PropertyType.Apartment.ToString();
                string area3 = "Saddar, Rawalpindi, Pakistan";
                string dimensionType3 = DimensionType.Kanal.ToString();
                string dimensionString3 = "3";
                decimal dimensionDecimal3 = 0;
                string ownerName3 = "Owner Name 3";
                string genderRestriction3 = GenderRestriction.FamiliesOnly.ToString();

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail3)
                    })
                });
                CreateHouseCommand house3 = new CreateHouseCommand(title3, rent3, numberOfBedrooms3, numberOfKitchens3,
                    numberOfBathrooms3, internetAvailable3, landlinePhoneAvailable3,
                    cableTvAvailable3, garageAvailable3, smokingAllowed3, propertyType3, ownerEmail3, ownerPhoneNumber3,
                    houseNo3, streetNo3, area3, dimensionType3, dimensionString3, dimensionDecimal3, ownerName3, description3, genderRestriction3);
                IHttpActionResult houseSaveResult3 = houseController.Post(JsonConvert.SerializeObject(house3));
                Assert.IsFalse(string.IsNullOrWhiteSpace(((OkNegotiatedContentResult<string>)houseSaveResult3).Content));
                string houseId3 = ((OkNegotiatedContentResult<string>)houseSaveResult3).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(houseId3));

                // Saving House # 4 - SET 2: Should NOT appear in search results, same property type as searched,
                // but outside bounds of the search radius
                string title4 = "Title # 4";
                string description4 = "Erebor 4. Built deep within the mountain itself the beauty of this fortress was legend.";
                int rent4 = 100004;
                string ownerEmail4 = "house@1234567-4.com";
                string ownerPhoneNumber4 = "01234567893";
                string houseNo4 = "House # 4";
                string streetNo4 = "4";
                int numberOfBathrooms4 = 4;
                int numberOfBedrooms4 = 4;
                int numberOfKitchens4 = 4;
                bool familiesOnly4 = false;
                bool boysOnly4 = false;
                bool girlsOnly4 = true;
                bool internetAvailable4 = true;
                bool landlinePhoneAvailable4 = true;
                bool cableTvAvailable4 = true;
                bool garageAvailable4 = true;
                bool smokingAllowed4 = true;
                string propertyType4 = PropertyType.House.ToString();
                string area4 = "Saddar, Rawalpindi, Punjab, Pakistan";
                string dimensionType4 = DimensionType.Kanal.ToString();
                string dimensionString4 = "4";
                decimal dimensionDecimal4 = 0;
                string ownerName4 = "Owner Name 4";
                string genderRestriction4 = GenderRestriction.FamiliesOnly.ToString();

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail4)
                    })
                });
                CreateHouseCommand house4 = new CreateHouseCommand(title4, rent4, numberOfBedrooms4, numberOfKitchens4,
                    numberOfBathrooms4, internetAvailable4, landlinePhoneAvailable4,
                    cableTvAvailable4, garageAvailable4, smokingAllowed4, propertyType4, ownerEmail4, ownerPhoneNumber4,
                    houseNo4, streetNo4, area4, dimensionType4, dimensionString4, dimensionDecimal4, ownerName4, description4, genderRestriction4);
                IHttpActionResult houseSaveResult4 = houseController.Post(JsonConvert.SerializeObject(house4));
                string houseId4 = ((OkNegotiatedContentResult<string>)houseSaveResult4).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(houseId4));

                // Saving House # 5 - SET 2: Should NOT appear in search results, outside bounds and different property 
                // type
                string title5 = "Title # 5";
                string description5 = "Erebor 5. Built deep within the mountain itself the beauty of this fortress was legend.";
                int rent5 = 100005;
                string ownerEmail5 = "house@1234567-5.com";
                string ownerPhoneNumber5 = "01234567894";
                string houseNo5 = "House # 5";
                string streetNo5 = "5";
                int numberOfBathrooms5 = 5;
                int numberOfBedrooms5 = 5;
                int numberOfKitchens5 = 5;
                bool familiesOnly5 = false;
                bool boysOnly5 = false;
                bool girlsOnly5 = true;
                bool internetAvailable5 = true;
                bool landlinePhoneAvailable5 = true;
                bool cableTvAvailable5 = true;
                bool garageAvailable5 = true;
                bool smokingAllowed5 = true;
                string propertyType5 = PropertyType.Apartment.ToString();
                string area5 = "Islamabad Railway Station, Islamabad, Pakistan";
                string dimensionType5 = DimensionType.Kanal.ToString();
                string dimensionString5 = "5";
                decimal dimensionDecimal5 = 0;
                string ownerName5 = "Owner Name 5";
                string genderRestriction5 = GenderRestriction.FamiliesOnly.ToString();

                // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
                houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, ownerEmail5)
                    })
                });
                CreateHouseCommand house5 = new CreateHouseCommand(title5, rent5, numberOfBedrooms5, numberOfKitchens5,
                    numberOfBathrooms5, internetAvailable5, landlinePhoneAvailable5,
                    cableTvAvailable5, garageAvailable5, smokingAllowed5, propertyType5, ownerEmail5, ownerPhoneNumber5,
                    houseNo5, streetNo5, area5, dimensionType5, dimensionString5, dimensionDecimal5, ownerName5, description5, genderRestriction5);
                IHttpActionResult houseSaveResult5 = houseController.Post(JsonConvert.SerializeObject(house5));
                string houseId5 = ((OkNegotiatedContentResult<string>)houseSaveResult5).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(houseId5));

                IHttpActionResult response = (IHttpActionResult)houseController.GetHouse(area:area, propertyType:propertyType);
                IList<HousePartialRepresentation> retreivedHouses = ((OkNegotiatedContentResult<IList<HousePartialRepresentation>>)response).Content;

                Assert.NotNull(retreivedHouses);
                Assert.AreEqual(2, retreivedHouses.Count);
                // Verification of House No 1
                Assert.AreEqual(houseId, retreivedHouses[0].HouseId);
                Assert.AreEqual(title, retreivedHouses[0].Title);
                Assert.AreEqual(house.Title, retreivedHouses[0].Title);
                Assert.AreEqual(description, retreivedHouses[0].Description);
                Assert.AreEqual(house.Description, retreivedHouses[0].Description);
                Assert.AreEqual(rent, retreivedHouses[0].Rent);
                Assert.AreEqual(house.MonthlyRent, retreivedHouses[0].Rent);
                Assert.AreEqual(numberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
                Assert.AreEqual(house.NumberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
                Assert.AreEqual(numberOfBedrooms, retreivedHouses[0].NumberOfBedrooms);
                Assert.AreEqual(house.NumberOfBedrooms, retreivedHouses[0].NumberOfBedrooms);
                Assert.AreEqual(numberOfKitchens, retreivedHouses[0].NumberOfKitchens);
                Assert.AreEqual(house.NumberOfKitchens, retreivedHouses[0].NumberOfKitchens);
                Assert.AreEqual(dimensionString + " " + dimensionType, retreivedHouses[0].Dimension);
                Assert.AreEqual(house.DimensionStringValue + " " + house.DimensionType, retreivedHouses[0].Dimension);
                
                Assert.AreEqual(propertyType, retreivedHouses[0].PropertyType);
                Assert.AreEqual(house.PropertyType, retreivedHouses[0].PropertyType);
                Assert.AreEqual(house.PropertyType, retreivedHouses[0].PropertyType);
                Assert.AreEqual(area, retreivedHouses[0].Area);
                Assert.AreEqual(house.Area, retreivedHouses[0].Area);
                Assert.AreEqual(ownerEmail, retreivedHouses[0].OwnerEmail);
                Assert.AreEqual(house.OwnerEmail, retreivedHouses[0].OwnerEmail);
                Assert.AreEqual(ownerPhoneNumber, retreivedHouses[0].OwnerPhoneNumber);
                Assert.AreEqual(house.OwnerPhoneNumber, retreivedHouses[0].OwnerPhoneNumber);
                Assert.AreEqual(ownerName, retreivedHouses[0].OwnerName);
                Assert.AreEqual(house.OwnerName, retreivedHouses[0].OwnerName);


                // Verification of House No 2
                Assert.AreEqual(houseId2, retreivedHouses[1].HouseId);
                Assert.AreEqual(title2, retreivedHouses[1].Title);
                Assert.AreEqual(house2.Title, retreivedHouses[1].Title);
                Assert.AreEqual(description2, retreivedHouses[1].Description);
                Assert.AreEqual(house2.Description, retreivedHouses[1].Description);
                Assert.AreEqual(rent2, retreivedHouses[1].Rent);
                Assert.AreEqual(house2.MonthlyRent, retreivedHouses[1].Rent);
                Assert.AreEqual(numberOfBathrooms2, retreivedHouses[1].NumberOfBathrooms);
                Assert.AreEqual(house2.NumberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
                Assert.AreEqual(numberOfBedrooms2, retreivedHouses[1].NumberOfBedrooms);
                Assert.AreEqual(house2.NumberOfBedrooms, retreivedHouses[1].NumberOfBedrooms);
                Assert.AreEqual(numberOfKitchens2, retreivedHouses[1].NumberOfKitchens);
                Assert.AreEqual(house2.NumberOfKitchens, retreivedHouses[1].NumberOfKitchens);
                Assert.AreEqual(dimensionString2 + " " + dimensionType2, retreivedHouses[1].Dimension);
                Assert.AreEqual(house2.DimensionStringValue + " " + house2.DimensionType, retreivedHouses[1].Dimension);

                Assert.AreEqual(propertyType2, retreivedHouses[1].PropertyType);
                Assert.AreEqual(house2.PropertyType, retreivedHouses[1].PropertyType);
                Assert.AreEqual(area2, retreivedHouses[1].Area);
                Assert.AreEqual(house2.Area, retreivedHouses[1].Area);
                Assert.AreEqual(ownerEmail2, retreivedHouses[1].OwnerEmail);
                Assert.AreEqual(house2.OwnerEmail, retreivedHouses[1].OwnerEmail);
                Assert.AreEqual(ownerPhoneNumber2, retreivedHouses[1].OwnerPhoneNumber);
                Assert.AreEqual(house2.OwnerPhoneNumber, retreivedHouses[1].OwnerPhoneNumber);
                Assert.AreEqual(ownerName2, retreivedHouses[1].OwnerName);
                Assert.AreEqual(house2.OwnerName, retreivedHouses[1].OwnerName);
            }
        }

        [Category("Integration")]
        [Test]
        public void
            SearchHousesByPropertyTypeOnly_TestsThatHouseIsSavedAndRetreivedAsExpected_VerfiesThroughInstanceValue()
        {
            HouseController houseController = _kernel.Get<HouseController>();
            Assert.NotNull(houseController);

            // Saving House # 1: Should appear in search results with PropertyType = Apartment
            string title = "Title # 1";
            string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
            int rent = 100001;
            string ownerEmail = "house@1234567-1.com";
            string ownerPhoneNumber = "01234567890";
            string houseNo = "House # 1";
            string streetNo = "1";
            int numberOfBathrooms = 1;
            int numberOfBedrooms = 1;
            int numberOfKitchens = 1;
            bool familiesOnly = false;
            bool boysOnly = false;
            bool girlsOnly = true;
            bool internetAvailable = true;
            bool landlinePhoneAvailable = true;
            bool cableTvAvailable = true;
            bool garageAvailable = true;
            bool smokingAllowed = true;
            string propertyType = PropertyType.Apartment.ToString();
            string area = "Pindora, Rawalpindi, Pakistan";
            string dimensionType = DimensionType.Kanal.ToString();
            string dimensionString = "1";
            decimal dimensionDecimal = 0;
            string ownerName = "Owner Name 1";
            string genderRestriction = GenderRestriction.FamiliesOnly.ToString();

            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
            houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, ownerEmail)
                })
            });

            CreateHouseCommand house = new CreateHouseCommand(title, rent, numberOfBedrooms, numberOfKitchens,
                numberOfBathrooms, internetAvailable, landlinePhoneAvailable,
                cableTvAvailable, garageAvailable, smokingAllowed, propertyType, ownerEmail, ownerPhoneNumber,
                houseNo, streetNo, area, dimensionType, dimensionString, dimensionDecimal, ownerName, description, genderRestriction);
            IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(house));
            string houseId = ((OkNegotiatedContentResult<string>) houseSaveResult).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseId));

            // Saving House # 2 - SET 1: Should NOT appear in search results with PropertyType = House
            string title2 = "Title # 2";
            int rent2 = 100002;
            string ownerEmail2 = "house@1234567-2.com";
            string description2 =
                "Erebor 2. Built deep within the mountain itself the beauty of this fortress was legend.";
            string ownerPhoneNumber2 = "01234567892";
            string houseNo2 = "House # 2";
            string streetNo2 = "2";
            int numberOfBathrooms2 = 2;
            int numberOfBedrooms2 = 2;
            int numberOfKitchens2 = 2;
            bool familiesOnly2 = false;
            bool boysOnly2 = false;
            bool girlsOnly2 = true;
            bool internetAvailable2 = true;
            bool landlinePhoneAvailable2 = true;
            bool cableTvAvailable2 = true;
            bool garageAvailable2 = true;
            bool smokingAllowed2 = true;
            string propertyType2 = PropertyType.House.ToString();
            string area2 = "I-9, Islamabad, Pakistan";
            string dimensionType2 = DimensionType.Kanal.ToString();
            string dimensionString2 = "2";
            decimal dimensionDecimal2 = 0;
            string ownerName2 = "Owner Name 2";
            string genderRestriction2 = GenderRestriction.FamiliesOnly.ToString();

            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
            houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, ownerEmail2)
                })
            });
            CreateHouseCommand house2 = new CreateHouseCommand(title2, rent2, numberOfBedrooms2, numberOfKitchens2,
                numberOfBathrooms2, internetAvailable2, landlinePhoneAvailable2,
                cableTvAvailable2, garageAvailable2, smokingAllowed2, propertyType2, ownerEmail2, ownerPhoneNumber2,
                houseNo2, streetNo2, area2, dimensionType2, dimensionString2, dimensionDecimal2, ownerName2,
                description2, genderRestriction2);
            IHttpActionResult houseSaveResult2 = houseController.Post(JsonConvert.SerializeObject(house2));
            string houseId2 = ((OkNegotiatedContentResult<string>) houseSaveResult2).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseId2));

            // Saving House # 3: Should appear in search results with PropertyType = Apartment
            string title3 = "Title # 3";
            string description3 =
                "Erebor 3. Built deep within the mountain itself the beauty of this fortress was legend.";
            int rent3 = 100003;
            string ownerEmail3 = "house@1234567-3.com";
            string ownerPhoneNumber3 = "01234567893";
            string houseNo3 = "House # 3";
            string streetNo3 = "3";
            int numberOfBathrooms3 = 3;
            int numberOfBedrooms3 = 3;
            int numberOfKitchens3 = 3;
            bool familiesOnly3 = false;
            bool boysOnly3 = false;
            bool girlsOnly3 = true;
            bool internetAvailable3 = true;
            bool landlinePhoneAvailable3 = true;
            bool cableTvAvailable3 = true;
            bool garageAvailable3 = true;
            bool smokingAllowed3 = true;
            string propertyType3 = PropertyType.Apartment.ToString();
            string area3 = "Saddar, Rawalpindi, Pakistan";
            string dimensionType3 = DimensionType.Kanal.ToString();
            string dimensionString3 = "3";
            decimal dimensionDecimal3 = 0;
            string ownerName3 = "Owner Name 3";
            string genderRestriction3 = GenderRestriction.FamiliesOnly.ToString();

            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
            houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, ownerEmail3)
                })
            });
            CreateHouseCommand house3 = new CreateHouseCommand(title3, rent3, numberOfBedrooms3, numberOfKitchens3,
                numberOfBathrooms3, internetAvailable3, landlinePhoneAvailable3,
                cableTvAvailable3, garageAvailable3, smokingAllowed3, propertyType3, ownerEmail3, ownerPhoneNumber3,
                houseNo3, streetNo3, area3, dimensionType3, dimensionString3, dimensionDecimal3, ownerName3,
                description3, genderRestriction3);
            IHttpActionResult houseSaveResult3 = houseController.Post(JsonConvert.SerializeObject(house3));
            Assert.IsFalse(string.IsNullOrWhiteSpace(((OkNegotiatedContentResult<string>) houseSaveResult3).Content));
            string houseId3 = ((OkNegotiatedContentResult<string>) houseSaveResult3).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseId3));
            
            IHttpActionResult response = (IHttpActionResult)houseController.GetHouse(propertyType: propertyType);
            IList<HousePartialRepresentation> retreivedHouses = ((OkNegotiatedContentResult<IList<HousePartialRepresentation>>)response).Content;

            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(2, retreivedHouses.Count);

            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(2, retreivedHouses.Count);
            // Verification of House No 1
            Assert.AreEqual(houseId, retreivedHouses[0].HouseId);
            Assert.AreEqual(title, retreivedHouses[0].Title);
            Assert.AreEqual(house.Title, retreivedHouses[0].Title);
            Assert.AreEqual(description, retreivedHouses[0].Description);
            Assert.AreEqual(house.Description, retreivedHouses[0].Description);
            Assert.AreEqual(rent, retreivedHouses[0].Rent);
            Assert.AreEqual(house.MonthlyRent, retreivedHouses[0].Rent);
            Assert.AreEqual(numberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(numberOfBedrooms, retreivedHouses[0].NumberOfBedrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouses[0].NumberOfBedrooms);
            Assert.AreEqual(numberOfKitchens, retreivedHouses[0].NumberOfKitchens);
            Assert.AreEqual(house.NumberOfKitchens, retreivedHouses[0].NumberOfKitchens);
            Assert.AreEqual(dimensionString + " " + dimensionType, retreivedHouses[0].Dimension);
            Assert.AreEqual(house.DimensionStringValue + " " + house.DimensionType, retreivedHouses[0].Dimension);

            Assert.AreEqual(propertyType, retreivedHouses[0].PropertyType);
            Assert.AreEqual(house.PropertyType, retreivedHouses[0].PropertyType);
            Assert.AreEqual(area, retreivedHouses[0].Area);
            Assert.AreEqual(house.Area, retreivedHouses[0].Area);
            Assert.AreEqual(ownerEmail, retreivedHouses[0].OwnerEmail);
            Assert.AreEqual(house.OwnerEmail, retreivedHouses[0].OwnerEmail);
            Assert.AreEqual(ownerPhoneNumber, retreivedHouses[0].OwnerPhoneNumber);
            Assert.AreEqual(house.OwnerPhoneNumber, retreivedHouses[0].OwnerPhoneNumber);
            Assert.AreEqual(ownerName, retreivedHouses[0].OwnerName);
            Assert.AreEqual(house.OwnerName, retreivedHouses[0].OwnerName);


            // Verification of House No 3 (in order of saving houses above)
            Assert.AreEqual(houseId3, retreivedHouses[1].HouseId);
            Assert.AreEqual(title3, retreivedHouses[1].Title);
            Assert.AreEqual(house3.Title, retreivedHouses[1].Title);
            Assert.AreEqual(description3, retreivedHouses[1].Description);
            Assert.AreEqual(house3.Description, retreivedHouses[1].Description);
            Assert.AreEqual(rent3, retreivedHouses[1].Rent);
            Assert.AreEqual(house3.MonthlyRent, retreivedHouses[1].Rent);
            Assert.AreEqual(numberOfBathrooms3, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(house3.NumberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(numberOfBedrooms3, retreivedHouses[1].NumberOfBedrooms);
            Assert.AreEqual(house3.NumberOfBedrooms, retreivedHouses[1].NumberOfBedrooms);
            Assert.AreEqual(numberOfKitchens3, retreivedHouses[1].NumberOfKitchens);
            Assert.AreEqual(house3.NumberOfKitchens, retreivedHouses[1].NumberOfKitchens);
            Assert.AreEqual(dimensionString3 + " " + dimensionType2, retreivedHouses[1].Dimension);
            Assert.AreEqual(house3.DimensionStringValue + " " + house2.DimensionType, retreivedHouses[1].Dimension);

            Assert.AreEqual(propertyType3, retreivedHouses[1].PropertyType);
            Assert.AreEqual(house3.PropertyType, retreivedHouses[1].PropertyType);
            Assert.AreEqual(area3, retreivedHouses[1].Area);
            Assert.AreEqual(house3.Area, retreivedHouses[1].Area);
            Assert.AreEqual(ownerEmail3, retreivedHouses[1].OwnerEmail);
            Assert.AreEqual(house3.OwnerEmail, retreivedHouses[1].OwnerEmail);
            Assert.AreEqual(ownerPhoneNumber3, retreivedHouses[1].OwnerPhoneNumber);
            Assert.AreEqual(house3.OwnerPhoneNumber, retreivedHouses[1].OwnerPhoneNumber);
            Assert.AreEqual(ownerName3, retreivedHouses[1].OwnerName);
            Assert.AreEqual(house3.OwnerName, retreivedHouses[1].OwnerName);
        }

        [Category("Integration")]
        [Test]
        public void
            SearchHousesByAreaOnly_TestsThatHouseIsSavedAndRetreivedAsExpected_VerfiesThroughInstanceValue()
        {
            HouseController houseController = _kernel.Get<HouseController>();
            Assert.NotNull(houseController);

            // Saving House # 1: Should appear in search results with area = Pindora (near I-9, Islamabad)
            string title = "Title # 1";
            string description = "Erebor. Built deep within the mountain itself the beauty of this fortress was legend.";
            int rent = 100001;
            string ownerEmail = "house@1234567-1.com";
            string ownerPhoneNumber = "01234567890";
            string houseNo = "House # 1";
            string streetNo = "1";
            int numberOfBathrooms = 1;
            int numberOfBedrooms = 1;
            int numberOfKitchens = 1;
            bool familiesOnly = false;
            bool boysOnly = false;
            bool girlsOnly = true;
            bool internetAvailable = true;
            bool landlinePhoneAvailable = true;
            bool cableTvAvailable = true;
            bool garageAvailable = true;
            bool smokingAllowed = true;
            string propertyType = PropertyType.Apartment.ToString();
            string area = "Pindora, Rawalpindi, Pakistan";
            string dimensionType = DimensionType.Kanal.ToString();
            string dimensionString = "1";
            decimal dimensionDecimal = 0;
            string ownerName = "Owner Name 1";
            string genderRestriction = GenderRestriction.NoRestriction.ToString();

            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
            houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, ownerEmail)
                })
            });

            CreateHouseCommand house = new CreateHouseCommand(title, rent, numberOfBedrooms, numberOfKitchens,
                numberOfBathrooms, internetAvailable, landlinePhoneAvailable,
                cableTvAvailable, garageAvailable, smokingAllowed, propertyType, ownerEmail, ownerPhoneNumber,
                houseNo, streetNo, area, dimensionType, dimensionString, dimensionDecimal, ownerName, description, genderRestriction);
            IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(house));
            string houseId = ((OkNegotiatedContentResult<string>)houseSaveResult).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseId));

            // Saving House # 2 - SET 1: Should appear in search results with area = I-9, Islamabad, which would be the search criteria
            string title2 = "Title # 2";
            int rent2 = 100002;
            string ownerEmail2 = "house@1234567-2.com";
            string description2 =
                "Erebor 2. Built deep within the mountain itself the beauty of this fortress was legend.";
            string ownerPhoneNumber2 = "01234567891";
            string houseNo2 = "House # 2";
            string streetNo2 = "2";
            int numberOfBathrooms2 = 2;
            int numberOfBedrooms2 = 2;
            int numberOfKitchens2 = 2;
            bool familiesOnly2 = false;
            bool boysOnly2 = false;
            bool girlsOnly2 = true;
            bool internetAvailable2 = true;
            bool landlinePhoneAvailable2 = true;
            bool cableTvAvailable2 = true;
            bool garageAvailable2 = true;
            bool smokingAllowed2 = true;
            string propertyType2 = PropertyType.House.ToString();
            string area2 = "I-9, Islamabad, Pakistan";
            string dimensionType2 = DimensionType.Kanal.ToString();
            string dimensionString2 = "2";
            decimal dimensionDecimal2 = 0;
            string ownerName2 = "Owner Name 2";
            string genderRestriction2 = GenderRestriction.FamiliesOnly.ToString();

            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
            houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, ownerEmail2)
                })
            });
            CreateHouseCommand house2 = new CreateHouseCommand(title2, rent2, numberOfBedrooms2, numberOfKitchens2,
                numberOfBathrooms2, internetAvailable2, landlinePhoneAvailable2,
                cableTvAvailable2, garageAvailable2, smokingAllowed2, propertyType2, ownerEmail2, ownerPhoneNumber2,
                houseNo2, streetNo2, area2, dimensionType2, dimensionString2, dimensionDecimal2, ownerName2,
                description2, genderRestriction2);
            IHttpActionResult houseSaveResult2 = houseController.Post(JsonConvert.SerializeObject(house2));
            string houseId2 = ((OkNegotiatedContentResult<string>)houseSaveResult2).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseId2));

            // Saving House # 3: Should NOT appear in search results with area = Saddar, Rawalpindi, which is more than 2 kilometers 
            // away from I-9, Islamabad. Remember the search radius is 2 Kilometer
            string title3 = "Title # 3";
            string description3 =
                "Erebor 3. Built deep within the mountain itself the beauty of this fortress was legend.";
            int rent3 = 100003;
            string ownerEmail3 = "house@1234567-3.com";
            string ownerPhoneNumber3 = "01234567892";
            string houseNo3 = "House # 3";
            string streetNo3 = "3";
            int numberOfBathrooms3 = 3;
            int numberOfBedrooms3 = 3;
            int numberOfKitchens3 = 3;
            bool familiesOnly3 = false;
            bool boysOnly3 = false;
            bool girlsOnly3 = true;
            bool internetAvailable3 = true;
            bool landlinePhoneAvailable3 = true;
            bool cableTvAvailable3 = true;
            bool garageAvailable3 = true;
            bool smokingAllowed3 = true;
            string propertyType3 = PropertyType.Apartment.ToString();
            string area3 = "Saddar, Rawalpindi, Pakistan";
            string dimensionType3 = DimensionType.Kanal.ToString();
            string dimensionString3 = "3";
            decimal dimensionDecimal3 = 0;
            string ownerName3 = "Owner Name 3";
            string genderRestriction3 = GenderRestriction.FamiliesOnly.ToString();

            // Set the Current User's username(which is the same as his email), otherwise the request for posting a new house will fail
            houseController.User = new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, ownerEmail3)
                })
            });
            CreateHouseCommand house3 = new CreateHouseCommand(title3, rent3, numberOfBedrooms3, numberOfKitchens3,
                numberOfBathrooms3, internetAvailable3, landlinePhoneAvailable3,
                cableTvAvailable3, garageAvailable3, smokingAllowed3, propertyType3, ownerEmail3, ownerPhoneNumber3,
                houseNo3, streetNo3, area3, dimensionType3, dimensionString3, dimensionDecimal3, ownerName3,
                description3, genderRestriction3);
            IHttpActionResult houseSaveResult3 = houseController.Post(JsonConvert.SerializeObject(house3));
            Assert.IsFalse(string.IsNullOrWhiteSpace(((OkNegotiatedContentResult<string>)houseSaveResult3).Content));
            string houseId3 = ((OkNegotiatedContentResult<string>)houseSaveResult3).Content;
            Assert.IsFalse(string.IsNullOrWhiteSpace(houseId3));

            // Search the 
            IHttpActionResult response = (IHttpActionResult)houseController.GetHouse(area: area2);
            IList<HousePartialRepresentation> retreivedHouses = ((OkNegotiatedContentResult<IList<HousePartialRepresentation>>)response).Content;

            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(2, retreivedHouses.Count);

            Assert.NotNull(retreivedHouses);
            Assert.AreEqual(2, retreivedHouses.Count);
            // Verification of House No 1 (in order of saving houses above)
            Assert.AreEqual(houseId2, retreivedHouses[0].HouseId);
            Assert.AreEqual(title2, retreivedHouses[0].Title);
            Assert.AreEqual(house2.Title, retreivedHouses[0].Title);
            Assert.AreEqual(description2, retreivedHouses[0].Description);
            Assert.AreEqual(house2.Description, retreivedHouses[0].Description);
            Assert.AreEqual(rent2, retreivedHouses[0].Rent);
            Assert.AreEqual(house2.MonthlyRent, retreivedHouses[0].Rent);
            Assert.AreEqual(numberOfBathrooms2, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(house2.NumberOfBathrooms, retreivedHouses[0].NumberOfBathrooms);
            Assert.AreEqual(numberOfBedrooms2, retreivedHouses[0].NumberOfBedrooms);
            Assert.AreEqual(house2.NumberOfBedrooms, retreivedHouses[0].NumberOfBedrooms);
            Assert.AreEqual(numberOfKitchens2, retreivedHouses[0].NumberOfKitchens);
            Assert.AreEqual(house2.NumberOfKitchens, retreivedHouses[0].NumberOfKitchens);
            Assert.AreEqual(dimensionString2 + " " + dimensionType2, retreivedHouses[0].Dimension);
            Assert.AreEqual(house2.DimensionStringValue + " " + house2.DimensionType, retreivedHouses[0].Dimension);

            Assert.AreEqual(propertyType2, retreivedHouses[0].PropertyType);
            Assert.AreEqual(house2.PropertyType, retreivedHouses[0].PropertyType);
            Assert.AreEqual(area2, retreivedHouses[0].Area);
            Assert.AreEqual(house2.Area, retreivedHouses[0].Area);
            Assert.AreEqual(ownerEmail2, retreivedHouses[0].OwnerEmail);
            Assert.AreEqual(house2.OwnerEmail, retreivedHouses[0].OwnerEmail);
            Assert.AreEqual(ownerPhoneNumber2, retreivedHouses[0].OwnerPhoneNumber);
            Assert.AreEqual(house2.OwnerPhoneNumber, retreivedHouses[0].OwnerPhoneNumber);
            Assert.AreEqual(ownerName2, retreivedHouses[0].OwnerName);
            Assert.AreEqual(house2.OwnerName, retreivedHouses[0].OwnerName);
            
            // Verification of House No 2 (in order of saving houses above)
            Assert.AreEqual(houseId, retreivedHouses[1].HouseId);
            Assert.AreEqual(title, retreivedHouses[1].Title);
            Assert.AreEqual(house.Title, retreivedHouses[1].Title);
            Assert.AreEqual(description, retreivedHouses[1].Description);
            Assert.AreEqual(house.Description, retreivedHouses[1].Description);
            Assert.AreEqual(rent, retreivedHouses[1].Rent);
            Assert.AreEqual(house.MonthlyRent, retreivedHouses[1].Rent);
            Assert.AreEqual(numberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(house.NumberOfBathrooms, retreivedHouses[1].NumberOfBathrooms);
            Assert.AreEqual(numberOfBedrooms, retreivedHouses[1].NumberOfBedrooms);
            Assert.AreEqual(house.NumberOfBedrooms, retreivedHouses[1].NumberOfBedrooms);
            Assert.AreEqual(numberOfKitchens, retreivedHouses[1].NumberOfKitchens);
            Assert.AreEqual(house.NumberOfKitchens, retreivedHouses[1].NumberOfKitchens);
            Assert.AreEqual(dimensionString + " " + dimensionType2, retreivedHouses[1].Dimension);
            Assert.AreEqual(house.DimensionStringValue + " " + house2.DimensionType, retreivedHouses[1].Dimension);

            Assert.AreEqual(propertyType, retreivedHouses[1].PropertyType);
            Assert.AreEqual(house.PropertyType, retreivedHouses[1].PropertyType);
            Assert.AreEqual(area, retreivedHouses[1].Area);
            Assert.AreEqual(house.Area, retreivedHouses[1].Area);
            Assert.AreEqual(ownerEmail, retreivedHouses[1].OwnerEmail);
            Assert.AreEqual(house.OwnerEmail, retreivedHouses[1].OwnerEmail);
            Assert.AreEqual(ownerPhoneNumber, retreivedHouses[1].OwnerPhoneNumber);
            Assert.AreEqual(house.OwnerPhoneNumber, retreivedHouses[1].OwnerPhoneNumber);
            Assert.AreEqual(ownerName, retreivedHouses[1].OwnerName);
            Assert.AreEqual(house.OwnerName, retreivedHouses[1].OwnerName);
        }
    }
}
