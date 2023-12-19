using ExtradosStore.Common.CustomRequest.CarRequest;
using ExtradosStore.Entities.DTOs.CarDTO;

namespace ExtradosStore.Services.Interfaces
{
    public interface ICarService
    {
        Task<int> AddTocar(AddToCarRequest addToCarRequest, int userId);
        Task<List<CarItemDTO>> GetCarByUserId(int userId);
        Task<int> RemoveOneQuantityOrDeleteItemCar(int postId, int userId);
    }
}