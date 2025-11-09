// File: AnosheCms.Infrastructure/Persistence/Data/ApplicationDbContext.cs
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities;
using AnosheCms.Domain.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;

namespace AnosheCms.Infrastructure.Persistence.Data
{
    public class ApplicationDbContext : IdentityDbContext<
        ApplicationUser,
        ApplicationRole,
        Guid,
        IdentityUserClaim<Guid>,
        ApplicationUserRole,
        IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>>
    {
        private readonly ICurrentUserService _currentUserService;

        // (DbSets)
        public DbSet<ContentType> ContentTypes { get; set; }
        public DbSet<ContentField> ContentFields { get; set; }
        public DbSet<ContentItem> ContentItems { get; set; }
        public DbSet<MediaFile> MediaFiles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<UserLoginHistory> UserLoginHistories { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }


        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService currentUserService = null // (اجازه null برای DesignTime)
            ) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService?.UserId; // (بررسی null بودن)
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is IAuditable auditableEntity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditableEntity.CreatedBy = userId;
                            auditableEntity.CreatedDate = now;
                            break;
                        case EntityState.Modified:
                            auditableEntity.LastModifiedBy = userId;
                            auditableEntity.LastModifiedDate = now;
                            break;
                    }
                }

                if (entry.Entity is ISoftDelete softDeleteEntity)
                {
                    if (entry.State == EntityState.Deleted)
                    {
                        entry.State = EntityState.Modified;
                        softDeleteEntity.IsDeleted = true;
                        softDeleteEntity.DeletedBy = userId;
                        softDeleteEntity.DeletedDate = now;
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // (تنظیم نام جداول Identity)
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<ApplicationRole>().ToTable("Roles");
            builder.Entity<ApplicationUserRole>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

            // (روابط Identity)
            builder.Entity<ApplicationUser>(b =>
            {
                b.HasMany(e => e.UserRoles).WithOne(e => e.User).HasForeignKey(ur => ur.UserId).IsRequired();
                b.HasMany(e => e.RefreshTokens).WithOne(e => e.User).HasForeignKey(rt => rt.UserId).IsRequired();
                b.HasMany(e => e.LoginHistories).WithOne(e => e.User).HasForeignKey(lh => lh.UserId).IsRequired();
                b.HasMany(e => e.Sessions).WithOne(e => e.User).HasForeignKey(s => s.UserId).IsRequired();
            });

            builder.Entity<ApplicationRole>(b =>
            {
                b.HasMany(e => e.UserRoles).WithOne(e => e.Role).HasForeignKey(ur => ur.RoleId).IsRequired();
            });

            // (پیکربندی ContentItem JSON)
            builder.Entity<ContentItem>(b =>
            {
                b.Property(ci => ci.ContentData)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, (JsonSerializerOptions)null)
                    );
            });

            // (اعمال فیلترهای سراسری)
            builder.Entity<ApplicationUser>().HasQueryFilter(u => !u.IsDeleted);
            builder.Entity<ContentType>().HasQueryFilter(ct => !ct.IsDeleted);
            builder.Entity<ContentField>().HasQueryFilter(cf => !cf.IsDeleted);
            builder.Entity<ContentItem>().HasQueryFilter(ci => !ci.IsDeleted);
            builder.Entity<MediaFile>().HasQueryFilter(mf => !mf.IsDeleted);
            builder.Entity<ApplicationRole>().HasQueryFilter(ar => !ar.IsDeleted);

            // (ایندکس‌های 177.txt)
            builder.Entity<RefreshToken>().HasIndex(r => r.Token).IsUnique();
            builder.Entity<RefreshToken>().HasIndex(r => new { r.UserId, r.IsRevoked });
            builder.Entity<UserLoginHistory>().HasIndex(h => h.UserId);
            builder.Entity<UserLoginHistory>().HasIndex(h => h.LoginDate);
            builder.Entity<UserSession>().HasIndex(s => s.SessionId).IsUnique();
            builder.Entity<UserSession>().HasIndex(s => new { s.UserId, s.IsActive });

            SeedData(builder);
        }

        private void SeedData(ModelBuilder builder)
        {
            Guid SUPER_ADMIN_ROLE_ID = Guid.Parse("a1b1c1d1-1111-4444-8888-a1b1c1d1e1f1");
            Guid ADMIN_ROLE_ID = Guid.Parse("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2");
            Guid USER_ROLE_ID = Guid.Parse("a3b3c3d3-3333-4444-8888-a3b3c3d3e3f3");
            Guid SUPER_ADMIN_USER_ID = Guid.Parse("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1");

            // ---*** (تصحیح شد: فیلد 'Description' اضافه شد) ***---
            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole
                {
                    Id = SUPER_ADMIN_ROLE_ID,
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN",
                    DisplayName = "سوپر ادمین",
                    Description = "دسترسی کامل به تمام سیستم", // (اضافه شد)
                    IsSystemRole = true,
                    CreatedDate = DateTime.UtcNow
                },
                new ApplicationRole
                {
                    Id = ADMIN_ROLE_ID,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    DisplayName = "ادمین",
                    Description = "دسترسی به بخش مدیریت محتوا و ساختار", // (اضافه شد)
                    IsSystemRole = true,
                    CreatedDate = DateTime.UtcNow
                },
                new ApplicationRole
                {
                    Id = USER_ROLE_ID,
                    Name = "User",
                    NormalizedName = "USER",
                    DisplayName = "کاربر",
                    Description = "دسترسی پایه (در صورت نیاز)", // (اضافه شد)
                    IsSystemRole = true,
                    CreatedDate = DateTime.UtcNow
                }
            );

            // (Seed SuperAdmin User)
            var hasher = new PasswordHasher<ApplicationUser>();
            var adminUser = new ApplicationUser
            {
                Id = SUPER_ADMIN_USER_ID,
                UserName = "admin@system.com",
                NormalizedUserName = "ADMIN@SYSTEM.COM",
                Email = "admin@system.com",
                NormalizedEmail = "ADMIN@SYSTEM.COM",
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Administrator",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = Guid.NewGuid().ToString("D")
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin@1Sg");
            builder.Entity<ApplicationUser>().HasData(adminUser);

            // (Assign Role to User)
            builder.Entity<ApplicationUserRole>().HasData(
                new ApplicationUserRole
                {
                    UserId = SUPER_ADMIN_USER_ID,
                    RoleId = SUPER_ADMIN_ROLE_ID,
                    AssignedAt = DateTime.UtcNow
                }
            );
        }
    }
}