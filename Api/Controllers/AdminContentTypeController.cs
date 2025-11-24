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
    [Route("api/admin/content-types")] // (مسیر هماهنگ با فرانت)
    [Authorize]
    public class AdminContentTypeController : ControllerBase
    {
        private readonly IContentTypeService _contentTypeService;

        public AdminContentTypeController(IContentTypeService contentTypeService)
        {
            _contentTypeService = contentTypeService;
        }

        [HttpGet]
        [Authorize(Policy = Permissions.ManageContentTypes)]
        public async Task<IActionResult> GetAll()
        {
            var types = await _contentTypeService.GetAllContentTypesAsync();
            return Ok(types);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = Permissions.ManageContentTypes)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var type = await _contentTypeService.GetContentTypeByIdAsync(id);
            if (type == null) return NotFound();
            return Ok(type);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.ManageContentTypes)]
        public async Task<IActionResult> Create([FromBody] CreateContentTypeDto request)
        {
            var id = await _contentTypeService.CreateContentTypeAsync(request);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = Permissions.ManageContentTypes)]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateContentTypeDto request)
        {
            await _contentTypeService.UpdateContentTypeAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.ManageContentTypes)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _contentTypeService.DeleteContentTypeAsync(id);
            return NoContent();
        }
    }
}