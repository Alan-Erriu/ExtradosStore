using ExtradosStore.Common.CustomRequest.OfferRequest;
using ExtradosStore.Entities.Models;

namespace ExtradosStore.Data.DAOs.Interfaces
{
    public interface IOfferPostDAO
    {
        Task<int> AddPostToOfferData(AddPostToOfferRequest addPostToOfferRequest);

        Task<int> DataGetOfferId(int postId);
        Task<List<OfferPost>> DataGetAllOfferPost();
        Task<int> DeleteOfferPostByPostId(int postId);
        Task<int> DeleteAllOfferPostByOfferId(int offerId);
    }
}