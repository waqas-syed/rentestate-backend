using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using RentStuff.Services.Infrastructure.Persistence.Repositories;

namespace RentStuff.Services.Infrastructure.Persistence.NHibernateSession
{
    /// <summary>
    /// NHibernate Session Factory for the Services Bounded Context
    /// </summary>
    public class NHibernateSessionCompound
    {
        private ISessionFactory _sessionFactory;

        public ISessionFactory SessionFactory
        {
            get { return _sessionFactory ?? (_sessionFactory = GetSessionFactory()); }
        }

        public ISessionFactory GetSessionFactory()
        {
            return
                Fluently.Configure()
                .Database(MySQLConfiguration.Standard
                  .ConnectionString(
                  @"Server=localhost;Port=3306;Database=rentstuff;Uid=rentstuffuser;Password=LosSantosCrib786!;")
                  .ShowSql())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ServicesRepository>())
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))//.Create(true, true)
                .BuildSessionFactory();
        }
    }
}
