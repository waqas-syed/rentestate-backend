using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using RentStuff.Identity.Application.Account.Commands;
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
        
        public async Task<bool> Register(CreateUserCommand userModel)
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
            Tuple<IdentityResult,string> emailConfirmationToken = await _accountRepository.SaveUser(userModel.FullName, userModel.Email, 
                                                                                                                   userModel.Password);
            if (emailConfirmationToken.Item1.Succeeded && !string.IsNullOrWhiteSpace(emailConfirmationToken.Item2))
            {
                var retreivedUser = await _accountRepository.FindByEmailAsync(userModel.Email);
                #pragma warning disable 4014
                Task.Run(() => SendActivationEmail(retreivedUser.Id, retreivedUser.Email, retreivedUser.FullName, 
                    emailConfirmationToken.Item2));
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

        public async Task<CustomIdentityUser> FindUser(string userName, string password)
        {
            CustomIdentityUser user = await _accountRepository.FindUser(userName, password);

            return user;
        }

        public async Task<bool> Activate(ActivateAccountCommand activateAccountCommand)
        {
            if (string.IsNullOrWhiteSpace(activateAccountCommand.Email))
            {
                throw new ArgumentException("Email not provided");
            }
            if (string.IsNullOrWhiteSpace(activateAccountCommand.ActivationCode))
            {
                throw new ArgumentException("Activation Code not provided");
            }
            var user = await _accountRepository.FindByEmailAsync(activateAccountCommand.Email);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }
            if (_accountRepository.UserManager.IsEmailConfirmed(user.Id))
            {
                throw new InvalidOperationException("User account is already activated");
            }
            var confirmEmailResult = await _accountRepository.UserManager.ConfirmEmailAsync(user.Id, activateAccountCommand.ActivationCode);

            if (!confirmEmailResult.Succeeded)
            {
                throw new ArgumentException("Could not confirm email.");
            }
            else
            {
                return true;
            }
        }

        public void Dispose()
        {
            _accountRepository.Dispose();
        }
    }
}
