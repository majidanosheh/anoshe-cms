using AnosheCms.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnosheCms.Infrastructure.Persistence.Configurations
{
    public class ContentItemConfiguration : IEntityTypeConfiguration<ContentItem>
    {
        public void Configure(EntityTypeBuilder<ContentItem> builder)
        {
            builder.Property(ci => ci.Status)
                .IsRequired()
                .HasMaxLength(50);

            // ✅ این فیلد اصلی ماست که در دیتابیس ذخیره می‌شود
            builder.Property(ci => ci.DataJson)
                .IsRequired();

            //  FIX: این فیلد دیکشنری را از دیتابیس حذف می‌کنیم تا ارور ندهد
            // (چون ما دیتا را دستی در DataJson سریالایز می‌کنیم)
            builder.Ignore(ci => ci.ContentData);

            // تعریف رابطه با ContentType
            builder.HasOne(ci => ci.ContentType)
                .WithMany() // اگر در ContentType کالکشن ندارید
                .HasForeignKey(ci => ci.ContentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // اعمال فیلتر گلوبال برای Soft Delete
            builder.HasQueryFilter(ci => !ci.IsDeleted);
        }
    }
}