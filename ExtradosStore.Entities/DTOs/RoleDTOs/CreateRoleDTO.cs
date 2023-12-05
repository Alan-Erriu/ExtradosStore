using System.ComponentModel.DataAnnotations;

namespace ExtradosStore.Entities.DTOs.RoleDTOs
{
    public class CreateRoleDTO
    {
        [Required(ErrorMessage = "Role name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Role name must be between 3 and 50 characters.")]
        public string role_name { get; set; }

        [Required(ErrorMessage = "Role description is required.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Role description must be between 10 and 200 characters.")]
        public string role_description { get; set; }

    }
}
