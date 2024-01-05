using ExtradosStore.Common.CustomExceptions.GenericResponsesExceptions;
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

            var BrandIdFromDB = await _brandDao.DataGetBrandIdByName(brandNameRequest.ToLower());
            if (BrandIdFromDB != 0) throw new ConflictException("the name brand is already in use");
            var rowsAffected = await _brandDao.DataCreateNewBrand(brandNameRequest.ToLower());
            return rowsAffected;

        }
        public async Task<List<Brand>> GetAllBrandsService()
        {

            return await _brandDao.DataGetAllBrands();


        }

        public async Task<int> DeleteBrandByIdService(int BrandId)
        {
            if (await _brandDao.DataGetBrandIdByID(BrandId) == 0) throw new NotFoundException("id brand not found in database");
            var rowsAffected = await _brandDao.DataDeleteBrandByID(BrandId);
            if (rowsAffected == 0) throw new InvalidOperationException("this brand is associate a to post");
            return rowsAffected;



        }

    }
}
