using System;
using System.Security.Cryptography;
using api.Application.Services.Interfaces;

namespace api.Infrastructure.ExternalServices
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltLength = 32; 
        private const int KeyLength = 64; 
        private const int Pbkdf2Iterations = 100000; 
        private const char Delimiter = '|'; 
        private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256; 

        // Método que genera un hash de la contraseña con un salt aleatorio.
        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("Passwords can't be null or empty..", nameof(password));
            
            var saltBytes = GenerateSalt(SaltLength);
            var saltString = Convert.ToBase64String(saltBytes);
            var hashBytes = DeriveHash(password, saltBytes);
            var hashString = Convert.ToBase64String(hashBytes);
            
            return $"{hashString}{Delimiter}{saltString}";
        }

        // Método que verifica si la contraseña introducida coincide con la almacenada.
        public bool CheckPassword(string storedPassword, string inputPassword)
        {
            if (string.IsNullOrEmpty(storedPassword) || string.IsNullOrEmpty(inputPassword))
                throw new ArgumentException("Passwords can't be null or empty.");

            var parts = storedPassword.Split(Delimiter);
            if (parts.Length != 2) return false; 

            var storedHash = Convert.FromBase64String(parts[0]);
            var storedSalt = Convert.FromBase64String(parts[1]);
            var inputHash = DeriveHash(inputPassword, storedSalt);
            
            return SlowEquals(storedHash, inputHash);
        }

        // Método para generar un salt aleatorio de un tamaño especificado.
        private byte[] GenerateSalt(int size)
        {
            var salt = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        // Método que deriva un hash de la contraseña usando PBKDF2 y un salt.
        private byte[] DeriveHash(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Pbkdf2Iterations, HashAlgorithm))
            {
                return pbkdf2.GetBytes(KeyLength);
            }
        }

        // Método para comparar dos arrays de bytes de forma segura, evitando ataques de temporización.
        private bool SlowEquals(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) return false;

            int difference = 0;
            for (int i = 0; i < a.Length; i++)
            {
                difference |= a[i] ^ b[i];
            }

            return difference == 0;
        }
    }
}
