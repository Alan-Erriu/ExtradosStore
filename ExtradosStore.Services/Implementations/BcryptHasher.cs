using ExtradosStore.Services.Interfaces;

namespace ExtradosStore.Services.Implementations
{
    public class BcryptHasher : IHasherService
    {

        public string HashPasswordUser(string password)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password);

            return hash;

        }
        public bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
        }
    }
}
