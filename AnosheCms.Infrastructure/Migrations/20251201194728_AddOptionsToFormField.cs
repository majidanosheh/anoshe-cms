using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnosheCms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOptionsToFormField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Options",
                table: "FormFields",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a1b1c1d1-1111-4444-8888-a1b1c1d1e1f1"),
                column: "CreatedDate",
                value: new DateTime(2025, 12, 1, 19, 47, 28, 149, DateTimeKind.Utc).AddTicks(372));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2"),
                column: "CreatedDate",
                value: new DateTime(2025, 12, 1, 19, 47, 28, 149, DateTimeKind.Utc).AddTicks(375));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a3b3c3d3-3333-4444-8888-a3b3c3d3e3f3"),
                column: "CreatedDate",
                value: new DateTime(2025, 12, 1, 19, 47, 28, 149, DateTimeKind.Utc).AddTicks(376));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a1b1c1d1-1111-4444-8888-a1b1c1d1e1f1"), new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1") },
                column: "AssignedAt",
                value: new DateTime(2025, 12, 1, 19, 47, 28, 215, DateTimeKind.Utc).AddTicks(2072));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2"), new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1") },
                column: "AssignedAt",
                value: new DateTime(2025, 12, 1, 19, 47, 28, 215, DateTimeKind.Utc).AddTicks(2074));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "20f6914c-f3e8-4bc5-b98d-95b02aa1821a", new DateTime(2025, 12, 1, 19, 47, 28, 149, DateTimeKind.Utc).AddTicks(646), "AQAAAAIAAYagAAAAEGTOMxHmQ3ACL0nb6eVECwcgcU2r0aM940sYKpqMhW4X8m6l1NJTGALwAVj7Lk3inA==", "19bceaae-3a1c-491e-8791-db2b13a09981" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Options",
                table: "FormFields");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a1b1c1d1-1111-4444-8888-a1b1c1d1e1f1"),
                column: "CreatedDate",
                value: new DateTime(2025, 12, 1, 5, 25, 46, 931, DateTimeKind.Utc).AddTicks(3248));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2"),
                column: "CreatedDate",
                value: new DateTime(2025, 12, 1, 5, 25, 46, 931, DateTimeKind.Utc).AddTicks(3251));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a3b3c3d3-3333-4444-8888-a3b3c3d3e3f3"),
                column: "CreatedDate",
                value: new DateTime(2025, 12, 1, 5, 25, 46, 931, DateTimeKind.Utc).AddTicks(3252));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a1b1c1d1-1111-4444-8888-a1b1c1d1e1f1"), new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1") },
                column: "AssignedAt",
                value: new DateTime(2025, 12, 1, 5, 25, 46, 995, DateTimeKind.Utc).AddTicks(6575));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2"), new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1") },
                column: "AssignedAt",
                value: new DateTime(2025, 12, 1, 5, 25, 46, 995, DateTimeKind.Utc).AddTicks(6576));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8f9bc97a-a91d-48c8-ad39-72670884be38", new DateTime(2025, 12, 1, 5, 25, 46, 931, DateTimeKind.Utc).AddTicks(3455), "AQAAAAIAAYagAAAAEPS7RLjbXJzKrX5Ze9IHNKbtPhkURoeiFdunkW5tm/vrCeZ7fLQrCcr6CeyaksWseg==", "07b8d2fe-f5c4-44ae-810c-686c5c08e0a2" });
        }
    }
}
