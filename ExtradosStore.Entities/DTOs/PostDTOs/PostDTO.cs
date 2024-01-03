namespace ExtradosStore.Entities.DTOs.PostDTOs
{
    public class PostDTO
    {
        public int post_id { get; set; }
        public int post_userId { get; set; }
        public string post_name { get; set; }
        public string post_description { get; set; }
        public decimal post_price { get; set; }
        public byte post_stock { get; set; }
        public string post_img { get; set; }
        public string brand_name { get; set; }
        public string category_name { get; set; }
        public string post_status_name { get; set; }
    }
}
