using Dapper;
using ExtradosStore.Common.CustomRequest.AuthRequest;
using ExtradosStore.Configuration.DBConfiguration;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.DTOs.AuthDTOs;
using ExtradosStore.Entities.Models;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace ExtradosStore.Data.DAOs.Implementations
{
    public class AuthDAO : IAuthDAO
    {
        private SQLServerConfig _SQLServerConfig;

        #region querys
        private string _sqlInsertUser = @"INSERT INTO [user] (user_name, user_lastname, user_email,user_password_hash, user_created_at, user_date_of_birth, user_roleid, user_phone_number)
                                         VALUES (@Name, @LastName, @Email,@Password, @CreatedAt, @DateOfbirth, @Roleid, @PhoneNumber)";


        private string _sqlSelectEmailUser = "SELECT user_email FROM[user] where user_email = @Email";

        private string _sqlSelectPhoneNumberUser = "SELECT user_phone_number FROM [user] WHERE user_phone_number = @PhoneNumber";

        private string _sqlSelectAuthUserLogin = @"SELECT user_id,user_name, user_email,user_password_hash, user_date_of_birth, user_roleid,user_status 
                                                   FROM [user] WHERE user_email = @Mail";
        #endregion
        public AuthDAO(IOptions<SQLServerConfig> bdConfig)
        {
            _SQLServerConfig = bdConfig.Value;
        }

        //iniciar sesion
        public async Task<AuthLoginUserDTO> DataSignIn(LoginUserRequest loginRequest)
        {


            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new { Mail = loginRequest.user_email };
                var user = await connection.QueryFirstOrDefaultAsync<AuthLoginUserDTO>(_sqlSelectAuthUserLogin, parameters);

                return user;
            }

        }

        public async Task<int> DataSignUp(User newUser)
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {

                var parameters = new
                {
                    Name = newUser.user_name.ToLower(),
                    LastName = newUser.user_lastname.ToLower(),
                    Email = newUser.user_email,
                    Password = newUser.user_password_hash,
                    DateOfbirth = newUser.user_date_of_birth,
                    CreatedAt = newUser.user_created_at,
                    Roleid = newUser.user_roleid,
                    PhoneNumber = newUser.user_phone_number
                };
                var rowsAffected = await connection.ExecuteAsync(_sqlInsertUser, parameters);

                return rowsAffected;
            }

        }
        public async Task<string> DataGetEmailUser(string email)
        {



            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new { Email = email };
                var emailFound = await connection.QueryFirstOrDefaultAsync<string>(_sqlSelectEmailUser, parameters);
                return emailFound;

            }


        }
        public async Task<string> DataGetPhoneNumberUser(string PohoneNumberRequest)
        {



            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new { PhoneNumber = PohoneNumberRequest };

                return await connection.QueryFirstOrDefaultAsync<string>(_sqlSelectPhoneNumberUser, parameters);


            }

        }
    }
}
