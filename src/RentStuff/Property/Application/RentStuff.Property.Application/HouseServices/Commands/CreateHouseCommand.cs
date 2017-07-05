﻿using System;
using System.Runtime.Serialization;

namespace RentStuff.Property.Application.HouseServices.Commands
{
    /// <summary>
    /// Command for the House instance
    /// </summary>
    [Serializable]
    [DataContract]
    public class CreateHouseCommand
    {
        public CreateHouseCommand(string title, long monthlyRent, int numberOfBedrooms,
            int numberOfKitchens, int numberOfBathrooms,
            bool internetAvailable, bool landlinePhoneAvailable, bool cableTvAvailable, 
            bool garageAvailable, bool smokingAllowed, string propertyType, string ownerEmail, string ownerPhoneNumber, 
            string houseNo, string streetNo, string area, string dimensionType, string dimensionStringValue, 
            decimal dimensionIntValue, string ownerName, string description, string genderRestriction, bool isShared)
        {
            Title = title;
            MonthlyRent = monthlyRent;
            NumberOfBedrooms = numberOfBedrooms;
            NumberOfKitchens = numberOfKitchens;
            NumberOfBathrooms = numberOfBathrooms;
            InternetAvailable = internetAvailable;
            LandlinePhoneAvailable = landlinePhoneAvailable;
            CableTvAvailable = cableTvAvailable;
            GarageAvailable = garageAvailable;
            SmokingAllowed = smokingAllowed;
            PropertyType = propertyType;
            OwnerEmail = ownerEmail;
            OwnerPhoneNumber = ownerPhoneNumber;
            HouseNo = houseNo;
            StreetNo = streetNo;
            Area = area;
            DimensionType = dimensionType;
            DimensionStringValue = dimensionStringValue;
            DimensionIntValue = dimensionIntValue;
            OwnerName = ownerName;
            Description = description;
            GenderRestriction = genderRestriction;
            IsShared = isShared;
        }

        [DataMember]
        public string Title { get; private set; }

        [DataMember]
        public string Description { get; private set; }

        [DataMember]
        public long MonthlyRent { get; private set; }

        [DataMember]
        public int NumberOfBedrooms { get; private set; }

        [DataMember]
        public int NumberOfKitchens { get; private set; }
        
        [DataMember]
        public int NumberOfBathrooms { get; private set; }
        
        [DataMember]
        public bool InternetAvailable { get; private set; }

        [DataMember]
        public bool LandlinePhoneAvailable { get; private set; }

        [DataMember]
        public bool CableTvAvailable { get; private set; }

        [DataMember]
        public bool GarageAvailable { get; private set; }

        [DataMember]
        public bool SmokingAllowed { get; private set; }

        [DataMember]
        public string PropertyType { get; private set; }

        [DataMember]
        public string OwnerEmail { get; private set; }

        [DataMember]
        public string OwnerPhoneNumber { get; private set; }

        [DataMember]
        public string HouseNo { get; private set; }

        [DataMember]
        public string StreetNo { get; private set; }

        [DataMember]
        public string Area { get; private set; }

        [DataMember]
        public string DimensionType { get; private set; }

        [DataMember]
        public string DimensionStringValue { get; private set; }

        [DataMember]
        public decimal DimensionIntValue { get; private set; }

        [DataMember]
        public string OwnerName { get; private set; }

        [DataMember]
        public string GenderRestriction { get; private set; }

        [DataMember]
        public bool IsShared { get; private set; }
    }
}
