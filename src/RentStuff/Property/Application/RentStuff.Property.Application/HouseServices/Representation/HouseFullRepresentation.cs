using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RentStuff.Property.Application.HouseServices.Representation
{
    /// <summary>
    /// The complete representation for House entity
    /// </summary>
    [Serializable]
    [DataContract]
    public class HouseFullRepresentation
    {
        public HouseFullRepresentation(string id, string title, long rent, int numberOfBedrooms, int numberOfKitchens, 
            int numberOfBathrooms, bool internetAvailable, 
            bool landlinePhoneAvailable, bool cableTvAvailable, string dimension, bool garageAvailable, bool smokingAllowed, 
            string propertyType, string ownerEmail, string ownerPhoneNumber, decimal latitude, decimal longitude, string houseNo, 
            string streetNo, string area, IList<string> houseImages, string ownerName, string description,
            string genderRestriction, bool isShared, string rentUnit)
        {
            Id = id;
            Title = title;
            Rent = rent;
            NumberOfBedrooms = numberOfBedrooms;
            NumberOfKitchens = numberOfKitchens;
            NumberOfBathrooms = numberOfBathrooms;
            InternetAvailable = internetAvailable;
            LandlinePhoneAvailable = landlinePhoneAvailable;
            CableTvAvailable = cableTvAvailable;
            Dimension = dimension;
            GarageAvailable = garageAvailable;
            SmokingAllowed = smokingAllowed;
            PropertyType = propertyType;
            OwnerEmail = ownerEmail;
            OwnerPhoneNumber = ownerPhoneNumber;
            Latitude = latitude;
            Longitude = longitude;
            HouseNo = houseNo;
            StreetNo = streetNo;
            Area = area;
            HouseImages = houseImages;
            OwnerName = ownerName;
            Description = description;
            GenderRestriction = genderRestriction;
            IsShared = isShared;
            RentUnit = rentUnit;
        }

        [DataMember]
        public string Id { get; private set; }

        [DataMember]
        public string Title { get; private set; }

        [DataMember]
        public string Description { get; private set; }

        [DataMember]
        public long Rent
        {
            get; private set;
        }

        [DataMember]
        public int NumberOfBedrooms
        {
            get; private set;
        }

        [DataMember]
        public int NumberOfKitchens
        {
            get; private set;
        }
        
        [DataMember]
        public int NumberOfBathrooms
        {
            get; private set;
        }
        
        [DataMember]
        public bool InternetAvailable
        {
            get; private set;
        }

        [DataMember]
        public bool LandlinePhoneAvailable
        {
            get; private set;
        }

        [DataMember]
        public bool CableTvAvailable
        {
            get; private set;
        }

        [DataMember]
        public string Dimension
        {
            get; private set;
        }

        [DataMember]
        public bool GarageAvailable
        {
            get; private set;
        }

        [DataMember]
        public bool SmokingAllowed
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
        public decimal Latitude
        {
            get; private set;
        }

        [DataMember]
        public decimal Longitude
        {
            get; private set;
        }

        [DataMember]
        public string HouseNo
        {
            get; private set;
        }

        [DataMember]
        public string StreetNo
        {
            get; private set;
        }

        [DataMember]
        public string Area
        {
            get; private set;
        }

        [DataMember]
        public IList<string> HouseImages
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
