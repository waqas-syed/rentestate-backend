using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace RentStuff.IdentityAndAccess.Adapter.Rest
{
    public class AuthContext : IdentityDbContext<IdentityUser>
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