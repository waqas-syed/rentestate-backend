using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.OAuth;
using RentStuff.Identity.Application.Account.Commands;
using RentStuff.Identity.Application.Account.Representations;
using RentStuff.Identity.Domain.Model.Entities;

namespace RentStuff.Identity.Application.Account
{
    public interface IAccountApplicationService
    {
        string Register(CreateUserCommand createUserCommand, bool isExternalUser = false);
        bool Activate(ActivateAccountCommand activateAccountCommand);
        InternalLoginDataRepresentation RegisterExternalUser(RegisterExternalUserCommand registerExternalUserCommand);
        InternalLoginDataRepresentation ObtainAccessToken(string provider, string externalAccessToken);
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
