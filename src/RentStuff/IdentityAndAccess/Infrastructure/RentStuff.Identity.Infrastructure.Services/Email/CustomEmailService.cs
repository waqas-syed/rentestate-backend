using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace RentStuff.Identity.Infrastructure.Services.Email
{
    public class CustomEmailService : ICustomEmailService
    {
        // Get these settings from the appsettings from the root app.config/web.config file
        private static readonly string CompanyEmailAddress = ConfigurationManager.AppSettings.Get("CompanyEmailAddress");
        private static readonly string CompanyEmailHost = ConfigurationManager.AppSettings.Get("CompanyEmailHost");
        
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
            SmtpClient client = new SmtpClient();
            client.Send(mail);
        }

        /*private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            EmailSent?.Invoke();
        }*/

        /*private static readonly string CompanyEmailPassword = ConfigurationManager.AppSettings.Get("CompanyEmailPassword");
        
        private static readonly int CompanyEmailPort = int.Parse(ConfigurationManager.AppSettings.Get("CompanyEmailPort"));

        private SmtpClient _smtpClient = null;*/
        /*private void InitializeSmtpClient()
        {
            _smtpClient = new SmtpClient(CompanyEmailHost, CompanyEmailPort);
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            _smtpClient.UseDefaultCredentials = false;
            _smtpClient.Host = CompanyEmailHost;
            _smtpClient.Credentials = new NetworkCredential(CompanyEmailAddress, CompanyEmailPassword);
            _smtpClient.EnableSsl = false;
            _smtpClient.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
        }*/
    }
}
