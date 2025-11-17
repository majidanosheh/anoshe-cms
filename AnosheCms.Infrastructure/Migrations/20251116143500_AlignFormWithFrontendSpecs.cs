// AnosheCms/Infrastructure/Migrations/20251116143500_AlignFormWithFrontendSpecs.cs
// NEW FILE

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnosheCms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlignFormWithFrontendSpecs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. حذف ستون JSON قدیمی Settings از جدول Forms
            migrationBuilder.DropColumn(
                name: "Settings",
                table: "Forms");

            // 2. افزودن فیلدهای جدید به جدول Forms (بر اساس سند فرانت‌اند)
            migrationBuilder.AddColumn<string>(
                name: "ConfirmationMessage",
                table: "Forms",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotificationEmailRecipient",
                table: "Forms",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RedirectUrl",
                table: "Forms",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SendEmailNotification",
                table: "Forms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SubmitButtonText",
                table: "Forms",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "Submit");

            // 3. افزودن فیلدهای جدید به جدول FormFields (بر اساس سند فرانت‌اند)
            migrationBuilder.AddColumn<string>(
                name: "HelpText",
                table: "FormFields",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Placeholder",
                table: "FormFields",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmationMessage",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "NotificationEmailRecipient",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "RedirectUrl",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "SendEmailNotification",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "SubmitButtonText",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "HelpText",
                table: "FormFields");

            migrationBuilder.DropColumn(
                name: "Placeholder",
                table: "FormFields");

            migrationBuilder.AddColumn<string>(
                name: "Settings",
                table: "Forms",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}