using ExtradosStore.Common.CustomRequest.CarRequest;
using ExtradosStore.Entities.DTOs.PostDTOs;

namespace ExtradosStore.Data.DAOs.Interfaces
{
    public interface ICarDAO
    {
        Task<int> DataAddtoCar(AddToCarRequest addToCarRequest, int userId);
        Task<List<CarDTO>> DataGetCarByUserId(int userId);
        Task<int> DataUpdateQuantity(int quantity, int userId);
        Task<int> DataGetQuantityByPostAndUserId(int postId, int userId);
        Task<int> DataDeleteCarItem(int postId, int userId);
    }
}