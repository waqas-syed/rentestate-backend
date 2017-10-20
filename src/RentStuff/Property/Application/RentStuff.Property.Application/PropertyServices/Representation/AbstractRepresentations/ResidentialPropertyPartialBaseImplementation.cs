namespace RentStuff.Property.Application.PropertyServices.Representation.AbstractRepresentations
{
    /// <summary>
    /// Partial representation for al residential properties
    /// </summary>
    public class ResidentialPropertyPartialBaseImplementation
    {
        public ResidentialPropertyPartialBaseImplementation(string id, string title, long rentPrice,
            string ownerPhoneNumber, string ownerLandlineNumber, string area, string ownerName,
            string genderRestriction, bool isShared, string rentUnit, bool internetAvailable,
            bool cableTvAvailable, string propertyType, string defaultImage)
        {
            Id = id;
            Title = title;
            RentPrice = rentPrice;
            OwnerPhoneNumber = ownerPhoneNumber;
            OwnerLandlineNumber = ownerLandlineNumber;
            Area = area;
            OwnerName = ownerName;
            GenderRestriction = genderRestriction;
            IsShared = isShared;
            RentUnit = rentUnit;
            Internet = internetAvailable;
            CableTv = cableTvAvailable;
            PropertyType = propertyType;
            DefaultImage = defaultImage;
        }

        public string Id { get; private set; }
        public string Title { get; private set; }
        public long RentPrice { get; private set; }
        public string OwnerPhoneNumber { get; private set; }
        public string OwnerLandlineNumber { get; private set; }
        public string Area { get; private set; }
        public string OwnerName { get; private set; }
        public string GenderRestriction { get; private set; }
        public bool IsShared { get; private set; }
        public string RentUnit { get; private set; }
        public bool Internet { get; private set; }
        public bool CableTv { get; private set; }
        public string PropertyType { get; private set; }
        public string DefaultImage { get; set; }
    }
}
