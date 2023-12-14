using System.ComponentModel.DataAnnotations;

namespace ExtradosStore.Common.CustomRequest.OfferRequest
{
    public class AddPostToOfferRequest
    {
        [Required]
        public int offer_post_postId { get; set; }
        [Required]
        public int offer_post_offerId { get; set; }
        [Required]
        public int offer_post_discount { get; set; }
    }
}
