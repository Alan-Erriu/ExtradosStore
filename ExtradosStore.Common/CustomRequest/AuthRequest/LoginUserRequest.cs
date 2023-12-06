using System.ComponentModel.DataAnnotations;

namespace ExtradosStore.Common.CustomRequest.AuthRequest
{
    public class LoginUserRequest
    {
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string user_email { get; set; }
        [Required]
        public string user_password { get; set; }
    }
}
