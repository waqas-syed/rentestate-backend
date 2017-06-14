using System;
using System.Runtime.Serialization;

namespace RentStuff.Services.Application.Commands
{
    /// <summary>
    /// Data Object for updating a Service
    /// </summary>
    [DataContract]
    public class UpdateServiceCommand
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public UpdateServiceCommand(string id, string name, string description, string location, 
            string mobileNumber, string serviceEmail, string uploaderEmail, string serviceProfesionType, 
            string serviceEntityType, DateTime? dateEstablished, string facebookLink, string instagramLink, 
            string twitterLink, string websiteLink)
        {
            Id = id;
            Name = name;
            Description = description;
            Location = location;
            MobileNumber = mobileNumber;
            ServiceEmail = serviceEmail;
            UploaderEmail = uploaderEmail;
            ServiceProfesionType = serviceProfesionType;
            ServiceEntityType = serviceEntityType;
            DateEstablished = dateEstablished;
            FacebookLink = facebookLink;
            InstagramLink = instagramLink;
            TwitterLink = twitterLink;
            WebsiteLink = websiteLink;
        }

        [DataMember]
        public string Id { get; private set; }
        [DataMember]
        public string Name { get; private set; }
        [DataMember]
        public string Description { get; private set; }
        [DataMember]
        public string Location { get; private set; }
        [DataMember]
        public string MobileNumber { get; private set; }
        [DataMember]
        public string ServiceEmail { get; private set; }
        [DataMember]
        public string UploaderEmail { get; private set; }
        [DataMember]
        public string ServiceProfesionType { get; private set; }
        [DataMember]
        public string ServiceEntityType { get; private set; }
        [DataMember]
        public DateTime? DateEstablished { get; private set; }
        [DataMember]
        public string FacebookLink { get; private set; }
        [DataMember]
        public string InstagramLink { get; private set; }
        [DataMember]
        public string TwitterLink { get; private set; }
        [DataMember]
        public string WebsiteLink { get; private set; }
    }
}
