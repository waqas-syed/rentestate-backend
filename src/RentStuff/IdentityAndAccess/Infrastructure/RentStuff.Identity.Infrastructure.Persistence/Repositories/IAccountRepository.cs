using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using RentStuff.Identity.Infrastructure.Services.Identity;

namespace RentStuff.Identity.Infrastructure.Persistence.Repositories
{
    public interface IAccountRepository : IDisposable
    {
        string GetEmailActivationToken(string userId);

        bool ConfirmEmail(string userId, string token);

        /// <summary>
        /// Registers a user and returns a tuple with the following two items:
        /// IdentityResult : EmailConfirmationToken
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        IdentityResult RegisterUser(string name, string email, string password);

        CustomIdentityUser GetUserByEmail(string email);

        CustomIdentityUser GetUserByPassword(string userName, string password);

        IdentityResult UpdateUser(CustomIdentityUser customerIdentityUser);

        bool IsEmailConfirmed(string userId);

        string GetPasswordResetToken(string userId);

        bool ResetPassword(string userId, string token, string newPassword);
    }
}
