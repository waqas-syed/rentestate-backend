using System;
using System.Collections;
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

        /// <summary>
        /// Get activation token for the user to send via email
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        string GetEmailActivationToken(string userId);

        /// <summary>
        /// Confirm email using the given token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get the user by password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        CustomIdentityUser GetUserByPassword(string userName, string password);
        
        /// <summary>
        /// Add a new login to the database for an external user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userLoginInfo"></param>
        /// <returns></returns>
        IdentityResult AddLogin(string userId, UserLoginInfo userLoginInfo);

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="customerIdentityUser"></param>
        /// <returns></returns>
        IdentityResult UpdateUser(CustomIdentityUser customerIdentityUser);

        /// <summary>
        /// Is email confirmed via email
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool IsEmailConfirmed(string userId);

        /// <summary>
        /// Get the reset token for password
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        string GetPasswordResetToken(string userId);

        /// <summary>
        /// Reset the password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        bool ResetPassword(string userId, string token, string newPassword);

        /// <summary>
        /// Saves an ExternalAccessTokenIdentifier to the database
        /// </summary>
        /// <param name="externalAccessTokenIdentifier"></param>
        ExternalAccessTokenIdentifier SaveExternalAccessTokenIdentifier(ExternalAccessTokenIdentifier externalAccessTokenIdentifier);

        /// <summary>
        /// Delete the external token identifier, this is used to be sent to the frontend client that it will
        /// use in the next request to identify themselves. A one time token, not to be used as a token to be used
        /// after the login
        /// </summary>
        /// <param name="externalAccessTokenIdentifier"></param>
        void DeleteExternalAccessTokenIdentifier(ExternalAccessTokenIdentifier externalAccessTokenIdentifier);

        /// <summary>
        /// Finds the ExternalAccessTokenIdentifier given the InternalIdentifier
        /// </summary>
        /// <param name="internalIdentifier"></param>
        /// <returns></returns>
        ExternalAccessTokenIdentifier GetExternalAccessIdentifierByInternalId(string internalIdentifier);
    }
}
