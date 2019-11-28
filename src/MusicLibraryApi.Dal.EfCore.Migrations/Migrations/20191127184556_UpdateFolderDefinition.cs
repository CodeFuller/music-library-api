using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicLibraryApi.Dal.EfCore.Migrations.Migrations
{
    public partial class UpdateFolderDefinition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Folders_ParentFolderId",
                table: "Folders");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ParentFolderId_Name",
                table: "Folders",
                columns: new[] { "ParentFolderId", "Name" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Folders_ParentFolderId_Name",
                table: "Folders");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ParentFolderId",
                table: "Folders",
                column: "ParentFolderId");
        }
    }
}
