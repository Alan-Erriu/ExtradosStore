using ExtradosStore.Entities.Models;

namespace ExtradosStore.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<int> CreateCategoryService(string categoryName);
        Task<int> DeleteCategoryByIdService(int categoryId);
        Task<List<Category>> GetAllCategorysService();
    }
}