using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;
using NLog;
using RentStuff.Identity.Application.Account.Commands;
using RentStuff.Identity.Application.Account.Representations;
using RentStuff.Identity.Application.Facebook;
using RentStuff.Identity.Domain.Model.Entities;
using RentStuff.Identity.Infrastructure.Persistence.Repositories;
using RentStuff.Identity.Infrastructure.Services.Email;
using RentStuff.Identity.Infrastructure.Services.Identity;
using Constants = RentStuff.Common.Utilities.Constants;

namespace RentStuff.Identity.Application.Account
{
    public class AccountApplicationService : IAccountApplicationService
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private ICustomEmailService _emailService;
        private IAccountRepository _accountRepository;

        public AccountApplicationService(IAccountRepository accountRepository, 
            ICustomEmailService customEmailService)
        {
            _accountRepository = accountRepository;
            _emailService = customEmailService;
        }
        
        public string Register(CreateUserCommand userModel, bool isExternalUser = false)
        {
            if (string.IsNullOrWhiteSpace(userModel.FullName))
            {
                throw new ArgumentException("Name cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(userModel.Email))
            {
                throw new ArgumentException("Email cannot be empty");
            }
            if (!isExternalUser && string.IsNullOrWhiteSpace(userModel.Password))
            {
                throw new ArgumentException("Password cannot be empty");
            }
            if (!isExternalUser && string.IsNullOrWhiteSpace(userModel.ConfirmPassword))
            {
                throw new ArgumentException("Confirm Password cannot be empty");
            }
            if (userModel.FullName.Length > 19)
            {
                throw new ArgumentException("Upto 19 characters are allowed in property FullName, not more");
            }
            if (!isExternalUser && !userModel.Password.Equals(userModel.ConfirmPassword))
            {
                throw new ArgumentException("Password and confirm password are not the same");
            }
            // Register the User
            IdentityResult registrationResult = _accountRepository.RegisterUser(userModel.FullName, userModel.Email, 
                                                                                userModel.Password, isExternalUser);
            if (registrationResult == null)
            {
                throw new NullReferenceException("Whoa! Unexpected error happened while registering the user. Didnt expect that");
            }
            if (!registrationResult.Succeeded)
            {
                throw new InvalidOperationException(registrationResult.Errors.First());
            }
            _logger.Info("Registered User Successfuly. Email: {0}  FullName: {1}", userModel.Email, userModel.FullName);
            // Get the User instance to have her Id
            var retreivedUser = _accountRepository.GetUserByEmail(userModel.Email);
            // Generate the token for this user using email and user Id
            //var emailVerificationToken = _emailTokenGenerationService.GenerateEmailToken(retreivedUser.Email, retreivedUser.Id);
            var emailVerificationToken = _accountRepository.GetEmailActivationToken(retreivedUser.Id);
            if (string.IsNullOrWhiteSpace(emailVerificationToken))
            {
                _logger.Error("Error while generating email confirmation token for user. Email: {0}", userModel.Email);
                throw new NullReferenceException("Could not generate token for user: " + retreivedUser.Id);
            }
            if (!isExternalUser)
            {
                // Send email to the user
                #pragma warning disable 4014
                Task.Run(() => SendActivationEmail(retreivedUser.Email, retreivedUser.FullName,
                    emailVerificationToken));
                #pragma warning restore 4014
            }
            else
            {
                _accountRepository.ConfirmEmail(retreivedUser.Id, emailVerificationToken);
            }
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
                _logger.Error("No email provided for ActivateAccountCommand");
                throw new ArgumentException("Email not provided");
            }
            if (string.IsNullOrWhiteSpace(activateAccountCommand.ActivationCode))
            {
                _logger.Error("No activation code provided for ActivateAccountCommand. Email: {0}", activateAccountCommand.Email);
                throw new ArgumentException("Activation Code not provided");
            }
            
            // Get the user from the database so that we have the userId which we need to verify the token
            var user = _accountRepository.GetUserByEmail(activateAccountCommand.Email);
            if (user == null)
            {
                _logger.Error("User not found for the given email: {0}", activateAccountCommand.Email);
                throw new ArgumentException("User not found");
            }
            var confirmEmailSucceeded = _accountRepository.ConfirmEmail(user.Id, activateAccountCommand.ActivationCode);
            if (!confirmEmailSucceeded)
            {
                throw new InvalidOperationException("Error arose while confirming email. Please try again later");
            }
            else
            {
                _logger.Info("Email Confirmation successful. Email: {0}", activateAccountCommand.Email);
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

        /// <summary>
        /// Saves user who registers using Facebook or other third party account
        /// </summary>
        /// <returns></returns>
        public InternalLoginDataRepresentation RegisterExternalUser(RegisterExternalUserCommand registerExternalUserCommand)
        {
            // Verify that the External is legit and is providing us correct external access token
            var verifiedAccessToken = VerifyExternalAccessToken(registerExternalUserCommand.Provider, registerExternalUserCommand.ExternalAccessToken);
            if (verifiedAccessToken == null)
            {
                throw new InvalidOperationException("Invalid Provider or External Access Token");
            }

            // Check if the user is not already registered with our app
            var hasRegistered = this.UserExistsByUserLoginInfo(new UserLoginInfo(registerExternalUserCommand.Provider, verifiedAccessToken.UserId));
            if (hasRegistered)
            {
                throw new InvalidOperationException("External user is already registered");
            }

            //CustomIdentityUser user = new IdentityUser() { UserName = model.UserName };
            // Register the user in our database
            CreateUserCommand createUserCommand = new CreateUserCommand(registerExternalUserCommand.FullName, 
                registerExternalUserCommand.Email, null, null);
            var registeredUserId = Register(createUserCommand, true);
            _logger.Info("External User registered successfuly");
            if (string.IsNullOrEmpty(registeredUserId))
            {
                throw new SystemException("An error occured while registering external user");
            }
            // Add the external user's details related to the external account which they are using to register in our system
            var info = new ExternalLoginInfo()
            {
                DefaultUserName = registerExternalUserCommand.Email,
                Login = new UserLoginInfo(registerExternalUserCommand.Provider, verifiedAccessToken.UserId)
            };

            this.AddLogin(registeredUserId, info.Login);
            
            //generate access token response
            var localAccessToken = GenerateLocalAccessTokenResponse(registerExternalUserCommand.Email);
            if (!string.IsNullOrWhiteSpace(localAccessToken))
            {
                _logger.Info("RegisterExternal: LocalAccessToken craeted successfully");
            }

            return new InternalLoginDataRepresentation(registerExternalUserCommand.FullName, registerExternalUserCommand.Email, 
                localAccessToken);
        }

        /// <summary>
        /// Provides external access token to retrieve a local token, by which this backend API can be accessed
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="externalAccessToken"></param>
        /// <returns></returns>
        public InternalLoginDataRepresentation ObtainAccessToken(string provider, string externalAccessToken)
        {
            if (string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(externalAccessToken))
            {
                throw new NullReferenceException("Provider or external access token is not sent");
            }

            // Verify that the External is legit and is providing us correct external access token
            var verifiedAccessToken = VerifyExternalAccessToken(provider, externalAccessToken);
            if (verifiedAccessToken == null)
            {
                throw new InvalidOperationException("Invalid Provider or External Access Token");
            }

            // Check that this user exists in our database
            var user = this.GetUserByUserLoginInfoRepresentation(new UserLoginInfo(provider, verifiedAccessToken.UserId));
            bool hasRegistered = user != null;
            if (!hasRegistered)
            {
                throw new InvalidOperationException("External user is not registered");
            }

            // Generate access token response
            var localAccessToken = GenerateLocalAccessTokenResponse(user.Email);
            if (!string.IsNullOrWhiteSpace(localAccessToken))
            {
                _logger.Info("RegisterExternal: LocalAccessToken craeted successfully");
            }
            return new InternalLoginDataRepresentation(user.FullName, user.Email, localAccessToken);
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

        public bool UserExistsByUserLoginInfo(UserLoginInfo userLoginInfo)
        {
            CustomIdentityUser customIdentityUser = _accountRepository.GetUserByUserLoginInfo(userLoginInfo);
            if (customIdentityUser != null)
            {
                return true;
            }
            return false;
        }

        public UserRepresentation GetUserByUserLoginInfoRepresentation(UserLoginInfo userLoginInfo)
        {
            CustomIdentityUser customIdentityUser = _accountRepository.GetUserByUserLoginInfo(userLoginInfo);
            if (customIdentityUser != null)
            {
                return new UserRepresentation(customIdentityUser.FullName, customIdentityUser.Email, customIdentityUser.EmailConfirmed);
            }
            throw new NullReferenceException("No user found with the given UserLoginInfo");
        }
        
        /// <summary>
        /// Sends user token to reset password
        /// </summary>
        /// <param name="forgotPasswordCommand"></param>
        public void ForgotPassword(ForgotPasswordCommand forgotPasswordCommand)
        {
            if (string.IsNullOrWhiteSpace(forgotPasswordCommand.Email))
            {
                throw new NullReferenceException("Email for ForgotPassword is empty");
            }
            var user = _accountRepository.GetUserByEmail(forgotPasswordCommand.Email);
            if (user == null)
            {
                throw new NullReferenceException("Email not found");
            }
            if (user.EmailConfirmed)
            {
                // Generate the token
                var resetPasswordToken = _accountRepository.GetPasswordResetToken(user.Id);
                
                Task.Run(() => SendPasswordResetEmail(user.Email, resetPasswordToken, user.FullName));
                // Update the password reset settings
                user.IsPasswordResetRequested = true;
                user.PasswordResetExpiryDate = DateTime.Now.AddHours(24);
                _accountRepository.UpdateUser(user);
                _logger.Info("Password Reset request accepted for this user. Email: {0}", forgotPasswordCommand.Email);
            }
            else
            {
                throw new InvalidOperationException("Cannot reset password");
            }
        }

        private void SendPasswordResetEmail(string email, string resetPasswordToken, string userFullName)
        {
            // Create the link that will be sent in the email
            var passwordResetEmailBody = Constants.FrontEndUrl + "/" + Constants.PasswordResetUrlLocation + "?email="
                + email + "&resettoken=" + resetPasswordToken;
            // Sned the email with the generated token within the generated link
            _emailService.SendEmail(email, EmailConstants.PasswordResetSubject,
                EmailConstants.PasswordResetEmail(userFullName, passwordResetEmailBody));
        }

        /// <summary>
        ///  Reset the password for the user when they provide a new one after clicking on the link sent to them on email
        /// </summary>
        /// <param name="resetPasswordCommand"></param>
        /// <returns></returns>
        public bool ResetPassword(ResetPasswordCommand resetPasswordCommand)
        {
            if (string.IsNullOrWhiteSpace(resetPasswordCommand.Email))
            {
                throw new NullReferenceException("No email received");
            }
            if (string.IsNullOrWhiteSpace(resetPasswordCommand.Password))
            {
                throw new NullReferenceException("No password received");
            }
            if (string.IsNullOrWhiteSpace(resetPasswordCommand.ConfirmPassword))
            {
                throw new NullReferenceException("No confirm password received");
            }
            if (string.IsNullOrWhiteSpace(resetPasswordCommand.Token))
            {
                throw new NullReferenceException("No token provided");
            }
            if (!resetPasswordCommand.Password.Equals(resetPasswordCommand.ConfirmPassword))
            {
                throw new NullReferenceException("Passwords do not match");
            }
            
            var user = _accountRepository.GetUserByEmail(resetPasswordCommand.Email);
            if (user == null)
            {
                throw new NullReferenceException("User could not be found");
            }
            if (user.IsPasswordResetRequested)
            {
                if (user.PasswordResetExpiryDate >= DateTime.Now)
                {
                    var resetPasswordResponse = _accountRepository.ResetPassword(user.Id, resetPasswordCommand.Token,
                        resetPasswordCommand.Password);
                    if (resetPasswordResponse)
                    {
                        user.IsPasswordResetRequested = false;
                        user.PasswordResetExpiryDate = null;
                        _accountRepository.UpdateUser(user);
                        _logger.Info("Password reset successful. Email: {0}", resetPasswordCommand.Email);
                        return true;
                    }
                }
                else
                {
                    user.IsPasswordResetRequested = false;
                    user.PasswordResetExpiryDate = null;
                    _accountRepository.UpdateUser(user);
                    _logger.Warn("Password reset token has expired: Email {0}", resetPasswordCommand.Email);
                    return false;
                }
            }
            else
            {
                _logger.Warn("Password reset is not requested for this account. Email: {0}", resetPasswordCommand.Email);
                throw new InvalidOperationException("Reset password not requested so the password wont be reset");
            }
            return false;
        }

        /// <summary>
        /// Add externa user's login details
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userLoginInfo"></param>
        /// <returns></returns>
        public bool AddLogin(string userId, UserLoginInfo userLoginInfo)
        {
            var identityResult = _accountRepository.AddLogin(userId, userLoginInfo);
            if (identityResult != null && identityResult.Succeeded)
            {
                return true;
            }
            throw new ArgumentException("Could not save UserLoginInfo");
        }

        public void Dispose()
        {
            _accountRepository.Dispose();
        }

        #region Private Methods

        private ParsedExternalAccessTokenRepresentation VerifyExternalAccessToken(string provider, string accessToken)
        {
            ParsedExternalAccessTokenRepresentation parsedToken = null;

            var verifyTokenEndPoint = "";

            if (provider == "Facebook")
            {
                //You can get it from here: https://developers.facebook.com/tools/accesstoken/
                //More about debug_tokn here: http://stackoverflow.com/questions/16641083/how-does-one-get-the-app-access-token-for-debug-token-inspection-on-facebook

                var appToken = RentStuff.Common.Utilities.Constants.FacebookAcccessToken;
                verifyTokenEndPoint = string.Format("https://graph.facebook.com/debug_token?input_token={0}&access_token={1}", accessToken, appToken);
            }
            else if (provider == "Google")
            {
                verifyTokenEndPoint = string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}", accessToken);
            }
            else
            {
                return null;
            }

            var client = new HttpClient();
            var uri = new Uri(verifyTokenEndPoint);
            var response = client.GetAsync(uri);
            response.Wait(5000);
            if (response.Result.IsSuccessStatusCode)
            {
                var content = response.Result.Content.ReadAsStringAsync();
                content.Wait(5000);
                dynamic jObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content.Result);
                
                if (provider == "Facebook")
                {
                    parsedToken = new ParsedExternalAccessTokenRepresentation(jObj["data"]["user_id"].ToString(), jObj["data"]["app_id"].ToString());
                    
                    if (!string.Equals(FacebookAuthenticationOptions.AppId, parsedToken.AppId, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                }
                /*else if (provider == "Google")
                {
                    parsedToken.user_id = jObj["user_id"];
                    parsedToken.app_id = jObj["audience"];

                    if (!string.Equals(googleAuthOptions.ClientId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }

                }*/

            }

            return parsedToken;
        }

        private string GenerateLocalAccessTokenResponse(string email)
        {

            var tokenExpiration = TimeSpan.FromDays(1);

            ClaimsIdentity identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Email, email));
            identity.AddClaim(new Claim("role", "user"));

            var props = new Microsoft.Owin.Security.AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
            };

            var ticket = new AuthenticationTicket(identity, props);

            var accessToken = OAuthBearerAuthenticationOptions.AccessTokenFormat.Protect(ticket);
            return accessToken;

            /* --- The following code is not necessary, we only need to return the access token ---
            JObject tokenResponse = new JObject(
                new JProperty("userName", userName),
                new JProperty("access_token", accessToken),
                new JProperty("token_type", "bearer"),
                new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
                new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
                new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
            );

            return tokenResponse;*/
        }

        /// <summary>
        /// Creates a new ExternalLoginDataRepresentation given a ClaimsIdentity
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static ExternalLoginDataRepresentation ExternalLoginRepresentationFromIdentity(ClaimsIdentity identity)
        {
            if (identity == null)
            {
                throw new NullReferenceException("ClaimsIdentity is null");
            }

            Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

            if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer) || String.IsNullOrEmpty(providerKeyClaim.Value))
            {
                return null;
            }

            if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
            {
                return null;
            }

            return new ExternalLoginDataRepresentation(
                providerKeyClaim.Issuer, 
                providerKeyClaim.Value,
                identity.FindFirstValue(ClaimTypes.Name), 
                identity.FindFirstValue(ClaimTypes.Email),
                identity.FindFirstValue("ExternalAccessToken"));
        }

        #endregion Private Methods

        private static FacebookAuthenticationOptions _facebookAuthenticationOptions;
        
        public static FacebookAuthenticationOptions FacebookAuthenticationOptions
        {
            get
            {
                if (_facebookAuthenticationOptions == null)
                {
                    _facebookAuthenticationOptions = new FacebookAuthenticationOptions()
                    {
                        AppId = RentStuff.Common.Utilities.Constants.FacebookAppId,
                        AppSecret = RentStuff.Common.Utilities.Constants.FacebookAppSecret,
                        Provider = new FacebookAuthProvider()
                    };
                    _facebookAuthenticationOptions.Scope.Add(Constants.FacebookEmailScope);
                    _facebookAuthenticationOptions.BackchannelHttpHandler = new HttpClientHandler();
                    _facebookAuthenticationOptions.UserInformationEndpoint = Constants.FacebookUserInformationEndpoint;
                }
                return _facebookAuthenticationOptions;
            }
        }

        private static OAuthBearerAuthenticationOptions _oAuthBearerAuthenticationOptions;

        public static OAuthBearerAuthenticationOptions OAuthBearerAuthenticationOptions
        {
            get
            {
                if (_oAuthBearerAuthenticationOptions == null)
                {
                    _oAuthBearerAuthenticationOptions = new OAuthBearerAuthenticationOptions();
                }
                return _oAuthBearerAuthenticationOptions;
            }
        }
    }
}
