using System.ComponentModel.DataAnnotations;

namespace ExtradosStore.Common.CustomRequest.OfferRequest
{
    public class CreateOfferRequest
    {
        [Required]
        public string offer_name { get; set; }
        //evitar que nos manden fechas en tiempo pasado (utc)//

        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format")]
        [FutureDate(ErrorMessage = "The offer start date must be in the future")]
        public DateTime offer_date_start { get; set; }
        //****************************************//

        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format")]
        [Minimum24HoursFromNow(ErrorMessage = "The date must be at least 24 hours from now.")]
        public DateTime offer_date_expiration { get; set; }
    }
}
