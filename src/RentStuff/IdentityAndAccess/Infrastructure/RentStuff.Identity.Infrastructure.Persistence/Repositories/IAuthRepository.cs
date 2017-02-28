using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using RentStuff.Identity.Infrastructure.Persistence.Model;

namespace RentStuff.Identity.Infrastructure.Persistence.Repositories
{
    public interface IAuthRepository : IDisposable
    {
        Task<IdentityResult> RegisterUser(string name, string email, string password);

        Task<CustomIdentityUser> FindUser(string userName, string password);
    }
}
