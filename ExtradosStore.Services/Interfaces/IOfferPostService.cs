using ExtradosStore.Common.CustomRequest.OfferRequest;

namespace ExtradosStore.Services.Interfaces
{
    public interface IOfferPostService
    {
        Task<int> AddPostToOfferService(AddPostToOfferRequest addPostToOfferRequest, int userIdFromToken);

        Task<int> DeleteOfferPostByPostId(int postId, int userIdFromToken, string nameRole);
        Task<int> DeleteAllOfferPostByOfferId(int offerId);


    }
}