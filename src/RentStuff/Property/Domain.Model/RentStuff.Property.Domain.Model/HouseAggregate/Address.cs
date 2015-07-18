
namespace RentStuff.Property.Domain.Model.HouseAggregate
{
    /// <summary>
    /// Address of a property
    /// </summary>
    public class Address
    {
        private string _streetAddress;
        private string _city;
        private string _country;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Address()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Address(string streetAddress, string city, string country)
        {
            _streetAddress = streetAddress;
            _city = city;
            _country = country;
        }

        /// <summary>
        /// Street Address
        /// </summary>
        public string StreetAddress
        {
            get { return _streetAddress; } 
            private set { _streetAddress = value; }
        }

        /// <summary>
        /// City
        /// </summary>
        public string City
        {
            get { return _city; }
            private set { _city = value; }
        }

        /// <summary>
        /// Country
        /// </summary>
        public string Country
        {
            get { return _country; }
            private set { _country = value; }
        }
    }
}
