using Dapper;
using ExtradosStore.Configuration.DBConfiguration;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.Models;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace ExtradosStore.Data.DAOs.Implementations
{
    public class AuthDAO : IAuthDAO
    {
        private SQLServerConfig _SQLServerConfig;

        private string _sqlInsertUser = @"INSERT INTO [user] (user_name, user_lastname, user_email,user_password_hash, user_created_at, user_date_of_birth, user_roleid)
                                         VALUES (@Name, @LastName, @Email,@Password, @CreatedAt, @DateOfbirth, @Roleid)";


        private string _sqlSelectEmailUser = "SELECT user_email FROM[user] where user_email = @Email";

        public AuthDAO(IOptions<SQLServerConfig> bdConfig)
        {
            _SQLServerConfig = bdConfig.Value;
        }


        // registrarse como usuario, el rol obtenido es "user"
        public async Task<int> DataSignUp(User newUser)
        {
            try
            {
                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {

                    var parameters = new
                    {
                        Name = newUser.user_name,
                        LastName = newUser.user_lastname,
                        Email = newUser.user_email,
                        Password = newUser.user_password_hash,
                        DateOfbirth = newUser.date_of_birth,
                        CreatedAt = newUser.user_created_at,
                        Roleid = newUser.user_roleid
                    };
                    var rowsAffected = await connection.ExecuteAsync(_sqlInsertUser, parameters);

                    return rowsAffected;
                }
            }
            catch
            {
                throw;

            }


        }
        public async Task<string> DataGetEmailUser(string email)
        {
            try
            {

                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    var parameters = new { Email = email };
                    var emailFound = await connection.QueryFirstOrDefaultAsync<string>(_sqlSelectEmailUser, parameters);
                    return emailFound;

                }

            }
            catch
            {

                throw;
            }

        }
    }
}
