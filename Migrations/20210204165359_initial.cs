using Microsoft.EntityFrameworkCore.Migrations;

namespace Player.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SongPlaylist",
                table: "SongPlaylist");

            migrationBuilder.DropIndex(
                name: "IX_SongPlaylist_SongId",
                table: "SongPlaylist");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SongPlaylist",
                table: "SongPlaylist",
                columns: new[] { "SongId", "PlaylistId" });

            migrationBuilder.CreateIndex(
                name: "IX_SongPlaylist_PlaylistId",
                table: "SongPlaylist",
                column: "PlaylistId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SongPlaylist",
                table: "SongPlaylist");

            migrationBuilder.DropIndex(
                name: "IX_SongPlaylist_PlaylistId",
                table: "SongPlaylist");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SongPlaylist",
                table: "SongPlaylist",
                columns: new[] { "PlaylistId", "SongId" });

            migrationBuilder.CreateIndex(
                name: "IX_SongPlaylist_SongId",
                table: "SongPlaylist",
                column: "SongId");
        }
    }
}
