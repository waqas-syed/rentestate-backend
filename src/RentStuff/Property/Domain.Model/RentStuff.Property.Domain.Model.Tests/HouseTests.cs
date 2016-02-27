using System;
using NUnit.Framework;
using RentStuff.Property.Domain.Model.HouseAggregate;

namespace RentStuff.Property.Domain.Model.Tests
{
    [TestFixture]
    class HouseTests
    {
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHouseFailTest1_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            string email = "w@12344321.com";

            // No Latitude is given. So the house instance should not be created
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long price = 90000;

            House house = new House.HouseBuilder().OwnerEmail(email)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).FamiliesOnly(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(price).Longitude(73.41M)
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
            long price = 90000;

            House house = new House.HouseBuilder().OwnerEmail(email)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).FamiliesOnly(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(price).Latitude(73.41M)
                .HouseNo("123").Area("Pindora").StreetNo("13").Build();
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHouseFailTest3_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            string email = "w@12344321.com";

            // No Area is given. So the house instance should not be created
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            long price = 90000;

            House house = new House.HouseBuilder().OwnerEmail(email)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).FamiliesOnly(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(price).Latitude(73.41M).Longitude(102.54M)
                .HouseNo("123").StreetNo("13").Build();
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SaveHouseFailTest4_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            string email = "w@12344321.com";

            // No Rent is given. So the house instance should not be created
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            
            House house = new House.HouseBuilder().OwnerEmail(email)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).FamiliesOnly(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).Latitude(23.65M).Longitude(73.41M)
                .HouseNo("123").Area("Pindora").StreetNo("13").Build();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void SaveHouseFailTest5_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            string email = "w@12344321.com";

            // Both for boys and for girls is allowed, so exception should be raised
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;

            House house = new House.HouseBuilder().OwnerEmail(email)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).GirlsOnly(true).BoysOnly(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(130000).Latitude(23.65M).Longitude(73.41M)
                .HouseNo("123").Area("Pindora").StreetNo("13").Build();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void SaveHouseFailTest6_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            string email = "w@12344321.com";

            // Both for boys and for girls is allowed, so exception should be raised
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;

            House house = new House.HouseBuilder().OwnerEmail(email)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).GirlsOnly(true).BoysOnly(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(130000).Latitude(23.65M).Longitude(73.41M)
                .HouseNo("123").Area("Pindora").StreetNo("13").Build();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void SaveHouseFailTest7_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            string email = "w@12344321.com";

            // Both for families and for girls is allowed, so exception should be raised
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;

            House house = new House.HouseBuilder().OwnerEmail(email)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).FamiliesOnly(true).BoysOnly(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(130000).Latitude(23.65M).Longitude(73.41M)
                .HouseNo("123").Area("Pindora").StreetNo("13").Build();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void SaveHouseFailTest8_VerifiesThatTheHouseInstanceCreationFailsWhenInputIsNotSuitable_VerifiesThroughDatabaseQuery()
        {
            string email = "w@12344321.com";

            // Both for families and for girls is allowed, so exception should be raised
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;

            House house = new House.HouseBuilder().OwnerEmail(email)
                .NumberOfBedrooms(numberOfBedrooms).NumberOfBathrooms(numberofBathrooms)
                .NumberOfKitchens(numberOfKitchens).CableTvAvailable(true).FamiliesOnly(true).GirlsOnly(true)
                .GarageAvailable(true).LandlinePhoneAvailable(true).SmokingAllowed(false).WithInternetAvailable(true)
                .PropertyType(PropertyType.Apartment).MonthlyRent(130000).Latitude(23.65M).Longitude(73.41M)
                .HouseNo("123").Area("Pindora").StreetNo("13").Build();
        }
    }
}
