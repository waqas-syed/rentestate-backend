using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RentStuff.Property.Application.PropertyServices.Representation.AbstractRepresentations
{
    /// <summary>
    /// Full abstract representation for all Residential Properties
    /// </summary>
    public abstract class ResidentialPropertyFullBaseRepresentation : PropertyBaseRepresentation
    {
        public ResidentialPropertyFullBaseRepresentation(string id, string title, long rentPrice,
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
        
        public string Id { get; private set; }
        
        public string Title { get; private set; }
        
        public string Description { get; private set; }
        
        public long RentPrice
        {
            get; private set;
        }
        
        public bool InternetAvailable
        {
            get; private set;
        }
        
        public bool CableTvAvailable
        {
            get; private set;
        }
        
        public string PropertyType
        {
            get; private set;
        }
        
        public string OwnerEmail
        {
            get; private set;
        }
        
        public string OwnerPhoneNumber
        {
            get; private set;
        }
        
        public string LandlineNumber
        {
            get; private set;
        }
        
        public string Fax
        {
            get; private set;
        }
        
        public string Area
        {
            get; private set;
        }
        
        public IList<string> Images
        {
            get; private set;
        }
        
        public string OwnerName
        {
            get; private set;
        }
        
        public string GenderRestriction
        {
            get; private set;
        }
        
        public bool IsShared
        {
            get; private set;
        }
        
        public string RentUnit
        {
            get; private set;
        }
    }
}
