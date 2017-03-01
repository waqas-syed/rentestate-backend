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


        public async Task<IdentityResult> Register(CreateUserCommand userModel)
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
            IdentityResult identityResult = await _accountRepository.RegisterUser(userModel.FullName, userModel.Email, 
                                                                                                                   userModel.Password);
            if (identityResult.Succeeded)
            {
                var retreivedUser = _accountRepository.FindByEmailAsync(userModel.Email);
                #pragma warning disable 4014
                Task.Run(() => SendActivationEmail(userModel.Email, retreivedUser.Result.FullName, retreivedUser.Result.ActivationCode));
                #pragma warning restore 4014
            }
            return identityResult;
        }

        private void SendActivationEmail(string email, string fullName, string activationCode)
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

        public void Dispose()
        {
            _accountRepository.Dispose();
        }
    }
}
