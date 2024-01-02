using ExtradosStore.Common.CustomRequest.PostSearchRequest;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.DTOs.PostDTOs;
using ExtradosStore.Services.Interfaces;

namespace ExtradosStore.Services.Implementations
{
    public class PostSearchService : IPostSearchService
    {

        private readonly IPostDAO _postDao;

        private readonly IPostStatusDAO _postStatusDAO;



        public PostSearchService(IPostDAO postDAO, IPostStatusDAO postStatusDAO)
        {
            _postDao = postDAO;

            _postStatusDAO = postStatusDAO;

        }




        public async Task<List<PostWithOfferDTO>> GetAllPostWithOfferService()
        {
            try
            {
                var statusActiveId = await _postStatusDAO.DataGetPostStatusIdByName("active");
                var listPostFromDB = await _postDao.GetAllPostActiveWithOffer(statusActiveId);
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
                var statusActiveId = await _postStatusDAO.DataGetPostStatusIdByName("active");
                return await _postDao.SearchPostActive(postSearchRequest, statusActiveId);
            }
            catch
            {

                throw;
            }
        }
    }
}
