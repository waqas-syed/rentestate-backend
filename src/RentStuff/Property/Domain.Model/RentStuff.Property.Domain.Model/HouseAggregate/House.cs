using RentStuff.Property.Domain.Model.PropertyAggregate;

namespace RentStuff.Property.Domain.Model.HouseAggregate
{
    /// <summary>
    /// House Aggregate. A house that can be put on rent or sale
    /// </summary>
    public class House : PropertyAggregate.Property
    {
        private int _numberOfBedrooms;
        private int _numberOfKitchens;
        private int _numberOfBathrooms;
        private bool _internetAvailable;
        private bool _landlinePhoneAvailable;
        private bool _cableTvAvailable;
        private Dimension _dimension;
        private bool _garageAvailable;
        private bool _smokingAllowed;
        private string _houseNo;
        private string _streetNo;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public House()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public House(string title, long rentPrice, int numberOfBedrooms,
            int numberOfKitchens, int numberOfBathrooms,
            bool internetAvailable, bool landlinePhoneAvailable, bool cableTvAvailable, Dimension dimension, 
            bool garageAvailable, bool smokingAllowed, string propertyType, string ownerEmail, string ownerPhoneNumber,
            decimal latitude, decimal longitude, string houseNo, string streetNo, string area, string ownerName, string description, 
            GenderRestriction genderRestriction, bool isShared, string rentUnit) 
            // Initiate the parent Property class as well
            : base(title, rentPrice, ownerEmail,
                ownerPhoneNumber, latitude, longitude, area, ownerName, description, genderRestriction, isShared,
                rentUnit)
        {
            NumberOfBedrooms = numberOfBedrooms;
            NumberOfKitchens = numberOfKitchens;
            NumberOfBathrooms = numberOfBathrooms;
            InternetAvailable = internetAvailable;
            LandlinePhoneAvailable = landlinePhoneAvailable;
            CableTvAvailable = cableTvAvailable;
            Dimension = dimension;
            GarageAvailable = garageAvailable;
            SmokingAllowed = smokingAllowed;
            PropertyType = propertyType;
            HouseNo = houseNo;
            StreetNo = streetNo;
        }

        /// <summary>
        /// House instance updates itself with the given values
        /// </summary>
        /// <param name="title"></param>
        /// <param name="rentPrice"></param>
        /// <param name="numberOfBedrooms"></param>
        /// <param name="numberOfKitchens"></param>
        /// <param name="numberOfBathrooms"></param>
        /// <param name="internetAvailable"></param>
        /// <param name="landlinePhoneAvailable"></param>
        /// <param name="cableTvAvailable"></param>
        /// <param name="garageAvailable"></param>
        /// <param name="smokingAllowed"></param>
        /// <param name="propertyType"></param>
        /// <param name="ownerEmail"></param>
        /// <param name="ownerPhoneNumber"></param>
        /// <param name="houseNo"></param>
        /// <param name="streetNo"></param>
        /// <param name="area"></param>
        /// <param name="ownerName"></param>
        /// <param name="description"></param>
        /// <param name="dimension"></param>
        /// <param name="genderRestriction"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="isShared"></param>
        /// <param name="rentUnit"></param>
        public void UpdateHouse(string title, long rentPrice, int numberOfBedrooms,
            int numberOfKitchens, int numberOfBathrooms,
            bool internetAvailable, bool landlinePhoneAvailable, bool cableTvAvailable, Dimension dimension,
            bool garageAvailable, bool smokingAllowed, string propertyType, string ownerEmail, string ownerPhoneNumber,
            string houseNo, string streetNo, string area, string ownerName, string description, GenderRestriction genderRestriction,
            decimal latitude, decimal longitude, bool isShared, string rentUnit)
        {
            NumberOfBedrooms = numberOfBedrooms;
            NumberOfKitchens = numberOfKitchens;
            NumberOfBathrooms = numberOfBathrooms;
            InternetAvailable = internetAvailable;
            LandlinePhoneAvailable = landlinePhoneAvailable;
            CableTvAvailable = cableTvAvailable;
            Dimension = dimension;
            GarageAvailable = garageAvailable;
            SmokingAllowed = smokingAllowed;
            PropertyType = propertyType;
            HouseNo = houseNo;
            StreetNo = streetNo;
            // Update the parent property class
            base.UpdateHouse(title, rentPrice, ownerEmail.ToLower(), ownerPhoneNumber, area, ownerName, description,
                genderRestriction, latitude, longitude, isShared, rentUnit);
        }
        
        /// <summary>
        /// Number of bedrooms
        /// </summary>
        public int NumberOfBedrooms
        {
            get { return _numberOfBedrooms; }
            set { _numberOfBedrooms = value; }
        }

        /// <summary>
        /// Number of kitchens
        /// </summary>
        public int NumberOfKitchens
        {
            get { return _numberOfKitchens; }
            set { _numberOfKitchens = value; }
        }
        
        /// <summary>
        /// Number of bathrooms
        /// </summary>
        public int NumberOfBathrooms
        {
            get { return _numberOfBathrooms; }
            set { _numberOfBathrooms = value; }
        }
        
        /// <summary>
        /// Internet available
        /// </summary>
        public bool InternetAvailable
        {
            get { return _internetAvailable; }
            set { _internetAvailable = value; }
        }

        /// <summary>
        /// Landline phone number
        /// </summary>
        public bool LandlinePhoneAvailable
        {
            get { return _landlinePhoneAvailable; }
            set { _landlinePhoneAvailable = value; }
        }

        /// <summary>
        /// Cable TV available
        /// </summary>
        public bool CableTvAvailable
        {
            get { return _cableTvAvailable; }
            set { _cableTvAvailable = value; }
        }

        /// <summary>
        /// Dimension
        /// </summary>
        public Dimension Dimension
        {
            get { return _dimension; } 
            set { _dimension = value; }
        }

        /// <summary>
        /// Garage available
        /// </summary>
        public bool GarageAvailable
        {
            get { return _garageAvailable; }
            set { _garageAvailable = value; }
        }

        /// <summary>
        /// Is Smoking allowed
        /// </summary>
        public bool SmokingAllowed
        {
            get { return _smokingAllowed; }
            set { _smokingAllowed = value; }
        }
        
        /// <summary>
        /// House Number
        /// </summary>
        public string HouseNo
        {
            get { return _houseNo; }
            set
            {
                //Assertion.AssertStringNotNullorEmpty(value);
                _houseNo = value;
            }
        }

        /// <summary>
        /// Street Number
        /// </summary>
        public string StreetNo
        {
            get { return _streetNo; }
            set
            {
                //Assertion.AssertStringNotNullorEmpty(value);
                _streetNo = value;
            }
        }
        
        /// <summary>
        /// House Builder, using the builder pattern that allows easy refactoring of the references and usages of
        /// the initialization of the House entity
        /// </summary>
        public class HouseBuilder
        {
            #region Generic Property attributes

            private string _title;
            private string _description;
            private long _rentPrice;
            private string _propertyType;
            private string _ownerEmail;
            private string _ownerPhoneNumber;
            private decimal _latitude;
            private decimal _longitude;
            private string _area;
            private string _ownerName;
            private GenderRestriction _genderRestriction;
            private bool _isShared;
            private string _rentUnit;

            /// <summary>
            /// Title
            /// </summary>
            /// <param name="title"></param>
            /// <returns></returns>
            public HouseBuilder Title(string title)
            {
                _title = title;
                return this;
            }

            /// <summary>
            /// Description
            /// </summary>
            /// <param name="description"></param>
            /// <returns></returns>
            public HouseBuilder Description(string description)
            {
                _description = description;
                return this;
            }

            /// <summary>
            /// Rent price
            /// </summary>
            /// <param name="rentPrice"></param>
            /// <returns></returns>
            public HouseBuilder RentPrice(long rentPrice)
            {
                _rentPrice = rentPrice;
                return this;
            }

            /// <summary>
            /// Property type
            /// </summary>
            /// <param name="propertyType"></param>
            /// <returns></returns>
            public HouseBuilder PropertyType(string propertyType)
            {
                _propertyType = propertyType;
                return this;
            }

            /// <summary>
            /// Owner email
            /// </summary>
            /// <param name="ownerEmail"></param>
            /// <returns></returns>
            public HouseBuilder OwnerEmail(string ownerEmail)
            {
                _ownerEmail = ownerEmail;
                return this;
            }

            /// <summary>
            /// Owner phone number
            /// </summary>
            /// <param name="ownerPhoneNumber"></param>
            /// <returns></returns>
            public HouseBuilder OwnerPhoneNumber(string ownerPhoneNumber)
            {
                _ownerPhoneNumber = ownerPhoneNumber;
                return this;
            }

            /// <summary>
            /// Latitude
            /// </summary>
            /// <param name="latitude"></param>
            /// <returns></returns>
            public HouseBuilder Latitude(decimal latitude)
            {
                _latitude = latitude;
                return this;
            }

            /// <summary>
            /// Longitude
            /// </summary>
            /// <param name="longitude"></param>
            /// <returns></returns>
            public HouseBuilder Longitude(decimal longitude)
            {
                _longitude = longitude;
                return this;
            }

            /// <summary>
            /// Area
            /// </summary>
            /// <param name="area"></param>
            /// <returns></returns>
            public HouseBuilder Area(string area)
            {
                _area = area;
                return this;
            }

            /// <summary>
            /// Owner name
            /// </summary>
            /// <param name="ownerName"></param>
            /// <returns></returns>
            public HouseBuilder OwnerName(string ownerName)
            {
                _ownerName = ownerName;
                return this;
            }

            /// <summary>
            /// Gender restrictions
            /// </summary>
            /// <param name="genderRestriction"></param>
            /// <returns></returns>
            public HouseBuilder GenderRestriction(GenderRestriction genderRestriction)
            {
                _genderRestriction = genderRestriction;
                return this;
            }

            /// <summary>
            /// Is this property shared
            /// </summary>
            /// <param name="isShared"></param>
            /// <returns></returns>
            public HouseBuilder IsShared(bool isShared)
            {
                _isShared = isShared;
                return this;
            }

            /// <summary>
            /// What is the unit of rent payment
            /// </summary>
            /// <param name="rentUnit"></param>
            /// <returns></returns>
            public HouseBuilder RentUnit(string rentUnit)
            {
                _rentUnit = rentUnit;
                return this;
            }

            #endregion Generic Property attributes

            #region House Specific Properties 

            private int _numberOfBedrooms;
            private int _numberOfKitchens;
            private int _numberOfBathrooms;
            private bool _internetAvailable;
            private bool _landlinePhoneAvailable;
            private bool _cableTvAvailable;
            private Dimension _dimension;
            private bool _garageAvailable;
            private bool _smokingAllowed;
            private string _houseNo;
            private string _streetNo;

            /// <summary>
            /// Number of Bedrooms
            /// </summary>
            /// <param name="numberOfBedrooms"></param>
            /// <returns></returns>
            public HouseBuilder NumberOfBedrooms(int numberOfBedrooms)
            {
                _numberOfBedrooms = numberOfBedrooms;
                return this;
            }

            /// <summary>
            /// Number of kitchens
            /// </summary>
            /// <param name="numberOfKitchens"></param>
            /// <returns></returns>
            public HouseBuilder NumberOfKitchens(int numberOfKitchens)
            {
                _numberOfKitchens = numberOfKitchens;
                return this;
            }

            /// <summary>
            /// Number of bathrooms
            /// </summary>
            /// <param name="numberOfBathrooms"></param>
            /// <returns></returns>
            public HouseBuilder NumberOfBathrooms(int numberOfBathrooms)
            {
                _numberOfBathrooms = numberOfBathrooms;
                return this;
            }
            
            /// <summary>
            /// Is internet available
            /// </summary>
            /// <param name="internetAvailable"></param>
            /// <returns></returns>
            public HouseBuilder WithInternetAvailable(bool internetAvailable)
            {
                _internetAvailable = internetAvailable;
                return this;
            }

            /// <summary>
            /// Is landline phone available
            /// </summary>
            /// <param name="landlinePhoneAvailable"></param>
            /// <returns></returns>
            public HouseBuilder LandlinePhoneAvailable(bool landlinePhoneAvailable)
            {
                _landlinePhoneAvailable = landlinePhoneAvailable;
                return this;
            }

            /// <summary>
            /// is cable tv available
            /// </summary>
            /// <param name="cableTvAvailable"></param>
            /// <returns></returns>
            public HouseBuilder CableTvAvailable(bool cableTvAvailable)
            {
                _cableTvAvailable = cableTvAvailable;
                return this;
            }

            /// <summary>
            /// Dimension
            /// </summary>
            /// <param name="dimension"></param>
            /// <returns></returns>
            public HouseBuilder Dimension(Dimension dimension)
            {
                _dimension = dimension;
                return this;
            }

            /// <summary>
            /// Is garage available
            /// </summary>
            /// <param name="garageAvailable"></param>
            /// <returns></returns>
            public HouseBuilder GarageAvailable(bool garageAvailable)
            {
                _garageAvailable = garageAvailable;
                return this;
            }

            /// <summary>
            /// Is smoking available
            /// </summary>
            /// <param name="smokingAllowed"></param>
            /// <returns></returns>
            public HouseBuilder SmokingAllowed(bool smokingAllowed)
            {
                _smokingAllowed = smokingAllowed;
                return this;
            }
            
            /// <summary>
            /// House No
            /// </summary>
            /// <param name="houseNo"></param>
            /// <returns></returns>
            public HouseBuilder HouseNo(string houseNo)
            {
                _houseNo = houseNo;
                return this;
            }

            /// <summary>
            /// Street No
            /// </summary>
            /// <param name="streetNo"></param>
            /// <returns></returns>
            public HouseBuilder StreetNo(string streetNo)
            {
                _streetNo = streetNo;
                return this;
            }

            #endregion House Specific Properties 

            /// <summary>
            /// Build a new instance of House
            /// </summary>
            /// <returns></returns>
            public House Build()
            {
                return new House(_title, _rentPrice, _numberOfBedrooms, _numberOfKitchens,
                                 _numberOfBathrooms, _internetAvailable,
                                 _landlinePhoneAvailable, _cableTvAvailable, _dimension, _garageAvailable,
                                 _smokingAllowed, _propertyType, _ownerEmail, _ownerPhoneNumber, _latitude, 
                                 _longitude, _houseNo, _streetNo, _area, _ownerName, _description, 
                                 _genderRestriction, _isShared, _rentUnit);
            }
        }
    }
}
