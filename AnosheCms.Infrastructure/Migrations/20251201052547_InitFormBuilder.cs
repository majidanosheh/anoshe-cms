using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnosheCms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitFormBuilder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a1b1c1d1-1111-4444-8888-a1b1c1d1e1f1"),
                column: "CreatedDate",
                value: new DateTime(2025, 12, 1, 5, 5, 47, 20, DateTimeKind.Utc).AddTicks(8045));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2"),
                column: "CreatedDate",
                value: new DateTime(2025, 12, 1, 5, 5, 47, 20, DateTimeKind.Utc).AddTicks(8047));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a3b3c3d3-3333-4444-8888-a3b3c3d3e3f3"),
                column: "CreatedDate",
                value: new DateTime(2025, 12, 1, 5, 5, 47, 20, DateTimeKind.Utc).AddTicks(8049));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a1b1c1d1-1111-4444-8888-a1b1c1d1e1f1"), new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1") },
                column: "AssignedAt",
                value: new DateTime(2025, 12, 1, 5, 5, 47, 86, DateTimeKind.Utc).AddTicks(2121));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2"), new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1") },
                column: "AssignedAt",
                value: new DateTime(2025, 12, 1, 5, 5, 47, 86, DateTimeKind.Utc).AddTicks(2122));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d5be2286-b28d-41d7-8afe-6187dff219b6", new DateTime(2025, 12, 1, 5, 5, 47, 20, DateTimeKind.Utc).AddTicks(8309), "AQAAAAIAAYagAAAAEBXewp4upzJZRA5775LDMoMGv1KFCntYKYAObesQo6NvKphvnv3rLKALzDsGKnkcWg==", "c2174e4a-8ed0-4414-b742-0dfce562186b" });
        }
    }
}
