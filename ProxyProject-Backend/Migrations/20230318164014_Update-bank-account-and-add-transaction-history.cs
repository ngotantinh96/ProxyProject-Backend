using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProxyProject_Backend.Migrations
{
    /// <inheritdoc />
    public partial class Updatebankaccountandaddtransactionhistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApiLink",
                table: "BankAccounts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "BankAccounts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "BankAccounts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TransactionHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BankAccount = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Comment = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TransactionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionHistories_AspNetUsers_UserId",
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
                values: new object[] { "g9S-5C9GDWSMEOPQs6NtVmBlLp8GFNcH", "603eac07-87af-4829-9b85-e0dc283c0b47", "AQAAAAIAAYagAAAAEFEyNjOWatwQbwI6q8uQ6lL1C733Y49Q5EI+XB21aYacYRIDZfxVuLUCQyfNVz5PeA==", "8e1ecec4-6db9-4ebd-ba02-1fc1031b8bdd", "C9JG3YBz5BPzOHiDe9D61yeYW3qS9A4J" });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistories_UserId",
                table: "TransactionHistories",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionHistories");

            migrationBuilder.DropColumn(
                name: "ApiLink",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "BankAccounts");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                columns: new[] { "APIKey", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletKey" },
                values: new object[] { "bxy296yegSBn9U0Ih-hCjRf6b4rGsG8x", "cb0004f7-2701-417c-a8ba-7c448f137bdd", "AQAAAAIAAYagAAAAEIrw9+yLJgAVImi47QWgj7OikbHuY7OBFRSPxOn3ENtdPbWFoUfAzIzKHUZc2wK6+g==", "359326af-6f57-43fb-bbee-21fa88f1b3e4", "eWL0sCsJAVK6CZYCz5JoxU9mxllNuegl" });
        }
    }
}
