using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnosheCms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixLoginSchema1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ContentTypes_ApiSlug",
                table: "ContentTypes");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTypes_ApiSlug",
                table: "ContentTypes",
                column: "ApiSlug",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ContentTypes_ApiSlug",
                table: "ContentTypes");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTypes_ApiSlug",
                table: "ContentTypes",
                column: "ApiSlug",
                unique: true);
        }
    }
}
