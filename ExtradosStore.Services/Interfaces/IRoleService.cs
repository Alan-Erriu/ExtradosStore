using ExtradosStore.Entities.DTOs.RoleDTOs;

namespace ExtradosStore.Services.Interfaces
{
    public interface IRoleService
    {
        Task<CreateRoleDTO> CreateRoleService(CreateRoleDTO roleRequest);
    }
}