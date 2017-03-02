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
    public class ActivateAccountCommand
    {
        public ActivateAccountCommand(string email, string activationCode)
        {
            Email = email;
            ActivationCode = activationCode;
        }
        
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string ActivationCode { get; set; }
    }
}
