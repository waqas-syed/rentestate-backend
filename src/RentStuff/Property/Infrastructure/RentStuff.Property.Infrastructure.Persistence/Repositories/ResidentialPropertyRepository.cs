using RentStuff.Common.NHibernate.Wrappers;
using RentStuff.Property.Domain.Model.HouseAggregate;
using RentStuff.Property.Domain.Model.PropertyAggregate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RentStuff.Common.Utilities;
using RentStuff.Property.Domain.Model.HostelAggregate;
using RentStuff.Property.Domain.Model.HotelAggregate;

namespace RentStuff.Property.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// House Repository
    /// </summary>
    public class ResidentialPropertyRepository : IResidentialPropertyRepository
    {
        // The radius in kilometers from the location that was searched. We search within this radius for results
        // The formula is given here: https://developers.google.com/maps/articles/phpsqlsearch_v3
        // http://stackoverflow.com/questions/9686309/list-of-surrounding-towns-within-a-given-radius
        private readonly int _radius = 40;
        private readonly int _resultsPerPage = 10;
        private INhibernateSessionWrapper _session;

        public ResidentialPropertyRepository(INhibernateSessionWrapper nhibernateSessionWrapper)
        {
            _session = nhibernateSessionWrapper;
        }

        /// <summary>
        /// Saves new Property or updates existing property
        /// </summary>
        /// <param name="property"></param>
        public void SaveorUpdate(object property)
        {
            using (var transaction = _session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                _session.Session.SaveOrUpdate(property);
                transaction.Commit();
            }
        }

        /// <summary>
        /// Save or update the Dimension instance, which resides within the aggregate limits of the House Entity
        /// </summary>
        /// <param name="dimension"></param>
        public void SaveorUpdateDimension(Dimension dimension)
        {
            using (var transaction = _session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                _session.Session.SaveOrUpdate(dimension);
                transaction.Commit();
            }
            
        }
        
        /// <summary>
        /// Delete the house entity
        /// </summary>
        /// <param name="house"></param>
        public void Delete(object house)
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
        public Domain.Model.PropertyAggregate.Property GetPropertyById(string houseId)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return _session.Session.QueryOver<ResidentialProperty>().Where(x => x.Id == houseId).SingleOrDefault();
            }
        }
        
        /// <summary>
        /// Get the owner house by email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        //[Transaction]
        public IList<Domain.Model.PropertyAggregate.Property> GetHouseByOwnerEmail(string email, int pageNo = 0)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return _session.Session.QueryOver<Domain.Model.PropertyAggregate.Property>()
                    .Where(x => x.OwnerEmail == email)
                    .Skip(pageNo*_resultsPerPage)
                    .Take(_resultsPerPage)
                    .List<Domain.Model.PropertyAggregate.Property>();
            }
        }

        /// <summary>
        /// Get the owner house by email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        //[Transaction]
        public IList<House> GetApartmentByOwnerEmail(string email, int pageNo = 0)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return _session.Session.QueryOver<House>()
                    .Where(x => x.OwnerEmail == email && x.PropertyType == Constants.Apartment)
                    .Skip(pageNo * _resultsPerPage)
                    .Take(_resultsPerPage)
                    .List<House>();
            }
        }

        /// <summary>
        /// Get all the Hostels by the poster's email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IList<Hostel> GetHostelsByOwnerEmail(string email, int pageNo = 0)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return _session.Session.QueryOver<Hostel>()
                    .Where(x => x.OwnerEmail == email && x.PropertyType == Constants.Hostel)
                    .Skip(pageNo * _resultsPerPage)
                    .Take(_resultsPerPage)
                    .List<Hostel>();
            }
        }

        /// <summary>
        /// Get all the Hotels and Guest Houses by the poster's email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IList<Hotel> GetHotelsByOwnerEmail(string email, int pageNo = 0)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return _session.Session.QueryOver<Hotel>()
                    .Where(x => x.OwnerEmail == email && x.PropertyType == Constants.Hotel)
                    .Skip(pageNo * _resultsPerPage)
                    .Take(_resultsPerPage)
                    .List<Hotel>();
            }
        }

        /// <summary>
        /// Get Guest houses by email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IList<Hotel> GetGuestHousesByOwnerEmail(string email, int pageNo = 0)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return _session.Session.QueryOver<Hotel>()
                    .Where(x => x.OwnerEmail == email && x.PropertyType == Constants.GuestHouse)
                    .Skip(pageNo * _resultsPerPage)
                    .Take(_resultsPerPage)
                    .List<Hotel>();
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
        /// <param name="pageNo"></param>
        /// <returns></returns>
        //[Transaction]
        public IList<House> GetAllHouses(int pageNo = 0)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return _session.Session.QueryOver<House>()
                    .Where(x => x.PropertyType == Constants.House)
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
        /// Gets Apartments with reference to their PropertyType
        /// </summary>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        //[Transaction]
        public IList<House> GetAllApartments(int pageNo = 0)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return _session.Session.QueryOver<House>()
                    .Where(x => x.PropertyType == Constants.Apartment)
                    .Skip(pageNo * _resultsPerPage)
                    .Take(_resultsPerPage)
                    .List<House>();
            }
        }

        /// <summary>
        /// Search All Hostels
        /// </summary>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IList<Hostel> GetAllHostels(int pageNo = 0)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return _session.Session.QueryOver<Hostel>()
                    .Skip(pageNo * _resultsPerPage)
                    .Take(_resultsPerPage)
                    .List<Hostel>();
            }
        }

        /// <summary>
        /// Search all the Hotels
        /// </summary>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IList<Hotel> GetAllHotels(int pageNo = 0)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return _session.Session.QueryOver<Hotel>()
                    .Where(x => x.PropertyType == Constants.Hotel)
                    .Skip(pageNo * _resultsPerPage)
                    .Take(_resultsPerPage)
                    .List<Hotel>();
            }
        }

        /// <summary>
        /// Search all the Guest Houses
        /// </summary>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IList<Hotel> GetAllGuestHouses(int pageNo = 0)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return _session.Session.QueryOver<Hotel>()
                    .Where(x => x.PropertyType == Constants.GuestHouse)
                    .Skip(pageNo * _resultsPerPage)
                    .Take(_resultsPerPage)
                    .List<Hotel>();
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
        public IList<House> SearchHousesByCoordinates(decimal latitude, decimal longitude, string propertyType, int pageNo = 0)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                IList houses = _session.Session.CreateSQLQuery(
                            "SELECT *, ( 6371 * acos( cos( radians(:inputLatitude) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians(:inputLongitude) ) + sin( radians(:inputLatitude) ) * sin( radians( latitude ) ) ) ) AS distance FROM house WHERE property_type = :propertyType HAVING distance < :radius ORDER BY distance")
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
        /// Search Hostels in the radius sorrounding the coordinates
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IList<Hostel> SearchHostelByCoordinates(decimal latitude, decimal longitude, int pageNo = 0)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                IList houses = _session.Session.CreateSQLQuery(
                        "SELECT *, ( 6371 * acos( cos( radians(:inputLatitude) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians(:inputLongitude) ) + sin( radians(:inputLatitude) ) * sin( radians( latitude ) ) ) ) AS distance FROM hostel HAVING distance < :radius ORDER BY distance")
                    .AddEntity(typeof(Hostel))
                    //.AddScalar("latitude", NHibernateUtil.Decimal)
                    //.AddScalar("longitude", NHibernateUtil.Decimal)
                    //.AddScalar("distance", NHibernateUtil.Decimal)
                    .SetParameter("inputLatitude", latitude)
                    .SetParameter("inputLongitude", longitude)
                    .SetParameter("radius", _radius)
                    .SetFirstResult(pageNo * _resultsPerPage)
                    .SetMaxResults(_resultsPerPage)
                    .List();

                return houses.Cast<Hostel>().ToList();
            }
        }

        public IList<Hotel> SearchHotelByCoordinates(decimal latitude, decimal longitude, string propertyType, int pageNo = 0)
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                IList houses = _session.Session.CreateSQLQuery(
                        "SELECT *, ( 6371 * acos( cos( radians(:inputLatitude) ) * cos( radians( latitude ) ) * cos( radians( longitude ) - radians(:inputLongitude) ) + sin( radians(:inputLatitude) ) * sin( radians( latitude ) ) ) ) AS distance FROM hotel WHERE property_type = :propertyType HAVING distance < :radius ORDER BY distance")
                    .AddEntity(typeof(Hotel))
                    //.AddScalar("latitude", NHibernateUtil.Decimal)
                    //.AddScalar("longitude", NHibernateUtil.Decimal)
                    //.AddScalar("distance", NHibernateUtil.Decimal)
                    .SetParameter("inputLatitude", latitude)
                    .SetParameter("inputLongitude", longitude)
                    .SetParameter("propertyType", propertyType)
                    .SetParameter("radius", _radius)
                    .SetFirstResult(pageNo * _resultsPerPage)
                    .SetMaxResults(_resultsPerPage)
                    .List();

                return houses.Cast<Hotel>().ToList();
            }
        }
        
        /// <summary>
        /// Gets the number of records for the given criteria in the database
        /// Item 1: RecordCount 
        /// Item 2: Items Per Page
        /// </summary>
        /// <returns></returns>
        //[Transaction]
        public Tuple<int, int> GetRecordCountByPropertyType(string propertyType)
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
        public Tuple<int, int> GetRecordCountByLocationAndPropertyType(decimal latitude, decimal longitude, 
            string propertyType)
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
        public Tuple<int, int> GetTotalRecordCount()
        {
            using (_session.Session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                return new Tuple<int, int>(_session.Session.QueryOver<House>().RowCount(), _resultsPerPage);
            }
        }
    }
}
