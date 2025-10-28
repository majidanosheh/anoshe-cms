// File: AnosheCms.Application/Interfaces/IContentTypeService.cs
using AnosheCms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnosheCms.Application.Interfaces
{
    // DTOs (Data Transfer Objects) برای ایجاد و به‌روزرسانی
    // ما از رکوردهای C# 9 برای سادگی استفاده می‌کنیم

    public record CreateContentTypeDto(string Name, string ApiSlug);
    public record CreateContentFieldDto(string Name, string ApiSlug, string FieldType, bool IsRequired, string? Settings);

    // DTO برای نمایش
    public record ContentFieldDto(Guid Id, string Name, string ApiSlug, string FieldType, bool IsRequired, string? Settings);
    public record ContentTypeDto(Guid Id, string Name, string ApiSlug, List<ContentFieldDto> Fields);


    public interface IContentTypeService
    {
        /// <summary>
        /// ایجاد یک نوع محتوای جدید
        /// </summary>
        Task<ContentTypeDto?> CreateContentTypeAsync(CreateContentTypeDto dto);

        /// <summary>
        /// دریافت یک نوع محتوا با فیلدهایش، بر اساس ApiSlug
        /// </summary>
        Task<ContentTypeDto?> GetContentTypeBySlugAsync(string apiSlug);

        /// <summary>
        /// دریافت لیست تمام انواع محتوا (فقط اطلاعات اصلی)
        /// </summary>
        Task<List<ContentTypeDto>> GetAllContentTypesAsync();

        /// <summary>
        /// افزودن یک فیلد جدید به یک نوع محتوای موجود
        /// </summary>
        Task<ContentFieldDto?> AddFieldToContentTypeAsync(Guid contentTypeId, CreateContentFieldDto dto);

        /// <summary>
        /// حذف یک نوع محتوا (Soft Delete)
        /// </summary>
        Task<bool> DeleteContentTypeAsync(Guid id);

        /// <summary>
        /// حذف یک فیلد از یک نوع محتوا (Soft Delete)
        /// </summary>
        Task<bool> DeleteContentFieldAsync(Guid fieldId);
    }
}