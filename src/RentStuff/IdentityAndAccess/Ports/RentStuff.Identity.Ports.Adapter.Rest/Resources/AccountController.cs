using System;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using NLog;
using RentStuff.Identity.Application.Account;
using RentStuff.Identity.Application.Account.Commands;
using RentStuff.Identity.Application.Account.Representations;

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
                    if (!string.IsNullOrWhiteSpace(forgotPasswordCommand.Email))
                    {
                        _logger.Info("Forgot-password process started. Email: {0}", forgotPasswordCommand.Email);
                        _accountApplicationService.ForgotPassword(forgotPasswordCommand);
                        return Ok();
                    }
                    else
                    {
                        _logger.Error("No email provided for processing Forgot-Password request.");
                        return BadRequest("No email provided");
                    }
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
                _logger.Info("Reset-password process started.");
                _accountApplicationService.ResetPassword(resetPasswordCommand);
                return Ok();
            }
            catch (Exception exception)
            {
                _logger.Error("Error while resetting the password. {0}", exception.ToString());
                return BadRequest(exception.ToString());
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
                    _logger.Error("No Email provided");
                    return BadRequest("No email provided");
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

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}