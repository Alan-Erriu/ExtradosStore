using ExtradosStore.Common.CustomExceptions.CarExceptions;
using ExtradosStore.Common.CustomExceptions.PostStatusExceptions;
using ExtradosStore.Common.CustomRequest.CarRequest;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.DTOs.CarDTO;
using ExtradosStore.Entities.DTOs.PostDTOs;
using ExtradosStore.Services.Interfaces;

namespace ExtradosStore.Services.Implementations
{
    public class CarService : ICarService
    {
        ICarDAO _carDAO;
        IPostDAO _postDAO;
        IPostStatusDAO _postStatusDAO;
        IOfferPostDAO _offerPostDAO;
        IOfferDAO _offerDAO;

        public CarService(ICarDAO carDAO, IPostDAO postDAO, IPostStatusDAO postStatusDAO, IOfferPostDAO offerPostDAO, IOfferDAO offerDAO)
        {
            _carDAO = carDAO;
            _postDAO = postDAO;
            _postStatusDAO = postStatusDAO;
            _offerPostDAO = offerPostDAO;
            _offerDAO = offerDAO;
        }

        public async Task<int> AddTocar(AddToCarRequest addToCarRequest, int userId)
        {
            try
            {
                var stockAndStatus = await _postDAO.DataGetStatusAndStockByPostId(addToCarRequest.post_id);
                if (stockAndStatus.post_userId == 0) throw new FileNotFoundException("post not found in data base");
                var statusActiveId = await _postStatusDAO.DataGetPostStatusIdByName("active");
                if (statusActiveId == 0) throw new PostStatusNotFoundException();
                if (stockAndStatus.post_status_id != statusActiveId) throw new StatusIsNotActiveException();
                if (stockAndStatus.post_stock < addToCarRequest.quantity) throw new StockIsLessThanQuantity();
                if (stockAndStatus.post_userId == userId) throw new InvalidOperationException("a user cannot buy your posts");
                return await _carDAO.DataAddtoCar(addToCarRequest, userId);
            }
            catch
            {

                throw;
            }
        }
        public async Task<List<CarDTO>> GetCarByUserId(int userId)
        {
            try
            {
                var listCarItemsFromDB = await _carDAO.DataGetCarByUserId(userId);


            }
            catch
            {

                throw;
            }
        }
        public async Task<List<CarItemDTO>> GetCarItemsWithDetails(List<CarDTO> listCarFromDB)
        {
            var listCarItems = new List<CarItemDTO>();
            var currentEpochTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            foreach (var item in listCarFromDB)
            {

                var offerId = await _offerPostDAO.DataGetOfferId(item.postId);
                var expirateDate = await _offerDAO.DataGetExpirationDateByOfferId(offerId);
                var discount = _offerPostDAO.DataGerDiscountByPostId(item.postId);
                var postDetails = await _postDAO.DataGetPostPriceNameAndImgById(item.postId);
                new CarItemDTO
                {
                    name = postDetails.post_name,
                    price = postDetails.post_price,
                    img = postDetails.post_img,
                    quantity = item.quantity
                };

            };
        }

    }
}
