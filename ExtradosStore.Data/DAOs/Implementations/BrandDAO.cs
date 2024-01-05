using Dapper;
using ExtradosStore.Configuration.DBConfiguration;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.Models;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace ExtradosStore.Data.DAOs.Implementations
{
    public class BrandDAO : IBrandDAO
    {


        private SQLServerConfig _SQLServerConfig;
        public BrandDAO(IOptions<SQLServerConfig> bdConfig)
        {
            _SQLServerConfig = bdConfig.Value;

        }

        private string _sqlInsertNewBrand = "INSERT INTO [brand] (brand_name) values (@BrandName) ";

        private string _sqlGetBrandIdById = "SELECT brand_id FROM [brand] where brand_id = @BrandId";

        private string _sqlDeleteBrandById = "DELETE FROM [brand] WHERE brand_id =@BrandId";

        private string _sqlGetBrandIdByName = "SELECT brand_id FROM [brand] WHERE brand_name =@BrandName";

        private string _getAllBrands = "select brand_id, brand_name FROM [brand]";

        public async Task<int> DataCreateNewBrand(string brandName)
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new
                {
                    BrandName = brandName
                };

                var rowsAffected = await connection.ExecuteAsync(_sqlInsertNewBrand, parameters);

                return rowsAffected;
            }

        }

        public async Task<int> DataGetBrandIdByID(int brandId)
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new
                {
                    BrandId = brandId
                };

                var brandIdFromDB = await connection.QueryFirstOrDefaultAsync<int>(_sqlGetBrandIdById, parameters);

                return brandIdFromDB;
            }

        }

        public async Task<int> DataGetBrandIdByName(string brandName)
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new
                {
                    BrandName = brandName
                };

                var rowsAffected = await connection.QueryFirstOrDefaultAsync<int>(_sqlGetBrandIdByName, parameters);

                return rowsAffected;
            }

        }

        public async Task<int> DataDeleteBrandByID(int brandId)
        {
            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new
                {
                    BrandId = brandId
                };

                var rowsAffected = await connection.ExecuteAsync(_sqlDeleteBrandById, parameters);

                return rowsAffected;
            }

        }

        public async Task<List<Brand>> DataGetAllBrands()
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {



                return (await connection.QueryAsync<Brand>(_getAllBrands)).ToList();

            }

        }

    }

}
