using RentStuff.Common.Domain.Model;
using System;
using System.Collections.Generic;

namespace RentStuff.Property.Domain.Model.PropertyAggregate
{
    /// <summary>
    /// The base class for all the property types.
    /// </summary>
    public class Property : Entity
    {
        private string _title;
        private string _description;
        private long _rentPrice;
        private string _ownerEmail;
        private string _ownerPhoneNumber;
        private decimal _latitude;
        private decimal _longitude;
        private string _area;
        private string _ownerName;
        private string _rentUnit;
        private string _propertyType;
        private string _landlineNumber;
        private string _fax;
        private IList<string> _images = new List<string>();

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
        /// Default Constructor
        /// </summary>
        public Property()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Property(string title, long rentPrice, string ownerEmail, string ownerPhoneNumber,
            decimal latitude, decimal longitude, string area, string ownerName, string description,
            GenderRestriction genderRestriction, bool isShared, string rentUnit, bool internetAvailable, 
            bool cableTvAvailable, bool garageAvailable, string propertyType, string landlineNumber, string fax)
        {
            if (string.IsNullOrWhiteSpace(ownerPhoneNumber) && string.IsNullOrWhiteSpace(landlineNumber))
            {
                throw new InvalidOperationException("Atleast one of Mobile or Landline number is required");
            }
            Title = title;
            RentPrice = rentPrice;
            OwnerEmail = ownerEmail.ToLower();
            OwnerPhoneNumber = ownerPhoneNumber;
            LandlineNumber = landlineNumber;
            Fax = fax;
            Latitude = latitude;
            Longitude = longitude;
            Area = area;
            OwnerName = ownerName;
            Description = description;
            GenderRestriction = genderRestriction;
            IsShared = isShared;
            RentUnit = rentUnit;
            InternetAvailable = internetAvailable;
            CableTvAvailable = cableTvAvailable;
            GarageAvailable = garageAvailable;
            PropertyType = propertyType;
            DateCreated = DateTime.Now;
            LastModified = DateTime.Now;
        }

        /// <summary>
        /// Update the property with the new values
        /// </summary>
        /// <param name="title"></param>
        /// <param name="rentPrice"></param>
        /// <param name="ownerEmail"></param>
        /// <param name="ownerPhoneNumber"></param>
        /// <param name="area"></param>
        /// <param name="ownerName"></param>
        /// <param name="description"></param>
        /// <param name="genderRestriction"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="isShared"></param>
        /// <param name="rentUnit"></param>
        /// <param name="internetAvailable"></param>
        /// <param name="cableTvAvailable"></param>
        /// <param name="garageAvailable"></param>
        /// <param name="propertyType"></param>
        /// <param name="landlineNumber"></param>
        /// <param name="fax"></param>
        public void Update(string title, long rentPrice, string ownerEmail, string ownerPhoneNumber,
            string area, string ownerName, string description, GenderRestriction genderRestriction,
            decimal latitude, decimal longitude, bool isShared, string rentUnit, bool internetAvailable,
            bool cableTvAvailable, bool garageAvailable, string propertyType, string landlineNumber, string fax)
        {
            if (string.IsNullOrWhiteSpace(ownerPhoneNumber) && string.IsNullOrWhiteSpace(landlineNumber))
            {
                throw new InvalidOperationException("Atleast one of Mobile or Landline number is required");
            }
            Title = title;
            RentPrice = rentPrice;
            OwnerEmail = ownerEmail.ToLower();
            OwnerPhoneNumber = ownerPhoneNumber;
            LandlineNumber = landlineNumber;
            Fax = fax;
            Latitude = latitude;
            Longitude = longitude;
            Area = area;
            OwnerName = ownerName;
            Description = description;
            GenderRestriction = genderRestriction;
            IsShared = isShared;
            RentUnit = rentUnit;
            InternetAvailable = internetAvailable;
            CableTvAvailable = cableTvAvailable;
            GarageAvailable = garageAvailable;
            PropertyType = propertyType;
            LastModified = DateTime.Now;
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
        /// Add Image's ID to this Property
        /// </summary>
        public void AddImage(string id)
        {
            _images.Add(id);
        }

        /// <summary>
        /// Get the list of Images for this Property
        /// </summary>
        /// <returns></returns>
        public IList<string> GetImageList()
        {
            return _images;
        }

        /// <summary>
        /// Images IDs for this Property
        /// </summary>
        public IList<string> Images
        {
            get { return _images; }
            private set { _images = value; }
        }

        /// <summary>
        /// Title of the house
        /// </summary>
        public string Title
        {
            get { return _title; }
            private set
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
            private set { _description = value; }
        }

        /// <summary>
        /// Rent
        /// </summary>
        public long RentPrice
        {
            get { return _rentPrice; }
            private set
            {
                Assertion.AssertNumberNotZero(value);
                _rentPrice = value;
            }
        }

        /// <summary>
        /// Email of the owner of the posted submission
        /// </summary>
        public string OwnerEmail
        {
            get { return _ownerEmail; }
            private set
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
            private set
            {
                _ownerPhoneNumber = value;
            }
        }

        /// <summary>
        /// Landline number that can be reached
        /// </summary>
        public string LandlineNumber
        {
            get { return _landlineNumber; }
            private set
            {
                _landlineNumber = value;
            }
        }

        /// <summary>
        /// Fax
        /// </summary>
        public string Fax
        {
            get { return _fax; }
            private set
            {
                _fax = value;
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
        /// Area that contains Town, State, City and Country
        /// </summary>
        public string Area
        {
            get { return _area; }
            private set
            {
                Assertion.AssertStringNotNullorEmpty(value);
                _area = value;
            }
        }
        
        /// <summary>
        /// Name of the owner
        /// </summary>
        public string OwnerName
        {
            get { return _ownerName; }
            private set
            {
                Assertion.AssertStringNotNullorEmpty(value);
                _ownerName = value;
            }
        }
        
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
        /// Rent unit
        /// </summary>
        public string RentUnit
        {
            get { return _rentUnit; }
            private set
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
        /// Is it for boys only, girls only or for families only
        /// </summary>
        public GenderRestriction GenderRestriction { get; private set; }

        /// <summary>
        /// Is this property shared
        /// </summary>
        public bool IsShared { get; private set; }

        /// <summary>
        /// Internet available
        /// </summary>
        public bool InternetAvailable
        {
            get;
            private set;
        }

        /// <summary>
        /// Cable TV available
        /// </summary>
        public bool CableTvAvailable
        {
            get;
            private set;
        }

        /// <summary>
        /// Garage available
        /// </summary>
        public bool GarageAvailable
        {
            get;
            private set;
        }

        /// <summary>
        /// The time this instance was created
        /// </summary>
        public DateTime? DateCreated { get; private set; }

        /// <summary>
        /// The time this instance was last modified
        /// </summary>
        public DateTime? LastModified { get; private set; }
    }
}
