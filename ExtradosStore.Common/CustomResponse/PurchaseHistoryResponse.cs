namespace ExtradosStore.Common.CustomResponse
{
    public class PurchaseHistoryResponse
    {
        public int post_id { get; set; }
        public int quantity { get; set; }
        public decimal total { get; set; }

        public long BuyDate { get; set; }
    }
}
