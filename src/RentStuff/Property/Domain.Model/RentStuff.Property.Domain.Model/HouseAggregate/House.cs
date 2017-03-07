using System;
using System.Collections;
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
        private long _monthlyRent;
        private int _numberOfBedrooms;
        private int _numberOfKitchens;
        private int _numberOfBathrooms;
        private bool _familiesOnly;
        private bool _boysOnly;
        private bool _girlsOnly;
        private bool _internetAvailable;
        private bool _landlinePhoneAvailable;
        private bool _cableTvAvailable;
        private Dimension _dimension;
        private bool _garageAvailable;
        private bool _smokingAllowed;
        private PropertyType _propertyType;
        private string _ownerEmail;
        private string _ownerPhoneNumber;
        private decimal _latitude;
        private decimal _longitude;
        private string _houseNo;
        private string _streetNo;
        private string _area;
        private IList<string> _houseImages = new List<string>();
        private string _ownerName;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public House()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public House(string title, long monthlyRent, int numberOfBedrooms,
            int numberOfKitchens, int numberOfBathrooms, bool familiesOnly, bool boysOnly, bool girlsOnly,
            bool internetAvailable, bool landlinePhoneAvailable, bool cableTvAvailable, Dimension dimension, 
            bool garageAvailable, bool smokingAllowed, PropertyType propertyType, string ownerEmail, string ownerPhoneNumber,
            decimal latitude, decimal longitude, string houseNo, string streetNo, string area, string ownerName, string description)
        {
            Title = title;
            MonthlyRent = monthlyRent;
            NumberOfBedrooms = numberOfBedrooms;
            NumberOfKitchens = numberOfKitchens;
            NumberOfBathrooms = numberOfBathrooms;
            FamiliesOnly = familiesOnly;
            BoysOnly = boysOnly;
            GirlsOnly = girlsOnly;
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
        }

        /// <summary>
        /// House instance updates itself with the given values
        /// </summary>
        /// <param name="monthlyRent"></param>
        /// <param name="numberOfBedrooms"></param>
        /// <param name="numberOfKitchens"></param>
        /// <param name="numberOfBathrooms"></param>
        /// <param name="familiesOnly"></param>
        /// <param name="boysOnly"></param>
        /// <param name="girlsOnly"></param>
        /// <param name="internetAvailable"></param>
        /// <param name="landlinePhoneAvailable"></param>
        /// <param name="cableTvAvailable"></param>
        /// <param name="size"></param>
        /// <param name="garageAvailable"></param>
        /// <param name="smokingAllowed"></param>
        /// <param name="propertyType"></param>
        /// <param name="ownerEmail"></param>
        /// <param name="ownerPhoneNumber"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="houseNo"></param>
        /// <param name="streetNo"></param>
        /// <param name="area"></param>
        public void UpdateHouse(long monthlyRent, int numberOfBedrooms,
            int numberOfKitchens, int numberOfBathrooms, bool familiesOnly, bool boysOnly, bool girlsOnly,
            bool internetAvailable, bool landlinePhoneAvailable, bool cableTvAvailable, Dimension size,
            bool garageAvailable, bool smokingAllowed, string propertyType, string ownerEmail, string ownerPhoneNumber,
            decimal latitude, decimal longitude, string houseNo, string streetNo, string area)
        {
            MonthlyRent = monthlyRent;
            NumberOfBedrooms = numberOfBedrooms;
            NumberOfKitchens = numberOfKitchens;
            NumberOfBathrooms = numberOfBathrooms;
            FamiliesOnly = familiesOnly;
            BoysOnly = boysOnly;
            GirlsOnly = girlsOnly;
            InternetAvailable = internetAvailable;
            LandlinePhoneAvailable = landlinePhoneAvailable;
            CableTvAvailable = cableTvAvailable;
            GarageAvailable = garageAvailable;
            SmokingAllowed = smokingAllowed;
            PropertyType = (PropertyType)Enum.Parse(typeof(PropertyType), propertyType);
            OwnerEmail = ownerEmail;
            OwnerPhoneNumber = ownerPhoneNumber;
            HouseNo = houseNo;
            StreetNo = streetNo;
            Area = area;
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

        public string Title
        {
            get { return _title; }
            set
            {
                Assertion.AssertStringNotNullorEmpty(value);
                _title = value;
            }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Monthly Rent
        /// </summary>
        public long MonthlyRent
        {
            get { return _monthlyRent; }
            set
            {
                Assertion.AssertNumberNotZero(value);
                _monthlyRent = value;
            }
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
            set
            {
                if (value)
                {
                    if (!_boysOnly & !_girlsOnly)
                    {
                        _familiesOnly = true;
                    }
                    else
                    {
                        throw new ArgumentException(
                            "Cannot mark FamiliesOnly when either BoysOnly or GirlsOnly is marked true already");
                    }
                }
                else
                {
                    _familiesOnly = false;
                }
            }
        }

        public int NumberOfBathrooms
        {
            get { return _numberOfBathrooms; }
            set { _numberOfBathrooms = value; }
        }

        public bool GirlsOnly
        {
            get { return _girlsOnly; }
            set
            {
                if (value)
                {
                    if (!_boysOnly & !_familiesOnly)
                    {
                        _girlsOnly = true;
                    }
                    else
                    {
                        throw new ArgumentException(
                            "Cannot mark GirlsOnly when either BoysOnly or FamiliesOnly is marked true already");
                    }
                }
                else
                {
                    _girlsOnly = false;
                }
            }
        }

        public bool BoysOnly
        {
            get { return _boysOnly; }
            set
            {
                if (value)
                {
                    if(!_girlsOnly & !_familiesOnly)
                    {
                        _boysOnly = true;
                    }
                else
                    {
                        throw new ArgumentException(
                            "Cannot mark BoysOnly when either GirlsOnly or FamiliesOnly is marked true already");
                    }
                }
                else
                {
                    _boysOnly = false;
                }
            }
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

        public Dimension Dimension
        {
            get { return _dimension; } 
            set { _dimension = value; }
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
            set
            {
                _propertyType = value;
            }
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
                _ownerEmail = value;
            }
        }

        /// <summary>
        /// Phone number of the owner of the posted submission
        /// </summary>
        public string OwnerPhoneNumber
        {
            get { return _ownerPhoneNumber; }
            set
            {
                Assertion.AssertStringNotNullorEmpty(value);
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
                Assertion.AssertStringNotNullorEmpty(value);
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
                Assertion.AssertStringNotNullorEmpty(value);
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
        /// House Builder
        /// </summary>
        public class HouseBuilder
        {
            private string _title;
            private string _description;
            private Location _location;
            private long _monthlyRent;
            private int _numberOfBedrooms;
            private int _numberOfKitchens;
            private int _numberOfBathrooms;
            private bool _familiesOnly;
            private bool _boysOnly;
            private bool _girlsOnly;
            private bool _internetAvailable;
            private bool _landlinePhoneAvailable;
            private bool _cableTvAvailable;
            private Dimension _dimension;
            private bool _garageAvailable;
            private bool _smokingAllowed;
            private PropertyType _propertyType;
            private string _ownerEmail;
            private string _ownerPhoneNumber;
            private decimal _latitude;
            private decimal _longitude;
            private string _houseNo;
            private string _streetNo;
            private string _area;
            private string _ownerName;

            public HouseBuilder Title(string title)
            {
                _title = title;
                return this;
            }

            public HouseBuilder Description(string description)
            {
                _description = description;
                return this;
            }

            public HouseBuilder MonthlyRent(long monthlyRent)
            {
                _monthlyRent = monthlyRent;
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

            public HouseBuilder BoysOnly(bool boysOnly)
            {
                _boysOnly = boysOnly;
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

            public HouseBuilder Dimension(Dimension dimension)
            {
                _dimension = dimension;
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

            public HouseBuilder OwnerPhoneNumber(string ownerPhoneNumber)
            {
                _ownerPhoneNumber = ownerPhoneNumber;
                return this;
            }

            public HouseBuilder Latitude(decimal latitude)
            {
                _latitude = latitude;
                return this;
            }

            public HouseBuilder Longitude(decimal longitude)
            {
                _longitude = longitude;
                return this;
            }

            public HouseBuilder HouseNo(string houseNo)
            {
                _houseNo = houseNo;
                return this;
            }

            public HouseBuilder StreetNo(string streetNo)
            {
                _streetNo = streetNo;
                return this;
            }

            public HouseBuilder Area(string area)
            {
                _area = area;
                return this;
            }

            public HouseBuilder OwnerName(string ownerName)
            {
                _ownerName = ownerName;
                return this;
            }

            /// <summary>
            /// Build a new instance of House
            /// </summary>
            /// <returns></returns>
            public House Build()
            {
                return new House(_title, _monthlyRent, _numberOfBedrooms, _numberOfKitchens,
                                 _numberOfBathrooms, _familiesOnly, _boysOnly, _girlsOnly, _internetAvailable,
                                 _landlinePhoneAvailable, _cableTvAvailable, _dimension, _garageAvailable,
                                 _smokingAllowed, _propertyType, _ownerEmail, _ownerPhoneNumber, _latitude, _longitude,
                                 _houseNo, _streetNo, _area, _ownerName, _description);
            }
        }
    }
}
