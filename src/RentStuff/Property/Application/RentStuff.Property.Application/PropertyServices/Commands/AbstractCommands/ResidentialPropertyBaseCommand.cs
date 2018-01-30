using System;

namespace RentStuff.Property.Application.PropertyServices.Commands.AbstractCommands
{
    [Serializable]
    public class ResidentialPropertyBaseCommand : PropertyBaseCommand
    {
        public ResidentialPropertyBaseCommand(string title, long rentPrice,
            bool internetAvailable, bool cableTvAvailable,
            string propertyType, string ownerEmail,
            string ownerPhoneNumber, string area, string ownerName, string description, string genderRestriction,
            bool isShared, string rentUnit, string landlineNumber, string fax)
        {
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
        }

        public string Id { get; set; }
        
        public string Title { get; private set; }
        
        public string Description { get; private set; }
        
        public long RentPrice { get; private set; }
        
        public bool InternetAvailable { get; private set; }
        
        public bool CableTvAvailable { get; private set; }
        
        public string PropertyType { get; private set; }
        
        public string OwnerEmail { get; private set; }
        
        public string OwnerPhoneNumber { get; private set; }
        
        public string LandlineNumber { get; private set; }
        
        public string Fax { get; private set; }
        
        public string Area { get; private set; }
        
        public string OwnerName { get; private set; }
        
        public string GenderRestriction { get; private set; }
        
        public bool IsShared { get; private set; }
        
        public string RentUnit { get; private set; }
    }
}
