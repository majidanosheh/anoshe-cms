// File: Api/Controllers/AdminSettingsController.cs
using AnosheCms.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AnosheCms.Domain.Constants;
using System.Collections.Generic;

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/admin/settings")]
    [Authorize]
    public class AdminSettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ICurrentUserService _currentUserService;
        private const string SettingsSlug = "global-settings"; // اسلاگ هارد-کد شده

        public AdminSettingsController(ISettingsService settingsService, ICurrentUserService currentUserService)
        {
            _settingsService = settingsService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        [Authorize(Policy = Permissions.ViewSettings)]
        public async Task<IActionResult> GetSettings()
        {
            var (data, error) = await _settingsService.GetSettingsAsync(SettingsSlug);
            if (error != null)
            {
                return BadRequest(new { message = error });
            }
            return Ok(data);
        }

        [HttpPut]
        [Authorize(Policy = Permissions.EditSettings)]
        public async Task<IActionResult> UpdateSettings([FromBody] Dictionary<string, object> data)
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
            {
                return Unauthorized();
            }

            var (updatedData, error) = await _settingsService.UpdateSettingsAsync(SettingsSlug, data, userId.Value);

            if (error != null)
            {
                return BadRequest(new { message = error });
            }

            return Ok(updatedData);
        }
    }
}