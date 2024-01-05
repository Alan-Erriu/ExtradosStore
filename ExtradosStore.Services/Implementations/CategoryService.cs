using ExtradosStore.Common.CustomExceptions.GenericResponsesExceptions;
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
                if (vategoryIdFromDB != 0) throw new ConflictException("The name category is already in use");
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
                if (await _categoryDao.DataGetCategoryIdByID(categoryId) == 0) throw new NotFoundException("id category not found");
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
            return await _categoryDao.DataGetAllCategorys();
        }
    }


}

