// مسیر: AnosheCms.Application/Interfaces/IMediaService.cs
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnosheCms.Application.Interfaces
{
    // DTO برای بازگرداندن اطلاعات فایل
    public record MediaDto(
        Guid Id,
        string FileName,
        string ContentType,
        long Size,
        string Url
    );

    public interface IMediaService
    {
        Task<List<MediaDto>> GetAllMediaAsync();
        Task<MediaDto?> GetMediaByIdAsync(Guid id);
        Task<List<MediaDto>> UploadMediaAsync(IFormFileCollection files);
        Task<bool> DeleteMediaAsync(Guid id);
    }
}