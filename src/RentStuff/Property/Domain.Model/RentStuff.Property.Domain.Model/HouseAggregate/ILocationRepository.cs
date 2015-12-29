using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Property.Domain.Model.HouseAggregate
{
    /// <summary>
    /// Location Repository
    /// </summary>
    public interface ILocationRepository
    {
        /// <summary>
        /// Saves or Updates the Location instance
        /// </summary>
        /// <param name="location"></param>
        void SaveOrUpdate(Location location);

        /// <summary>
        /// Get the Location and info given the coordinates
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        IList GetLocationByCoordinates(decimal latitude, decimal longitude);
    }
}
