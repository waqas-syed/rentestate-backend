using NUnit.Framework;
using RentStuff.Property.Application.HouseServices;
using RentStuff.Property.Application.HouseServices.Commands;
using Spring.Context.Support;

namespace RentStuff.Property.Application.IntegrationTests
{
    [TestFixture]
    public class HouseApplicationServiceTests
    {
        [Test]
        public void SaveHouseTest_TestsThatANewHouseIsSavedInTheDatabaseAsExpected_VerifiesByOutput()
        {
            IHouseApplicationService houseApplicationService =
                (IHouseApplicationService) ContextRegistry.GetContext()["HouseApplicationService"];
            
            string email = "w@12344321.com";
            string phoneNumber = "+923331234567";
            long monthlyRent = 130000;
            int numberOfBedrooms = 3;
            int numberofBathrooms = 1;
            int numberOfKitchens = 1;
            string houseNo = "747";
            string streetNo = "13";
            string area = "1600+Amphitheatre+Parkway,+Mountain+View,+CA";
            decimal latitude = 33.39M;
            decimal longitude = 73.41M;
            var createNewHouseCommand = new CreateHouseCommand(monthlyRent, numberOfBedrooms, numberOfKitchens, numberofBathrooms,
                false, false, true, true, true, true, true, true, "Apartment", email, phoneNumber, latitude,
                longitude, houseNo, streetNo, area);
            bool houseCreated = houseApplicationService.SaveNewHouseOffer(createNewHouseCommand);
            Assert.IsTrue(houseCreated);
        }
    }
}
