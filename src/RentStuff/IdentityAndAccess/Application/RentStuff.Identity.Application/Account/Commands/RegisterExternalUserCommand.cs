using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RentStuff.Identity.Application.Account.Commands
{
    /// <summary>
    /// For users registering though Facebook or other ocial media apps
    /// </summary>
    [Serializable]
    [DataContract]
    public class RegisterExternalUserCommand
    {
        [Required]
        [DataMember]
        public string FullName { get; set; }

        [Required]
        [DataMember]
        public string Email { get; set; }

        [Required]
        [DataMember]
        public string Provider { get; set; }

        [Required]
        [DataMember]
        public string ExternalAccessToken { get; set; }
    }
}
