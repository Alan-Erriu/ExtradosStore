using ExtradosStore.Common.CustomExceptions.PostExceptions;
using ExtradosStore.Common.CustomRequest.PostRequest;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Services.Interfaces;

namespace ExtradosStore.Services.Implementations
{
    public class PostService : IPostService
    {
        private readonly IPostDAO _postDao;
        private readonly IBrandDAO _brandDao;
        private readonly ICategoryDAO _categoryDao;


        public PostService(IPostDAO postDAO, IBrandDAO brandDAO, ICategoryDAO categoryDAO)
        {
            _postDao = postDAO;
            _brandDao = brandDAO;
            _categoryDao = categoryDAO;
        }





        public async Task<int> CreatePostService(CreateNewPostRequest postRequest)
        {
            try
            {
                var brandId = await _brandDao.DataGetBrandIdByID(postRequest.brand_id);
                if (brandId == 0) throw new BrandNotFoundException();
                var categoryId = await _categoryDao.DataGetCategoryIdByID(postRequest.category_id);
                if (categoryId == 0) throw new CategoryNotFoundException();

                return await _postDao.DataCreateNewPost(postRequest);

            }
            catch
            {

                throw;
            }

        }
    }
}
