using System;
using System.Collections.Generic;
using RentStuff.Common.Domain.Model;

namespace RentStuff.Property.Domain.Model.HouseAggregate
{
    /// <summary>
    /// House Aggregate. A house that can be put on rent or sale
    /// </summary>
    public class House : Entity
    {
        private string _title;
        private string _description;
        private long _rentPrice;
        private int _numberOfBedrooms;
        private int _numberOfKitchens;
        private int _numberOfBathrooms;
        private bool _internetAvailable;
        private bool _landlinePhoneAvailable;
        private bool _cableTvAvailable;
        private Dimension _dimension;
        private bool _garageAvailable;
        private bool _smokingAllowed;
        private string _propertyType;
        private string _ownerEmail;
        private string _ownerPhoneNumber;
        private decimal _latitude;
        private decimal _longitude;
        private string _houseNo;
        private string _streetNo;
        private string _area;
        private IList<string> _houseImages = new List<string>();
        private string _ownerName;
        private string _rentUnit;
        
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
        {
            Title = title;
            RentPrice = rentPrice;
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
            OwnerEmail = ownerEmail;
            OwnerPhoneNumber = ownerPhoneNumber;
            Latitude = latitude;
            Longitude = longitude;
            HouseNo = houseNo;
            StreetNo = streetNo;
            Area = area;
            OwnerName = ownerName;
            Description = description;
            GenderRestriction = genderRestriction;
            IsShared = isShared;
            RentUnit = rentUnit;
            DateCreated = DateTime.Now;
            LastModified = DateTime.Now;
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
        public void UpdateHouse(string title, long rentPrice, int numberOfBedrooms,
            int numberOfKitchens, int numberOfBathrooms,
            bool internetAvailable, bool landlinePhoneAvailable, bool cableTvAvailable, Dimension dimension,
            bool garageAvailable, bool smokingAllowed, string propertyType, string ownerEmail, string ownerPhoneNumber,
            string houseNo, string streetNo, string area, string ownerName, string description, GenderRestriction genderRestriction,
            decimal latitude, decimal longitude, bool isShared, string rentUnit)
        {
            Title = title;
            RentPrice = rentPrice;
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
            OwnerEmail = ownerEmail;
            OwnerPhoneNumber = ownerPhoneNumber;
            HouseNo = houseNo;
            StreetNo = streetNo;
            Area = area;
            OwnerName = ownerName;
            Description = description;
            GenderRestriction = genderRestriction;
            Latitude = latitude;
            Longitude = longitude;
            IsShared = isShared;
            RentUnit = rentUnit;
            LastModified = DateTime.Now;
        }

        /// <summary>
        /// Add Image's ID to this House
        /// </summary>
        public void AddImage(string id)
        {
            _houseImages.Add(id);
        }

        /// <summary>
        /// Get the list of Images for this House
        /// </summary>
        /// <returns></returns>
        public IList<string> GetImageList()
        {
            return _houseImages;
        }

        /// <summary>
        /// Title of the house
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                Assertion.AssertStringNotNullorEmpty(value);
                _title = value;
            }
        }

        /// <summary>
        /// Description of the house
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Rent
        /// </summary>
        public long RentPrice
        {
            get { return _rentPrice; }
            set
            {
                Assertion.AssertNumberNotZero(value);
                _rentPrice = value;
            }
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
        /// The types of property we provide
        /// Not using enum so these values can be used directly on the frontend clients without any parsing
        /// Saving the values in-memory instead of saving in database as to reduce the overhead caused by DB 
        /// operations
        /// </summary>
        private static readonly IList<string> _allPropertyTypes = new List<string>()
        {
            "Hostel",
            "House",
            "Apartment",
            "Hotel",
            "Guest House"
        };
        
        /// <summary>
        /// Type of property
        /// </summary>
        public string PropertyType
        {
            get { return _propertyType; }
            set
            {
                //Assertion.AssertStringNotNullorEmpty(value);
                if (string.IsNullOrWhiteSpace(value))
                {
                    // If the value is null, just assign House.
                    value = _allPropertyTypes[1];
                }
                if (GetAllPropertyTypes().Contains(value))
                {
                    _propertyType = value;
                }
                else
                {
                    throw new InvalidOperationException("The provided Property Type is not supported");
                }
            }
        }

        /// <summary>
        /// Returns a lsit of all supported property types
        /// </summary>
        /// <returns></returns>
        public static IList<string> GetAllPropertyTypes()
        {
            return _allPropertyTypes;
        }

        /// <summary>
        /// Email of the owner of the posted submission
        /// </summary>
        public string OwnerEmail
        {
            get { return _ownerEmail; }
            set
            {
                Assertion.AssertStringNotNullorEmpty(value);
                Assertion.IsEmailValid(value);
                _ownerEmail = value;
            }
        }

        /// <summary>
        /// Phone number of the owner of the posted submission
        /// </summary>
        //[RegularExpression(@"^\d{1,11}$", ErrorMessage = "Invalid Phone Number")]
        public string OwnerPhoneNumber
        {
            get { return _ownerPhoneNumber; }
            set
            {
                Assertion.AssertStringNotNullorEmpty(value);
                Assertion.IsPhoneNumberValid(value);
                _ownerPhoneNumber = value;
            }
        }

        /// <summary>
        /// Latitude
        /// </summary>
        public decimal Latitude
        {
            get { return _latitude; }
            private set
            {
                Assertion.AssertDecimalNotZero(value);
                _latitude = value;
            }
        }

        /// <summary>
        /// Longitude
        /// </summary>
        public decimal Longitude
        {
            get { return _longitude; }
            private set
            {
                Assertion.AssertDecimalNotZero(value);
                _longitude = value;
            }
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
        /// Area that contains Town, State, City and Country
        /// </summary>
        public string Area
        {
            get { return _area; }
            set
            {
                Assertion.AssertStringNotNullorEmpty(value);
                _area = value;
            }
        }

        /// <summary>
        /// Images IDs for this house
        /// </summary>
        public IList<string> HouseImages
        {
            get { return _houseImages; }
            set { _houseImages = value; }
        }

        /// <summary>
        /// Name of the owner
        /// </summary>
        public string OwnerName
        {
            get { return _ownerName; }
            set
            {
                Assertion.AssertStringNotNullorEmpty(value);
                _ownerName = value;
            }
        }
        
        /// <summary>
        /// Is it for boys only, girls only or for families only
        /// </summary>
        public GenderRestriction GenderRestriction { get; set; }

        /// <summary>
        /// The types of rent units we provide
        /// Not using enum so these values can be used directly on the frontend clients without any parsing
        /// Saving the values in-memory instead of saving in database as to reduce the overhead caused by DB 
        /// operations
        /// </summary>
        private static readonly IList<string> _allRentUnits = new List<string>()
        {
            "Month",
            "Week",
            "Day",
            "Hour"
        };

        /// <summary>
        /// Is this property shared
        /// </summary>
        public bool IsShared { get; set; }

        /// <summary>
        /// Rent unit
        /// </summary>
        public string RentUnit
        {
            get { return _rentUnit; }
            set
            {
                //Assertion.AssertStringNotNullorEmpty(value);
                if (string.IsNullOrWhiteSpace(value))
                {
                    value = _allRentUnits[0];
                }
                if (!_allRentUnits.Contains(value))
                {
                    throw new InvalidOperationException("RentUnit value is not supported");
                }
                _rentUnit = value;
            }
        }

        /// <summary>
        /// Get all the rent units
        /// </summary>
        /// <returns></returns>
        public static IList<string> GetAllRentUnits()
        {
            return _allRentUnits;
        }

        /// <summary>
        /// The time this instance was created
        /// </summary>
        public DateTime? DateCreated { get; private set; }

        /// <summary>
        /// The time this instance was last modified
        /// </summary>
        public DateTime? LastModified { get; private set; }

        /// <summary>
        /// House Builder, using the builder pattern that allows easy refactoring of the references and usages of
        /// the initialization of the House entity
        /// </summary>
        public class HouseBuilder
        {
            private string _title;
            private string _description;
            private long _rentPrice;
            private int _numberOfBedrooms;
            private int _numberOfKitchens;
            private int _numberOfBathrooms;
            private bool _internetAvailable;
            private bool _landlinePhoneAvailable;
            private bool _cableTvAvailable;
            private Dimension _dimension;
            private bool _garageAvailable;
            private bool _smokingAllowed;
            private string _propertyType;
            private string _ownerEmail;
            private string _ownerPhoneNumber;
            private decimal _latitude;
            private decimal _longitude;
            private string _houseNo;
            private string _streetNo;
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
