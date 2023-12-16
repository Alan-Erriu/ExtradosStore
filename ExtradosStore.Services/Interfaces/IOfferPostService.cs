using ExtradosStore.Common.CustomRequest.OfferRequest;

namespace ExtradosStore.Services.Interfaces
{
    public interface IOfferPostService
    {
        Task<int> AddPostToOfferService(AddPostToOfferRequest addPostToOfferRequest, int userIdFromToken, string nameRole);

        Task<int> DeleteOfferPostByPostId(int postId, int userIdFromToken, string nameRole);


    }
}