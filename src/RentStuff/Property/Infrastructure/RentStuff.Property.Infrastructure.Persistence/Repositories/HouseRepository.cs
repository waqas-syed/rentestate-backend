using System.Collections.Generic;
using RentStuff.Property.Domain.Model.HouseAggregate;
using Spring.Transaction;
using Spring.Transaction.Interceptor;

namespace RentStuff.Property.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// House Repository
    /// </summary>
    public class HouseRepository : NHibernateSessionFactory, IHouseRepository
    {
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
        public House GetHouseById(long houseId)
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
