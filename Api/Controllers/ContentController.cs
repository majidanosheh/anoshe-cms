// File: Api/Controllers/ContentController.cs
using AnosheCms.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/content")] // (مسیر عمومی، بدون 'admin')
    public class ContentController : ControllerBase
    {
        private readonly IContentEntryService _entryService;
        private readonly IContentTypeService _typeService;

        public ContentController(IContentEntryService entryService, IContentTypeService typeService)
        {
            _entryService = entryService;
            _typeService = typeService;
        }

        // --- Content Type Endpoints ---

        // GET: /api/content/types
        [HttpGet("types")]
        public async Task<IActionResult> GetAllContentTypes()
        {
            // (استفاده از سرویس موجود)
            var result = await _typeService.GetAllContentTypesAsync();
            return Ok(result);
        }

        // GET: /api/content/types/{apiSlug}
        [HttpGet("types/{apiSlug}")]
        public async Task<IActionResult> GetContentType(string apiSlug)
        {
            var result = await _typeService.GetContentTypeBySlugAsync(apiSlug);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // --- Content Entry Endpoints ---

        // GET: /api/content/{contentTypeSlug}
        [HttpGet("{contentTypeSlug}")]
        public async Task<IActionResult> GetPublishedItems(string contentTypeSlug)
        {
            // (استفاده از متد 'Published' که قبلاً نوشتیم) [cite: 387-415]
            var result = await _entryService.GetPublishedContentEntriesAsync(contentTypeSlug);
            return Ok(result);
        }

        // GET: /api/content/{contentTypeSlug}/{itemApiSlug}
        [HttpGet("{contentTypeSlug}/{itemApiSlug}")]
        public async Task<IActionResult> GetPublishedItem(string contentTypeSlug, string itemApiSlug)
        {
            // (استفاده از متد 'Published' که قبلاً نوشتیم) [cite: 387-415]
            var result = await _entryService.GetPublishedContentEntryAsync(contentTypeSlug, itemApiSlug);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}