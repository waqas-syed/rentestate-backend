using Microsoft.AspNet.Identity.EntityFramework;

namespace RentStuff.Identity.Ports.Adapter.Rest.Models
{
    /// <summary>
    /// Implements the IdentityUser class, as we need to have Username and password
    /// </summary>
   public class CustomIdentityUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
