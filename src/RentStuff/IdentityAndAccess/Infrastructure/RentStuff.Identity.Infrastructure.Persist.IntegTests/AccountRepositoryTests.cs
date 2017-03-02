using System;
using System.Configuration;
using System.Runtime.Remoting;
using Microsoft.AspNet.Identity;
using NUnit.Framework;
using RentStuff.Common;
using RentStuff.Identity.Infrastructure.Persistence.Model;
using RentStuff.Identity.Infrastructure.Persistence.Repositories;
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

            Tuple<IdentityResult,string> result = accountRepository.RegisterUser(name, email, password);            
            Assert.IsTrue(result.Item1.Succeeded);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.Item2));
            
            customIdentityuser = accountRepository.GetUserByPassword(email, password);
            Assert.NotNull(customIdentityuser);
            Assert.AreEqual(name, customIdentityuser.FullName);
            Assert.AreEqual(email, customIdentityuser.Email);
            Assert.IsFalse(accountRepository.IsEmailConfirmed(customIdentityuser.Id));
        }

        [Test]
        public void ActivateUserTest_TestsIfTheUserIsActivatedAsExpected_VerifiesThroughTheReturnedValueUponRetreival()
        {
            IAccountRepository accountRepository = (IAccountRepository)ContextRegistry.GetContext()["AccountRepository"];
            Assert.NotNull(accountRepository);

            string name = "Thorin";
            // Email is used as both Email and username
            string email = "thorin@dummyemail123456.com";
            string password = "Erebor123!";

            // No user should be retreived using this email
            CustomIdentityUser customIdentityuser = accountRepository.GetUserByEmail(email);
            Assert.Null(customIdentityuser);

            // Register the user
            Tuple<IdentityResult, string> registerResult = accountRepository.RegisterUser(name, email, password);
            Assert.IsTrue(registerResult.Item1.Succeeded);
            Assert.IsFalse(string.IsNullOrWhiteSpace(registerResult.Item2));

            // Retreive the registered user and verify that the email has not yet been confirmed
            customIdentityuser = accountRepository.GetUserByEmail(email);
            Assert.NotNull(customIdentityuser);
            Assert.IsFalse(accountRepository.IsEmailConfirmed(customIdentityuser.Id));

            // Activate the user's account by confirming the email address
            var isAccountActivated = accountRepository.ActivateUser(customIdentityuser.Id, registerResult.Item2);
            Assert.IsTrue(isAccountActivated);
            // Retreive the user and verify that the email has now been confirmed
            customIdentityuser = accountRepository.GetUserByEmail(email);
            Assert.NotNull(customIdentityuser);
            Assert.AreEqual(name, customIdentityuser.FullName);
            Assert.AreEqual(email, customIdentityuser.Email);
            Assert.IsTrue(customIdentityuser.EmailConfirmed);
            Assert.IsTrue(accountRepository.IsEmailConfirmed(customIdentityuser.Id));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ActivateUserFailTest_TestsIfExceptionIsRaisedWhenWrongTokenIsProvided_VerifiesThroughExceptionRaised()
        {
            IAccountRepository accountRepository = (IAccountRepository)ContextRegistry.GetContext()["AccountRepository"];
            Assert.NotNull(accountRepository);

            string name = "Thorin";
            // Email is used as both Email and username
            string email = "thorin@dummyemail123456.com";
            string password = "Erebor123!";

            // No user should be retreived using this email
            CustomIdentityUser customIdentityuser = accountRepository.GetUserByEmail(email);
            Assert.Null(customIdentityuser);

            // Register the user
            Tuple<IdentityResult, string> registerResult = accountRepository.RegisterUser(name, email, password);
            Assert.IsTrue(registerResult.Item1.Succeeded);
            Assert.IsFalse(string.IsNullOrWhiteSpace(registerResult.Item2));

            // Retreive the registered user and verify that the email has not yet been confirmed
            customIdentityuser = accountRepository.GetUserByEmail(email);
            Assert.NotNull(customIdentityuser);
            Assert.IsFalse(accountRepository.IsEmailConfirmed(customIdentityuser.Id));

            // Provide wrong EmailConfirmationToken
            var isAccountActivated = accountRepository.ActivateUser(customIdentityuser.Id, registerResult.Item2 + "l");
        }

        [Test]
        public async void UpdateUserTest_TestsWhetherTheUserGetsUpdatedAsExpected_VerifiesThroughDatabaseRetreival()
        {
            IAccountRepository accountRepository = (IAccountRepository)ContextRegistry.GetContext()["AccountRepository"];
            Assert.NotNull(accountRepository);

            string name = "Thorin";
            // Email is used as both Email and username
            string email = "thorin@dummyemail123456.com";
            string password = "Erebor123!";
            
            // Register the user
            Tuple<IdentityResult, string> result = accountRepository.RegisterUser(name, email, password);
            Assert.IsTrue(result.Item1.Succeeded);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.Item2));

            CustomIdentityUser customIdentityuser = accountRepository.GetUserByEmail(email);
            Assert.NotNull(customIdentityuser);
            Assert.AreEqual(name, customIdentityuser.FullName);
            Assert.AreEqual(email, customIdentityuser.Email);
            Assert.AreEqual(email, customIdentityuser.UserName);
            Assert.IsFalse(customIdentityuser.EmailConfirmed);
            Assert.IsFalse(accountRepository.IsEmailConfirmed(customIdentityuser.Id));
            
            // Update the user by changing the name
            string newName = "New Name";
            customIdentityuser.FullName = newName;
            IdentityResult updateResponse = accountRepository.UpdateUser(customIdentityuser);
            Assert.IsTrue(updateResponse.Succeeded);
            customIdentityuser = accountRepository.GetUserByEmail(email);
            Assert.NotNull(customIdentityuser);
            Assert.AreEqual(newName, customIdentityuser.FullName);
            Assert.AreEqual(email, customIdentityuser.Email);
            Assert.AreEqual(email, customIdentityuser.UserName);
            Assert.IsFalse(customIdentityuser.EmailConfirmed);
        }
    }
}
