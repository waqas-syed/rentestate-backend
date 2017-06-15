using System.Runtime.Serialization;

namespace RentStuff.Services.Application.ApplicationServices.Representations
{
    /// <summary>
    /// Representation for the Review type
    /// </summary>
    [DataContract]
    public class ReviewRepresentation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public ReviewRepresentation(string authorname, string authorEmail, string reviewDescription, 
            string service)
        {
            Authorname = authorname;
            AuthorEmail = authorEmail;
            ReviewDescription = reviewDescription;
            ServiceId = service;
        }
        
        [DataMember]
        public virtual string Authorname { get; private set; }
        [DataMember]
        public virtual string AuthorEmail { get; private set; }
        [DataMember]
        public virtual string ReviewDescription { get; private set; }
        [DataMember]
        public virtual string ServiceId { get; private set; }
    }
}
