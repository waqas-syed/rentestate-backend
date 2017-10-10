using System.Runtime.Serialization;

namespace RentStuff.Property.Application.HouseServices.Commands.AbstractCommands
{
    /// <summary>
    /// Base Command for properties such as Hotel & Guest House
    /// </summary>
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
        [DataMember]
        public bool Laundry { get; private set; }

        /// <summary>
        /// Is AC available
        /// </summary>
        [DataMember]
        public bool AC { get; private set; }

        /// <summary>
        /// Is Geyser available
        /// </summary>
        [DataMember]
        public bool Geyser { get; private set; }

        /// <summary>
        /// Is Fitness Centre available
        /// </summary>
        [DataMember]
        public bool FitnessCentre { get; private set; }

        /// <summary>
        /// Is Attached bathroom available
        /// </summary>
        [DataMember]
        public bool AttachedBathroom { get; private set; }

        /// <summary>
        /// Is ironing available
        /// </summary>
        [DataMember]
        public bool Ironing { get; private set; }

        /// <summary>
        /// Is Balcony available
        /// </summary>
        [DataMember]
        public bool Balcony { get; private set; }

        /// <summary>
        /// Is Elevator available in the building
        /// </summary>
        [DataMember]
        public bool Elevator { get; private set; }

        /// <summary>
        /// Is Lawn available
        /// </summary>
        [DataMember]
        public bool Lawn { get; private set; }

        /// <summary>
        /// Are CCTV Cameras available
        /// </summary>
        [DataMember]
        public bool CctvCameras { get; private set; }

        /// <summary>
        /// Is backup electricity available
        /// </summary>
        [DataMember]
        public bool BackupElectricity { get; private set; }

        /// <summary>
        /// Does the place have Heating facility
        /// </summary>
        [DataMember]
        public bool Heating { get; set; }

        /// <summary>
        /// is parking available
        /// </summary>
        [DataMember]
        public bool ParkingAvailable { get; set; }
    }
}
