using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProxyProject_Backend.Migrations
{
    /// <inheritdoc />
    public partial class addBankTypeintransactionhistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankType",
                table: "TransactionHistories",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                columns: new[] { "APIKey", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletKey" },
                values: new object[] { "cAfzfPBBjaQhvirXrDf8BVEur6cekax3", "11028b48-d9e1-437c-8989-a9cc768b4914", "AQAAAAIAAYagAAAAEJxyE1mAD7tgrLZX+GE9XlEeI/fwOhHFxm44ybl4eWDBhX83QqrO2h9ktCVDcuZx8w==", "fe2b94dd-2434-447f-9fab-66839a363e1e", "3iOtvG7eJ221pBMVw1UydY9tyvOdh_G5" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankType",
                table: "TransactionHistories");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                columns: new[] { "APIKey", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletKey" },
                values: new object[] { "hyNErwWeuV9crUFo5VRGpRnpglNhEcmi", "ba8eae7a-8d2e-4833-88dc-27506e8661b1", "AQAAAAIAAYagAAAAEMzo9JtxuW1H0Kb2Kv9eja0RDDH+slbAZcgNqZNH4m+APsdwHFSsXlbpjIkbJT4kVw==", "7cde9ad6-2230-484a-9f95-079cbec32abb", "lnQms4w6YPt3gnxuKr1WMA_Y1BhaD2ew" });
        }
    }
}
