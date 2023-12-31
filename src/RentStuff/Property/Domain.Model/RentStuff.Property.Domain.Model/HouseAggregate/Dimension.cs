﻿using System;

namespace RentStuff.Property.Domain.Model.HouseAggregate
{
    /// <summary>
    /// Specifies the size of the house in terms of a type, e.g., Marla, Kanal, Acre etc
    /// </summary>
    public class Dimension
    {
        private string _id = Guid.NewGuid().ToString();
        // The type of dimension in which value is mentioned
        private DimensionType _dimensionType;
        // The value can either be mentioned as a string
        private string _stringValue;
        // Or the value can either be mentioned as a decimal
        private decimal _decimalValue;
        private House _house;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Dimension()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Dimension(DimensionType dimensionType, string stringValue, decimal decimalValue, House house)
        {
            DimensionType = dimensionType;
            if (!string.IsNullOrWhiteSpace(stringValue))
            {
                StringValue = stringValue;
            }
            else
            {
                DecimalValue = decimalValue;
            }
            House = house;
        }

        /// <summary>
        /// Id
        /// </summary>
        public string Id
        {
            get { return _id; }
            private set { _id = value; }
        }

        /// <summary>
        /// The type of dimension in which value is mentioned, e.g., Marla, Kanal, Acre etc
        /// </summary>
        public DimensionType DimensionType
        {
            get { return _dimensionType; }
            private set { _dimensionType = value; }
        }

        /// <summary>
        /// The string representation of the value, can be mentioned as string (or int which is another memeber of this class)
        /// </summary>
        public string StringValue
        {
            get { return _stringValue; }
            private set { _stringValue = value; }
        }

        /// <summary>
        /// The decimal representation of the value, can be mentioned as decimal (or string which is another memeber of this class)
        /// </summary>
        public decimal DecimalValue
        {
            get { return _decimalValue; }
            private set { _decimalValue = value; }
        }

        /// <summary>
        /// House to which this dimension is related to
        /// </summary>
        public House House
        {
            get { return _house; }
            private set { _house = value; }
        }
    }
}
