using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace RentStuff.Property.Infrastructure.Persistence.NHibernate.Wrappers
{
    public class NHibernateSessionWrapper : IDisposable, INhibernateSessionWrapper
    {
        private ISessionFactory _sessionFactory;
        private ISession _session;

        public NHibernateSessionWrapper(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _session.Dispose();
        }
        
        /// <summary>
        /// The NHibernate Session
        /// </summary>
        public ISession Session { get { return _session ?? (_session = _sessionFactory.OpenSession()); } }
    }
}
