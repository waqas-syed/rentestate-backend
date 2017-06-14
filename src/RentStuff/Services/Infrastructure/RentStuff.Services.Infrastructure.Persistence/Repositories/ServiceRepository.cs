using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate;
using RentStuff.Services.Domain.Model.ServiceAggregate;

namespace RentStuff.Services.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repository for Services
    /// </summary>
    public class ServiceRepository : IServiceRepository
    {
        // The radius in kilometers from the location that was searched. We search within this radius for results
        // The formula is given here: https://developers.google.com/maps/articles/phpsqlsearch_v3
        // http://stackoverflow.com/questions/9686309/list-of-surrounding-towns-within-a-given-radius

        // The radius that we need to search in. Starting point is the location entered by the user
        private readonly int _radius = 35;
        private readonly int _resultsPerPage = 10;
        
        private ISession _session;

        public ServiceRepository(ISession session)
        {
            _session = session;
        }

        /// <summary>
        /// Save or update the service
        /// </summary>
        /// <param name="service"></param>
        public string SaveOrUpdate(Service service)
        {
            using (var transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                _session.SaveOrUpdate(service);
                transaction.Commit();
            }
            return service.Id;
        }

        /// <summary>
        /// Get the Service by it's ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Service GetServiceById(string id)
        {
            using (_session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return _session.QueryOver<Service>().Where(x => x.Id == id).SingleOrDefault();
            }
        }

        /// <summary>
        /// Get Services by Email
        /// </summary>
        /// <param name="uploaderEmail"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IList<Service> GetServicesByEmail(string uploaderEmail, int pageNo = 0)
        {
            using (_session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return _session.QueryOver<Service>()
                        .Where(x => x.UploaderEmail == uploaderEmail)
                        .Skip(pageNo*_resultsPerPage)
                        .Take(_resultsPerPage)
                        .List<Service>();
            }
        }

        /// <summary>
        /// Get Services by providing latitude and longitude
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IList<Service> GetServicesByLocation(decimal latitude, decimal longitude, int pageNo = 0)
        {
            using (_session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                // In the below formula, enter 3959 for miles; 6371 for kilometers
                IList houses =
                    _session.CreateSQLQuery(
                            "SELECT *, ( 6371 * acos( cos( radians(:inputLatitude) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians(:inputLongitude) ) + sin( radians(:inputLatitude) ) * sin( radians( latitude ) ) ) ) AS distance FROM service HAVING distance < :radius ORDER BY distance")
                        // LIMIT 0 , 20")//("SELECT name, latitude, longitude, ( 6371 * acos( cos( radians(:inputLatitude) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians(:inputLongitude) ) + sin( radians(:inputLatitude) ) * sin( radians( latitude ) ) ) ) AS distance FROM geo_location HAVING distance < 25 ORDER BY distance LIMIT 0 , 20")
                        .AddEntity(typeof(Service))
                        .SetParameter("inputLatitude", latitude)
                        .SetParameter("inputLongitude", longitude)
                        .SetParameter("radius", _radius)
                        .SetFirstResult(pageNo*_resultsPerPage)
                        .SetMaxResults(_resultsPerPage)
                        .List();

                return houses.Cast<Service>().ToList();
            }
        }

        /// <summary>
        /// Get Services by providing latitude and longitude and the ServiceProfessionType
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="serviceProfessionType"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IList<Service> GetServicesByLocationAndProfession(decimal latitude, decimal longitude,
            string serviceProfessionType, int pageNo = 0)
        {
            using (_session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                // In the below formula, enter 3959 for miles; 6371 for kilometers
                IList houses =
                    _session.CreateSQLQuery(
                            "SELECT *, ( 6371 * acos( cos( radians(:inputLatitude) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians(:inputLongitude) ) + sin( radians(:inputLatitude) ) * sin( radians( latitude ) ) ) ) AS distance FROM service HAVING distance < :radius AND service_profession_type=:serviceProfessionType ORDER BY distance")
                        // LIMIT 0 , 20")//("SELECT name, latitude, longitude, ( 6371 * acos( cos( radians(:inputLatitude) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians(:inputLongitude) ) + sin( radians(:inputLatitude) ) * sin( radians( latitude ) ) ) ) AS distance FROM geo_location HAVING distance < 25 ORDER BY distance LIMIT 0 , 20")
                        .AddEntity(typeof(Service))
                        .SetParameter("inputLatitude", latitude)
                        .SetParameter("inputLongitude", longitude)
                        .SetParameter("serviceProfessionType", serviceProfessionType)
                        .SetParameter("radius", _radius)
                        .SetFirstResult(pageNo*_resultsPerPage)
                        .SetMaxResults(_resultsPerPage)
                        .List();

                return houses.Cast<Service>().ToList();
            }
        }

        /// <summary>
        /// Get Services by providing ServiceProfessionType
        /// </summary>
        /// <param name="serviceProfessionType"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IList<Service> GetServicesByProfession(string serviceProfessionType, 
            int pageNo = 0)
        {
            using (_session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return _session
                    .QueryOver<Service>()
                    .Where(x => x.ServiceProfessionType == serviceProfessionType)
                    .Skip(pageNo * _resultsPerPage)
                    .Take(_resultsPerPage)
                    .List<Service>();
            }
        }

        /// <summary>
        /// Delete the service
        /// </summary>
        /// <param name="service"></param>
        public void DeleteService(Service service)
        {
            using (var transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                _session.Delete(service);
                transaction.Commit();
            }
        }
    }
}
