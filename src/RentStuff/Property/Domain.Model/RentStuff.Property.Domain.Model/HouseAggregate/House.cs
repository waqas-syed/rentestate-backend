﻿using System;
using RentStuff.Common.Utilities;
using RentStuff.Property.Domain.Model.PropertyAggregate;

namespace RentStuff.Property.Domain.Model.HouseAggregate
{
    /// <summary>
    /// House Aggregate. A house that can be put on rent or sale
    /// </summary>
    public class House : ResidentialProperty
    {
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
            decimal latitude, decimal longitude, string houseNo, string streetNo, string area, string ownerName, 
            string description, GenderRestriction genderRestriction, bool isShared, string rentUnit, 
            string landlineNumber, string fax, bool ac,
                bool geyser,
                bool balcony, bool lawn, bool cctvCameras, bool backupElectricity, bool heating,
                bool bathtub, bool elevator) 
            // Initiate the parent Property class as well
            : base(title, rentPrice, ownerEmail,
                ownerPhoneNumber, latitude, longitude, area, ownerName, description, genderRestriction, 
                isShared, rentUnit, internetAvailable, cableTvAvailable, propertyType, landlineNumber, fax)
        {
            if (string.IsNullOrWhiteSpace(propertyType))
            {
                throw new NullReferenceException("PropertyType is required");
            }
            if (!propertyType.Equals(Constants.House) && !propertyType.Equals(Constants.Apartment) 
                && !propertyType.Equals(Constants.Hostel))
            {
                throw new InvalidOperationException("House/Apartment instance can only be created with type House/Apartment");
            }
            NumberOfBedrooms = numberOfBedrooms;
            NumberOfKitchens = numberOfKitchens;
            NumberOfBathrooms = numberOfBathrooms;
            LandlinePhoneAvailable = landlinePhoneAvailable;
            Dimension = dimension;
            SmokingAllowed = smokingAllowed;
            PropertyType = propertyType;
            HouseNo = houseNo;
            StreetNo = streetNo;
            GarageAvailable = garageAvailable;
            Bathtub = bathtub;
            AC = ac;
            Geyser = geyser;
            Balcony = balcony;
            Lawn = lawn;
            CctvCameras = cctvCameras;
            BackupElectricity = backupElectricity;
            Heating = heating;
            Elevator = elevator;
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
            decimal latitude, decimal longitude, bool isShared, string rentUnit, string landlineNumber, string fax,
            bool ac,
            bool geyser,
            bool balcony, bool lawn, bool cctvCameras, bool backupElectricity, bool heating,
            bool bathtub, bool elevator)
        {
            if (!propertyType.Equals("House") && !propertyType.Equals("Apartment"))
            {
                throw new InvalidOperationException("House/Apartment instance can only be created with type House/Apartment");
            }
            NumberOfBedrooms = numberOfBedrooms;
            NumberOfKitchens = numberOfKitchens;
            NumberOfBathrooms = numberOfBathrooms;
            LandlinePhoneAvailable = landlinePhoneAvailable;
            Dimension = dimension;
            SmokingAllowed = smokingAllowed;
            PropertyType = propertyType;
            HouseNo = houseNo;
            StreetNo = streetNo;
            GarageAvailable = garageAvailable;
            Bathtub = bathtub;
            AC = ac;
            Geyser = geyser;
            Balcony = balcony;
            Lawn = lawn;
            CctvCameras = cctvCameras;
            BackupElectricity = backupElectricity;
            Heating = heating;
            Elevator = elevator;
            // Update the parent property class
            base.Update(title, rentPrice, ownerEmail, ownerPhoneNumber, area, ownerName, description,
                genderRestriction, latitude, longitude, isShared, rentUnit, internetAvailable, cableTvAvailable,
                propertyType, landlineNumber, fax);
        }
        
        /// <summary>
        /// Number of bedrooms
        /// </summary>
        public int NumberOfBedrooms
        {
            get;
            private set;
        }

        /// <summary>
        /// Number of kitchens
        /// </summary>
        public int NumberOfKitchens
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Number of bathrooms
        /// </summary>
        public int NumberOfBathrooms
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Landline phone number
        /// </summary>
        public bool LandlinePhoneAvailable
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Dimension
        /// </summary>
        public Dimension Dimension
        {
            get;
            set;
        }
        
        /// <summary>
        /// Is Smoking allowed
        /// </summary>
        public bool SmokingAllowed
        {
            get;
            private set;
        }
        
        /// <summary>
        /// House Number
        /// </summary>
        public string HouseNo
        {
            get;
            private set;
        }

        /// <summary>
        /// Street Number
        /// </summary>
        public string StreetNo
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
        
        public bool Bathtub { get; private set; }
        
        /// <summary>
        /// Is AC available
        /// </summary>
        public bool AC { get; private set; }

        /// <summary>
        /// Is Geyser available
        /// </summary>
        public bool Geyser { get; private set; }
        
        /// <summary>
        /// Is Balcony available
        /// </summary>
        public bool Balcony { get; private set; }

        /// <summary>
        /// Is Elevator available in the building
        /// </summary>
        public bool Elevator { get; set; }

        /// <summary>
        /// Is Lawn available
        /// </summary>
        public bool Lawn { get; private set; }

        /// <summary>
        /// Are CCTV Cameras available
        /// </summary>
        public bool CctvCameras { get; private set; }

        /// <summary>
        /// Is backup electricity available
        /// </summary>
        public bool BackupElectricity { get; set; }

        /// <summary>
        /// Does the place have Heating facility
        /// </summary>
        public bool Heating { get; set; }

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
            private bool _cableTvAvailable;
            private bool _garageAvailable;
            private bool _internetAvailable;
            private string _landlineNumber;
            private string _fax;
            
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
            /// Landline Number
            /// </summary>
            /// <param name="landlineNumber"></param>
            /// <returns></returns>
            public HouseBuilder LandlineNumber(string landlineNumber)
            {
                _landlineNumber = landlineNumber;
                return this;
            }

            /// <summary>
            /// Fax
            /// </summary>
            /// <param name="fax"></param>
            /// <returns></returns>
            public HouseBuilder Fax(string fax)
            {
                _fax = fax;
                return this;
            }

            #endregion Generic Property attributes

            #region House Specific Properties 

            private int _numberOfBedrooms;
            private int _numberOfKitchens;
            private int _numberOfBathrooms;
            private Dimension _dimension;
            private bool _smokingAllowed;
            private bool _landlinePhoneAvailable;
            private string _houseNo;
            private string _streetNo;
            private bool _ac;
            private bool _geyser;
            private bool _balcony;
            private bool _lawn;
            private bool _cctvCameras;
            private bool _backupElectricity;
            private bool _heating;
            private bool _bathtub;
            private int _numberOfBeds;
            private bool _elevator;

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

            public HouseBuilder AC(bool ac)
            {
                _ac = ac;
                return this;
            }

            public HouseBuilder Geyser(bool geyser)
            {
                _geyser = geyser;
                return this;
            }

            public HouseBuilder Balcony(bool balcony)
            {
                _balcony = balcony;
                return this;
            }

            public HouseBuilder Lawn(bool lawn)
            {
                _lawn = lawn;
                return this;
            }

            public HouseBuilder CctvCameras(bool cctvCameras)
            {
                _cctvCameras = cctvCameras;
                return this;
            }

            public HouseBuilder BackupElectricity(bool backupElectricity)
            {
                _backupElectricity = backupElectricity;
                return this;
            }

            public HouseBuilder Heating(bool heating)
            {
                _heating = heating;
                return this;
            }

            public HouseBuilder Bathtub(bool bathtub)
            {
                _bathtub = bathtub;
                return this;
            }
            
            public HouseBuilder Elevator(bool elevator)
            {
                _elevator = elevator;
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
                                 _genderRestriction, _isShared, _rentUnit, _landlineNumber, _fax,
                                 _ac, _geyser, _balcony, _lawn, _cctvCameras, _backupElectricity,
                                 _heating, _bathtub, _elevator);
            }
        }
    }
}
