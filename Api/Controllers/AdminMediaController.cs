// File: AnosheCms.Api/Controllers/AdminMediaController.cs
using AnosheCms.Application.Interfaces;
using Microsoft.AspNetCore.Authorization; // برای [Authorize]
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/admin/media")]
    [Authorize] // <-- تمام این کنترلر نیازمند لاگین است
    public class AdminMediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;

        public AdminMediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        /// <summary>
        /// آپلود یک فایل جدید.
        /// </summary>
        /// <param name="file">فایل آپلود شده (به عنوان form-data)</param>
        /// <param name="altText">متن جایگزین (به عنوان form-data)</param>
        [HttpPost("upload")]
        [RequestSizeLimit(10 * 1024 * 1024)] // محدودیت 10 مگابایت برای آپلود
        public async Task<IActionResult> UploadFile(IFormFile file, [FromForm] string? altText)
        {
            if (file == null)
            {
                return BadRequest(new { message = "No file provided." });
            }

            var result = await _mediaService.UploadFileAsync(file, altText);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            // یک URL کامل برای فایل ایجاد می‌کنیم تا فرانت‌اند بتواند آن را نمایش دهد
            var url = $"{Request.Scheme}://{Request.Host}/{result.MediaItem.FolderPath}/{result.MediaItem.FileName}";

            return Ok(new
            {
                id = result.MediaItem.Id,
                url = url,
                originalFileName = result.MediaItem.OriginalFileName,
                altText = result.MediaItem.AltText,
                createdDate = result.MediaItem.CreatedDate
            });
        }

        /// <summary>
        /// دریافت لیست تمام آیتم‌های رسانه‌ای
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllMediaItems()
        {
            var items = await _mediaService.GetAllMediaItemsAsync();

            // تبدیل آیتم‌ها به فرمتی که URL کامل را شامل شود
            var result = items.Select(item => new
            {
                id = item.Id,
                url = $"{Request.Scheme}://{Request.Host}/{item.FolderPath}/{item.FileName}",
                originalFileName = item.OriginalFileName,
                altText = item.AltText,
                createdDate = item.CreatedDate,
                mimeType = item.MimeType,
                fileSize = item.FileSize
            });

            return Ok(result);
        }

        /// <summary>
        /// به‌روزرسانی متادیتای یک آیتم
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMediaItem(Guid id, [FromBody] MediaUpdateDto updateDto)
        {
            var success = await _mediaService.UpdateMediaItemAsync(id, updateDto);
            if (!success)
            {
                return NotFound(new { message = "Media item not found." });
            }
            return Ok(new { message = "Media item updated successfully." });
        }

        /// <summary>
        /// حذف یک آیتم رسانه‌ای
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMediaItem(Guid id)
        {
            var success = await _mediaService.DeleteMediaItemAsync(id);
            if (!success)
            {
                return NotFound(new { message = "Media item not found or already deleted." });
            }
            return Ok(new { message = "Media item deleted successfully." });
        }
    }
}