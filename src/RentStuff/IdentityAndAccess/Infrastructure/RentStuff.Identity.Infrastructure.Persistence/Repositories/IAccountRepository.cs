using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using RentStuff.Identity.Infrastructure.Persistence.Model;

namespace RentStuff.Identity.Infrastructure.Persistence.Repositories
{
    public interface IAccountRepository : IDisposable
    {
        /// <summary>
        /// Registers a user and returns a tuple with the following two items:
        /// IdentityResult : EmailConfirmationToken
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<Tuple<IdentityResult, string>> RegisterUser(string name, string email, string password);

        Task<CustomIdentityUser> FindByEmailAsync(string email);

        Task<CustomIdentityUser> FindUser(string userName, string password);

        Task<IdentityResult> UpdateUser(CustomIdentityUser customerIdentityUser);

        UserManager<CustomIdentityUser> UserManager { get; }
    }
}
