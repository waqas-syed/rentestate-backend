using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using RentStuff.Identity.Application.Account;
using RentStuff.Identity.Application.Account.Commands;
using RentStuff.Identity.Application.Account.Representations;
using RentStuff.Identity.Ports.Adapter.Rest.DTOs;
using RentStuff.Identity.Ports.Adapter.Rest.Results;

namespace RentStuff.Identity.Ports.Adapter.Rest.Resources
{
    /// <summary>
    /// Controller to handle requests related to accounts
    /// </summary>
    [RoutePrefix("v1/Account")]
    public class AccountController : ApiController
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private IAccountApplicationService _accountApplicationService;
        
        /// <summary>
        /// Initialize the controller
        /// </summary>
        /// <param name="accountApplicationService"></param>
        public AccountController(IAccountApplicationService accountApplicationService)
        {
            _accountApplicationService = accountApplicationService;
        }

        /// <summary>
        /// Authentication attribute from Owin's Context
        /// </summary>
        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        /// <summary>
        /// This method verifies the user is a legitimate user of the third party app(Facebook, Twitter etc) by 
        /// OWIN's pipeline to request the third party to authenticate the user and when successful, redirects the 
        /// user to the front end client by providing a token that will be used to register this user with our 
        /// system
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            try
            {
                _logger.Info("External Login Request Recieved for provider: {0}", provider);
                string redirectUri = string.Empty;

                // If there was an error during authentication reported by the third party, we cannot proceed
                if (error != null)
                {
                    return BadRequest(Uri.EscapeDataString(error));
                }

                // If the user is not yet authenticated, submit the ChallengeResult so the OWIN middleware can 
                // request the third party to authenticate the user
                if (!User.Identity.IsAuthenticated)
                {
                    return new ChallengeResult(provider, this);
                }

                // Validates the ClientId and RedirectUri, throws exception if values are not valid
                ValidateClientAndRedirectUri(this.Request, ref redirectUri);
                
                // Get the Identity information as a ClaimsIdentity
                ExternalLoginDataRepresentation externalLogin = 
                    AccountApplicationService.ExternalLoginRepresentationFromIdentity(User.Identity as ClaimsIdentity);
                
                // If the claims Identity for the external user is null, return error
                if (externalLogin == null)
                {
                    _logger.Error("External Login could not be processed");
                    return InternalServerError();
                }

                _logger.Info("ExternalLogin request properties: Email = {0} | FullName = {1} | ExternalAccessToken = {2}",
                    externalLogin.Email, externalLogin.FullName, externalLogin.ExternalAccessToken);

                // If the provider (Facebook, Twitter etc) in the claim and the one requested for this operation
                // are not the same, request a new login from the user using a ChallengeResult
                if (externalLogin.LoginProvider != provider)
                {
                    Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    return new ChallengeResult(provider, this);
                }
                
                // Does the user already exist?
                bool hasRegistered = _accountApplicationService.UserExistsByUserLoginInfo(new UserLoginInfo(externalLogin.LoginProvider,
                        externalLogin.ProviderKey));

                // Get the internal Id for the external access token
                string internalIdForExternalAccessToken = _accountApplicationService.MapExternalAccessTokenToInternalId(externalLogin.ExternalAccessToken);
                
                // Redirect uri. We are only supporting Facebook at this time so only use this for Facebook
                redirectUri = string.Format(
                    "{0}?external_access_token={1}&provider={2}&haslocalaccount={3}&email={4}&full_name={5}",
                    RentStuff.Common.Utilities.Constants.FacebookRedirectUri,
                    internalIdForExternalAccessToken,
                    externalLogin.LoginProvider,
                    hasRegistered.ToString(),
                    externalLogin.Email,
                    externalLogin.FullName);
                _logger.Info("FacebookRedirectUri = {0}", redirectUri);
                
                // Redirect with the given internal token for registration. This will automatically submit this 
                // token in the next action below, but we need to follow this procedure for proper redirection
                // for the frontend client after the third part authentication window opens
                return Redirect(redirectUri);
            }
            catch (Exception e)
            {
                // Return internal server error in case of an exception that was not handled by our code
                _logger.Error(e, e.Message);
                return InternalServerError();
            }
        }

        /// <summary>
        /// After the user has been redirected to the frontend client, this action is called to register the user 
        /// using the internal token that was provided in the redirect action above
        /// </summary>
        /// <param name="registerExternalUserCommand"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("register-external")]
        public IHttpActionResult RegisterExternal([FromBody]RegisterExternalUserCommand registerExternalUserCommand)
        {
            try
            {
                // Check that the model state is valid
                if (!ModelState.IsValid)
                {
                    _logger.Error("RegisterExternal's model state is invalid");
                    return BadRequest(ModelState);
                }
                _logger.Info("RegisterExternal user request recieved: Email = {0} | FullName = {1} | ExternalAccessToken = {2} | " +
                              "Provider = {3}", registerExternalUserCommand.Email, registerExternalUserCommand.FullName, 
                              registerExternalUserCommand.ExternalAccessToken, registerExternalUserCommand.Provider);
                // Register the user in the database, log the user in, and provide an access token for authenticating
                // in subsequent requests
                var accessToken = _accountApplicationService.RegisterExternalUser(registerExternalUserCommand);
                // Return an ok result
                return Ok(accessToken);
            }
            catch (Exception e)
            {
                // Return internal server error in case of an exception that was not handled by our code
                _logger.Error("Error while registering external user. Exception: {0}", e.ToString());
                return InternalServerError();
            }
        }

        /// <summary>
        /// Registered users use this action to obtain an access token to login
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="externalAccessToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("obtain-local-access-token")]
        public async Task<IHttpActionResult> ObtainLocalAccessToken(string provider, string externalAccessToken)
        {
            try
            {
                // Get the token
                var accessToken = _accountApplicationService.ObtainAccessToken(provider, externalAccessToken);

                // Return
                return Ok(accessToken);
            }
            catch (Exception e)
            {
                // Return internal server error in case of an exception that was not handled by our code
                _logger.Error(e, "Error while obtaining access token for external user");
                return InternalServerError();
            }
        }

        /// <summary>
        /// Register an account using an email
        /// </summary>
        /// <param name="userObject"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("Register")]
        public IHttpActionResult Register([FromBody] Object userObject)
        {
            try
            {
                // Check for null reference
                if (userObject != null)
                {
                    // Convert JSON into a POCO object. This is because there were some issues in catching the 
                    // object directly
                    var jsonString = userObject.ToString();
                    var createUserCommand = JsonConvert.DeserializeObject<CreateUserCommand>(jsonString);

                    if (createUserCommand != null)
                    {
                        // Register the user
                        _logger.Info("Starting process to create a new user. Email: {0}", createUserCommand.Email);
                        string identityResult = _accountApplicationService.Register(createUserCommand);
                        // If the user was registered successfully, return ok
                        if (!string.IsNullOrWhiteSpace(identityResult))
                        {
                            return Ok(true);
                        }
                        // Otherwise return an error
                        else
                        {
                            _logger.Error("Could not register User. Email: {0}", createUserCommand.Email);
                            return BadRequest("Could not register user");
                        }
                    }
                    else
                    {
                        // If the json data could not be converted successfully.
                        _logger.Error("Could not cast retreived object to CreateUserCommand. Object: {0}", userObject);
                        return BadRequest("User data not in expected format");
                    }
                }
                else
                {
                    // If the recieved object is null
                    _logger.Error("Register User object is null");
                    return BadRequest("No user data received");
                }
            }
            catch (Exception exception)
            {
                // Return internal server error in case of an exception that was not handled by our code
                _logger.Error("Error while registering user. {0}", exception.ToString());
                return InternalServerError();
            }
        }

        /// <summary>
        /// Activate the account when the email is confirmed
        /// </summary>
        /// <param name="activateAccountObject"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("activate-account")]
        public IHttpActionResult Activate([FromBody] Object activateAccountObject)
        {
            try
            {
                // Check for null reference
                if (activateAccountObject != null)
                {
                    // Convert JSON into a POCO object. This is because there were some issues in catching the 
                    // object directly
                    var jsonString = activateAccountObject.ToString();
                    var activateAccountCommand = JsonConvert.DeserializeObject<ActivateAccountCommand>(jsonString);

                    // If the object is okay.
                    if (activateAccountCommand != null)
                    {
                        _logger.Info("Starting process to activate user by email. Email: {0}", activateAccountCommand.Email);
                        // Activate the account and return the result
                        bool activationResult = _accountApplicationService.Activate(activateAccountCommand);
                        
                        // If account is activated, return ok
                        if (activationResult)
                        {
                            return Ok();
                        }
                        // Otherwise return error
                        else
                        {
                            _logger.Error("Could not activate account. Emai: {0}", activateAccountCommand.Email);
                            return BadRequest("Could not activate account");
                        }
                    }
                    // When JSON is not in expected format
                    else
                    {
                        _logger.Error("Could not parse object to ActivateAccountCommand. Object: {0}", activateAccountObject);
                        return BadRequest("User data not in expected format");
                    }
                }
                // Null object recieved
                else
                {
                    _logger.Error("Received ActivateAccount object is null.");
                    return BadRequest("No user data received");
                }
            }
            catch (Exception exception)
            {
                // Return internal server error in case of an exception that was not handled by our code
                _logger.Error("Error while activating user account. {0}", exception.ToString());
                return InternalServerError();
            }
        }

        /// <summary>
        /// For requests for resetting passwords
        /// </summary>
        /// <param name="forgotPasswordCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("forgot-password")]
        [AllowAnonymous]
        public IHttpActionResult ForgotPassword(ForgotPasswordCommand forgotPasswordCommand)
        {
            try
            {
                // Check for null reference and empty string
                if (forgotPasswordCommand != null)
                {
                    if (string.IsNullOrWhiteSpace(forgotPasswordCommand.Email))
                    {
                        _logger.Error("ForgotPassword email is empty");
                        return BadRequest("ForgotPassword email is empty");
                    }
                    _logger.Info("Forgot-password process started. Email: {0}", forgotPasswordCommand.Email);
                    // Process a forgot password request
                    _accountApplicationService.ForgotPassword(forgotPasswordCommand);
                    return Ok();
                }
                else
                {
                    _logger.Error("ForgotPasswordCommand is null");
                    return BadRequest("No data received");
                }
            }
            catch (Exception exception)
            {
                // Return internal server error in case of an exception that was not handled by our code
                _logger.Error("Error while requesting to reset password. {0}", exception.ToString());
                return InternalServerError();
            }
        }

        /// <summary>
        /// When the user submits the token that was sent to him for the resetting of a password
        /// </summary>
        /// <param name="resetPasswordCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("reset-password")]
        [AllowAnonymous]
        public IHttpActionResult ResetPassword(ResetPasswordCommand resetPasswordCommand)
        {
            try
            {
                // Check for null reference
                if (resetPasswordCommand == null)
                {
                    return BadRequest("ResetpasswordCommand object is null");
                }
                _logger.Info("Reset-password process started.");
                // Send to application service for the password to get reset
                _accountApplicationService.ResetPassword(resetPasswordCommand);
                return Ok();
            }
            catch (Exception exception)
            {
                _logger.Error("Error while resetting the password. {0}", exception.ToString());
                return InternalServerError();
            }
        }

        /// <summary>
        /// Get the user by their email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("get-user")]
        [HttpGet]
        public IHttpActionResult GetUser(string email)
        {
            try
            {
                // Check for empty string
                if (!string.IsNullOrWhiteSpace(email))
                {
                    _logger.Info("Get user process started. Email: {0}", email);
                    // Get the user by email and return it to the client
                    UserRepresentation user = _accountApplicationService.GetUserByEmail(email);
                    return Ok(user);
                }
                else
                {
                    _logger.Error("No Email provided to GetUser");
                    return BadRequest("No email provided to GetUser");
                }
            }
            catch (Exception exception)
            {
                // Return internal server error in case of an exception that was not handled by our code
                _logger.Error("Error while getting user. {0}", exception.ToString());
                return InternalServerError();
            }
        }

        protected override void Dispose(bool disposing)
        {
            /*if (disposing)
            {
                _accountApplicationService.Dispose();
            }

            base.Dispose(disposing);*/
        }
        
        #region Private Methods

        /// <summary>
        /// Validate the client and the redirect uri it provided
        /// </summary>
        /// <param name="request"></param>
        /// <param name="redirectUriOutput"></param>
        /// <returns></returns>
        private bool ValidateClientAndRedirectUri(HttpRequestMessage request, ref string redirectUriOutput)
        {
            Uri redirectUri;

            var redirectUriString = GetQueryString(Request, "redirect_uri");

            if (string.IsNullOrWhiteSpace(redirectUriString))
            {
                throw new ArgumentException("redirect_uri is required");
            }

            bool validUri = Uri.TryCreate(redirectUriString, UriKind.Absolute, out redirectUri);

            if (!validUri)
            {
                throw new ArgumentException("redirect_uri is invalid");
            }

            var clientId = GetQueryString(Request, "client_id");

            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new ArgumentException("client_Id is required");
            }
            
            // Check if the Frontend client Id is what we are expecting
            if (!clientId.Equals(RentStuff.Common.Utilities.Constants.FrontendClientId))
            {
                throw new ArgumentException($"Client_id '{clientId}' is not registered in the system.");
            }

            /*if (!string.Equals(client.AllowedOrigin, redirectUri.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("The given URL is not allowed by Client_id '{0}' configuration.", clientId);
            }*/

            redirectUriOutput = redirectUri.AbsoluteUri;

            return true;
        }

        /// <summary>
        /// Get the query string from the request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetQueryString(HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();

            if (queryStrings == null) return null;

            var match = queryStrings.FirstOrDefault(keyValue => string.Compare(keyValue.Key, key, true) == 0);

            if (string.IsNullOrEmpty(match.Value)) return null;

            return match.Value;
        }

        #endregion Private Methods

        #region Properties

        public static OAuthBearerAuthenticationOptions OAuthBearerAuthenticationOptions => AccountApplicationService.OAuthBearerAuthenticationOptions;

        public static FacebookAuthenticationOptions FacebookAuthenticationOptions => AccountApplicationService.FacebookAuthenticationOptions;
        
        #endregion Properties
    }
}