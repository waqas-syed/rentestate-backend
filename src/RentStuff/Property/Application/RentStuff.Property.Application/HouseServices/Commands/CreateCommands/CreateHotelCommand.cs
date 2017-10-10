using RentStuff.Property.Application.HouseServices.Commands.AbstractCommands;
using System.Runtime.Serialization;

namespace RentStuff.Property.Application.HouseServices.Commands.CreateCommands
{
    /// <summary>
    /// Command for types like Hotel and Guest House
    /// </summary>
    [DataContract]
    public class CreateHotelCommand : GuestPropertyBaseCommand
    {
        public CreateHotelCommand(string title, long rentPrice,
            bool internetAvailable, bool cableTvAvailable,
            bool parkingAvailable, string propertyType, string ownerEmail,
            string ownerPhoneNumber, string area, string ownerName, string description, string genderRestriction,
            bool isShared, string rentUnit, bool laundry, bool ac,
            bool geyser, bool fitnessCentre, bool attachedBathroom, bool ironing,
            bool balcony, bool lawn, bool cctvCameras, bool backupElectricity, bool heating,
            string landlineNumber, string fax, bool elevator, bool restaurant, bool airportShuttle, bool breakfastIncluded, bool sittingArea, bool carRental,
            bool spa, bool salon, bool bathtub, bool swimmingPool, bool kitchen,
            int numberOfAdults, int numberOfChildren)
            : base(title, rentPrice, internetAvailable, cableTvAvailable, parkingAvailable,
                propertyType, ownerEmail, ownerPhoneNumber, area, ownerName, description, genderRestriction,
                isShared, rentUnit, laundry, ac, geyser, fitnessCentre, attachedBathroom, ironing, balcony, lawn,
                cctvCameras, backupElectricity, heating, landlineNumber, fax, elevator)
        {
            Restaurant = restaurant;
            AirportShuttle = airportShuttle;
            BreakfastIncluded = breakfastIncluded;
            SittingArea = sittingArea;
            CarRental = carRental;
            Spa = spa;
            Salon = salon;
            Bathtub = bathtub;
            SwimmingPool = swimmingPool;
            Kitchen = kitchen;
            NumberOfAdults = numberOfAdults;
            NumberOfChildren = numberOfChildren;
        }

        public bool Restaurant { get; private set; }

        public bool AirportShuttle { get; private set; }

        public bool BreakfastIncluded { get; private set; }

        public bool SittingArea { get; private set; }

        public bool CarRental { get; private set; }

        public bool Spa { get; private set; }

        public bool Salon { get; private set; }

        public bool Bathtub { get; private set; }

        public bool SwimmingPool { get; private set; }

        public bool Kitchen { get; private set; }
        
        public int NumberOfAdults { get; private set; }

        public int NumberOfChildren { get; private set; }
    }
}
