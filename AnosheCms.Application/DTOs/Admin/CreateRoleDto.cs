using System.ComponentModel.DataAnnotations;

namespace AnosheCms.Application.DTOs.Admin
{
    public class CreateRoleDto
    {
        [Required(ErrorMessage = "نام نقش الزامی است")]
        public string Name { get; set; }

        public string? DisplayName { get; set; }
        public string? Description { get; set; }
    }
}