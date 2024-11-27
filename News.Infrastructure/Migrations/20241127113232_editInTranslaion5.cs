using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace News.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editInTranslaion5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "newsTranslations");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "newsTranslations",
                newName: "Language2Title");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "newsTranslations",
                newName: "Language2Content");

            migrationBuilder.AddColumn<string>(
                name: "Language1Content",
                table: "newsTranslations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Language1Title",
                table: "newsTranslations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language1Content",
                table: "newsTranslations");

            migrationBuilder.DropColumn(
                name: "Language1Title",
                table: "newsTranslations");

            migrationBuilder.RenameColumn(
                name: "Language2Title",
                table: "newsTranslations",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Language2Content",
                table: "newsTranslations",
                newName: "Content");

            migrationBuilder.AddColumn<int>(
                name: "Language",
                table: "newsTranslations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
