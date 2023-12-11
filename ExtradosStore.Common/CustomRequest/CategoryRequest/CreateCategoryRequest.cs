using System.ComponentModel.DataAnnotations;

namespace ExtradosStore.Common.CustomRequest.CategoryRequest
{
    public class CreateCategoryRequest
    {
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The name  must be at least 6 characters long")]
        public string category_name { get; set; }
    }
}
