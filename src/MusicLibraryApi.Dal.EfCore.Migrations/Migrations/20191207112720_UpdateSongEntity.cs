using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicLibraryApi.Dal.EfCore.Migrations.Migrations
{
    public partial class UpdateSongEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Checksum",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "Songs");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Duration",
                table: "Songs",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "interval",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeleteComment",
                table: "Songs",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeleteDate",
                table: "Songs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TreeTitle",
                table: "Songs",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteComment",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "DeleteDate",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "TreeTitle",
                table: "Songs");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Duration",
                table: "Songs",
                type: "interval",
                nullable: true,
                oldClrType: typeof(TimeSpan));

            migrationBuilder.AddColumn<int>(
                name: "Checksum",
                table: "Songs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FileSize",
                table: "Songs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
