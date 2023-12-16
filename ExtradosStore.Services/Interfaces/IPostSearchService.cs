using ExtradosStore.Common.CustomRequest.PostSearchRequest;
using ExtradosStore.Entities.DTOs.PostDTOs;
using ExtradosStore.Entities.Models;

namespace ExtradosStore.Services.Interfaces
{
    public interface IPostSearchService
    {
        Task<List<PostWithOfferDTO>> GetAllPostWithOfferService();
        Task<List<PostWithOfferDTO>> GetAllPostActiveWithOfferService();
        Task<List<PostWithOfferDTO>> GetAllPostActiveByOfferId(int offerId);
        Task<List<PostWithOfferDTO>> GetAllActivePostsWithNoOfferOrExpiredOffer();
        Task<List<Post>> SearchPost(PostSearchRequest postSearchRequest);
    }
}