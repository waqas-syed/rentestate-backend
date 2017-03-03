using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using RentStuff.Identity.Application.Account;
using RentStuff.Identity.Application.Account.Commands;

namespace RentStuff.Identity.Ports.Adapter.Rest.Controllers
{
    [RoutePrefix("v1/Account")]
    //[EnableCors(origins: "http://mywebclient.azurewebsites.net", headers: "*", methods: "*")]
    public class AccountController : ApiController
    {
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
                        string identityResult = _accountApplicationService.Register(createUserCommand);
                        if (!string.IsNullOrWhiteSpace(identityResult))
                        {
                            return Ok(true);
                        }
                        else
                        {
                            return BadRequest("Could not register user");
                        }
                    }
                    else
                    {
                        return BadRequest("User data not in expected format");
                    }
                }
                else
                {
                    return BadRequest("No user data received");
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
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
                        bool activationResult = _accountApplicationService.Activate(activateAccountCommand);
                        
                        if (activationResult)
                        {
                            return Ok();
                        }
                        else
                        {
                            return BadRequest("Could not activate account");
                        }
                    }
                    else
                    {
                        return BadRequest("User data not in expected format");
                    }
                }
                else
                {
                    return BadRequest("No user data received");
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
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