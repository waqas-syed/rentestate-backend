using System.Collections.Generic;

namespace RentStuff.Services.Domain.Model.ServiceAggregate
{
    /// <summary>
    /// Interface for ServicesRepository
    /// </summary>
    public interface IServicesRepository
    {
        /// <summary>
        /// Save or update the service
        /// </summary>
        /// <param name="service"></param>
        void SaveOrUpdate(Service service);
        
        /// <summary>
        /// Get the Service by it's ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Service GetServiceById(string id);

        /// <summary>
        /// Get Services by providing latitude and longitude
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<Service> GetServicesByLocation(decimal latitude, decimal longitude, int pageNo = 0);

        /// <summary>
        /// Get Services by providing latitude and longitude and the ServiceProfessionType
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="serviceProfessionType"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<Service> GetServicesByLocationAndProfession(decimal latitude, decimal longitude, 
            ServiceProfessionType serviceProfessionType, int pageNo = 0);

        /// <summary>
        /// Get Services by providing ServiceProfessionType
        /// </summary>
        /// <param name="serviceProfessionType"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<Service> GetServicesByProfession(ServiceProfessionType serviceProfessionType, int pageNo = 0);

        /// <summary>
        /// Delete the service
        /// </summary>
        /// <param name="id"></param>
        void DeleteService(string id);

        void Commit();
    }
}
