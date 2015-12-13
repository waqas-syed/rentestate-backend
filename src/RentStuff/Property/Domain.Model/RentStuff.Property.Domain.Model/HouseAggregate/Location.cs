
namespace RentStuff.Property.Domain.Model.HouseAggregate
{
    /// <summary>
    /// Address of a property
    /// </summary>
    public class Location
    {
        private decimal _latitude;
        private decimal _longitude;
        private string _streetAddress;
        private string _city;
        private string _country;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Location()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Location(decimal latitude, decimal longitude, string streetAddress, string city, string country)
        {
            _latitude = latitude;
            _longitude = longitude;
            _streetAddress = streetAddress;
            _city = city;
            _country = country;
        }

        /// <summary>
        /// Database primary key
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public decimal Latitude
        {
            get { return _latitude; }
            private set { _latitude = value; }
        }

        /// <summary>
        /// Longitude
        /// </summary>
        public decimal Longitude
        {
            get { return _longitude; }
            private set { _longitude = value; }
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
