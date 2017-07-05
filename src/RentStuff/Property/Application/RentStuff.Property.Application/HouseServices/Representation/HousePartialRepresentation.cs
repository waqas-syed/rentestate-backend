using System;
using System.Runtime.Serialization;
using RentStuff.Property.Domain.Model.HouseAggregate;

namespace RentStuff.Property.Application.HouseServices.Representation
{
    [Serializable]
    [DataContract]
    public class HousePartialRepresentation
    {
        public HousePartialRepresentation(string houseId, string title, string area, long rent, string propertyType, 
            Dimension dimension, int numberOfBedrooms, int numberOfBathrooms, int numberOfKitchens,
            string ownerEmail, string ownerPhoneNumber, string image, string ownerName, string description, bool isShared)
        {
            HouseId = houseId;
            Title = title;
            Area = area;
            Rent = rent;
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
        public long Rent { get; private set; }

        [DataMember]
        public string PropertyType { get; private set; }

        [DataMember]
        public string Dimension { get; set; }

        [DataMember]
        public int NumberOfBedrooms { get; set; }

        [DataMember]
        public int NumberOfBathrooms { get; set; }

        [DataMember]
        public int NumberOfKitchens { get; set; }

        [DataMember]
        public string OwnerEmail { get; set; }

        [DataMember]
        public string OwnerPhoneNumber { get; set; }

        [DataMember]
        public string Image { get; set; }

        [DataMember]
        public string OwnerName { get; set; }

        [DataMember]
        public bool IsShared { get; set; }
    }
}
