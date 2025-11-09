// File: AnosheCms.Api/Controllers/PublicContentController.cs
using AnosheCms.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/public/content/{contentTypeSlug}")] // مسیر داینامیک
    public class PublicContentController : ControllerBase
    {
        private readonly IContentItemService _contentItemService;

        public PublicContentController(IContentItemService contentItemService)
        {
            _contentItemService = contentItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPublishedItems(string contentTypeSlug)
        {
            var result = await _contentItemService.GetPublishedContentItemsAsync(contentTypeSlug);
            return Ok(result);
        }

        [HttpGet("{itemSlugOrId}")]
        public async Task<IActionResult> GetPublishedItem(string contentTypeSlug, string itemSlugOrId)
        {
            var result = await _contentItemService.GetPublishedContentItemAsync(contentTypeSlug, itemSlugOrId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}