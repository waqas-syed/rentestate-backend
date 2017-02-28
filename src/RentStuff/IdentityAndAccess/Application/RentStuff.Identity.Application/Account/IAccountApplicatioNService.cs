using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentStuff.Identity.Infrastructure.Persistence.Model;

namespace RentStuff.Identity.Application.Account
{
    public interface IAccountApplicationService
    {
        bool Register();
        bool SendActivationEmail(string email, string name);
        Task<CustomIdentityUser> FindUser(string userName, string password);
    }
}
