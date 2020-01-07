using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicLibraryApi.Dal.EfCore.Migrations.Migrations
{
    public partial class AddSongContentColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Checksum",
                table: "Songs",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "Songs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Checksum",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Songs");
        }
    }
}
