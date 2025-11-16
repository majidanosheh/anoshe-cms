using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnosheCms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addformbuilder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Forms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApiSlug = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Forms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormFields",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FieldType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    Settings = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_FormFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormFields_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormSubmissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmissionData = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_FormSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormSubmissions_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a1b1c1d1-1111-4444-8888-a1b1c1d1e1f1"),
                column: "CreatedDate",
                value: new DateTime(2025, 11, 15, 11, 55, 14, 657, DateTimeKind.Utc).AddTicks(889));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2"),
                column: "CreatedDate",
                value: new DateTime(2025, 11, 15, 11, 55, 14, 657, DateTimeKind.Utc).AddTicks(891));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a3b3c3d3-3333-4444-8888-a3b3c3d3e3f3"),
                column: "CreatedDate",
                value: new DateTime(2025, 11, 15, 11, 55, 14, 657, DateTimeKind.Utc).AddTicks(893));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a1b1c1d1-1111-4444-8888-a1b1c1d1e1f1"), new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1") },
                column: "AssignedAt",
                value: new DateTime(2025, 11, 15, 11, 55, 14, 723, DateTimeKind.Utc).AddTicks(4252));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2"), new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1") },
                column: "AssignedAt",
                value: new DateTime(2025, 11, 15, 11, 55, 14, 723, DateTimeKind.Utc).AddTicks(4254));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7c7bcbe9-43a3-4a1d-9a80-7e106eb089f1", new DateTime(2025, 11, 15, 11, 55, 14, 657, DateTimeKind.Utc).AddTicks(1120), "AQAAAAIAAYagAAAAED2CHS9+EL7WDXS2Rk1e9ggmgHow4Ng2OeD1CAPmGlxVn0WgXNeYt3AKGnhQ1/nCkQ==", "cbf44a5f-1b4e-47a3-b55d-db1b67ed67d1" });

            migrationBuilder.CreateIndex(
                name: "IX_FormFields_FormId",
                table: "FormFields",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_ApiSlug",
                table: "Forms",
                column: "ApiSlug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormSubmissions_FormId",
                table: "FormSubmissions",
                column: "FormId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormFields");

            migrationBuilder.DropTable(
                name: "FormSubmissions");

            migrationBuilder.DropTable(
                name: "Forms");

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
    }
}
