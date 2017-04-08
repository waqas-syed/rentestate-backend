using RentStuff.Identity.Application.Account.Commands;
using RentStuff.Identity.Application.Account.Representations;

namespace RentStuff.Identity.Application.Account
{
    public interface IAccountApplicationService
    {
        string Register(CreateUserCommand createUserCommand);
        bool Activate(ActivateAccountCommand activateAccountCommand);
        UserRepresentation GetUserByEmail(string email);

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

        void Dispose();
    }
}
