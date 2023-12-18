using ExtradosStore.Common.CustomRequest.CarRequest;
using ExtradosStore.Entities.DTOs.PostDTOs;

namespace ExtradosStore.Data.DAOs.Interfaces
{
    public interface ICarDAO
    {
        Task<int> DataAddtoCar(AddToCarRequest addToCarRequest, int userId);
        Task<List<CarDTO>> DataGetCarByUserId(int userId);
        Task<int> DataUpdateQuantity(int quantity);
    }
}