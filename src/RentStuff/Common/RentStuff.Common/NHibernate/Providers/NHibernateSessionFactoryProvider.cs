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

        private static IPropertyMappingProvider _propertyMappingProvider;
        private static IServiceMappingProvider _serviceMappingProvider;

        /// <summary>
        /// Accepts both the PropertyMappingProvider and ServiceMappingProvider. This way using Dependency
        /// Injection it can call these implementations to add assemblies without having to reference their 
        /// implementation projects, as it results in Circular Dependency
        /// </summary>
        /// <param name="mappingProvider"></param>
        /// <param name="serviceMappingProvider"></param>
        public NHibernateSessionFactoryProvider(IPropertyMappingProvider mappingProvider, 
            IServiceMappingProvider serviceMappingProvider)
        {
            _propertyMappingProvider = mappingProvider;
            _serviceMappingProvider = serviceMappingProvider;
        }

        /// <summary>
        /// Accepts Only the PropertyMappingProvider when Property BC's test cases are run. This way using
        /// Dependency Injection, it can call these implementations to add assemblies without having to reference their 
        /// implementation projects, as it results in Circular Dependency
        /// </summary>
        /// <param name="mappingProvider"></param>
        public NHibernateSessionFactoryProvider(IPropertyMappingProvider mappingProvider)
        {
            _propertyMappingProvider = mappingProvider;
        }

        /// <summary>
        /// Accepts Only the ServiceMappingProvider when Property BC's test cases are run. This way using
        /// Dependency Injection, it can call these implementations to add assemblies without having to reference their 
        /// implementation projects, as it results in Circular Dependency
        /// </summary>
        /// <param name="serviceMappingProvider"></param>
        public NHibernateSessionFactoryProvider(IServiceMappingProvider serviceMappingProvider)
        {
            _serviceMappingProvider = serviceMappingProvider;
        }
        
        /// <summary>
        /// Creates and returns the Session Factory
        /// </summary>
        /// <returns></returns>
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
            
            // If the PropertyMapping Provider is implemented, then call the implementation to add the 
            // NHibernate mapping assemblies to the Nhibernate configuration
            if (_propertyMappingProvider != null)
            {
                _propertyMappingProvider.AddMappingAssemblies(cfg);
            }
            // Likewise if the PropertyMapping Provider is implemented, then call the implementation to add the 
            // NHibernate mapping assemblies to the Nhibernate configuration
            if (_serviceMappingProvider != null)
            {
                _serviceMappingProvider.AddMappingAssemblies(cfg);
            }
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
    }
}
