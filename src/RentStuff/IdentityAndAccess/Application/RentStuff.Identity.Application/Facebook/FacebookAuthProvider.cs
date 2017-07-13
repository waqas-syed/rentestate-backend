using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Facebook;

namespace RentStuff.Identity.Application.Facebook
{
    public class FacebookAuthProvider : FacebookAuthenticationProvider
    {
        public override Task Authenticated(FacebookAuthenticatedContext context)
        {
            context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));
            //context.Identity.AddClaim(new Claim("emailaddress", context.Email));
            return Task.FromResult<object>(null);
        }
    }
}