using NHibernate;
using NHibernate.Cfg;
using RentStuff.Common;

namespace RentStuff.Property.Infrastructure.Persistence.NHibernate.Providers
{
    public class NHibernateSessionFactoryProvider
    {
        private static ISessionFactory _sessionFactory;
        public static ISessionFactory SessionFactory
        {
            get { return _sessionFactory ?? (_sessionFactory = GetSessionFactory()); }
        }
        
        public static ISessionFactory GetSessionFactory()
        {
            // The connection string provided in the config file is encrypted. We need to decrypt it and 
            // provide the original conenction string to the NHibernate connection
            var decipheredConnectionString = StringCipher.DecipheredConnectionString;
            // Create an NHibernate configuration by code and specify the properties
            global::NHibernate.Cfg.Configuration cfg = new global::NHibernate.Cfg.Configuration()
                .DataBaseIntegration(x =>
                {
                    x.ConnectionString = decipheredConnectionString;
                });
            
            // Configure NHibernate
            cfg.Configure();
            // Build the Session Factory using the Configuration
            return cfg.BuildSessionFactory();
        }
    }
}
