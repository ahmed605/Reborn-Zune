using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicLibraryEFCoreModel.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artist",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artist", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Playlst",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlst", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Thumbnail",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Image = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thumbnail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Album",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Title = table.Column<string>(nullable: false),
                    ArtistId = table.Column<string>(maxLength: 450, nullable: false),
                    ThumbnailId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Album", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Album_Artist_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Album_Thumbnail_ThumbnailId",
                        column: x => x.ThumbnailId,
                        principalTable: "Thumbnail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Music",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 450, nullable: false),
                    Path = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    ThumbnailId = table.Column<string>(nullable: true),
                    AlbumId = table.Column<string>(nullable: true),
                    ArtistId = table.Column<string>(nullable: true),
                    PlaylistId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Music", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Music_Album_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Album",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Music_Artist_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Music_Playlst_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlst",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Music_Thumbnail_ThumbnailId",
                        column: x => x.ThumbnailId,
                        principalTable: "Thumbnail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MusicInPlaylist",
                columns: table => new
                {
                    MusicId = table.Column<string>(maxLength: 450, nullable: false),
                    PlaylistId = table.Column<string>(maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicInPlaylist", x => new { x.MusicId, x.PlaylistId });
                    table.ForeignKey(
                        name: "FK_MusicInPlaylist_Music_MusicId",
                        column: x => x.MusicId,
                        principalTable: "Music",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MusicInPlaylist_Playlst_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlst",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Album_ArtistId",
                table: "Album",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Album_ThumbnailId",
                table: "Album",
                column: "ThumbnailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Music_AlbumId",
                table: "Music",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_Music_ArtistId",
                table: "Music",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "UQ__Music__AUQKR5RFNOUO6UFR",
                table: "Music",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Music_PlaylistId",
                table: "Music",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_Music_ThumbnailId",
                table: "Music",
                column: "ThumbnailId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicInPlaylist_PlaylistId",
                table: "MusicInPlaylist",
                column: "PlaylistId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MusicInPlaylist");

            migrationBuilder.DropTable(
                name: "Music");

            migrationBuilder.DropTable(
                name: "Album");

            migrationBuilder.DropTable(
                name: "Playlst");

            migrationBuilder.DropTable(
                name: "Artist");

            migrationBuilder.DropTable(
                name: "Thumbnail");
        }
    }
}
