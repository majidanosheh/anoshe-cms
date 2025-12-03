using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnosheCms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixLoginSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentItems_ContentTypes_ContentTypeId",
                table: "ContentItems");

            migrationBuilder.DropIndex(
                name: "IX_UserSessions_SessionId",
                table: "UserSessions");

            migrationBuilder.DropIndex(
                name: "IX_UserSessions_UserId_IsActive",
                table: "UserSessions");

            migrationBuilder.DropIndex(
                name: "IX_UserLoginHistories_LoginDate",
                table: "UserLoginHistories");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_UserId_IsRevoked",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_FormSubmissionData_FieldName",
                table: "FormSubmissionData");

            migrationBuilder.DropIndex(
                name: "IX_Forms_ApiSlug",
                table: "Forms");

            migrationBuilder.DropIndex(
                name: "IX_ContentFields_ContentTypeId",
                table: "ContentFields");

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a3b3c3d3-3333-4444-8888-a3b3c3d3e3f3"));

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a1b1c1d1-1111-4444-8888-a1b1c1d1e1f1"), new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2"), new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1") });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a1b1c1d1-1111-4444-8888-a1b1c1d1e1f1"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1"));

            migrationBuilder.DropColumn(
                name: "ContentData",
                table: "ContentItems");

            migrationBuilder.AlterColumn<string>(
                name: "SessionId",
                table: "UserSessions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Roles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "RefreshTokens",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "FieldName",
                table: "FormSubmissionData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ContentTypes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ContentItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FieldType",
                table: "ContentFields",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "MediaItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    AltText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FolderPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaItem", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_UserId",
                table: "UserSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTypes_ApiSlug",
                table: "ContentTypes",
                column: "ApiSlug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContentFields_ContentTypeId_ApiSlug",
                table: "ContentFields",
                columns: new[] { "ContentTypeId", "ApiSlug" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_Timestamp",
                table: "AuditLogs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentItems_ContentTypes_ContentTypeId",
                table: "ContentItems",
                column: "ContentTypeId",
                principalTable: "ContentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentItems_ContentTypes_ContentTypeId",
                table: "ContentItems");

            migrationBuilder.DropTable(
                name: "MediaItem");

            migrationBuilder.DropIndex(
                name: "IX_UserSessions_UserId",
                table: "UserSessions");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_ContentTypes_ApiSlug",
                table: "ContentTypes");

            migrationBuilder.DropIndex(
                name: "IX_ContentFields_ContentTypeId_ApiSlug",
                table: "ContentFields");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_Timestamp",
                table: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs");

            migrationBuilder.AlterColumn<string>(
                name: "SessionId",
                table: "UserSessions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "RefreshTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FieldName",
                table: "FormSubmissionData",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ContentTypes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ContentItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "ContentData",
                table: "ContentItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "FieldType",
                table: "ContentFields",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "Description", "DisplayName", "IsDeleted", "IsSystemRole", "LastModifiedBy", "LastModifiedDate", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("a1b1c1d1-1111-4444-8888-a1b1c1d1e1f1"), null, null, new DateTime(2025, 12, 1, 19, 47, 28, 149, DateTimeKind.Utc).AddTicks(372), null, null, "دسترسی کامل به تمام سیستم", "سوپر ادمین", false, true, null, null, "SuperAdmin", "SUPERADMIN" },
                    { new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2"), null, null, new DateTime(2025, 12, 1, 19, 47, 28, 149, DateTimeKind.Utc).AddTicks(375), null, null, "دسترسی به بخش مدیریت محتوا و ساختار", "ادمین", false, true, null, null, "Admin", "ADMIN" },
                    { new Guid("a3b3c3d3-3333-4444-8888-a3b3c3d3e3f3"), null, null, new DateTime(2025, 12, 1, 19, 47, 28, 149, DateTimeKind.Utc).AddTicks(376), null, null, "دسترسی پایه (در صورت نیاز)", "کاربر", false, true, null, null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "Email", "EmailConfirmed", "FirstName", "IsActive", "IsDeleted", "LastLoginDate", "LastModifiedBy", "LastModifiedDate", "LastName", "LastPasswordChangedDate", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePictureUrl", "SecurityStamp", "TwoFactorEnabled", "TwoFactorSecretKey", "UserName" },
                values: new object[] { new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1"), 0, "20f6914c-f3e8-4bc5-b98d-95b02aa1821a", null, new DateTime(2025, 12, 1, 19, 47, 28, 149, DateTimeKind.Utc).AddTicks(646), null, null, "admin@system.com", true, "System", true, false, null, null, null, "Administrator", null, false, null, "ADMIN@SYSTEM.COM", "ADMIN@SYSTEM.COM", "AQAAAAIAAYagAAAAEGTOMxHmQ3ACL0nb6eVECwcgcU2r0aM940sYKpqMhW4X8m6l1NJTGALwAVj7Lk3inA==", null, false, null, "19bceaae-3a1c-491e-8791-db2b13a09981", false, null, "admin@system.com" });

            migrationBuilder.InsertData(
                table: "RoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "Permission", "Permissions.Dashboard.View", new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2") },
                    { 2, "Permission", "Permissions.ContentTypes.View", new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2") },
                    { 3, "Permission", "Permissions.ContentTypes.Create", new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2") },
                    { 4, "Permission", "Permissions.ContentTypes.Edit", new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2") },
                    { 5, "Permission", "Permissions.ContentTypes.Delete", new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2") },
                    { 6, "Permission", "Permissions.Media.View", new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2") },
                    { 7, "Permission", "Permissions.Media.Create", new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2") },
                    { 8, "Permission", "Permissions.Media.Delete", new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2") },
                    { 9, "Permission", "Permissions.Settings.View", new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2") },
                    { 10, "Permission", "Permissions.Settings.Edit", new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2") },
                    { 11, "Permission", "Permissions.Content.View", new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2") },
                    { 12, "Permission", "Permissions.Content.Create", new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2") },
                    { 13, "Permission", "Permissions.Content.Edit", new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2") },
                    { 14, "Permission", "Permissions.Content.Delete", new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2") }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId", "AssignedAt", "AssignedBy" },
                values: new object[,]
                {
                    { new Guid("a1b1c1d1-1111-4444-8888-a1b1c1d1e1f1"), new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1"), new DateTime(2025, 12, 1, 19, 47, 28, 215, DateTimeKind.Utc).AddTicks(2072), null },
                    { new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2"), new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1"), new DateTime(2025, 12, 1, 19, 47, 28, 215, DateTimeKind.Utc).AddTicks(2074), null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_SessionId",
                table: "UserSessions",
                column: "SessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_UserId_IsActive",
                table: "UserSessions",
                columns: new[] { "UserId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginHistories_LoginDate",
                table: "UserLoginHistories",
                column: "LoginDate");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId_IsRevoked",
                table: "RefreshTokens",
                columns: new[] { "UserId", "IsRevoked" });

            migrationBuilder.CreateIndex(
                name: "IX_FormSubmissionData_FieldName",
                table: "FormSubmissionData",
                column: "FieldName");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_ApiSlug",
                table: "Forms",
                column: "ApiSlug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContentFields_ContentTypeId",
                table: "ContentFields",
                column: "ContentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentItems_ContentTypes_ContentTypeId",
                table: "ContentItems",
                column: "ContentTypeId",
                principalTable: "ContentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
