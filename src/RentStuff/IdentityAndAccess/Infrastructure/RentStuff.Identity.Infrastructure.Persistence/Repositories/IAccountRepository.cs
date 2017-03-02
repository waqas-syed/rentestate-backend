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
        Tuple<IdentityResult, string> RegisterUser(string name, string email, string password);

        CustomIdentityUser GetUserByEmail(string email);

        CustomIdentityUser GetUserByPassword(string userName, string password);

        IdentityResult UpdateUser(CustomIdentityUser customerIdentityUser);

        bool ActivateUser(string userId, string emailConfirmationToken);

        bool IsEmailConfirmed(string userId);
    }
}
