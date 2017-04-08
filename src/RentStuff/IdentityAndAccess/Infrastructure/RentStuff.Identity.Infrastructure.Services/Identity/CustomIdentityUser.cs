using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace RentStuff.Identity.Infrastructure.Services.Identity
{
    /// <summary>
    /// Implements the IdentityUser class, as we need to have Username and password
    /// </summary>
   public class CustomIdentityUser : IdentityUser
    {
        public string FullName { get; set; }

        public bool IsPasswordResetRequested { get; set; }

        public DateTime? PasswordResetExpiryDate { get; set; }
    }
}
