using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using RentStuff.Property.Domain.Model.HouseAggregate;

namespace RentStuff.Property.Application.HouseServices.Representation
{
    [Serializable]
    [DataContract]
    public class HouseRepresentation
    {
        public HouseRepresentation(string houseId, string title, string area, long rent, string propertyType, 
            Dimension dimension, int numberOfBedrooms, int numberOfBathrooms, int numberOfKitchens,
            string ownerEmail, string ownerPhoneNumber, string imageString)
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
            ImageString = imageString;
        }

        [DataMember]
        public string HouseId { get; private set; }

        [DataMember]
        public string Title { get; private set; }

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
        public string ImageString { get; set; }
    }
}
