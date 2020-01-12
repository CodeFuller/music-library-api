using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicLibraryApi.Dal.EfCore.Migrations.Migrations
{
    public partial class AddDiscCoverColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CoverChecksum",
                table: "Discs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverFileName",
                table: "Discs",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CoverSize",
                table: "Discs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverChecksum",
                table: "Discs");

            migrationBuilder.DropColumn(
                name: "CoverFileName",
                table: "Discs");

            migrationBuilder.DropColumn(
                name: "CoverSize",
                table: "Discs");
        }
    }
}
