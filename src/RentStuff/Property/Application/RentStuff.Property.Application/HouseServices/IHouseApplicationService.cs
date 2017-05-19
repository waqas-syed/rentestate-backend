using System.Collections.Generic;
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
        string SaveNewHouseOffer(CreateHouseCommand house);

        /// <summary>
        /// Update an existing house
        /// </summary>
        /// <param name="updateHouseCommand"></param>
        /// <returns></returns>
        bool UpdateHouse(UpdateHouseCommand updateHouseCommand);

        /// <summary>
        /// Delete the given house instance
        /// </summary>
        /// <param name="houseId"></param>
        void DeleteHouse(string houseId);
        
        /// <summary>
        /// Get House by it's ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        HouseFullRepresentation GetHouseById(string id);

        /// <summary>
        /// Gets the house by providing the owner's email id
        /// </summary>
        /// <returns></returns>
        IList<HousePartialRepresentation> GetHouseByEmail(string email, int pageNo = 0);

        /// <summary>
        /// Search nearby houses by providing the address
        /// </summary>
        /// <param name="address"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<HousePartialRepresentation> SearchHousesByArea(string address, int pageNo = 0);

        /// <summary>
        /// Searches the houses with reference to area
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<HousePartialRepresentation> SearchHousesByPropertyType(string propertyType, int pageNo = 0);

        /// <summary>
        /// Search the house by providing the address and propertyType
        /// </summary>
        /// <param name="area"></param>
        /// <param name="propertyType"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<HousePartialRepresentation> SearchHousesByAreaAndPropertyType(string area, string propertyType, 
            int pageNo = 0);


        /// <summary>
        /// Get the number of records in the database for the given criteria
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        HouseCountRepresentation GetRecordsCount(string propertyType, string location);

        /// <summary>
        /// Get all houses
        /// </summary>
        /// <returns></returns>
        IList<HousePartialRepresentation> GetAllHouses(int pageNo = 0);

        /// <summary>
        /// Get the types of property avaialable on our partal: House, Apartment, Hostel, Room
        /// </summary>
        /// <returns></returns>
        IList<string> GetPropertyTypes();

        /// <summary>
        /// Add images to an existing House instance
        /// </summary>
        void AddImagesToHouse(string houseId, IList<string> imagesList);

        /// <summary>
        /// Deletes the image from the house
        /// </summary>
        /// <param name="houseId"></param>
        /// <param name="imagesList"></param>
        /// <returns></returns>
        void DeleteImageFromHouse(string houseId, IList<string> imagesList);

        /// <summary>
        /// Checks that the call requester willing to update the house is the actual poster of the house by comparing emails
        /// </summary>
        /// <param name="houseId"></param>
        /// <param name="requesterEmail"></param>
        /// <returns></returns>
        bool HouseOwnershipEmailCheck(string houseId, string requesterEmail);
    }
}
