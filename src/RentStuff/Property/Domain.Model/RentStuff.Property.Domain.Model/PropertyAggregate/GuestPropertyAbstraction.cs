using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Property.Domain.Model.PropertyAggregate
{
    /// <summary>
    /// This class is an abstraction of proeprty types Hostel, Hotel and Guest Houses, thus it contains the attributes
    /// that are common within all of these
    /// </summary>
    public class GuestPropertyAbstraction : ResidentialProperty
    {
        /// <summary>
        /// Default Constructor to support Nhibernate
        /// </summary>
        public GuestPropertyAbstraction()
        {
            
        }

        /// <summary>
        /// Initializes the general abstraction for Hostel
        /// </summary>
        /// <param name="title"></param>
        /// <param name="rentPrice"></param>
        /// <param name="ownerEmail"></param>
        /// <param name="ownerPhoneNumber"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="area"></param>
        /// <param name="ownerName"></param>
        /// <param name="description"></param>
        /// <param name="genderRestriction"></param>
        /// <param name="isShared"></param>
        /// <param name="internetAvailable"></param>
        /// <param name="cableTvAvailable"></param>
        /// <param name="parkingAvailable"></param>
        /// <param name="rentUnit"></param>
        /// <param name="laundry"></param>
        /// <param name="ac"></param>
        /// <param name="geyser"></param>
        /// <param name="fitnessCentre"></param>
        /// <param name="attachedBathroom"></param>
        /// <param name="ironing"></param>
        /// <param name="balcony"></param>
        /// <param name="lawn"></param>
        /// <param name="cctvCameras"></param>
        /// <param name="backupElectricity"></param>
        /// <param name="heating"></param>
        /// <param name="elevator"></param>
        public GuestPropertyAbstraction(string title, long rentPrice, string ownerEmail,
                string ownerPhoneNumber,
                decimal latitude, decimal longitude, string area, string ownerName,
                string description,
                GenderRestriction genderRestriction, bool isShared, string rentUnit, bool internetAvailable,
                bool cableTvAvailable, bool parkingAvailable, string propertyType, bool laundry, bool ac, 
                bool geyser, bool fitnessCentre, bool attachedBathroom, bool ironing,
                bool balcony, bool lawn, bool cctvCameras, bool backupElectricity, bool heating, 
                string landlineNumber, string fax, bool elevator)
            // Initiate the parent Property class as well
            : base(title, rentPrice, ownerEmail,
                ownerPhoneNumber, latitude, longitude, area, ownerName, description, genderRestriction, isShared,
                rentUnit, internetAvailable, cableTvAvailable, propertyType, landlineNumber, fax)
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
        /// Update the GuestProperty Abstraction
        /// </summary>
        /// <param name="title"></param>
        /// <param name="rentPrice"></param>
        /// <param name="ownerEmail"></param>
        /// <param name="ownerPhoneNumber"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="area"></param>
        /// <param name="ownerName"></param>
        /// <param name="description"></param>
        /// <param name="genderRestriction"></param>
        /// <param name="isShared"></param>
        /// <param name="rentUnit"></param>
        /// <param name="internetAvailable"></param>
        /// <param name="cableTvAvailable"></param>
        /// <param name="parkingAvailable"></param>
        /// <param name="propertyType"></param>
        /// <param name="laundry"></param>
        /// <param name="ac"></param>
        /// <param name="geyser"></param>
        /// <param name="fitnessCentre"></param>
        /// <param name="attachedBathroom"></param>
        /// <param name="ironing"></param>
        /// <param name="balcony"></param>
        /// <param name="lawn"></param>
        /// <param name="cctvCameras"></param>
        /// <param name="backupElectricity"></param>
        /// <param name="heating"></param>
        /// <param name="landlineNumber"></param>
        /// <param name="fax"></param>
        /// <param name="elevator"></param>
        protected void UpdateGuestProperty(string title, long rentPrice, string ownerEmail,
            string ownerPhoneNumber,
            decimal latitude, decimal longitude, string area, string ownerName,
            string description,
            GenderRestriction genderRestriction, bool isShared, string rentUnit, bool internetAvailable,
            bool cableTvAvailable, bool parkingAvailable, string propertyType, bool laundry, bool ac,
            bool geyser, bool fitnessCentre, bool attachedBathroom, bool ironing,
            bool balcony, bool lawn, bool cctvCameras, bool backupElectricity, bool heating,
            string landlineNumber, string fax, bool elevator)
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
            base.Update(title, rentPrice, ownerEmail, ownerPhoneNumber, area, ownerName, description, 
                genderRestriction, latitude, longitude, isShared, rentUnit, internetAvailable, cableTvAvailable, 
                propertyType, landlineNumber, fax);
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
        
        /// <summary>
        /// Garage available
        /// </summary>
        public bool ParkingAvailable
        {
            get;
            private set;
        }
    }
}
