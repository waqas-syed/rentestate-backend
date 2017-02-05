using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;
using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Newtonsoft.Json;
using NUnit.Framework;
using RentStuff.Common;
using RentStuff.Property.Application.HouseServices.Commands;
using RentStuff.Property.Application.HouseServices.Representation;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Domain.Model.Services;
using RentStuff.Property.Ports.Adapter.Rest.Resources;
using Spring.Context.Support;

namespace RentStuff.Property.Ports.Tests
{
    [TestFixture]
    public class HouseControllerTests
    {
        // Flag which allows tests to run or not, as these tests call Google APIs which has a threshold limie. So we want to be sure
        // that we are not exhausting resources by unknowingly running such tests
        private bool _runTests = true;
        private DatabaseUtility _databaseUtility;

        [SetUp]
        public void Setup()
        {
            var connection = ConfigurationManager.ConnectionStrings["MySql"].ToString();
            _databaseUtility = new DatabaseUtility(connection);
            _databaseUtility.Create();            
            //_databaseUtility.Populate();
            ShowNhibernateLogging();
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
        public void SaveAndGetHouseInstanceByIdTest_TestsThatHouseIsSavedAndRetreivedAsExpected_VerfiesThroughInstanceValue()
        {
            if (_runTests)
            {
                HouseController houseController = (HouseController) ContextRegistry.GetContext()["HouseController"];
                Assert.NotNull(houseController);

                string title = "House For Rent";
                int rent = 105000;
                string ownerEmail = "thorin@oakenshield123.com";
                string ownerPhoneNumber = "+923211234567";
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
                string area = "1600 Amphitheatre Parkway, Mountain View, CA";
                //string area = "Pindora, Rawalpindi, Pakistan";
                string dimensionType = DimensionType.Kanal.ToString();
                string dimensionString = "5";
                decimal dimensionDecimal = 0;

                CreateHouseCommand house = new CreateHouseCommand(title, rent,numberOfBedrooms, numberOfKitchens, 
                    numberOfBathrooms,
                    familiesOnly, boysOnly, girlsOnly, internetAvailable, landlinePhoneAvailable,
                    cableTvAvailable, garageAvailable, smokingAllowed, propertyType, ownerEmail, ownerPhoneNumber, 
                    houseNo, streetNo, area, dimensionType, dimensionString, dimensionDecimal);
                IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(house));
                string houseId = ((OkNegotiatedContentResult<string>) houseSaveResult).Content;
                
                IHttpActionResult response = (IHttpActionResult)houseController.GetHouse(houseId:houseId);
                House retreivedHouse = ((OkNegotiatedContentResult<House>) response).Content;
                Assert.NotNull(retreivedHouse);
                Assert.AreEqual(title, retreivedHouse.Title);
                Assert.AreEqual(house.Title, retreivedHouse.Title);
                Assert.AreEqual(rent, retreivedHouse.MonthlyRent);
                Assert.AreEqual(house.MonthlyRent, retreivedHouse.MonthlyRent);
                Assert.AreEqual(numberOfBathrooms, retreivedHouse.NumberOfBathrooms);
                Assert.AreEqual(house.NumberOfBathrooms, retreivedHouse.NumberOfBathrooms);
                Assert.AreEqual(numberOfBedrooms, retreivedHouse.NumberOfBedrooms);
                Assert.AreEqual(house.NumberOfBedrooms, retreivedHouse.NumberOfBedrooms);
                Assert.AreEqual(numberOfKitchens, retreivedHouse.NumberOfKitchens);
                Assert.AreEqual(house.NumberOfKitchens, retreivedHouse.NumberOfKitchens);
                Assert.AreEqual(familiesOnly, retreivedHouse.FamiliesOnly);
                Assert.AreEqual(house.FamiliesOnly, retreivedHouse.FamiliesOnly);
                Assert.AreEqual(boysOnly, retreivedHouse.BoysOnly);
                Assert.AreEqual(house.BoysOnly, retreivedHouse.BoysOnly);
                Assert.AreEqual(girlsOnly, retreivedHouse.GirlsOnly);
                Assert.AreEqual(house.GirlsOnly, retreivedHouse.GirlsOnly);
                Assert.AreEqual(landlinePhoneAvailable, retreivedHouse.LandlinePhoneAvailable);
                Assert.AreEqual(house.LandlinePhoneAvailable, retreivedHouse.LandlinePhoneAvailable);
                Assert.AreEqual(garageAvailable, retreivedHouse.GarageAvailable);
                Assert.AreEqual(house.GarageAvailable, retreivedHouse.GarageAvailable);
                Assert.AreEqual(smokingAllowed, retreivedHouse.SmokingAllowed);
                Assert.AreEqual(house.SmokingAllowed, retreivedHouse.SmokingAllowed);
                Assert.AreEqual(internetAvailable, retreivedHouse.InternetAvailable);
                Assert.AreEqual(house.InternetAvailable, retreivedHouse.InternetAvailable);                
                Assert.AreEqual(Enum.Parse(typeof(PropertyType), propertyType), retreivedHouse.PropertyType);
                Assert.AreEqual(Enum.Parse(typeof(PropertyType), house.PropertyType), retreivedHouse.PropertyType);

                // Get the coordinates to verify we have stored the correct ones
                IGeocodingService geocodingService = (IGeocodingService)ContextRegistry.GetContext()["GeocodingService"];
                var coordinatesFromAddress = geocodingService.GetCoordinatesFromAddress(area);
                Assert.AreEqual(coordinatesFromAddress.Item1, retreivedHouse.Latitude);
                Assert.AreEqual(coordinatesFromAddress.Item2, retreivedHouse.Longitude);

                Assert.AreEqual(houseNo, retreivedHouse.HouseNo);
                Assert.AreEqual(house.HouseNo, retreivedHouse.HouseNo);
                Assert.AreEqual(area, retreivedHouse.Area);
                Assert.AreEqual(house.Area, retreivedHouse.Area);
                Assert.AreEqual(streetNo, retreivedHouse.StreetNo);
                Assert.AreEqual(house.StreetNo, retreivedHouse.StreetNo);
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
                HouseController houseController = (HouseController)ContextRegistry.GetContext()["HouseController"];
                Assert.NotNull(houseController);

                // Saving House # 1 - SET 1: Should appear in search results
                string title = "Title # 1";
                int rent = 100001;
                string ownerEmail = "house@1234567-1.com";
                string ownerPhoneNumber = "+925000000001";
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
                
                CreateHouseCommand house = new CreateHouseCommand(title, rent, numberOfBedrooms, numberOfKitchens,
                    numberOfBathrooms,
                    familiesOnly, boysOnly, girlsOnly, internetAvailable, landlinePhoneAvailable,
                    cableTvAvailable, garageAvailable, smokingAllowed, propertyType, ownerEmail, ownerPhoneNumber,
                    houseNo, streetNo, area, dimensionType, dimensionString, dimensionDecimal);
                IHttpActionResult houseSaveResult = houseController.Post(JsonConvert.SerializeObject(house));
                string houseId = ((OkNegotiatedContentResult<string>) houseSaveResult).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(houseId));

                // Saving House # 2 - SET 1: Should appear in search results
                string title2 = "Title # 2";
                int rent2 = 100002;
                string ownerEmail2 = "house@1234567-2.com";
                string ownerPhoneNumber2 = "+925000000002";
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

                CreateHouseCommand house2 = new CreateHouseCommand(title2, rent2, numberOfBedrooms2, numberOfKitchens2,
                    numberOfBathrooms2, familiesOnly2, boysOnly2, girlsOnly2, internetAvailable2, landlinePhoneAvailable2,
                    cableTvAvailable2, garageAvailable2, smokingAllowed2, propertyType2, ownerEmail2, ownerPhoneNumber2,
                    houseNo2, streetNo2, area2, dimensionType2, dimensionString2, dimensionDecimal2);
                IHttpActionResult houseSaveResult2 = houseController.Post(JsonConvert.SerializeObject(house2));
                string houseId2 = ((OkNegotiatedContentResult<string>)houseSaveResult2).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(houseId2));

                // Saving House # 3 - SET 1: Should NOT appear in search results
                string title3 = "Title # 3";
                int rent3 = 100003;
                string ownerEmail3 = "house@1234567-3.com";
                string ownerPhoneNumber3 = "+925000000003";
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

                CreateHouseCommand house3 = new CreateHouseCommand(title3, rent3, numberOfBedrooms3, numberOfKitchens3,
                    numberOfBathrooms3, familiesOnly3, boysOnly3, girlsOnly3, internetAvailable3, landlinePhoneAvailable3,
                    cableTvAvailable3, garageAvailable3, smokingAllowed3, propertyType3, ownerEmail3, ownerPhoneNumber3,
                    houseNo3, streetNo3, area3, dimensionType3, dimensionString3, dimensionDecimal3);
                IHttpActionResult houseSaveResult3 = houseController.Post(JsonConvert.SerializeObject(house3));
                Assert.IsFalse(string.IsNullOrWhiteSpace(((OkNegotiatedContentResult<string>)houseSaveResult3).Content));
                string houseId3 = ((OkNegotiatedContentResult<string>)houseSaveResult3).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(houseId3));

                // Saving House # 4 - SET 2: Should NOT appear in search results, same property type as searched,
                // but outside bounds of the search radius
                string title4 = "Title # 4";
                int rent4 = 100004;
                string ownerEmail4 = "house@1234567-4.com";
                string ownerPhoneNumber4 = "+925000000004";
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

                CreateHouseCommand house4 = new CreateHouseCommand(title4, rent4, numberOfBedrooms4, numberOfKitchens4,
                    numberOfBathrooms4, familiesOnly4, boysOnly4, girlsOnly4, internetAvailable4, landlinePhoneAvailable4,
                    cableTvAvailable4, garageAvailable4, smokingAllowed4, propertyType4, ownerEmail4, ownerPhoneNumber4,
                    houseNo4, streetNo4, area4, dimensionType4, dimensionString4, dimensionDecimal4);
                IHttpActionResult houseSaveResult4 = houseController.Post(JsonConvert.SerializeObject(house4));
                string houseId4 = ((OkNegotiatedContentResult<string>)houseSaveResult4).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(houseId4));

                // Saving House # 5 - SET 2: Should NOT appear in search results, outside bounds and different property 
                // type
                string title5 = "Title # 5";
                int rent5 = 100005;
                string ownerEmail5 = "house@1234567-5.com";
                string ownerPhoneNumber5 = "+925000000005";
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

                CreateHouseCommand house5 = new CreateHouseCommand(title5, rent5, numberOfBedrooms5, numberOfKitchens5,
                    numberOfBathrooms5, familiesOnly5, boysOnly5, girlsOnly5, internetAvailable5, landlinePhoneAvailable5,
                    cableTvAvailable5, garageAvailable5, smokingAllowed5, propertyType5, ownerEmail5, ownerPhoneNumber5,
                    houseNo5, streetNo5, area5, dimensionType5, dimensionString5, dimensionDecimal5);
                IHttpActionResult houseSaveResult5 = houseController.Post(JsonConvert.SerializeObject(house5));
                string houseId5 = ((OkNegotiatedContentResult<string>)houseSaveResult5).Content;
                Assert.IsFalse(string.IsNullOrWhiteSpace(houseId5));

                IHttpActionResult response = (IHttpActionResult)houseController.GetHouse(area:area, propertyType:propertyType);
                IList<HouseRepresentation> retreivedHouses = ((OkNegotiatedContentResult<IList<HouseRepresentation>>)response).Content;

                Assert.NotNull(retreivedHouses);
                Assert.AreEqual(2, retreivedHouses.Count);
                // Verification of House No 1
                Assert.AreEqual(houseId, retreivedHouses[0].HouseId);
                Assert.AreEqual(title, retreivedHouses[0].Title);
                Assert.AreEqual(house.Title, retreivedHouses[0].Title);
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


                // Verification of House No 2
                Assert.AreEqual(houseId2, retreivedHouses[1].HouseId);
                Assert.AreEqual(title2, retreivedHouses[1].Title);
                Assert.AreEqual(house2.Title, retreivedHouses[1].Title);
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
            }
        }
    }
}
