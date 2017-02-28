using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RentStuff.Identity.Infrastructure.Persistence.Model;
using RentStuff.Identity.Infrastructure.Services.Hashers;
using RentStuff.Identity.Infrastructure.Services.Validators;

namespace RentStuff.Identity.Infrastructure.Persistence.Repositories
{
    public class AuthRepository : IAuthRepository, IDisposable
    {
        private AuthContext _ctx;

        private UserManager<CustomIdentityUser> _userManager;

        public AuthRepository()
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<CustomIdentityUser>(new UserStore<CustomIdentityUser>(_ctx));
            _userManager.UserValidator = new CustomUserValidator<CustomIdentityUser>(_userManager);
            _userManager.PasswordHasher = new CustomPasswordHasher();
        }

        public async Task<IdentityResult> RegisterUser(string name, string email, string password)
        {
            string activationCode = Guid.NewGuid().ToString();
            // Assign email to the uername property, as we will use email in place of username
            CustomIdentityUser user = new CustomIdentityUser
            {
                UserName = email,
                Email = email,
                FullName = name,
                ActivationCode = activationCode,
                AccountActivated = false
            };
            var result = await _userManager.CreateAsync(user, password);

            return result;
        }

        public async Task<CustomIdentityUser> FindUser(string userName, string password)
        {
            CustomIdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();
        }
    }
}