// --- شروع Using Directives ---
using AnosheCms.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Text.Json;
// --- پایان Using Directives ---

namespace AnosheCms.Infrastructure.Persistence.Configurations
{
    public class ContentItemConfiguration : IEntityTypeConfiguration<ContentItem>
    {
        public void Configure(EntityTypeBuilder<ContentItem> builder)
        {
            builder.Property(ci => ci.Status)
                .IsRequired()
                .HasMaxLength(50);

            // --- پیکربندی کلیدی JSON ---
            builder.Property(ci => ci.ContentData)
                .HasConversion(
                    // تابع تبدیل C# (Dictionary) به دیتابیس (string)
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),

                    // تابع تبدیل دیتابیس (string) به C# (Dictionary)
                    v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, (JsonSerializerOptions?)null) ?? new Dictionary<string, object>()
                );
            // --- خط .IsJson() در اینجا حذف شد ---

            // تعریف رابطه با ContentType
            builder.HasOne(ci => ci.ContentType)
                .WithMany()
                .HasForeignKey(ci => ci.ContentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // اعمال فیلتر گلوبال برای Soft Delete
            builder.HasQueryFilter(ci => !ci.IsDeleted);
        }
    }
}