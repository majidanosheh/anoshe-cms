using AnosheCms.Application.DTOs.ContentEntry;
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ContentEntryCreateDto = AnosheCms.Application.Interfaces.ContentEntryCreateDto;

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/content-admin/{apiSlug}")]
    [Authorize]
    public class ContentAdminController : ControllerBase
    {
        private readonly IContentEntryService _entryService;

        public ContentAdminController(IContentEntryService entryService)
        {
            _entryService = entryService;
        }

        // GET: api/content-admin/{apiSlug}
        [HttpGet]
        [Authorize(Policy = Permissions.ViewContent)]
        // (اصلاح شد) نام متد برای مطابقت با اینترفیس تغییر کرد (GetAll -> Get)
        public async Task<IActionResult> GetEntries(string apiSlug)
        {
            // (اصلاح شد) فراخوانی متد با نام صحیح در اینترفیس
            var result = await _entryService.GetContentEntriesAsync(apiSlug);
            return Ok(result);
        }

        // GET: api/content-admin/{apiSlug}/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = Permissions.ViewContent)]
        public async Task<IActionResult> GetEntry(string apiSlug, Guid id)
        {
            var result = await _entryService.GetContentEntryByIdAsync(apiSlug, id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // POST: api/content-admin/{apiSlug}
        [HttpPost]
        [Authorize(Policy = Permissions.CreateContent)]
        public async Task<IActionResult> CreateEntry(string apiSlug, [FromBody] ContentEntryCreateDto dto)
        {
            var (result, error) = await _entryService.CreateContentEntryAsync(apiSlug, dto);
            if (error != null) return BadRequest(new { Errors = new[] { error } });
            return CreatedAtAction(nameof(GetEntry), new { apiSlug = apiSlug, id = result.Id }, result);
        }

        // PUT: api/content-admin/{apiSlug}/{id}
        [HttpPut("{id}")]
        [Authorize(Policy = Permissions.EditContent)]
        public async Task<IActionResult> UpdateEntry(string apiSlug, Guid id, [FromBody] ContentEntryCreateDto dto)
        {
            var (result, error) = await _entryService.UpdateContentEntryAsync(apiSlug, id, dto);
            if (error != null) return BadRequest(new { Errors = new[] { error } });
            if (result == null) return NotFound();
            return Ok(result);
        }

        // DELETE: api/content-admin/{apiSlug}/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.DeleteContent)]
        public async Task<IActionResult> DeleteEntry(string apiSlug, Guid id)
        {
            var success = await _entryService.DeleteContentEntryAsync(apiSlug, id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}