using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Identity.Application.Account.Representations
{
    /// <summary>
    /// The public facing representation of the user. Not everything is exposed
    /// </summary>
    [Serializable]
    [DataContract]
    public class UserRepresentation
    {
        public UserRepresentation(string fullName, string email, bool isEmailConfirmed)
        {
            FullName = fullName;
            Email = email;
            IsEmailConfirmed = isEmailConfirmed;
        }
        
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public bool IsEmailConfirmed { get; set; }
    }
}
