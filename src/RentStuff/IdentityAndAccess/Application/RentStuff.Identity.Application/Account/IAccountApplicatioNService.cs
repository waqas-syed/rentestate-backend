using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using RentStuff.Identity.Application.Account.Commands;
using RentStuff.Identity.Infrastructure.Persistence.Model;

namespace RentStuff.Identity.Application.Account
{
    public interface IAccountApplicationService
    {
        Task<bool> Register(CreateUserCommand createUserCommand);
        Task<CustomIdentityUser> FindUser(string userName, string password);
        Task<bool> Activate(ActivateAccountCommand activateAccountCommand);
        void Dispose();
    }
}
