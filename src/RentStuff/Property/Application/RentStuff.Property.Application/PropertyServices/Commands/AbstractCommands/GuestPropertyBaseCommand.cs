using System;
using System.Runtime.Serialization;

namespace RentStuff.Property.Application.PropertyServices.Commands.AbstractCommands
{
    /// <summary>
    /// Base Command for properties such as Hotel & Guest House
    /// </summary>
    [Serializable]
    public abstract class GuestPropertyBaseCommand : ResidentialPropertyBaseCommand
    {
        public GuestPropertyBaseCommand(string title, long rentPrice,
            bool internetAvailable, bool cableTvAvailable, bool parkingAvailable,
            string propertyType, string ownerEmail,
            string ownerPhoneNumber, string area, string ownerName, string description, string genderRestriction,
            bool isShared, string rentUnit, bool laundry, bool ac,
            bool geyser, bool fitnessCentre, bool attachedBathroom, bool ironing,
            bool balcony, bool lawn, bool cctvCameras, bool backupElectricity, bool heating,
            string landlineNumber, string fax, bool elevator)
            : base(title, rentPrice, internetAvailable, cableTvAvailable,
                  propertyType, ownerEmail, ownerPhoneNumber, area, ownerName, description, genderRestriction, 
                  isShared, rentUnit, landlineNumber, fax)
        {
            Laundry = laundry;
            AC = ac;
            Geyser = geyser;
            FitnessCentre = fitnessCentre;
            AttachedBathroom = attachedBathroom;
            Ironing = ironing;
            Balcony = balcony;
            Lawn = lawn;
            CctvCameras = cctvCameras;
            BackupElectricity = backupElectricity;
            Heating = heating;
            Elevator = elevator;
            ParkingAvailable = parkingAvailable;
        }

        /// <summary>
        /// Is laundry available
        /// </summary>
        public bool Laundry { get; private set; }

        /// <summary>
        /// Is AC available
        /// </summary>
        public bool AC { get; private set; }

        /// <summary>
        /// Is Geyser available
        /// </summary>
        public bool Geyser { get; private set; }

        /// <summary>
        /// Is Fitness Centre available
        /// </summary>
        public bool FitnessCentre { get; private set; }

        /// <summary>
        /// Is Attached bathroom available
        /// </summary>
        public bool AttachedBathroom { get; private set; }

        /// <summary>
        /// Is ironing available
        /// </summary>
        public bool Ironing { get; private set; }

        /// <summary>
        /// Is Balcony available
        /// </summary>
        public bool Balcony { get; private set; }

        /// <summary>
        /// Is Elevator available in the building
        /// </summary>
        public bool Elevator { get; private set; }

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
        public bool BackupElectricity { get; private set; }

        /// <summary>
        /// Does the place have Heating facility
        /// </summary>        
        public bool Heating { get; set; }

        /// <summary>
        /// is parking available
        /// </summary>        
        public bool ParkingAvailable { get; set; }
    }
}
