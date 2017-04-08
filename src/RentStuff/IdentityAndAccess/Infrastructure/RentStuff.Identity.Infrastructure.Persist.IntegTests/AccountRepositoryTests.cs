using System;
using System.Configuration;
using System.Runtime.Remoting;
using Microsoft.AspNet.Identity;
using NUnit.Framework;
using RentStuff.Common;
using RentStuff.Identity.Infrastructure.Persistence.Repositories;
using RentStuff.Identity.Infrastructure.Services.Email;
using RentStuff.Identity.Infrastructure.Services.Identity;
using Spring.Context.Support;

namespace RentStuff.Identity.Infrastructure.Persist.IntegTests
{
    [TestFixture]
    public class AccountRepositoryTests
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
        public void RegisterUserTest_TestsIfTheUserIsSavedAsExpectedWhenRegisterMethodIsCalled_VerifiesThroughTheRetruendValueUponRetreival()
        {
            IAccountRepository accountRepository = (IAccountRepository)ContextRegistry.GetContext()["AccountRepository"];
            Assert.NotNull(accountRepository);
            
            string name = "Thorin";
            // Email is used as both Email and username
            string email = "thorin@dummyemail123456.com";
            string password = "Erebor123!";

            CustomIdentityUser customIdentityuser = accountRepository.GetUserByPassword(email, password);
            Assert.Null(customIdentityuser);

            IdentityResult result = accountRepository.RegisterUser(name, email, password);            
            Assert.IsTrue(result.Succeeded);
            
            customIdentityuser = accountRepository.GetUserByPassword(email, password);
            Assert.NotNull(customIdentityuser);
            Assert.AreEqual(name, customIdentityuser.FullName);
            Assert.AreEqual(email, customIdentityuser.Email);
            Assert.IsFalse(customIdentityuser.EmailConfirmed);
        }
        
        [Test]
        public void UpdateUserTest_TestsWhetherTheUserGetsUpdatedAsExpected_VerifiesThroughDatabaseRetreival()
        {
            IAccountRepository accountRepository = (IAccountRepository)ContextRegistry.GetContext()["AccountRepository"];
            Assert.NotNull(accountRepository);

            string name = "Thorin";
            // Email is used as both Email and username
            string email = "thorin@dummyemail123456.com";
            string password = "Erebor123!";
            
            // Register the user
            IdentityResult result = accountRepository.RegisterUser(name, email, password);
            Assert.IsTrue(result.Succeeded);
            // Verify it is in the database
            CustomIdentityUser customIdentityuser = accountRepository.GetUserByEmail(email);
            Assert.NotNull(customIdentityuser);
            Assert.AreEqual(name, customIdentityuser.FullName);
            Assert.AreEqual(email, customIdentityuser.Email);
            Assert.AreEqual(email, customIdentityuser.UserName);
            Assert.IsFalse(customIdentityuser.EmailConfirmed);
            
            // Update the user by changing the name and setting EmailConfirmed
            string newName = "New Name";
            customIdentityuser.FullName = newName;
            customIdentityuser.EmailConfirmed = true;
            IdentityResult updateResponse = accountRepository.UpdateUser(customIdentityuser);
            Assert.IsTrue(updateResponse.Succeeded);
            CustomIdentityUser retreivedUser = accountRepository.GetUserByEmail(email);
            Assert.NotNull(retreivedUser);
            Assert.AreEqual(newName, retreivedUser.FullName);
            Assert.AreEqual(email, retreivedUser.Email);
            Assert.AreEqual(email, retreivedUser.UserName);
            Assert.IsTrue(retreivedUser.EmailConfirmed);
        }

        [Test]
        public void RegisterActivateAndPasswordResetSuccessfulTest_ChecksThatThePasswordIsResetSuccessfully_VerifiesThroughTheReturnedValue()
        {
            IAccountRepository accountRepository = (IAccountRepository)ContextRegistry.GetContext()["AccountRepository"];
            Assert.NotNull(accountRepository);

            string name = "Thorin";
            // Email is used as both Email and username
            string email = "thorin@dummyemail123456.com";
            string password = "Erebor123!";

            // Register the user
            IdentityResult result = accountRepository.RegisterUser(name, email, password);
            Assert.IsTrue(result.Succeeded);
            // Verify it is in the database
            CustomIdentityUser customIdentityuser = accountRepository.GetUserByEmail(email);
            Assert.NotNull(customIdentityuser);
            Assert.AreEqual(name, customIdentityuser.FullName);
            Assert.AreEqual(email, customIdentityuser.Email);
            Assert.AreEqual(email, customIdentityuser.UserName);
            Assert.IsFalse(customIdentityuser.EmailConfirmed);

            // Get the Email activation Token for this user. This is a bit of a hack. We can't get this from the email address
            var emailActivationToken = accountRepository.GetEmailActivationToken(customIdentityuser.Id);
            // Activate The User using the token
            var confirmEmail = accountRepository.ConfirmEmail(customIdentityuser.Id, emailActivationToken);
            Assert.IsTrue(confirmEmail);
            customIdentityuser = accountRepository.GetUserByEmail(email);
            Assert.IsTrue(customIdentityuser.EmailConfirmed);
            Assert.IsTrue(accountRepository.IsEmailConfirmed(customIdentityuser.Id));
            var oldPasswordHash = customIdentityuser.PasswordHash;

            var newPassword = "TheArkenstone123!";
            // Now request a password reset token and verifies that the password gets updated. Use the hack to get the reset token
            var passwordResetToken = accountRepository.GetPasswordResetToken(customIdentityuser.Id);
            // Reset the password
            var resetPasswordResponse = accountRepository.ResetPassword(customIdentityuser.Id, passwordResetToken, newPassword);
            Assert.IsTrue(resetPasswordResponse);
            // Check that the password has been updated
            customIdentityuser = accountRepository.GetUserByEmail(email);
            Assert.AreNotEqual(oldPasswordHash, customIdentityuser.PasswordHash);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EmailActivationFailTest_ChecksThatAccountIsNotActivatedIfTheEmailTokenIsInvalid_VerifiesThroughTheReturnedValue()
        {
            IAccountRepository accountRepository = (IAccountRepository)ContextRegistry.GetContext()["AccountRepository"];
            Assert.NotNull(accountRepository);

            string name = "Thorin";
            // Email is used as both Email and username
            string email = "thorin@dummyemail123456.com";
            string password = "Erebor123!";

            // Register the user
            IdentityResult result = accountRepository.RegisterUser(name, email, password);
            Assert.IsTrue(result.Succeeded);
            // Verify it is in the database
            CustomIdentityUser customIdentityuser = accountRepository.GetUserByEmail(email);

            // Get the Email activation Token for this user. This is a bit of a hack. We can't get this from the email address
            var emailActivationToken = accountRepository.GetEmailActivationToken(customIdentityuser.Id);
            // Activate The User using the invalid token
            // EXCEPTION EXPECTED HERE
            var confirmEmail = accountRepository.ConfirmEmail(customIdentityuser.Id, emailActivationToken + "1");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PasswordResetFailTest_ChecksThatThePasswordIsNotResetWhenTheTokenIsIncorrect_VerifiesThroughTheReturnedValue()
        {
            IAccountRepository accountRepository = (IAccountRepository)ContextRegistry.GetContext()["AccountRepository"];
            Assert.NotNull(accountRepository);

            string name = "Thorin";
            // Email is used as both Email and username
            string email = "thorin@dummyemail123456.com";
            string password = "Erebor123!";

            // Register the user
            IdentityResult result = accountRepository.RegisterUser(name, email, password);
            Assert.IsTrue(result.Succeeded);
            // Verify it is in the database
            CustomIdentityUser customIdentityuser = accountRepository.GetUserByEmail(email);

            // Get the Email activation Token for this user. This is a bit of a hack. We can't get this from the email address
            var emailActivationToken = accountRepository.GetEmailActivationToken(customIdentityuser.Id);
            // Activate The User using the token
            var confirmEmail = accountRepository.ConfirmEmail(customIdentityuser.Id, emailActivationToken);
            Assert.IsTrue(confirmEmail);
            var newPassword = "TheArkenstone123!";
            // Now request a password reset token and verifies that the password gets updated. Use the hack to get the reset token
            var passwordResetToken = accountRepository.GetPasswordResetToken(customIdentityuser.Id);
            // Reset the password. Provide an invalid token so that it fails
            // EXCEPTION IS EXPECTED HERE
            var resetPasswordResponse = accountRepository.ResetPassword(customIdentityuser.Id, passwordResetToken + "1", newPassword);
        }
    }
}
