using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Common.Services.LocationServices.Mocks
{
    /// <summary>
    /// Mock for the Geocoding service
    /// </summary>
    public class MockGeocodingService : IGeocodingService
    {
        /// <summary>
        /// Gets the corordinates given the address
        /// Item1 = Latitude, Item2 = Longitude
        /// </summary>
        public Tuple<decimal, decimal> GetCoordinatesFromAddress(string address)
        {
            // Just return a dummy value
            return new Tuple<decimal, decimal>(33.50M, 73.0M);
        }
    }
}
