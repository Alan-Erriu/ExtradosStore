using System.ComponentModel.DataAnnotations;

namespace ExtradosStore.Common.CustomRequest.BrandRequest
{
    public class CreateBrandRequest
    {

        [StringLength(50, MinimumLength = 3, ErrorMessage = "The name  must be at least 6 characters long")]
        public string brand_name { get; set; }
    }
}
