using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json;
using Ninject;
using NUnit.Framework;
using RentStuff.Common;
using RentStuff.Common.Utilities;
using RentStuff.Identity.Application.Account.Commands;
using RentStuff.Identity.Application.Ninject.Modules;
using RentStuff.Identity.Infrastructure.Persistence.Ninject.Modules;
using RentStuff.Identity.Infrastructure.Persistence.Repositories;
using RentStuff.Identity.Infrastructure.Services.Ninject.Modules;
using RentStuff.Identity.Ports.Adapter.Rest.Ninject.Modules;
using RentStuff.Identity.Ports.Adapter.Rest.Resources;

namespace RentStuff.Identity.Ports.Adapter.Rest.IntegrationTests
{
    [TestFixture]
    public class IdentityEndToEndTests
    {
        private DatabaseUtility _databaseUtility;

        [SetUp]
        public void Setup()
        {
            var connection = StringCipher.DecipheredConnectionString;
            _databaseUtility = new DatabaseUtility(connection);
            _databaseUtility.Create();
            //_databaseUtility.Populate();
        }

        [TearDown]
        public void Teardown()
        {
            _databaseUtility.Create();
        }

        /// <summary>
        /// Loads the dependencies defined in Ninject modules and injects them where necessary
        /// </summary>
        private IKernel InitializeNinjectDepedencyInjection()
        {
            var kernel = new StandardKernel();
            kernel.Load<MockIdentityAccessServicesNinjectModule>();
            kernel.Load<IdentityAccessPersistenceNinjectModule>();
            kernel.Load<IdentityAccessApplicationNinjectModule>();
            kernel.Load<IdentityAccessPortsNinjectModule>();
            return kernel;
        }

        [Test]
        public void RegisterUserTest_TestsIfTheUserIsSavedAsExpectedWhenRegisterMethodIsCalled_VerifiesThroughTheRetruendValue()
        {
            var kernel = InitializeNinjectDepedencyInjection();
            AccountController accountController = kernel.Get<AccountController>();
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
