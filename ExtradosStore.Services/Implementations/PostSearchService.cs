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




        public async Task<List<PostWithOfferDTO>> GetAllPostWithOfferService()
        {
            try
            {
                var listPostFromDB = await _postDao.GetAllPostActiveWithOffer();

                return listPostFromDB;

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
