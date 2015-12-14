using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// <summary>
        /// Saves or Updates the Location instance
        /// </summary>
        /// <param name="location"></param>
        [Transaction(TransactionPropagation.Required, ReadOnly = false)]
        public void SaveOrUpdate(Location location)
        {
            CurrentSession.SaveOrUpdate(location);
        }
    }
}
