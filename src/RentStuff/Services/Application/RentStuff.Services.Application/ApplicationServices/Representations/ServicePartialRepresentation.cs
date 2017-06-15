using System.Runtime.Serialization;

namespace RentStuff.Services.Application.ApplicationServices.Representations
{
    /// <summary>
    /// Partial Representation showing minimal information that is a must to show
    /// </summary>
    [DataContract]
    public class ServicePartialRepresentation
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public ServicePartialRepresentation(string name, string location, string mobileNumber, 
            string serviceEmail, string serviceProfessionType, string serviceEntityType, string facebookLink,
            string instagramLink, string twitterLink, string websiteLink, string defaultImageLink)
        {
            Name = name;
            Location = location;
            MobileNumber = mobileNumber;
            ServiceEmail = serviceEmail;
            ServiceProfessionType = serviceProfessionType;
            ServiceEntityType = serviceEntityType;
            FacebookLink = facebookLink;
            InstagramLink = instagramLink;
            TwitterLink = twitterLink;
            WebsiteLink = websiteLink;
            DefaultImageLink = defaultImageLink;
        }

        [DataMember]
        public string Name { get; private set; }
        [DataMember]
        public string Location { get; private set; }
        [DataMember]
        public string MobileNumber { get; private set; }
        [DataMember]
        public string ServiceEmail { get; private set; }
        [DataMember]
        public string ServiceProfessionType { get; private set; }
        [DataMember]
        public string ServiceEntityType { get; private set; }
        [DataMember]
        public string FacebookLink { get; private set; }
        [DataMember]
        public string InstagramLink { get; private set; }
        [DataMember]
        public string TwitterLink { get; private set; }
        [DataMember]
        public string WebsiteLink { get; private set; }
        [DataMember]
        public string DefaultImageLink { get; private set; }
    }
}
