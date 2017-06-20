using System.Collections;
using NHibernate;
using RentStuff.Property.Domain.Model.HouseAggregate;

namespace RentStuff.Property.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Location repository
    /// </summary>
    public class LocationRepository : ILocationRepository
    {
        private readonly int _radius = 30;
        private ISession _session;

        /// <summary>
        /// Saves or Updates the Location instance
        /// </summary>
        /// <param name="location"></param>
        //[Transaction(TransactionPropagation.Required, ReadOnly = false)]
        public void SaveOrUpdate(Location location)
        {
            _session.SaveOrUpdate(location);
        }

        /// <summary>
        /// Get Location by Coordinates
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        //[Transaction]
        public IList GetLocationByCoordinates(decimal latitude, decimal longitude)
        {
            return _session.CreateSQLQuery("SELECT , ( 6371 * acos( cos( radians(:inputLatitude) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians(:inputLongitude) ) + sin( radians(:inputLatitude) ) * sin( radians( latitude ) ) ) ) AS distance FROM location HAVING distance < :radius ORDER BY distance LIMIT 0 , 20")//("SELECT name, latitude, longitude, ( 6371 * acos( cos( radians(:inputLatitude) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians(:inputLongitude) ) + sin( radians(:inputLatitude) ) * sin( radians( latitude ) ) ) ) AS distance FROM geo_location HAVING distance < 25 ORDER BY distance LIMIT 0 , 20")
                .AddEntity(typeof(Location))
                //.AddScalar("latitude", NHibernateUtil.Decimal)
                //.AddScalar("longitude", NHibernateUtil.Decimal)
                .SetParameter("inputLatitude", latitude)
                .SetParameter("inputLongitude", longitude)
                .SetParameter("radius", _radius)
                .List();
            
        }
    }
}
