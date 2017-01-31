﻿
using System.Collections;
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

        void SaveorUpdateDimension(Dimension dimension);

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
        House GetHouseById(string id);

        /// <summary>
        /// Get the owner house by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        IList<House> GetHouseByOwnerEmail(string email);

        /// <summary>
        /// Get the house by providing the latitude and longitude
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        IList<House> GetHouseByCoordinates(decimal latitude, decimal longitude);

        /// <summary>
        /// Get all the houses
        /// </summary>
        /// <returns></returns>
        IList<House> GetAllHouses();

        /// <summary>
        /// Gets the houses by their coordinates
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        IList<House> SearchHousesByCoordinates(decimal latitude, decimal longitude);

        /// <summary>
        /// Gets the houses by coordinates and property type
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        IList<House> SearchHousesByCoordinatesAndPropertyType(decimal latitude, decimal longitude, PropertyType propertyType);
    }
}
