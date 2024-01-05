using Dapper;
using ExtradosStore.Configuration.DBConfiguration;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.DTOs.SalesDTO;
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

        private string _sqlSelectAllSalesDetailsBySalesId = @"SELECT post_id,quantity,subtotal,sales_id FROM[sales_detail]WHERE sales_id=@SaleId";


        public async Task<int> DataCreateNewSalesDetail(int salesId, int postId, int quantity, decimal price, int discount)
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
        public async Task<List<SalesDetailDTO>> DataGetAllSalesDetailBySalesId(int saleId)
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new { SaleId = saleId };
                var AllSalesDetail = (await connection.QueryAsync<SalesDetailDTO>(_sqlSelectAllSalesDetailsBySalesId, parameters)).ToList();

                return AllSalesDetail;
            }


        }
    }
}
