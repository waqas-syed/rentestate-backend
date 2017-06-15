using System;

namespace RentStuff.Services.Application.ApplicationServices.Commands
{
    /// <summary>
    /// Create Service Command
    /// </summary>
    public class CreateServiceCommand
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public CreateServiceCommand(string name, string description, string location, string mobileNumber, 
            string serviceEmail, string uploaderEmail, string serviceProfesionType, string serviceEntityType, 
            DateTime? dateEstablished, string facebookLink, string instagramLink, string twitterLink, string websiteLink)
        {
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

        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Location { get; private set; }
        public string MobileNumber { get; private set; }
        public string ServiceEmail { get; private set; }
        public string UploaderEmail { get; private set; }
        public string ServiceProfesionType { get; private set; }
        public string ServiceEntityType { get; private set; }
        public DateTime? DateEstablished { get; private set; }
        public string FacebookLink { get; private set; }
        public string InstagramLink { get; private set; }
        public string TwitterLink { get; private set; }
        public string WebsiteLink { get; private set; }
    }
}
