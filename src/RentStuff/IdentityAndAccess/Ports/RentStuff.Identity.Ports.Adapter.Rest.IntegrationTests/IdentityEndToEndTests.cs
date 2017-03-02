using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json;
using NUnit.Framework;
using RentStuff.Common;
using RentStuff.Identity.Application.Account.Commands;
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
        public async void RegisterUserTest_TestsIfTheUserIsSavedAsExpectedWhenRegisterMethodIsCalled_VerifiesThroughTheRetruendValue()
        {
            AccountController accountController = (AccountController) ContextRegistry.GetContext()["AccountController"];
            Assert.NotNull(accountController);

            string name = "Thorin";
            // Email is used as both Email and username
            string email = "thorin@dummyemail123456.com";
            string password = "Erebor123!";
            CreateUserCommand createUserCommand = new CreateUserCommand(name, email, password, password);
            IHttpActionResult registerHttpActionResult = accountController.Register(JsonConvert.SerializeObject(createUserCommand));
            Assert.NotNull(registerHttpActionResult);
            var registerResponse = ((OkNegotiatedContentResult<bool>)registerHttpActionResult).Content;
            Assert.IsTrue(registerResponse);
        }
    }
}
