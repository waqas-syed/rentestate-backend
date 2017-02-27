using System;
using System.Net.Mail;
using System.Threading;
using NUnit.Framework;
using RentStuff.Identity.Infrastructure.Services.Email;
using Spring.Context.Support;

namespace RentStuff.Identity.Infrastructure.Services.IntegTests
{
    [TestFixture]
    public class EmailServiceTests
    {
        [Test]
        public void SendActivationEmailTest_ChecksTheprocessOfSendingAnEmail()
        {
            IEmailService emailService = (IEmailService) ContextRegistry.GetContext()["EmailService"];

            bool eventRaised = false;
            var wait = new AutoResetEvent(false);
            emailService.EmailSent += () =>
            {
                eventRaised = true;
                wait.Set();
            };
            string email = "waqas.shah.syed@gmail.com";
            string name = "Syed Waqas Mahmood";
            string activationLink = "https://localhost:11803/index.html#/login";
            emailService.SendEmail(email, "Test Flight", EmailConstants.ActivationEmailMessage(name, activationLink));
            Assert.IsTrue(wait.WaitOne(TimeSpan.FromSeconds(60)));
            Assert.IsTrue(eventRaised);
        }
    }
}
