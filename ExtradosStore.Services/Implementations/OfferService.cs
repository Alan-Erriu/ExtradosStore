using ExtradosStore.Common.CustomRequest.OfferRequest;
using ExtradosStore.Data.DAOs.Implementations;
using ExtradosStore.Services.Interfaces;

namespace ExtradosStore.Services.Implementations
{
    public class OfferService : IOfferService
    {
        private readonly IOfferDAO _offerDAO;


        public OfferService(IOfferDAO offerDAO)
        {
            _offerDAO = offerDAO;

        }

        public async Task<int> CreateOfferService(CreateOfferRequest offerRequest)
        {
            try
            {
                var rowsAffected = await _offerDAO.DataCreateOffer(offerRequest);
                return rowsAffected;
            }
            catch
            {

                throw;
            }
        }
    }
}
