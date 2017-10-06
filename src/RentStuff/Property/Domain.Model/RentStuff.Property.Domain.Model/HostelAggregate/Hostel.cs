using RentStuff.Property.Domain.Model.PropertyAggregate;
using System;

namespace RentStuff.Property.Domain.Model.HostelAggregate
{
    /// <summary>
    /// Hostel property type
    /// </summary>
    public class Hostel : PropertyAggregate.GuestPropertyAbstraction
    {
        /// <summary>
        /// Default Constructor to support NHibernate
        /// </summary>
        public Hostel()
        {
            
        }

        public Hostel(string title, long rentPrice, string ownerEmail,
            string ownerPhoneNumber,
            decimal latitude, decimal longitude, string area, string ownerName,
            string description,
            GenderRestriction genderRestriction, bool isShared, string rentUnit, bool internetAvailable,
            bool cableTvAvailable, bool parkingAvailable, string propertyType, bool laundry, bool ac,
            bool geyser, bool fitnessCentre, bool attachedBathroom, bool ironing,
            bool balcony, bool lawn, bool cctvCameras, bool backupElectricity, bool heating, bool meals, 
            bool picknDrop, int numberOfSeats, string landlineNumber, string fax, bool elevator)
            // Initiate the parent GuerstPropertyAbstraction class as well
            : base(title, rentPrice, ownerEmail,
                ownerPhoneNumber, latitude, longitude, area, ownerName, description, genderRestriction, isShared,
                rentUnit, internetAvailable, cableTvAvailable, parkingAvailable, propertyType, laundry, ac, geyser,
                fitnessCentre,
                attachedBathroom, ironing, balcony, lawn, cctvCameras, backupElectricity, heating, landlineNumber, 
                fax, elevator)
        {
            // Property Type must be Hostel. Will be used by Frontend client to check the type and proceed accordingly
            if (!propertyType.Equals("Hostel"))
            {
                throw new InvalidOperationException("Hostel instance can only be created with type Hostel");
            }
            Meals = meals;
            PicknDrop = picknDrop;
            NumberOfSeats = numberOfSeats;
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
        /// <param name="landlineNumber"></param>
        /// <param name="fax"></param>
        /// <param name="elevator"></param>
        public void Update(string title, long rentPrice, string ownerEmail,
            string ownerPhoneNumber,
            decimal latitude, decimal longitude, string area, string ownerName,
            string description,
            GenderRestriction genderRestriction, bool isShared, string rentUnit, bool internetAvailable,
            bool cableTvAvailable, bool parkingAvailable, string propertyType, bool laundry, bool ac,
            bool geyser, bool fitnessCentre, bool attachedBathroom, bool ironing,
            bool balcony, bool lawn, bool cctvCameras, bool backupElectricity, bool meals, bool picknDrop,
            int numberOfSeats, bool heating, string landlineNumber, string fax, bool elevator)
        {
            // Property Type must be Hostel. Will be used by Frontend client to check the type and proceed accordingly
            if (!propertyType.Equals("Hostel"))
            {
                throw new InvalidOperationException("Hostel instance can only be created with type Hostel");
            }
            Meals = meals;
            PicknDrop = picknDrop;
            NumberOfSeats = numberOfSeats;
            base.UpdateGuestProperty(title, rentPrice, ownerEmail,
                ownerPhoneNumber, latitude, longitude, area, ownerName, description, genderRestriction, isShared,
                rentUnit, internetAvailable, cableTvAvailable, parkingAvailable, propertyType, laundry, ac, geyser, fitnessCentre,
                attachedBathroom, ironing, balcony, lawn, cctvCameras, backupElectricity, heating, landlineNumber,
                fax, elevator);
        }

        /// <summary>
        /// Is Meals available
        /// </summary>
        public bool Meals { get; private set; }

        /// <summary>
        /// Is Pick n Drop available
        /// </summary>
        public bool PicknDrop { get; private set; }

        /// <summary>
        /// How many seats are there in this hostel room
        /// </summary>
        public int NumberOfSeats { get; private set; }

        /// <summary>
        /// House Builder, using the builder pattern that allows easy refactoring of the references and usages of
        /// the initialization of the House entity
        /// </summary>
        public class HostelBuilder
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
            private bool _garageAvailable;
            private string _landlineNumber;
            private string _fax;
            
            /// <summary>
            /// Title
            /// </summary>
            /// <param name="title"></param>
            /// <returns></returns>
            public HostelBuilder Title(string title)
            {
                _title = title;
                return this;
            }

            /// <summary>
            /// Description
            /// </summary>
            /// <param name="description"></param>
            /// <returns></returns>
            public HostelBuilder Description(string description)
            {
                _description = description;
                return this;
            }

            /// <summary>
            /// Rent price
            /// </summary>
            /// <param name="rentPrice"></param>
            /// <returns></returns>
            public HostelBuilder RentPrice(long rentPrice)
            {
                _rentPrice = rentPrice;
                return this;
            }

            /// <summary>
            /// Property type
            /// </summary>
            /// <param name="propertyType"></param>
            /// <returns></returns>
            public HostelBuilder PropertyType(string propertyType)
            {
                _propertyType = propertyType;
                return this;
            }

            /// <summary>
            /// Owner email
            /// </summary>
            /// <param name="ownerEmail"></param>
            /// <returns></returns>
            public HostelBuilder OwnerEmail(string ownerEmail)
            {
                _ownerEmail = ownerEmail;
                return this;
            }

            /// <summary>
            /// Owner phone number
            /// </summary>
            /// <param name="ownerPhoneNumber"></param>
            /// <returns></returns>
            public HostelBuilder OwnerPhoneNumber(string ownerPhoneNumber)
            {
                _ownerPhoneNumber = ownerPhoneNumber;
                return this;
            }

            /// <summary>
            /// Latitude
            /// </summary>
            /// <param name="latitude"></param>
            /// <returns></returns>
            public HostelBuilder Latitude(decimal latitude)
            {
                _latitude = latitude;
                return this;
            }

            /// <summary>
            /// Longitude
            /// </summary>
            /// <param name="longitude"></param>
            /// <returns></returns>
            public HostelBuilder Longitude(decimal longitude)
            {
                _longitude = longitude;
                return this;
            }

            /// <summary>
            /// Area
            /// </summary>
            /// <param name="area"></param>
            /// <returns></returns>
            public HostelBuilder Area(string area)
            {
                _area = area;
                return this;
            }

            /// <summary>
            /// Owner name
            /// </summary>
            /// <param name="ownerName"></param>
            /// <returns></returns>
            public HostelBuilder OwnerName(string ownerName)
            {
                _ownerName = ownerName;
                return this;
            }

            /// <summary>
            /// Gender restrictions
            /// </summary>
            /// <param name="genderRestriction"></param>
            /// <returns></returns>
            public HostelBuilder GenderRestriction(GenderRestriction genderRestriction)
            {
                _genderRestriction = genderRestriction;
                return this;
            }

            /// <summary>
            /// Is this property shared
            /// </summary>
            /// <param name="isShared"></param>
            /// <returns></returns>
            public HostelBuilder IsShared(bool isShared)
            {
                _isShared = isShared;
                return this;
            }

            /// <summary>
            /// What is the unit of rent payment
            /// </summary>
            /// <param name="rentUnit"></param>
            /// <returns></returns>
            public HostelBuilder RentUnit(string rentUnit)
            {
                _rentUnit = rentUnit;
                return this;
            }

            /// <summary>
            /// Is internet available
            /// </summary>
            /// <param name="internetAvailable"></param>
            /// <returns></returns>
            public HostelBuilder WithInternetAvailable(bool internetAvailable)
            {
                _internetAvailable = internetAvailable;
                return this;
            }

            /// <summary>
            /// Is Parking available
            /// </summary>
            /// <param name="garageAvailable"></param>
            /// <returns></returns>
            public HostelBuilder GarageAvailable(bool garageAvailable)
            {
                _garageAvailable = garageAvailable;
                return this;
            }

            /// <summary>
            /// is cable tv available
            /// </summary>
            /// <param name="cableTvAvailable"></param>
            /// <returns></returns>
            public HostelBuilder CableTvAvailable(bool cableTvAvailable)
            {
                _cableTvAvailable = cableTvAvailable;
                return this;
            }

            /// <summary>
            /// Landline Number
            /// </summary>
            /// <param name="landlineNumber"></param>
            /// <returns></returns>
            public HostelBuilder LandlineNumber(string landlineNumber)
            {
                _landlineNumber = landlineNumber;
                return this;
            }

            /// <summary>
            /// Fax
            /// </summary>
            /// <param name="fax"></param>
            /// <returns></returns>
            public HostelBuilder Fax(string fax)
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
            private bool _meals;
            private int _numberOfSeats;
            private bool _picknDrop;
            private bool _heating;
            private bool _elevator;

            public HostelBuilder Laundry(bool laundry)
            {
                _laundry = laundry;
                return this;
            }

            public HostelBuilder AC(bool ac)
            {
                _ac = ac;
                return this;
            }

            public HostelBuilder Geyser(bool geyser)
            {
                _geyser = geyser;
                return this;
            }

            public HostelBuilder FitnessCentre(bool fitnessCentre)
            {
                _fitnessCentre = fitnessCentre;
                return this;
            }

            public HostelBuilder AttachedBathroom(bool attachedBathroom)
            {
                _attachedBathroom = attachedBathroom;
                return this;
            }

            public HostelBuilder Ironing(bool ironing)
            {
                _ironing = ironing;
                return this;
            }

            public HostelBuilder Balcony(bool balcony)
            {
                _balcony = balcony;
                return this;
            }

            public HostelBuilder Lawn(bool lawn)
            {
                _lawn = lawn;
                return this;
            }

            public HostelBuilder CctvCameras(bool cctvCameras)
            {
                _cctvCameras = cctvCameras;
                return this;
            }

            public HostelBuilder BackupElectricity(bool backupElectricity)
            {
                _backupElectricity = backupElectricity;
                return this;
            }

            public HostelBuilder Meals(bool meals)
            {
                _meals = meals;
                return this;
            }

            public HostelBuilder NumberOfSeats(int numberOfSeats)
            {
                _numberOfSeats = numberOfSeats;
                return this;
            }

            public HostelBuilder PicknDrop(bool picknDrop)
            {
                _picknDrop = picknDrop;
                return this;
            }

            public HostelBuilder Heating(bool heating)
            {
                _heating = heating;
                return this;
            }

            public HostelBuilder Elevator(bool elevator)
            {
                _elevator = elevator;
                return this;
            }

            #endregion Hostel Specific Properties 

            /// <summary>
            /// Build a new instance of House
            /// </summary>
            /// <returns></returns>
            public Hostel Build()
            {
                return new Hostel(_title, _rentPrice, _ownerEmail, _ownerPhoneNumber, _latitude, _longitude, 
                    _area, _ownerName, _description, _genderRestriction, _isShared, _rentUnit, _internetAvailable,
                    _cableTvAvailable, _garageAvailable, _propertyType, _laundry, _ac, _geyser, _fitnessCentre, _attachedBathroom,
                    _ironing, _balcony, _lawn, _cctvCameras, _backupElectricity, _heating, _meals, _picknDrop, 
                    _numberOfSeats, _landlineNumber, _fax, _elevator);
            }
        }
    }
}
