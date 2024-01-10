using Dapper;
using ExtradosStore.Common.CustomRequest.PostRequest;
using ExtradosStore.Common.CustomRequest.PostSearchRequest;
using ExtradosStore.Configuration.DBConfiguration;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.DTOs.PostDTOs;
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
        #region querys
        private string _sqlInsertNewPost = @"Insert INTO[post] (post_userid,post_name,post_description,post_price,post_stock,post_categoryId,post_create_at,post_brandId, post_img)
                                              VALUES (@UserId,@PostName,@PostDescription,@PostPrice,@PostStock,@PostCategoryId,@CreateAt,@PostBrandId,@PostImg) ";

        private string _sqlSetStatusActiveToPaused = @"UPDATE [post] SET post_status_id = @StatusId WHERE post_id = @PostId";

        private string _sqlSelectUserIdByPostId = @"SELECT post_userId FROM [post] WHERE post_id = @PostId";

        private string _sqlUpdateStockPost = @"UPDATE [post] SET post_stock = @Stock, post_status_id = @StatusId WHERE post_id = @PostId";

        private string _sqlSelecPostStatusByPostId = @"select post_status_id From [post] where post_id = @PostId";

        private string _selectStatusAndStockByPostID = "SELECT post_userId,post_stock, post_status_id FROM [post] WHERE post_id = @PostId";

        private string _selectPostNamePriceAndImgByPostId = @"Select post_name,post_price, post_img from [post] where post_id = @PostId";

        private string _selectAllPostActive = @"SELECT 
        p.post_id,
        p.post_name,
        u.user_name,
        p.post_description,
        p.post_price,
        op.offer_post_discount,
        p.post_img,
        o.offer_name,
        c.category_name,
        b.brand_name,
        o.offer_date_expiration
  

FROM 
    [post] p
JOIN 
     [user] u on p.post_userId = u.user_id
LEFT JOIN 
    [offer_post] op on op.offer_post_postId = p.post_id
LEFT JOIN
    [offer] o on op.offer_post_offerId = o.offer_id
JOIN
    [category] c on p.post_categoryId = c.category_id
JOIN
    [brand] b on b.brand_id = p.post_brandId
";
        private string _selectAllPostByUserId = @"SELECT 
        p.post_id,
        p.post_name,
        p.post_userId,
        u.user_name,
        p.post_description,
        p.post_price,
        p.post_img,
        c.category_name,
        b.brand_name,
        ps.post_status_name,
        p.post_stock


FROM 
    [post] p
JOIN 
     [user] u on p.post_userId = u.user_id
JOIN
    [category] c on p.post_categoryId = c.category_id
JOIN
    [brand] b on b.brand_id = p.post_brandId
JOIN
     [post_status] ps on p.post_status_id = ps.post_status_id
 


";
        private string _selectAllPostActiveWithOffer = @"SELECT 
        p.post_id,
        p.post_name,
        u.user_name,
        p.post_description,
        p.post_price,
        op.offer_post_discount,
        p.post_img,
        o.offer_name,
        c.category_name,
        b.brand_name,
        o.offer_date_expiration
  

FROM 
    [post] p
JOIN 
     [user] u on p.post_userId = u.user_id
JOIN 
    [offer_post] op on op.offer_post_postId = p.post_id
JOIN
    [offer] o on op.offer_post_offerId = o.offer_id
JOIN
    [category] c on p.post_categoryId = c.category_id
JOIN
    [brand] b on b.brand_id = p.post_brandId
";

        #endregion

        public async Task<int> DataCreateNewPost(CreateNewPostRequest CreateNewPostRequest)
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
        public async Task<int> DataGetUserIdByPostId(int postId)
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new { PostId = postId };
                return await connection.QueryFirstOrDefaultAsync<int>(_sqlSelectUserIdByPostId, parameters);
            }


        }
        public async Task<StockAndStatusDTO> DataGetStatusAndStockByPostId(int postId)
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new { PostId = postId };
                return await connection.QueryFirstOrDefaultAsync<StockAndStatusDTO>(_selectStatusAndStockByPostID, parameters);
            }


        }

        public async Task<PostPriceImgAndName> DataGetPostPriceNameAndImgById(int postId)
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new { PostId = postId };
                return await connection.QueryFirstOrDefaultAsync<PostPriceImgAndName>(_selectPostNamePriceAndImgByPostId, parameters);
            }

        }
        public async Task<int> DataGetStatusIdByPostId(int postId)
        {
            {
                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    var parameters = new { PostId = postId };

                    var PostStatusId = await connection.QueryFirstOrDefaultAsync<int>(_sqlSelecPostStatusByPostId, parameters);

                    return PostStatusId;
                }
            }

        }
        public async Task<int> DataUpdateStockAndSetStatusActive(int postId, int statusId, int newStock)
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new { PostId = postId, StatusId = statusId, Stock = newStock };
                return await connection.ExecuteAsync(_sqlUpdateStockPost, parameters);
            }

        }
        public async Task<int> DataSetStatusActiveToPaused(int statusId, int postId)
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new { StatusId = statusId, PostId = postId };
                return await connection.ExecuteAsync(_sqlSetStatusActiveToPaused, parameters);
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


        public async Task<List<PostWithOfferDTO>> GetAllPostActive(int statusActiveId)
        {


            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var sqlBuilder = new StringBuilder(_selectAllPostActive);
                sqlBuilder.Append("where post_status_id = @StatusId");
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("StatusId", statusActiveId);

                var allPostActivedWithOffer = (await connection.QueryAsync<PostWithOfferDTO>(sqlBuilder.ToString(), dynamicParameters)).ToList();
                return allPostActivedWithOffer;
            }

        }
        public async Task<List<PostWithOfferDTO>> GetAllPostActiveWithOffer(int statusActiveId, long currentTimeEpoch)
        {


            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var sqlBuilder = new StringBuilder(_selectAllPostActiveWithOffer);
                sqlBuilder.Append("where post_status_id = @StatusId And offer_date_expiration > @CurrentTime");

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("StatusId", statusActiveId);
                dynamicParameters.Add("CurrentTime", currentTimeEpoch);

                var allPostActivedWithOffer = (await connection.QueryAsync<PostWithOfferDTO>(sqlBuilder.ToString(), dynamicParameters)).ToList();
                return allPostActivedWithOffer;
            }

        }


        public async Task<List<PostWithOfferDTO>> SearchPostActive(PostSearchRequest postSearchRequest, int statusActiveId)
        {

            var sqlBuilder = new StringBuilder(_selectAllPostActive);

            var dynamicParameters = new DynamicParameters();

            if (postSearchRequest.postCategoryId != 0)
            {
                sqlBuilder.Append(" AND post_categoryId = @CategoryId");
                dynamicParameters.Add("CategoryId", postSearchRequest.postCategoryId);
            }

            if (postSearchRequest.postBrandId != 0)
            {
                sqlBuilder.Append(" AND post_brandId = @BrandId");
                dynamicParameters.Add("BrandId", postSearchRequest.postBrandId);
            }

            if (!string.IsNullOrEmpty(postSearchRequest.postName))
            {
                sqlBuilder.Append(" AND post_name LIKE @PostName");
                dynamicParameters.Add("PostName", $"%{postSearchRequest.postName}%");
            }
            sqlBuilder.Append("where post_status_id = @StatusId");
            dynamicParameters.Add("StatusId", statusActiveId);
            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var listPost = (await connection.QueryAsync<PostWithOfferDTO>(sqlBuilder.ToString(), dynamicParameters)).ToList();

                return listPost;
            }


        }

        public async Task<PostWithOfferDTO> DataGetPostByPostId(int postId, int statusActiveId)
        {


            var sqlBuilder = new StringBuilder(_selectAllPostActive);
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("PostId", postId);
            sqlBuilder.Append(" where post_status_id = @StatusId");
            sqlBuilder.Append(" AND post_id = @PostId");
            dynamicParameters.Add("StatusId", statusActiveId);

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                return await connection.QueryFirstOrDefaultAsync<PostWithOfferDTO>(sqlBuilder.ToString(), dynamicParameters);
            }


        }
        public async Task<List<PostDTO>> DataAllPostByUserId(int userId)
        {

            var sqlBuilder = new StringBuilder(_selectAllPostByUserId);
            sqlBuilder.Append("where post_userId = @UserId");
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("UserId", userId);



            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                return (await connection.QueryAsync<PostDTO>(sqlBuilder.ToString(), dynamicParameters)).ToList();
            }


        }

    }
}
