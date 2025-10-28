
using AnosheCms.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AnosheCms.Infrastructure.Persistence.Configurations
{
    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.Property(r => r.Description)
                .HasMaxLength(500);

            // اعمال فیلتر گلوبال برای Soft Delete
            builder.HasQueryFilter(r => !r.IsDeleted);
        }
    }
}