using System;
using System.ComponentModel;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace RentStuff.Identity.Infrastructure.Services.Email
{
    public class CustomEmailService : ICustomEmailService
    {
        // Get these settings from the appsettings from the root app.config/web.config file
        private static readonly string CompanyEmailAddress = ConfigurationManager.AppSettings.Get("CompanyEmailAddress");

        private static readonly string CompanyEmailPassword =
            ConfigurationManager.AppSettings.Get("CompanyEmailPassword");

        private static readonly string CompanyEmailHost = ConfigurationManager.AppSettings.Get("CompanyEmailHost");

        private static readonly int CompanyEmailPort =
            int.Parse(ConfigurationManager.AppSettings.Get("CompanyEmailPort"));

        private SmtpClient _smtpClient = new SmtpClient(CompanyEmailHost, CompanyEmailPort);
        public event Action EmailSent;

        public CustomEmailService()
        {
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            _smtpClient.UseDefaultCredentials = false;
            _smtpClient.Host = CompanyEmailHost;
            _smtpClient.Credentials = new NetworkCredential(CompanyEmailAddress, CompanyEmailPassword);
            _smtpClient.EnableSsl = true;
            _smtpClient.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
        }

        /// <summary>
        /// Send activation email after the user has registered
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public void SendEmail(string to, string subject, string body)
        {
            MailMessage mail = new MailMessage(CompanyEmailAddress, to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            try
            {
                _smtpClient.SendAsync(mail, null);
            }
            catch (Exception)
            {
                // If the email did not got sent for some reason, try to send it again
                _smtpClient.SendAsync(mail, null);
            }
        }

        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            EmailSent?.Invoke();
        }
    }
}
