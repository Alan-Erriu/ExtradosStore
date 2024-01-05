using ExtradosStore.Common.CustomExceptions.GenericResponsesExceptions;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.DTOs.RoleDTOs;
using ExtradosStore.Entities.Models;
using ExtradosStore.Services.Interfaces;

namespace ExtradosStore.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IRoleDAO _roleDAO;
        public RoleService(IRoleDAO roleDAO)
        {
            _roleDAO = roleDAO;
        }
        // crear un nuevo rol
        public async Task<CreateRoleDTO> CreateRoleService(CreateRoleDTO roleRequest)
        {

            var roleAlreadyExists = await _roleDAO.DataCompareNameRole(roleRequest.role_name);

            if (roleAlreadyExists != null) throw new ConflictException("The name role is already in use");

            await _roleDAO.DataCreateRole(roleRequest);

            return new CreateRoleDTO { role_name = roleRequest.role_name, role_description = roleRequest.role_description };

        }
        public async Task<List<Role>> GetRolesService()
        {

            var rolesDB = await _roleDAO.DataGetRoles();
            return rolesDB;

        }
    }
}
