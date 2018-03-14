using RentStuff.Property.Domain.Model.PropertyAggregate;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RentStuff.Common.Utilities;

namespace RentStuff.Property.Domain.Model.HotelAggregate
{
    /// <summary>
    /// Hotel or Guest House
    /// </summary>
    public class Hotel : GuestPropertyAbstraction
    {
        /// <summary>
        /// Defaut Constructor  required by NHibernate
        /// </summary>
        public Hotel()
        {
            
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="title"></param>
        /// <param name="rentPrice"></param>
        /// <param name="ownerEmail"></param>
        /// <param name="ownerPhoneNumber"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="area"></param>
        /// <param name="ownerName"></param>
        /// <param name="description"></param>
        /// <param name="genderRestriction"></param>
        /// <param name="isShared"></param>
        /// <param name="rentUnit"></param>
        /// <param name="internetAvailable"></param>
        /// <param name="cableTvAvailable"></param>
        /// <param name="parkingAvailable"></param>
        /// <param name="propertyType"></param>
        /// <param name="laundry"></param>
        /// <param name="ac"></param>
        /// <param name="geyser"></param>
        /// <param name="fitnessCentre"></param>
        /// <param name="attachedBathroom"></param>
        /// <param name="ironing"></param>
        /// <param name="balcony"></param>
        /// <param name="lawn"></param>
        /// <param name="cctvCameras"></param>
        /// <param name="backupElectricity"></param>
        /// <param name="heating"></param>
        /// <param name="restaurant"></param>
        /// <param name="airportShuttle"></param>
        /// <param name="breakfastIncluded"></param>
        /// <param name="sittingArea"></param>
        /// <param name="carRental"></param>
        /// <param name="spa"></param>
        /// <param name="salon"></param>
        /// <param name="bathtub"></param>
        /// <param name="swimmingPool"></param>
        /// <param name="kitchen"></param>
        /// <param name="beds"></param>
        /// <param name="occupants"></param>
        /// <param name="landlineNumber"></param>
        /// <param name="fax"></param>
        /// <param name="elevator"></param>
        public Hotel(string title, long rentPrice, string ownerEmail,
                string ownerPhoneNumber,
                decimal latitude, decimal longitude, string area, string ownerName,
                string description,
                GenderRestriction genderRestriction, bool isShared, string rentUnit, bool internetAvailable,
                bool cableTvAvailable, bool parkingAvailable, string propertyType, bool laundry, bool ac,
                bool geyser, bool fitnessCentre, bool attachedBathroom, bool ironing,
                bool balcony, bool lawn, bool cctvCameras, bool backupElectricity, bool heating,
                bool restaurant, bool airportShuttle, bool breakfastIncluded, bool sittingArea, bool carRental,
                bool spa, bool salon, bool bathtub, bool swimmingPool, bool kitchen, IList<Bed> beds,
                Occupants occupants, string landlineNumber, string fax, bool elevator)
            // Initiate the parent GuerstPropertyAbstraction class as well
            : base(title, rentPrice, ownerEmail,
                ownerPhoneNumber, latitude, longitude, area, ownerName, description, genderRestriction, isShared,
                rentUnit, internetAvailable, cableTvAvailable, parkingAvailable, propertyType, laundry, ac, geyser,
                fitnessCentre,
                attachedBathroom, ironing, balcony, lawn, cctvCameras, backupElectricity, heating, landlineNumber, 
                fax, elevator)
        {
            // Property Type must be Hotel. Will be used by Frontend client to check the type and proceed accordingly
            if (!propertyType.Equals(Constants.Hotel) && !propertyType.Equals(Constants.GuestHouse))
            {
                throw new InvalidOperationException("Hotel instance can only be created with type Hotel");
            }
            Restaurant = restaurant;
            AirportShuttle = airportShuttle;
            BreakfastIncluded = breakfastIncluded;
            SittingArea = sittingArea;
            CarRental = carRental;
            Spa = spa;
            Salon = salon;
            Bathtub = bathtub;
            SwimmingPool = swimmingPool;
            Kitchen = kitchen;
            Beds = beds;
            Occupants = occupants;
        }

        /// <summary>
        /// update the hostel instance with new values
        /// </summary>
        /// <param name="title"></param>
        /// <param name="rentPrice"></param>
        /// <param name="ownerEmail"></param>
        /// <param name="ownerPhoneNumber"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="area"></param>
        /// <param name="ownerName"></param>
        /// <param name="description"></param>
        /// <param name="genderRestriction"></param>
        /// <param name="isShared"></param>
        /// <param name="internetAvailable"></param>
        /// <param name="cableTvAvailable"></param>
        /// <param name="parkingAvailable"></param>
        /// <param name="rentUnit"></param>
        /// <param name="propertyType"></param>
        /// <param name="laundry"></param>
        /// <param name="ac"></param>
        /// <param name="geyser"></param>
        /// <param name="fitnessCentre"></param>
        /// <param name="attachedBathroom"></param>
        /// <param name="ironing"></param>
        /// <param name="balcony"></param>
        /// <param name="lawn"></param>
        /// <param name="cctvCameras"></param>
        /// <param name="backupElectricity"></param>
        /// <param name="meals"></param>
        /// <param name="picknDrop"></param>
        /// <param name="numberOfSeats"></param>
        /// <param name="heating"></param>
        /// <param name="beds"></param>
        /// <param name="occupants"></param>
        /// <param name="landlineNumber"></param>
        /// <param name="fax"></param>
        /// <param name="restaurant"></param>
        /// <param name="airportShuttle"></param>
        /// <param name="breakfastIncluded"></param>
        /// <param name="sittingArea"></param>
        /// <param name="carRental"></param>
        /// <param name="spa"></param>
        /// <param name="salon"></param>
        /// <param name="bathtub"></param>
        /// <param name="swimmingPool"></param>
        /// <param name="kitchen"></param>
        /// <param name="elevator"></param>
        public void Update(string title, long rentPrice, string ownerEmail,
            string ownerPhoneNumber,
            decimal latitude, decimal longitude, string area, string ownerName,
            string description,
            GenderRestriction genderRestriction, bool isShared, string rentUnit, bool internetAvailable,
            bool cableTvAvailable, bool parkingAvailable, string propertyType, bool laundry, bool ac,
            bool geyser, bool fitnessCentre, bool attachedBathroom, bool ironing,
            bool balcony, bool lawn, bool cctvCameras, bool backupElectricity, bool heating, bool restaurant,
            bool airportShuttle, bool breakfastIncluded, 
            bool sittingArea, bool carRental, bool spa, bool salon, bool bathtub, bool swimmingPool, bool kitchen, 
            IList<Bed> beds, Occupants occupants, string landlineNumber, string fax, bool elevator)
        {
            // Property Type must be Hotel. Will be used by Frontend client to check the type and proceed accordingly
            if (!propertyType.Equals(Constants.Hotel) && !propertyType.Equals(Constants.GuestHouse))
            {
                throw new InvalidOperationException("Hotel instance can only be created with type Hotel or Guest House");
            }
            base.UpdateGuestProperty(title, rentPrice, ownerEmail,
                ownerPhoneNumber, latitude, longitude, area, ownerName, description, genderRestriction, isShared,
                rentUnit, internetAvailable, cableTvAvailable, parkingAvailable, propertyType, laundry, ac, geyser, fitnessCentre,
                attachedBathroom, ironing, balcony, lawn, cctvCameras, backupElectricity, heating, landlineNumber,
                fax, elevator);
            Restaurant = restaurant;
            AirportShuttle = airportShuttle;
            BreakfastIncluded = breakfastIncluded;
            SittingArea = sittingArea;
            CarRental = carRental;
            Spa = spa;
            Salon = salon;
            Bathtub = bathtub;
            SwimmingPool = swimmingPool;
            Kitchen = kitchen;
            Beds = beds;
            Occupants = occupants;
        }

        public bool Restaurant { get; private set; }

        public bool AirportShuttle { get; private set; }

        public bool BreakfastIncluded { get; private set; }

        public bool SittingArea { get; private set; }

        public bool CarRental { get; private set; }

        public bool Spa { get; private set; }

        public bool Salon { get; private set; }

        public bool Bathtub { get; private set; }

        public bool SwimmingPool { get; private set; }
        
        public bool Kitchen { get; private set; }

        private IList<Bed> _beds;

        [JsonIgnore]
        public virtual IList<Bed> Beds
        {
            get
            {
                return _beds;
            }

            set
            {
                if (value != null)
                {
                    _beds = new List<Bed>();
                    foreach (var currentValue in value)
                    {
                        if (_beds.Count > 0)
                        {
                            var bedTypeFound = false;
                            for (int i = 0; i < _beds.Count; i++)
                            {
                                if (_beds[i].BedType == currentValue.BedType)
                                {
                                    _beds[i].BedCount += currentValue.BedCount;
                                    bedTypeFound = true;
                                }
                            }
                            if (!bedTypeFound)
                            {
                                _beds.Add(currentValue);
                            }
                        }
                        else
                        {
                            _beds.Add(currentValue);
                        }
                    }
                }
            }
        }

        public Occupants Occupants { get; set; }

        /// <summary>
        /// Hotel Builder, using the builder pattern that allows easy refactoring of the initialization of the Hotel
        /// entity
        /// </summary>
        public class HotelBuilder
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
            private bool _internetAvailable;
            private bool _cableTvAvailable;
            private bool _parkingAvailable;
            private string _landlineNumber;
            private string _fax;

            /// <summary>
            /// Title
            /// </summary>
            /// <param name="title"></param>
            /// <returns></returns>
            public HotelBuilder Title(string title)
            {
                _title = title;
                return this;
            }

            /// <summary>
            /// Description
            /// </summary>
            /// <param name="description"></param>
            /// <returns></returns>
            public HotelBuilder Description(string description)
            {
                _description = description;
                return this;
            }

            /// <summary>
            /// Rent price
            /// </summary>
            /// <param name="rentPrice"></param>
            /// <returns></returns>
            public HotelBuilder RentPrice(long rentPrice)
            {
                _rentPrice = rentPrice;
                return this;
            }

            /// <summary>
            /// Property type
            /// </summary>
            /// <param name="propertyType"></param>
            /// <returns></returns>
            public HotelBuilder PropertyType(string propertyType)
            {
                _propertyType = propertyType;
                return this;
            }

            /// <summary>
            /// Owner email
            /// </summary>
            /// <param name="ownerEmail"></param>
            /// <returns></returns>
            public HotelBuilder OwnerEmail(string ownerEmail)
            {
                _ownerEmail = ownerEmail;
                return this;
            }

            /// <summary>
            /// Owner phone number
            /// </summary>
            /// <param name="ownerPhoneNumber"></param>
            /// <returns></returns>
            public HotelBuilder OwnerPhoneNumber(string ownerPhoneNumber)
            {
                _ownerPhoneNumber = ownerPhoneNumber;
                return this;
            }

            /// <summary>
            /// Latitude
            /// </summary>
            /// <param name="latitude"></param>
            /// <returns></returns>
            public HotelBuilder Latitude(decimal latitude)
            {
                _latitude = latitude;
                return this;
            }

            /// <summary>
            /// Longitude
            /// </summary>
            /// <param name="longitude"></param>
            /// <returns></returns>
            public HotelBuilder Longitude(decimal longitude)
            {
                _longitude = longitude;
                return this;
            }

            /// <summary>
            /// Area
            /// </summary>
            /// <param name="area"></param>
            /// <returns></returns>
            public HotelBuilder Area(string area)
            {
                _area = area;
                return this;
            }

            /// <summary>
            /// Owner name
            /// </summary>
            /// <param name="ownerName"></param>
            /// <returns></returns>
            public HotelBuilder OwnerName(string ownerName)
            {
                _ownerName = ownerName;
                return this;
            }

            /// <summary>
            /// Gender restrictions
            /// </summary>
            /// <param name="genderRestriction"></param>
            /// <returns></returns>
            public HotelBuilder GenderRestriction(GenderRestriction genderRestriction)
            {
                _genderRestriction = genderRestriction;
                return this;
            }

            /// <summary>
            /// Is this property shared
            /// </summary>
            /// <param name="isShared"></param>
            /// <returns></returns>
            public HotelBuilder IsShared(bool isShared)
            {
                _isShared = isShared;
                return this;
            }

            /// <summary>
            /// What is the unit of rent payment
            /// </summary>
            /// <param name="rentUnit"></param>
            /// <returns></returns>
            public HotelBuilder RentUnit(string rentUnit)
            {
                _rentUnit = rentUnit;
                return this;
            }

            /// <summary>
            /// Is internet available
            /// </summary>
            /// <param name="internetAvailable"></param>
            /// <returns></returns>
            public HotelBuilder WithInternetAvailable(bool internetAvailable)
            {
                _internetAvailable = internetAvailable;
                return this;
            }

            /// <summary>
            /// Is Parking available
            /// </summary>
            /// <param name="parkingAvailable"></param>
            /// <returns></returns>
            public HotelBuilder ParkingAvailable(bool parkingAvailable)
            {
                _parkingAvailable = parkingAvailable;
                return this;
            }

            /// <summary>
            /// is cable tv available
            /// </summary>
            /// <param name="cableTvAvailable"></param>
            /// <returns></returns>
            public HotelBuilder CableTvAvailable(bool cableTvAvailable)
            {
                _cableTvAvailable = cableTvAvailable;
                return this;
            }

            /// <summary>
            /// Landline Number
            /// </summary>
            /// <param name="landlineNumber"></param>
            /// <returns></returns>
            public HotelBuilder LandlineNumber(string landlineNumber)
            {
                _landlineNumber = landlineNumber;
                return this;
            }

            /// <summary>
            /// Fax
            /// </summary>
            /// <param name="fax"></param>
            /// <returns></returns>
            public HotelBuilder Fax(string fax)
            {
                _fax = fax;
                return this;
            }

            #endregion Generic Property attributes

            #region Hostel Specific Properties 

            private bool _laundry;
            private bool _ac;
            private bool _geyser;
            private bool _fitnessCentre;
            private bool _attachedBathroom;
            private bool _ironing;
            private bool _balcony;
            private bool _lawn;
            private bool _cctvCameras;
            private bool _backupElectricity;
            private bool _heating;

            private bool _restaurant;
            private bool _airportShuttle;
            private bool _breakfastIncluded;
            private bool _sittingArea;
            private bool _carRental;
            private bool _spa;
            private bool _salon;
            private bool _bathtub;
            private bool _swimmingPool;
            private bool _kitchen;
            private IList<Bed> _beds;
            private Occupants _occupants;
            private bool _elevator;

            public HotelBuilder Laundry(bool laundry)
            {
                _laundry = laundry;
                return this;
            }

            public HotelBuilder AC(bool ac)
            {
                _ac = ac;
                return this;
            }

            public HotelBuilder Geyser(bool geyser)
            {
                _geyser = geyser;
                return this;
            }

            public HotelBuilder FitnessCentre(bool fitnessCentre)
            {
                _fitnessCentre = fitnessCentre;
                return this;
            }

            public HotelBuilder AttachedBathroom(bool attachedBathroom)
            {
                _attachedBathroom = attachedBathroom;
                return this;
            }

            public HotelBuilder Ironing(bool ironing)
            {
                _ironing = ironing;
                return this;
            }

            public HotelBuilder Balcony(bool balcony)
            {
                _balcony = balcony;
                return this;
            }

            public HotelBuilder Lawn(bool lawn)
            {
                _lawn = lawn;
                return this;
            }

            public HotelBuilder Heating(bool heating)
            {
                _heating = heating;
                return this;
            }

            public HotelBuilder CctvCameras(bool cctvCameras)
            {
                _cctvCameras = cctvCameras;
                return this;
            }

            public HotelBuilder BackupElectricity(bool backupElectricity)
            {
                _backupElectricity = backupElectricity;
                return this;
            }

            public HotelBuilder Restaurant(bool restaurant)
            {
                _restaurant = restaurant;
                return this;
            }

            public HotelBuilder AirportShuttle(bool airportShuttle)
            {
                _airportShuttle = airportShuttle;
                return this;
            }

            public HotelBuilder BreakfastIncluded(bool breakfastIncluded)
            {
                _breakfastIncluded = breakfastIncluded;
                return this;
            }

            public HotelBuilder SittingArea(bool sittingArea)
            {
                _sittingArea = sittingArea;
                return this;
            }

            public HotelBuilder CarRental(bool carRental)
            {
                _carRental = carRental;
                return this;
            }

            public HotelBuilder Spa(bool spa)
            {
                _spa = spa;
                return this;
            }

            public HotelBuilder Salon(bool salon)
            {
                _salon = salon;
                return this;
            }

            public HotelBuilder Bathtub(bool bathtub)
            {
                _bathtub = bathtub;
                return this;
            }

            public HotelBuilder SwimmingPool(bool swimmingPool)
            {
                _swimmingPool = swimmingPool;
                return this;
            }

            public HotelBuilder Kitchen(bool kitchen)
            {
                _kitchen = kitchen;
                return this;
            }

            public HotelBuilder Beds(IList<Bed> beds)
            {
                _beds = beds;
                return this;
            }

            public HotelBuilder Occupants(Occupants occupants)
            {
                _occupants = occupants;
                return this;
            }

            public HotelBuilder Elevator(bool elevator)
            {
                _elevator = elevator;
                return this;
            }

            #endregion Hostel Specific Properties 

            /// <summary>
            /// Build a new instance of House
            /// </summary>
            /// <returns></returns>
            public Hotel Build()
            {
                return new Hotel(_title, _rentPrice, _ownerEmail, _ownerPhoneNumber, _latitude, _longitude,
                    _area, _ownerName, _description, _genderRestriction, _isShared, _rentUnit, _internetAvailable,
                    _cableTvAvailable, _parkingAvailable, _propertyType, _laundry, _ac, _geyser, _fitnessCentre, _attachedBathroom,
                    _ironing, _balcony, _lawn, _cctvCameras, _backupElectricity, _heating, _restaurant,
                    _airportShuttle, _breakfastIncluded, _sittingArea, _carRental, _spa, _salon, _bathtub, 
                    _swimmingPool, _kitchen, _beds, _occupants, _landlineNumber, _fax, _elevator);
            }
        }
    }
}
