using Dapper;
using ExtradosStore.Common.CustomRequest.PostRequest;
using ExtradosStore.Configuration.DBConfiguration;
using ExtradosStore.Data.DAOs.Interfaces;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace ExtradosStore.Data.DAOs.Implementations
{
    public class PostDAO : IPostDAO
    {

        private SQLServerConfig _SQLServerConfig;
        public PostDAO(IOptions<SQLServerConfig> bdConfig)
        {
            _SQLServerConfig = bdConfig.Value;
        }

        private string _sqlInsertNewPost = @"Insert INTO[post] (post_userid,post_name,post_description,post_price,post_stock,post_categoryId,post_create_at,post_brandId)
                                              VALUES (@UserId,@PostName,@PostDescription,@PostPrice,@PostStock,@PostCategoryId,@CreateAt,@PostBrandId) ";




        public async Task<int> DataCreateNewPost(CreateNewPostRequest CreateNewPostRequest)
        {
            try
            {
                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    var parameters = new
                    {
                        UserId = CreateNewPostRequest.post_userId,
                        PostName = CreateNewPostRequest.post_name,
                        PostDescription = CreateNewPostRequest.post_description,
                        PostPrice = CreateNewPostRequest.post_price,
                        PostStock = CreateNewPostRequest.post_stock,
                        PostImg = CreateNewPostRequest.post_img,
                        PostCategoryId = CreateNewPostRequest.category_id,
                        PostBrandId = CreateNewPostRequest.brand_id,
                        CreateAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                    };

                    var rowsAffected = await connection.ExecuteAsync(_sqlInsertNewPost, parameters);

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
