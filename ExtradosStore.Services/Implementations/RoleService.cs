using ExtradosStore.Data.DAOs.Intefaces;
using ExtradosStore.Entities.DTOs.RoleDTOs;
using ExtradosStore.Services.Interfaces;
using System.Data;

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
            try
            {

                var roleAlreadyExists = await _roleDAO.DataCompareNameRole(roleRequest.name_role);

                if (roleAlreadyExists != null) throw new DuplicateNameException("The name role is already in use");

                await _roleDAO.DataCreateRole(roleRequest);

                return new CreateRoleDTO { name_role = roleRequest.name_role, description_role = roleRequest.description_role };
            }
            catch
            {

                throw;
            }


        }
    }
}
