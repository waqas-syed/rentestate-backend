using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Ninject;
using NUnit.Framework;
using RentStuff.Identity.Infrastructure.Services.Identity;
using RentStuff.Identity.Infrastructure.Services.Ninject.Modules;

namespace RentStuff.Identity.Infrastructure.Services.IntegTests
{
    [TestFixture]
    class PasswordResetServiceIntegTests
    {
        /// <summary>
        /// Loads the instances that will be used in the production environment. No Mocks or fakes
        /// </summary>
        private IKernel InitializeNinjectMockDependencies()
        {
            var kernel = new StandardKernel();
            kernel.Load<MockIdentityAccessServicesNinjectModule>();
            return kernel;
        }

        [Test]
        public void GenerateAndValidatePasswordResetTokenSuccessTest_CheckThatTokenIsCreateAndValidatedAsExpected_VerifiesThroughTheReturnValue()
        {
            IKernel kernel = InitializeNinjectMockDependencies();
            IUserTokenProvider<CustomIdentityUser, string> passwordResetTokenService =
                kernel.Get<IUserTokenProvider<CustomIdentityUser, string>>();
            Assert.NotNull(passwordResetTokenService);

            string email = "gandalfthewhite@1234567.com";
            CustomIdentityUser customIdentityUser = new CustomIdentityUser {Email = email};
            Task<string> generateTokenTask = passwordResetTokenService.GenerateAsync("purpose", null, customIdentityUser);
            generateTokenTask.Wait();
            Assert.IsFalse(string.IsNullOrWhiteSpace(generateTokenTask.Result));
            Task<bool> validateTokenTask = passwordResetTokenService.ValidateAsync("purpose", generateTokenTask.Result,
                null, customIdentityUser);
            validateTokenTask.Wait();
            Assert.IsTrue(validateTokenTask.Result);
        }
        
        [Test]
        [ExpectedException(typeof(AggregateException))]
        public void ValidateTokenFailTest_CheckThatTokenIsNotValidatedSuccessfullyIfTheEmailHashIsWrong_VerifiesThroughTheRaisedEexception()
        {
            IKernel kernel = InitializeNinjectMockDependencies();
            IUserTokenProvider<CustomIdentityUser, string> passwordResetTokenService =
                kernel.Get<IUserTokenProvider<CustomIdentityUser, string>>();
            Assert.NotNull(passwordResetTokenService);

            string email = "gandalfthewhite@1234567.com";
            CustomIdentityUser customIdentityUser = new CustomIdentityUser { Email = email };
            Task<string> generateTokenTask = passwordResetTokenService.GenerateAsync("purpose", null, customIdentityUser);
            generateTokenTask.Wait();
            Assert.IsFalse(string.IsNullOrWhiteSpace(generateTokenTask.Result));

            string finalGeneratedToken = null;
            var separatedHashes = generateTokenTask.Result.Split('|');
            finalGeneratedToken = separatedHashes[0] + "2|" + separatedHashes[1];
            
            // Exception should be raised
            var validateTokenTask = passwordResetTokenService.ValidateAsync("purpose", finalGeneratedToken, null, customIdentityUser);
            validateTokenTask.Wait();
        }

        [Test]
        [ExpectedException(typeof(AggregateException))]
        public void ValidateTokenFailTest_CheckThatTokenIsNotValidatedSuccessfullyIfTheUserIdHashIsWrong_VerifiesThroughTheRaisedEexception()
        {
            IKernel kernel = InitializeNinjectMockDependencies();
            IUserTokenProvider<CustomIdentityUser, string> passwordResetTokenService =
                kernel.Get<IUserTokenProvider<CustomIdentityUser, string>>();
            Assert.NotNull(passwordResetTokenService);

            string email = "gandalfthewhite@1234567.com";
            CustomIdentityUser customIdentityUser = new CustomIdentityUser { Email = email };
            Task<string> generateTokenTask = passwordResetTokenService.GenerateAsync("purpose", null, customIdentityUser);
            generateTokenTask.Wait();
            Assert.IsFalse(string.IsNullOrWhiteSpace(generateTokenTask.Result));

            string finalGeneratedToken = null;
            var separatedHashes = generateTokenTask.Result.Split('|');
            finalGeneratedToken = separatedHashes[0] + "|2" + separatedHashes[1];
            // Exception should be raised
            var validateTokenTask = passwordResetTokenService.ValidateAsync("purpose", finalGeneratedToken, null, customIdentityUser);
            validateTokenTask.Wait();
        }
    }
}
