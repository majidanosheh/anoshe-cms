// File: AnosheCms.Infrastructure/Persistence/Configurations/MediaItemConfiguration.cs
using AnosheCms.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnosheCms.Infrastructure.Persistence.Configurations
{
    public class MediaItemConfiguration : IEntityTypeConfiguration<MediaItem>
    {
        public void Configure(EntityTypeBuilder<MediaItem> builder)
        {
            builder.Property(m => m.FileName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(m => m.OriginalFileName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(m => m.MimeType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.FolderPath)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(m => m.AltText)
                .HasMaxLength(500);

            // اعمال فیلتر گلوبال برای Soft Delete
            builder.HasQueryFilter(m => !m.IsDeleted);
        }
    }
}