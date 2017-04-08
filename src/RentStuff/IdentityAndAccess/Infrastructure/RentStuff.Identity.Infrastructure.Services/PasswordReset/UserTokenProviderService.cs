using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using RentStuff.Identity.Infrastructure.Services.Identity;

namespace RentStuff.Identity.Infrastructure.Services.PasswordReset
{
    /// <summary>
    /// Generates and validates password reset token
    /// </summary>
    public class UserTokenProviderService : IUserTokenProvider<CustomIdentityUser, string>
    {
        /// <summary>
        ///     Generate a token for a user with a specific purpose
        /// </summary>
        /// <param name="purpose"></param>
        /// <param name="manager"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GenerateAsync(string purpose, UserManager<CustomIdentityUser, string> manager, CustomIdentityUser user)
        {
            Task<string> task = Task<string>.Factory.StartNew(() =>
            {
                string mySalt = BCrypt.Net.BCrypt.GenerateSalt();
                //mySalt == "$2a$10$rBV2JDeWW3.vKyeQcM8fFO"
                string emailHash = BCrypt.Net.BCrypt.HashPassword(user.Email, mySalt);
                string userIdHash = BCrypt.Net.BCrypt.HashPassword(user.Id, mySalt);
                DateTime? tokenExpirationTime = DateTime.Now.AddDays(1);
                string finalHash = userIdHash + "|" +  emailHash;
                return finalHash;
            });
            return task;
        }

        /// <summary>
        ///     Validate a token for a user with a specific purpose
        /// </summary>
        /// <param name="purpose"></param>
        /// <param name="token"></param>
        /// <param name="manager"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> ValidateAsync(string purpose, string token, UserManager<CustomIdentityUser, string> manager, CustomIdentityUser user)
        {
            Task<bool> task = Task<bool>.Factory.StartNew(() =>
            {
                string[] separatedHashes = token.Split('|');
                if (separatedHashes.Length.Equals(2) && BCrypt.Net.BCrypt.Verify(user.Id, separatedHashes[0])
                    && BCrypt.Net.BCrypt.Verify(user.Email, separatedHashes[1])
                )
                {
                    return true;
                }
                throw new InvalidOperationException("Invalid token.");
            });
            return task;
        }

        /// <summary>
        ///     Notifies the user that a token has been generated, for example an email or sms could be sent, or
        ///     this can be a no-op
        /// </summary>
        /// <param name="token"></param>
        /// <param name="manager"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task NotifyAsync(string token, UserManager<CustomIdentityUser, string> manager, CustomIdentityUser user)
        {
            return null;
        }

        /// <summary>
        ///     Returns true if provider can be used for this user, i.e. could require a user to have an email
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> IsValidProviderForUserAsync(UserManager<CustomIdentityUser, string> manager, CustomIdentityUser user)
        {
            return null;
        }
    }
}
