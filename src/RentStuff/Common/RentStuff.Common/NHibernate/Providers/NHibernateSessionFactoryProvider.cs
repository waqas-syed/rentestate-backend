using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using Ninject;
using RentStuff.Common.Utilities;

namespace RentStuff.Common.NHibernate.Providers
{
    public class NHibernateSessionFactoryProvider : INHibernateSessionFactoryProvider
    {
        private ISessionFactory _sessionFactory;
        public ISessionFactory SessionFactory
        {
            get { return _sessionFactory ?? (_sessionFactory = GetSessionFactory()); }
        }

        private static IMappingProvider _mappingProvider;
        public NHibernateSessionFactoryProvider(IMappingProvider mappingProvider)
        {
            _mappingProvider = mappingProvider;
        }
        
        private ISessionFactory GetSessionFactory()
        {
            // The connection string provided in the config file is encrypted. We need to decrypt it and 
            // provide the original connection string to the NHibernate connection. The decryption is done 
            // entirely in the following Utitlity method present in the RentStuff.Common.Utility.StringCipher
            // class
            var decipheredConnectionString = StringCipher.DecipheredConnectionString;
            // Create an NHibernate configuration by code and specify the properties
            global::NHibernate.Cfg.Configuration cfg = new global::NHibernate.Cfg.Configuration()
                .DataBaseIntegration(x =>
                {
                    x.ConnectionString = decipheredConnectionString;
                    x.Dialect<MySQL5Dialect>();
                    x.Driver<MySqlDataDriver>();
                    x.LogFormattedSql = true;
                });
            
            _mappingProvider.GetMappings(cfg);
            cfg.Configure();
            // Build the Session Factory using the Configuration
            return cfg.BuildSessionFactory();

            // Properties that can be used in NHibernate Configuration:
            // http://nhibernate.info/doc/nhibernate-reference/session-configuration.html
            // GenerateStatistics
            // BatchSize
            // Listeners
            // SetCacheConcurrencyStrategy
            // SetCollectionCacheConcurrencyStrategy
            // bytecode-provider
            // reflection-optimizer
            // proxyfactory.factory_class
            // DatabaseIntegration.IsolationLevel 
            // DatabaseIntegration.LogFormattedSql 
            // DatabaseIntegration.AutoCommentSql 
        }

        [Inject]
        public static IMappingProvider MappingProvider { get;
            set;
        }
    }
}
