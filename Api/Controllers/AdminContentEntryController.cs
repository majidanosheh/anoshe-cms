// File: Api/Controllers/AdminContentEntryController.cs
using AnosheCms.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/admin/content-entries/{apiSlug}")]
    [Authorize]
    public class AdminContentEntryController : ControllerBase
    {
        private readonly IContentEntryService _itemService;
        public AdminContentEntryController(IContentEntryService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetItems(string apiSlug)
        {
            var result = await _itemService.GetContentEntriesAsync(apiSlug);
            return Ok(result);
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetItemById(string apiSlug, Guid itemId)
        {
            var result = await _itemService.GetContentEntryByIdAsync(apiSlug, itemId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(string apiSlug, [FromBody] ContentEntryCreateDto dto)
        {
            var (result, error) = await _itemService.CreateContentEntryAsync(apiSlug, dto);
            if (error != null) return BadRequest(new { message = error });
            return CreatedAtAction(nameof(GetItemById), new { apiSlug = apiSlug, itemId = result.Id }, result);
        }

        [HttpPut("{itemId}")]
        public async Task<IActionResult> UpdateItem(string apiSlug, Guid itemId, [FromBody] ContentEntryCreateDto dto)
        {
            var (result, error) = await _itemService.UpdateContentEntryAsync(apiSlug, itemId, dto);
            if (error != null) return BadRequest(new { message = error });
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteItem(string apiSlug, Guid itemId)
        {
            var success = await _itemService.DeleteContentEntryAsync(apiSlug, itemId);
            if (!success) return NotFound();
            return Ok(new { message = "آیتم با موفقیت حذف شد." });
        }
    }
}