using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Services.Application.Commands
{
    /// <summary>
    /// Data Object for updating a Service
    /// </summary>
    public class UpdateServiceCommand
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public UpdateServiceCommand(string name, string description, string location, string phoneNumber, string serviceEmail, string uploaderEmail, string serviceProfesionType, string serviceEntityType, DateTime dateEstablished, string facebookLink, string instagramLink, string twitterLink, string websiteLink)
        {
            Name = name;
            Description = description;
            Location = location;
            PhoneNumber = phoneNumber;
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
        public string PhoneNumber { get; private set; }
        public string ServiceEmail { get; private set; }
        public string UploaderEmail { get; private set; }
        public string ServiceProfesionType { get; private set; }
        public string ServiceEntityType { get; private set; }
        public DateTime DateEstablished { get; private set; }
        public string FacebookLink { get; private set; }
        public string InstagramLink { get; private set; }
        public string TwitterLink { get; private set; }
        public string WebsiteLink { get; private set; }
    }
}
