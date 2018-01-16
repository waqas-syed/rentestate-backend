using System.Runtime.Serialization;
using RentStuff.Property.Application.PropertyServices.Representation.AbstractRepresentations;

namespace RentStuff.Property.Application.PropertyServices.Representation.PartialRepresentations
{
    /// <summary>
    /// Partial Representation of a Hostel
    /// </summary>
    [DataContract]
    public class HostelPartialRepresentation : ResidentialPropertyPartialBaseImplementation
    {
        public HostelPartialRepresentation(string id, string title, long rentPrice,
        string ownerPhoneNumber, string ownerLandlineNumber, string area, string ownerName,
        string genderRestriction, bool isShared, string rentUnit, bool internetAvailable,
        bool cableTvAvailable, string propertyType, bool parkingAvailable,  bool laundry, bool ac,
        bool geyser, bool attachedBathroom, bool backupElectricity, bool meals, int numberOfSeats,
        string defautImage)
            : base(id, title, rentPrice, ownerPhoneNumber, ownerLandlineNumber, area, ownerName,
                  genderRestriction, isShared, rentUnit, internetAvailable, cableTvAvailable, propertyType,
                  defautImage)
        {
            Parking = parkingAvailable;
            Laundry = laundry;
            AC = ac;
            Geyser = geyser;
            AttachedBathroom = attachedBathroom;
            BackupElectricity = backupElectricity;
            Meals = meals;
            NumberOfSeats = numberOfSeats;
        }
        
        public bool Parking { get; private set; }
        public bool Laundry { get; private set; }
        public bool AC { get; private set; }
        public bool Geyser { get; private set; }
        public bool AttachedBathroom { get; private set; }
        public bool BackupElectricity { get; private set; }
        public bool Meals { get; private set; }
        public int NumberOfSeats { get; private set; }
    }
}
