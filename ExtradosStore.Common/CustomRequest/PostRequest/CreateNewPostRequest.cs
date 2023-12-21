using ExtradosStore.Common.CustomValidationAtributes;
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
        [Range(1, byte.MaxValue, ErrorMessage = "Stock must be greater than 0.")]
        public byte post_stock { get; set; }
        [Base64Image(ErrorMessage = "The string is not a valid Base64 representation")]
        public string post_img { get; set; }
        [Required]
        public int brand_id { get; set; }
        [Required]
        public int category_id { get; set; }
        [Required]
        public int post_status_id { get; set; }
    }
}
