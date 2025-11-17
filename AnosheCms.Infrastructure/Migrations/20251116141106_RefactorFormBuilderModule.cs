using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnosheCms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorFormBuilderModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "UserTokens");

            migrationBuilder.RenameColumn(
                name: "SubmissionData",
                table: "FormSubmissions",
                newName: "UserAgent");

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "FormSubmissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Settings",
                table: "Forms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Settings",
                table: "FormFields",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ConditionalLogic",
                table: "FormFields",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "FormFields",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ValidationRules",
                table: "FormFields",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FormSubmissionData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FieldValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormSubmissionData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormSubmissionData_FormSubmissions_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "FormSubmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a1b1c1d1-1111-4444-8888-a1b1c1d1e1f1"),
                column: "CreatedDate",
                value: new DateTime(2025, 11, 16, 14, 11, 5, 805, DateTimeKind.Utc).AddTicks(5741));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2"),
                column: "CreatedDate",
                value: new DateTime(2025, 11, 16, 14, 11, 5, 805, DateTimeKind.Utc).AddTicks(5744));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a3b3c3d3-3333-4444-8888-a3b3c3d3e3f3"),
                column: "CreatedDate",
                value: new DateTime(2025, 11, 16, 14, 11, 5, 805, DateTimeKind.Utc).AddTicks(5746));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a1b1c1d1-1111-4444-8888-a1b1c1d1e1f1"), new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1") },
                column: "AssignedAt",
                value: new DateTime(2025, 11, 16, 14, 11, 5, 872, DateTimeKind.Utc).AddTicks(6755));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a2b2c2d2-2222-4444-8888-a2b2c2d2e2f2"), new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1") },
                column: "AssignedAt",
                value: new DateTime(2025, 11, 16, 14, 11, 5, 872, DateTimeKind.Utc).AddTicks(6757));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d1a1b1c1-1111-4444-8888-d1a1b1c1e1f1"),
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3d69374e-3a68-476c-b542-1ef722afcf4d", new DateTime(2025, 11, 16, 14, 11, 5, 805, DateTimeKind.Utc).AddTicks(5964), "AQAAAAIAAYagAAAAEPxCrPOZP6eIzdshUxSVX7yUH+qelUtoY4xwF45syfgVRvsQFgdGwEgukUAGAtJhXA==", "93b2ab8e-2495-4d3d-879d-9bd33902e872" });

            migrationBuilder.CreateIndex(
                name: "IX_FormSubmissionData_FieldName",
                table: "FormSubmissionData",
                column: "FieldName");

            migrationBuilder.CreateIndex(
                name: "IX_FormSubmissionData_SubmissionId",
                table: "FormSubmissionData",
                column: "SubmissionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormSubmissionData");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "FormSubmissions");

            migrationBuilder.DropColumn(
                name: "Settings",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "ConditionalLogic",
                table: "FormFields");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "FormFields");

            migrationBuilder.DropColumn(
                name: "ValidationRules",
                table: "FormFields");

            migrationBuilder.RenameColumn(
                name: "UserAgent",
                table: "FormSubmissions",
                newName: "SubmissionData");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "UserTokens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Settings",
                table: "FormFields",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
        }
    }
}
