using System;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RentStuff.Identity.Domain.Model.Entities;
using RentStuff.Identity.Infrastructure.Services.Identity;

namespace RentStuff.Identity.Infrastructure.Persistence.Repositories
{
    public interface IAccountRepository : IDisposable
    {
        /// <summary>
        /// Creates the DbConnection required for the AuthContext
        /// </summary>
        /// <returns></returns>
        DbConnection CreateDbConnection();

        string GetEmailActivationToken(string userId);

        bool ConfirmEmail(string userId, string token);

        /// <summary>
        /// Registers a user and returns a tuple with the following two items:
        /// IdentityResult : EmailConfirmationToken
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="isExternalUser"></param>
        /// <returns></returns>
        IdentityResult RegisterUser(string name, string email, string password, bool isExternalUser = false);

        /// <summary>
        /// Get user by UserLoginInfo
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        CustomIdentityUser GetUserByUserLoginInfo(UserLoginInfo loginInfo);

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        CustomIdentityUser GetUserByEmail(string email);

        CustomIdentityUser GetUserByPassword(string userName, string password);
        
        IdentityResult AddLogin(string userId, UserLoginInfo userLoginInfo);

        IdentityResult UpdateUser(CustomIdentityUser customerIdentityUser);

        bool IsEmailConfirmed(string userId);

        string GetPasswordResetToken(string userId);

        bool ResetPassword(string userId, string token, string newPassword);

        /// <summary>
        /// Saves an ExternalAccessTokenIdentifier to the database
        /// </summary>
        /// <param name="externalAccessTokenIdentifier"></param>
        ExternalAccessTokenIdentifier SaveExternalAccessTokenIdentifier(ExternalAccessTokenIdentifier externalAccessTokenIdentifier);

        void DeleteExternalAccessTokenIdentifier(ExternalAccessTokenIdentifier externalAccessTokenIdentifier);

        /// <summary>
        /// Finds the ExternalAccessTokenIdentifier given the InternalIdentifier
        /// </summary>
        /// <param name="internalIdentifier"></param>
        /// <returns></returns>
        ExternalAccessTokenIdentifier GetExternalAccessIdentifierByInternalId(string internalIdentifier);

        ExternalAccessTokenIdentifier GetExternalAccessIdentifierByToken(string externalAccessToken);
    }
}
