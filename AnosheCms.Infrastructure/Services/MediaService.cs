// File: AnosheCms.Infrastructure/Services/MediaService.cs
// (تصحیح‌شده برای خطای Guid? to string)

using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities;
using AnosheCms.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AnosheCms.Infrastructure.Services
{
    public class MediaService : IMediaService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly string _uploadPath;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MediaService(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _currentUserService = currentUserService;
            _httpContextAccessor = httpContextAccessor;
            _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        private string GetBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null) return string.Empty;
            return $"{request.Scheme}://{request.Host}";
        }

        private MediaDto MapToDto(MediaFile mediaFile)
        {
            var baseUrl = GetBaseUrl();
            return new MediaDto(
                mediaFile.Id,
                mediaFile.FileName,
                mediaFile.ContentType,
                mediaFile.Size,
                $"{baseUrl}/uploads/{mediaFile.StoredFileName}"
            );
        }

        public async Task<List<MediaDto>> GetAllMediaAsync()
        {
            var items = await _context.MediaFiles
                .AsNoTracking()
                .OrderByDescending(m => m.CreatedDate)
                .ToListAsync();

            return items.Select(MapToDto).ToList();
        }

        public async Task<MediaDto?> GetMediaByIdAsync(Guid id)
        {
            var mediaFile = await _context.MediaFiles.FindAsync(id);
            if (mediaFile == null) return null;
            return MapToDto(mediaFile);
        }

        public async Task<bool> DeleteMediaAsync(Guid id)
        {
            var mediaFile = await _context.MediaFiles.FindAsync(id);
            if (mediaFile == null) return false;

            var filePath = Path.Combine(_uploadPath, mediaFile.StoredFileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            _context.MediaFiles.Remove(mediaFile);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<MediaDto>> UploadMediaAsync(IFormFileCollection files)
        {
            var uploadedDtos = new List<MediaDto>();
            var userId = _currentUserService.UserId; // (این Guid? است)

            foreach (var file in files)
            {
                if (file.Length == 0) continue;

                var fileExtension = Path.GetExtension(file.FileName);
                var storedFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(_uploadPath, storedFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var mediaFile = new MediaFile
                {
                    FileName = file.FileName,
                    StoredFileName = storedFileName,
                    ContentType = file.ContentType,
                    Size = file.Length,

                    // ---*** (این خط تصحیح شد) ***---
                    CreatedBy = userId // (تبدیل Guid? به string?)
                };

                _context.MediaFiles.Add(mediaFile);
                await _context.SaveChangesAsync();

                uploadedDtos.Add(MapToDto(mediaFile));
            }

            return uploadedDtos;
        }
    }
}