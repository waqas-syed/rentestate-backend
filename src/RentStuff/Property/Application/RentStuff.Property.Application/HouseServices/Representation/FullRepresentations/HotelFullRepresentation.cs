using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentStuff.Property.Application.HouseServices.Representation.AbstractRepresentations;

namespace RentStuff.Property.Application.HouseServices.Representation.FullRepresentations
{
    /// <summary>
    /// Full representation of Hotel
    /// </summary>
    public class HotelFullRepresentation : GuestPropertyBaseRepresentation
    {
        public HotelFullRepresentation(string id, string title, long rentPrice, string ownerEmail,
                string ownerPhoneNumber,
                string area, string ownerName,
                string description,
                string genderRestriction, bool isShared, string rentUnit, bool internetAvailable,
                bool cableTvAvailable, bool parkingAvailable, string propertyType, bool laundry, bool ac,
                bool geyser, bool fitnessCentre, bool attachedBathroom, bool ironing,
                bool balcony, bool lawn, bool cctvCameras, bool backupElectricity, bool heating,
                bool restaurant, bool airportShuttle, bool breakfastIncluded, bool sittingArea, bool carRental,
                bool spa, bool salon, bool bathtub, bool swimmingPool, bool kitchen,
                int numberOfAdults, int numberOfChildren, int totalOccupants, string landlineNumber, string fax, 
                bool elevator, IList<string> images)
            // Initiate the parent GuerstPropertyAbstraction class as well
            : base(id, title, rentPrice, internetAvailable, cableTvAvailable, parkingAvailable, propertyType,
                  ownerEmail, ownerPhoneNumber, area, ownerName, description, genderRestriction, isShared,
                  rentUnit, laundry, ac, geyser, fitnessCentre, attachedBathroom, ironing, balcony, lawn, 
                  cctvCameras, backupElectricity, heating, landlineNumber, fax, elevator, images)
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
            TotalOccupants = totalOccupants;
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

        public int TotalOccupants { get; private set; }
    }
}
