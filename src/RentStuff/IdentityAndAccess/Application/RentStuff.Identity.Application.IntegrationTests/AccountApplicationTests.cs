using System;
using System.Configuration;
using NUnit.Framework;
using RentStuff.Common;
using RentStuff.Identity.Application.Account;
using RentStuff.Identity.Application.Account.Commands;
using RentStuff.Identity.Application.Account.Representations;
using Spring.Context.Support;

namespace RentStuff.Identity.Application.IntegrationTests
{
    [TestFixture]
    public class AccountApplicationTests
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
        public void RegisterUserTest_RegistersAUserAndActivatesHerAccount_VerifiesByDatabaseObjectRetreivalForThatUser()
        {
            IAccountApplicationService accountApplicationService = (IAccountApplicationService)ContextRegistry.GetContext()["AccountApplicationService"];
            //IAccountRepository accountRepository = (IAccountRepository)ContextRegistry.GetContext()["AccountRepository"];
            Assert.NotNull(accountApplicationService);
            string name = "Gandalf The Grey";
            string email = "gandalfthegrey@wizardry123456.com";
            string password = "TheStaff123!";
            
            UserRepresentation userRepresentation = accountApplicationService.GetUserByEmail(email);
            Assert.Null(userRepresentation);

            // Register the user
            CreateUserCommand createUserCommand = new CreateUserCommand(name, email, password, password);
            bool registerResult = accountApplicationService.Register(createUserCommand);
            Assert.IsTrue(registerResult);

            // Retreive the registered user and verify that the email has not yet been confirmed
            userRepresentation = accountApplicationService.GetUserByEmail(email);
            Assert.NotNull(userRepresentation);
            Assert.AreEqual(name, userRepresentation.FullName);
            Assert.AreEqual(email, userRepresentation.Email);
            Assert.IsFalse(userRepresentation.IsEmailConfirmed);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public async void RegisterUserFailTest_ChecksExceptionIsThrownWhenEmailIsNull_VerifiesByRaisedException()
        {
            IAccountApplicationService accountApplicationService = (IAccountApplicationService)ContextRegistry.GetContext()["AccountApplicationService"];
            Assert.NotNull(accountApplicationService);
            string name = "Gandalf The Grey";
            string email = "";
            string password = "TheStaff123!";
            string confirmPassword = "TheStaff123!!";

            // Register the user
            CreateUserCommand createUserCommand = new CreateUserCommand(name, email, password, password);
            bool registerResult = accountApplicationService.Register(createUserCommand);
            Assert.IsTrue(registerResult);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterUserFailTest_ChecksExceptionIsThrownWhenNameIsNull_VerifiesByRaisedException()
        {
            IAccountApplicationService accountApplicationService = (IAccountApplicationService)ContextRegistry.GetContext()["AccountApplicationService"];
            Assert.NotNull(accountApplicationService);
            string name = "";
            string email = "gandalfthegrey@wizardry123456.com";
            string password = "TheStaff123!";

            // Register the user
            CreateUserCommand createUserCommand = new CreateUserCommand(name, email, password, password);
            bool registerResult = accountApplicationService.Register(createUserCommand);
            Assert.IsTrue(registerResult);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterUserFailTest_ChecksExceptionIsThrownWhenPasswordIsNull_VerifiesByRaisedException()
        {
            IAccountApplicationService accountApplicationService = (IAccountApplicationService)ContextRegistry.GetContext()["AccountApplicationService"];
            Assert.NotNull(accountApplicationService);
            string name = "Gandalf";
            string email = "gandalfthegrey@wizardry123456.com";
            string password = "";

            // Register the user
            CreateUserCommand createUserCommand = new CreateUserCommand(name, email, password, password);
            bool registerResult = accountApplicationService.Register(createUserCommand);
            Assert.IsTrue(registerResult);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterUserFailTest_ChecksExceptionIsThrownWhenNameIsTooLong_VerifiesByRaisedException()
        {
            IAccountApplicationService accountApplicationService = (IAccountApplicationService)ContextRegistry.GetContext()["AccountApplicationService"];
            Assert.NotNull(accountApplicationService);
            // name should be <= 19 characters long
            string name = "Gandalf The Grey - And White";
            string email = "gandalfthegrey@wizardry123456.com";
            string password = "TheStaff123!";

            // Register the user
            CreateUserCommand createUserCommand = new CreateUserCommand(name, email, password, password);
            bool registerResult = accountApplicationService.Register(createUserCommand);
            Assert.IsTrue(registerResult);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterUserFailTest_ChecksThatUserIsNotRegisteredWhenThePasswordAndConfirmPasswordDontMatch_VerifiesByRaisedException()
        {
            IAccountApplicationService accountApplicationService = (IAccountApplicationService)ContextRegistry.GetContext()["AccountApplicationService"];
            Assert.NotNull(accountApplicationService);
            string name = "Gandalf The Grey";
            string email = "gandalfthegrey@wizardry123456.com";
            string password = "TheStaff123!";
            string confirmPassword = "TheStaff123!!";
            
            // Register the user
            CreateUserCommand createUserCommand = new CreateUserCommand(name, email, password, confirmPassword);
            bool registerResult = accountApplicationService.Register(createUserCommand);
            Assert.IsTrue(registerResult);
        }
    }
}
