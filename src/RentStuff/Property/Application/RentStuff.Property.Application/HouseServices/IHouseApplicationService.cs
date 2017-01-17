using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using RentStuff.Property.Application.HouseServices.Commands;
using RentStuff.Property.Application.HouseServices.Representation;
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
        bool SaveNewHouseOffer(CreateHouseCommand house);

        /// <summary>
        /// Update an existing house
        /// </summary>
        /// <param name="updateHouseCommand"></param>
        /// <returns></returns>
        bool UpdateHouse(UpdateHouseCommand updateHouseCommand);

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
        /// Get House by it's ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Domain.Model.HouseAggregate.House GetHouseById(string id);

        /// <summary>
        /// Gets the house by providing the owner's email id
        /// </summary>
        /// <returns></returns>
        IList<HouseRepresentation> GetHouseByEmail(string email);
        
        /// <summary>
        /// Search nearby houses by providing the address
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        IList<HouseRepresentation> SearchHousesByAddress(string address);

        /// <summary>
        /// Get all houses
        /// </summary>
        /// <returns></returns>
        IList<HouseRepresentation> GetAllHouses();

        /// <summary>
        /// Get the types of property avaialable on our partal: House, Apartment, Hostel, Room
        /// </summary>
        /// <returns></returns>
        IList<string> GetPropertyTypes();

        /// <summary>
        /// Add images to an existing House instance
        /// </summary>
        void AddImagesToHouse(string houseId, IList<string> imagesList);
    }
}
