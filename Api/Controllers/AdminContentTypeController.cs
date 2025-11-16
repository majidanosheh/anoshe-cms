using AnosheCms.Application.DTOs.ContentType;
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/content-type-admin")] // (مسیر صحیح بر اساس Store فرانت‌اند)
    [Authorize]
    public class AdminContentTypeController : ControllerBase // (تغییر نام برای مطابقت با فایل شما)
    {
        private readonly IContentTypeService _contentTypeService;

        public AdminContentTypeController(IContentTypeService contentTypeService)
        {
            _contentTypeService = contentTypeService;
        }

        // GET: api/content-type-admin
        [HttpGet]
        [Authorize(Policy = Permissions.ViewContentTypes)]
        public async Task<IActionResult> GetAllContentTypes()
        {
            var result = await _contentTypeService.GetAllContentTypesAsync();
            return Ok(result);
        }

        // GET: api/content-type-admin/{apiSlug}
        [HttpGet("{apiSlug}")]
        [Authorize(Policy = Permissions.ViewContentTypes)]
        public async Task<IActionResult> GetContentType(string apiSlug)
        {
            var result = await _contentTypeService.GetContentTypeBySlugAsync(apiSlug);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // POST: api/content-type-admin
        [HttpPost]
        [Authorize(Policy = Permissions.CreateContentTypes)]
        public async Task<IActionResult> CreateContentType([FromBody] CreateContentTypeDto dto)
        {
            var result = await _contentTypeService.CreateContentTypeAsync(dto);
            if (result == null)
            {
                return BadRequest("Failed to create content type. ApiSlug might already exist.");
            }
            return CreatedAtAction(nameof(GetContentType), new { apiSlug = result.ApiSlug }, result);
        }

        // DELETE: api/content-type-admin/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.DeleteContentTypes)]
        public async Task<IActionResult> DeleteContentType(Guid id)
        {
            var success = await _contentTypeService.DeleteContentTypeAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        // POST: api/content-type-admin/{id}/fields
        [HttpPost("{id}/fields")]
        [Authorize(Policy = Permissions.EditContentTypes)]
        public async Task<IActionResult> AddField(Guid id, [FromBody] CreateContentFieldDto dto)
        {
            var field = await _contentTypeService.AddFieldToContentTypeAsync(id, dto);
            if (field == null)
            {
                return BadRequest("Failed to add field. Field ApiSlug might already exist.");
            }
            return Ok(field);
        }

        // DELETE: api/content-type-admin/{id}/fields/{fieldId}
        [HttpDelete("{id}/fields/{fieldId}")] // (روت صحیح)
        [Authorize(Policy = Permissions.EditContentTypes)]
        // (اصلاح شد)
        // این اکشن اکنون هر دو پارامتر 'id' (به عنوان contentTypeId) و 'fieldId'
        // را از روت دریافت می‌کند
        public async Task<IActionResult> DeleteContentField(Guid id, Guid fieldId)
        {
            // (اصلاح شد)
            // هر دو پارامتر به سرویس ارسال می‌شوند
            var success = await _contentTypeService.DeleteContentFieldAsync(id, fieldId);
            if (!success) return NotFound();
            return NoContent(); // (تغییر به NoContent برای هماهنگی با سایر اکشن‌های Delete)
        }
    }
}