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
using RentStuff.Identity.Application.Account.Representations;
using RentStuff.Identity.Application.Ninject.Modules;
using RentStuff.Identity.Infrastructure.Persistence.Ninject.Modules;
using RentStuff.Identity.Infrastructure.Persistence.Repositories;
using RentStuff.Identity.Infrastructure.Services.Identity;
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

            // Get the user and verify the results
            var getHttpResult = accountController.GetUser(email);
            var userRepresentation = ((OkNegotiatedContentResult<UserRepresentation>)getHttpResult).Content;
            Assert.IsNotNull(userRepresentation);
            Assert.AreEqual(email, userRepresentation.Email);
            Assert.AreEqual(name, userRepresentation.FullName);
            Assert.IsFalse(userRepresentation.IsEmailConfirmed);
        }

        [Test]
        public void RegisterFailTest_ChecksWhenNullValueisProvidedExceptionisRaised_VerifiesByTheRaisedException()
        {
            var kernel = InitializeNinjectDepedencyInjection();
            AccountController accountController = kernel.Get<AccountController>();

            Assert.NotNull(accountController);

            // Null is provided
            var httpActionResult = accountController.Register(null);
            Assert.AreEqual(httpActionResult.GetType().Name, typeof(BadRequestErrorMessageResult).Name);
        }

        [Test]
        public void RegisterFailTest_ChecksWhenAnObjectOtherThanCreateUserIsSentValueisProvidedExceptionisRaised_VerifiesByTheRaisedException()
        {
            var kernel = InitializeNinjectDepedencyInjection();
            AccountController accountController = kernel.Get<AccountController>();

            Assert.NotNull(accountController);

            // ActivateCommand is provided instead of the accepted RegisterCommand
            var httpActionResult = accountController.Register(JsonConvert.SerializeObject(
                new ActivateAccountCommand("dummy@dumdum1234567.com", "dummy")));
            Assert.AreEqual(httpActionResult.GetType().Name, typeof(InternalServerErrorResult).Name);
        }

        [Test]
        public void ActivationFailTest_ChecksThatFalseTokenCannotActivateAccount_VerifiesByRetrievingInstanceFromDatabase()
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

            // Get the user and verify the results
            var getHttpResult = accountController.GetUser(email);
            var userRepresentation = ((OkNegotiatedContentResult<UserRepresentation>)getHttpResult).Content;
            Assert.IsNotNull(userRepresentation);
            Assert.AreEqual(email, userRepresentation.Email);
            Assert.AreEqual(name, userRepresentation.FullName);
            Assert.IsFalse(userRepresentation.IsEmailConfirmed);

            var activateHttpResult = accountController.Activate(JsonConvert.SerializeObject(
                new ActivateAccountCommand(email,
                // This is a dummy value
                "23refdefee232edef3r423t#!%&&**(()^3vd^$$^^$CDFW87867")));
            Assert.AreEqual(typeof(InternalServerErrorResult).Name, activateHttpResult.GetType().Name);
        }

        // Forgot password fails because the object provided is null
        [Test]
        public void ForgotPasswordFailTest_RequestFailsasTheObjetProvidedIsNull_VerifiesByRetrievingInstanceFromDatabase()
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

            // Get the user and verify the results
            var getHttpResult = accountController.GetUser(email);
            var userRepresentation = ((OkNegotiatedContentResult<UserRepresentation>)getHttpResult).Content;
            Assert.IsNotNull(userRepresentation);
            Assert.AreEqual(email, userRepresentation.Email);
            Assert.AreEqual(name, userRepresentation.FullName);
            Assert.IsFalse(userRepresentation.IsEmailConfirmed);

            var activateHttpResult = accountController.ForgotPassword(null);
            Assert.AreEqual(typeof(BadRequestErrorMessageResult).Name, activateHttpResult.GetType().Name);
        }

        // Forgot password request fails because the email provided is empty
        [Test]
        public void ForgotPasswordFailTest_RequestFailsasTheEmailIsEmpty_VerifiesByRetrievingInstanceFromDatabase()
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

            // Get the user and verify the results
            var getHttpResult = accountController.GetUser(email);
            var userRepresentation = ((OkNegotiatedContentResult<UserRepresentation>)getHttpResult).Content;
            Assert.IsNotNull(userRepresentation);
            Assert.AreEqual(email, userRepresentation.Email);
            Assert.AreEqual(name, userRepresentation.FullName);
            Assert.IsFalse(userRepresentation.IsEmailConfirmed);

            var activateHttpResult = accountController.ForgotPassword(new ForgotPasswordCommand(""));
            Assert.AreEqual(typeof(BadRequestErrorMessageResult).Name, activateHttpResult.GetType().Name);
        }

        // Forgot password request fails because the account is not yet activated
        [Test]
        public void ForgotPasswordFailTest_RequestFailsAsTheAccountIsNotYetActivated_VerifiesByRetrievingInstanceFromDatabase()
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

            // Get the user and verify the results
            var getHttpResult = accountController.GetUser(email);
            var userRepresentation = ((OkNegotiatedContentResult<UserRepresentation>)getHttpResult).Content;
            Assert.IsNotNull(userRepresentation);
            Assert.AreEqual(email, userRepresentation.Email);
            Assert.AreEqual(name, userRepresentation.FullName);
            Assert.IsFalse(userRepresentation.IsEmailConfirmed);

            var activateHttpResult = accountController.ForgotPassword(new ForgotPasswordCommand(email));
            Assert.AreEqual(typeof(InternalServerErrorResult).Name, activateHttpResult.GetType().Name);
        }

        // ResetPassword request fails as there is no active reset password request against this account
        [Test]
        public void ResetPasswordFailTest_RequestFailsAsThereIsNoActiveResetPasswordRequestAgainstThisAccount_VerifiesByRetrievingInstanceFromDatabase()
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

            // Get the user and verify the results
            var getHttpResult = accountController.GetUser(email);
            var userRepresentation = ((OkNegotiatedContentResult<UserRepresentation>)getHttpResult).Content;
            Assert.IsNotNull(userRepresentation);
            Assert.AreEqual(email, userRepresentation.Email);
            Assert.AreEqual(name, userRepresentation.FullName);
            Assert.IsFalse(userRepresentation.IsEmailConfirmed);

            var activateHttpResult = accountController.ResetPassword(new ResetPasswordCommand(email,
                "TheStaff123!", "TheStaff123!", "123143234^&^45hjjkj*)hft54"));
            Assert.AreEqual(typeof(InternalServerErrorResult).Name, activateHttpResult.GetType().Name);
        }

        // ResetPassword request fails as the provided object is empty
        [Test]
        public void ResetPasswordFailTest_RequestFailsAsTheObjectSuppliedIsEmpty_VerifiesByRetrievingInstanceFromDatabase()
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

            // Get the user and verify the results
            var getHttpResult = accountController.GetUser(email);
            var userRepresentation = ((OkNegotiatedContentResult<UserRepresentation>)getHttpResult).Content;
            Assert.IsNotNull(userRepresentation);
            Assert.AreEqual(email, userRepresentation.Email);
            Assert.AreEqual(name, userRepresentation.FullName);
            Assert.IsFalse(userRepresentation.IsEmailConfirmed);

            var activateHttpResult = accountController.ResetPassword(null);
            Assert.AreEqual(typeof(BadRequestErrorMessageResult).Name, activateHttpResult.GetType().Name);
        }

        // GetUser Fails as the email provided is empty
        [Test]
        public void GetUserFailTest_TRequestfailsWhenTheEmailisnotProvided_VerifiesThroughTheReturnedResult()
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

            // Get the user and verify the results
            var getHttpResult = accountController.GetUser("");
            Assert.AreEqual(typeof(BadRequestErrorMessageResult).Name, getHttpResult.GetType().Name);
        }
    }
}
