using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RentStuff.IdentityAndAccess.Adapter.Rest.Models;

namespace RentStuff.IdentityAndAccess.Adapter.Rest
{
    public interface IAuthRepository : IDisposable
    {
        Task<IdentityResult> RegisterUser(UserModel userModel);

        Task<IdentityUser> FindUser(string userName, string password);
    }
}
