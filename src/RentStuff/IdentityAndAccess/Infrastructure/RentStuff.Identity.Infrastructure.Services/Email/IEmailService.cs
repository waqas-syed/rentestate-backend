using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Identity.Infrastructure.Services.Email
{
    public interface IEmailService
    {
        event Action EmailSent;
        void SendEmail(string to, string subject, string message);
    }
}
