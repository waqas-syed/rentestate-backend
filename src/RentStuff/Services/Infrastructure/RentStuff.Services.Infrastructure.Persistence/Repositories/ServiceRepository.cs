using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RentStuff.Common.NHibernate.Wrappers;
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
        
        private INhibernateSessionWrapper _sessionWrapper;

        public ServiceRepository(INhibernateSessionWrapper sessionWrapper)
        {
            _sessionWrapper = sessionWrapper;
        }

        /// <summary>
        /// Save or update the service
        /// </summary>
        /// <param name="service"></param>
        public string SaveOrUpdate(Service service)
        {
            using (var transaction = _sessionWrapper.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                _sessionWrapper.Session.SaveOrUpdate(service);
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
            using (_sessionWrapper.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return _sessionWrapper.Session.QueryOver<Service>().Where(x => x.Id == id).SingleOrDefault();
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
            using (_sessionWrapper.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return _sessionWrapper.Session.QueryOver<Service>()
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
            using (_sessionWrapper.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                // In the below formula, enter 3959 for miles; 6371 for kilometers
                IList houses =
                    _sessionWrapper.Session.CreateSQLQuery(
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
            using (_sessionWrapper.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                // In the below formula, enter 3959 for miles; 6371 for kilometers
                IList houses =
                    _sessionWrapper.Session.CreateSQLQuery(
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
            using (_sessionWrapper.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return _sessionWrapper.Session
                    .QueryOver<Service>()
                    .Where(x => x.ServiceProfessionType == serviceProfessionType)
                    .Skip(pageNo * _resultsPerPage)
                    .Take(_resultsPerPage)
                    .List<Service>();
            }
        }

        /// <summary>
        /// Gets all the services
        /// </summary>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IList<Service> GetAllServices(int pageNo = 0)
        {
            using (_sessionWrapper.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return _sessionWrapper.Session
                    .QueryOver<Service>()
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
            using (var transaction = _sessionWrapper.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                _sessionWrapper.Session.Delete(service);
                transaction.Commit();
            }
        }

        /// <summary>
        /// Get the total number of services with the given property type present in the database
        /// </summary>
        /// <param name="serviceProfessionType"></param>
        /// <returns></returns>
        public Tuple<int, int> GetRecordCountByProfessionType(string serviceProfessionType)
        {
            using (_sessionWrapper.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return new Tuple<int, int>(_sessionWrapper.Session
                                            .QueryOver<Service>()
                                            .Where(x => x.ServiceProfessionType == serviceProfessionType)
                                            .RowCount(),
                                            _resultsPerPage);
            }
        }

        /// <summary>
        /// Get the total number of services with the given location present in the database
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public Tuple<int, int> GetRecordCountByLocation(decimal latitude, decimal longitude)
        {
            using (_sessionWrapper.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return new Tuple<int, int>(
                         _sessionWrapper.Session.CreateSQLQuery(
                            "SELECT *, ( 6371 * acos( cos( radians(:inputLatitude) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians(:inputLongitude) ) + sin( radians(:inputLatitude) ) * sin( radians( latitude ) ) ) ) AS distance FROM service HAVING distance < :radius ORDER BY distance")
                        .AddEntity(typeof(Service))
                        .SetParameter("inputLatitude", latitude)
                        .SetParameter("inputLongitude", longitude)
                        .SetParameter("radius", _radius)
                        .List().Count, _resultsPerPage);
            }
        }

        /// <summary>
        /// Get the total number of services with the given email present in the database
        /// </summary>
        /// <returns></returns>
        public Tuple<int, int> GetRecordCountByEmail(string uploaderEmail)
        {
            using (_sessionWrapper.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return new Tuple<int, int>(_sessionWrapper.Session
                                            .QueryOver<Service>()
                                            .Where(x => x.UploaderEmail == uploaderEmail)
                                            .RowCount(),
                                            _resultsPerPage);
            }
        }

        /// <summary>
        /// Get the total number of services with the given location  and PropertyType present in the database
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="serviceProfessionType"></param>
        /// <returns></returns>
        public Tuple<int, int> GetRecordCountByLocationAndProfessionType(decimal latitude, decimal longitude, string serviceProfessionType)
        {
            using (_sessionWrapper.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return new Tuple<int, int>(
                        _sessionWrapper.Session.CreateSQLQuery(
                        "SELECT *, ( 6371 * acos( cos( radians(:inputLatitude) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians(:inputLongitude) ) + sin( radians(:inputLatitude) ) * sin( radians( latitude ) ) ) ) AS distance FROM service HAVING distance < :radius AND service_profession_type=:serviceProfessionType ORDER BY distance")
                        .AddEntity(typeof(Service))
                        .SetParameter("inputLatitude", latitude)
                        .SetParameter("inputLongitude", longitude)
                        .SetParameter("serviceProfessionType", serviceProfessionType)
                        .SetParameter("radius", _radius)
                        .List().Count,
                    _resultsPerPage);
            }
        }

        /// <summary>
        /// Get the total number of services present in the database
        /// </summary>
        /// <returns></returns>
        public Tuple<int, int> GetTotalRecordCount()
        {
            using (_sessionWrapper.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return new Tuple<int, int>(_sessionWrapper.Session
                                            .QueryOver<Service>()
                                            .RowCount(),
                                            _resultsPerPage);
            }
        }
    }
}
