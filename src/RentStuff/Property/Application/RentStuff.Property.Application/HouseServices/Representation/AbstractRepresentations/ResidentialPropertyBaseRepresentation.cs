using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RentStuff.Property.Application.HouseServices.Representation.AbstractRepresentations
{
    [DataContract]
    public abstract class ResidentialPropertyBaseRepresentation : PropertyBaseRepresentation
    {
        public ResidentialPropertyBaseRepresentation(string id, string title, long rentPrice,
            bool internetAvailable, bool cableTvAvailable,
            string propertyType, string ownerEmail, string ownerPhoneNumber, string area, string ownerName, 
            string description,
            string genderRestriction, bool isShared, string rentUnit, string landlineNumber, string fax, 
            IList<string> images)
        {
            Id = id;
            Title = title;
            RentPrice = rentPrice;
            InternetAvailable = internetAvailable;
            CableTvAvailable = cableTvAvailable;
            PropertyType = propertyType;
            OwnerEmail = ownerEmail;
            OwnerPhoneNumber = ownerPhoneNumber;
            Area = area;
            OwnerName = ownerName;
            Description = description;
            GenderRestriction = genderRestriction;
            IsShared = isShared;
            RentUnit = rentUnit;
            LandlineNumber = landlineNumber;
            Fax = fax;
            Images = images;
        }

        [DataMember]
        public string Id { get; private set; }

        [DataMember]
        public string Title { get; private set; }

        [DataMember]
        public string Description { get; private set; }

        [DataMember]
        public long RentPrice
        {
            get; private set;
        }
        
        [DataMember]
        public bool InternetAvailable
        {
            get; private set;
        }
        
        [DataMember]
        public bool CableTvAvailable
        {
            get; private set;
        }

        [DataMember]
        public string PropertyType
        {
            get; private set;
        }

        [DataMember]
        public string OwnerEmail
        {
            get; private set;
        }

        [DataMember]
        public string OwnerPhoneNumber
        {
            get; private set;
        }

        [DataMember]
        public string LandlineNumber
        {
            get; private set;
        }

        [DataMember]
        public string Fax
        {
            get; private set;
        }
        
        [DataMember]
        public string Area
        {
            get; private set;
        }

        [DataMember]
        public IList<string> Images
        {
            get; private set;
        }

        [DataMember]
        public string OwnerName
        {
            get; private set;
        }

        [DataMember]
        public string GenderRestriction
        {
            get; private set;
        }

        [DataMember]
        public bool IsShared
        {
            get; private set;
        }

        [DataMember]
        public string RentUnit
        {
            get; private set;
        }
    }
}
