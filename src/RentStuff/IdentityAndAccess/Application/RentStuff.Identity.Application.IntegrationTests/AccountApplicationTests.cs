using System;
using System.Configuration;
using NUnit.Framework;
using RentStuff.Common;
using RentStuff.Identity.Application.Account;
using RentStuff.Identity.Application.Account.Commands;
using RentStuff.Identity.Application.Account.Representations;
using RentStuff.Identity.Infrastructure.Services.Email;
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
            
            // Register the user
            CreateUserCommand createUserCommand = new CreateUserCommand(name, email, password, password);
            string registerResult = accountApplicationService.Register(createUserCommand);
            Assert.IsFalse(string.IsNullOrWhiteSpace(registerResult));

            // Retreive the registered user and verify that the email has not yet been confirmed
            UserRepresentation userRepresentation = accountApplicationService.GetUserByEmail(email);
            Assert.NotNull(userRepresentation);
            Assert.AreEqual(name, userRepresentation.FullName);
            Assert.AreEqual(email, userRepresentation.Email);
            Assert.IsFalse(userRepresentation.IsEmailConfirmed);
        }

        [Test]
        public void RegisterAndActivateUserTest_RegistersAUserAndActivatesHerAccount_VerifiesByDatabaseObjectRetreivalForThatUser()
        {
            IAccountApplicationService accountApplicationService = (IAccountApplicationService)ContextRegistry.GetContext()["AccountApplicationService"];
            IEmailTokenGenerationService emailTokenGenerationService = (IEmailTokenGenerationService)ContextRegistry.GetContext()["EmailTokenGenerationService"];
            Assert.NotNull(accountApplicationService);
            string name = "Gandalf The Grey";
            string email = "gandalfthegrey@wizardry123456.com";
            string password = "TheStaff123!";
            
            // Register the user
            CreateUserCommand createUserCommand = new CreateUserCommand(name, email, password, password);
            string userId = accountApplicationService.Register(createUserCommand);
            Assert.IsFalse(string.IsNullOrWhiteSpace(userId));

            // Retreive the registered user and verify that the email has not yet been confirmed
            UserRepresentation userRepresentation = accountApplicationService.GetUserByEmail(email);
            Assert.NotNull(userRepresentation);
            Assert.AreEqual(name, userRepresentation.FullName);
            Assert.AreEqual(email, userRepresentation.Email);
            Assert.IsFalse(userRepresentation.IsEmailConfirmed);

            // Generate the email token to pass it to the token verifier
            var emailToken = emailTokenGenerationService.GenerateEmailToken(email, userId);
            // Now Activate the account through email verification, which depends upon UserId and Email of the account being verified
            var activateResult = accountApplicationService.Activate(new ActivateAccountCommand(email, emailToken));
            Assert.IsTrue(activateResult);
            userRepresentation = accountApplicationService.GetUserByEmail(email);
            Assert.IsTrue(userRepresentation.IsEmailConfirmed);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ActivationFailBadTokenTest_ChecksIfActivationFailsIfTheTokenProvidedIsNotValid_VerifiesByTheRaisedException()
        {
            IAccountApplicationService accountApplicationService = (IAccountApplicationService)ContextRegistry.GetContext()["AccountApplicationService"];
            IEmailTokenGenerationService emailTokenGenerationService = (IEmailTokenGenerationService)ContextRegistry.GetContext()["EmailTokenGenerationService"];
            Assert.NotNull(accountApplicationService);
            string name = "Gandalf The Grey";
            string email = "gandalfthegrey@wizardry123456.com";
            string password = "TheStaff123!";

            // Register the user
            CreateUserCommand createUserCommand = new CreateUserCommand(name, email, password, password);
            string userId = accountApplicationService.Register(createUserCommand);
            Assert.IsFalse(string.IsNullOrWhiteSpace(userId));

            // Retreive the registered user and verify that the email has not yet been confirmed
            UserRepresentation userRepresentation = accountApplicationService.GetUserByEmail(email);
            Assert.IsFalse(userRepresentation.IsEmailConfirmed);

            // Generate the email token to pass it to the token verifier
            var emailToken = emailTokenGenerationService.GenerateEmailToken(email, userId);
            // Provide an invalid token
            var activateResult = accountApplicationService.Activate(new ActivateAccountCommand(email, emailToken + "1"));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ActivationFailBadEmailTest_ChecksIfActivationFailsIfTheTokenProvidedIsNotValid_VerifiesByTheRaisedException()
        {
            IAccountApplicationService accountApplicationService = (IAccountApplicationService)ContextRegistry.GetContext()["AccountApplicationService"];
            IEmailTokenGenerationService emailTokenGenerationService = (IEmailTokenGenerationService)ContextRegistry.GetContext()["EmailTokenGenerationService"];
            Assert.NotNull(accountApplicationService);
            string name = "Gandalf The Grey";
            string email = "gandalfthegrey@wizardry123456.com";
            string password = "TheStaff123!";

            // Register the user
            CreateUserCommand createUserCommand = new CreateUserCommand(name, email, password, password);
            string userId = accountApplicationService.Register(createUserCommand);
            Assert.IsFalse(string.IsNullOrWhiteSpace(userId));

            // Retreive the registered user and verify that the email has not yet been confirmed
            UserRepresentation userRepresentation = accountApplicationService.GetUserByEmail(email);
            Assert.IsFalse(userRepresentation.IsEmailConfirmed);

            // Generate the email token to pass it to the token verifier
            var emailToken = emailTokenGenerationService.GenerateEmailToken(email, userId);
            // Provide an invalid email, not the one associated with this token
            string invalidEmail = "gandalfthegrey1@wizardry123456.com";
            var activateResult = accountApplicationService.Activate(new ActivateAccountCommand(invalidEmail, emailToken));
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
            var registerResult = accountApplicationService.Register(createUserCommand);
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
            var registerResult = accountApplicationService.Register(createUserCommand);
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
            var registerResult = accountApplicationService.Register(createUserCommand);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterUserFailTest_ChecksExceptionIsThrownWhenConfirmPasswordIsEmpty_VerifiesByRaisedException()
        {
            IAccountApplicationService accountApplicationService = (IAccountApplicationService)ContextRegistry.GetContext()["AccountApplicationService"];
            Assert.NotNull(accountApplicationService);
            string name = "Gandalf";
            string email = "gandalfthegrey@wizardry123456.com";
            string password = "TheStaff123!";

            // Register the user
            CreateUserCommand createUserCommand = new CreateUserCommand(name, email, password, "");
            var registerResult = accountApplicationService.Register(createUserCommand);
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
            var registerResult = accountApplicationService.Register(createUserCommand);
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
            var registerResult = accountApplicationService.Register(createUserCommand);
        }
    }
}
