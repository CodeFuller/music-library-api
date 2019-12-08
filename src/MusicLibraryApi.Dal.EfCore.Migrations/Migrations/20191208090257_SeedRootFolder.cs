using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicLibraryApi.Dal.EfCore.Migrations.Migrations
{
    public partial class SeedRootFolder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FolderId",
                table: "Discs",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Folders",
                columns: new[] { "Id", "Name", "ParentFolderId" },
                values: new object[] { 1, "<ROOT>", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "FolderId",
                table: "Discs",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
