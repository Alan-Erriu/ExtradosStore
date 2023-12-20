using ExtradosStore.Common.CustomExceptions.CarExceptions;
using ExtradosStore.Common.CustomExceptions.PostExceptions;
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
        ISalesDAO _salesDAO;
        ISalesDetailDAO _salesDetailDAO;

        public CarService(ICarDAO carDAO, IPostDAO postDAO, IPostStatusDAO postStatusDAO, IOfferPostDAO offerPostDAO,
            IOfferDAO offerDAO
            , ISalesDAO salesDAO, ISalesDetailDAO salesDetailDAO)
        {
            _carDAO = carDAO;
            _postDAO = postDAO;
            _postStatusDAO = postStatusDAO;
            _offerPostDAO = offerPostDAO;
            _offerDAO = offerDAO;
            _salesDAO = salesDAO;
            _salesDetailDAO = salesDetailDAO;
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
                var listCarItemsFromBack = await _carDAO.DataGetCarByUserId(userId);


                var existingCarItem = listCarItemsFromBack.FirstOrDefault(item => item.post_id == addToCarRequest.post_id);

                if (existingCarItem == null)
                {

                    return await _carDAO.DataAddtoCar(addToCarRequest, userId);
                }
                existingCarItem.quantity += addToCarRequest.quantity;

                await _carDAO.DataUpdateQuantity(existingCarItem.quantity, userId);


                return existingCarItem.quantity;
            }
            catch
            {

                throw;
            }
        }
        public async Task<int> RemoveOneQuantityOrDeleteItemCar(int postId, int userId)
        {
            try
            {

                var quantityFromDB = await _carDAO.DataGetQuantityByPostAndUserId(postId, userId);
                if (quantityFromDB == 0) throw new PostNotFoundException();
                if (quantityFromDB == 1) return await _carDAO.DataDeleteCarItem(postId, userId);

                quantityFromDB--;

                return await _carDAO.DataUpdateQuantity(quantityFromDB, userId);
            }
            catch
            {

                throw;
            }
        }
        public async Task<List<CarItemDTO>> GetCarByUserId(int userId)
        {
            try
            {
                var listCarItemsFromDB = await _carDAO.DataGetCarByUserId(userId);

                var responseCarsitems = await GetCarItemsWithDetails(listCarItemsFromDB);

                return responseCarsitems;
            }
            catch
            {

                throw;
            }
        }
        public async Task<List<CarItemDTO>> GetCarItemsWithDetails(List<CarDTO> listCarFromDB)
        {
            try
            {

                var listCarItems = new List<CarItemDTO>();
                var currentEpochTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                foreach (var item in listCarFromDB)
                {
                    var offerId = await _offerPostDAO.DataGetOfferId(item.post_id);
                    var expirationDate = await _offerDAO.DataGetExpirationDateByOfferId(offerId);
                    var discount = await _offerPostDAO.DataGerDiscountByPostId(item.post_id);
                    var postDetails = await _postDAO.DataGetPostPriceNameAndImgById(item.post_id);
                    var discountedPrice = (offerId != 0 && expirationDate > currentEpochTime)
                                            ? postDetails.post_price - (postDetails.post_price * discount / 100)
                                            : postDetails.post_price;

                    listCarItems.Add(new CarItemDTO
                    {
                        post_id = item.post_id,
                        name = postDetails.post_name,
                        price = discountedPrice,
                        img = postDetails.post_img,
                        quantity = item.quantity
                    });
                }

                return listCarItems;
            }
            catch
            {

                throw;
            }
        }
        public async Task<int> BuyCar(int userId)
        {
            try
            {

                var carItemsFromDB = await _carDAO.DataGetCarByUserId(userId);

                if (carItemsFromDB == null || carItemsFromDB.Count == 0)
                {

                    return 0;
                }
                var carItems = await GetCarItemsWithDetails(carItemsFromDB);

                decimal totalCompra = carItems.Sum(item => item.price * item.quantity);

                int salesId = await _salesDAO.DataCreateNewSales(userId, totalCompra);

                foreach (var carItem in carItems)
                {

                    var offerId = await _offerPostDAO.DataGetOfferId(carItem.post_id);
                    var expirationDate = await _offerDAO.DataGetExpirationDateByOfferId(offerId);
                    var discount = await _offerPostDAO.DataGerDiscountByPostId(carItem.post_id);



                    await _salesDetailDAO.DataCreateNewSalesDetail(
                        salesId,
                        carItem.post_id,
                        carItem.quantity,
                        carItem.price,
                        discount = (offerId != 0 && expirationDate > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()) ? discount : 0
                    );
                }


                await _carDAO.DataDeleteAllItemsFromCar(userId);

                return salesId;
            }
            catch
            {

                throw;
            }
        }


    }
}
