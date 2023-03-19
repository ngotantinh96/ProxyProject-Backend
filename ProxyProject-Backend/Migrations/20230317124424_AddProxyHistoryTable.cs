using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProxyProject_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddProxyHistoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProxyHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProxyId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UsedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProxyHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProxyHistory_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                columns: new[] { "APIKey", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletKey" },
                values: new object[] { "0Ic4fH15lEOjmcaFZHumOE8V6kqf0xyY", "47ecdca0-eef2-420b-aac3-73650f97fd61", "AQAAAAIAAYagAAAAEFyG15gj6//ph56GDkZPRC7DUfdSyeTkllvrYwOOsGChiydk3HuscZQKC5yE/4JD+Q==", "c9dc1341-9758-4a47-81bf-e4b6b28bee69", "cQ7157TQyG1Htd0AA7jJ4kbFcq_lwCN5" });

            migrationBuilder.CreateIndex(
                name: "IX_ProxyHistory_UserId",
                table: "ProxyHistory",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProxyHistory");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                columns: new[] { "APIKey", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletKey" },
                values: new object[] { "1VlYYNoHZPSy9jRhA_jBN5ny7dtxt5Ip", "d79ff99b-b8ea-41e8-8136-89cf206f6b2a", "AQAAAAIAAYagAAAAEHsnL1j7r44n1FF4Eme3pGiK18tySyK7FgY4zulQUc6IOD/mvSVtdpnMcX2hlfck7g==", "23045105-c8e7-4aa5-b53c-3bf2009d49ba", "6CZG9-YIeQPKloG8gAu79bqPuz7pPuGi" });
        }
    }
}
