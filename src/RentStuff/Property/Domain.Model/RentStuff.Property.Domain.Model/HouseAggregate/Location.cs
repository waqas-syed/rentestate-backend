
namespace RentStuff.Property.Domain.Model.HouseAggregate
{
    /// <summary>
    /// Address of a property
    /// </summary>
    public class Location
    {
        private decimal _latitude;
        private decimal _longitude;
        private string _houseNo;
        private string _streetNo;
        private string _area;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Location()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Location(decimal latitude, decimal longitude, string houseNo, string streetNo, string area)
        {
            _latitude = latitude;
            _longitude = longitude;
            _houseNo = houseNo;
            _streetNo = streetNo;
            _area = area;
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
        /// House Number
        /// </summary>
        public string HouseNo
        {
            get { return _houseNo; } 
            private set { _houseNo = value; }
        }

        /// <summary>
        /// Street Number
        /// </summary>
        public string StreetNo
        {
            get { return _streetNo; }
            private set { _streetNo = value; }
        }

        /// <summary>
        /// Area that contains Town, State, City and Country
        /// </summary>
        public string Area
        {
            get { return _area; }
            private set { _area = value; }
        }
    }
}
