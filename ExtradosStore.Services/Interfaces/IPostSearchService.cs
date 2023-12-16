using ExtradosStore.Entities.DTOs.PostDTOs;

namespace ExtradosStore.Services.Interfaces
{
    public interface IPostSearchService
    {
        Task<List<PostWithOfferDTO>> GetAllPostWithOfferService();

        Task<List<PostWithOfferDTO>> GetAllPostActiveWithOfferService();

        Task<List<PostWithOfferDTO>> GetAllPostActiveByOfferId(int offerId);
        Task<List<PostWithOfferDTO>> GetAllActivePostsWithNoOfferOrExpiredOffer();
    }
}