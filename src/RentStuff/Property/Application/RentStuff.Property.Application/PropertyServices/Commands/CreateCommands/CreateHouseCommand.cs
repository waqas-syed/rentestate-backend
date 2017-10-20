using System;
using RentStuff.Property.Application.PropertyServices.Commands.AbstractCommands;

namespace RentStuff.Property.Application.PropertyServices.Commands.CreateCommands
{
    /// <summary>
    /// Command for the House instance
    /// </summary>
    [Serializable]
    public class CreateHouseCommand : ResidentialPropertyBaseCommand
    {
        public CreateHouseCommand(string title, long rentPrice, int numberOfBedrooms,
            int numberOfKitchens, int numberOfBathrooms,
            bool internetAvailable, bool landlinePhoneAvailable, bool cableTvAvailable, 
            bool garageAvailable, bool smokingAllowed, string propertyType, string ownerEmail, string ownerPhoneNumber, 
            string houseNo, string streetNo, string area, string dimensionType, string dimensionStringValue, 
            decimal dimensionIntValue, string ownerName, string description, string genderRestriction, bool isShared,
            string rentUnit, string landlineNumber, string fax)
            : base(title, rentPrice, internetAvailable, cableTvAvailable, propertyType,
                  ownerEmail, ownerPhoneNumber, area, ownerName, description, genderRestriction, isShared, rentUnit,
                  landlineNumber, fax)
        {
            NumberOfBedrooms = numberOfBedrooms;
            NumberOfKitchens = numberOfKitchens;
            NumberOfBathrooms = numberOfBathrooms;
            SmokingAllowed = smokingAllowed;
            HouseNo = houseNo;
            StreetNo = streetNo;
            DimensionType = dimensionType;
            DimensionStringValue = dimensionStringValue;
            DimensionIntValue = dimensionIntValue;
            GarageAvailable = garageAvailable;
            LandlinePhoneAvailable = landlinePhoneAvailable;
        }
        
        public int NumberOfBedrooms { get; private set; }
        
        public int NumberOfKitchens { get; private set; }
        
        public int NumberOfBathrooms { get; private set; }
        
        public bool GarageAvailable { get; private set; }
        
        public bool SmokingAllowed { get; private set; }
        
        public bool LandlinePhoneAvailable { get; private set; }
        
        public string HouseNo { get; private set; }
        
        public string StreetNo { get; private set; }
        
        public string DimensionType { get; private set; }
        
        public string DimensionStringValue { get; private set; }
        
        public decimal DimensionIntValue { get; private set; }
    }
}
