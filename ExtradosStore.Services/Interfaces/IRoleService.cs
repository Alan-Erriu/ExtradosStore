using ExtradosStore.Entities.DTOs.RoleDTOs;
using ExtradosStore.Entities.Models;

namespace ExtradosStore.Services.Interfaces
{
    public interface IRoleService
    {
        Task<CreateRoleDTO> CreateRoleService(CreateRoleDTO roleRequest);

        Task<List<Role>> GetRolesService();

    }
}