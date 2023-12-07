using ExtradosStore.Entities.Models;

namespace ExtradosStore.Data.DAOs.Interfaces
{
    public interface IUserDAO
    {
        Task<int> DataUpdateRolUser(int userId, int roleId);
        Task<int> DataUpdateStatusUser(int userId, int statusUser);
        Task<int> DataUpgradeRoleFromUserToAdmin(int userId, int roleId);
        Task<User> DataGetUserById(int userId);
    }
}