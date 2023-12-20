using ExtradosStore.Entities.DTOs.UserDTOs;

namespace ExtradosStore.Services.Interfaces
{
    public interface IUserService
    {
        Task<int> DisableUserService(int idUser);
        Task<int> EnableUserService(int idUser);
        Task<int> UpgradeRoleFromUserToAdminService(int userId);
        Task<List<UserWithRolesNameDTO>> GetUsersService();
    }
}