using System.Collections.Generic;
using System.Runtime.Serialization;
using RentStuff.Property.Application.PropertyServices.Representation.AbstractRepresentations;

namespace RentStuff.Property.Application.PropertyServices.Representation.FullRepresentations
{
    /// <summary>
    /// Full representation for Hostel
    /// </summary>
    [DataContract]
    public class HostelFullRepresentation : GuestPropertyBaseRepresentation
    {
        public HostelFullRepresentation(string id, string title, long rentPrice, string ownerEmail,
                string ownerPhoneNumber,
                decimal latitude, decimal longitude, string area, string ownerName,
                string description,
                string genderRestriction, bool isShared, string rentUnit, bool internetAvailable,
                bool cableTvAvailable, bool parkingAvailable, string propertyType, bool laundry, bool ac,
                bool geyser, bool fitnessCentre, bool attachedBathroom, bool ironing,
                bool balcony, bool lawn, bool cctvCameras, bool backupElectricity, bool heating, bool meals,
                bool picknDrop, int numberOfSeats, string landlineNumber, string fax, bool elevator,
                IList<string> images)
            // Initiate the parent GuerstPropertyAbstraction class as well
            : base(id, title, rentPrice,internetAvailable, cableTvAvailable, parkingAvailable, propertyType,
                  ownerEmail, ownerPhoneNumber, area, ownerName, description, genderRestriction, isShared,
                  rentUnit, laundry, ac, geyser, fitnessCentre, attachedBathroom, ironing, balcony, lawn,
                  cctvCameras, backupElectricity, heating, landlineNumber, fax, elevator, images)
        {
            Meals = meals;
            PicknDrop = picknDrop;
            NumberOfSeats = numberOfSeats;
        }

        /// <summary>
        /// Is Meals available
        /// </summary>
        public bool Meals { get; private set; }

        /// <summary>
        /// Is Pick n Drop available
        /// </summary>
        public bool PicknDrop { get; private set; }

        /// <summary>
        /// How many seats are there in this hostel room
        /// </summary>
        public int NumberOfSeats { get; private set; }
    }
}
