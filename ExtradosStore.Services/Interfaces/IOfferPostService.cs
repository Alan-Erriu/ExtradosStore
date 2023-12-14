using ExtradosStore.Common.CustomRequest.OfferRequest;

namespace ExtradosStore.Services.Interfaces
{
    public interface IOfferPostService
    {
        Task<int> AddPostToOfferService(AddPostToOfferRequest addPostToOfferRequest);

        Task<int> DeleteAllOfferPostExpiratedService();
    }
}