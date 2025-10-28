// File: AnosheCms.Api/Controllers/AuthController.cs

// --- شروع Using Directives ---
using AnosheCms.Application.Interfaces; // <-- حیاتی: برای IAuthService و DTOs
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks; // برای Task
// --- پایان Using Directives ---

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            var result = await _authService.LoginAsync(request);
            if (!result.Succeeded)
            {
                return Unauthorized(new { message = result.ErrorMessage });
            }
            return Ok(new { token = result.Token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }
            return Ok(new { message = "User registered successfully. Please login." });
        }
    }
}