using ExtradosStore.Common.CustomExceptions.GenericResponsesExceptions;
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



        public PostService(IPostDAO postDAO, IBrandDAO brandDAO, ICategoryDAO categoryDAO,
            IPostStatusDAO postStatusDAO)
        {
            _postDao = postDAO;
            _brandDao = brandDAO;
            _categoryDao = categoryDAO;
            _postStatusDAO = postStatusDAO;

        }





        public async Task<int> CreatePostService(CreateNewPostRequest postRequest)
        {

            var brandId = await _brandDao.DataGetBrandIdByID(postRequest.brand_id);
            if (brandId == 0) throw new NotFoundException("The id brand not found");

            var categoryId = await _categoryDao.DataGetCategoryIdByID(postRequest.category_id);
            if (categoryId == 0) throw new NotFoundException("The id category not found");

            var statusActiveId = await _postStatusDAO.DataGetPostStatusIdByName("active");
            if (statusActiveId == 0) throw new NotFoundException("The id postStatus not found");
            postRequest.post_status_id = statusActiveId;

            return await _postDao.DataCreateNewPost(postRequest);

        }

        public async Task<int> SetPostStatus(int postId, int userIdFromToken, string nameRole, string newStatus)
        {

            var postUserIdFromDB = await _postDao.DataGetUserIdByPostId(postId);
            if (userIdFromToken != postUserIdFromDB && nameRole == "user")
            {
                throw new UnauthorizedException("the user role can only modify the status of their posts");
            }

            var statusPausedId = await _postStatusDAO.DataGetPostStatusIdByName(newStatus);
            if (statusPausedId == 0) throw new NotFoundException("The id postStatus not found");


            return await _postDao.DataSetStatusActiveToPaused(statusPausedId, postId);
        }


        public async Task<int> SetStatusActiveAndUpdateStock(int postId, int userIdFromToken, string newStatus, int newStock)
        {


            var postUserIdFromDB = await _postDao.DataGetUserIdByPostId(postId);
            if (userIdFromToken != postUserIdFromDB) throw new UnauthorizedException("the user can only modify the status of their posts");

            var statusPausedId = await _postStatusDAO.DataGetPostStatusIdByName(newStatus);
            if (statusPausedId == 0) throw new NotFoundException("The id postStatus not found");

            var rowsAffected = await _postDao.DataUpdateStockAndSetStatusActive(postId, statusPausedId, newStock);


            return rowsAffected;


        }



        public async Task<int> UpdatePostService(UpdatePostRequest updateRequest, int userIdFromToken)
        {

            var postUserIdFromDB = await _postDao.DataGetUserIdByPostId(updateRequest.postId);
            if (postUserIdFromDB == 0) throw new NotFoundException("post not found");
            if (userIdFromToken != postUserIdFromDB) throw new UnauthorizedException("the user can only modify the status of their posts");

            var rowsAffected = await _postDao.DataUpdatePost(updateRequest);
            return rowsAffected;



        }

    }
}

