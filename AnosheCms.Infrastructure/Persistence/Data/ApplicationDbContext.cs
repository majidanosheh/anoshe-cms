// File: AnosheCms.Infrastructure/Persistence/Data/ApplicationDbContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AnosheCms.Domain.Common;
using AnosheCms.Domain.Entities;
using System.Reflection;
using AnosheCms.Application.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AnosheCms.Infrastructure.Persistence.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        private readonly ICurrentUserService _currentUserService;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService currentUserService
            ) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<MediaItem> MediaItems { get; set; }
        public DbSet<ContentType> ContentTypes { get; set; }
        public DbSet<ContentField> ContentFields { get; set; }

        // ---*** کد جدید اضافه شده ***---
        /// <summary>
        /// جدول آیتم‌های محتوای واقعی
        /// </summary>
        public DbSet<ContentItem> ContentItems { get; set; }
        // ---*** پایان کد جدید ***---


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            ApplyAuditAndSoftDeleteRules();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditAndSoftDeleteRules()
        {
            // ... (کد Auditing تغییری نمی‌کند)
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is IAuditable && (
                        e.State == EntityState.Added ||
                        e.State == EntityState.Modified ||
                        e.State == EntityState.Deleted));

            var now = DateTime.UtcNow;
            var userId = _currentUserService.UserId?.ToString() ?? "system";

            foreach (var entry in entries)
            {
                var entity = (IAuditable)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = now;
                    entity.CreatedBy = userId;
                    entity.IsDeleted = false;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.LastModifiedDate = now;
                    entity.LastModifiedBy = userId;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entity.IsDeleted = true;
                    entity.LastModifiedDate = now;
                    entity.LastModifiedBy = userId;
                }
            }
        }
    }
}