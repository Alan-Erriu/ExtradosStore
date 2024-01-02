using ExtradosStore.Common.CustomRequest.PostSearchRequest;
using ExtradosStore.Entities.DTOs.PostDTOs;

namespace ExtradosStore.Services.Interfaces
{
    public interface IPostSearchService
    {
        Task<List<PostWithOfferDTO>> GetAllPostWithOfferService();
        Task<List<PostWithOfferDTO>> SearchPost(PostSearchRequest postSearchRequest);
    }
}