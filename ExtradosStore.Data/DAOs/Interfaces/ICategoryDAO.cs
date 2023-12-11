using ExtradosStore.Entities.Models;

namespace ExtradosStore.Data.DAOs.Interfaces
{
    public interface ICategoryDAO
    {
        Task<int> DataCreateNewCategory(string CategoryName);
        Task<int> DataDeleteCategoryByID(int categoryId);
        Task<List<Category>> DataGetAllCategorys();
        Task<int> DataGetCategoryIdByID(int categoryId);

        Task<int> DataGetCategoryIdByName(string categoryName);
    }
}