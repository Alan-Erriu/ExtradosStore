using ExtradosStore.Common.CustomExceptions.PostExceptions;
using ExtradosStore.Common.CustomExceptions.PostStatusExceptions;
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
        private readonly IPostStatusDAO _postStatusDAO;
        private readonly IOfferDAO _offerDao;
        private readonly IOfferPostDAO _offerPostDao;
        private readonly IUserDAO _userDao;


        public PostService(IPostDAO postDAO, IBrandDAO brandDAO, ICategoryDAO categoryDAO,
            IPostStatusDAO postStatusDAO, IOfferDAO offerDao, IOfferPostDAO offerPostDao, IUserDAO userDao)
        {
            _postDao = postDAO;
            _brandDao = brandDAO;
            _categoryDao = categoryDAO;
            _postStatusDAO = postStatusDAO;
            _offerDao = offerDao;
            _offerPostDao = offerPostDao;
            _userDao = userDao;
        }





        public async Task<int> CreatePostService(CreateNewPostRequest postRequest)
        {
            try
            {
                var brandId = await _brandDao.DataGetBrandIdByID(postRequest.brand_id);
                if (brandId == 0) throw new BrandNotFoundException();

                var categoryId = await _categoryDao.DataGetCategoryIdByID(postRequest.category_id);
                if (categoryId == 0) throw new CategoryNotFoundException();

                var statusActiveId = await _postStatusDAO.DataGetPostStatusIdByName("active");
                if (statusActiveId == 0) throw new PostStatusNotFoundException();
                postRequest.post_status_id = statusActiveId;



                return await _postDao.DataCreateNewPost(postRequest);

            }
            catch
            {

                throw;
            }
        }

        public async Task<int> SetPostStatus(int postId, int userIdFromToken, string nameRole, string newStatus)
        {
            try
            {

                var postUserIdFromDB = await _postDao.DataGetUserIdByPostId(postId);
                if (userIdFromToken != postUserIdFromDB && nameRole == "user")
                {
                    throw new UnauthorizedAccessException("the user role can only modify the status of their posts");
                }

                var statusPausedId = await _postStatusDAO.DataGetPostStatusIdByName(newStatus);
                if (statusPausedId == 0) throw new PostStatusNotFoundException();


                return await _postDao.DataSetStatusActiveToPaused(statusPausedId, postId);
            }
            catch
            {

                throw;
            }
        }
        public async Task<int> SetStatusActiveAndUpdateStock(int postId, int userIdFromToken, string newStatus, int newStock)
        {
            try
            {

                var postUserIdFromDB = await _postDao.DataGetUserIdByPostId(postId);
                if (userIdFromToken != postUserIdFromDB) throw new UnauthorizedAccessException("the user can only modify the status of their posts");

                var statusPausedId = await _postStatusDAO.DataGetPostStatusIdByName(newStatus);
                if (statusPausedId == 0) throw new PostStatusNotFoundException();

                var rowsAffected = await _postDao.DataUpdateStockAndSetStatusActive(postId, statusPausedId, newStock);


                return rowsAffected;
            }
            catch
            {

                throw;
            }
        }



        public async Task<int> UpdatePostService(UpdatePostRequest updateRequest, int userIdFromToken)
        {
            try
            {
                var postUserIdFromDB = await _postDao.DataGetUserIdByPostId(updateRequest.postId);
                if (postUserIdFromDB == 0) throw new KeyNotFoundException("post not found in data base");
                if (userIdFromToken != postUserIdFromDB) throw new UnauthorizedAccessException("the user can only modify the status of their posts");

                var rowsAffected = await _postDao.DataUpdatePost(updateRequest);
                return rowsAffected;

            }
            catch
            {
                throw;
            }
        }

    }
}

