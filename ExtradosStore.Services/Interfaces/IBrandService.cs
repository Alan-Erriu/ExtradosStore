using ExtradosStore.Entities.Models;

namespace ExtradosStore.Services.Interfaces
{
    public interface IBrandService
    {
        Task<int> CreateBrandService(string brandName);
        Task<int> DeleteBrandByIdService(int BrandId);
        Task<List<Brand>> GetAllBrandsService();
    }
}