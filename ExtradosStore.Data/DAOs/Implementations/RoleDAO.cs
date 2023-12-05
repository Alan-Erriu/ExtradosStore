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
                    var parameters = new { Name = roleRequest.name_role, Description = roleRequest.description_role };
                    var queryInsert = await connection.ExecuteAsync(_sqlInsertRole, parameters);

                    return new CreateRoleDTO
                    {
                        name_role = roleRequest.name_role,
                        description_role = roleRequest.description_role
                    };

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
