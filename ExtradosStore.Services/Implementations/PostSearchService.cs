using ExtradosStore.Common.CustomRequest.PostSearchRequest;
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



        //todas las publicaciones con ofertas, vencidas o no vencidas
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
        //todas las publicaciones activas, con ofertas vencidas
        public async Task<List<PostWithOfferDTO>> GetAllActivePostsWithNoOfferOrExpiredOffer()
        {
            try
            {
                var listPost = await _postDao.DataGetAllPostActive();
                var listUser = await _userDao.DataGetAllUser();
                var listOfferPost = await _offerPostDao.DataGetAllOfferPost();
                var listOffer = await _offerDao.GetAllOffer();
                var listBrand = await _brandDao.DataGetAllBrands();
                var listCategory = await _categoryDao.DataGetAllCategorys();
                var listPostStatus = await _postStatusDAO.DataGetAllPostStatus();


                var currentEpochTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();


                var result = (from post in listPost
                              join user in listUser on post.post_userId equals user.user_id
                              join offerPost in listOfferPost on post.post_id equals offerPost.offer_post_postId into offerPosts
                              from offerPost in offerPosts.DefaultIfEmpty()
                              join offer in listOffer on offerPost?.offer_post_offerId equals offer?.offer_id into offers
                              from offer in offers.DefaultIfEmpty()
                              join category in listCategory on post.post_categoryId equals category.category_id
                              join brand in listBrand on post.post_brandId equals brand.brand_id
                              join postStatus in listPostStatus on post.post_status_id equals postStatus.post_status_id
                              where (offer == null || offer.offer_date_expiration < currentEpochTime) && postStatus.post_status_name == "active"
                              select new PostWithOfferDTO
                              {
                                  post_id = post.post_id,
                                  post_name = post.post_name,
                                  user_name = user.user_name,
                                  post_description = post.post_description,
                                  post_price = post.post_price,
                                  offer_post_discount = offerPost?.offer_post_discount ?? 0,
                                  priceNow = post.post_price - (post.post_price * (offerPost?.offer_post_discount ?? 0) / 100),
                                  img = post.post_img,
                                  offer_name = offer?.offer_name,
                                  offer_status = (offer?.offer_date_expiration > currentEpochTime) ? "active" : "expired",
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
        //todas las publicaciones activas con ofertas no vencidas
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
                var listPostStatus = await _postStatusDAO.DataGetAllPostStatus();





                var result = (from post in listPost
                              join user in listUser on post.post_userId equals user.user_id
                              join offerPost in listOfferPost on post.post_id equals offerPost.offer_post_postId
                              join offer in listOffer on offerPost.offer_post_offerId equals offer.offer_id
                              join category in listCategory on post.post_categoryId equals category.category_id
                              join brand in listBrand on post.post_brandId equals brand.brand_id
                              join postStatus in listPostStatus on post.post_status_id equals postStatus.post_status_id
                              where offer.offer_date_expiration > currentEpochTime && postStatus.post_status_name == "active"
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
        //todas las publicaciones activas con ofertas no vencidas pertenecientes a un id de oferta
        public async Task<List<PostWithOfferDTO>> GetAllPostActiveByOfferId(int offerId)
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
                var listPostStatus = await _postStatusDAO.DataGetAllPostStatus();




                var result = (from post in listPost
                              join user in listUser on post.post_userId equals user.user_id
                              join offerPost in listOfferPost on post.post_id equals offerPost.offer_post_postId
                              join offer in listOffer on offerPost.offer_post_offerId equals offer.offer_id
                              join category in listCategory on post.post_categoryId equals category.category_id
                              join brand in listBrand on post.post_brandId equals brand.brand_id
                              join postStatus in listPostStatus on post.post_status_id equals postStatus.post_status_id
                              where offer.offer_date_expiration > currentEpochTime && offer.offer_id == offerId && postStatus.post_status_name == "active"

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
        //todas las publicaciones activas, no activas, con o sin oferta vencida. Perteneciente a un id de usuario.
        public async Task<List<PostWithOfferDTO>> GetAllPostByUserId(int userId)
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
                var listPostStatus = await _postStatusDAO.DataGetAllPostStatus();

                var result = (from post in listPost
                              join user in listUser on post.post_userId equals user.user_id
                              join offerPost in listOfferPost on post.post_id equals offerPost.offer_post_postId into offerPosts
                              from offerPost in offerPosts.DefaultIfEmpty()
                              join offer in listOffer on (offerPost != null ? (int?)offerPost.offer_post_offerId : null) equals offer.offer_id into offers
                              from offer in offers.DefaultIfEmpty()
                              join category in listCategory on post.post_categoryId equals category.category_id
                              join brand in listBrand on post.post_brandId equals brand.brand_id
                              join postStatus in listPostStatus on post.post_status_id equals postStatus.post_status_id
                              where post.post_userId == userId
                              select new PostWithOfferDTO
                              {
                                  post_id = post.post_id,
                                  post_name = post.post_name,
                                  user_name = user.user_name,
                                  post_description = post.post_description,
                                  post_price = post.post_price,
                                  offer_post_discount = (offerPost != null) ? offerPost.offer_post_discount : 0,
                                  priceNow = post.post_price - (post.post_price * ((offerPost != null) ? offerPost.offer_post_discount : 0) / 100),
                                  img = post.post_img,
                                  offer_name = (offer != null) ? offer.offer_name : null,
                                  offer_status = (offer != null && offer.offer_date_expiration > currentEpochTime) ? "active" : "expired",
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

        public async Task<List<PostWithOfferDTO>> SearchPost(PostSearchRequest postSearchRequest)
        {
            try
            {
                return await _postDao.SearchPost(postSearchRequest);
            }
            catch
            {

                throw;
            }
        }
    }
}
