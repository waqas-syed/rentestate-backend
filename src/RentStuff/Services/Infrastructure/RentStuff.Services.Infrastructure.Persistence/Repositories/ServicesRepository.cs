using System.Collections.Generic;
using System.Data;
using NHibernate;
using RentStuff.Services.Domain.Model.ServiceAggregate;

namespace RentStuff.Services.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repository for Services
    /// </summary>
    public class ServicesRepository : IServicesRepository
    {
        private ISession _session;
        private readonly ITransaction _transaction;

        public ServicesRepository(ISession session)
        {
            _session = session;
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
        /// Get the Service by it's Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IList<Service> GetServiceByName(string name)
        {
            return _session.QueryOver<Service>().Where(x => x.Name == name).List();
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
