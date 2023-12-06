using ExtradosStore.Entities.Models;

namespace ExtradosStore.Data.DAOs.Interfaces
{
    public interface IAuthDAO
    {
        Task<string> DataGetEmailUser(string email);
        Task<int> DataSignUp(User newUser);
    }
}