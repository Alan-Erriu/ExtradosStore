using System.ComponentModel.DataAnnotations;

namespace ExtradosStore.Common.CustomRequest.PostRequest
{
    public class CreateNewPostRequest
    {
        [Required]
        public int post_userId { get; set; }
        [Required]
        public string post_name { get; set; }
        [Required]
        public string post_description { get; set; }
        [Required]
        public decimal post_price { get; set; }
        [Required]
        public byte post_stock { get; set; }
        //[Required]
        public byte[] post_img { get; set; }
        [Required]
        public int brand_id { get; set; }
        [Required]
        public int category_id { get; set; }
        [Required]
        public bool post_status { get; set; }
    }
}
