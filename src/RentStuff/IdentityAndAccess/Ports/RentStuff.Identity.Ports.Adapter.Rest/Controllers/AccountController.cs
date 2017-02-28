using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using RentStuff.Identity.Domain.Model.UserAggregate;
using RentStuff.Identity.Infrastructure.Persistence.Repositories;

namespace RentStuff.Identity.Ports.Adapter.Rest.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private IAuthRepository _repo = null;
        
        public AccountController(IAuthRepository authRepository)
        {
            _repo = authRepository;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register([FromBody] Object userObject)
        {
            try
            {
                if (userObject != null)
                {
                    var jsonString = userObject.ToString();
                    var userModel = JsonConvert.DeserializeObject<UserModel>(jsonString);

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
                    if (!userModel.Password.Equals(userModel.ConfirmPassword))
                    {
                        return BadRequest("Password and confirm password are not the same");
                    }
                    IdentityResult result = await _repo.RegisterUser(userModel.FullName, userModel.Email, userModel.Password);

                    IHttpActionResult errorResult = GetErrorResult(result);

                    if (errorResult != null)
                    {
                        return errorResult;
                    }

                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception exception)
            {
                return InternalServerError();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
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