using System.ComponentModel.DataAnnotations;

namespace ExtradosStore.Entities.DTOs.RoleDTOs
{
    public class CreateRoleDTO
    {
        [Required(ErrorMessage = "Role name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Role name must be between 3 and 50 characters.")]
        public string name_role { get; set; }

        [Required(ErrorMessage = "Role description is required.")]
        [StringLength(150, MinimumLength = 10, ErrorMessage = "Role description must be between 10 and 200 characters.")]
        public string description_role { get; set; }

    }
}
