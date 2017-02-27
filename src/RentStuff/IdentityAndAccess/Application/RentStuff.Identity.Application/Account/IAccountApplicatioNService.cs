using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Identity.Application.Account
{
    public interface IAccountApplicationService
    {
        bool SendActivationEmail(string email, string name);
    }
}
