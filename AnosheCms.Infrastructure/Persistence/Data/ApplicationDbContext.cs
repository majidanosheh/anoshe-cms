using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities;
using AnosheCms.Domain.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AnosheCms.Infrastructure.Persistence.Data
{
    public class ApplicationDbContext : IdentityDbContext<
        ApplicationUser, ApplicationRole, Guid,
        IdentityUserClaim<Guid>, ApplicationUserRole, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        private readonly ICurrentUserService _currentUserService;

        // --- Core Tables ---
        public DbSet<ContentType> ContentTypes { get; set; }
        public DbSet<ContentField> ContentFields { get; set; }
        public DbSet<ContentItem> ContentItems { get; set; }
        public DbSet<MediaFile> MediaFiles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<UserLoginHistory> UserLoginHistories { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        // --- Form Builder Tables ---
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

        // --- SaveChanges & Auditing ---
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditRules();
            var auditEntries = OnBeforeSaveChanges();
            var result = await base.SaveChangesAsync(cancellationToken);
            await OnAfterSaveChanges(auditEntries);
            return result;
        }

        private void ApplyAuditRules()
        {
            var userId = _currentUserService?.UserId;
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is IAuditable auditableEntity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        auditableEntity.CreatedBy = userId;
                        auditableEntity.CreatedDate = now;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        auditableEntity.LastModifiedBy = userId;
                        auditableEntity.LastModifiedDate = now;
                    }
                }

                if (entry.Entity is ISoftDelete softDeleteEntity && entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    softDeleteEntity.IsDeleted = true;
                    softDeleteEntity.DeletedBy = userId;
                    softDeleteEntity.DeletedDate = now;
                }
            }
        }

        private List<AuditEntry> OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry(entry)
                {
                    TableName = entry.Entity.GetType().Name,
                    UserId = _currentUserService?.UserId,
                    IpAddress = _currentUserService?.RemoteIpAddress,
                    UserAgent = _currentUserService?.UserAgent
                };

                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = "Create";
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = "Delete";
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.Metadata.Name == "IsDeleted" && property.CurrentValue is bool isDeleted && isDeleted)
                                auditEntry.AuditType = "SoftDelete";
                            else
                                auditEntry.AuditType = "Update";

                            if (property.IsModified)
                            {
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            return auditEntries.Where(e => e.HasTemporaryProperties || e.AuditType == "Delete" || e.AuditType == "SoftDelete" || e.NewValues.Count > 0).ToList();
        }

        private async Task OnAfterSaveChanges(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || !auditEntries.Any()) return;

            foreach (var auditEntry in auditEntries)
            {
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    else
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                }
                AuditLogs.Add(auditEntry.ToAudit());
            }
            await base.SaveChangesAsync();
        }

        // --- Configurations ---
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1. Identity Tables Mapping
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<ApplicationRole>().ToTable("Roles");
            builder.Entity<ApplicationUserRole>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

            // 2. User & Role Relationships (Explicit Config)
            builder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
                userRole.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId).IsRequired();
                userRole.HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasForeignKey(ur => ur.UserId).IsRequired();
            });

            // 3. Custom User Relationships (Explicit Config to fix UserId1)
            // --- RefreshToken ---
            builder.Entity<RefreshToken>(entity => {
                entity.HasOne(rt => rt.User)
                      .WithMany(u => u.RefreshTokens)
                      .HasForeignKey(rt => rt.UserId)
                      .OnDelete(DeleteBehavior.Cascade); // تنظیم رفتار حذف
            });

            // --- UserLoginHistory ---
            builder.Entity<UserLoginHistory>(entity => {
                entity.HasOne(lh => lh.User)
                      .WithMany(u => u.LoginHistories)
                      .HasForeignKey(lh => lh.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- UserSession ---
            builder.Entity<UserSession>(entity => {
                entity.HasOne(us => us.User)
                      .WithMany(u => u.Sessions)
                      .HasForeignKey(us => us.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 4. AuditLog Config
            builder.Entity<AuditLog>(b => {
                b.ToTable("AuditLogs");
                b.HasIndex(a => a.Timestamp);
                b.HasIndex(a => a.UserId);
                b.Property(a => a.OldValues).HasColumnType("nvarchar(max)");
                b.Property(a => a.NewValues).HasColumnType("nvarchar(max)");
            });

            // 5. Apply Other Configurations
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }

    // Helper Class for Audit
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry) => Entry = entry;
        public EntityEntry Entry { get; }
        public Guid? UserId { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new();
        public Dictionary<string, object> OldValues { get; } = new();
        public Dictionary<string, object> NewValues { get; } = new();
        public List<PropertyEntry> TemporaryProperties { get; } = new();
        public string AuditType { get; set; }
        public bool HasTemporaryProperties => TemporaryProperties.Any();

        public AuditLog ToAudit()
        {
            var audit = new AuditLog
            {
                UserId = UserId,
                Action = AuditType,
                EntityName = TableName,
                Timestamp = DateTime.UtcNow,
                IpAddress = IpAddress,
                UserAgent = UserAgent,
                EntityId = JsonSerializer.Serialize(KeyValues),
                OldValues = OldValues.Count == 0 ? null : JsonSerializer.Serialize(OldValues),
                NewValues = NewValues.Count == 0 ? null : JsonSerializer.Serialize(NewValues)
            };
            return audit;
        }
    }
}