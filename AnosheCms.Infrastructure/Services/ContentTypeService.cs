using AnosheCms.Application.DTOs.ContentType;
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities;
using AnosheCms.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace AnosheCms.Infrastructure.Services
{
    public class ContentTypeService : IContentTypeService
    {
        private readonly ApplicationDbContext _context;

        public ContentTypeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ContentTypeDto>> GetAllContentTypesAsync()
        {
            var types = await _context.ContentTypes
                .Include(ct => ct.Fields)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
            return types.Select(MapToDto).ToList();
        }

        public async Task<ContentTypeDto> GetContentTypeByIdAsync(Guid id)
        {
            var ct = await _context.ContentTypes
                .Include(c => c.Fields)
                .FirstOrDefaultAsync(c => c.Id == id);
            return ct == null ? null : MapToDto(ct);
        }

        public async Task<ContentTypeDto?> GetContentTypeBySlugAsync(string apiSlug)
        {
            var ct = await _context.ContentTypes
                .Include(c => c.Fields)
                .FirstOrDefaultAsync(c => c.ApiSlug == apiSlug);
            return ct == null ? null : MapToDto(ct);
        }

        public async Task<Guid> CreateContentTypeAsync(CreateContentTypeDto dto)
        {
            if (await _context.ContentTypes.AnyAsync(ct => ct.ApiSlug == dto.ApiSlug))
                throw new Exception("این ApiSlug تکراری است.");

            var contentType = new ContentType
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                ApiSlug = dto.ApiSlug,
                Description = dto.Description,
                Fields = dto.Fields?.Select((f, i) => new ContentField
                {
                    Name = f.Name,
                    Label = f.Label,
                    FieldType = f.FieldType,
                    IsRequired = f.IsRequired,
                    Options = f.Options,
                    Order = i
                }).ToList() ?? new List<ContentField>()
            };

            await _context.ContentTypes.AddAsync(contentType);
            await _context.SaveChangesAsync();
            return contentType.Id;
        }

        public async Task UpdateContentTypeAsync(Guid id, CreateContentTypeDto dto)
        {
            var contentType = await _context.ContentTypes
                .Include(c => c.Fields)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contentType == null) throw new Exception("نوع محتوا یافت نشد.");

            contentType.Name = dto.Name;
            contentType.Description = dto.Description;

            _context.ContentFields.RemoveRange(contentType.Fields);
            contentType.Fields = dto.Fields?.Select((f, i) => new ContentField
            {
                Name = f.Name,
                Label = f.Label,
                FieldType = f.FieldType,
                IsRequired = f.IsRequired,
                Options = f.Options,
                Order = i,
                ContentTypeId = id
            }).ToList();

            await _context.SaveChangesAsync();
        }

        public async Task DeleteContentTypeAsync(Guid id)
        {
            var ct = await _context.ContentTypes.FindAsync(id);
            if (ct != null)
            {
                _context.ContentTypes.Remove(ct);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ContentFieldDto?> AddFieldToContentTypeAsync(Guid contentTypeId, CreateContentFieldDto dto)
        {
            var field = new ContentField
            {
                ContentTypeId = contentTypeId,
                Name = dto.Name,
                Label = dto.Label,
                FieldType = dto.FieldType,
                IsRequired = dto.IsRequired,
                Options = dto.Options
            };
            await _context.ContentFields.AddAsync(field);
            await _context.SaveChangesAsync();
            return new ContentFieldDto { Id = field.Id, Name = field.Name, Label = field.Label, FieldType = field.FieldType, Options = field.Options };
        }

        public async Task<bool> DeleteContentFieldAsync(Guid contentTypeId, Guid fieldId)
        {
            var field = await _context.ContentFields.FirstOrDefaultAsync(f => f.Id == fieldId && f.ContentTypeId == contentTypeId);
            if (field == null) return false;
            _context.ContentFields.Remove(field);
            await _context.SaveChangesAsync();
            return true;
        }

        private ContentTypeDto MapToDto(ContentType ct)
        {
            return new ContentTypeDto
            {
                Id = ct.Id,
                Name = ct.Name,
                ApiSlug = ct.ApiSlug,
                Description = ct.Description,
                Fields = ct.Fields?.OrderBy(f => f.Order).Select(f => new ContentFieldDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Label = f.Label,
                    FieldType = f.FieldType,
                    IsRequired = f.IsRequired,
                    Options = f.Options
                }).ToList() ?? new List<ContentFieldDto>()
            };
        }
    }
}