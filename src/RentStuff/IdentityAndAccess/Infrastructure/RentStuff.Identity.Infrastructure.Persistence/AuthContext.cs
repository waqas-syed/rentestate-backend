using System.Data.Common;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using RentStuff.Identity.Domain.Model.Entities;
using RentStuff.Identity.Infrastructure.Services.Identity;

namespace RentStuff.Identity.Infrastructure.Persistence
{
    public class AuthContext : IdentityDbContext<CustomIdentityUser>
    {
        public AuthContext(DbConnection dbConnection, bool ownsConnection) : base(dbConnection, ownsConnection)
        {
            Database.SetInitializer(new MySqlInitializer());
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<BlogContext, Configuration>());
        }
        // Used to map the ExternalAccessTokens to Internal Identifiers that we create internally. Thus we never expose the
        // ExternalAccessToken to the outside world
        public DbSet<ExternalAccessTokenIdentifier> ExternalAccessExternalTokenIdentifiers { get; set; }
        //public DbSet<Client> Clients { get; set; }
        //public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}