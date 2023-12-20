using ExtradosStore.Common.CustomRequest.OfferRequest;

namespace ExtradosStore.Services.Interfaces
{
    public interface IOfferService
    {
        Task<int> CreateOfferService(CreateOfferRequest offerRequest, int userId);
    }
}