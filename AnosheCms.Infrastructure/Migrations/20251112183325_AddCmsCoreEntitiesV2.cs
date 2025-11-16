using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnosheCms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCmsCoreEntitiesV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a1b1c1d1-1111-4444-8888-a1b1c1d1e1f1"),
                column: "CreatedDate",
                value: new DateTime(2025, 11, 12, 18, 17, 57, 913, DateTimeKind.Utc).AddTicks(8573));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2"),
                column: "CreatedDate",
                value: new DateTime(2025, 11, 12, 18, 17, 57, 913, DateTimeKind.Utc).AddTicks(8576));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a3b3c3d3-3333-4444-8888-a3b3c3d3e3f3"),
                column: "CreatedDate",
                value: new DateTime(2025, 11, 12, 18, 17, 57, 913, DateTimeKind.Utc).AddTicks(8579));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a1b1c1d1-1111-4444-8888-a1b1c1d1e1f1"), new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1") },
                column: "AssignedAt",
                value: new DateTime(2025, 11, 12, 18, 17, 58, 17, DateTimeKind.Utc).AddTicks(9037));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2"), new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1") },
                column: "AssignedAt",
                value: new DateTime(2025, 11, 12, 18, 17, 58, 17, DateTimeKind.Utc).AddTicks(9041));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3f1f271e-e45b-4656-b082-c57519b3ce1e", new DateTime(2025, 11, 12, 18, 17, 57, 913, DateTimeKind.Utc).AddTicks(8982), "AQAAAAIAAYagAAAAEAvRRQJ3HNPfz+nkSN7lhRArKO65jjuK9Qvf3OxLEOpZrj+/tRL76sfDRhoBQENO0A==", "cf97a4e9-bdd8-4e0a-a032-083eb520220d" });
        }
    }
}
