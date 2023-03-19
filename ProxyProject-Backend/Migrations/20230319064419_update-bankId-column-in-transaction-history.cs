using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProxyProject_Backend.Migrations
{
    /// <inheritdoc />
    public partial class updatebankIdcolumnintransactionhistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BankId",
                table: "TransactionHistories",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "TransactionHistories",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                columns: new[] { "APIKey", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletKey" },
                values: new object[] { "CMM3hVWZAKnLcIqGO5zZhbg_3XexHpJ1", "38939643-0a40-4be4-af3c-7a6ae985d2b3", "AQAAAAIAAYagAAAAEArR8t+H96fj8vIb24YtDDAu00S6I2xIeZ//d9BCp+QN+vVbIgG/JcrwfETut0co3g==", "3c71eb65-31a6-417f-8766-1bb557fc5159", "tF6jsZ5bYgY-siBJXuS2JbfKHsHtR0jM" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankId",
                table: "TransactionHistories");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "TransactionHistories");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                columns: new[] { "APIKey", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletKey" },
                values: new object[] { "g9S-5C9GDWSMEOPQs6NtVmBlLp8GFNcH", "603eac07-87af-4829-9b85-e0dc283c0b47", "AQAAAAIAAYagAAAAEFEyNjOWatwQbwI6q8uQ6lL1C733Y49Q5EI+XB21aYacYRIDZfxVuLUCQyfNVz5PeA==", "8e1ecec4-6db9-4ebd-ba02-1fc1031b8bdd", "C9JG3YBz5BPzOHiDe9D61yeYW3qS9A4J" });
        }
    }
}
