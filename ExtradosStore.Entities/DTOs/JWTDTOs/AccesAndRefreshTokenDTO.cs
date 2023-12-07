using System.ComponentModel.DataAnnotations;

namespace ExtradosStore.Entities.DTOs.JWTDTOs
{
    public class AccesAndRefreshTokenDTO
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string refreshToken { get; set; }
    }
}
