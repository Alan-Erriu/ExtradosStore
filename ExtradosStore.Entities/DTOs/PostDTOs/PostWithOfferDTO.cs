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
        public decimal priceNow
        {
            get
            {
                if (!offer_status) return post_price;
                return post_price - (post_price * offer_post_discount / 100);
            }
        }
        public string post_img { get; set; }
        public string offer_name { get; set; }
        public long offer_date_expiration { get; set; }
        public bool offer_status
        {
            get
            {

                return offer_date_expiration > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() ? true : false;
            }
        }
        public string category_name { get; set; }
        public string brand_name { get; set; }


    }
}
