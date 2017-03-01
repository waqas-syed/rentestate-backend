using System;
using System.Linq;
using System.Threading.Tasks;
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
            var provider = new DpapiDataProtectionProvider("Sample");

            
            _userManager = new UserManager<CustomIdentityUser>(new UserStore<CustomIdentityUser>(_ctx));
            _userManager.UserTokenProvider =
                new DataProtectorTokenProvider<CustomIdentityUser>(provider.Create("EmailConfirmation"));
            _userManager.UserValidator = new CustomUserValidator<CustomIdentityUser>(_userManager);
            _userManager.PasswordHasher = new CustomPasswordHasher();
        }

        public async Task<Tuple<IdentityResult, string>> SaveUser(string name, string email, string password)
        {
            // Assign email to the uername property, as we will use email in place of username
            CustomIdentityUser user = new CustomIdentityUser
            {
                UserName = email,
                Email = email,
                FullName = name
            };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var emailConfirmationResult = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
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

        public async Task<IdentityResult> UpdateUser(CustomIdentityUser customerIdentityUser)
        {
            return await _userManager.UpdateAsync(customerIdentityUser);
        }

        public async Task<CustomIdentityUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<CustomIdentityUser> FindUser(string userName, string password)
        {
            return await _userManager.FindAsync(userName, password);
        }

        public UserManager<CustomIdentityUser> UserManager { get { return _userManager; } }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();
        }
    }
}