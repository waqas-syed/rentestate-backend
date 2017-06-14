using System.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using RentStuff.Common;
using RentStuff.Services.Infrastructure.Persistence.Repositories;

namespace RentStuff.Services.Infrastructure.Persistence.NHibernateCompound
{
    /// <summary>
    /// NHibernate Session Factory for the Services Bounded Context
    /// </summary>
    public class NHibernateSessionFactoryProvider
    {
        private ISessionFactory _sessionFactory;
        //private ISession _session;
        private string _connectionString;

        public ISessionFactory SessionFactory
        {
            get { return _sessionFactory ?? (_sessionFactory = GetSessionFactory()); }
        }

        /*public ISession Session
        {
            get { return _session ?? (_session = _sessionFactory.OpenSession()); }
        }*/

        public NHibernateSessionFactoryProvider()
        {
            // Get the deciphered connection string from the StringCipher class
            _connectionString = StringCipher.DecipheredConnectionString;
        }

        public ISessionFactory GetSessionFactory()
        {
            return
                Fluently.Configure()
                .Database(MySQLConfiguration.Standard
                .ConnectionString(_connectionString).ShowSql())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ServiceRepository>())
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                .BuildSessionFactory();
        }
    }
}
