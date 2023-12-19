namespace ExtradosStore.Entities.DTOs.SalesDTO
{
    public class SalesDetailDTO
    {
        public int post_id { get; set; }
        public int sales_id { get; set; }
        public int quantity { get; set; }
        public decimal subtotal { get; set; }
    }
}
