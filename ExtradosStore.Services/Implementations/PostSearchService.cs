using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.DTOs.PostDTOs;
using ExtradosStore.Services.Interfaces;

namespace ExtradosStore.Services.Implementations
{
    public class PostSearchService : IPostSearchService
    {



        private readonly IPostDAO _postDao;
        private readonly IBrandDAO _brandDao;
        private readonly ICategoryDAO _categoryDao;
        private readonly IPostStatusDAO _postStatusDAO;
        private readonly IOfferDAO _offerDao;
        private readonly IOfferPostDAO _offerPostDao;
        private readonly IUserDAO _userDao;


        public PostSearchService(IPostDAO postDAO, IBrandDAO brandDAO, ICategoryDAO categoryDAO,
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



        //faltan validaciones
        public async Task<List<PostWithOfferDTO>> GetAllPostWithOfferService()
        {
            try
            {
                var currentEpochTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
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
                              select new PostWithOfferDTO
                              {
                                  post_id = post.post_id,
                                  post_name = post.post_name,
                                  user_name = user.user_name,
                                  post_description = post.post_description,
                                  post_price = post.post_price,
                                  offer_post_discount = offerPost.offer_post_discount,
                                  priceNow = post.post_price - (post.post_price * offerPost.offer_post_discount / 100),
                                  img = post.post_img,
                                  offer_name = offer.offer_name,
                                  offer_status = (offer.offer_date_expiration > currentEpochTime) ? "active" : "expired",
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
        //faltan validaciones
        public async Task<List<PostWithOfferDTO>> GetAllPostActiveWithOfferService()
        {
            try
            {
                var currentEpochTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
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
                              where offer.offer_date_expiration > currentEpochTime
                              select new PostWithOfferDTO
                              {
                                  post_id = post.post_id,
                                  post_name = post.post_name,
                                  user_name = user.user_name,
                                  post_description = post.post_description,
                                  post_price = post.post_price,
                                  offer_post_discount = offerPost.offer_post_discount,
                                  priceNow = post.post_price - (post.post_price * offerPost.offer_post_discount / 100),
                                  img = post.post_img,
                                  offer_name = offer.offer_name,
                                  offer_status = (offer.offer_date_expiration > currentEpochTime) ? "active" : "expired",
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
