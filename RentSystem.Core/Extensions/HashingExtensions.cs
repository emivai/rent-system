using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using RentSystem.Core.DTOs;
using RentSystem.Core.Entities;
using System.Security.Cryptography;

namespace RentSystem.Core.Extensions
{
    public static class HashingExtensions
    {
        private static readonly int iterations = 100000;
        private static readonly int keySize = 256 / 8;
        private static HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
        public static HashValue Hash(this string str)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(keySize);


            var hash = Convert.ToBase64String(Rfc2898DeriveBytes.Pbkdf2(
                                password: str!,
                                salt: salt,
                                iterations: iterations,
                                hashAlgorithm: hashAlgorithm,
                                keySize));

            return new HashValue { Hash = hash, Salt = salt };
        }

        public static bool Verify(this User user, string password) 
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, user.Salt, iterations, hashAlgorithm, keySize);

            return hashToCompare.SequenceEqual(Convert.FromBase64String(user.Password));
        }

        public class HashValue 
        { 
            public string Hash { get; set; }
            public byte[] Salt { get; set; }
        }
    }
}
