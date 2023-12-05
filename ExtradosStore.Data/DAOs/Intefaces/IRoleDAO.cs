using ExtradosStore.Entities.DTOs.RoleDTOs;

namespace ExtradosStore.Data.DAOs.Intefaces
{
    public interface IRoleDAO
    {
        Task<string> DataCompareNameRole(string nameRole);
        Task<CreateRoleDTO> DataCreateRole(CreateRoleDTO roleRequest);
        Task<List<CreateRoleDTO>> DataGetRoles();

    }
}