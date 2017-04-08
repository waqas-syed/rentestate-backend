using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using NUnit.Framework;
using RentStuff.Identity.Infrastructure.Services.Identity;
using Spring.Context.Support;

namespace RentStuff.Identity.Infrastructure.Services.IntegTests
{
    [TestFixture]
    class PasswordResetServiceIntegTests
    {
        [Test]
        public void GenerateAndValidatePasswordResetTokenSuccessTest_CheckThatTokenIsCreateAndValidatedAsExpected_VerifiesThroughTheReturnValue()
        {
            IUserTokenProvider<CustomIdentityUser, string> passwordResetTokenService = (IUserTokenProvider<CustomIdentityUser, string>) ContextRegistry.GetContext()["UserTokenProviderService"];
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
            IUserTokenProvider<CustomIdentityUser, string> passwordResetTokenService = (IUserTokenProvider<CustomIdentityUser, string>)ContextRegistry.GetContext()["UserTokenProviderService"];
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
            IUserTokenProvider<CustomIdentityUser, string> passwordResetTokenService = (IUserTokenProvider<CustomIdentityUser, string>)ContextRegistry.GetContext()["UserTokenProviderService"];
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
