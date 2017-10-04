using System;
using System.Runtime.Serialization;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Domain.Model.PropertyAggregate;

namespace RentStuff.Property.Application.HouseServices.Representation
{
    [Serializable]
    [DataContract]
    public class HousePartialRepresentation
    {
        public HousePartialRepresentation(string houseId, string title, string area, long rentPrice, string propertyType, 
            Dimension dimension, int numberOfBedrooms, int numberOfBathrooms, int numberOfKitchens,
            string ownerEmail, string ownerPhoneNumber, string image, string ownerName, string description, bool isShared,
            string genderRestriction, string rentUnit)
        {
            HouseId = houseId;
            Title = title;
            Area = area;
            RentPrice = rentPrice;
            PropertyType = propertyType;
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
            OwnerEmail = ownerEmail;
            OwnerPhoneNumber = ownerPhoneNumber;
            Image = image;
            OwnerName = ownerName;
            Description = description;
            IsShared = isShared;
            GenderRestriction = genderRestriction;
            RentUnit = rentUnit;
        }

        [DataMember]
        public string HouseId { get; private set; }

        [DataMember]
        public string Title { get; private set; }

        [DataMember]
        public string Description { get; private set; }

        [DataMember]
        public string Area { get; private set; }

        [DataMember]
        public long RentPrice { get; private set; }

        [DataMember]
        public string PropertyType { get; private set; }

        [DataMember]
        public string Dimension { get; private set; }

        [DataMember]
        public int NumberOfBedrooms { get; private set; }

        [DataMember]
        public int NumberOfBathrooms { get; private set; }

        [DataMember]
        public int NumberOfKitchens { get; private set; }

        [DataMember]
        public string OwnerEmail { get; private set; }

        [DataMember]
        public string OwnerPhoneNumber { get; private set; }

        [DataMember]
        public string Image { get; private set; }

        [DataMember]
        public string OwnerName { get; private set; }

        [DataMember]
        public bool IsShared { get; private set; }

        [DataMember]
        public string GenderRestriction { get; private set; }

        [DataMember]
        public string RentUnit { get; private set; }
    }
}
