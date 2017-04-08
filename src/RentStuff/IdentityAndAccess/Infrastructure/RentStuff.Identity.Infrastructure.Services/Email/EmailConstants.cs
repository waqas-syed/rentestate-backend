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
        public static string PasswordResetSubject = $"Please reset your {Constants.CompanyName} password";

        public static string ActivationEmailMessage(string name, string activationLink)
        {
            return $"Dear {name},<br/><br/>" +
                   $"Thank you for registering at {Constants.CompanyName}. To complete the last step of the registration process, just click <a href=\"{activationLink}\">here</a> to activate your account. <br/><br/>" +
                   $"Or copy & paste this url in your browser: <br/> {activationLink} <br/><br/><br/>" +
                   $"Have a great day, <br/> Your friends at {Constants.CompanyName}";
        }

        public static string PasswordResetEmail(string name, string passwordResetLink)
        {
            return $"Dear {name},<br/><br/>" +
                "We heard from you that you have forgotten your password. " +
                "It's alright, you can reset it right away by clicking on the following link: <br/><br/>" +
                $"{passwordResetLink} <br/><br/>" + 
                $"This link will expire in 24 hours <br/> <br/>" +
                $"Have a nice day, <br/> Your friends at {Constants.CompanyName}";
        }
    }
}
