using System.Collections.Generic;
using RentStuff.Property.Application.HouseServices.Commands;
using RentStuff.Property.Domain.Model.HouseAggregate;

namespace RentStuff.Property.Application.HouseServices
{
    /// <summary>
    /// Interface for House aggregate related operations
    /// </summary>
    public interface IHouseApplicationService
    {
        /// <summary>
        /// Saves a new house instance to the database
        /// </summary>
        void SaveNewHouseOffer(CreateHouseCommand house);

        /// <summary>
        /// Delete the given house instance
        /// </summary>
        /// <param name="house"></param>
        void DeleteHouse(House house);

        /// <summary>
        /// Delete the house by providing the id
        /// </summary>
        /// <param name="id"></param>
        void DeleteHouseById(string id);

        /// <summary>
        /// Gets the house by providing the owner's email id
        /// </summary>
        /// <returns></returns>
        IList<House> GetHouseByEmail(string email);

        /// <summary>
        /// Get all houses
        /// </summary>
        /// <returns></returns>
        IList<House> GetAllHouses();
    }
}
