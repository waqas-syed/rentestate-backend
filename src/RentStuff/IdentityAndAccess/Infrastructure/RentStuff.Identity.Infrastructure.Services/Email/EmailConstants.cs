﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Identity.Infrastructure.Services.Email
{
    public class EmailConstants
    {
        public static string ActivationEmailMessage(string name, string activationLink)
        {
            return $"Dear {name},<br/><br/>" +
                   $"Thank you for registering at RentStuff. To complete the last step of the registration process, just click <a href=\"{activationLink}\">here</a> to activate your account. <br/><br/>" +
                   $"Or copy & paste this url in your browser: <br/> {activationLink} <br/><br/>" +
                   $"Have a great day, <br/><br/> You friends at RentStuff";
        }
    }
}
