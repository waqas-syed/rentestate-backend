using System;
using System.Runtime.Serialization;

namespace RentStuff.Identity.Application.Account.Representations
{
    /// <summary>
    /// Represents the account details of a user who logged in through an external provider.
    /// </summary>
    [Serializable]
    [DataContract]
    public class ExternalLoginDataRepresentation
    {
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <param name="fullName"></param>
        /// <param name="email"></param>
        /// <param name="externalAccessToken"></param>
        public ExternalLoginDataRepresentation(string loginProvider, string providerKey, string fullName, string email, 
            string externalAccessToken)
        {
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
            FullName = fullName;
            Email = email;
            ExternalAccessToken = externalAccessToken;
        }

        [DataMember]
        public string LoginProvider { get; private set; }
        [DataMember]
        public string ProviderKey { get; private set; }
        [DataMember]
        public string FullName { get; private set; }
        [DataMember]
        public string Email { get; private set; }
        [DataMember]
        public string ExternalAccessToken { get; private set; }
    }
}