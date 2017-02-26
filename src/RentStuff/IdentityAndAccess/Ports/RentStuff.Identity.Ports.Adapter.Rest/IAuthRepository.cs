using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RentStuff.Identity.Ports.Adapter.Rest.DTOs;

namespace RentStuff.Identity.Ports.Adapter.Rest
{
    public interface IAuthRepository : IDisposable
    {
        Task<IdentityResult> RegisterUser(UserModel userModel);

        Task<IdentityUser> FindUser(string userName, string password);
    }
}
