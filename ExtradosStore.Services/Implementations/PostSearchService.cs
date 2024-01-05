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




        public async Task<List<PostWithOfferDTO>> GetAllPostService()
        {

            var statusActiveId = await _postStatusDAO.DataGetPostStatusIdByName("active");
            var listPostFromDB = await _postDao.GetAllPostActive(statusActiveId);
            return listPostFromDB;


        }
        public async Task<List<PostWithOfferDTO>> GetAllPostWithOffer()
        {

            var currentTimeEpoch = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var statusActiveId = await _postStatusDAO.DataGetPostStatusIdByName("active");
            var listPostFromDB = await _postDao.GetAllPostActiveWithOffer(statusActiveId, currentTimeEpoch);
            return listPostFromDB;

        }


        public async Task<List<PostWithOfferDTO>> SearchPost(PostSearchRequest postSearchRequest)
        {

            var statusActiveId = await _postStatusDAO.DataGetPostStatusIdByName("active");
            return await _postDao.SearchPostActive(postSearchRequest, statusActiveId);


        }

        public async Task<PostWithOfferDTO> GetPostByPostId(int postId)
        {

            var statusActiveId = await _postStatusDAO.DataGetPostStatusIdByName("active");
            return await _postDao.DataGetPostByPostId(postId, statusActiveId);

        }

        public async Task<List<PostDTO>> GetAllPostByUserId(int userId)
        {


            return await _postDao.DataAllPostByUserId(userId);

        }
    }
}
