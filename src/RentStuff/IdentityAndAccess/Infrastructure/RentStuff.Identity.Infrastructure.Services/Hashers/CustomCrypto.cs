using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace RentStuff.Identity.Infrastructure.Services.Hashers
{
    internal static class CustomCrypto
    {
        private const int IterationCount = 50000;
        private const int SubkeyLength = 32;
        private const int SaltSize = 16;

        public static string HashPassword(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");
            byte[] salt;
            byte[] bytes;
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, SaltSize, IterationCount))
            {
                salt = rfc2898DeriveBytes.Salt;
                bytes = rfc2898DeriveBytes.GetBytes(SubkeyLength);
            }
            byte[] inArray = new byte[49];
            Buffer.BlockCopy((Array)salt, 0, (Array)inArray, 1, SaltSize);
            Buffer.BlockCopy((Array)bytes, 0, (Array)inArray, 17, SubkeyLength);
            return Convert.ToBase64String(inArray);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            if (hashedPassword == null)
                return false;
            if (password == null)
                throw new ArgumentNullException("password");
            byte[] numArray = Convert.FromBase64String(hashedPassword);
            if (numArray.Length != 49 || (int)numArray[0] != 0)
                return false;
            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy((Array)numArray, 1, (Array)salt, 0, SaltSize);
            byte[] a = new byte[SubkeyLength];
            Buffer.BlockCopy((Array)numArray, 17, (Array)a, 0, SubkeyLength);
            byte[] bytes;
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, IterationCount))
                bytes = rfc2898DeriveBytes.GetBytes(SubkeyLength);
            return CustomCrypto.ByteArraysEqual(a, bytes);
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (object.ReferenceEquals((object)a, (object)b))
                return true;
            if (a == null || b == null || a.Length != b.Length)
                return false;
            bool flag = true;
            for (int index = 0; index < a.Length; ++index)
                flag &= (int)a[index] == (int)b[index];
            return flag;
        }
    }
}
