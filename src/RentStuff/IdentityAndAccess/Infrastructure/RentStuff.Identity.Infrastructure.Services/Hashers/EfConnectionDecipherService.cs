using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using NLog;
using RentStuff.Common;

namespace RentStuff.Identity.Infrastructure.Services.Hashers
{
    /// <summary>
    /// Service that will decipher the connection string and return the DB Connection used by Entity Framework
    /// </summary>
    public class EfConnectionDecipherService
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        
        public static DbConnection GetEntityFrameDecipheredString()
        {
            var connection = DbProviderFactories.GetFactory("MySql.Data.MySqlClient").CreateConnection();
            if (connection == null)
            {
                _logger.Error("Could not create connection to DB for Entity Framework.");
                throw new NullReferenceException("Could not create DB connection for Entity Framework");
            }
            connection.ConnectionString = StringCipher.DecipheredConnectionString;
            return connection;
        }
    }
}
