using System;
using System.Security.Cryptography;
using System.Text;

namespace SosyalApp1.src.auth
{
    public static class PasswordHasher
    {
        // Salt size in bytes
        private const int SaltSize = 16;
        
        // Hash size in bytes
        private const int HashSize = 20;
        
        // Number of iterations for PBKDF2
        private const int Iterations = 10000;

        /// <summary>
        /// Hashes a password using PBKDF2 algorithm with salt
        /// </summary>
        /// <param name="password">Plain text password to hash</param>
        /// <returns>Formatted hash string containing salt and hash</returns>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be null or empty", nameof(password));

            // Generate a random salt
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hash the password with the salt
            byte[] hash = PBKDF2(password, salt, Iterations, HashSize);

            // Combine salt and hash into a single string
            byte[] saltAndHash = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, saltAndHash, 0, SaltSize);
            Array.Copy(hash, 0, saltAndHash, SaltSize, HashSize);

            // Return as base64 string
            return Convert.ToBase64String(saltAndHash);
        }

        /// <summary>
        /// Verifies a password against its hash
        /// </summary>
        /// <param name="password">Plain text password to verify</param>
        /// <param name="hashedPassword">Stored hash to compare against</param>
        /// <returns>True if password matches hash, false otherwise</returns>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be null or empty", nameof(password));
                
            if (string.IsNullOrEmpty(hashedPassword))
                return false;

            try
            {
                // Decode the stored hash
                byte[] saltAndHash = Convert.FromBase64String(hashedPassword);
                
                // Extract salt and hash
                byte[] salt = new byte[SaltSize];
                byte[] hash = new byte[HashSize];
                Array.Copy(saltAndHash, 0, salt, 0, SaltSize);
                Array.Copy(saltAndHash, SaltSize, hash, 0, HashSize);

                // Hash the provided password with the extracted salt
                byte[] computedHash = PBKDF2(password, salt, Iterations, HashSize);

                // Compare hashes
                return ByteArraysEqual(hash, computedHash);
            }
            catch
            {
                // If there's any error during verification, return false
                return false;
            }
        }

        /// <summary>
        /// PBKDF2 implementation
        /// </summary>
        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputLength)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(outputLength);
            }
        }

        /// <summary>
        /// Compares two byte arrays for equality in constant time to prevent timing attacks
        /// </summary>
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;

            var result = 0;
            for (int i = 0; i < a.Length; i++)
            {
                result |= a[i] ^ b[i];
            }
            
            return result == 0;
        }
    }
}