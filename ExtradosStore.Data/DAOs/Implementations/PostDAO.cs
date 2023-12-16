using Dapper;
using ExtradosStore.Common.CustomRequest.PostRequest;
using ExtradosStore.Common.CustomRequest.PostSearchRequest;
using ExtradosStore.Configuration.DBConfiguration;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.Models;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Text;

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

        private string _sqlSelectAllPost = "SELECT post_id,post_userid,post_name,post_description,post_price,post_stock,post_categoryId,post_create_at,post_brandId,post_status_id FROM [post] ";

        private string _sqlSetStatusActiveToPaused = @"UPDATE [post] SET post_status_id = @StatusId WHERE post_id = @PostId";

        private string _sqlSelectUserIdByPostId = @"SELECT post_userId FROM [post] WHERE post_id = @PostId";

        private string _sqlUpdateStockPost = @"UPDATE [post] SET post_stock = @Stock, post_status_id = @StatusId WHERE post_id = @PostId";


        private string _sqlSelecPostStatusByPostId = @"select post_status_id From [post] where post_id = @PostId";





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
        public async Task<int> DataGetUserIdByPostId(int postId)
        {
            try
            {
                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    var parameters = new { PostId = postId };
                    return await connection.QueryFirstOrDefaultAsync<int>(_sqlSelectUserIdByPostId, parameters);
                }
            }
            catch
            {

                throw;
            }
        }

        public async Task<int> DataGetStatusIdByPostId(int postId)
        {
            try
            {
                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    var parameters = new { PostId = postId };

                    var PostStatusId = await connection.QueryFirstOrDefaultAsync<int>(_sqlSelecPostStatusByPostId, parameters);

                    return PostStatusId;
                }
            }
            catch
            {

                throw;
            }
        }
        public async Task<int> DataUpdateStockAndSetStatusActive(int postId, int statusId, int newStock)
        {
            try
            {
                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    var parameters = new { PostId = postId, StatusId = statusId, Stock = newStock };
                    return await connection.ExecuteAsync(_sqlUpdateStockPost, parameters);
                }
            }
            catch
            {

                throw;
            }
        }
        public async Task<int> DataSetStatusActiveToPaused(int statusId, int postId)
        {
            try
            {
                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    var parameters = new { StatusId = statusId, PostId = postId };
                    return await connection.ExecuteAsync(_sqlSetStatusActiveToPaused, parameters);
                }
            }
            catch
            {

                throw;
            }
        }

        public async Task<List<Post>> DataGetAllPostActive()
        {
            try
            {
                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {


                    var AllPostActived = (await connection.QueryAsync<Post>(_sqlSelectAllPost)).ToList();
                    return AllPostActived;
                }

            }
            catch
            {

                throw;
            }
        }

        public async Task<int> DataUpdatePost(UpdatePostRequest updateRequest)
        {
            var dynamicParameters = new DynamicParameters();
            var updateFields = new List<string>();

            if (!string.IsNullOrEmpty(updateRequest.postName))
            {
                dynamicParameters.Add("PostName", updateRequest.postName);
                updateFields.Add("post_name = @PostName");
            }

            if (!string.IsNullOrEmpty(updateRequest.postDescription))
            {
                dynamicParameters.Add("PostDescription", updateRequest.postDescription);
                updateFields.Add("post_description = @PostDescription");
            }

            if (updateRequest.postPrice != 0)
            {
                dynamicParameters.Add("PostPrice", updateRequest.postPrice);
                updateFields.Add("post_price = @PostPrice");
            }
            if (updateRequest.postStock != 0)
            {
                dynamicParameters.Add("PostStock", updateRequest.postStock);
                updateFields.Add("post_stock = @PostStock");
            }
            if (updateRequest.postCategoryId != 0)
            {
                dynamicParameters.Add("PostCategoryId", updateRequest.postCategoryId);
                updateFields.Add("post_categoryId = @PostCategoryId");
            }
            if (updateRequest.postBrandId != 0)
            {
                dynamicParameters.Add("PostBranId", updateRequest.postBrandId);
                updateFields.Add("post_brandId = @PostBranId");
            }


            var sql = $"UPDATE [post] SET {string.Join(", ", updateFields)} WHERE post_id = @PostId";
            dynamicParameters.Add("PostId", updateRequest.postId);

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                return await connection.ExecuteAsync(sql, dynamicParameters);
            }
        }


        public async Task<List<Post>> SearchPost(PostSearchRequest postSearchRequest)
        {
            var sqlBuilder = new StringBuilder("SELECT * FROM [post] WHERE 1 = 1");

            var dynamicParameters = new DynamicParameters();

            if (postSearchRequest.postCategoryId != 0)
            {
                sqlBuilder.Append(" AND post_categoryId = @CategoryId");
                dynamicParameters.Add("CategoryId", postSearchRequest.postCategoryId);
            }

            if (postSearchRequest.postBrandId != 0)  // Corregir la condición aquí
            {
                sqlBuilder.Append(" AND post_brandId = @BrandId");
                dynamicParameters.Add("BrandId", postSearchRequest.postBrandId);
            }

            if (!string.IsNullOrEmpty(postSearchRequest.postName))
            {
                sqlBuilder.Append(" AND post_name LIKE @PostName");
                dynamicParameters.Add("PostName", $"%{postSearchRequest.postName}%");
            }

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var listPost = (await connection.QueryAsync<Post>(sqlBuilder.ToString(), dynamicParameters)).ToList();
                Console.WriteLine(dynamicParameters);
                return listPost;
            }
        }



    }
}
