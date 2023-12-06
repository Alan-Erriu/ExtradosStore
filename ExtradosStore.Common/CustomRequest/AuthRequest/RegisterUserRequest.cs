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
        // Para TelefonoCelular validamos que sea obligatorio, sea solo caracteres del tipo numero, y que el largo del string esté entre 10 y 14 caracteres (+xx xx-xxxx-xxxx)
        [MinLength(10, ErrorMessage = "The mobile phone must have at least 10 digits")]
        [MaxLength(14, ErrorMessage = "The phone number cannot have more than 14 digits")]
        [RegularExpression(@"^[0-9+]+$", ErrorMessage = "The Mobile Phone field should only have numeric values")]
        public string user_phone_number { get; set; }
        [Required]
        public string user_password_hash { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format")]
        public DateTime user_date_of_birth { get; set; }
    }
}
