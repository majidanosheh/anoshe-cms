using AnosheCms.Application.DTOs.ContentEntry;
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/admin/content-items")] // (مسیر استاندارد شده)
    [Authorize]
    public class AdminContentEntryController : ControllerBase
    {
        private readonly IContentEntryService _entryService;

        public AdminContentEntryController(IContentEntryService entryService)
        {
            _entryService = entryService;
        }

        // GET: api/admin/content-items/{typeSlug}
        [HttpGet("{typeSlug}")]
        [Authorize(Policy = Permissions.ManageContentEntries)]
        public async Task<IActionResult> GetItems(string typeSlug, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            // (استفاده از متد جدید با صفحه‌بندی)
            var result = await _entryService.GetEntriesAsync(typeSlug, page, pageSize);
            return Ok(result);
        }

        // GET: api/admin/content-items/{typeSlug}/{id}
        [HttpGet("{typeSlug}/{id}")]
        [Authorize(Policy = Permissions.ManageContentEntries)]
        public async Task<IActionResult> GetItem(string typeSlug, Guid id)
        {
            var item = await _entryService.GetEntryByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/admin/content-items/{typeSlug}
        [HttpPost("{typeSlug}")]
        [Authorize(Policy = Permissions.ManageContentEntries)]
        public async Task<IActionResult> Create(string typeSlug, [FromBody] ContentEntryCreateDto request)
        {
            try
            {
                // (اصلاح شد: متد جدید CreateEntryAsync خروجی Guid می‌دهد، نه Tuple)
                var id = await _entryService.CreateEntryAsync(typeSlug, request);
                // (برگرداندن ID در پاسخ برای استفاده فرانت‌اند)
                return Ok(new { Id = id });
            }
            catch (Exception ex)
            {
                // (مدیریت خطا: اگر سرویس خطا داد، آن را به فرانت برمی‌گردانیم)
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/admin/content-items/{typeSlug}/{id}
        [HttpPut("{typeSlug}/{id}")]
        [Authorize(Policy = Permissions.ManageContentEntries)]
        public async Task<IActionResult> Update(string typeSlug, Guid id, [FromBody] ContentEntryCreateDto request)
        {
            try
            {
                // (اصلاح شد: متد جدید UpdateEntryAsync خروجی ندارد)
                await _entryService.UpdateEntryAsync(id, request);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/admin/content-items/{typeSlug}/{id}
        [HttpDelete("{typeSlug}/{id}")]
        [Authorize(Policy = Permissions.ManageContentEntries)]
        public async Task<IActionResult> Delete(string typeSlug, Guid id)
        {
            await _entryService.DeleteEntryAsync(id);
            return NoContent();
        }
    }
}