using System.Collections.Generic;
using System.IO;
using RentStuff.Services.Application.ApplicationServices.Commands;
using RentStuff.Services.Application.ApplicationServices.Representations;

namespace RentStuff.Services.Application.ApplicationServices
{
    /// <summary>
    /// Service Application level interface
    /// </summary>
    public interface IServiceApplicationService
    {
        /// <summary>
        /// Save the new given Service
        /// </summary>
        /// <param name="createServiceCommand"></param>
        string SaveNewService(CreateServiceCommand createServiceCommand);

        /// <summary>
        /// Update the given Service
        /// </summary>
        /// <param name="updateServiceCommand"></param>
        void UpdateService(UpdateServiceCommand updateServiceCommand);

        /// <summary>
        /// Add a new review to the given Service
        /// </summary>
        /// <param name="addReviewCommand"></param>
        void AddReview(AddReviewCommand addReviewCommand);
        
        /// <summary>
        /// Get the Service by Id
        /// </summary>
        /// <param name="id"></param>
        ServiceFullRepresentation GetServiceById(string id);

        /// <summary>
        /// Get the Services given the uploader's email
        /// </summary>
        /// <param name="uploaderEmail"></param>
        /// <param name="pageNo"></param>
        IList<ServicePartialRepresentation> GetServicesByUploaderEmail(string uploaderEmail, int pageNo = 0);

        /// <summary>
        /// Search the service by the given Location
        /// </summary>
        /// <param name="location"></param>
        /// <param name="pageNo"></param>
        IList<ServicePartialRepresentation> SearchServicesByLocation(string location, int pageNo = 0);

        /// <summary>
        /// Search the service by the given Location and ServiceProfesionType
        /// </summary>
        /// <param name="location"></param>
        /// <param name="serviceProfessionType"></param>
        /// <param name="pageNo"></param>
        IList<ServicePartialRepresentation> SearchServicesByLocationAndProfession(string location, 
            string serviceProfessionType, int pageNo = 0);

        /// <summary>
        /// Search the service by the given ServiceProfesionType
        /// </summary>
        /// <param name="serviceProfessionType"></param>
        /// <param name="pageNo"></param>
        IList<ServicePartialRepresentation> SearchServicesByProfession(string serviceProfessionType, int pageNo = 0);

        /// <summary>
        /// Gets all the services present in the database
        /// </summary>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<ServicePartialRepresentation> GetAllServices(int pageNo = 0);

        /// <summary>
        /// Adds the given image to this Service
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="photoStream"></param>
        void AddSingleImageToService(string serviceId, Stream photoStream);

        /// <summary>
        /// Delete the Service with the given ID
        /// </summary>
        /// <param name="serviceId"></param>
        void DeleteService(string serviceId);

        /// <summary>
        /// Delete the provided images from the given Service
        /// </summary>
        void DeleteImagesFromService(string serviceId, IList<string> images);

        /// <summary>
        /// Check that the current HTTP request initiator's email is the same as the provided email
        /// </summary>
        /// <param name="currentUserEmail"></param>
        /// <param name="uploaderEmail"></param>
        void NewServiceEmailOwnershipCheck(string currentUserEmail, string uploaderEmail);

        /// <summary>
        ///  Checks that the Service to be modified belongs to the current user
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="currentUserEmail"></param>
        /// <returns></returns>
        bool ServiceOwnershipCheck(string serviceId, string currentUserEmail);

        /// <summary>
        /// Get the count of records using the given criteria
        /// </summary>
        /// <param name="serviceProfessionType"></param>
        /// <param name="location"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        ServiceCountRepresentation GetServicesCount(string serviceProfessionType, string location,
            string email);

        /// <summary>
        /// Returns all the Service profession types that are provided by our app
        /// </summary>
        /// <returns></returns>
        IReadOnlyDictionary<string, IReadOnlyList<string>> GetAllServiceProfessionTypes();
    }
}
