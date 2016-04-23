using System;
using System.IO;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Dimension(DimensionType dimensionType, string stringValue, decimal decimalValue)
        {
            DimensionType = dimensionType;
            StringValue = stringValue;
            DecimalValue = decimalValue;
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
            set { _dimensionType = value; }
        }

        /// <summary>
        /// The string representation of the value, can be mentioned as string (or int which is another memeber of this class)
        /// </summary>
        public string StringValue
        {
            get { return _stringValue; }
            private set
            {
                if (_decimalValue==decimal.Zero)
                {
                    _stringValue = value;   
                }
                else
                {
                    throw new InvalidDataException("Cannot assign string value when decimal value is already assigned for type Dimension");
                }
            }
        }

        /// <summary>
        /// The decimal representation of the value, can be mentioned as decimal (or string which is another memeber of this class)
        /// </summary>
        public decimal DecimalValue
        {
            get { return _decimalValue; }
            private set
            {
                if (string.IsNullOrEmpty(_stringValue))
                {
                    _decimalValue = value;
                }
                else
                {
                    throw new InvalidDataException("Cannot assign decimal value when string value is already assigned for type Dimension");
                }
            }
        }
    }
}
