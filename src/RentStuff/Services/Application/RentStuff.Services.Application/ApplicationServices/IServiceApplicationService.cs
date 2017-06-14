using System.Collections.Generic;
using RentStuff.Services.Application.Commands;
using RentStuff.Services.Application.Representations;

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
        void SaveNewService(CreateServiceCommand createServiceCommand);

        /// <summary>
        /// Update the given Service
        /// </summary>
        /// <param name="updateServiceCommand"></param>
        void UpdateService(UpdateServiceCommand updateServiceCommand);

        void AddImageToService(string service, IList<string> iamges);

        /// <summary>
        /// Get the Service by Id
        /// </summary>
        /// <param name="id"></param>
        void GetServiceById(string id);

        /// <summary>
        /// Get the Services given the uploader's email
        /// </summary>
        /// <param name="uploaderEmail"></param>
        ServicePartialRepresentation GetServicesByUploaderEmail(string uploaderEmail);

        /// <summary>
        /// Search the service by the given Location
        /// </summary>
        /// <param name="location"></param>
        /// <param name="pageNo"></param>
        ServicePartialRepresentation SearchServicesByLocation(string location, int pageNo = 0);

        /// <summary>
        /// Search the service by the given Location and ServiceProfesionType
        /// </summary>
        /// <param name="location"></param>
        /// <param name="serviceProfessionType"></param>
        /// <param name="pageNo"></param>
        ServicePartialRepresentation SearchServicesByLocationAndProfession(string location, string serviceProfessionType, 
            int pageNo = 0);

        /// <summary>
        /// Search the service by the given ServiceProfesionType
        /// </summary>
        /// <param name="serviceProfessionType"></param>
        /// <param name="pageNo"></param>
        ServicePartialRepresentation SearchServicesByProfession(string serviceProfessionType, int pageNo = 0);

        /// <summary>
        /// Delete the Service with the given ID
        /// </summary>
        /// <param name="serviceId"></param>
        void DeleteService(string serviceId);

        /// <summary>
        /// Delete the provided images from the given Service
        /// </summary>
        void DeleteImagesFromService(string serviceId, List<string> images);
    }
}
