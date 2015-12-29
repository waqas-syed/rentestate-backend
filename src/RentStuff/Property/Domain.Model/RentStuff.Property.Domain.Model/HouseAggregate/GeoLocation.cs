
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Property.Domain.Model.HouseAggregate
{
    /// <summary>
    /// Represents the location & coordinates for a given address
    /// </summary>
    public class GeoLocation
    {
        private string _name;
        private string _alternateNames;
        private decimal _latitude;
        private decimal _longitude;
        private string _countryCode;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public GeoLocation()
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="alternateNames"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="countryCode"></param>
        public GeoLocation(string name, string alternateNames, decimal latitude, decimal longitude, string countryCode)
        {
            Name = name;
            _alternateNames = alternateNames;
            _latitude = latitude;
            _longitude = longitude;
            _countryCode = countryCode;
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// AlternateNames
        /// </summary>
        public string AlternateNames { get; private set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public decimal Latitude { get; private set; }

        /// <summary>
        /// Longitude
        /// </summary>
        public string Longitude { get; private set; }

        /// <summary>
        /// CountryCode
        /// </summary>
        public string CountryCode { get; private set; }
    }
}
