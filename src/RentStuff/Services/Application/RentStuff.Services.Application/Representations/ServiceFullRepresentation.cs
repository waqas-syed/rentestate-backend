using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using RentStuff.Services.Domain.Model.ServiceAggregate;

namespace RentStuff.Services.Application.Representations
{
    /// <summary>
    /// Represents as complete set of the Service type
    /// </summary>
    [DataContract]
    public class ServiceFullRepresentation
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public ServiceFullRepresentation(string id, string name, string description, string location, string mobileNumber,
            string serviceEmail, string serviceProfessionType, string serviceEntityType, string facebookLink, 
            string instagramLink, string twitterLink, string websiteLink, DateTime? dateEstablished, 
            IReadOnlyList<string> images, IReadOnlyList<Review> reviews)
        {
            Id = id;
            Name = name;
            Description = description;
            Location = location;
            MobileNumber = mobileNumber;
            ServiceEmail = serviceEmail;
            ServiceProfessionType = serviceProfessionType;
            ServiceEntityType = serviceEntityType;
            FacebookLink = facebookLink;
            InstagramLink = instagramLink;
            TwitterLink = twitterLink;
            WebsiteLink = websiteLink;
            DateEstablished = dateEstablished;
            Images = images;
            Reviews = reviews;
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
        public DateTime? DateEstablished { get; private set; }
        [DataMember]
        public IReadOnlyList<string> Images { get; private set; }
        [DataMember]
        public IReadOnlyList<Review> Reviews { get; private set; }
    }
}
