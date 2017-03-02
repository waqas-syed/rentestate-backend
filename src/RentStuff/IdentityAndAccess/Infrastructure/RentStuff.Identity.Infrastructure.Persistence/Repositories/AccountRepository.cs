using System;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RentStuff.Identity.Infrastructure.Persistence.Model;
using RentStuff.Identity.Infrastructure.Services.Email;
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
            _userManager = new UserManager<CustomIdentityUser>(new UserStore<CustomIdentityUser>(_ctx));
            _userManager.UserValidator = new CustomUserValidator<CustomIdentityUser>(_userManager);
            _userManager.PasswordHasher = new CustomPasswordHasher();
        }

        public IdentityResult RegisterUser(string name, string email, string password)
        {
            // Assign email to the uername property, as we will use email in place of username
            CustomIdentityUser user = new CustomIdentityUser
            {
                UserName = email,
                Email = email,
                FullName = name
            };
            var result = _userManager.Create(user, password);
            
            return result;
        }
        
        public CustomIdentityUser GetUserByEmail(string email)
        {
            return _userManager.FindByEmail(email);
        }

        public CustomIdentityUser GetUserByPassword(string userName, string password)
        {
            return _userManager.Find(userName, password);
        }

        public IdentityResult UpdateUser(CustomIdentityUser customerIdentityUser)
        {
            return _userManager.Update(customerIdentityUser);
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();
        }
    }
}