using ExtradosStore.Common.CustomRequest.CarRequest;
using ExtradosStore.Entities.DTOs.PostDTOs;

namespace ExtradosStore.Services.Interfaces
{
    public interface ICarService
    {
        Task<int> AddTocar(AddToCarRequest addToCarRequest, int userId);
        Task<List<CarDTO>> GetCarByUserId(int userId);
    }
}