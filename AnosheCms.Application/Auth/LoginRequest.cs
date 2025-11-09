// File: AnosheCms.Application/DTOs/Auth/LoginRequest.cs
using System.ComponentModel.DataAnnotations;

namespace AnosheCms.Application.DTOs.Auth
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}