using System;
using System.Runtime.Serialization;
using RentStuff.Property.Application.PropertyServices.Representation.AbstractRepresentations;
using RentStuff.Property.Domain.Model.HouseAggregate;
using Newtonsoft.Json;

namespace RentStuff.Property.Application.PropertyServices.Representation
{
    /// <summary>
    /// Partial Representation for a House
    /// </summary>
    [Serializable]
    [JsonObject]
    public class HousePartialRepresentation : ResidentialPropertyPartialBaseImplementation
    {
        public HousePartialRepresentation(string houseId, string title, string area, long rentPrice, 
            string propertyType, Dimension dimension, string ownerPhoneNumber, 
            string ownerLandlineNumber,
            string image, string ownerName, bool isShared, string genderRestriction, 
            string rentUnit, bool internet, bool cableTv, int numberOfBedrooms, int numberOfBathrooms,
            int numberOfKitchens, bool ac, bool geyser, bool balcony, bool lawn, bool cctvCameras,
            bool backupElectricity, bool heating, bool bathtub, bool elevator, bool garageAvailable,
            bool landlinePhoneAvailable)
            : base(houseId, title, rentPrice, ownerPhoneNumber, ownerLandlineNumber, area, ownerName,
                  genderRestriction, isShared, rentUnit, internet, cableTv, propertyType, image)
        {
            if (dimension != null)
            {
                if (!string.IsNullOrWhiteSpace(dimension.StringValue))
                {
                    Dimension = dimension.StringValue + " " + dimension.DimensionType.ToString();
                }
                else if (!dimension.DecimalValue.Equals(0))
                {
                    Dimension = dimension.DecimalValue + " " + dimension.DimensionType.ToString();
                }
            }
            NumberOfBedrooms = numberOfBedrooms;
            NumberOfBathrooms = numberOfBathrooms;
            NumberOfKitchens = numberOfKitchens;

            GarageAvailable = garageAvailable;
            LandlinePhoneAvailable = landlinePhoneAvailable;
            AC = ac;
            Bathtub = bathtub;
            Geyser = geyser;
            Balcony = balcony;
            Lawn = lawn;
            CctvCameras = cctvCameras;
            BackupElectricity = backupElectricity;
            Heating = heating;
            Elevator = elevator;
        }
        
        public string Dimension { get; set; }
        
        public int NumberOfBedrooms { get; set; }
        
        public int NumberOfBathrooms { get; set; }
        
        public int NumberOfKitchens { get; set; }

        public bool Bathtub { get; private set; }

        public bool GarageAvailable { get; private set; }

        public bool LandlinePhoneAvailable { get; private set; }

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
