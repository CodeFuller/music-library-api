using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicLibraryApi.Dal.EfCore.Migrations.Migrations
{
    public partial class RestrictCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Playbacks_Songs_SongId",
                table: "Playbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Discs_DiscId",
                table: "Songs");

            migrationBuilder.AddForeignKey(
                name: "FK_Playbacks_Songs_SongId",
                table: "Playbacks",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Discs_DiscId",
                table: "Songs",
                column: "DiscId",
                principalTable: "Discs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Playbacks_Songs_SongId",
                table: "Playbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Discs_DiscId",
                table: "Songs");

            migrationBuilder.AddForeignKey(
                name: "FK_Playbacks_Songs_SongId",
                table: "Playbacks",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Discs_DiscId",
                table: "Songs",
                column: "DiscId",
                principalTable: "Discs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
