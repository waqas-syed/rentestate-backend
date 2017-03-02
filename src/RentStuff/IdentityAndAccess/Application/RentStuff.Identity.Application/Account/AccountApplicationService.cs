using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using RentStuff.Identity.Application.Account.Commands;
using RentStuff.Identity.Application.Account.Representations;
using RentStuff.Identity.Infrastructure.Persistence.Model;
using RentStuff.Identity.Infrastructure.Persistence.Repositories;
using RentStuff.Identity.Infrastructure.Services.Email;
using Constants = RentStuff.Common.Constants;

namespace RentStuff.Identity.Application.Account
{
    public class AccountApplicationService : IAccountApplicationService
    {
        private ICustomEmailService _emailService;
        private IAccountRepository _accountRepository;

        public AccountApplicationService(IAccountRepository accountRepository, ICustomEmailService customEmailService)
        {
            _accountRepository = accountRepository;
            _emailService = customEmailService;
        }
        
        public bool Register(CreateUserCommand userModel)
        {
            if (string.IsNullOrWhiteSpace(userModel.FullName))
            {
                throw new ArgumentException("Name cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(userModel.Email))
            {
                throw new ArgumentException("Email cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(userModel.Password))
            {
                throw new ArgumentException("Password cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(userModel.ConfirmPassword))
            {
                throw new ArgumentException("Confirm Password cannot be empty");
            }
            if (userModel.FullName.Length > 19)
            {
                throw new ArgumentException("Upto 19 characters are allowed in property FullName, not more");
            }
            if (!userModel.Password.Equals(userModel.ConfirmPassword))
            {
                throw new ArgumentException("Password and confirm password are not the same");
            }
            Tuple<IdentityResult,string> emailConfirmationToken = _accountRepository.RegisterUser(userModel.FullName, userModel.Email, 
                                                                                                                   userModel.Password);
            if (emailConfirmationToken.Item1.Succeeded && !string.IsNullOrWhiteSpace(emailConfirmationToken.Item2))
            {
                var retreivedUser = _accountRepository.GetUserByEmail(userModel.Email);
                //SendActivationEmail(retreivedUser.Id, retreivedUser.Email, retreivedUser.FullName, emailConfirmationToken.Item2);
                #pragma warning disable 4014
                Task.Run(() => SendActivationEmail(retreivedUser.Id, retreivedUser.Email, retreivedUser.FullName, emailConfirmationToken.Item2));
                #pragma warning restore 4014
                return true;
            }
            else
            {
                throw new ArgumentException("Could not register User");
            }
        }

        private void SendActivationEmail(string userId, string email, string fullName, string activationCode)
        {
            var activationLink = Constants.DOMAINURL + Constants.AccountActivationUrlLocation + "?email=" + email +
                                 "&activationcode=" + activationCode;
            _emailService.SendEmail(email, EmailConstants.ActivationEmailSubject, EmailConstants.ActivationEmailMessage(fullName, activationLink));
        }

        public CustomIdentityUser FindUser(string userName, string password)
        {
            CustomIdentityUser user = _accountRepository.GetUserByPassword(userName, password);

            return user;
        }

        public bool Activate(ActivateAccountCommand activateAccountCommand)
        {
            if (string.IsNullOrWhiteSpace(activateAccountCommand.Email))
            {
                throw new ArgumentException("Email not provided");
            }
            if (string.IsNullOrWhiteSpace(activateAccountCommand.ActivationCode))
            {
                throw new ArgumentException("Activation Code not provided");
            }
            var user = _accountRepository.GetUserByEmail(activateAccountCommand.Email);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }
            if (_accountRepository.IsEmailConfirmed(user.Id))
            {
                throw new InvalidOperationException("User account is already activated");
            }
            return _accountRepository.ActivateUser(user.Id, activateAccountCommand.ActivationCode);
        }

        public UserRepresentation GetUserByEmail(string email)
        {
            CustomIdentityUser customIdentityUser = _accountRepository.GetUserByEmail(email);
            if (customIdentityUser != null)
            {
                return new UserRepresentation(customIdentityUser.FullName, customIdentityUser.Email, customIdentityUser.EmailConfirmed);
            }
            throw new NullReferenceException("Could not find the user for the given email");
        }

        public void Dispose()
        {
            _accountRepository.Dispose();
        }
    }
}
