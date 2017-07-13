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
    public class ParsedExternalAccessTokenRepresentation
    {
        public ParsedExternalAccessTokenRepresentation(string userId, string appId)
        {
            UserId = userId;
            AppId = appId;
        }
        
        [DataMember]
        public string UserId { get; private set; }

        [DataMember]
        public string AppId { get; private set; }
    }
}
