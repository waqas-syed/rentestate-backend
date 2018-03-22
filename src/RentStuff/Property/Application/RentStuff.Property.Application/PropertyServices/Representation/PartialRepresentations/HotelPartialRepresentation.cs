using System.Collections.Generic;
using RentStuff.Property.Application.PropertyServices.Representation.AbstractRepresentations;
using RentStuff.Property.Domain.Model.HotelAggregate;

namespace RentStuff.Property.Application.PropertyServices.Representation.PartialRepresentations
{
    /// <summary>
    /// partial repreentation for Hotel and Guest House
    /// </summary>
    public class HotelPartialRepresentation : ResidentialPropertyPartialBaseImplementation
    {
        public HotelPartialRepresentation(string id, string title, long rentPrice,
            string ownerPhoneNumber, string area, string ownerName,
            string genderRestriction, bool isShared, string rentUnit, bool internetAvailable,
            bool cableTvAvailable, bool parkingAvailable, string propertyType, bool ac,
            bool geyser, bool attachedBathroom, bool fitnessCenter,
            bool backupElectricity, bool heating,
            bool airportShuttle, bool breakfastIncluded,
            Occupants occupants, string landlineNumber, string defaultImage, int numberOfSingleBeds, 
            int numberOfDoubleBeds)
            : base(id, title, rentPrice, ownerPhoneNumber, landlineNumber, area, ownerName, genderRestriction,
                  isShared, rentUnit, internetAvailable, cableTvAvailable, propertyType, defaultImage)
        {
            Parking = parkingAvailable;
            AC = ac;
            Geyser = geyser;
            AttachedBathroom = attachedBathroom;
            FitnessCentre = fitnessCenter;
            BackupElectricity = backupElectricity;
            Heating = heating;
            AirportShuttle = airportShuttle;
            BreakfastIncluded = breakfastIncluded;
            NumberOfSingleBeds = numberOfSingleBeds;
            NumberOfDoubleBeds = numberOfDoubleBeds;
            Occupants = occupants;
        }

        public bool Parking { get; private set; }
        public bool AC { get; private set; }
        public bool Geyser { get; private set; }
        public bool AttachedBathroom { get; private set; }
        public bool FitnessCentre { get; private set; }
        public bool BackupElectricity { get; private set; }
        public bool Heating { get; private set; }
        public bool AirportShuttle { get; private set; }
        public bool BreakfastIncluded { get; private set; }
        public int NumberOfSingleBeds { get; private set; }
        public int NumberOfDoubleBeds { get; private set; }
        public Occupants Occupants { get; private set; }
    }
}
