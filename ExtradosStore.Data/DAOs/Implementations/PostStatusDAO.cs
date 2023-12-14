using Dapper;
using ExtradosStore.Configuration.DBConfiguration;
using ExtradosStore.Data.DAOs.Interfaces;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace ExtradosStore.Data.DAOs.Implementations
{
    public class PostStatusDAO : IPostStatusDAO
    {

        private SQLServerConfig _SQLServerConfig;

        private string _sqlSelectPostStatusIdByName = "SELECT post_status_id FROM [post_status] WHERE post_status_name = @NameStatus";

        public PostStatusDAO(IOptions<SQLServerConfig> bdConfig)
        {
            _SQLServerConfig = bdConfig.Value;
        }

        public async Task<int> DataGetPostStatusIdByName(string statusName)
        {
            try
            {

                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    var parameters = new { NameStatus = statusName };
                    var idStatus = await connection.QueryFirstOrDefaultAsync<int>(_sqlSelectPostStatusIdByName, parameters);
                    return idStatus;
                }
            }
            catch
            {

                throw;
            }

        }



    }

}
