using System.ComponentModel.DataAnnotations;

namespace ExtradosStore.Common.CustomRequest.OfferRequest
{
    public class CreateOfferRequest
    {
        [Required]
        public string offer_name { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format")]
        public DateTime offer_date_start { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format")]
        public DateTime offer_date_expiration { get; set; }
    }
}
