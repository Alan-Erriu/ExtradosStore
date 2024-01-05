using Dapper;
using ExtradosStore.Configuration.DBConfiguration;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.Models;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace ExtradosStore.Data.DAOs.Implementations
{
    public class CategoryDAO : ICategoryDAO
    {
        private SQLServerConfig _SQLServerConfig;
        public CategoryDAO(IOptions<SQLServerConfig> bdConfig)
        {
            _SQLServerConfig = bdConfig.Value;

        }
        #region querys
        private string _sqlInsertNewCategory = "INSERT INTO [category] (category_name) values (@CategoryName) ";

        private string _sqlGetCategoryIdById = "SELECT category_id FROM [category] where category_id = @CategoryId";

        private string _sqlDeleteCategoryById = "DELETE FROM [category] WHERE category_id =@CategoryId";

        private string _getAllCategorys = "select category_id, category_name FROM [category]";

        private string _sqlgetCategoryIdByname = "select category_id from [category] where category_name = @CategoryName";
        #endregion
        public async Task<int> DataCreateNewCategory(string categoryName)
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new
                {
                    CategoryName = categoryName
                };

                var rowsAffected = await connection.ExecuteAsync(_sqlInsertNewCategory, parameters);

                return rowsAffected;
            }

        }

        public async Task<int> DataGetCategoryIdByID(int categoryId)
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new
                {
                    CategoryID = categoryId
                };

                var categoryFromDB = await connection.QueryFirstOrDefaultAsync<int>(_sqlGetCategoryIdById, parameters);

                return categoryFromDB;
            }

        }
        public async Task<int> DataGetCategoryIdByName(string categoryName)
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new
                {
                    CategoryName = categoryName
                };

                var categoryFromDB = await connection.QueryFirstOrDefaultAsync<int>(_sqlgetCategoryIdByname, parameters);

                return categoryFromDB;
            }

        }

        public async Task<int> DataDeleteCategoryByID(int categoryId)
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new
                {
                    CategoryId = categoryId
                };

                var rowsAffected = await connection.ExecuteAsync(_sqlDeleteCategoryById, parameters);
                Console.WriteLine(rowsAffected);
                return rowsAffected;
            }

        }

        public async Task<List<Category>> DataGetAllCategorys()
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {



                return (await connection.QueryAsync<Category>(_getAllCategorys)).ToList();

            }

        }
    }
}
