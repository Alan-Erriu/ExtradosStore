using Dapper;
using ExtradosStore.Configuration.DBConfiguration;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.DTOs.JWTDTOs;
using ExtradosStore.Entities.Models;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace ExtradosStore.Data.DAOs.Implementations
{
    public class JWTDAO : IJWTDAO
    {
        private SQLServerConfig _SQLServerConfig;

        private string _sqlInsertRefreshToken = @"INSERT INTO [token] (token_userid,token_accesstoken,token_refreshToken,
          token_expiration_date_refreshtoken) VALUES (@IdUser,@Token,@RefreshToken,@ExpirationDate)";

        private string _SqlSelectRefreshToken = @"SELECT token_userid,token_accesstoken,token_refreshToken, token_expiration_date_refreshtoken 
                                                  FROM [token] WHERE token_userid = @Id";

        private string _sqlDeleteRefreshTokenExpired = "delete from [token] where token_userid = @Id";

        public JWTDAO(IOptions<SQLServerConfig> bdConfig)
        {
            _SQLServerConfig = bdConfig.Value;
        }
        public async Task<AccesAndRefreshTokenDTO> DataInsertRefreshToken(Token tokenRequest)
        {
            try
            {
                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    var parameters = new
                    {
                        IdUser = tokenRequest.token_userid,
                        Token = tokenRequest.token_accesstoken,
                        RefreshToken = tokenRequest.token_refreshToken,
                        ExpirationDate = tokenRequest.token_expiration_date_refreshtoken
                    };
                    var tokensDB = await connection.QueryFirstOrDefaultAsync<AccesAndRefreshTokenDTO>(_sqlInsertRefreshToken, parameters);

                    return tokensDB;
                }
            }
            catch
            {

                throw;
            }

        }

        //devuelve un objeto con todos los campos de la tabla token_history 
        public async Task<Token> DataSelectRefreshToken(int id_user)
        {
            try
            {
                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {

                    var parameters = new { Id = id_user };
                    var refreshToken = await connection.QueryFirstOrDefaultAsync<Token>(_SqlSelectRefreshToken, parameters);
                    return refreshToken;
                }
            }
            catch
            {
                throw;

            }

        }

        public async Task<int> DataDeleteRefreshTokenExpired(int userId)
        {


            try
            {
                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    var parameters = new { @Id = userId };
                    var rowsAffected = await connection.ExecuteAsync(_sqlDeleteRefreshTokenExpired, parameters);

                    return rowsAffected;
                }
            }
            catch
            {
                throw;
            }

        }

    }
}
