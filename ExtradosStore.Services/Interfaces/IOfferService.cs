using ExtradosStore.Common.CustomRequest.OfferRequest;
using ExtradosStore.Entities.Models;

namespace ExtradosStore.Services.Interfaces
{
    public interface IOfferService
    {
        Task<int> CreateOfferService(CreateOfferRequest offerRequest, int userId);
        Task<List<Offer>> GetAllOffer();
        Task<List<Offer>> GetAllOfferActive();
    }
}