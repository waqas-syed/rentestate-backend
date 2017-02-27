using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RentStuff.Identity.Ports.Adapter.Rest.DTOs;
using RentStuff.Identity.Ports.Adapter.Rest.Hashers;
using RentStuff.Identity.Ports.Adapter.Rest.Models;
using RentStuff.Identity.Ports.Adapter.Rest.Validators;

namespace RentStuff.Identity.Ports.Adapter.Rest
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

        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            // Assign email to the uername property, as we will use email in place of username
            CustomIdentityUser user = new CustomIdentityUser
            {
                UserName = userModel.Email,
                Email = userModel.Email,
                FullName = userModel.FullName
            };
            var result = await _userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();
        }
    }
}