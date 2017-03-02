using System;
using NUnit.Framework;
using RentStuff.Identity.Infrastructure.Services.Email;
using Spring.Context.Support;

namespace RentStuff.Identity.Infrastructure.Services.IntegTests
{
    [TestFixture]
    class EmailTokenGenerationServiceTests
    {
        [Test]
        public void EmailTokenGenerationTest_TestsThatTheTokenIsGeneratedAndVerifiedAsExpected_VerifiesThroughTheReturnValue()
        {
            IEmailTokenGenerationService emailTokenService = (IEmailTokenGenerationService)ContextRegistry.GetContext()["EmailTokenGenerationService"];
            string email = "legolas@woodland123456.com";
            string userId = Guid.NewGuid().ToString();
            var activationToken = emailTokenService.GenerateEmailToken(email, userId);
            var verifyToken = emailTokenService.VerifyToken(email, userId, activationToken);
            Assert.IsTrue(verifyToken);

            string email2 = "aragorn@rangers123456.com";
            string userId2 = Guid.NewGuid().ToString();
            var activationToken2 = emailTokenService.GenerateEmailToken(email2, userId2);
            var verifyToken2 = emailTokenService.VerifyToken(email2, userId2, activationToken2);
            Assert.IsTrue(verifyToken2);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidEmailFailTest_TestsThatProvidingWrongEmailRaisesException_VerifiesThroughTheRaisedException()
        {
            IEmailTokenGenerationService emailTokenService = (IEmailTokenGenerationService)ContextRegistry.GetContext()["EmailTokenGenerationService"];
            string email = "legolas@woodland123456.com";
            string userId = Guid.NewGuid().ToString();
            string email2 = "legolas@woodland223456.com";
            var activationToken = emailTokenService.GenerateEmailToken(email, userId);
            var verifyToken = emailTokenService.VerifyToken(email2, userId, activationToken);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidUserIdFailTest_TestsThatProvidingWrongEmailRaisesException_VerifiesThroughTheRaisedException()
        {
            IEmailTokenGenerationService emailTokenService = (IEmailTokenGenerationService)ContextRegistry.GetContext()["EmailTokenGenerationService"];
            string email = "legolas@woodland123456.com";
            string userId = Guid.NewGuid().ToString();
            string email2 = "legolas@woodland223456.com";
            var activationToken = emailTokenService.GenerateEmailToken(email, userId);
            var verifyToken = emailTokenService.VerifyToken(email2, userId = "1", activationToken);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EmailInvalidTokenFailTest1_TestsThatProvidingWrongTokenRaisesException_VerifiesThroughTheRaisedException()
        {
            IEmailTokenGenerationService emailTokenService = (IEmailTokenGenerationService)ContextRegistry.GetContext()["EmailTokenGenerationService"];
            string email = "legolas@woodland123456.com";
            string userId = Guid.NewGuid().ToString();
            var activationToken = emailTokenService.GenerateEmailToken(email, userId);
            var verifyToken = emailTokenService.VerifyToken(email, userId, activationToken + "1");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EmailInvalidTokenFailTest2_TestsThatProvidingWrongTokenRaisesException_VerifiesThroughTheRaisedException()
        {
            IEmailTokenGenerationService emailTokenService = (IEmailTokenGenerationService)ContextRegistry.GetContext()["EmailTokenGenerationService"];
            string email = "legolas@woodland123456.com";
            string userId = Guid.NewGuid().ToString();
            var activationToken = emailTokenService.GenerateEmailToken(email, userId);
            activationToken = activationToken.Replace("|", "");
            var verifyToken = emailTokenService.VerifyToken(email, userId, activationToken + "1");
        }
    }
}
