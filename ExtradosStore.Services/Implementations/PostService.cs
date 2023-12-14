using ExtradosStore.Common.CustomExceptions.PostExceptions;
using ExtradosStore.Common.CustomExceptions.PostStatusExceptions;
using ExtradosStore.Common.CustomRequest.PostRequest;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.DTOs.PostDTOs;
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

        public async Task<int> SetStatusActiveToPaused(int postId, int userIdFromToken, string nameRole, string newStatus)
        {
            try
            {

                var postUserIdFromDB = await _postDao.DataGetUserIdByPostUserId(postId);
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

                var postUserIdFromDB = await _postDao.DataGetUserIdByPostUserId(postId);
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
        //faltan validaciones
        public async Task<List<PostActiveDTO>> GetAllPostActiveService()
        {
            try
            {

                var listPost = await _postDao.DataGetAllPostActive();
                var listUser = await _userDao.DataGetAllUser();
                var listOfferPost = await _offerPostDao.DataGetAllOfferPost();
                var listOffer = await _offerDao.GetAllOffer();
                var listBrand = await _brandDao.DataGetAllBrands();
                var listCategory = await _categoryDao.DataGetAllCategorys();


                var result = (from post in listPost
                              join user in listUser on post.post_userId equals user.user_id
                              join offerPost in listOfferPost on post.post_id equals offerPost.offer_post_postId
                              join offer in listOffer on offerPost.offer_post_offerId equals offer.offer_id
                              join category in listCategory on post.post_categoryId equals category.category_id
                              join brand in listBrand on post.post_brandId equals brand.brand_id
                              select new PostActiveDTO
                              {
                                  post_id = post.post_id,
                                  post_name = post.post_name,
                                  user_name = user.user_name,
                                  post_description = post.post_description,
                                  post_price = post.post_price,
                                  offer_post_discount = offerPost.offer_post_discount,
                                  priceNow = post.post_price,
                                  img = post.post_img,
                                  offer_name = offer.offer_name,
                                  category_name = category.category_name,
                                  brand_name = brand.brand_name
                              }).ToList();

                return result;
            }
            catch
            {
                throw;
            }
        }


    }
}
