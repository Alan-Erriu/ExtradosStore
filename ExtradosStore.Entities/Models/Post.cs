namespace ExtradosStore.Entities.Models
{
    public class Post
    {
        public int post_id { get; set; }
        public int post_userId { get; set; }
        public string post_name { get; set; }
        public string post_description { get; set; }
        public decimal post_price { get; set; }
        public byte post_stock { get; set; }
        public byte[] post_img { get; set; }
        public int post_brandId { get; set; }
        public int post_categoryId { get; set; }
        public long post_create_at { get; set; }
        public int post_status_id { get; set; }

    }
}
