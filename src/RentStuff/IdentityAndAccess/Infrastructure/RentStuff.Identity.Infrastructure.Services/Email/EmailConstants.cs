using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentStuff.Common;

namespace RentStuff.Identity.Infrastructure.Services.Email
{
    public class EmailConstants
    {
        public static string ActivationEmailSubject = $"Activate your {Constants.CompanyName} account";

        public static string ActivationEmailMessage(string name, string activationLink)
        {
            return $"Dear {name},<br/><br/>" +
                   $"Thank you for registering at {Constants.CompanyName}. To complete the last step of the registration process, just click <a href=\"{activationLink}\">here</a> to activate your account. <br/><br/>" +
                   $"Or copy & paste this url in your browser: <br/> {activationLink} <br/><br/><br/>" +
                   $"Have a great day, <br/> You friends at {Constants.CompanyName}";
        }
    }
}
