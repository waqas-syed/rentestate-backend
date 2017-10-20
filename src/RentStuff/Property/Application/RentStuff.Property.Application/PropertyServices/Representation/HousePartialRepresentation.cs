using System;
using System.Runtime.Serialization;
using RentStuff.Property.Application.PropertyServices.Representation.AbstractRepresentations;
using RentStuff.Property.Domain.Model.HouseAggregate;

namespace RentStuff.Property.Application.PropertyServices.Representation
{
    /// <summary>
    /// Partial Representation for a House
    /// </summary>
    [Serializable]
    [DataContract]
    public class HousePartialRepresentation : ResidentialPropertyPartialBaseImplementation
    {
        public HousePartialRepresentation(string houseId, string title, string area, long rentPrice, 
            string propertyType, Dimension dimension, string ownerPhoneNumber, 
            string ownerLandlineNumber,
            string image, string ownerName, bool isShared, string genderRestriction, 
            string rentUnit, bool internet, bool cableTv, int numberOfBedrooms, int numberOfBathrooms,
            int numberOfKitchens)
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
        }
        
        [DataMember]
        public string Dimension { get; private set; }

        [DataMember]
        public int NumberOfBedrooms { get; private set; }

        [DataMember]
        public int NumberOfBathrooms { get; private set; }

        [DataMember]
        public int NumberOfKitchens { get; private set; }
    }
}
