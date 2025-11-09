// File: AnosheCms.Application/DTOs/Auth/TokenRequest.cs
using System.ComponentModel.DataAnnotations;

namespace AnosheCms.Application.DTOs.Auth
{
    // (بر اساس 177.txt، برای Refresh Token)
    public class TokenRequest
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}