// مسیر: AnosheCms.Infrastructure/Persistence/Data/ApplicationDbContext.cs
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
using AnosheCms.Domain.Constants;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AnosheCms.Infrastructure.Persistence.Data
{
    public class ApplicationDbContext : IdentityDbContext<
        ApplicationUser, ApplicationRole, Guid,
        IdentityUserClaim<Guid>, ApplicationUserRole, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        private readonly ICurrentUserService _currentUserService;

        // --- DbSet های هسته CMS ---
        public DbSet<ContentType> ContentTypes { get; set; }
        public DbSet<ContentField> ContentFields { get; set; }
        public DbSet<ContentItem> ContentItems { get; set; }
        public DbSet<MediaFile> MediaFiles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<UserLoginHistory> UserLoginHistories { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        // --- (DbSet های فرم‌ساز بازنگری‌شده) ---
        public DbSet<Form> Forms { get; set; }
        public DbSet<FormField> FormFields { get; set; }
        public DbSet<FormSubmission> FormSubmissions { get; set; }
        public DbSet<FormSubmissionData> FormSubmissionData { get; set; } 

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService currentUserService = null
            ) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService?.UserId;
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

            // (پیکربندی‌های Identity)
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<ApplicationRole>().ToTable("Roles");
            builder.Entity<ApplicationUserRole>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

            builder.Entity<ApplicationUser>(b => {
                b.HasMany(e => e.UserRoles).WithOne(e => e.User).HasForeignKey(ur => ur.UserId).IsRequired();
                b.HasMany(e => e.RefreshTokens).WithOne(e => e.User).HasForeignKey(rt => rt.UserId).IsRequired();
                b.HasMany(e => e.LoginHistories).WithOne(e => e.User).HasForeignKey(lh => lh.UserId).IsRequired();
                b.HasMany(e => e.Sessions).WithOne(e => e.User).HasForeignKey(s => s.UserId).IsRequired();
            });
            builder.Entity<ApplicationRole>(b => {
                b.HasMany(e => e.UserRoles).WithOne(e => e.Role).HasForeignKey(ur => ur.RoleId).IsRequired();
            });

            // (پیکربندی ContentItem)
            builder.Entity<ContentItem>(b => {
                b.Property(ci => ci.ContentData)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, (JsonSerializerOptions)null)
                    );
            });


            builder.Entity<Form>(b => {
                b.HasIndex(f => f.ApiSlug).IsUnique();
                b.Property(f => f.Settings).HasColumnType("nvarchar(max)");
                b.HasQueryFilter(f => !f.IsDeleted);
            });

            builder.Entity<FormField>(b => {
                b.Property(ff => ff.Settings).HasColumnType("nvarchar(max)");
                b.Property(ff => ff.ValidationRules).HasColumnType("nvarchar(max)");
                b.Property(ff => ff.ConditionalLogic).HasColumnType("nvarchar(max)");

                b.HasOne(ff => ff.Form)
                 .WithMany(f => f.Fields)
                 .HasForeignKey(ff => ff.FormId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasQueryFilter(ff => !ff.IsDeleted);
            });

            builder.Entity<FormSubmission>(b => {
                b.HasOne(fs => fs.Form)
                 .WithMany(f => f.Submissions)
                 .HasForeignKey(fs => fs.FormId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasQueryFilter(fs => !fs.IsDeleted);
            });

            builder.Entity<FormSubmissionData>(b => {
                b.HasKey(fsd => fsd.Id); 

                b.HasOne(fsd => fsd.Submission)
                 .WithMany(fs => fs.SubmissionData)
                 .HasForeignKey(fsd => fsd.SubmissionId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.Property(fsd => fsd.FieldValue).HasColumnType("nvarchar(max)");
                b.HasIndex(fsd => fsd.SubmissionId);
                b.HasIndex(fsd => fsd.FieldName);
            });
            // --- (پایان پیکربندی‌های ) ---

            // (فیلترهای SoftDelete)
            builder.Entity<ApplicationUser>().HasQueryFilter(u => !u.IsDeleted);
            builder.Entity<ContentType>().HasQueryFilter(ct => !ct.IsDeleted);
            builder.Entity<ContentField>().HasQueryFilter(cf => !cf.IsDeleted);
            builder.Entity<ContentItem>().HasQueryFilter(ci => !ci.IsDeleted);
            builder.Entity<MediaFile>().HasQueryFilter(mf => !mf.IsDeleted);
            builder.Entity<ApplicationRole>().HasQueryFilter(ar => !ar.IsDeleted);

            // (پیکربندی‌های روابط موجود)
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

            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole
                {
                    Id = SUPER_ADMIN_ROLE_ID,
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN",
                    DisplayName = "سوپر ادمین",
                    Description = "دسترسی کامل به تمام سیستم",
                    IsSystemRole = true,
                    CreatedDate = DateTime.UtcNow
                },
                new ApplicationRole
                {
                    Id = ADMIN_ROLE_ID,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    DisplayName = "ادمین",
                    Description = "دسترسی به بخش مدیریت محتوا و ساختار",
                    IsSystemRole = true,
                    CreatedDate = DateTime.UtcNow
                },
                new ApplicationRole
                {
                    Id = USER_ROLE_ID,
                    Name = "User",
                    NormalizedName = "USER",
                    DisplayName = "کاربر",
                    Description = "دسترسی پایه (در صورت نیاز)",
                    IsSystemRole = true,
                    CreatedDate = DateTime.UtcNow
                }
            );

            var adminPermissions = new List<IdentityRoleClaim<Guid>> {
                new IdentityRoleClaim<Guid> { Id = 1, RoleId = ADMIN_ROLE_ID, ClaimType = "Permission", ClaimValue = Permissions.ViewDashboard },
                new IdentityRoleClaim<Guid> { Id = 2, RoleId = ADMIN_ROLE_ID, ClaimType = "Permission", ClaimValue = Permissions.ViewContentTypes },
                new IdentityRoleClaim<Guid> { Id = 3, RoleId = ADMIN_ROLE_ID, ClaimType = "Permission", ClaimValue = Permissions.CreateContentTypes },
                new IdentityRoleClaim<Guid> { Id = 4, RoleId = ADMIN_ROLE_ID, ClaimType = "Permission", ClaimValue = Permissions.EditContentTypes },
                new IdentityRoleClaim<Guid> { Id = 5, RoleId = ADMIN_ROLE_ID, ClaimType = "Permission", ClaimValue = Permissions.DeleteContentTypes },
                new IdentityRoleClaim<Guid> { Id = 6, RoleId = ADMIN_ROLE_ID, ClaimType = "Permission", ClaimValue = Permissions.ViewMedia },
                new IdentityRoleClaim<Guid> { Id = 7, RoleId = ADMIN_ROLE_ID, ClaimType = "Permission", ClaimValue = Permissions.CreateMedia },
                new IdentityRoleClaim<Guid> { Id = 8, RoleId = ADMIN_ROLE_ID, ClaimType = "Permission", ClaimValue = Permissions.DeleteMedia },
                new IdentityRoleClaim<Guid> { Id = 9, RoleId = ADMIN_ROLE_ID, ClaimType = "Permission", ClaimValue = Permissions.ViewSettings },
                new IdentityRoleClaim<Guid> { Id = 10, RoleId = ADMIN_ROLE_ID, ClaimType = "Permission", ClaimValue = Permissions.EditSettings },
                new IdentityRoleClaim<Guid> { Id = 11, RoleId = ADMIN_ROLE_ID, ClaimType = "Permission", ClaimValue = Permissions.ViewContent },
                new IdentityRoleClaim<Guid> { Id = 12, RoleId = ADMIN_ROLE_ID, ClaimType = "Permission", ClaimValue = Permissions.CreateContent },
                new IdentityRoleClaim<Guid> { Id = 13, RoleId = ADMIN_ROLE_ID, ClaimType = "Permission", ClaimValue = Permissions.EditContent },
                new IdentityRoleClaim<Guid> { Id = 14, RoleId = ADMIN_ROLE_ID, ClaimType = "Permission", ClaimValue = Permissions.DeleteContent }
            };
            builder.Entity<IdentityRoleClaim<Guid>>().HasData(adminPermissions);

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

            builder.Entity<ApplicationUserRole>().HasData(
                new ApplicationUserRole
                {
                    UserId = SUPER_ADMIN_USER_ID,
                    RoleId = SUPER_ADMIN_ROLE_ID,
                    AssignedAt = DateTime.UtcNow
                },
                new ApplicationUserRole
                {
                    UserId = SUPER_ADMIN_USER_ID,
                    RoleId = ADMIN_ROLE_ID,
                    AssignedAt = DateTime.UtcNow
                }
            );
        }
    }
}