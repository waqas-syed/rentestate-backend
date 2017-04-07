using RentStuff.Identity.Application.Account.Commands;
using RentStuff.Identity.Application.Account.Representations;

namespace RentStuff.Identity.Application.Account
{
    public interface IAccountApplicationService
    {
        string Register(CreateUserCommand createUserCommand);
        bool Activate(ActivateAccountCommand activateAccountCommand);
        UserRepresentation GetUserByEmail(string email);
        void ForgotPassword(ForgotPasswordCommand forgotPasswordCommand);
        void Dispose();
    }
}
