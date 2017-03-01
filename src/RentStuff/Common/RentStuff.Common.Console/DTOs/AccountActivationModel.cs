using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Common.Console.DTOs
{
    public class AccountActivationModel
    {
        public AccountActivationModel(string email, string activationCode)
        {
            Email = email;
            ActivationCode = activationCode;
        }

        public string Email { get; set; }
        public string ActivationCode { get; set; }
    }
}
