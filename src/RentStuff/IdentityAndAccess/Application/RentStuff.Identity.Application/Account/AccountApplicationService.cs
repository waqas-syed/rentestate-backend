using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using RentStuff.Identity.Application.Account.Commands;
using RentStuff.Identity.Application.Account.Representations;
using RentStuff.Identity.Infrastructure.Persistence.Repositories;
using RentStuff.Identity.Infrastructure.Services.Email;
using RentStuff.Identity.Infrastructure.Services.Identity;
using Constants = RentStuff.Common.Constants;

namespace RentStuff.Identity.Application.Account
{
    public class AccountApplicationService : IAccountApplicationService
    {
        private ICustomEmailService _emailService;
        private IAccountRepository _accountRepository;
        private IUserTokenProvider<CustomIdentityUser,string> _userTokenService;

        public AccountApplicationService(IAccountRepository accountRepository, ICustomEmailService customEmailService,
            IUserTokenProvider<CustomIdentityUser, string> userTokenProviderService)
        {
            _accountRepository = accountRepository;
            _emailService = customEmailService;
            _userTokenService = userTokenProviderService;
        }
        
        public string Register(CreateUserCommand userModel)
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
            // Register the User
            IdentityResult registrationResult = _accountRepository.RegisterUser(userModel.FullName, userModel.Email, 
                                                                                                                   userModel.Password);
            if (registrationResult == null)
            {
                throw new NullReferenceException("Whoa! Execpected error happened while registering the user. Didnt expect that");
            }
            if (!registrationResult.Succeeded)
            {
                throw new InvalidOperationException(registrationResult.Errors.First());
            }
            // Get the User instance to have her Id
            var retreivedUser = _accountRepository.GetUserByEmail(userModel.Email);
            // Generate the token for this user using email and user Id
            //var emailVerificationToken = _emailTokenGenerationService.GenerateEmailToken(retreivedUser.Email, retreivedUser.Id);
            var emailVerificationToken = _accountRepository.GetEmailActivationToken(retreivedUser.Id);
            if (string.IsNullOrWhiteSpace(emailVerificationToken))
            {
                throw new NullReferenceException("Could not generate token for user: " + retreivedUser.Id);
            }
            // Send email to the user
            #pragma warning disable 4014
            Task.Run(() => SendActivationEmail(retreivedUser.Email, retreivedUser.FullName, emailVerificationToken));
            #pragma warning restore 4014
            return retreivedUser.Id;
        }

        private void SendActivationEmail(string email, string fullName, string activationCode)
        {
            var activationLink = Constants.FrontEndUrl + "/" + Constants.AccountActivationUrlLocation + "?email=" + email +
                                 "&activationcode=" + activationCode;
            _emailService.SendEmail(email, EmailConstants.ActivationEmailSubject, EmailConstants.ActivationEmailMessage(fullName, activationLink));
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
            
            // Get the user from the database so that we have the userId which we need to verify the token
            var user = _accountRepository.GetUserByEmail(activateAccountCommand.Email);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }
            var confirmEmailSucceeded = _accountRepository.ConfirmEmail(user.Id, activateAccountCommand.ActivationCode);
            if (!confirmEmailSucceeded)
            {
                throw new InvalidOperationException("Error arose while confirming email. Please try again later");
            }
            else
            {
                return true;
            }
            /*if (user.EmailConfirmed)
            {
                throw new InvalidOperationException("User account is already activated");
            }

            // Verify the Email Token for the user
            if (_emailTokenGenerationService.VerifyToken(activateAccountCommand.Email, user.Id, activateAccountCommand.ActivationCode))
            {*/
                /*user.EmailConfirmed = true;
                var identityResult = _accountRepository.UpdateUser(user);*/
                /*var confirmEmailSucceeded = _accountRepository.ConfirmEmail(user.Id, activateAccountCommand.ActivationCode);
                if (!confirmEmailSucceeded)
                {
                    throw new InvalidOperationException("Error arose while confirming email. Please try again later");
                }
                return true;
            }
            throw new InvalidOperationException("Invalid token");*/
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

        /// <summary>
        /// Sends user token to reset password
        /// </summary>
        /// <param name="forgotPasswordCommand"></param>
        public void ForgotPassword(ForgotPasswordCommand forgotPasswordCommand)
        {
            var user = _accountRepository.GetUserByEmail(forgotPasswordCommand.Email);
            if (user.EmailConfirmed)
            {
                // Generate the token
                var resetPasswordToken = _accountRepository.GetPasswordResetToken(user.Id);
                // Create the link that will be sent in the email
                var passwordResetEmailBody = Constants.FrontEndUrl + "/" + Constants.AccountActivationUrlLocation + "?email=" 
                    + forgotPasswordCommand.Email + "&resettoken=" + resetPasswordToken;
                // Sned the email with the generated token within the generated link
                _emailService.SendEmail(forgotPasswordCommand.Email, EmailConstants.PasswordResetSubject, 
                    EmailConstants.PasswordResetEmail(user.FullName, passwordResetEmailBody));

                // Update the password reset settings
                user.IsPasswordResetRequested = true;
                user.PasswordResetExpiryDate = DateTime.Now.AddHours(24);
                _accountRepository.UpdateUser(user);
            }
            else
            {
                throw new InvalidOperationException("Cannot reset password");
            }
        }

        /// <summary>
        ///  Reset the password for the user when they provide a new one after clicking on the link sent to them on email
        /// </summary>
        /// <param name="resetPasswordCommand"></param>
        /// <returns></returns>
        public bool ResetPassword(ResetPasswordCommand resetPasswordCommand)
        {
            var user = _accountRepository.GetUserByEmail(resetPasswordCommand.Email);
            if (user == null)
            {
                throw new NullReferenceException("User could not be found");
            }
            if (!resetPasswordCommand.Password.Equals(resetPasswordCommand.ConfirmPassword))
            {
                throw new NullReferenceException("Passwords do not match");
            }
            if (user.IsPasswordResetRequested)
            {
                if (user.PasswordResetExpiryDate >= DateTime.Now)
                {
                    //var validateTask = _userTokenService.ValidateAsync("purpose", resetPasswordCommand.Token, null, user);
                    var resetPasswordResponse = _accountRepository.ResetPassword(user.Id, resetPasswordCommand.Token,
                        resetPasswordCommand.Password);
                    if (resetPasswordResponse)
                    {
                        user.IsPasswordResetRequested = false;
                        user.PasswordResetExpiryDate = null;
                        _accountRepository.UpdateUser(user);
                        return true;
                    }
                }
                else
                {
                    user.IsPasswordResetRequested = false;
                    user.PasswordResetExpiryDate = null;
                    _accountRepository.UpdateUser(user);
                    return false;
                }
            }
            else
            {
                throw new InvalidOperationException("Reset password not requested so the password wont be reset");
            }
            return false;
        }
        
        public void Dispose()
        {
            _accountRepository.Dispose();
        }
    }
}
