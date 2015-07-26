using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RentStuff.Property.Application.HouseServices;
using Spring.Context.Support;

namespace RentStuff.Property.Application.IntegrationTests
{
    [TestFixture]
    public class HouseApplicationServiceTests
    {
        [Test]
        public void InitialzieHouseApplicatioNServiceTest_VerifiesThaTheServiceIsINitializedAsExpected_VerfifiesThroughTheInstanceValue()
        {
            IHouseApplicationService houseApplicationService =
                (IHouseApplicationService) ContextRegistry.GetContext()["HouseApplicationService"];
            Assert.NotNull(houseApplicationService);
        }
    }
}
