using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RentStuff.Common.NHibernate.Wrappers;
using RentStuff.Property.Domain.Model.HouseAggregate;

//using Spring.Transaction;
//using Spring.Transaction.Interceptor;

namespace RentStuff.Property.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// House Repository
    /// </summary>
    public class HouseRepository : IHouseRepository
    {
        // The radius in kilometers from the location that was searched. We search within this radius for results
        // The formula is given here: https://developers.google.com/maps/articles/phpsqlsearch_v3
        // http://stackoverflow.com/questions/9686309/list-of-surrounding-towns-within-a-given-radius
        private readonly int _radius = 10;
        private readonly int _resultsPerPage = 10;
        private INhibernateSessionWrapper _session;

        public HouseRepository(INhibernateSessionWrapper nhibernateSessionWrapper)
        {
            _session = nhibernateSessionWrapper;
        }

        /// <summary>
        /// Saves new House or updates existing house
        /// </summary>
        /// <param name="house"></param>
        //[Transaction(TransactionPropagation.Required, ReadOnly = false)]
        public void SaveorUpdate(House house)
        {
            using (var transaction = _session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                _session.Session.SaveOrUpdate(house);
                transaction.Commit();
            }
        }

        //[Transaction(TransactionPropagation.Required, ReadOnly = false)]
        public void SaveorUpdateDimension(Dimension dimension)
        {
            using (var transaction = _session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                _session.Session.SaveOrUpdate(dimension);
                transaction.Commit();
            }
            
        }

       // [Transaction(TransactionPropagation.Required, ReadOnly = false)]
        public void Delete(House house)
        {
            using (var transaction = _session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                _session.Session.Delete(house);
                transaction.Commit();
            }
        }

        /// <summary>
        /// Gets the house by the given id
        /// </summary>
        /// <param name="houseId"></param>
        /// <returns></returns>
        //[Transaction]
        public House GetHouseById(string houseId)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return _session.Session.QueryOver<House>().Where(x => x.Id == houseId).SingleOrDefault();
            }
        }

        /// <summary>
        /// Get the owner house by email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        //[Transaction]
        public IList<House> GetHouseByOwnerEmail(string email, int pageNo = 0)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return _session.Session.QueryOver<House>()
                    .Where(x => x.OwnerEmail == email)
                    .Skip(pageNo*_resultsPerPage)
                    .Take(_resultsPerPage)
                    .List<House>();
            }
        }

        /// <summary>
        /// Get the house by latitude and longitude
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        //[Transaction]
        public IList<House> GetHouseByCoordinates(decimal latitude, decimal longitude)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return _session.Session.QueryOver<House>()
                        .Where(x => x.Latitude == latitude)
                        .Where(x => x.Longitude == longitude)
                        .List<House>();
            }
        }

        /// <summary>
        /// Gets houses with reference to their PropertyType
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        //[Transaction]
        public IList<House> SearchHousesByPropertyType(PropertyType propertyType, int pageNo = 0)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return _session.Session.QueryOver<House>().Where(x => x.PropertyType == propertyType)
                        .Skip(pageNo*_resultsPerPage)
                        .Take(_resultsPerPage)
                        .List<House>();
            }
            /*return CurrentSession.CreateCriteria(typeof(House))
            .Add(Restrictions.Eq("PropertyType", propertyType))
            .SetFirstResult(pageNo * 10)
            .SetMaxResults(10)
            .Future<House>().ToList();*/
        }

        /// <summary>
        /// Get Location by Coordinates
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
       // [Transaction]
        public IList<House> SearchHousesByCoordinates(decimal latitude, decimal longitude, int pageNo = 0)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                IList houses =
                    _session.Session.CreateSQLQuery("SELECT *, ( 6371 * acos( cos( radians(:inputLatitude) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians(:inputLongitude) ) + sin( radians(:inputLatitude) ) * sin( radians( latitude ) ) ) ) AS distance FROM house HAVING distance < :radius ORDER BY distance")
                        // LIMIT 0 , 20")//("SELECT name, latitude, longitude, ( 6371 * acos( cos( radians(:inputLatitude) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians(:inputLongitude) ) + sin( radians(:inputLatitude) ) * sin( radians( latitude ) ) ) ) AS distance FROM geo_location HAVING distance < 25 ORDER BY distance LIMIT 0 , 20")
                        .AddEntity(typeof(House))
                        .SetParameter("inputLatitude", latitude)
                        .SetParameter("inputLongitude", longitude)
                        .SetParameter("radius", _radius)
                        .SetFirstResult(pageNo*_resultsPerPage)
                        .SetMaxResults(_resultsPerPage)
                        .List();

                return houses.Cast<House>().ToList();
            }
        }

        /// <summary>
        /// Searches houses by coordinates and given propertytype
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="propertyType"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        //[Transaction]
        public IList<House> SearchHousesByCoordinatesAndPropertyType(decimal latitude, decimal longitude,
            PropertyType propertyType, int pageNo = 0)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                IList houses = _session.Session.CreateSQLQuery(
                            "SELECT *, ( 6371 * acos( cos( radians(:inputLatitude) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians(:inputLongitude) ) + sin( radians(:inputLatitude) ) * sin( radians( latitude ) ) ) ) AS distance FROM house HAVING distance < :radius AND property_type=:propertyType ORDER BY distance")
                        .AddEntity(typeof(House))
                        //.AddScalar("latitude", NHibernateUtil.Decimal)
                        //.AddScalar("longitude", NHibernateUtil.Decimal)
                        //.AddScalar("distance", NHibernateUtil.Decimal)
                        .SetParameter("inputLatitude", latitude)
                        .SetParameter("inputLongitude", longitude)
                        .SetParameter("propertyType", propertyType)
                        .SetParameter("radius", _radius)
                        .SetFirstResult(pageNo*_resultsPerPage)
                        .SetMaxResults(_resultsPerPage)
                        .List();

                return houses.Cast<House>().ToList();
            }
        }

        /// <summary>
        /// Get all the houses
        /// </summary>
        /// <returns></returns>
        //[Transaction]
        public IList<House> GetAllHouses(int pageNo = 0)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return _session
                        .Session.QueryOver<House>()
                        .Skip(pageNo*_resultsPerPage)
                        .Take(_resultsPerPage)
                        .List<House>();
            }
        }

        /// <summary>
        /// Gets the number of records for the given criteria in the database
        /// Item 1: RecordCount 
        /// Item 2: Items Per Page
        /// </summary>
        /// <returns></returns>
        //[Transaction]
        public Tuple<int, int> GetRecordCountByPropertyType(PropertyType propertyType)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return new Tuple<int, int>(
                    _session.Session
                    .QueryOver<House>()
                    .Where(x => x.PropertyType == propertyType)
                    .RowCount(),
                    _resultsPerPage);
            }
        }

        /// <summary>
        /// Get the total number of houses with the given location present in the database
        /// Item 1: RecordCount 
        /// Item 2: Items Per Page
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        //[Transaction]
        public Tuple<int, int> GetRecordCountByLocation(decimal latitude, decimal longitude)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return new Tuple<int, int>(_session.Session.CreateSQLQuery(
                        "SELECT *, ( 6371 * acos( cos( radians(:inputLatitude) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians(:inputLongitude) ) + sin( radians(:inputLatitude) ) * sin( radians( latitude ) ) ) ) AS distance FROM house HAVING distance < :radius ORDER BY distance")
                        // LIMIT 0 , 20")//("SELECT name, latitude, longitude, ( 6371 * acos( cos( radians(:inputLatitude) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians(:inputLongitude) ) + sin( radians(:inputLatitude) ) * sin( radians( latitude ) ) ) ) AS distance FROM geo_location HAVING distance < 25 ORDER BY distance LIMIT 0 , 20")
                        .AddEntity(typeof(House))
                        .SetParameter("inputLatitude", latitude)
                        .SetParameter("inputLongitude", longitude)
                        .SetParameter("radius", _radius)
                        .List().Count
                    , _resultsPerPage);
            }
        }

        /// <summary>
        /// Get the total number of houses with the given email present in the database
        /// </summary>
        /// /// <param name="email"></param>
        /// <returns></returns>
        //[Transaction]
        public Tuple<int, int> GetRecordCountByEmail(string email)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return new Tuple<int, int>(_session.Session
                                            .QueryOver<House>()
                                            .Where(x => x.OwnerEmail == email)
                                            .RowCount(), _resultsPerPage);
            }
        }

        /// <summary>
        /// Get the total number of houses with the given location  and PropertyType present in the database
        /// Item 1: RecordCount 
        /// Item 2: Items Per Page
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        //[Transaction]
        public Tuple<int, int> GetRecordCountByLocationAndPropertyType(decimal latitude, decimal longitude, PropertyType propertyType)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return new Tuple<int, int>(
                        _session.Session.CreateSQLQuery(
                        "SELECT *, ( 6371 * acos( cos( radians(:inputLatitude) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians(:inputLongitude) ) + sin( radians(:inputLatitude) ) * sin( radians( latitude ) ) ) ) AS distance FROM house HAVING distance < :radius AND property_type=:propertyType ORDER BY distance")
                        .AddEntity(typeof(House))
                        .SetParameter("inputLatitude", latitude)
                        .SetParameter("inputLongitude", longitude)
                        .SetParameter("propertyType", propertyType)
                        .SetParameter("radius", _radius)
                        .List().Count, _resultsPerPage);
            }
        }

        /// <summary>
        /// Get the total number of houses present in the database
        /// Item 1: RecordCount 
        /// Item 2: Items Per Page
        /// </summary>
        /// <returns></returns>
        //[Transaction]
        public Tuple<int, int> GetTotalRecordCount()
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return new Tuple<int, int>(_session.Session.QueryOver<House>().RowCount(), _resultsPerPage);
            }
        }
    }
}
