using System.ComponentModel.DataAnnotations;

namespace ExtradosStore.Common.CustomRequest.AuthRequest
{
    public class RegisterUserRequest
    {
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The name  must be at least 6 characters long")]
        public string user_name { get; set; }
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The lastname  must be at least 6 characters long")]
        public string user_lastname { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string user_email { get; set; }
        [Required]
        public string user_password_hash { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format")]
        public DateTime user_date_of_birth { get; set; }
    }
}
