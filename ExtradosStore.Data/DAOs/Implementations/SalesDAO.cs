using Dapper;
using ExtradosStore.Configuration.DBConfiguration;
using ExtradosStore.Data.DAOs.Interfaces;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace ExtradosStore.Data.DAOs.Implementations
{
    public class SalesDAO : ISalesDAO
    {
        private SQLServerConfig _SQLServerConfig;
        public SalesDAO(IOptions<SQLServerConfig> bdConfig)
        {
            _SQLServerConfig = bdConfig.Value;

        }

        private string _sqlInsertSales = @"INSERT INTO [sales] (user_id, date_sale, total) 
                                       OUTPUT INSERTED.sales_id
                                       VALUES (@UserId, @DateSale, @Total)";


        public async Task<int> DataCreateNewSales(int userId, decimal total)
        {
            try
            {
                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    var parameters = new
                    {
                        UserId = userId,
                        Total = total,
                        DateSale = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                    };
                    return await connection.QueryFirstOrDefaultAsync<int>(_sqlInsertSales, parameters);
                }
            }
            catch
            {

                throw;
            }
        }
    }
}
