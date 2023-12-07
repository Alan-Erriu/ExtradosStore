﻿using Dapper;
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

        private string _sqlUpdateStatusUser = "UPDATE [user] SET user_status = @StatusUser WHERE user_id = @UserId";

        private string _sqlUpdateRolUser = "UPDATE [user] SET user_roleid = @RoleId WHERE user_id = @UserId";

        private string _sqlSelectUserById = @"SELECT user_id,user_name,user_lastname, user_email, user_date_of_birth, user_roleid,user_status,user_created_at
                                              FROM [user] WHERE user_id = @Id ";

        // habilita o deshabilita. StatusUser en 1 para habilitar, statusUser en 0 para deshabilitar
        public async Task<int> DataUpdateStatusUser(int userId, int statusUser)
        {
            try
            {

                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    var parameters = new { UserId = userId, StatusUser = statusUser };
                    var queryUpdateStatusUser = await connection.ExecuteAsync(_sqlUpdateStatusUser, parameters);
                    return queryUpdateStatusUser;
                }
            }
            catch
            {

                throw;
            }

        }
        //actualiza el rol del usario, espera id de usuario y id del rol 
        public async Task<int> DataUpdateRolUser(int userId, int roleId)
        {
            try
            {

                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    var parameters = new { UserId = userId, RoleId = roleId };
                    var queryUpdateStatusUser = await connection.ExecuteAsync(_sqlUpdateStatusUser, parameters);
                    return queryUpdateStatusUser;
                }
            }
            catch
            {

                throw;
            }

        }


        public async Task<User> DataGetUserById(int userId)
        {
            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new { UserId = userId };
                return await connection.QueryFirstOrDefaultAsync<User>(_sqlSelectUserById, parameters);

            }

        }
    }
}
