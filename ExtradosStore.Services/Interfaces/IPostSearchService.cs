using ExtradosStore.Entities.DTOs.PostDTOs;

namespace ExtradosStore.Services.Interfaces
{
    public interface IPostSearchService
    {
        Task<List<PostWithOfferDTO>> GetAllPostWithOfferService();

        Task<List<PostWithOfferDTO>> GetAllPostActiveWithOfferService();
    }
}