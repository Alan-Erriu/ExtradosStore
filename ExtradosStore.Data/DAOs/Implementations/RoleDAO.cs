using Dapper;
using ExtradosStore.Configuration.DBConfiguration;
using ExtradosStore.Data.DAOs.Intefaces;
using ExtradosStore.Entities.DTOs.RoleDTOs;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace ExtradosStore.Data.DAOs.Implementations
{
    public class RoleDAO : IRoleDAO
    {
        private SQLServerConfig _SQLServerConfig;

        private string _sqlInsertRole = "INSERT INTO [role] (role_name, role_description ) VALUES (@Name, @Description)";

        private string _sqlSelectAllRoles = "SELECT role_name, role_description FROM [role]";

        private string _sqlSelectAllNamesRole = "SELECT role_name FROM [role] where role_name = @Name";

        public RoleDAO(IOptions<SQLServerConfig> bdConfig)
        {
            _SQLServerConfig = bdConfig.Value;
        }

        //crear un nuevo rol
        public async Task<CreateRoleDTO> DataCreateRole(CreateRoleDTO roleRequest)
        {
            try
            {
                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    var parameters = new { Name = roleRequest.role_name, Description = roleRequest.role_description };
                    var queryInsert = await connection.ExecuteAsync(_sqlInsertRole, parameters);

                    return new CreateRoleDTO
                    {
                        role_name = roleRequest.role_name,
                        role_description = roleRequest.role_description
                    };

                }
            }
            catch
            {

                throw;
            }
        }


        //devuelve todos los roles de la base de datos
        public async Task<List<CreateRoleDTO>> DataGetRoles()
        {
            try
            {
                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    return (await connection.QueryAsync<CreateRoleDTO>(_sqlSelectAllRoles)).ToList();

                }

            }
            catch
            {

                throw;
            }
        }

        //comprobar si el nombre del rol esta en uso, devuelve null si el nombre no esta en uso
        public async Task<string> DataCompareNameRole(string nameRole)
        {
            try
            {

                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {

                    var parameters = new { Name = nameRole };
                    var nameRoleFound = await connection.QueryFirstOrDefaultAsync<string>(_sqlSelectAllNamesRole, parameters);

                    return nameRoleFound;

                }
            }
            catch
            {

                throw;
            }
        }
    }
}
