using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Identity.Application.Account.Commands
{
    /// <summary>
    /// DTO to reset the password when the user clicks on he link from their email
    /// </summary>
    [Serializable]
    [DataContract]
    public class ResetPasswordCommand
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public ResetPasswordCommand(string email, string password, string confirmPassword, string token)
        {
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
            Token = token;
        }

        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string ConfirmPassword { get; set; }
        [DataMember]
        public string Token { get; set; }
    }
}
