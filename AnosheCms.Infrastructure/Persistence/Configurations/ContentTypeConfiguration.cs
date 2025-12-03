// File: AnosheCms.Infrastructure/Persistence/Configurations/ContentTypeConfiguration.cs
using AnosheCms.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnosheCms.Infrastructure.Persistence.Configurations
{
    public class ContentTypeConfiguration : IEntityTypeConfiguration<ContentType>
    {
        public void Configure(EntityTypeBuilder<ContentType> builder)
        {
            builder.Property(ct => ct.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(ct => ct.ApiSlug)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");
            builder.Property(ct => ct.ApiSlug)
                .IsRequired()
                .HasMaxLength(100);

            // ApiSlug باید در کل جدول یونیک باشد
            builder.HasIndex(ct => ct.ApiSlug)
                .IsUnique();

            // تعریف رابطه یک-به-چند با ContentField
            builder.HasMany(ct => ct.Fields)
                .WithOne(cf => cf.ContentType)
                .HasForeignKey(cf => cf.ContentTypeId)
                .OnDelete(DeleteBehavior.Cascade); // اگر ContentType حذف شد، فیلدهایش هم حذف شوند

            // اعمال فیلتر گلوبال برای Soft Delete
            builder.HasQueryFilter(ct => !ct.IsDeleted);
        }
    }
}