
using System.Collections.Generic;

namespace RentStuff.Property.Domain.Model.HouseAggregate
{
    /// <summary>
    /// Repository for saving and retreving House aggregate instances
    /// </summary>
    public interface IHouseRepository
    {
        /// <summary>
        /// Saves new House or updates existing house
        /// </summary>
        /// <param name="house"></param>
        void SaveorUpdate(House house);

        /// <summary>
        /// Deletes the house object from the database
        /// </summary>
        /// <param name="house"></param>
        void Delete(House house);

        /// <summary>
        /// Gets the house by the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        House GetHouseById(long id);

        /// <summary>
        /// Get the owner house by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        IList<House> GetHouseByOwnerEmail(string email);

        /// <summary>
        /// Get all the houses
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        IList<House> GetAllHouses();
    }
}
