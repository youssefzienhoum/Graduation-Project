using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Localization.Persistence.Context.Migrations
{
    /// <inheritdoc />
    public partial class FixTranslationSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMachineTranslated",
                table: "TranslationValues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReviewed",
                table: "TranslationValues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ContentJson",
                table: "TranslationBundles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TranslationStatus",
                table: "Languages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMachineTranslated",
                table: "TranslationValues");

            migrationBuilder.DropColumn(
                name: "IsReviewed",
                table: "TranslationValues");

            migrationBuilder.DropColumn(
                name: "ContentJson",
                table: "TranslationBundles");

            migrationBuilder.DropColumn(
                name: "TranslationStatus",
                table: "Languages");
        }
    }
}
