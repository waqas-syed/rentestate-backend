using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using RentStuff.Property.Application.HouseServices.Representation.AbstractRepresentations;

namespace RentStuff.Property.Application.HouseServices.Representation
{
    /// <summary>
    /// The complete representation for House entity
    /// </summary>
    [Serializable]
    [DataContract]
    public class HouseFullRepresentation : ResidentialPropertyBaseRepresentation
    {
        public HouseFullRepresentation(string id, string title, long rentPrice, int numberOfBedrooms, int numberOfKitchens, 
            int numberOfBathrooms, bool internetAvailable, 
            bool landlinePhoneAvailable, bool cableTvAvailable, string dimension, bool garageAvailable, bool smokingAllowed, 
            string propertyType, string ownerEmail, string ownerPhoneNumber, string houseNo, 
            string streetNo, string area, IList<string> houseImages, string ownerName, string description,
            string genderRestriction, bool isShared, string rentUnit, string landlineNumber, string fax)
            : base(id, title, rentPrice, internetAvailable, cableTvAvailable, propertyType, ownerEmail, 
                  ownerPhoneNumber, area, ownerName, description, genderRestriction, isShared, rentUnit, 
                  landlineNumber, fax, houseImages)
        {
            NumberOfBedrooms = numberOfBedrooms;
            NumberOfKitchens = numberOfKitchens;
            NumberOfBathrooms = numberOfBathrooms;
            LandlinePhoneAvailable = landlinePhoneAvailable;
            Dimension = dimension;
            GarageAvailable = garageAvailable;
            SmokingAllowed = smokingAllowed;
            HouseNo = houseNo;
            StreetNo = streetNo;
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
        public bool LandlinePhoneAvailable
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
        public IList<string> HouseImages
        {
            get; private set;
        }
    }
}
