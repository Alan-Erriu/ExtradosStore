using ExtradosStore.Common.CustomRequest.OfferRequest;

namespace ExtradosStore.Data.DAOs.Implementations
{
    public interface IOfferDAO
    {
        Task<int> DataCreateOffer(CreateOfferRequest offerRequest);
    }
}