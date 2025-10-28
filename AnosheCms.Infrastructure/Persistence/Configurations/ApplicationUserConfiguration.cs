using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AnosheCms.Domain.Entities;

namespace AnosheCms.Infrastructure.Persistence.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // اعمال پیکربندی‌هایی که از Domain حذف کردیم
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            // اعمال فیلتر گلوبال برای Soft Delete
            builder.HasQueryFilter(u => !u.IsDeleted);
        }
    }
}