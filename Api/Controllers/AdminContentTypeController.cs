// File: Api/Controllers/AdminContentTypeController.cs
using AnosheCms.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AnosheCms.Domain.Constants; // <-- (جدید)

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/admin/content-types")]
    [Authorize]
    public class AdminContentTypeController : ControllerBase
    {
        private readonly IContentTypeService _contentTypeService;
        public AdminContentTypeController(IContentTypeService contentTypeService)
        {
            _contentTypeService = contentTypeService;
        }

        [HttpPost]
        [Authorize(Policy = Permissions.CreateContentTypes)] // <-- پالیسی اعمال شد
        public async Task<IActionResult> CreateContentType([FromBody] CreateContentTypeDto dto)
        {
            var result = await _contentTypeService.CreateContentTypeAsync(dto);
            if (result == null) return BadRequest(new { message = "API Slug already exists." });
            return CreatedAtAction(nameof(GetContentTypeBySlug), new { apiSlug = result.ApiSlug }, result);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.ViewContentTypes)] // <-- پالیسی اعمال شد
        public async Task<IActionResult> GetAllContentTypes()
        {
            var result = await _contentTypeService.GetAllContentTypesAsync();
            return Ok(result);
        }

        [HttpGet("{apiSlug}")]
        [Authorize(Policy = Permissions.ViewContentTypes)] // <-- پالیسی اعمال شد
        public async Task<IActionResult> GetContentTypeBySlug(string apiSlug)
        {
            var result = await _contentTypeService.GetContentTypeBySlugAsync(apiSlug);
            if (result == null) return NotFound(new { message = "Content type not found." });
            return Ok(result);
        }

        [HttpPost("{id}/fields")]
        [Authorize(Policy = Permissions.EditContentTypes)] // <-- پالیسی اعمال شد
        public async Task<IActionResult> AddField(Guid id, [FromBody] CreateContentFieldDto dto)
        {
            var result = await _contentTypeService.AddFieldToContentTypeAsync(id, dto);
            if (result == null) return BadRequest(new { message = "Content type not found or field ApiSlug already exists." });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.DeleteContentTypes)] // <-- پالیسی اعمال شد
        public async Task<IActionResult> DeleteContentType(Guid id)
        {
            var success = await _contentTypeService.DeleteContentTypeAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Content type deleted." });
        }

        [HttpDelete("fields/{fieldId}")]
        [Authorize(Policy = Permissions.EditContentTypes)] // <-- پالیسی اعمال شد
        public async Task<IActionResult> DeleteContentField(Guid fieldId)
        {
            var success = await _contentTypeService.DeleteContentFieldAsync(fieldId);
            if (!success) return NotFound();
            return Ok(new { message = "Content field deleted." });
        }
    }
}