using ExtradosStore.Common.CustomRequest.OfferRequest;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Services.Interfaces;

namespace ExtradosStore.Services.Implementations
{
    public class OfferPostService : IOfferPostService
    {
        IOfferPostDAO _offerPostDAO;
        IOfferDAO _offerDAO;
        IPostDAO _postDAO;
        IPostStatusDAO _postStatusDAO;
        public OfferPostService(IOfferPostDAO offerPostDAO, IOfferDAO offerDAO, IPostDAO postDAO, IPostStatusDAO postStatusDAO)
        {
            _offerPostDAO = offerPostDAO;
            _offerDAO = offerDAO;
            _postDAO = postDAO;
            _postStatusDAO = postStatusDAO;


        }

        public async Task<int> AddPostToOfferService(AddPostToOfferRequest addPostToOfferRequest)
        {
            try
            {
                var postStatusIdFromBack = await _postDAO.DataGetStatusIdByPostId(addPostToOfferRequest.offer_post_postId);
                var statusActiveId = await _postStatusDAO.DataGetPostStatusIdByName("active");
                if (postStatusIdFromBack != statusActiveId) throw new InvalidOperationException("Only active posts can be added to offers");
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

                var listOfferExpirated = await _offerDAO.GetOfferExpiratedData();
                int rowsAffected = 0;
                foreach (var item in listOfferExpirated)
                {
                    rowsAffected = await _offerPostDAO.DeleteOfferPostExpirated(item);

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
