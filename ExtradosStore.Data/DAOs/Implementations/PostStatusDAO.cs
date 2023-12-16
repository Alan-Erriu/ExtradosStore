using Dapper;
using ExtradosStore.Configuration.DBConfiguration;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.Models;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace ExtradosStore.Data.DAOs.Implementations
{
    public class PostStatusDAO : IPostStatusDAO
    {

        private SQLServerConfig _SQLServerConfig;

        private string _sqlSelectPostStatusIdByName = "SELECT post_status_id FROM [post_status] WHERE post_status_name = @NameStatus";

        private string _sqlSelectAllPostStatus = @"SELECT post_status_id, post_status_name FROM [post_status]";

        public PostStatusDAO(IOptions<SQLServerConfig> bdConfig)
        {
            _SQLServerConfig = bdConfig.Value;
        }


        public async Task<List<PostStatus>> DataGetAllPostStatus()
        {
            try
            {
                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    var lisPostStatus = (await connection.QueryAsync<PostStatus>(_sqlSelectAllPostStatus)).ToList();
                    return lisPostStatus;
                }
            }
            catch
            {

                throw;
            }
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
