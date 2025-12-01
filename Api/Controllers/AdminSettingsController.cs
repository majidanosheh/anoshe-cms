using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/admin/settings")]
    [Authorize(Policy = Permissions.ManageSettings)]
    public class AdminSettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;

        public AdminSettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet]
        [Authorize(Policy = Permissions.ManageSettings)]
        public async Task<IActionResult> GetSettings()
        {
            // (متد جدید: GetAllSettingsAsync)
            var settings = await _settingsService.GetAllSettingsAsync();
            return Ok(settings);
        }
        [HttpPost]
        [Authorize(Policy = Permissions.ManageSettings)]
        public async Task<IActionResult> UpdateSettings([FromBody] Dictionary<string, string> settings)
        {
            // (متد جدید: UpdateSettingsAsync با یک پارامتر)
            await _settingsService.UpdateSettingsAsync(settings);
            return Ok(new { Message = "تنظیمات با موفقیت ذخیره شد" });
        }
    }
}