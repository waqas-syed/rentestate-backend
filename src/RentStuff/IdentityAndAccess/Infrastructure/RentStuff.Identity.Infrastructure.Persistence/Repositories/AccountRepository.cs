using System;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using RentStuff.Identity.Infrastructure.Persistence.Model;
using RentStuff.Identity.Infrastructure.Services.Hashers;
using RentStuff.Identity.Infrastructure.Services.Validators;

namespace RentStuff.Identity.Infrastructure.Persistence.Repositories
{
    public class AccountRepository : IAccountRepository, IDisposable
    {
        private AuthContext _ctx;

        private UserManager<CustomIdentityUser> _userManager;

        public AccountRepository()
        {
            _ctx = new AuthContext();
            var provider = new DpapiDataProtectionProvider("RentStuff");
            
            _userManager = new UserManager<CustomIdentityUser>(new UserStore<CustomIdentityUser>(_ctx));
            _userManager.UserTokenProvider =
                new DataProtectorTokenProvider<CustomIdentityUser>(provider.Create("EmailConfirmation"));
            _userManager.UserValidator = new CustomUserValidator<CustomIdentityUser>(_userManager);
            _userManager.PasswordHasher = new CustomPasswordHasher();
        }

        public Tuple<IdentityResult, string> RegisterUser(string name, string email, string password)
        {
            // Assign email to the uername property, as we will use email in place of username
            CustomIdentityUser user = new CustomIdentityUser
            {
                UserName = email,
                Email = email,
                FullName = name
            };
            var result = _userManager.Create(user, password);
            if (result.Succeeded)
            {
                var emailConfirmationResult = _userManager.GenerateEmailConfirmationToken(user.Id);
                if (string.IsNullOrWhiteSpace(emailConfirmationResult))
                {
                    throw new ArgumentException("User saved but could not retreive Email Confirmation Token");
                }
                else
                {
                    return new Tuple<IdentityResult, string>(result, emailConfirmationResult);
                }
            }
            else
            {
                throw new InvalidOperationException("Not able to save user. Error: " + result.Errors.First());
            }
        }

        public IdentityResult UpdateUser(CustomIdentityUser customerIdentityUser)
        {
            return _userManager.Update(customerIdentityUser);
        }

        public bool ActivateUser(string userId, string emailConfirmationToken)
        {
            var identityResult = _userManager.ConfirmEmail(userId, emailConfirmationToken);
            if (identityResult != null && identityResult.Succeeded)
            {
                return true;
            }
            throw new InvalidOperationException("Could not activate user with ID: " + userId);
        }

        /// <summary>
        /// Is Email Confirmed for the given user
        /// </summary>
        /// <returns></returns>
        public bool IsEmailConfirmed(string userId)
        {
            return _userManager.IsEmailConfirmed(userId);
        }           

        public CustomIdentityUser GetUserByEmail(string email)
        {
            return _userManager.FindByEmail(email);
        }

        public CustomIdentityUser GetUserByPassword(string userName, string password)
        {
            return _userManager.Find(userName, password);
        }
        
        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();
        }
    }
}