using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using RentStuff.Common;
using Spring.Context.Support;

namespace RentStuff.Property.Infrastructure.Services.DbDecipherServices
{
    /// <summary>
    /// Deciphers the connection string to be used by NHibernate
    /// </summary>
    public class NhConnectionDecipherService
    {
        public static void SetupDecipheredConnectionString()
        {
            // Get the deciphered connection string
            var connectionString = StringCipher.DecipheredConnectionString;

            // Get the DbProvider from the Spring conect registry
            var dbProvider = (Spring.Data.Common.IDbProvider)Spring.Context.Support.ContextRegistry.GetContext().GetObject("DbProvider");
            // Update the DbProvider instance with the new connection string
            dbProvider.ConnectionString = connectionString;

            // Cast the SesionFactory to SessionFactoryImpl
            var nHibernateSessionFactory = (NHibernate.Impl.SessionFactoryImpl)ContextRegistry.GetContext()["NHibernateSessionFactory"];
            // Get the ConnectionProvider
            var connectionProviderType = nHibernateSessionFactory.ConnectionProvider.GetType();
            // Get the DbProvider PropertyTypeInfo
            var connectionProviderProperty = connectionProviderType.GetProperty("DbProvider");
            connectionProviderProperty.SetValue(nHibernateSessionFactory.ConnectionProvider, dbProvider, null);

            var connectionStringDict = new Dictionary<string, string>();
            connectionStringDict.Add("connection.connection_string", connectionString);
            connectionStringDict.Add("connection.driver_class", "NHibernate.Driver.MySqlDataDriver");
            nHibernateSessionFactory.ConnectionProvider.Configure(connectionStringDict);
        }
    }
}
