using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Identity.Infrastructure.Services.Email
{
    public interface IEmailTokenGenerationService
    {
        string GenerateEmailToken(string email, string userId);
        bool VerifyToken(string email, string userId, string activationToken);
    }
}
