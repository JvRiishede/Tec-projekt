using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Data.Classes
{
    public static class PasswordHash
    {
        public const int SaltSize = 16;
        public const int HashSize = 16;
        public const int HashIterations = 1000;

        public const int IterationIndex = 0;
        public const int SaltIndex = 2;
        public const int HashIndex = 1;

        public static string Hash(string password)
        {
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            var salt = new byte[SaltSize];
            crypto.GetBytes(salt);

            var hash = new Rfc2898DeriveBytes(password, salt, HashIterations).GetBytes(HashSize);



            return string.Format("{0}:{1}:{2}", HashIterations, Convert.ToBase64String(hash), Convert.ToBase64String(salt));
        }

        public static bool Validate(string password, string passwordHash)
        {
            try
            {
                var split = passwordHash.Split(':');

                if (split.Length != 3)
                {
                    return false;
                }
                var iterations = int.Parse(split[IterationIndex]);
                var hash = Convert.FromBase64String(split[HashIndex]);
                var salt = Convert.FromBase64String(split[SaltIndex]);

                var hashTest = new Rfc2898DeriveBytes(password, salt, iterations).GetBytes(hash.Length);
                return Equals(hashTest, hash);
            }
            catch
            {
                return false;
            }
        }

        private static bool Equals(byte[] a, byte[] b)
        {
            //Længden tjekkes om den forskellig ved at udføre en bitwise xor
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                //Der udføres en bitwise or på diff og resultatet af vores expression.
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }
    }
}