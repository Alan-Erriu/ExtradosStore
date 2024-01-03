using ExtradosStore.Common.CustomRequest.PostSearchRequest;
using ExtradosStore.Entities.DTOs.PostDTOs;

namespace ExtradosStore.Services.Interfaces
{
    public interface IPostSearchService
    {
        Task<List<PostWithOfferDTO>> GetAllPostService();
        Task<List<PostWithOfferDTO>> SearchPost(PostSearchRequest postSearchRequest);
        Task<PostWithOfferDTO> GetPostByPostId(int postId);
        Task<List<PostDTO>> GetAllPostByUserId(int userId);

        Task<List<PostWithOfferDTO>> GetAllPostWithOffer();
    }
}