using System;
using NUnit.Framework;
using RentStuff.Property.Domain.Model.HouseAggregate;

namespace RentStuff.Property.Domain.Model.Tests
{
    [TestFixture]
    class HouseTests
    {
        [Test]
        public void CreateHouseInstanceSuccessTest_TestsThatAHouseInstanceIsCreatedAsExpected_VerifiesByTheReturnValue()
        {
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";

            // No Latitude is given. So the house instance should not be created
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            bool smokingAllowed = false;
            bool landline = true;
            bool cableTv = false;
            bool internet = true;
            bool garage = true;
            decimal latitude = 25.43M;
            decimal longitude = 73.41M;
            PropertyType propertyType = PropertyType.Apartment;
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
            string area = "Pindora, Rawalpindi, Pakistan";
            long monthlyRent = 90000;
            string image1 = "Image1.jpg";
            string image2 = "Image2.png";
            
            House house = new House.HouseBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title).OwnerName(name)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(cableTv)
                .GarageAvailable(garage).LandlinePhoneAvailable(landline).SmokingAllowed(smokingAllowed).WithInternetAvailable(internet)
                .PropertyType(propertyType).MonthlyRent(monthlyRent).Latitude(latitude).Longitude(73.41M)
                .Area(area).GenderRestriction(genderRestriction).Description(description).Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, "10", 0, house);
            house.Dimension = dimension;
            house.AddImage(image1);
            house.AddImage(image2);
            Assert.IsNotNull(house);
            Assert.AreEqual(title, house.Title);
            Assert.AreEqual(description, house.Description);
            Assert.AreEqual(email, house.OwnerEmail);
            Assert.AreEqual(name, house.OwnerName);
            Assert.AreEqual(phoneNumber, house.OwnerPhoneNumber);
            Assert.AreEqual(numberOfBedrooms, house.NumberOfBedrooms);
            Assert.AreEqual(numberofBathrooms, house.NumberOfBathrooms);
            Assert.AreEqual(numberOfKitchens, house.NumberOfKitchens);
            Assert.AreEqual(smokingAllowed, house.SmokingAllowed);
            Assert.AreEqual(landline, house.LandlinePhoneAvailable);
            Assert.AreEqual(cableTv, house.CableTvAvailable);
            Assert.AreEqual(internet, house.InternetAvailable);
            Assert.AreEqual(garage, house.GarageAvailable);
            Assert.AreEqual(latitude, house.Latitude);
            Assert.AreEqual(longitude, house.Longitude);
            Assert.AreEqual(propertyType, house.PropertyType);
            Assert.AreEqual(genderRestriction, house.GenderRestriction);
            Assert.AreEqual(area, house.Area);
            Assert.AreEqual(monthlyRent, house.MonthlyRent);
            Assert.AreEqual(dimension.DimensionType, house.Dimension.DimensionType);
            Assert.AreEqual(dimension.StringValue, house.Dimension.StringValue);
            Assert.AreEqual(dimension.DecimalValue, house.Dimension.DecimalValue);

            Assert.AreEqual(image1, house.HouseImages[0]);
            Assert.AreEqual(image2, house.HouseImages[1]);
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHouseFailTest1_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            string title = "Title No 1";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";

            // No Latitude is given. So the house instance should not be created
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long monthlyRent = 90000;

            House house = new House.HouseBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title).OwnerName(name)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(monthlyRent).Longitude(73.41M)
                .HouseNo("123").Area("Pindora").StreetNo("13").Build();
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHouseFailTest2_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            string email = "w@12344321.com";

            // No Longitude is given. So the house instance should not be created
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long monthlyRent = 90000;

            House house = new House.HouseBuilder().OwnerEmail(email)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(monthlyRent).Latitude(73.41M)
                .HouseNo("123").Area("Pindora").StreetNo("13").Build();
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHouseFailTest3_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            string title = "Title No 1";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";

            // No Longitude is given. So the house instance should not be created
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long monthlyRent = 90000;

            House house = new House.HouseBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title).OwnerName(name)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(monthlyRent).Latitude(73.41M)
                .HouseNo("123").Area("Pindora").StreetNo("13").Build();
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHouseFailTest4_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            string title = "Title No 1";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";

            // No Rent is given. So the house instance should not be created
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            
            House house = new House.HouseBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title).OwnerName(name)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).Latitude(23.45M).Longitude(73.41M)
                .HouseNo("123").Area("Pindora").StreetNo("13").Build();
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHouseFailTest5_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            // No OwnerName provided, so exception should be raised
            string title = "Title No 1";
            string email = "w@12344321.com";
            string phoneNumber = "03455138018";
            
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long monthlyRent = 1000001;

            House house = new House.HouseBuilder().Title(title).MonthlyRent(monthlyRent).OwnerEmail(email)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(130000).Latitude(23.65M).Longitude(73.41M)
                .HouseNo("123").Area("Pindora").StreetNo("13").OwnerPhoneNumber(phoneNumber).Build();
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHouseFailTest6_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            // No OwnerPhoneNumber provided, so exception should be raised
            string title = "Title No 1";
            string email = "w@12344321.com";
            string name = "OwnerName";
            
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long monthlyRent = 1000001;

            House house = new House.HouseBuilder().Title(title).MonthlyRent(monthlyRent).OwnerEmail(email)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(130000).Latitude(23.65M).Longitude(73.41M)
                .HouseNo("123").Area("Pindora").StreetNo("13").OwnerName(name).Build();
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHouseFailTest7_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            // No OwnerEmail provided, so exception should be raised
            string title = "Title No 1";
            string phoneNumber = "03455138018";
            string name = "OwnerName";
            
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long monthlyRent = 1000001;

            House house = new House.HouseBuilder().Title(title).MonthlyRent(monthlyRent)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(130000).Latitude(23.65M).Longitude(73.41M)
                .HouseNo("123").Area("Pindora").StreetNo("13").OwnerPhoneNumber(phoneNumber).OwnerName(name).Build();
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHouseFailTest8_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            // No Title is provided, so exception should be raised
            string email = "w@12344321.com";
            string phoneNumber = "03455138018";
            string name = "OwnerName";
            long monthlyRent = 1000001;

            // Both for families and for girls is allowed, so exception should be raised
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;

            House house = new House.HouseBuilder().OwnerEmail(email).MonthlyRent(monthlyRent)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(130000).Latitude(23.65M).Longitude(73.41M)
                .HouseNo("123").Area("Pindora").StreetNo("13").OwnerPhoneNumber(phoneNumber).OwnerName(name).Build();
        }
    }
}
