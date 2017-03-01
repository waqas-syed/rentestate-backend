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
        public async void RegisterUserTest_TestsIfTheUserIsSavedAsExpectedWhenRegisterMethodIsCalled_VerifiesThroughTheRetruendValueUponRetreival()
        {
            IAccountRepository accountRepository = (IAccountRepository)ContextRegistry.GetContext()["AccountRepository"];
            Assert.NotNull(accountRepository);
            
            string name = "Thorin";
            // Email is used as both Email and username
            string email = "thorin@dummyemail123456.com";
            string password = "Erebor123!";

            CustomIdentityUser customIdentityuser = await accountRepository.FindUser(email, password);
            Assert.Null(customIdentityuser);

            Tuple<IdentityResult,string> result = await accountRepository.SaveUser(name, email, password);            
            Assert.IsTrue(result.Item1.Succeeded);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.Item2));

            customIdentityuser = await accountRepository.FindUser(email, password);
            Assert.NotNull(customIdentityuser);
            Assert.AreEqual(name, customIdentityuser.FullName);
            Assert.AreEqual(email, customIdentityuser.Email);
            Assert.IsFalse(accountRepository.UserManager.IsEmailConfirmed(customIdentityuser.Id));
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
            Tuple<IdentityResult, string> result = await accountRepository.SaveUser(name, email, password);
            Assert.IsTrue(result.Item1.Succeeded);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.Item2));

            CustomIdentityUser customIdentityuser = await accountRepository.FindByEmailAsync(email);
            Assert.NotNull(customIdentityuser);
            Assert.AreEqual(name, customIdentityuser.FullName);
            Assert.AreEqual(email, customIdentityuser.Email);
            Assert.AreEqual(email, customIdentityuser.UserName);
            Assert.IsFalse(accountRepository.UserManager.IsEmailConfirmed(customIdentityuser.Id));
            
            // Update the user by changing the name
            string newName = "New Name";
            customIdentityuser.FullName = newName;
            IdentityResult updateResponse = await accountRepository.UpdateUser(customIdentityuser);
            Assert.IsTrue(updateResponse.Succeeded);
            customIdentityuser = await accountRepository.FindByEmailAsync(email);
            Assert.NotNull(customIdentityuser);
            Assert.AreEqual(newName, customIdentityuser.FullName);
            Assert.AreEqual(email, customIdentityuser.Email);
            Assert.AreEqual(email, customIdentityuser.UserName);
            Assert.IsFalse(accountRepository.UserManager.IsEmailConfirmed(customIdentityuser.Id));
        }
    }
}
