using ExtradosStore.Common.CustomExceptions.PostExceptions;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.Models;
using ExtradosStore.Services.Interfaces;

namespace ExtradosStore.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryDAO _categoryDao;


        public CategoryService(ICategoryDAO categoryDao)
        {
            _categoryDao = categoryDao;
        }
        public async Task<int> CreateCategoryService(string categoryName)
        {

            try
            {
                var vategoryIdFromDB = await _categoryDao.DataGetCategoryIdByName(categoryName.ToLower());
                if (vategoryIdFromDB != 0) throw new DuplicateNameCategoryException();
                var rowsAffected = await _categoryDao.DataCreateNewCategory(categoryName.ToLower());
                return rowsAffected;
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> DeleteCategoryByIdService(int categoryId)
        {
            try
            {
                if (await _categoryDao.DataGetCategoryIdByID(categoryId) == 0) throw new KeyNotFoundException("id category not found in database");
                var rowsAffected = await _categoryDao.DataDeleteCategoryByID(categoryId);
                return rowsAffected;


            }
            catch
            {

                throw;
            }
        }

        public async Task<List<Category>> GetAllCategorysService()
        {
            try
            {
                return await _categoryDao.DataGetAllCategorys();

            }
            catch
            {

                throw;
            }
        }
    }


}

