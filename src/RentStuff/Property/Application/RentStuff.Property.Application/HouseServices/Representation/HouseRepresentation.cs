using System;
using System.Runtime.Serialization;
using RentStuff.Property.Domain.Model.HouseAggregate;

namespace RentStuff.Property.Application.HouseServices.Representation
{
    [Serializable]
    [DataContract]
    public class HouseRepresentation
    {
        public HouseRepresentation(string title, string area, long rent, string propertyType, Dimension dimension, int bedrooms, int bathrooms)
        {
            Title = title;
            Area = area;
            Rent = rent;
            PropertyType = propertyType;
            if (dimension != null)
            {
                if (!string.IsNullOrWhiteSpace(dimension.StringValue))
                {
                    Dimension = dimension.StringValue + dimension.DimensionType.ToString();
                }
                else if (!dimension.DecimalValue.Equals(0))
                {
                    Dimension = dimension.DecimalValue + dimension.DimensionType.ToString();
                }
            }
            Bedrooms = bedrooms;
            Bathrooms = bathrooms;
        }

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
        public int Bedrooms { get; set; }

        [DataMember]
        public int Bathrooms { get; set; }
    }
}
