namespace ExtradosStore.Entities.DTOs.CarDTO
{
    public class CarItemDTO
    {
        public int post_id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
        public byte[] img { get; set; }

    }
}
