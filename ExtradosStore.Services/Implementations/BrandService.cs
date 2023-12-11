using ExtradosStore.Common.CustomExceptions.PostExceptions;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.Models;
using ExtradosStore.Services.Interfaces;

namespace ExtradosStore.Services.Implementations
{
    public class BrandService : IBrandService
    {
        private readonly IBrandDAO _brandDao;


        public BrandService(IBrandDAO brandDao)
        {
            _brandDao = brandDao;

        }

        public async Task<int> CreateBrandService(string brandNameRequest)
        {
            try
            {
                var BrandIdFromDB = await _brandDao.DataGetBrandIdByName(brandNameRequest.ToLower());
                if (BrandIdFromDB != 0) throw new DuplicateNameBrandException();
                var rowsAffected = await _brandDao.DataCreateNewBrand(brandNameRequest.ToLower());
                return rowsAffected;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Brand>> GetAllBrandsService()
        {
            try
            {
                return await _brandDao.DataGetAllBrands();

            }
            catch
            {

                throw;
            }
        }

        public async Task<int> DeleteBrandByIdService(int BrandId)
        {
            try
            {
                if (await _brandDao.DataGetBrandIdByID(BrandId) == 0) throw new KeyNotFoundException("id brand not found in database");
                var rowsAffected = await _brandDao.DataDeleteBrandByID(BrandId);
                if (rowsAffected == 0) throw new InvalidOperationException("this brand is associate a to post");
                return rowsAffected;


            }
            catch
            {

                throw;
            }
        }

    }
}
