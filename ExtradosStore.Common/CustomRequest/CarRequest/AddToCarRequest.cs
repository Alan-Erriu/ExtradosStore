using System.ComponentModel.DataAnnotations;

namespace ExtradosStore.Common.CustomRequest.CarRequest
{
    public class AddToCarRequest
    {
        [Required]
        public int post_id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The quantity must be greater than 0")]
        public int quantity { get; set; }
    }
}
