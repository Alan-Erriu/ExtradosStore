namespace ExtradosStore.Services.Interfaces
{
    public interface IHasherService
    {
        string HashPasswordUser(string password);
        bool VerifyPassword(string hashedPassword, string inputPassword);
    }
}