using ExtradosStore.Services.Interfaces;
using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace ExtradosStore.Services.Implementations
{
    public class Argon2HasherService : IHasherService
    {
        public string HashPasswordUser(string password)
        {
            // Configura los parámetros del algoritmo Argon2id
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = GenerateSalt(),
                DegreeOfParallelism = 8,
                Iterations = 4,
                MemorySize = 65536
            };


            byte[] hashBytes = argon2.GetBytes(32);


            string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashedPassword;
        }

        // Genera un nuevo valor de sal aleatorio
        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }


        // Realiza la verificación del hash
        public bool VerifyPassword(string hashedPassword, string inputPassword)
        {

            byte[] storedHash = Enumerable.Range(0, hashedPassword.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hashedPassword.Substring(x, 2), 16))
                             .ToArray();


            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(inputPassword))
            {
                Salt = storedHash.Take(16).ToArray(),
                DegreeOfParallelism = 8,
                Iterations = 4,
                MemorySize = 65536
            };
            byte[] hashBytes = argon2.GetBytes(32);

            // compara el hash generado con el almacenado
            return storedHash.Skip(16).SequenceEqual(hashBytes);
        }
    }
}
