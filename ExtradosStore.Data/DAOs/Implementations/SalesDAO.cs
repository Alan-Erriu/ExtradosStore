﻿using Dapper;
using ExtradosStore.Configuration.DBConfiguration;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.DTOs.SalesDTO;
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
         private string test = "";
        private string _sqlInsertSales = @"INSERT INTO [sales] (user_id, date_sale, total) 
                                       OUTPUT INSERTED.sales_id
                                       VALUES (@UserId, @DateSale, @Total)";

        private string _sqlSelectAllSalesByUserId = "SELECT sales_id,user_id, date_sale FROM [sales] WHERE user_id=@UserId";

        public async Task<int> DataCreateNewSales(int userId, decimal total)
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
        public async Task<List<SaleDTO>> DataGetAllSalesByUserId(int userId)
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new { UserId = userId };
                var AllSales = (await connection.QueryAsync<SaleDTO>(_sqlSelectAllSalesByUserId, parameters)).ToList();

                return AllSales;
            }

        }

        public async Task<List<SaleDTO>> DataGetAllSalesByUserName(int userId)
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new { UserId = userId };
                var AllSales = (await connection.QueryAsync<SaleDTO>(_sqlSelectAllSalesByUserId, parameters)).ToList();

                return AllSales;
            }

        }
    }
}
