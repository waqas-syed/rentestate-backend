using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using RentStuff.Property.Domain.Model.HouseAggregate;
using Spring.Transaction;
using Spring.Transaction.Interceptor;

namespace RentStuff.Property.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Location repository
    /// </summary>
    public class LocationRepository : NHibernateSessionFactory, ILocationRepository
    {
        private readonly int _radius = 30;
        /// <summary>
        /// Saves or Updates the Location instance
        /// </summary>
        /// <param name="location"></param>
        [Transaction(TransactionPropagation.Required, ReadOnly = false)]
        public void SaveOrUpdate(Location location)
        {
            CurrentSession.SaveOrUpdate(location);
        }

        /// <summary>
        /// Get Location by Coordinates
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        [Transaction]
        public IList GetLocationByCoordinates(decimal latitude, decimal longitude)
        {
            return CurrentSession.CreateSQLQuery("SELECT , ( 6371 * acos( cos( radians(:inputLatitude) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians(:inputLongitude) ) + sin( radians(:inputLatitude) ) * sin( radians( latitude ) ) ) ) AS distance FROM location HAVING distance < :radius ORDER BY distance LIMIT 0 , 20")//("SELECT name, latitude, longitude, ( 6371 * acos( cos( radians(:inputLatitude) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians(:inputLongitude) ) + sin( radians(:inputLatitude) ) * sin( radians( latitude ) ) ) ) AS distance FROM geo_location HAVING distance < 25 ORDER BY distance LIMIT 0 , 20")
                .AddEntity(typeof(Location))
                //.AddScalar("latitude", NHibernateUtil.Decimal)
                //.AddScalar("longitude", NHibernateUtil.Decimal)
                .SetParameter("inputLatitude", latitude)
                .SetParameter("inputLongitude", longitude)
                .SetParameter("radius", _radius)
                .List();

            return CurrentSession.CreateSQLQuery("SELECT name,latitude,longitude, ( 6371 * acos( cos( radians(:inputLatitude) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians(:inputLongitude) ) + sin( radians(:inputLatitude) ) * sin( radians( latitude ) ) ) ) AS distance FROM geo_location HAVING distance < :radius ORDER BY distance LIMIT 0 , 20")//("SELECT name, latitude, longitude, ( 6371 * acos( cos( radians(:inputLatitude) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians(:inputLongitude) ) + sin( radians(:inputLatitude) ) * sin( radians( latitude ) ) ) ) AS distance FROM geo_location HAVING distance < 25 ORDER BY distance LIMIT 0 , 20")
                .AddScalar("latitude", NHibernateUtil.Decimal)
                .AddScalar("longitude", NHibernateUtil.Decimal)
                .AddScalar("distance", NHibernateUtil.Decimal)
                .AddScalar("name", NHibernateUtil.String)
                .SetParameter("inputLatitude", latitude)
                .SetParameter("inputLongitude", longitude)
                .SetParameter("radius", _radius)
                .List();
        }
    }
}
