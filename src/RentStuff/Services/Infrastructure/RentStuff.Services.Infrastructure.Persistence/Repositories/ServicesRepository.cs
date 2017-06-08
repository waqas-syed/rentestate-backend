﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate;
using RentStuff.Services.Domain.Model.ServiceAggregate;
using RentStuff.Services.Infrastructure.Persistence.NHibernateCompound;

namespace RentStuff.Services.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repository for Services
    /// </summary>
    public class ServicesRepository : IServicesRepository
    {
        // The radius in kilometers from the location that was searched. We search within this radius for results
        // The formula is given here: https://developers.google.com/maps/articles/phpsqlsearch_v3
        // http://stackoverflow.com/questions/9686309/list-of-surrounding-towns-within-a-given-radius

        // The radius that we need to search in. Starting point is the location entered by the user
        private readonly int _radius = 38;
        private readonly int _resultsPerPage = 10;
        
        private ISession _session;

        public ServicesRepository(ISession session)
        {
            _session = session;
        }

        /// <summary>
        /// Save or update the service
        /// </summary>
        /// <param name="service"></param>
        public void SaveOrUpdate(Service service)
        {
            using (var transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                _session.SaveOrUpdate(service);
                transaction.Commit();
            }
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
            ServiceProfessionType serviceProfessionType, int pageNo = 0)
        {
            return null;
        }

        /// <summary>
        /// Get Services by providing ServiceProfessionType
        /// </summary>
        /// <param name="serviceProfessionType"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IList<Service> GetServicesByProfession(ServiceProfessionType serviceProfessionType, 
            int pageNo = 0)
        {
            return null;
        }

        /// <summary>
        /// Delete the service
        /// </summary>
        /// <param name="id"></param>
        public void DeleteService(string id)
        {
        }
    }
}
