using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentStuff.Identity.Infrastructure.Services.Email
{
    /// <summary>
    /// Mock implementation of the CustomEmailService
    /// </summary>
    public class MockCustomEmailService : ICustomEmailService
    {
        public event Action EmailSent;
        public void SendEmail(string to, string subject, string message)
        {
            // Do nothing, just return
        }
    }
}
