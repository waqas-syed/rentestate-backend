
namespace RentStuff.Property.Application.HouseServices.Commands
{
    /// <summary>
    /// Command for the House instance
    /// </summary>
    public class CreateHouseCommand
    {
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
        private bool _garageAvailable;
        private bool _smokingAllowed;
        private string _propertyType;
        private string _ownerEmail;
        private string _ownerPhoneNumber;
        private string _houseNo;
        private string _streetNo;
        private string _area;

        public CreateHouseCommand(long monthlyRent, int numberOfBedrooms,
            int numberOfKitchens, int numberOfBathrooms, bool familiesOnly, bool boysOnly, bool girlsOnly,
            bool internetAvailable, bool landlinePhoneAvailable, bool cableTvAvailable, 
            bool garageAvailable, bool smokingAllowed, string propertyType, string ownerEmail, string ownerPhoneNumber, 
            string houseNo, string streetNo, string area)
        {
            _monthlyRent = monthlyRent;
            _numberOfBedrooms = numberOfBedrooms;
            _numberOfKitchens = numberOfKitchens;
            _numberOfBathrooms = numberOfBathrooms;
            _familiesOnly = familiesOnly;
            _boysOnly = boysOnly;
            _girlsOnly = girlsOnly;
            _internetAvailable = internetAvailable;
            _landlinePhoneAvailable = landlinePhoneAvailable;
            _cableTvAvailable = cableTvAvailable;
            _garageAvailable = garageAvailable;
            _smokingAllowed = smokingAllowed;
            _propertyType = propertyType;
            _ownerEmail = ownerEmail;
            _ownerPhoneNumber = ownerPhoneNumber;
            _houseNo = houseNo;
            _streetNo = streetNo;
            _area = area;
        }

        public long MonthlyRent
        {
            get { return _monthlyRent; }
            private set
            {
                _monthlyRent = value;
            }
        }

        public int NumberOfBedrooms
        {
            get { return _numberOfBedrooms; }
            private set { _numberOfBedrooms = value; }
        }

        public int NumberOfKitchens
        {
            get { return _numberOfKitchens; }
            private set { _numberOfKitchens = value; }
        }

        public bool FamiliesOnly
        {
            get { return _familiesOnly; }
            private set { _familiesOnly = value; }
        }

        public int NumberOfBathrooms
        {
            get { return _numberOfBathrooms; }
            private set { _numberOfBathrooms = value; }
        }

        public bool GirlsOnly
        {
            get { return _girlsOnly; }
            private set { _girlsOnly = value; }
        }

        public bool BoysOnly
        {
            get { return _boysOnly; }
            private set { _boysOnly = value; }
        }

        public bool InternetAvailable
        {
            get { return _internetAvailable; }
            private set { _internetAvailable = value; }
        }

        public bool LandlinePhoneAvailable
        {
            get { return _landlinePhoneAvailable; }
            private set { _landlinePhoneAvailable = value; }
        }

        public bool CableTvAvailable
        {
            get { return _cableTvAvailable; }
            private set { _cableTvAvailable = value; }
        }

        public bool GarageAvailable
        {
            get { return _garageAvailable; }
            private set { _garageAvailable = value; }
        }

        public bool SmokingAllowed
        {
            get { return _smokingAllowed; }
            private set { _smokingAllowed = value; }
        }

        public string PropertyType
        {
            get { return _propertyType; }
            private set { _propertyType = value; }
        }

        public string OwnerEmail
        {
            get { return _ownerEmail; }
            private set { _ownerEmail = value; }
        }

        public string OwnerPhoneNumber
        {
            get { return _ownerPhoneNumber; }
            private set { _ownerPhoneNumber = value; }
        }

        public string HouseNo
        {
            get { return _houseNo; }
            private set { _houseNo = value; }
        }

        public string StreetNo
        {
            get { return _streetNo; }
            private set { _streetNo = value; }
        }

        public string Area
        {
            get { return _area; }
            private set { _area = value; }
        }
    }
}
