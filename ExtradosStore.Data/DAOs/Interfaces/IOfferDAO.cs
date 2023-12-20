using ExtradosStore.Common.CustomRequest.OfferRequest;
using ExtradosStore.Entities.Models;

namespace ExtradosStore.Data.DAOs.Interfaces
{
    public interface IOfferDAO
    {
        Task<int> DataCreateOffer(CreateOfferRequest offerRequest, int userId);

        Task<List<int>> GetOfferExpiratedData();
        Task<List<Offer>> GetAllOffer();
        Task<long> DataGetExpirationDateByOfferId(int offerID);
    }
}