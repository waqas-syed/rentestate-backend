using Microsoft.AspNet.Identity;

namespace RentStuff.Identity.Infrastructure.Services.Hashers
{
    public class CustomPasswordHasher : IPasswordHasher
    {
        /// <summary>Hash a password</summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual string HashPassword(string password)
        {
            return CustomCrypto.HashPassword(password);
        }

        /// <summary>Verify that a password matches the hashedPassword</summary>
        /// <param name="hashedPassword"></param>
        /// <param name="providedPassword"></param>
        /// <returns></returns>
        public virtual PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            return CustomCrypto.VerifyHashedPassword(hashedPassword, providedPassword) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}
