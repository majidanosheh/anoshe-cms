// File: AnosheCms.Infrastructure/Persistence/Configurations/ContentFieldConfiguration.cs
using AnosheCms.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnosheCms.Infrastructure.Persistence.Configurations
{
    public class ContentFieldConfiguration : IEntityTypeConfiguration<ContentField>
    {
        public void Configure(EntityTypeBuilder<ContentField> builder)
        {
            builder.Property(cf => cf.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(cf => cf.ApiSlug)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(cf => cf.FieldType)
                .IsRequired()
                .HasMaxLength(50);

            // Settings را به عنوان JSON ذخیره می‌کنیم (اگر از SQL Server 2016+ استفاده می‌کنید)
            // .IsJson(); // این را فعلاً کامنت می‌کنیم تا از سازگاری اطمینان حاصل کنیم

            // ApiSlug باید در محدوده ContentType خود یونیک باشد
            builder.HasIndex(cf => new { cf.ContentTypeId, cf.ApiSlug })
                .IsUnique();

            // اعمال فیلتر گلوبال برای Soft Delete
            builder.HasQueryFilter(cf => !cf.IsDeleted);
        }
    }
}