// AnosheCms/Infrastructure/Migrations/20251116150000_Corrective_AlignEntitiesToSpecs.cs
// NEW FILE

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnosheCms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Corrective_AlignEntitiesToSpecs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // --- اصلاح جدول Forms ---

            // (فیلدهایی که در پکیج قبلی به اشتباه حذف شدند یا وجود نداشتند)
            migrationBuilder.AddColumn<string>(
                name: "Settings", // (بازگرداندن فیلد Settings از 26.txt)
                table: "Forms",
                type: "nvarchar(max)",
                nullable: true);

            // (فیلدهای جدید از 'فرم ساز فرانت اند.docx')
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

            // --- اصلاح جدول FormFields ---

            // (فیلدهای جدید از 'فرم ساز فرانت اند.docx')
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

            // (فیلد Settings در FormField در مایگریشن قبلی اضافه شده بود، نیازی به افزودن مجدد نیست)
            // (ValidationRules و ConditionalLogic نیز در مایگریشن RefactorFormBuilderModule اضافه شدند)
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Settings",
                table: "Forms");

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
        }
    }
}