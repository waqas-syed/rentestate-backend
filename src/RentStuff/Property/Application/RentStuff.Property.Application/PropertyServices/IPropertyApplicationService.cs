using System.Collections.Generic;
using System.IO;
using RentStuff.Property.Application.PropertyServices.Commands.UpdateCommands;
using RentStuff.Property.Application.PropertyServices.Representation;
using RentStuff.Property.Application.PropertyServices.Representation.AbstractRepresentations;

namespace RentStuff.Property.Application.PropertyServices
{
    /// <summary>
    /// Interface for House aggregate related operations
    /// </summary>
    public interface IPropertyApplicationService
    {
        /// <summary>
        /// Saves a new house instance to the database
        /// </summary>
        string SaveNewProperty(object propertyBaseCommand, string currentUserEmail);

        /// <summary>
        /// Update an existing house
        /// </summary>
        /// <param name="propertyJson"></param>
        /// <param name="currentUserEmail"></param>
        /// <returns></returns>
        void UpdateProperty(object propertyJson, string currentUserEmail);

        /// <summary>
        /// Delete the given house instance
        /// </summary>
        /// <param name="houseId"></param>
        /// <param name="currentUserEmail"></param>
        void DeleteHouse(string houseId, string currentUserEmail);

        /// <summary>
        /// Get House by it's ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        PropertyBaseRepresentation GetPropertyById(string id, string propertyType);

        /// <summary>
        /// Gets the house by providing the owner's email id
        /// </summary>
        /// <returns></returns>
        IList<ResidentialPropertyPartialBaseImplementation> GetPropertiesByEmail(string email, int pageNo = 0);
        
        /// <summary>
        /// Searches the houses with reference to area
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<ResidentialPropertyPartialBaseImplementation> SearchPropertiesByPropertyType(string propertyType, int pageNo = 0);

        /// <summary>
        /// Search the house by providing the address and propertyType
        /// </summary>
        /// <param name="area"></param>
        /// <param name="propertyType"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<ResidentialPropertyPartialBaseImplementation> SearchPropertiesByAreaAndPropertyType(string area, string propertyType, 
            int pageNo = 0);


        /// <summary>
        /// Get the number of records in the database for the given criteria
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="location"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        HouseCountRepresentation GetRecordsCount(string propertyType, string location, string email);
        
        /// <summary>
        /// Get the types of property avaialable on our partal: House, Apartment, Hostel, Room
        /// </summary>
        /// <returns></returns>
        IList<string> GetPropertyTypes();

        /// <summary>
        /// Adds a single image to a house
        /// </summary>
        /// <param name="houseId"></param>
        /// <param name="photoStream"></param>
        void AddSingleImageToHouse(string houseId, Stream photoStream);
        
        /// <summary>
        /// Deletes the image from the house
        /// </summary>
        /// <param name="houseId"></param>
        /// <param name="imagesList"></param>
        /// <returns></returns>
        void DeleteImagesFromHouse(string houseId, IList<string> imagesList);

        /// <summary>
        /// Checks that the call requester willing to update the house is the actual poster of the house by comparing emails
        /// </summary>
        /// <param name="houseId"></param>
        /// <param name="requesterEmail"></param>
        /// <returns></returns>
        bool HouseOwnershipCheck(string houseId, string requesterEmail);

        /// <summary>
        /// Get all the Rent Units, i.e., Month, Week, Day, Hour
        /// </summary>
        /// <returns></returns>
        IList<string> GetAllRentUnits();
    }
}
