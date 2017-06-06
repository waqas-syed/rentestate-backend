using System;

namespace RentStuff.Common.Services.LocationServices
{
    /// <summary>
    /// Service responsible for getting the corordinates given the address
    /// </summary>
    public interface IGeocodingService
    {
        /// <summary>
        /// Gets the corordinates given the address
        /// Item1 = Latitude, Item2 = Longitude
        /// </summary>
        Tuple<decimal, decimal> GetCoordinatesFromAddress(string address);
    }
}
