using Dapper;
using ExtradosStore.Configuration.DBConfiguration;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.DTOs.RoleDTOs;
using ExtradosStore.Entities.Models;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace ExtradosStore.Data.DAOs.Implementations
{
    public class RoleDAO : IRoleDAO
    {
        private SQLServerConfig _SQLServerConfig;

        #region querys

        private string _sqlInsertRole = "INSERT INTO [role] (role_name, role_description ) VALUES (@Name, @Description)";

        private string _sqlSelectAllRoles = "SELECT role_id,role_name, role_description FROM [role]";

        private string _sqlSelectAllNamesRole = "SELECT role_name FROM [role] where role_name = @Name";

        private string _sqlSelectNameRoleById = "SELECT role_name FROM [role] where role_id = @IdRole";

        private string _sqlSelecIdRolByName = "SELECT role_id FROM[role] where role_name = @NameRole";
        #endregion
        public RoleDAO(IOptions<SQLServerConfig> bdConfig)
        {
            _SQLServerConfig = bdConfig.Value;
        }

        //crear un nuevo rol
        public async Task<CreateRoleDTO> DataCreateRole(CreateRoleDTO roleRequest)
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


        //devuelve todos los roles de la base de datos
        public async Task<List<Role>> DataGetRoles()
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                return (await connection.QueryAsync<Role>(_sqlSelectAllRoles)).ToList();

            }


        }

        //comprobar si el nombre del rol esta en uso, devuelve null si el nombre no esta en uso
        public async Task<string> DataCompareNameRole(string nameRole)
        {


            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {

                var parameters = new { Name = nameRole };
                var nameRoleFound = await connection.QueryFirstOrDefaultAsync<string>(_sqlSelectAllNamesRole, parameters);

                return nameRoleFound;

            }
        }
        public async Task<string> DataGetNameRoleById(int idRole)
        {


            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {

                var parameters = new { IdRole = idRole };
                var nameRoleFound = await connection.QueryFirstOrDefaultAsync<string>(_sqlSelectNameRoleById, parameters);

                return nameRoleFound;

            }

        }
        public async Task<int> DataGetIdRoleByName(string name)
        {


            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {

                var parameters = new { NameRole = name };
                var nameRoleFound = await connection.QueryFirstOrDefaultAsync<int>(_sqlSelecIdRolByName, parameters);

                return nameRoleFound;

            }

        }
    }
}
