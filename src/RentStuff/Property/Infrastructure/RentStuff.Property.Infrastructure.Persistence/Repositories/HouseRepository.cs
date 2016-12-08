using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using RentStuff.Property.Domain.Model.HouseAggregate;
using Spring.Transaction;
using Spring.Transaction.Interceptor;
using NHibernate.Transform;

namespace RentStuff.Property.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// House Repository
    /// </summary>
    public class HouseRepository : NHibernateSessionFactory, IHouseRepository
    {
        private readonly int _radius = 30;

        /// <summary>
        /// Saves new House or updates existing house
        /// </summary>
        /// <param name="house"></param>
        [Transaction(TransactionPropagation.Required, ReadOnly = false)]
        public void SaveorUpdate(House house)
        {
            CurrentSession.SaveOrUpdate(house);
        }

        [Transaction(TransactionPropagation.Required, ReadOnly = false)]
        public void SaveorUpdateDimension(Dimension dimension)
        {
            CurrentSession.SaveOrUpdate(dimension);
        }

        [Transaction(TransactionPropagation.Required, ReadOnly = false)]
        public void Delete(House house)
        {
            CurrentSession.Delete(house);
        }

        /// <summary>
        /// Gets the house by the given id
        /// </summary>
        /// <param name="houseId"></param>
        /// <returns></returns>
        [Transaction]
        public House GetHouseById(string houseId)
        {
            return CurrentSession.QueryOver<House>().Where(x => x.Id == houseId).SingleOrDefault();
        }

        /// <summary>
        /// Get the owner house by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Transaction]
        public IList<House> GetHouseByOwnerEmail(string email)
        {
            return CurrentSession.QueryOver<House>().Where(x => x.OwnerEmail == email).List<House>();
        }

        /// <summary>
        /// Get the house by latitude and longitude
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        [Transaction]
        public IList<House> GetHouseByCoordinates(decimal latitude, decimal longitude)
        {
            return CurrentSession.QueryOver<House>().Where(x => x.Latitude == latitude).Where(x => x.Longitude == longitude).List<House>();
        }

        /// <summary>
        /// Get Location by Coordinates
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        [Transaction]
        public IList<House> SearchHousesByCoordinates(decimal latitude, decimal longitude)
        {
            IList houses = CurrentSession.CreateSQLQuery("SELECT *, ( 6371 * acos( cos( radians(:inputLatitude) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians(:inputLongitude) ) + sin( radians(:inputLatitude) ) * sin( radians( latitude ) ) ) ) AS distance FROM house HAVING distance < :radius ORDER BY distance LIMIT 0 , 20")//("SELECT name, latitude, longitude, ( 6371 * acos( cos( radians(:inputLatitude) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians(:inputLongitude) ) + sin( radians(:inputLatitude) ) * sin( radians( latitude ) ) ) ) AS distance FROM geo_location HAVING distance < 25 ORDER BY distance LIMIT 0 , 20")
                .AddEntity(typeof(House))
                //.AddScalar("latitude", NHibernateUtil.Decimal)
                //.AddScalar("longitude", NHibernateUtil.Decimal)
                //.AddScalar("distance", NHibernateUtil.Decimal)
                .SetParameter("inputLatitude", latitude)
                .SetParameter("inputLongitude", longitude)
                .SetParameter("radius", _radius)
                .List();

            return houses.Cast<House>().ToList();
        }

        /// <summary>
        /// Get all the houses
        /// </summary>
        /// <returns></returns>
        [Transaction]
        public IList<House> GetAllHouses()
        {
            return CurrentSession.QueryOver<House>().List<House>();
        }
    }
}
