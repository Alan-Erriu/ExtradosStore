using Dapper;
using ExtradosStore.Configuration.DBConfiguration;
using ExtradosStore.Data.DAOs.Interfaces;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace ExtradosStore.Data.DAOs.Implementations
{
    public class SalesDetailDAO : ISalesDetailDAO
    {
        private SQLServerConfig _SQLServerConfig;
        public SalesDetailDAO(IOptions<SQLServerConfig> bdConfig)
        {
            _SQLServerConfig = bdConfig.Value;

        }

        private string _sqlInsertSalesDetail = @"INSERT INTO [sales_detail] (sales_id,post_id,quantity,price,discount,subtotal)
                                                 VALUES(@SalesId,@PostId,@Quantity,@Price,@Discount,@Subtotal)";


        public async Task<int> DataCreateNewSalesDetail(int salesId, int postId, int quantity, decimal price, int discount)
        {
            try
            {
                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    var parameters = new
                    {
                        SalesId = salesId,
                        PostId = postId,
                        Quantity = quantity,
                        Price = price,
                        Discount = discount,
                        Subtotal = price * (1 - discount / 100) * quantity
                    };

                    return await connection.ExecuteAsync(_sqlInsertSalesDetail, parameters);
                }
            }
            catch
            {

                throw;
            }
        }
    }
}
