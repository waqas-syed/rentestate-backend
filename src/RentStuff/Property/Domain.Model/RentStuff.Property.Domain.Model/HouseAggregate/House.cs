
using RentStuff.Common.Domain.Model;

namespace RentStuff.Property.Domain.Model.HouseAggregate
{
    /// <summary>
    /// House Aggregate. A house that can be put on rent or sale
    /// </summary>
    public class House
    {
        private Address _location;
        private long _price;
        private bool _forRent;
        private int _numberOfBedrooms;
        private int _numberOfKitchens;
        private int _numberOfBathrooms;
        private bool _familiesOnly;
        private bool _girlsOnly;
        private bool _internetAvailable;
        private bool _landlinePhoneAvailable;
        private bool _cableTvAvailable;
        private Size _size;
        private bool _garageAvailable;
        private bool _smokingAllowed;
        private PropertyType _propertyType;
        private string _ownerEmail;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public House()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public House(Address location, long price, bool forRent, int numberOfBedrooms,
            int numberOfKitchens, int numberOfBathrooms, bool familiesOnly, bool girlsOnly,
            bool internetAvailable, bool landlinePhoneAvailable, bool cableTvAvailable, Size size, 
            bool garageAvailable, bool smokingAllowed, PropertyType propertyType, string ownerEmail)
        {
            _location = location;
            _price = price;
            _forRent = forRent;
            _numberOfBedrooms = numberOfBedrooms;
            _numberOfKitchens = numberOfKitchens;
            _numberOfBathrooms = numberOfBathrooms;
            _familiesOnly = familiesOnly;
            _girlsOnly = girlsOnly;
            _internetAvailable = internetAvailable;
            _landlinePhoneAvailable = landlinePhoneAvailable;
            _cableTvAvailable = cableTvAvailable;
            _size = size;
            _garageAvailable = garageAvailable;
            _smokingAllowed = smokingAllowed;
            _propertyType = propertyType;
            _ownerEmail = ownerEmail;
        }

        /// <summary>
        /// House id
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Location
        /// </summary>
        public Address Location
        {
            get { return _location; }
            set
            {
                Assertion.AssertNotNull(value);
                _location = value;
            }
        }

        /// <summary>
        /// Monthly Rent
        /// </summary>
        public long Price
        {
            get { return _price; }
            set
            {
                Assertion.AssertNumberNotZero(value);
                _price = value;
            }
        }

        /// <summary>
        /// Is the property for rent or for sale
        /// </summary>
        public bool ForRent
        {
            get { return _forRent; }
            set { _forRent = value; }
        }

        public int NumberOfBedrooms
        {
            get { return _numberOfBedrooms; }
            set { _numberOfBedrooms = value; }
        }

        public int NumberOfKitchens
        {
            get { return _numberOfKitchens; }
            set { _numberOfKitchens = value; }
        }

        public bool FamiliesOnly
        {
            get { return _familiesOnly; }
            set { _familiesOnly = value; }
        }

        public int NumberOfBathrooms
        {
            get { return _numberOfBathrooms; }
            set { _numberOfBathrooms = value; }
        }

        public bool GirlsOnly
        {
            get { return _girlsOnly; }
            set { _girlsOnly = value; }
        }

        public bool InternetAvailable
        {
            get { return _internetAvailable; }
            set { _internetAvailable = value; }
        }

        public bool LandlinePhoneAvailable
        {
            get { return _landlinePhoneAvailable; }
            set { _landlinePhoneAvailable = value; }
        }

        public bool CableTvAvailable
        {
            get { return _cableTvAvailable; }
            set { _cableTvAvailable = value; }
        }

        public Size Size
        {
            get { return _size; } 
            set { _size = value; }
        }

        public bool GarageAvailable
        {
            get { return _garageAvailable; }
            set { _garageAvailable = value; }
        }

        public bool SmokingAllowed
        {
            get { return _smokingAllowed; }
            set { _smokingAllowed = value; }
        }

        public PropertyType PropertyType
        {
            get { return _propertyType; }
            set { _propertyType = value; }
        }

        public string OwnerEmail
        {
            get { return _ownerEmail; }
            set { _ownerEmail = value; }
        }

        /// <summary>
        /// House Builder
        /// </summary>
        public class HouseBuilder
        {
            private Address _location;
            private long _price;
            private bool _forRent;
            private int _numberOfBedrooms;
            private int _numberOfKitchens;
            private int _numberOfBathrooms;
            private bool _familiesOnly;
            private bool _girlsOnly;
            private bool _internetAvailable;
            private bool _landlinePhoneAvailable;
            private bool _cableTvAvailable;
            private Size _size;
            private bool _garageAvailable;
            private bool _smokingAllowed;
            private PropertyType _propertyType;
            private string _ownerEmail;

            /// <summary>
            /// Location
            /// </summary>
            /// <param name="location"></param>
            /// <returns></returns>
            public HouseBuilder Location(Address location)
            {
                _location = location;
                return this;
            }

            /// <summary>
            /// Price
            /// </summary>
            /// <param name="price"></param>
            /// <returns></returns>
            public HouseBuilder Price(long price)
            {
                _price = price;
                return this;
            }

            /// <summary>
            /// is the property for rent or for sale
            /// </summary>
            /// <param name="forRent"></param>
            /// <returns></returns>
            public HouseBuilder ForRent(bool forRent)
            {
                _forRent = forRent;
                return this;
            }

            public HouseBuilder NumberOfBedrooms(int numberOfBedrooms)
            {
                _numberOfBedrooms = numberOfBedrooms;
                return this;
            }

            public HouseBuilder NumberOfKitchens(int numberOfKitchens)
            {
                _numberOfKitchens = numberOfKitchens;
                return this;
            }

            public HouseBuilder NumberOfBathrooms(int numberOfBathrooms)
            {
                _numberOfBathrooms = numberOfBathrooms;
                return this;
            }

            public HouseBuilder FamiliesOnly(bool familiesOnly)
            {
                _familiesOnly = familiesOnly;
                return this;
            }

            public HouseBuilder GirlsOnly(bool girlsOnly)
            {
                _girlsOnly = girlsOnly;
                return this;
            }

            public HouseBuilder WithInternetAvailable(bool internetAvailable)
            {
                _internetAvailable = internetAvailable;
                return this;
            }

            public HouseBuilder LandlinePhoneAvailable(bool landlinePhoneAvailable)
            {
                _landlinePhoneAvailable = landlinePhoneAvailable;
                return this;
            }

            public HouseBuilder CableTvAvailable(bool cableTvAvailable)
            {
                _cableTvAvailable = cableTvAvailable;
                return this;
            }

            public HouseBuilder Size(Size size)
            {
                _size = size;
                return this;
            }

            public HouseBuilder GarageAvailable(bool garageAvailable)
            {
                _garageAvailable = garageAvailable;
                return this;
            }

            public HouseBuilder SmokingAllowed(bool smokingAllowed)
            {
                _smokingAllowed = smokingAllowed;
                return this;
            }

            public HouseBuilder PropertyType(PropertyType propertyType)
            {
                _propertyType = propertyType;
                return this;
            }

            public HouseBuilder OwnerEmail(string ownerEmail)
            {
                _ownerEmail = ownerEmail;
                return this;
            }

            /// <summary>
            /// Build a new instance of House
            /// </summary>
            /// <returns></returns>
            public House Build()
            {
                // For now, we only deal with rented houses
                _forRent = true;
                return new House(_location, _price, _forRent, _numberOfBedrooms, _numberOfKitchens,
                                 _numberOfBathrooms, _familiesOnly, _girlsOnly, _internetAvailable,
                                 _landlinePhoneAvailable, _cableTvAvailable, _size, _garageAvailable,
                                 _smokingAllowed, _propertyType, _ownerEmail);
            }
        }
    }
}
