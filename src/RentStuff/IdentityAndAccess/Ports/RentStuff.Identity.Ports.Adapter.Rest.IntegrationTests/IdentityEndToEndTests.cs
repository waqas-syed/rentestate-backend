using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RentStuff.Common;
using RentStuff.Identity.Infrastructure.Persistence.Repositories;
using RentStuff.Identity.Ports.Adapter.Rest.Controllers;
using Spring.Context.Support;

namespace RentStuff.Identity.Ports.Adapter.Rest.IntegrationTests
{
    [TestFixture]
    public class IdentityEndToEndTests
    {
        private DatabaseUtility _databaseUtility;

        [SetUp]
        public void Setup()
        {
            var connection = ConfigurationManager.ConnectionStrings["MySql"].ToString();
            _databaseUtility = new DatabaseUtility(connection);
            _databaseUtility.Create();
            //_databaseUtility.Populate();
        }

        [TearDown]
        public void Teardown()
        {
            _databaseUtility.Create();
        }

        [Test]
        public void RegisterUserTest_TestsIfTheUserIsSavedAsExpectedWhenRegisterMethodIsCalled_VerifiesThroughTheRetruendValue()
        {
            AccountController accountRepository = (AccountController) ContextRegistry.GetContext()["AccountController"];
            Assert.NotNull(accountRepository);
        }
    }
}
