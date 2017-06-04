using System.Data;
using NHibernate;
using RentStuff.Services.Domain.Model.ServiceAggregate;
using RentStuff.Services.Infrastructure.Persistence.NHibernateSession;

namespace RentStuff.Services.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repository for Services
    /// </summary>
    public class ServicesRepository : NHibernateSessionCompound, IServicesRepository
    {
        private ISession _session;
        private readonly ITransaction _transaction;

        public ServicesRepository(ISessionFactory sessionFactory)
        {
            _session = sessionFactory.OpenSession();
            _transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        /// <summary>
        /// Save or update the service
        /// </summary>
        /// <param name="service"></param>
        public void SaveOrUpdate(Service service)
        {
            _session.SaveOrUpdate(service);
            _transaction.Commit();
        }

        /// <summary>
        /// Get the Service by it's ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Service GetServiceById(string id)
        {
            return _session.QueryOver<Service>().Where(x => x.Id == id).SingleOrDefault();
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
