using ExtradosStore.Common.CustomRequest.OfferRequest;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.Models;
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

        public async Task<int> CreateOfferService(CreateOfferRequest offerRequest, int userId)
        {

            var rowsAffected = await _offerDAO.DataCreateOffer(offerRequest, userId);
            return rowsAffected;

        }

        public async Task<List<Offer>> GetAllOfferActive()
        {

            var rowsAffected = await _offerDAO.GetAllOfferActive();
            return rowsAffected;

        }

        public async Task<List<Offer>> GetAllOffer()
        {

            var rowsAffected = await _offerDAO.GetAllOffer();
            return rowsAffected;

        }
    }
}
