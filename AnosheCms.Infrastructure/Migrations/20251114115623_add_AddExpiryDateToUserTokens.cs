using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnosheCms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_AddExpiryDateToUserTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "UserTokens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a1b1c1d1-1111-4444-8888-a1b1c1d1e1f1"),
                column: "CreatedDate",
                value: new DateTime(2025, 11, 14, 11, 56, 23, 205, DateTimeKind.Utc).AddTicks(8927));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2"),
                column: "CreatedDate",
                value: new DateTime(2025, 11, 14, 11, 56, 23, 205, DateTimeKind.Utc).AddTicks(8929));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a3b3c3d3-3333-4444-8888-a3b3c3d3e3f3"),
                column: "CreatedDate",
                value: new DateTime(2025, 11, 14, 11, 56, 23, 205, DateTimeKind.Utc).AddTicks(8930));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a1b1c1d1-1111-4444-8888-a1b1c1d1e1f1"), new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1") },
                column: "AssignedAt",
                value: new DateTime(2025, 11, 14, 11, 56, 23, 271, DateTimeKind.Utc).AddTicks(6235));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2"), new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1") },
                column: "AssignedAt",
                value: new DateTime(2025, 11, 14, 11, 56, 23, 271, DateTimeKind.Utc).AddTicks(6237));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2883b647-7200-418e-88f8-fab5e469472a", new DateTime(2025, 11, 14, 11, 56, 23, 205, DateTimeKind.Utc).AddTicks(9151), "AQAAAAIAAYagAAAAEJ0YIz8tufrKJP/HX/4eD+xeoyLayggeyjX8q5O8lE4Ua0JS4uPBA6eY/hethuZ5pw==", "e7786244-1337-4e4a-aaa1-f6d3be8f4cf3" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "UserTokens");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a1b1c1d1-1111-4444-8888-a1b1c1d1e1f1"),
                column: "CreatedDate",
                value: new DateTime(2025, 11, 12, 18, 33, 24, 814, DateTimeKind.Utc).AddTicks(8274));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2"),
                column: "CreatedDate",
                value: new DateTime(2025, 11, 12, 18, 33, 24, 814, DateTimeKind.Utc).AddTicks(8276));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a3b3c3d3-3333-4444-8888-a3b3c3d3e3f3"),
                column: "CreatedDate",
                value: new DateTime(2025, 11, 12, 18, 33, 24, 814, DateTimeKind.Utc).AddTicks(8278));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a1b1c1d1-1111-4444-8888-a1b1c1d1e1f1"), new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1") },
                column: "AssignedAt",
                value: new DateTime(2025, 11, 12, 18, 33, 24, 886, DateTimeKind.Utc).AddTicks(2235));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2"), new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1") },
                column: "AssignedAt",
                value: new DateTime(2025, 11, 12, 18, 33, 24, 886, DateTimeKind.Utc).AddTicks(2237));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e2f0a2d3-f8e3-4e15-aa50-cb47949fe7c0", new DateTime(2025, 11, 12, 18, 33, 24, 814, DateTimeKind.Utc).AddTicks(8541), "AQAAAAIAAYagAAAAEKQQLEJOvBrQRkMEQjHWNqPZMLQwkqqN0M/H4VikDx/dlSQA6+r0bzZLfXknFLF5uw==", "e1bee927-23bf-4cd8-a7eb-2ba8b5930f1a" });
        }
    }
}
