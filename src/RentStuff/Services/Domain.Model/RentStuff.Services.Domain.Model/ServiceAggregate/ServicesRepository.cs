
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
        /// Get the Services by their Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IList<Service> GetServiceByName(string name);

        /// <summary>
        /// Delete the service
        /// </summary>
        /// <param name="id"></param>
        void DeleteService(string id);
    }
}
