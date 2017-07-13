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
    [RoutePrefix("v1/Account")]
    //[EnableCors(origins: "http://mywebclient.azurewebsites.net", headers: "*", methods: "*")]
    public class AccountController : ApiController
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private IAccountApplicationService _accountApplicationService;
        
        public AccountController(IAccountApplicationService accountApplicationService)
        {
            _accountApplicationService = accountApplicationService;
        }

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        // GET api/Account/ExternalLogin
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

                if (error != null)
                {
                    return BadRequest(Uri.EscapeDataString(error));
                }

                if (!User.Identity.IsAuthenticated)
                {
                    return new ChallengeResult(provider, this);
                }

                // Validates the ClientId and RedirectUri, throws exception if values are not valid
                ValidateClientAndRedirectUri(this.Request, ref redirectUri);
                
                ExternalLoginDataRepresentation externalLogin = 
                    AccountApplicationService.ExternalLoginRepresentationFromIdentity(User.Identity as ClaimsIdentity);

                if (externalLogin == null)
                {
                    _logger.Error("External Login could not be processed");
                    return InternalServerError();
                }

                if (externalLogin.LoginProvider != provider)
                {
                    Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    return new ChallengeResult(provider, this);
                }
                
                bool hasRegistered = _accountApplicationService.UserExistsByUserLoginInfo(new UserLoginInfo(externalLogin.LoginProvider,
                        externalLogin.ProviderKey));

                redirectUri = string.Format(
                    "{0}?external_access_token={1}&provider={2}&haslocalaccount={3}&full_name={4}&email={5}",
                    RentStuff.Common.Utilities.Constants.FacebookRedirectUri,
                    externalLogin.ExternalAccessToken,
                    externalLogin.LoginProvider,
                    hasRegistered.ToString(),
                    externalLogin.FullName,
                    externalLogin.Email);
                _logger.Info("FacebookRedirectUri = {0}", redirectUri);
                
                return Redirect(redirectUri);
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return InternalServerError();
            }
        }

        // POST api/Account/RegisterExternal
        [AllowAnonymous]
        [HttpPost]
        [Route("register-external")]
        public IHttpActionResult RegisterExternal([FromBody]RegisterExternalUserCommand registerExternalUserCommand)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.Error("RegisterExternal's model state is invalid");
                    return BadRequest(ModelState);
                }
                _logger.Debug("RegisterExternal user request recieved: Email = {0} | FullName = {1} | ExternalAccessToken = {2} | " +
                              "Provider = {3}", registerExternalUserCommand.Email, registerExternalUserCommand.FullName, 
                              registerExternalUserCommand.ExternalAccessToken, registerExternalUserCommand.Provider);
                var accessToken = _accountApplicationService.RegisterExternalUser(registerExternalUserCommand);
                return Ok(accessToken);
            }
            catch (Exception e)
            {
                _logger.Error("Error while registering external user. Exception: {0}", e.ToString());
                return InternalServerError();
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("obtain-local-access-token")]
        public async Task<IHttpActionResult> ObtainLocalAccessToken(string provider, string externalAccessToken)
        {
            try
            {
                var accessToken = _accountApplicationService.ObtainAccessToken(provider, externalAccessToken);

                return Ok(accessToken);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error while obtaining access token for external user");
                return InternalServerError();
            }
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public IHttpActionResult Register([FromBody] Object userObject)
        {
            try
            {
                if (userObject != null)
                {
                    var jsonString = userObject.ToString();
                    var createUserCommand = JsonConvert.DeserializeObject<CreateUserCommand>(jsonString);

                    if (createUserCommand != null)
                    {
                        _logger.Info("Starting process to create a new user. Email: {0}", createUserCommand.Email);
                        string identityResult = _accountApplicationService.Register(createUserCommand);
                        if (!string.IsNullOrWhiteSpace(identityResult))
                        {
                            return Ok(true);
                        }
                        else
                        {
                            _logger.Error("Could not register User. Email: {0}", createUserCommand.Email);
                            return BadRequest("Could not register user");
                        }
                    }
                    else
                    {
                        _logger.Error("Could not cast retreived object to CreateUserCommand. Object: {0}", userObject);
                        return BadRequest("User data not in expected format");
                    }
                }
                else
                {
                    _logger.Error("Register User object is null");
                    return BadRequest("No user data received");
                }
            }
            catch (Exception exception)
            {
                _logger.Error("Error while registering user. {0}", exception.ToString());
                return InternalServerError();
            }
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("activate-account")]
        public IHttpActionResult Activate([FromBody] Object activateAccountObject)
        {
            try
            {
                if (activateAccountObject != null)
                {
                    var jsonString = activateAccountObject.ToString();
                    var activateAccountCommand = JsonConvert.DeserializeObject<ActivateAccountCommand>(jsonString);

                    if (activateAccountCommand != null)
                    {
                        _logger.Info("Starting process to activate user by email. Email: {0}", activateAccountCommand.Email);
                        bool activationResult = _accountApplicationService.Activate(activateAccountCommand);
                        
                        if (activationResult)
                        {
                            return Ok();
                        }
                        else
                        {
                            _logger.Error("Could not activate account. Emai: {0}", activateAccountCommand.Email);
                            return BadRequest("Could not activate account");
                        }
                    }
                    else
                    {
                        _logger.Error("Could not parse object to ActivateAccountCommand. Object: {0}", activateAccountObject);
                        return BadRequest("User data not in expected format");
                    }
                }
                else
                {
                    _logger.Error("Received ActivateAccount object is null.");
                    return BadRequest("No user data received");
                }
            }
            catch (Exception exception)
            {
                _logger.Error("Error while activating user account. {0}", exception.ToString());
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("forgot-password")]
        [AllowAnonymous]
        public IHttpActionResult ForgotPassword(ForgotPasswordCommand forgotPasswordCommand)
        {
            try
            {
                if (forgotPasswordCommand != null)
                {
                    if (string.IsNullOrWhiteSpace(forgotPasswordCommand.Email))
                    {
                        _logger.Error("ForgotPassword email is empty");
                        return BadRequest("ForgotPassword email is empty");
                    }
                    _logger.Info("Forgot-password process started. Email: {0}", forgotPasswordCommand.Email);
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
                _logger.Error("Error while requesting to reset password. {0}", exception.ToString());
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("reset-password")]
        [AllowAnonymous]
        public IHttpActionResult ResetPassword(ResetPasswordCommand resetPasswordCommand)
        {
            try
            {
                if (resetPasswordCommand == null)
                {
                    return BadRequest("ResetpasswordCommand object is null");
                }
                _logger.Info("Reset-password process started.");
                _accountApplicationService.ResetPassword(resetPasswordCommand);
                return Ok();
            }
            catch (Exception exception)
            {
                _logger.Error("Error while resetting the password. {0}", exception.ToString());
                return InternalServerError();
            }
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("get-user")]
        [HttpGet]
        public IHttpActionResult GetUser(string email)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(email))
                {
                    _logger.Info("Get user process started. Email: {0}", email);
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