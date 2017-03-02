using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using RentStuff.Identity.Application.Account.Commands;
using RentStuff.Identity.Application.Account.Representations;
using RentStuff.Identity.Infrastructure.Persistence.Model;
using RentStuff.Identity.Infrastructure.Persistence.Repositories;

namespace RentStuff.Identity.Application.Account
{
    public interface IAccountApplicationService
    {
        bool Register(CreateUserCommand createUserCommand);
        bool Activate(ActivateAccountCommand activateAccountCommand);
        UserRepresentation GetUserByEmail(string email);
        void Dispose();
    }
}
