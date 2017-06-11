
using System;
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
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<House> GetHouseByOwnerEmail(string email, int pageNo = 0);

        /// <summary>
        /// Get the house by providing the latitude and longitude
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<House> GetHouseByCoordinates(decimal latitude, decimal longitude);

        /// <summary>
        /// Get all the houses
        /// </summary>
        /// <returns></returns>
        IList<House> GetAllHouses(int pageNo = 0);

        /// <summary>
        /// Gets the houses by their coordinates
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<House> SearchHousesByCoordinates(decimal latitude, decimal longitude, int pageNo = 0);

        /// <summary>
        /// Get houses with reference to their PropertyType
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<House> SearchHousesByPropertyType(PropertyType propertyType, int pageNo = 0);

        /// <summary>
        /// Gets the houses by coordinates and property type
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="propertyType"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<House> SearchHousesByCoordinatesAndPropertyType(decimal latitude, decimal longitude,
            PropertyType propertyType, int pageNo = 0);

        /// <summary>
        /// Get the total number of houses with the given property type present in the database
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        Tuple<int,int> GetRecordCountByPropertyType(PropertyType propertyType);

        /// <summary>
        /// Get the total number of houses with the given location present in the database
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        Tuple<int, int> GetRecordCountByLocation(decimal latitude, decimal longitude);

        /// <summary>
        /// Get the total number of houses with the given email present in the database
        /// </summary>
        /// <returns></returns>
        Tuple<int, int> GetRecordCountByEmail(string email);

        /// <summary>
        /// Get the total number of houses with the given location  and PropertyType present in the database
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        Tuple<int, int> GetRecordCountByLocationAndPropertyType(decimal latitude, decimal longitude, PropertyType propertyType);

        /// <summary>
        /// Get the total number of houses present in the database
        /// </summary>
        /// <returns></returns>
        Tuple<int, int> GetTotalRecordCount();

        string Id { get; }
    }
}
