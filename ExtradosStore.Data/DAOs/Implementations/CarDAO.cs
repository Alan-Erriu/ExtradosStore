using Dapper;
using ExtradosStore.Common.CustomRequest.CarRequest;
using ExtradosStore.Configuration.DBConfiguration;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.DTOs.PostDTOs;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace ExtradosStore.Data.DAOs.Implementations
{
    public class CarDAO : ICarDAO
    {
        private SQLServerConfig _SQLServerConfig;
        public CarDAO(IOptions<SQLServerConfig> bdConfig)
        {
            _SQLServerConfig = bdConfig.Value;

        }
        #region querys
        private string _sqlInsertIntoCar = @"INSERT INTO [car] (user_id, post_id, quantity) VALUES (@UserID, @PostID,@Quantity)";

        private string _sqlSelectAllRows = @"SELECT  post_id, quantity FROM [car] WHERE user_id = @UserId";

        private string _sqlUpdateQuantity = @"UPDATE [car] SET quantity = @Quantity WHERE user_id = @UserId";

        private string _sqlSelectQuantityByPostAndUserId = @"SELECT quantity FROM [car] WHERE post_id = @PostId AND user_id = @UserId";

        private string _sqlDeleteCarItemByPostAndUserId = @"DELETE FROM [car] WHERE post_id = @PostId AND user_id = @UserId";

        private string _sqlDeleteAllItemsFromCarByuserId = @"DELETE FROM [car] WHERE user_id=@UserId";
        #endregion
        public async Task<int> DataDeleteAllItemsFromCar(int userId)
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new
                {
                    UserId = userId,

                };
                var rowsAffected = await connection.ExecuteAsync(_sqlDeleteAllItemsFromCarByuserId, parameters);
                return rowsAffected;
            };


        }


        public async Task<int> DataAddtoCar(AddToCarRequest addToCarRequest, int userId)
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new
                {
                    UserId = userId,
                    PostId = addToCarRequest.post_id,
                    Quantity = addToCarRequest.quantity
                };
                var rowsAffected = await connection.ExecuteAsync(_sqlInsertIntoCar, parameters);
                return rowsAffected;
            };


        }
        public async Task<List<CarDTO>> DataGetCarByUserId(int userId)
        {
            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new { UserId = userId };
                var listCar = (await connection.QueryAsync<CarDTO>(_sqlSelectAllRows, parameters)).ToList();
                return listCar;
            }


        }
        public async Task<int> DataGetQuantityByPostAndUserId(int postId, int userId)
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new { UserId = userId, PostId = postId };
                var quantity = await connection.QueryFirstOrDefaultAsync<int>(_sqlSelectQuantityByPostAndUserId, parameters);
                return quantity;
            }


        }
        public async Task<int> DataDeleteCarItem(int postId, int userId)
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new { UserId = userId, PostId = postId };
                return await connection.ExecuteAsync(_sqlDeleteCarItemByPostAndUserId, parameters);

            }


        }
        public async Task<int> DataUpdateQuantity(int quantity, int userId)
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new { Quantity = quantity, UserId = userId };
                return await connection.ExecuteAsync(_sqlUpdateQuantity, parameters);
            }

        }
    }
}
