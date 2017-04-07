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
    }
}
