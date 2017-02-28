using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using RentStuff.Identity.Infrastructure.Persistence.Model;

namespace RentStuff.Identity.Infrastructure.Persistence
{
    public class AuthContext : IdentityDbContext<CustomIdentityUser>
    {
        public AuthContext()
            : base("AuthContext")
        {
            Database.SetInitializer(new MySqlInitializer());
        }

        //public DbSet<Client> Clients { get; set; }
        //public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}