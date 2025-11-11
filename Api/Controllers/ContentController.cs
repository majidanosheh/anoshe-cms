// File: Api/Controllers/ContentController.cs
using AnosheCms.Application.Interfaces;
using Microsoft.AspNetCore.Authorization; // (برای AllowAnonymous)
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/content/{apiSlug}")] // (روت عمومی)
    [AllowAnonymous] // (مهم: این کنترلر عمومی است)
    public class ContentController : ControllerBase
    {
        private readonly IContentEntryService _itemService;

        public ContentController(IContentEntryService itemService)
        {
            _itemService = itemService;
        }

        /// <summary>
        /// دریافت لیست تمام آیتم‌های منتشر شده برای یک نوع محتوا
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPublishedItems(string apiSlug)
        {
            // (استفاده از متد عمومی که در سرویس ساخته بودیم)
            var result = await _itemService.GetPublishedContentEntriesAsync(apiSlug);
            return Ok(result);
        }

        /// <summary>
        /// دریافت یک آیتم منتشر شده خاص با استفاده از شناسه (Guid) آن
        /// </summary>
        /// <param name="apiSlug">شناسه نوع محتوا (e.g. 'blog-posts')</param>
        /// <param name="itemIdentifier">شناسه آیتم (e.g. 'guid' یا در آینده 'item-slug')</param>
        [HttpGet("{itemIdentifier}")]
        public async Task<IActionResult> GetPublishedItem(string apiSlug, string itemIdentifier)
        {
            // (استفاده از متد عمومی که در سرویس ساخته بودیم)
            var result = await _itemService.GetPublishedContentEntryAsync(apiSlug, itemIdentifier);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}