using ExtradosStore.Common.CustomRequest.OfferRequest;
using ExtradosStore.Entities.Models;

namespace ExtradosStore.Data.DAOs.Interfaces
{
    public interface IOfferPostDAO
    {
        Task<int> AddPostToOfferData(AddPostToOfferRequest addPostToOfferRequest);
        Task<int> DeleteOfferPostExpirated(int offerIdRequest);

        Task<List<OfferPost>> DataGetAllOfferPost();
    }
}