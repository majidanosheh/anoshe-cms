// File: Api/Controllers/AdminContentItemController.cs
using AnosheCms.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/admin/content-items/{apiSlug}")]
    [Authorize]
    public class AdminContentItemController : ControllerBase
    {
        private readonly IContentItemService _itemService;
        public AdminContentItemController(IContentItemService itemService)
        {
            _itemService = itemService;
        }
        [HttpGet]
        public async Task<IActionResult> GetItems(string apiSlug)
        {
            var result = await _itemService.GetContentItemsAsync(apiSlug);
            return Ok(result);
        }
        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetItemById(string apiSlug, Guid itemId)
        {
            var result = await _itemService.GetContentItemByIdAsync(apiSlug, itemId);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateItem(string apiSlug, [FromBody] ContentItemCreateDto dto)
        {
            var (result, error) = await _itemService.CreateContentItemAsync(apiSlug, dto);
            if (error != null) return BadRequest(new { message = error });
            return CreatedAtAction(nameof(GetItemById), new { apiSlug = apiSlug, itemId = result.Id }, result);
        }
        [HttpPut("{itemId}")]
        public async Task<IActionResult> UpdateItem(string apiSlug, Guid itemId, [FromBody] ContentItemCreateDto dto)
        {
            var (result, error) = await _itemService.UpdateContentItemAsync(apiSlug, itemId, dto);
            if (error != null) return BadRequest(new { message = error });
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteItem(string apiSlug, Guid itemId)
        {
            var success = await _itemService.DeleteContentItemAsync(apiSlug, itemId);
            if (!success) return NotFound();
            return Ok(new { message = "آیتم با موفقیت حذف شد." });
        }
    }
}