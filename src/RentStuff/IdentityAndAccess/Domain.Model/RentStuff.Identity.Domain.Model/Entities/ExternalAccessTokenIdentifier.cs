using System;
using System.ComponentModel.DataAnnotations;

namespace RentStuff.Identity.Domain.Model.Entities
{
    /// <summary>
    /// Maps the ExternalAccessToken for third-parties(Facebook etc) to an identifier that we generate internally. This way, we never
    /// expose the ExternalAccessToken to UI clients or anyone outside our server
    /// </summary>
    public class ExternalAccessTokenIdentifier
    {
        private string _internalId = Guid.NewGuid().ToString();

        public ExternalAccessTokenIdentifier()
        {
            
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="externalAccessToken"></param>
        public ExternalAccessTokenIdentifier(string externalAccessToken)
        {
            ExternalAccessToken = externalAccessToken;
        }

        /// <summary>
        /// Id that we create internally, a GUID. We pass this ID to UI clients instead of the actual ExternalAccessToken
        /// </summary>
        [Key]
        [MaxLength(255)]
        public string InternalId {
            get { return _internalId; }
            private set { _internalId = value; }
        }

        /// <summary>
        /// The token that we get from the external source(Facebook etc)
        /// </summary>
        [MaxLength(255)]
        public string ExternalAccessToken { get; private set; }
    }
}
