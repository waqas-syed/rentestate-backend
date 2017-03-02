using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace RentStuff.Identity.Infrastructure.Services.Email
{
    public class EmailTokenGenerationService : IEmailTokenGenerationService
    {
        public string GenerateEmailToken(string email, string userId)
        {
            string mySalt = BCrypt.Net.BCrypt.GenerateSalt();
            //mySalt == "$2a$10$rBV2JDeWW3.vKyeQcM8fFO"
            string emailHash = BCrypt.Net.BCrypt.HashPassword(email, mySalt);
            string userIdHash = BCrypt.Net.BCrypt.HashPassword(userId, mySalt);
            string finalHash = emailHash + "|" + userIdHash;
            return finalHash;
        }

        public bool VerifyToken(string email, string userId, string activationToken)
        {
            string[] separatedHashes = activationToken.Split('|');
            if (separatedHashes.Length.Equals(2) && BCrypt.Net.BCrypt.Verify(email, separatedHashes[0]) && BCrypt.Net.BCrypt.Verify(userId, separatedHashes[1]))
            {
                return true;
            }
            throw new InvalidOperationException("Invalid token.");
        }
    }
}
