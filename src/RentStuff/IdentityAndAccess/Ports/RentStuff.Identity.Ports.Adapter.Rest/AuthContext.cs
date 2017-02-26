using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using RentStuff.Identity.Ports.Adapter.Rest.Models;

namespace RentStuff.Identity.Ports.Adapter.Rest
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