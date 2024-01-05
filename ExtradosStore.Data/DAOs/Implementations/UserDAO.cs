using Dapper;
using ExtradosStore.Configuration.DBConfiguration;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.Models;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace ExtradosStore.Data.DAOs.Implementations
{
    public class UserDAO : IUserDAO
    {

        private SQLServerConfig _SQLServerConfig;


        public UserDAO(IOptions<SQLServerConfig> bdConfig)
        {
            _SQLServerConfig = bdConfig.Value;

        }
        #region querys
        private string _sqlUpdateStatusUser = "UPDATE [user] SET user_status = @StatusUser WHERE user_id = @UserId";

        private string _sqlSelectUserById = @"SELECT user_id,user_name,user_lastname, user_email, user_date_of_birth, user_roleid,user_status,user_created_at
                                              FROM [user] WHERE user_id = @UserId ";

        private string _sqlUpdateRoleFromUserToAdmin = "UPDATE [user] SET user_roleid = @roleId WHERE user_id = @UserId";

        private string _sqlSelecAllUser = "SELECT user_id,user_name,user_lastname, user_email, user_date_of_birth, user_roleid,user_status,user_created_at,user_phone_number FROM [user]";
        #endregion
        public async Task<List<User>> DataGetAllUser()
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var listUser = (await connection.QueryAsync<User>(_sqlSelecAllUser)).ToList();
                return listUser;
            }


        }




        // habilita o deshabilita. StatusUser en 1 para habilitar, statusUser en 0 para deshabilitar
        public async Task<int> DataUpdateStatusUser(int userId, int statusUser)
        {


            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new { UserId = userId, StatusUser = statusUser };
                var queryUpdateStatusUser = await connection.ExecuteAsync(_sqlUpdateStatusUser, parameters);
                return queryUpdateStatusUser;
            }


        }
        //actualiza el rol del usario, espera id de usuario y id del rol 
        public async Task<int> DataUpdateRolUser(int userId, int roleId)
        {


            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new { UserId = userId, RoleId = roleId };
                var queryUpdateStatusUser = await connection.ExecuteAsync(_sqlUpdateStatusUser, parameters);
                return queryUpdateStatusUser;
            }


        }


        public async Task<User> DataGetUserById(int userId)
        {


            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new { UserId = userId };
                var user = await connection.QueryFirstOrDefaultAsync<User>(_sqlSelectUserById, parameters);

                return user;
            }


        }
        public async Task<int> DataUpgradeRoleFromUserToAdmin(int userId, int roleId)
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new { UserId = userId, RoleId = roleId };
                var rowsAffected = await connection.ExecuteAsync(_sqlUpdateRoleFromUserToAdmin, parameters);

                return rowsAffected;
            }


        }
    }
}
