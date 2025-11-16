using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnosheCms.Application.DTOs.Admin // (فضای نام صحیح)
{
    public class UpdateUserRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }
        public List<string> Roles { get; set; }
    }
}