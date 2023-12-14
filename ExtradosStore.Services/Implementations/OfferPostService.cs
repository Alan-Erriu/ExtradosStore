using ExtradosStore.Common.CustomRequest.OfferRequest;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Services.Interfaces;

namespace ExtradosStore.Services.Implementations
{
    public class OfferPostService : IOfferPostService
    {
        IOfferPostDAO _offerPostDAO;
        IOfferDAO _offerDAO;
        public OfferPostService(IOfferPostDAO offerPostDAO, IOfferDAO offerDAO)
        {
            _offerPostDAO = offerPostDAO;
            _offerDAO = offerDAO;
        }

        public async Task<int> AddPostToOfferService(AddPostToOfferRequest addPostToOfferRequest)
        {
            try
            {

                var rowsAffected = await _offerPostDAO.AddPostToOfferData(addPostToOfferRequest);
                return rowsAffected;
            }
            catch
            {

                throw;
            }
        }

        public async Task<int> DeleteAllOfferPostExpiratedService()
        {
            try
            {
                Console.WriteLine("servicio background");
                var listOfferExpirated = await _offerDAO.GetOfferExpiratedData();
                int rowsAffected = 0;
                foreach (var item in listOfferExpirated)
                {
                    rowsAffected = await _offerPostDAO.DeleteOfferPostExpirated(item);
                    Console.WriteLine("servicio background");
                }
                return rowsAffected;

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
