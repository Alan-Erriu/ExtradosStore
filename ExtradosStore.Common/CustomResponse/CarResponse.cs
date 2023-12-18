using ExtradosStore.Entities.DTOs.CarDTO;

namespace ExtradosStore.Common.CustomResponse
{
    public class CarResponse
    {
        public List<CarItemDTO> post { get; set; }

        public decimal total { get; set; }
    }
}
