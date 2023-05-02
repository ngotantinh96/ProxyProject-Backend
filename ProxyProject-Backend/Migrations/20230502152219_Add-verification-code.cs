using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProxyProject_Backend.Migrations
{
    /// <inheritdoc />
    public partial class Addverificationcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VerificationCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpiredDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationCodes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                columns: new[] { "APIKey", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletKey" },
                values: new object[] { "Goet5S-m3j5uSvl4ySnQWYbq1xnX6ISK", "0df0f9c6-605f-485f-8f12-bdbc3e651260", "AQAAAAIAAYagAAAAEOqrUbDSDbY7HKQgOcUt3oMG0Edyj6BXwbxEAYvNB6VADu44gH6LC5EkJd2QtwT0pg==", "2bb75c19-6d87-4cc3-a21e-3b65f95a4615", "5DfPr3jeXqoPfwJMW4d9bUbsITRlT6tF" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VerificationCodes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                columns: new[] { "APIKey", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletKey" },
                values: new object[] { "Gj6dInLJQLOGOhrFp53ygb-4NxdQhGhm", "41850caf-b51b-4081-a56a-17400517932d", "AQAAAAIAAYagAAAAEGLQTh9YjGUtB28ayT3FVZMyxULk71D4qjggD6GgdO4KV6ffEt37jp+hJx1vgjIjPA==", "42d5ef60-09ad-49a1-8bf8-dc2b10a9801f", "rhFwHflDC2UJay0td473WUCGN-3sDaB8" });
        }
    }
}
