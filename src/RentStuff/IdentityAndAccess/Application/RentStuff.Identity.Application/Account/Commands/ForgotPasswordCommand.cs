using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Identity.Application.Account.Commands
{
    [Serializable]
    [DataContract]
    public class ForgotPasswordCommand
    {
        public ForgotPasswordCommand(string email)
        {
            Email = email;
        }

        public string Email { get; private set; }
    }
}
