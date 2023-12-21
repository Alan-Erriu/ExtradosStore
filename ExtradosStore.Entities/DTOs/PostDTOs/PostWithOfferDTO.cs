namespace ExtradosStore.Entities.DTOs.PostDTOs
{
    public class PostWithOfferDTO
    {
        public int post_id { get; set; }
        public string post_name { get; set; }
        public string user_name { get; set; }
        public string post_description { get; set; }
        public decimal post_price { get; set; }
        public int offer_post_discount { get; set; }
        public decimal priceNow { get; set; }
        public string img { get; set; }
        public string offer_name { get; set; }
        public string offer_status { get; set; }
        public string category_name { get; set; }
        public string brand_name { get; set; }


    }
}
