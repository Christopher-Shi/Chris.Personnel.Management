using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Chris.Personnel.Management.Common
{
    public static class PasswordHasher
    {
        public static string Hash(byte[] salt, string password)
        {
            var encodedPassword = Encoding.Unicode.GetBytes(password);
            var saltedPassword = new byte[salt.Length + encodedPassword.Length];
            Array.Copy(salt, 0, saltedPassword, 0, salt.Length);
            Array.Copy(encodedPassword, 0, saltedPassword, salt.Length, encodedPassword.Length);
            using (var alg = SHA256.Create())
            {
                var computeHash = alg.ComputeHash(saltedPassword);
                var result = computeHash.Aggregate("", (current, t) => current + t.ToString("X2"));
                return result;
            }
        }

        public static HashedPassword HashedPassword(string password)
        {
            var salt = Guid.NewGuid();
            return new HashedPassword()
            {
                Salt = salt.ToString(),
                Hash = Hash(salt.ToByteArray(), password)
            };
        }
    }

    public class HashedPassword
    {
        public string Salt { get; set; }
        public string Hash { get; set; }
    }
}