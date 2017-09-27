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
    /// <summary>
    /// Handles the workflow of operations related to user accounts
    /// </summary>
    public class AccountApplicationService : IAccountApplicationService
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private ICustomEmailService _emailService;
        private IAccountRepository _accountRepository;

        /// <summary>
        /// Initialize with repository and email service
        /// </summary>
        /// <param name="accountRepository"></param>
        /// <param name="customEmailService"></param>
        public AccountApplicationService(IAccountRepository accountRepository, 
            ICustomEmailService customEmailService)
        {
            _accountRepository = accountRepository;
            _emailService = customEmailService;
        }
        
        /// <summary>
        /// Registers the user
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="isExternalUser"></param>
        /// <returns></returns>
        public string Register(CreateUserCommand userModel, bool isExternalUser = false)
        {
            // Check all the criteria for the data contained in the model
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
            _logger.Info("Registering user: Email = {0} | FullName = {1}", userModel.Email, userModel.FullName);
            
            // Register the User in the repository
            IdentityResult registrationResult = _accountRepository.RegisterUser(userModel.FullName, userModel.Email, 
                                                                                userModel.Password, isExternalUser);

            // If the registration response is null, throw error
            if (registrationResult == null)
            {
                throw new NullReferenceException("Whoa! Unexpected error happened while registering the user. Didnt expect that");
            }
            // If the registration failed, raise exception
            if (!registrationResult.Succeeded)
            {
                throw new InvalidOperationException(registrationResult.Errors.First());
            }
            _logger.Info("Registered User Successfuly. Email: {0}  FullName: {1}", userModel.Email, userModel.FullName);
            // Get the User instance to have her Id
            var retreivedUser = _accountRepository.GetUserByEmail(userModel.Email);

            // Generate the token for this user using email and user Id
            var emailVerificationToken = _accountRepository.GetEmailActivationToken(retreivedUser.Id);

            // If the token is not generated, throw an exception
            if (string.IsNullOrWhiteSpace(emailVerificationToken))
            {
                _logger.Error("Error while generating email confirmation token for user. Email: {0}", userModel.Email);
                throw new NullReferenceException("Could not generate token for user: " + retreivedUser.Id);
            }
            // External user is someone who logged in through a third party platform such as Facebook or Twitter,
            // if the current requester is not an external user, send a confirmation mail to their email address,
            // as they need to confirm the email with which they registered
            if (!isExternalUser)
            {
                // Send email to the user
                #pragma warning disable 4014
                Task.Run(() => SendActivationEmail(retreivedUser.Email, retreivedUser.FullName,
                    emailVerificationToken));
                #pragma warning restore 4014
            }
            // If it's an external user, then confirm the email automatically
            else
            {
                _accountRepository.ConfirmEmail(retreivedUser.Id, emailVerificationToken);
            }
            return retreivedUser.Id;
        }

        /// <summary>
        /// Send email to the user's email address so they can verify it.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="fullName"></param>
        /// <param name="activationCode"></param>
        private void SendActivationEmail(string email, string fullName, string activationCode)
        {
            var activationLink = Constants.FrontEndUrl + "/" + Constants.AccountActivationUrlLocation + "?email=" + email +
                                 "&activationcode=" + activationCode;
            _emailService.SendEmail(email, EmailConstants.ActivationEmailSubject, EmailConstants.ActivationEmailMessage(fullName, activationLink));
        }
        
        /// <summary>
        /// When the user clicks on their email link to activate, we handle it here to activate the user's account
        /// </summary>
        /// <param name="activateAccountCommand"></param>
        /// <returns></returns>
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
            // Mark the email as verified in the database
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
        }
        
        /// <summary>
        /// Saves user who registers using Facebook or other third party account
        /// </summary>
        /// <returns></returns>
        public InternalLoginDataRepresentation RegisterExternalUser(RegisterExternalUserCommand registerExternalUserCommand)
        {
            _logger.Info("RegisterExternal Application Layer: Email = {0} | FullName = {1}",
                registerExternalUserCommand.Email, registerExternalUserCommand.FullName);

            var verifiedAccessToken = ProcessExternalAccessToken(registerExternalUserCommand.Provider, registerExternalUserCommand.ExternalAccessToken);

            // Check if the user is not already registered with our app
            var hasRegistered = this.UserExistsByUserLoginInfo(new UserLoginInfo(registerExternalUserCommand.Provider, verifiedAccessToken.UserId));
            if (hasRegistered)
            {
                throw new InvalidOperationException("External user is already registered");
            }
            
            CreateUserCommand createUserCommand = new CreateUserCommand(registerExternalUserCommand.FullName, 
                registerExternalUserCommand.Email, null, null);
            _logger.Info("RegisterExternal sending request to register user: Email = {0} | FullName = {1}", 
                createUserCommand.Email, createUserCommand.FullName);

            // Register the user in our database
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

            // Add information for this user as an external user, containing information with the third-party 
            // account they logged in with.
            this.AddLogin(registeredUserId, info.Login);
            _logger.Error("ExternalLoginInfo saved to database successfully. UserId = {0} | ExternalId = {1} | Email = {2}", 
                registeredUserId, verifiedAccessToken.UserId, registerExternalUserCommand.Email);
            
            // Generate access token response
            var localAccessToken = GenerateLocalAccessTokenResponse(registerExternalUserCommand.Email);
            if (!string.IsNullOrWhiteSpace(localAccessToken))
            {
                _logger.Info("RegisterExternal: LocalAccessToken created successfully");
            }

            // Return control to controller, passing the access token used for registering users using third party
            // platforms. This token will be used to provide this user with a token to login in our app. This 
            // procedure is required in order to complete the third-party login confirmation window loop.
            return new InternalLoginDataRepresentation(registerExternalUserCommand.FullName, registerExternalUserCommand.Email, 
                localAccessToken);
        }

        /// <summary>
        /// Provides external access token to retrieve a local token, by which this backend API can be accessed
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="internalId"></param>
        /// <returns></returns>
        public InternalLoginDataRepresentation ObtainAccessToken(string provider, string internalId)
        {
            if (string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(internalId))
            {
                throw new NullReferenceException("Provider or external access token is not sent");
            }

            // Check and verify the external access token provided by the external user
            var verifiedAccessToken = ProcessExternalAccessToken(provider, internalId);

            // Check that this user exists in our database
            var user = this.GetUserByUserLoginInfoRepresentation(new UserLoginInfo(provider, verifiedAccessToken.UserId));
            bool hasRegistered = user != null;
            if (!hasRegistered)
            {
                throw new InvalidOperationException("External user is not registered");
            }

            // Generate access token response that the user will use to login in subsequent login requests.
            var localAccessToken = GenerateLocalAccessTokenResponse(user.Email);
            if (!string.IsNullOrWhiteSpace(localAccessToken))
            {
                _logger.Info("RegisterExternal: LocalAccessToken created successfully");
            }
            // Represent the token that will now provide the user with access to our application
            return new InternalLoginDataRepresentation(user.FullName, user.Email, localAccessToken);
        }

        /// <summary>
        /// Gets ExternalAccessToken using the Internal Id, verifies the ExternalAccessToken, deletes the ExternalAccessTokenIdentifier
        /// and returns the VerifiedAccessToken
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="internalId"></param>
        /// <returns></returns>
        private ParsedExternalAccessTokenRepresentation ProcessExternalAccessToken(string provider, string internalId)
        {
            // Now get the ExternalAccessToken using the InternalId that we passed to the frontend
            var externalAccessTokenIdentifier = this.GetExternalAccessTokenIdentifier(internalId);
            // Verify that the External is legit and is providing us correct external access token
            var verifiedAccessToken = VerifyExternalAccessToken(provider, externalAccessTokenIdentifier.ExternalAccessToken);
            if (verifiedAccessToken == null)
            {
                throw new InvalidOperationException("Invalid Provider or External Access Token");
            }

            // Now Delete the ExternalAccessToken <--> InternalId Mapping
            _accountRepository.DeleteExternalAccessTokenIdentifier(externalAccessTokenIdentifier);

            return verifiedAccessToken;
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
        /// Check that the given user exists
        /// </summary>
        /// <param name="userLoginInfo"></param>
        /// <returns></returns>
        public bool UserExistsByUserLoginInfo(UserLoginInfo userLoginInfo)
        {
            CustomIdentityUser customIdentityUser = _accountRepository.GetUserByUserLoginInfo(userLoginInfo);
            if (customIdentityUser != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the user by their login info
        /// </summary>
        /// <param name="userLoginInfo"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Send the email with instructions to reset the password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="resetPasswordToken"></param>
        /// <param name="userFullName"></param>
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
            
            // Get the user
            var user = _accountRepository.GetUserByEmail(resetPasswordCommand.Email);
            if (user == null)
            {
                throw new NullReferenceException("User could not be found");
            }
            // Make sure this user actually requested a password change
            if (user.IsPasswordResetRequested)
            {
                // Check if expiry date of the password reset has not elapsed.
                if (user.PasswordResetExpiryDate >= DateTime.Now)
                {
                    // Reset pasword
                    var resetPasswordResponse = _accountRepository.ResetPassword(user.Id, resetPasswordCommand.Token,
                        resetPasswordCommand.Password);
                    // If the response is successful, update the fields in the database
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
        /// Add external user's login details
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

        /// <summary>
        /// Maps the ExternalAccessToken to an internal Id generated, well, internally. This way we never expose the ExternalAccessToken
        /// to the outside world
        /// </summary>
        /// <param name="externalAccessToken"></param>
        /// <returns>InternalId that maps to the ExternalAccessToken</returns>
        public string MapExternalAccessTokenToInternalId(string externalAccessToken)
        {
            var externalAccessTokenIdentifier = _accountRepository.SaveExternalAccessTokenIdentifier(
                new ExternalAccessTokenIdentifier(externalAccessToken));
            if (externalAccessTokenIdentifier != null)
            {
                return externalAccessTokenIdentifier.InternalId;
            }
            else
            {
                throw new NullReferenceException(string.Format("Error while saving the ExternalAccessTokenIdentifier. ExternalAccessToken: {0}", externalAccessToken));
            }
        }

        /// <summary>
        /// Retrieves the ExternalAccessToken for an existing ExternalAccessTokenIdentifier instance by providing the InternalId
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns>ExternalAccessToken</returns>
        public ExternalAccessTokenIdentifier GetExternalAccessTokenIdentifier(string internalId)
        {
            var externalAccessTokenIdentifier = _accountRepository.GetExternalAccessIdentifierByInternalId(internalId);
            if (externalAccessTokenIdentifier != null)
            {
                return externalAccessTokenIdentifier;
            }
            else
            {
                throw new NullReferenceException("Error while retrieving ExternalAccessTokenidentifier. InternalId: " + internalId);
            }
        }
        
        /// <summary>
        /// Dispose off the resources
        /// </summary>
        public void Dispose()
        {
            _accountRepository.Dispose();
        }

        #region Private Methods

        /// <summary>
        /// Convert the external token domain object to it's representation
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Generate Local Access Token. 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// Facebook Authentication
        /// </summary>
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

        /// <summary>
        /// OAuth Bearer Authentication Options
        /// </summary>
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
