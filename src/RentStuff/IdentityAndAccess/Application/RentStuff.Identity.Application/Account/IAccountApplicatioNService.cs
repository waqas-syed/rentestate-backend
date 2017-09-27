using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.OAuth;
using RentStuff.Identity.Application.Account.Commands;
using RentStuff.Identity.Application.Account.Representations;
using RentStuff.Identity.Domain.Model.Entities;

namespace RentStuff.Identity.Application.Account
{
    /// <summary>
    /// Acount's Application service
    /// </summary>
    public interface IAccountApplicationService
    {
        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="createUserCommand"></param>
        /// <param name="isExternalUser"></param>
        /// <returns></returns>
        string Register(CreateUserCommand createUserCommand, bool isExternalUser = false);
        
        /// <summary>
        /// Activate the account
        /// </summary>
        /// <param name="activateAccountCommand"></param>
        /// <returns></returns>
        bool Activate(ActivateAccountCommand activateAccountCommand);

        /// <summary>
        /// Register the external user
        /// </summary>
        /// <param name="registerExternalUserCommand"></param>
        /// <returns></returns>
        InternalLoginDataRepresentation RegisterExternalUser(RegisterExternalUserCommand registerExternalUserCommand);

        /// <summary>
        /// Obtain the access token
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="externalAccessToken"></param>
        /// <returns></returns>
        InternalLoginDataRepresentation ObtainAccessToken(string provider, string externalAccessToken);

        /// <summary>
        /// Get the user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        UserRepresentation GetUserByEmail(string email);
        bool UserExistsByUserLoginInfo(UserLoginInfo userLoginInfo);


        UserRepresentation GetUserByUserLoginInfoRepresentation(UserLoginInfo userLoginInfo);
        
        /// <summary>
        /// Sends user token to reset password
        /// </summary>
        /// <param name="forgotPasswordCommand"></param>
        void ForgotPassword(ForgotPasswordCommand forgotPasswordCommand);

        /// <summary>
        ///  Reset the password for the user when they provide a new one after clicking on the link sent to them on email
        /// </summary>
        /// <param name="resetPasswordCommand"></param>
        /// <returns></returns>
        bool ResetPassword(ResetPasswordCommand resetPasswordCommand);

        bool AddLogin(string userId, UserLoginInfo userLoginInfo);

        /// <summary>
        /// Maps the ExternalAccessToken to an internal Id and return the InternalId. This way we never expose the ExternalAccessToken
        /// to the outside world
        /// </summary>
        /// <param name="externalAccessToken"></param>
        /// <returns></returns>
        string MapExternalAccessTokenToInternalId(string externalAccessToken);

        /// <summary>
        /// Retrieves the ExternalAccessToken for an existing ExternalAccessTokenIdentifier instance by providing the InternalId
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns>ExternalAccessToken</returns>
        ExternalAccessTokenIdentifier GetExternalAccessTokenIdentifier(string internalId);
        
        void Dispose();
    }
}
