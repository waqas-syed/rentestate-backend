using NHibernate;

namespace RentStuff.Property.Infrastructure.Persistence.NHibernateCompound.Providers
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
            NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration();
            
            cfg.Configure();
            return cfg.BuildSessionFactory();

            // If you want ot odo it programmatically
            /* cfg.DataBaseIntegration(x =>
             {
                 x.ConnectionString =
                     "Server=localhost;Port=3306;Database=rentstuff;Uid=rentstuffuser;Password=LosSantosCrib786!;";
                 x.Driver<NHibernate.Driver.MySqlDataDriver>();
                 x.Dialect<MySQL5Dialect>();
                 x.LogSqlInConsole = false;
             });*/
        }
    }
}
