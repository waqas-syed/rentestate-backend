using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Identity.Application.Account.Representations
{
    [Serializable]
    [DataContract]
    public class InternalLoginDataRepresentation
    {
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="email"></param>
        /// <param name="internalAccessToken"></param>
        public InternalLoginDataRepresentation(string fullName, string email, string internalAccessToken)
        {
            FullName = fullName;
            Email = email;
            InternalAccessToken = internalAccessToken;
        }
        
        [DataMember]
        public string FullName { get; private set; }
        [DataMember]
        public string Email { get; private set; }
        [DataMember]
        public string InternalAccessToken { get; private set; }
    }
}
