using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Property.Domain.Model.PropertyAggregate
{
    /// <summary>
    /// Used for Rediential Properties such as House, Apartments, Hostels, Hotels & Guest Houses
    /// </summary>
    public class ResidentialProperty : Property
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ResidentialProperty()
        {
            
        }

        /// <summary>
        /// Just pass on the given parameters to the base Property class
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
        /// <param name="garageAvailable"></param>
        public ResidentialProperty(string title, long rentPrice, string ownerEmail, string ownerPhoneNumber,
            decimal latitude, decimal longitude, string area, string ownerName, string description,
            GenderRestriction genderRestriction, bool isShared, string rentUnit, bool internetAvailable,
            bool cableTvAvailable, bool garageAvailable, string propertytype, string landlineNumber, string fax)
            // Initiate the parent Property class as well
            : base(title, rentPrice, ownerEmail,
                ownerPhoneNumber, latitude, longitude, area, ownerName, description, genderRestriction, isShared,
                rentUnit, internetAvailable, cableTvAvailable, garageAvailable, propertytype, landlineNumber, fax)
        {
            
        }
    }
}
