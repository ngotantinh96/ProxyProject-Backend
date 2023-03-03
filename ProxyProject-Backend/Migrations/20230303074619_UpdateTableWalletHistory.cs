using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProxyProject_Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableWalletHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletHistory_AspNetUsers_WalletKey",
                table: "WalletHistory");

            migrationBuilder.DropIndex(
                name: "IX_WalletHistory_WalletKey",
                table: "WalletHistory");

            migrationBuilder.AlterColumn<string>(
                name: "WalletKey",
                table: "WalletHistory",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "WalletHistory",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_WalletHistory_UserId",
                table: "WalletHistory",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletHistory_AspNetUsers_UserId",
                table: "WalletHistory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletHistory_AspNetUsers_UserId",
                table: "WalletHistory");

            migrationBuilder.DropIndex(
                name: "IX_WalletHistory_UserId",
                table: "WalletHistory");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WalletHistory");

            migrationBuilder.AlterColumn<string>(
                name: "WalletKey",
                table: "WalletHistory",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_WalletHistory_WalletKey",
                table: "WalletHistory",
                column: "WalletKey");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletHistory_AspNetUsers_WalletKey",
                table: "WalletHistory",
                column: "WalletKey",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
