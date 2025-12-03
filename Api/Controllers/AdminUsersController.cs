using AnosheCms.Application.DTOs.Admin;
using AnosheCms.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using AnosheCms.Domain.Constants; 

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    
    [Authorize]
    public class AdminUsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;

        public AdminUsersController(IUserService userService, ICurrentUserService currentUserService)
        {
            _userService = userService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        [Authorize(Policy = Permissions.ViewUsers)] 
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = Permissions.ViewUsers)] 
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.CreateUsers)] 
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var (user, errors) = await _userService.CreateUserAsync(request, _currentUserService.UserId.Value);
            if (errors != null && errors.Any())
            {
                return BadRequest(new { Errors = errors });
            }
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = Permissions.EditUsers)] 
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
        {
            var (user, errors) = await _userService.UpdateUserAsync(id, request, _currentUserService.UserId.Value);
            if (errors != null && errors.Any())
            {
                return BadRequest(new { Errors = errors });
            }
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.DeleteUsers)] 
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var adminUserSeedId = Guid.Parse("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1");
            if (id == adminUserSeedId)
            {
                return BadRequest(new { Errors = new[] { "امکان حذف کاربر ادمین اصلی سیستم وجود ندارد." } });
            }

            var success = await _userService.DeleteUserAsync(id, _currentUserService.UserId.Value);
            if (!success) return NotFound();
            return Ok(new { Message = "کاربر با موفقیت حذف (غیرفعال) شد." });
        }
    }
}