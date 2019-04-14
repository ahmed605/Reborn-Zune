using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicLibraryEFCoreModel.Migrations
{
    public partial class SecondCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlaylistId",
                table: "Music",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailId",
                table: "Album",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Music_PlaylistId",
                table: "Music",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_Album_ThumbnailId",
                table: "Album",
                column: "ThumbnailId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Album_Thumbnail_ThumbnailId",
                table: "Album",
                column: "ThumbnailId",
                principalTable: "Thumbnail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Music_Playlst_PlaylistId",
                table: "Music",
                column: "PlaylistId",
                principalTable: "Playlst",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Album_Thumbnail_ThumbnailId",
                table: "Album");

            migrationBuilder.DropForeignKey(
                name: "FK_Music_Playlst_PlaylistId",
                table: "Music");

            migrationBuilder.DropIndex(
                name: "IX_Music_PlaylistId",
                table: "Music");

            migrationBuilder.DropIndex(
                name: "IX_Album_ThumbnailId",
                table: "Album");

            migrationBuilder.DropColumn(
                name: "PlaylistId",
                table: "Music");

            migrationBuilder.DropColumn(
                name: "ThumbnailId",
                table: "Album");
        }
    }
}
