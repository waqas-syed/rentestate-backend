
using System;
using RentStuff.Common.Domain.Model;

namespace RentStuff.Property.Domain.Model.HouseAggregate
{
    /// <summary>
    /// House Aggregate. A house that can be put on rent or sale
    /// </summary>
    public class House : Entity
    {
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
        private Size _size;
        private bool _garageAvailable;
        private bool _smokingAllowed;
        private PropertyType _propertyType;
        private string _ownerEmail;
        private string _ownerPhoneNumber;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public House()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public House(Location location, long monthlyRent, int numberOfBedrooms,
            int numberOfKitchens, int numberOfBathrooms, bool familiesOnly, bool boysOnly, bool girlsOnly,
            bool internetAvailable, bool landlinePhoneAvailable, bool cableTvAvailable, Size size, 
            bool garageAvailable, bool smokingAllowed, PropertyType propertyType, string ownerEmail, string ownerPhoneNumber)
        {
            Location = location;
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
            Size = size;
            GarageAvailable = garageAvailable;
            SmokingAllowed = smokingAllowed;
            PropertyType = propertyType;
            OwnerEmail = ownerEmail;
            OwnerPhoneNumber = ownerPhoneNumber;
        }

        /// <summary>
        /// Location
        /// </summary>
        public Location Location
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

        public string OwnerPhoneNumber
        {
            get { return _ownerPhoneNumber; }
            set { _ownerPhoneNumber = value; }
        }

        /// <summary>
        /// House Builder
        /// </summary>
        public class HouseBuilder
        {
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
            private Size _size;
            private bool _garageAvailable;
            private bool _smokingAllowed;
            private PropertyType _propertyType;
            private string _ownerEmail;
            private string _ownerPhoneNumber;

            /// <summary>
            /// Location
            /// </summary>
            /// <param name="location"></param>
            /// <returns></returns>
            public HouseBuilder Location(Location location)
            {
                _location = location;
                return this;
            }

            /// <summary>
            /// Price
            /// </summary>
            /// <param name="monthlyRent"></param>
            /// <returns></returns>
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

            public HouseBuilder OwnerPhoneNumber(string ownerPhoneNumber)
            {
                _ownerPhoneNumber = ownerPhoneNumber;
                return this;
            }

            /// <summary>
            /// Build a new instance of House
            /// </summary>
            /// <returns></returns>
            public House Build()
            {
                return new House(_location, _monthlyRent, _numberOfBedrooms, _numberOfKitchens,
                                 _numberOfBathrooms, _familiesOnly, _boysOnly, _girlsOnly, _internetAvailable,
                                 _landlinePhoneAvailable, _cableTvAvailable, _size, _garageAvailable,
                                 _smokingAllowed, _propertyType, _ownerEmail, _ownerPhoneNumber);
            }
        }
    }
}
