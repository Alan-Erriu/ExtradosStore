using ExtradosStore.Entities.DTOs.RoleDTOs;
using ExtradosStore.Entities.Models;

namespace ExtradosStore.Data.DAOs.Interfaces
{
    public interface IRoleDAO
    {
        Task<string> DataCompareNameRole(string nameRole);
        Task<CreateRoleDTO> DataCreateRole(CreateRoleDTO roleRequest);
        Task<List<Role>> DataGetRoles();

    }
}