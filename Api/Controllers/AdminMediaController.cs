// File: Api/Controllers/AdminMediaController.cs
using AnosheCms.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/admin/media")]
    [Authorize]
    public class AdminMediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;

        public AdminMediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMedia()
        {
            var result = await _mediaService.GetAllMediaAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMediaById(Guid id)
        {
            var result = await _mediaService.GetMediaByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> UploadMedia(IFormFileCollection files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest(new { message = "هیچ فایلی برای آپلود انتخاب نشده است." });
            }

            var result = await _mediaService.UploadMediaAsync(files);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedia(Guid id)
        {
            var success = await _mediaService.DeleteMediaAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "فایل با موفقیت حذف شد." });
        }
    }
}