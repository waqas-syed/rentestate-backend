using System;
using System.Collections.Generic;
using RentStuff.Property.Domain.Model.HostelAggregate;
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
        ResidentialProperty GetPropertyById(string id);

        /// <summary>
        /// Get the owner house by email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<House> GetHouseByOwnerEmail(string email, int pageNo = 0);
        
        /// <summary>
        /// Get houses with reference to their PropertyType
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<House> SearchHousesByPropertyType(string propertyType, int pageNo = 0);

        /// <summary>
        /// Gets the houses by coordinates and property type
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="propertyType"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<House> SearchHousesByCoordinatesAndPropertyType(decimal latitude, decimal longitude,
            string propertyType, int pageNo = 0);

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
