using ExtradosStore.Entities.Models;

namespace ExtradosStore.Data.DAOs.Interfaces
{
    public interface IBrandDAO
    {
        Task<int> DataCreateNewBrand(string brandName);
        Task<int> DataDeleteBrandByID(int brandId);

        Task<int> DataGetBrandIdByName(string brandName);
        Task<List<Brand>> DataGetAllBrands();
        Task<int> DataGetBrandIdByID(int brandId);
    }
}