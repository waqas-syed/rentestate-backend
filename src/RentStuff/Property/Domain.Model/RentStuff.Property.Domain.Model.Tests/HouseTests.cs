﻿using System;
using NUnit.Framework;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Domain.Model.PropertyAggregate;

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
            string propertyType = "Apartment";
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
            string area = "Pindora, Rawalpindi, Pakistan";
            long monthlyRent = 90000;
            string image1 = "Image1.jpg";
            string image2 = "Image2.png";
            string rentUnit = "Hour";
            bool isShared = true;
            string landlineNumber = "0510000000";
            string fax = "0510000000";

            bool ac = true;
            bool geyser = true;
            bool balcony = false;
            bool lawn = true;
            bool heating = false;
            bool cctvCameras = true;
            bool backupElectricity = true;
            bool bathtub = true;
            bool elevator = false;

            House house = new House.HouseBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title).OwnerName(name)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(cableTv)
                .GarageAvailable(garage).LandlinePhoneAvailable(landline).SmokingAllowed(smokingAllowed).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).LandlineNumber(landlineNumber).Fax(fax).AC(ac).Geyser(geyser)
                .Balcony(balcony).Lawn(lawn).Heating(heating).CctvCameras(cctvCameras)
                .BackupElectricity(backupElectricity).Bathtub(bathtub).Elevator(elevator)
                .Build();
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
            Assert.AreEqual(monthlyRent, house.RentPrice);
            
            Assert.AreEqual(dimension.DimensionType, house.Dimension.DimensionType);
            Assert.AreEqual(dimension.StringValue, house.Dimension.StringValue);
            Assert.AreEqual(dimension.DecimalValue, house.Dimension.DecimalValue);

            Assert.AreEqual(image1, house.Images[0]);
            Assert.AreEqual(image2, house.Images[1]);

            Assert.AreEqual(isShared, house.IsShared);
            Assert.AreEqual(rentUnit, house.RentUnit);
            Assert.AreEqual(landlineNumber, house.LandlineNumber);
            Assert.AreEqual(fax, house.Fax);

            Assert.AreEqual(ac, house.AC);
            Assert.AreEqual(geyser, house.Geyser);
            Assert.AreEqual(balcony, house.Balcony);
            Assert.AreEqual(lawn, house.Lawn);
            Assert.AreEqual(cctvCameras, house.CctvCameras);
            Assert.AreEqual(backupElectricity, house.BackupElectricity);
            Assert.AreEqual(heating, house.Heating);
            Assert.AreEqual(bathtub, house.Bathtub);
            Assert.AreEqual(elevator, house.Elevator);
        }

        [Test]
        public void UpdateHouseInstanceSuccessTest_TestsThatAHouseInstanceIsCreatedAndThenUpdatedAsExpected_VerifiesByTheReturnValue()
        {
            // Create and save the instance and then update it with new values
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
            string propertyType = "Apartment";
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
            string area = "Pindora, Rawalpindi, Pakistan";
            long monthlyRent = 90000;
            string image1 = "Image1.jpg";
            string image2 = "Image2.png";
            string rentUnit = "Hour";
            bool isShared = true;

            bool ac = true;
            bool geyser = true;
            bool balcony = true;
            bool lawn = true;
            bool heating = true;
            bool cctvCameras = true;
            bool backupElectricity = true;
            bool bathtub = true;
            bool elevator = true;

            House house = new House.HouseBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title).OwnerName(name)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(cableTv)
                .GarageAvailable(garage).LandlinePhoneAvailable(landline).SmokingAllowed(smokingAllowed).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).LandlineNumber(null).Fax(null).AC(ac).Geyser(geyser)
                .Balcony(balcony).Lawn(lawn).Heating(heating).CctvCameras(cctvCameras)
                .BackupElectricity(backupElectricity).Bathtub(bathtub).Elevator(elevator)
                .Build();
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
            Assert.AreEqual(monthlyRent, house.RentPrice);

            Assert.AreEqual(dimension.DimensionType, house.Dimension.DimensionType);
            Assert.AreEqual(dimension.StringValue, house.Dimension.StringValue);
            Assert.AreEqual(dimension.DecimalValue, house.Dimension.DecimalValue);

            Assert.AreEqual(image1, house.Images[0]);
            Assert.AreEqual(image2, house.Images[1]);

            Assert.AreEqual(isShared, house.IsShared);
            Assert.AreEqual(rentUnit, house.RentUnit);

            Assert.AreEqual(ac, house.AC);
            Assert.AreEqual(geyser, house.Geyser);
            Assert.AreEqual(balcony, house.Balcony);
            Assert.AreEqual(lawn, house.Lawn);
            Assert.AreEqual(cctvCameras, house.CctvCameras);
            Assert.AreEqual(backupElectricity, house.BackupElectricity);
            Assert.AreEqual(heating, house.Heating);
            Assert.AreEqual(bathtub, house.Bathtub);
            Assert.AreEqual(elevator, house.Elevator);

            // NOW UDPATE THE VALUES
            // Create and save the instance and then update it with new values
            string title2 = "Title No 2";
            string description2 = "Description of house update";
            string email2 = "w@12344321-3.com";
            string name2 = "OwnerName2";
            string phoneNumber2 = "03455138018";
            
            // No Latitude is given. So the house instance should not be created
            int numberOfBedrooms2 = 3;
            int numberofBathrooms2 = 3;
            int numberOfKitchens2 = 2;
            bool smokingAllowed2 = false;
            bool landline2 = true;
            bool cableTv2 = false;
            bool internet2 = true;
            bool garage2 = false;
            decimal latitude2 = 25.13M;
            decimal longitude2 = 73.11M;
            string propertyType2 = "House";
            GenderRestriction genderRestriction2 = GenderRestriction.BoysOnly;
            string area2 = "Pindora, Rawalpindi, Pakistan";
            long monthlyRent2 = 199000;
            string rentUnit2 = "Day";
            bool isShared2 = false;

            bool ac2 = false;
            bool geyser2 = false;
            bool balcony2 = false;
            bool lawn2 = false;
            bool heating2 = false;
            bool cctvCameras2 = false;
            bool backupElectricity2 = false;
            bool bathtub2 = false;
            bool elevator2 = false;

            Dimension dimension2 = new Dimension(DimensionType.Marla, "20", 0, house);
            house.UpdateHouse(title2,monthlyRent2, numberOfBedrooms2, numberOfKitchens2, numberofBathrooms2, internet2, landline2,
                cableTv2, dimension2, garage2, smokingAllowed2, propertyType2, email2, phoneNumber2, null, null, area2, name2, 
                description2, genderRestriction2, latitude2, longitude2, isShared2, rentUnit2, null, null,
                ac2,geyser2, balcony2, lawn2, cctvCameras2, backupElectricity2, heating2, bathtub2,
                elevator2);
            
            Assert.AreEqual(title2, house.Title);
            Assert.AreEqual(description2, house.Description);
            Assert.AreEqual(email2, house.OwnerEmail);
            Assert.AreEqual(name2, house.OwnerName);
            Assert.AreEqual(phoneNumber2, house.OwnerPhoneNumber);
            Assert.AreEqual(numberOfBedrooms2, house.NumberOfBedrooms);
            Assert.AreEqual(numberofBathrooms2, house.NumberOfBathrooms);
            Assert.AreEqual(numberOfKitchens2, house.NumberOfKitchens);
            Assert.AreEqual(smokingAllowed2, house.SmokingAllowed);
            Assert.AreEqual(landline2, house.LandlinePhoneAvailable);
            Assert.AreEqual(cableTv2, house.CableTvAvailable);
            Assert.AreEqual(internet2, house.InternetAvailable);
            Assert.AreEqual(garage2, house.GarageAvailable);
            Assert.AreEqual(latitude2, house.Latitude);
            Assert.AreEqual(longitude2, house.Longitude);
            Assert.AreEqual(propertyType2, house.PropertyType);
            Assert.AreEqual(genderRestriction2, house.GenderRestriction);
            Assert.AreEqual(area2, house.Area);
            Assert.AreEqual(monthlyRent2, house.RentPrice);

            Assert.AreEqual(dimension2.DimensionType, house.Dimension.DimensionType);
            Assert.AreEqual(dimension2.StringValue, house.Dimension.StringValue);
            Assert.AreEqual(dimension2.DecimalValue, house.Dimension.DecimalValue);
            
            Assert.AreEqual(isShared2, house.IsShared);
            Assert.AreEqual(rentUnit2, house.RentUnit);

            Assert.AreEqual(ac2, house.AC);
            Assert.AreEqual(geyser2, house.Geyser);
            Assert.AreEqual(balcony2, house.Balcony);
            Assert.AreEqual(lawn2, house.Lawn);
            Assert.AreEqual(cctvCameras2, house.CctvCameras);
            Assert.AreEqual(backupElectricity2, house.BackupElectricity);
            Assert.AreEqual(heating2, house.Heating);
            Assert.AreEqual(bathtub2, house.Bathtub);
            Assert.AreEqual(elevator2, house.Elevator);
        }

        [Test]
        public void RentUnitPropertyTypeAndIsSharedDefaultValueTest_TestsThatAHouseInstanceIsCreatedEvenIfValueIsnotProvidedForTheseProperties_VerifiesByTheReturnValue()
        {
            // If we dont provide RentUnit and IsShared, then the default value should be provided
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            
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
            string propertyType = "House";
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
            string area = "Pindora, Rawalpindi, Pakistan";
            long monthlyRent = 90000;
            string image1 = "Image1.jpg";
            string image2 = "Image2.png";
            
            House house = new House.HouseBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title).OwnerName(name)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(cableTv)
                .GarageAvailable(garage).LandlinePhoneAvailable(landline).SmokingAllowed(smokingAllowed).WithInternetAvailable(internet)
                .RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).PropertyType(propertyType).Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, "10", 0, house);
            house.Dimension = dimension;
            house.AddImage(image1);
            house.AddImage(image2);
            Assert.IsNotNull(house);

            // Check that the default valuesa are used
            string rentUnit = House.GetAllRentUnits()[0];
            bool isShared = false;
            Assert.AreEqual(rentUnit, house.RentUnit);
            Assert.AreEqual(propertyType, house.PropertyType);
            Assert.AreEqual(isShared, house.IsShared);
        }

        // Both Mobile Number and Landline Phone number have not been given, wso raise and exception
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateHouseInstanceFailTest_TestsThatBothMobileAndLandlineNumbersAreRequired_VerifiesByTheReturnValue()
        {
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";

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
            string propertyType = "Apartment";
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
            string area = "Pindora, Rawalpindi, Pakistan";
            long monthlyRent = 90000;
            string image1 = "Image1.jpg";
            string image2 = "Image2.png";
            string rentUnit = "Hour";
            bool isShared = true;

            House house = new House.HouseBuilder().OwnerEmail(email).OwnerPhoneNumber(null).Title(title).OwnerName(name)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(cableTv)
                .GarageAvailable(garage).LandlinePhoneAvailable(landline).SmokingAllowed(smokingAllowed).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared)
                .Build();
        }

        // Checks that House instance can be created with only a Landline Number provided
        [Test]
        public void CreateHouseInstanceWithLandlineNumberTest_TestsThatInstanceGetsCreatedWithJustTheLanslineNumber_VerifiesByTheReturnValue()
        {
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";

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
            string propertyType = "Apartment";
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
            string area = "Pindora, Rawalpindi, Pakistan";
            long monthlyRent = 90000;
            string image1 = "Image1.jpg";
            string image2 = "Image2.png";
            string rentUnit = "Hour";
            bool isShared = true;
            string landlineNumber = "0510000000";

            House house = new House.HouseBuilder().OwnerEmail(email).OwnerPhoneNumber(null).Title(title).OwnerName(name)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(cableTv)
                .GarageAvailable(garage).LandlinePhoneAvailable(landline).SmokingAllowed(smokingAllowed).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).LandlineNumber(landlineNumber)
                .Build();
            Assert.IsNotNull(house);
            Assert.AreEqual(landlineNumber, house.LandlineNumber);
            Assert.IsTrue(string.IsNullOrWhiteSpace(house.OwnerPhoneNumber));
        }

        // Checks that House instance can be created with only a Mobile Number provided without a landline number
        [Test]
        public void CreateHouseInstanceWithMobileNumberTest_TestsThatInstanceGetsCreatedWithJustTheLanslineNumber_VerifiesByTheReturnValue()
        {
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";

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
            string propertyType = "Apartment";
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
            string area = "Pindora, Rawalpindi, Pakistan";
            long monthlyRent = 90000;
            string image1 = "Image1.jpg";
            string image2 = "Image2.png";
            string rentUnit = "Hour";
            bool isShared = true;
            string mobileNumber = "03990000000";

            House house = new House.HouseBuilder().OwnerEmail(email).OwnerPhoneNumber(null).Title(title).OwnerName(name)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(cableTv)
                .GarageAvailable(garage).LandlinePhoneAvailable(landline).SmokingAllowed(smokingAllowed).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).OwnerPhoneNumber(mobileNumber)
                .Build();
            Assert.IsNotNull(house);
            Assert.AreEqual(mobileNumber, house.OwnerPhoneNumber);
            Assert.IsTrue(string.IsNullOrWhiteSpace(house.LandlineNumber));
        }

        // Checks that House instance will not be created if the PropertyType is anything other than House or 
        // Apartment
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateHouseFailForPropertyTypeTest_TestsThatInstanceWillNotGetCreatedWhenAnyOtherPropertyTypeisProvided_VerifiesByTheReturnValue()
        {
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";

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
            string propertyType = "Hostel";
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
            string area = "Pindora, Rawalpindi, Pakistan";
            long monthlyRent = 90000;
            string image1 = "Image1.jpg";
            string image2 = "Image2.png";
            string rentUnit = "Hour";
            bool isShared = true;
            string mobileNumber = "03990000000";

            House house = new House.HouseBuilder().OwnerEmail(email).OwnerPhoneNumber(null).Title(title).OwnerName(name)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(cableTv)
                .GarageAvailable(garage).LandlinePhoneAvailable(landline).SmokingAllowed(smokingAllowed).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit)
                .IsShared(isShared).OwnerPhoneNumber(mobileNumber)
                .Build();
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHouseFailTest1_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            string title = "Title No 1";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            string propertyType = "Apartment";

            // No Latitude is given. So the house instance should not be created
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long monthlyRent = 90000;

            House house = new House.HouseBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title).OwnerName(name)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(propertyType).RentPrice(monthlyRent).Longitude(73.41M)
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
            string mobileNumber = "03990000000";
            string propertyType = "Apartment";

            House house = new House.HouseBuilder().OwnerEmail(email)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(73.41M)
                .HouseNo("123").Area("Pindora").StreetNo("13").OwnerPhoneNumber(mobileNumber).Build();
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
            string propertyType = "Apartment";

            House house = new House.HouseBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title).OwnerName(name)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(73.41M)
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
            string propertyType = "Apartment";

            // No Rent is given. So the house instance should not be created
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            
            House house = new House.HouseBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title).OwnerName(name)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(propertyType).Latitude(23.45M).Longitude(73.41M)
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
            string propertyType = "Apartment";

            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long monthlyRent = 1000001;

            House house = new House.HouseBuilder().Title(title).RentPrice(monthlyRent).OwnerEmail(email)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(propertyType).RentPrice(130000).Latitude(23.65M).Longitude(73.41M)
                .HouseNo("123").Area("Pindora").StreetNo("13").OwnerPhoneNumber(phoneNumber).Build();
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
            string propertyType = "Apartment";

            House house = new House.HouseBuilder().Title(title).RentPrice(monthlyRent)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(propertyType).RentPrice(130000).Latitude(23.65M).Longitude(73.41M)
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
            string propertyType = "Apartment";

            House house = new House.HouseBuilder().OwnerEmail(email).RentPrice(monthlyRent)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(propertyType).RentPrice(130000).Latitude(23.65M).Longitude(73.41M)
                .HouseNo("123").Area("Pindora").StreetNo("13").OwnerPhoneNumber(phoneNumber).OwnerName(name).Build();
        }

        [Test]
        public void
            PhoneNumberRegexSuccessTest_ChecksThatTheFoneNumberFormatAcceptedByTheHouseIsAsExpected_VerifiesIfTheInstanceIsCreated
            ()
        {
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            string phoneNumber = "03455138018";
            
            decimal latitude = 25.43M;
            decimal longitude = 73.41M;
            string propertyType = "Apartment";
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
            string area = "Pindora, Rawalpindi, Pakistan";
            long monthlyRent = 90000;
            
            House house = new House.HouseBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title).OwnerName(name)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, "10", 0, house);
            house.Dimension = dimension;
            Assert.IsNotNull(house);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void
            PhoneNumberRegexFailTest_ChecksThatThePhoneNumberValidationFailsBecauseItHasMoreIntegersThanExpected_VerifiesIfTheInstanceIsCreated
            ()
        {
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            // Will fail because the phone number has more than 11 characters
            string phoneNumber = "034551380181";

            decimal latitude = 25.43M;
            decimal longitude = 73.41M;
            string propertyType = "Apartment";
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
            string area = "Pindora, Rawalpindi, Pakistan";
            long monthlyRent = 90000;

            House house = new House.HouseBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title).OwnerName(name)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, "10", 0, house);
            house.Dimension = dimension;
            Assert.IsNotNull(house);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void
            PhoneNumberRegexFailTest_ChecksThatThePhoneNumberValidationFailsBecauseItHasLessIntegersThanExpected_VerifiesIfTheInstanceIsCreated
            ()
        {
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            // Will fail because the phone number has less than 11 characters
            string phoneNumber = "0345513801";

            decimal latitude = 25.43M;
            decimal longitude = 73.41M;
            string propertyType = "Apartment";
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
            string area = "Pindora, Rawalpindi, Pakistan";
            long monthlyRent = 90000;

            House house = new House.HouseBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title).OwnerName(name)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, "10", 0, house);
            house.Dimension = dimension;
            Assert.IsNotNull(house);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void
            PhoneNumberRegexFailTest_ChecksThatThePhoneNumberValidationFailsBecauseItHasOneAlphabet_VerifiesIfTheInstanceIsCreated
            ()
        {
            string title = "Title No 1";
            string description = "Description of house";
            string email = "w@12344321.com";
            string name = "OwnerName";
            // Will fail because it contains an alphabet
            string phoneNumber = "0345513801d";

            decimal latitude = 25.43M;
            decimal longitude = 73.41M;
            string propertyType = "Apartment";
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
            string area = "Pindora, Rawalpindi, Pakistan";
            long monthlyRent = 90000;

            House house = new House.HouseBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title).OwnerName(name)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).Build();
            Dimension dimension = new Dimension(DimensionType.Kanal, "10", 0, house);
            house.Dimension = dimension;
            Assert.IsNotNull(house);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidRentUnitValueFailTest_TestsThatAHouseInstanceIsCreatedAsExpected_VerifiesByTheReturnValue()
        {
            // The value of RentUnit provided is not supported by the system so an exception is raised
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
            string propertyType = "Apartment";
            GenderRestriction genderRestriction = GenderRestriction.GirlsOnly;
            string area = "Pindora, Rawalpindi, Pakistan";
            long monthlyRent = 90000;
            string image1 = "Image1.jpg";
            string image2 = "Image2.png";
            string rentUnit = "Hourly";

            House house = new House.HouseBuilder().OwnerEmail(email).OwnerPhoneNumber(phoneNumber).Title(title).OwnerName(name)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(cableTv)
                .GarageAvailable(garage).LandlinePhoneAvailable(landline).SmokingAllowed(smokingAllowed).WithInternetAvailable(internet)
                .PropertyType(propertyType).RentPrice(monthlyRent).Latitude(latitude).Longitude(longitude)
                .Area(area).GenderRestriction(genderRestriction).Description(description).RentUnit(rentUnit).Build();
        }
    }
}
