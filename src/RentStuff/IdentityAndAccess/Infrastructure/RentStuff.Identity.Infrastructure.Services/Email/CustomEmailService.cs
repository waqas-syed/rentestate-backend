using System;
using System.Net;
using System.Net.Mail;
using NLog;
using RentStuff.Common.Utilities;

namespace RentStuff.Identity.Infrastructure.Services.Email
{
    public class CustomEmailService : ICustomEmailService
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private static string EmailCredentialSettingsName = "zarqoon-no-reply-email";
        // Get these settings from the appsettings from the root app.config/web.config file
        private static readonly string CompanyEmailAddress = "no-reply@zarqoon.com";
        private static readonly string CompanyEmailHost = "smtp.zoho.com";
        private static readonly int CompanyEmailPort = 587;

        public event Action EmailSent;

        public CustomEmailService()
        {
        }

        /// <summary>
        /// Send activation email after the user has registered
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public void SendEmail(string to, string subject, string body)
        {
            //InitializeSmtpClient();
            MailMessage mail = new MailMessage(CompanyEmailAddress, to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            mail.From = new MailAddress(CompanyEmailAddress);

            mail.To.Add(new MailAddress(to));
            // Picks up configuration from web.config/app.config
            SmtpClient client = new SmtpClient(CompanyEmailHost, CompanyEmailPort)
            {
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(CompanyEmailAddress, StringCipher.EmailDecipheredPassword)
            };

            client.Send(mail);
            _logger.Info("Email Sent. Email: {0}, Subject: {1}", to, subject);
        }
    }
}
