using System;
using System.Collections.Generic;

namespace RentStuff.Services.Domain.Model.ServiceAggregate
{
    /// <summary>
    /// Interface for ServicesRepository
    /// </summary>
    public interface IServiceRepository
    {
        /// <summary>
        /// Save or update the service
        /// </summary>
        /// <param name="service"></param>
        string SaveOrUpdate(Service service);
        
        /// <summary>
        /// Get the Service by it's ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Service GetServiceById(string id);

        /// <summary>
        /// Get Services by Email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<Service> GetServicesByEmail(string email, int pageNo = 0);

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
            string serviceProfessionType, int pageNo = 0);

        /// <summary>
        /// Get Services by providing ServiceProfessionType
        /// </summary>
        /// <param name="serviceProfessionType"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<Service> GetServicesByProfession(string serviceProfessionType, int pageNo = 0);

        /// <summary>
        /// Gets all the services
        /// </summary>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IList<Service> GetAllServices(int pageNo = 0);

        /// <summary>
        /// Delete the service
        /// </summary>
        /// <param name="service"></param>
        void DeleteService(Service service);

        /// <summary>
        /// Get the total number of services with the given property type present in the database
        /// </summary>
        /// <param name="serviceProfessionType"></param>
        /// <returns></returns>
        Tuple<int, int> GetRecordCountByProfessionType(string serviceProfessionType);

        /// <summary>
        /// Get the total number of services with the given location present in the database
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        Tuple<int, int> GetRecordCountByLocation(decimal latitude, decimal longitude);

        /// <summary>
        /// Get the total number of services with the given email present in the database
        /// </summary>
        /// <returns></returns>
        Tuple<int, int> GetRecordCountByEmail(string email);

        /// <summary>
        /// Get the total number of services with the given location and PropertyType present in the database
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="serviceProfessionType"></param>
        /// <returns></returns>
        Tuple<int, int> GetRecordCountByLocationAndProfessionType(decimal latitude, decimal longitude,
            string serviceProfessionType);

        /// <summary>
        /// Get the total number of services present in the database
        /// </summary>
        /// <returns></returns>
        Tuple<int, int> GetTotalRecordCount();
    }
}
