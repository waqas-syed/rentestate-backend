using System;
using System.Collections.Generic;
using RentStuff.Property.Domain.Model.HostelAggregate;
using RentStuff.Property.Domain.Model.HotelAggregate;
using RentStuff.Property.Domain.Model.HouseAggregate;

namespace RentStuff.Property.Domain.Model.PropertyAggregate
{
    /// <summary>
    /// Repository for saving and retreving House aggregate instances
    /// </summary>
    public interface IResidentialPropertyRepository
    {
        /// <summary>
        /// Saves new property or updates existing house
        /// </summary>
        /// <param name="property"></param>
        void SaveorUpdate(object property);
        
        /// <summary>
        /// Save or update dimension
        /// </summary>
        /// <param name="dimension"></param>
        void SaveorUpdateDimension(Dimension dimension);

        /// <summary>
        /// Deletes the Property object from the database
        /// </summary>
        /// <param name="house"></param>
        void Delete(object house);

        /// <summary>
        /// Gets the house by the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Property GetPropertyById(string id);

        /// <summary>
        /// Get the owner house by email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<House> GetHouseByOwnerEmail(string email, int pageNo = 0);

        /// <summary>
        /// Gets all the Hostels by the poster's email address
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<Hostel> GetHostelsByOwnerEmail(string email, int pageNo = 0);

        /// <summary>
        /// Get all the Hotels and Guest Houses by Owners email address
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<Hotel> GetHotelsByOwnerEmail(string email, int pageNo = 0);

        /// <summary>
        /// Get All houses
        /// </summary>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<House> GetAllHouses(int pageNo = 0);

        /// <summary>
        /// Search All Hostels
        /// </summary>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<Hostel> GetAllHostels(int pageNo = 0);

        /// <summary>
        /// Search all the hotels
        /// </summary>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<Hotel> GetAllHotels(int pageNo = 0);

        /// <summary>
        /// Gets the houses by coordinates and property type
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<House> SearchHousesByCoordinates(decimal latitude, decimal longitude, int pageNo = 0);

        /// <summary>
        /// Search Hostels in the radius sorrounding the given location
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<Hostel> SearchHostelByCoordinates(decimal latitude, decimal longitude, int pageNo = 0);

        /// <summary>
        /// Search Hotels in the radius sorrounding the given location
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<Hotel> SearchHotelByCoordinates(decimal latitude, decimal longitude, int pageNo = 0);

        /// <summary>
        /// Get the total number of houses with the given property type present in the database
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        Tuple<int,int> GetRecordCountByPropertyType(string propertyType);

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
        Tuple<int, int> GetRecordCountByLocationAndPropertyType(decimal latitude, decimal longitude, string propertyType);

        /// <summary>
        /// Get the total number of houses present in the database
        /// </summary>
        /// <returns></returns>
        Tuple<int, int> GetTotalRecordCount();
    }
}
