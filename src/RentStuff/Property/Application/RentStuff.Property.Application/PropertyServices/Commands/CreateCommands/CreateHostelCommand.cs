using System;
using System.Runtime.Serialization;
using RentStuff.Property.Application.PropertyServices.Commands.AbstractCommands;

namespace RentStuff.Property.Application.PropertyServices.Commands.CreateCommands
{
    /// <summary>
    /// Create a new Hostel
    /// </summary>
    [Serializable]
    public class CreateHostelCommand : GuestPropertyBaseCommand
    {
        public CreateHostelCommand(string title, long rentPrice,
            bool internetAvailable, bool cableTvAvailable,
            bool parkingAvailable, string propertyType, string ownerEmail,
            string ownerPhoneNumber, string area, string ownerName, string description, string genderRestriction,
            bool isShared, string rentUnit, bool laundry, bool ac,
            bool geyser, bool fitnessCentre, bool attachedBathroom, bool ironing,
            bool balcony, bool lawn, bool cctvCameras, bool backupElectricity, bool heating,
            string landlineNumber, string fax, bool elevator, bool meals, bool picknDrop,
            int numberOfSeats)
            : base(title, rentPrice, internetAvailable, cableTvAvailable, parkingAvailable,
                  propertyType, ownerEmail, ownerPhoneNumber, area, ownerName, description, genderRestriction,
                  isShared, rentUnit, laundry, ac, geyser, fitnessCentre, attachedBathroom, ironing, balcony, lawn,
                  cctvCameras, backupElectricity, heating, landlineNumber, fax, elevator)
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
