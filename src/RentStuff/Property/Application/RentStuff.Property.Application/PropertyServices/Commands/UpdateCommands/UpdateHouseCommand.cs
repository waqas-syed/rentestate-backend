﻿using System;
using System.Runtime.Serialization;

namespace RentStuff.Property.Application.PropertyServices.Commands.UpdateCommands
{
    /// <summary>
    /// Updates en existing House instance
    /// </summary>
    [Serializable]
    public class UpdateHouseCommand
    {
        public UpdateHouseCommand(string id, string title, long rentPrice, int numberOfBedrooms,
            int numberOfKitchens, int numberOfBathrooms,
            bool internetAvailable, bool landlinePhoneAvailable, bool cableTvAvailable,
            bool garageAvailable, bool smokingAllowed, string propertyType, string ownerEmail, string ownerPhoneNumber,
            string houseNo, string streetNo, string area, string dimensionType, string dimensionStringValue,
            decimal dimensionIntValue, string ownerName, string description, string genderRestriction, bool isShared,
            string rentUnit, string landlineNumber, string fax, bool ac, bool geyser,
            bool balcony, bool lawn, bool cctvCameras, bool backupElectricity, bool heating,
            bool bathtub, bool elevator)
        {
            Id = id;
            Title = title;
            RentPrice = rentPrice;
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
            RentUnit = rentUnit;
            LandlineNumber = landlineNumber;

            Fax = fax;
            Bathtub = bathtub;
            AC = ac;
            Geyser = geyser;
            Balcony = balcony;
            Lawn = lawn;
            CctvCameras = cctvCameras;
            BackupElectricity = backupElectricity;
            Heating = heating;
            Elevator = elevator;
        }

        
        public string Id { get; private set; }
        
        public string Title { get; private set; }
        
        public string Description { get; private set; }
        
        public long RentPrice { get; private set; }
        
        public int NumberOfBedrooms { get; private set; }
        
        public int NumberOfKitchens { get; private set; }
        
        public bool FamiliesOnly { get; private set; }
        
        public int NumberOfBathrooms { get; private set; }
        
        public bool GirlsOnly { get; private set; }
        
        public bool BoysOnly { get; private set; }
        
        public bool InternetAvailable { get; private set; }
        
        public bool LandlinePhoneAvailable { get; private set; }
        
        public bool CableTvAvailable { get; private set; }
        
        public bool GarageAvailable { get; private set; }
        
        public bool SmokingAllowed { get; private set; }
        
        public string PropertyType { get; private set; }
        
        public string OwnerEmail { get; private set; }
        
        public string OwnerPhoneNumber { get; private set; }
        
        public string HouseNo { get; private set; }
        
        public string StreetNo { get; private set; }
        
        public string Area { get; private set; }
        
        public string DimensionType { get; private set; }
        
        public string DimensionStringValue { get; private set; }
        
        public decimal DimensionIntValue { get; private set; }
        
        public string OwnerName { get; private set; }
        
        public string GenderRestriction { get; private set; }
        
        public bool IsShared { get; private set; }
        
        public string RentUnit { get; private set; }

        
        public string LandlineNumber { get; private set; }

        
        public string Fax { get; private set; }

        public bool Bathtub { get; private set; }

        /// <summary>
        /// Is AC available
        /// </summary>
        public bool AC { get; private set; }

        /// <summary>
        /// Is Geyser available
        /// </summary>
        public bool Geyser { get; private set; }

        /// <summary>
        /// Is Balcony available
        /// </summary>
        public bool Balcony { get; private set; }

        /// <summary>
        /// Is Elevator available in the building
        /// </summary>
        public bool Elevator { get; set; }

        /// <summary>
        /// Is Lawn available
        /// </summary>
        public bool Lawn { get; private set; }

        /// <summary>
        /// Are CCTV Cameras available
        /// </summary>
        public bool CctvCameras { get; private set; }

        /// <summary>
        /// Is backup electricity available
        /// </summary>
        public bool BackupElectricity { get; set; }

        /// <summary>
        /// Does the place have Heating facility
        /// </summary>
        public bool Heating { get; set; }
    }
}
